CREATE PROCEDURE sp_LoginUser
    @Логин NVARCHAR(32),
    @Пароль VARCHAR(32)
AS
BEGIN
	DECLARE @Хеш VARBINARY(32) = HASHBYTES('SHA2_256',@Пароль)

    SELECT 
        s.ID_сотрудника,
        s.ФИО,
        s.Должность,
        s.Электронная_почта,
        s.Телефон
    FROM 
        УчетныеЗаписи AS u
    INNER JOIN 
        Сотрудник AS s ON u.ID_сотрудника = s.ID_сотрудника
    WHERE 
        u.Логин = @Логин 
        AND u.Хеш_пароля = @Хеш;
END


CREATE PROCEDURE sp_RegisterUser
    @ID_сотрудника INT,
    @Логин NVARCHAR(32),
    @Пароль NVARCHAR(32)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (SELECT 1 FROM Сотрудник WHERE ID_сотрудника = @ID_сотрудника)
    BEGIN
        RAISERROR('Сотрудник с таким ID не найден.', 16, 1);
        RETURN;
    END;
    
    IF EXISTS (SELECT 1 FROM УчетныеЗаписи WHERE ID_сотрудника = @ID_сотрудника OR Логин = @Логин)
    BEGIN
        RAISERROR('Учетная запись для этого сотрудника уже существует или логин занят.', 16, 1);
        RETURN;
    END;
    
    INSERT INTO УчетныеЗаписи (ID_сотрудника, Логин, Хеш_пароля)
    VALUES (@ID_сотрудника, @Логин, HASHBYTES('SHA2_256', @Пароль));
END;

CREATE PROCEDURE sp_EndSessionAndCreatePayment
    @ID_сессии INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @t_start SMALLDATETIME;
    DECLARE @t_end SMALLDATETIME;
    DECLARE @duration_hours DECIMAL(10, 2);
    DECLARE @ID_тарифа INT;
    DECLARE @Сумма DECIMAL(10, 2);
    
    -- 1. Проверяем сессию и завершаем ее (устанавливаем Время_выезда)
    IF NOT EXISTS (SELECT 1 FROM Парковочная_сессия WHERE ID_сессии = @ID_сессии AND Время_выезда IS NULL)
    BEGIN
        RAISERROR('Активная сессия с таким ID не найдена.', 16, 1);
        RETURN;
    END;
    
    UPDATE Парковочная_сессия
    SET Время_выезда = GETDATE()
    WHERE ID_сессии = @ID_сессии;
    
    -- 2. Получаем время для расчета
    SELECT 
        @t_start = Время_заезда, 
        @t_end = Время_выезда
    FROM Парковочная_сессия
    WHERE ID_сессии = @ID_сессии;
    
    -- 3. Рассчитываем длительность в часах (с округлением вверх, как в Приложении Б)
    SET @duration_hours = CEILING(DATEDIFF(minute, @t_start, @t_end) / 60.0);
    
    -- 4. Находим подходящий тариф
    -- (Минимальный тариф, продолжительность которого >= длительности сессии)
    SELECT TOP 1 
        @ID_тарифа = ID_тарифа,
        @Сумма = Стоимость
    FROM Тариф
    WHERE Продолжительность_часов >= @duration_hours
    ORDER BY Продолжительность_часов ASC;
    
    IF @ID_тарифа IS NULL
    BEGIN
        SELECT TOP 1 
            @ID_тарифа = ID_тарифа,
            @Сумма = Стоимость
        FROM Тариф
        ORDER BY Продолжительность_часов DESC;
    END;

    -- 5. Создаем запись о платеже    
    INSERT INTO Платёж (ID_сессии, ID_тарифа, Сумма, Дата_платежа)
    VALUES (@ID_сессии, @ID_тарифа, @Сумма, GETDATE());
    
    -- Возвращаем рассчитанную сумму
    SELECT @Сумма AS Рассчитанная_сумма, @ID_тарифа AS ID_тарифа;
    
END;

