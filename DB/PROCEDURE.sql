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


