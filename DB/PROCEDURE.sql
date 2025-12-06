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
END;
GO


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
GO


CREATE OR ALTER PROCEDURE sp_UpdateEmployeeProfile
    @ID_сотрудника INT,
    @ФИО VARCHAR(100),
    @Телефон VARCHAR(20),
    @Электронная_почта VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Сотрудник WHERE (Телефон = @Телефон OR Электронная_почта = @Электронная_почта) AND ID_сотрудника != @ID_сотрудника)
    BEGIN
        RAISERROR('Этот телефон или почта уже используются другим сотрудником.', 16, 1);
        RETURN;
    END

    UPDATE Сотрудник
    SET ФИО = @ФИО,
        Телефон = @Телефон,
        Электронная_почта = @Электронная_почта
    WHERE ID_сотрудника = @ID_сотрудника;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateEmployeeCredentials
    @ID_сотрудника INT,
    @НовыйЛогин VARCHAR(50),
    @НовыйПароль VARCHAR(50)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM УчетныеЗаписи WHERE Логин = @НовыйЛогин AND ID_сотрудника != @ID_сотрудника)
    BEGIN
        RAISERROR('Такой логин уже занят.', 16, 1);
        RETURN;
    END

    UPDATE УчетныеЗаписи
    SET Логин = @НовыйЛогин,
        Хеш_пароля = HASHBYTES('SHA2_256', @НовыйПароль)
    WHERE ID_сотрудника = @ID_сотрудника;
END;
GO

CREATE   PROCEDURE sp_AddEmployee
    @ФИО VARCHAR(100),
    @Должность VARCHAR(18),
    @Телефон VARCHAR(20),
    @Электронная_почта VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Сотрудник WHERE Телефон = @Телефон OR Электронная_почта = @Электронная_почта)
    BEGIN
        RAISERROR('Сотрудник с таким телефоном или почтой уже существует.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION; 

        INSERT INTO Сотрудник (ФИО, Должность, Телефон, Электронная_почта)
        VALUES (@ФИО, @Должность, @Телефон, @Электронная_почта);

        INSERT INTO УчетныеЗаписи (Логин, Хеш_пароля)
        VALUES (@Электронная_почта, HASHBYTES('SHA2_256', 'password123'));

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteEmployee
    @Телефон VARCHAR(20)
AS
BEGIN

    IF NOT EXISTS (SELECT 1 FROM Сотрудник WHERE Телефон = @Телефон)
    BEGIN
        RAISERROR('Сотрудник с таким номером телефона не найден.', 16, 1);
        RETURN;
    END

    DELETE FROM Сотрудник WHERE Телефон = @Телефон;
END;
GO

CREATE OR ALTER PROCEDURE sp_CreateParkingSession
    @Гос_номер VARCHAR(12),
    @ID_сотрудника INT,
    @Номер_места INT = NULL,
    @Результат NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Проверка существования автомобиля
        IF NOT EXISTS (SELECT 1 FROM ТС WHERE Гос_номер = @Гос_номер)
        BEGIN
            SET @Результат = 'Ошибка: Автомобиль с номером ' + @Гос_номер + ' не найден в базе данных';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF EXISTS (
            SELECT 1 FROM Парковочная_сессия 
            WHERE Гос_номер = @Гос_номер AND Время_выезда IS NULL
        )
        BEGIN
            SET @Результат = 'Ошибка: Автомобиль ' + @Гос_номер + ' уже находится на парковке';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM Сотрудник WHERE ID_сотрудника = @ID_сотрудника)
        BEGIN
            SET @Результат = 'Ошибка: Сотрудник с ID ' + CAST(@ID_сотрудника AS VARCHAR) + ' не найден';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @Номер_места IS NULL
        BEGIN
            SELECT @Номер_места = Номер_места 
            FROM Парковочное_место 
            WHERE Статус = 'Свободно'
            ORDER BY Номер_места;
            
            -- Проверка наличия свободных мест
            IF @Номер_места IS NULL
            BEGIN
                SET @Результат = 'Ошибка: Нет свободных парковочных мест';
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        ELSE
        BEGIN
            -- Если место указано, проверяем его существование
            IF NOT EXISTS (SELECT 1 FROM Парковочное_место WHERE Номер_места = @Номер_места)
            BEGIN
                SET @Результат = 'Ошибка: Парковочное место ' + CAST(@Номер_места AS VARCHAR) + ' не существует';
                ROLLBACK TRANSACTION;
                RETURN;
            END
            
            -- Проверка, что место свободно
            IF EXISTS (SELECT 1 FROM Парковочное_место WHERE Номер_места = @Номер_места AND Статус = 'Занято')
            BEGIN
                SET @Результат = 'Ошибка: Парковочное место ' + CAST(@Номер_места AS VARCHAR) + ' уже занято';
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        
        INSERT INTO Парковочная_сессия (Гос_номер, Номер_места, ID_сотрудника, Время_заезда, Время_выезда)
        VALUES (@Гос_номер, @Номер_места, @ID_сотрудника, GETDATE(), NULL);
        
        DECLARE @ФИО_клиента VARCHAR(100);
        DECLARE @ID_сессии INT;
        
        SET @ID_сессии = SCOPE_IDENTITY();
        
        SELECT @ФИО_клиента = к.ФИО 
        FROM ТС т 
        INNER JOIN Клиент к ON т.ID_клиента = к.ID_клиента
        WHERE т.Гос_номер = @Гос_номер;
        
        SET @Результат = 'Успех: Создана сессия #' + CAST(@ID_сессии AS VARCHAR) + 
                        ' для автомобиля ' + @Гос_номер + 
                        ' (владелец: ' + @ФИО_клиента + ')' +
                        ' на месте ' + CAST(@Номер_места AS VARCHAR);
        
        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Результат = 'Ошибка: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_CloseParkingSession
(
    @Гос_номер VARCHAR(12),
    @Результат VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM ТС WHERE Гос_номер = @Гос_номер)
    BEGIN
        SET @Результат = 'Ошибка: автомобиль с таким номером не зарегистрирован.';
        RETURN;
    END;

    DECLARE @ID_сессии INT;

    SELECT @ID_сессии = ID_сессии
    FROM Парковочная_сессия
    WHERE Гос_номер = @Гос_номер AND Время_выезда IS NULL
    ORDER BY Время_заезда DESC;

    IF @ID_сессии IS NULL
    BEGIN
        SET @Результат = 'Ошибка: активная парковочная сессия не найдена.';
        RETURN;
    END;

    UPDATE Парковочная_сессия
    SET Время_выезда = GETDATE()
    WHERE ID_сессии = @ID_сессии;

    SET @Результат = 'Сессия успешно завершена. Платёж сформирован автоматически.';

END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateTariff
    @Наименование VARCHAR(50),
    @Часы INT,
    @Стоимость DECIMAL(10, 2)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Тариф WHERE Наименование = @Наименование)
    BEGIN
        RAISERROR('Тариф с таким названием не найден.', 16, 1);
        RETURN;
    END;

    IF @Часы <= 0 OR @Стоимость <= 0
    BEGIN
        RAISERROR('Продолжительность и стоимость должны быть больше нуля.', 16, 1);
        RETURN;
    END;

    UPDATE Тариф
    SET Продолжительность_часов = @Часы,
        Стоимость = @Стоимость
    WHERE Наименование = @Наименование;
END;
GO

CREATE OR ALTER PROCEDURE sp_AddTariff
    @Наименование VARCHAR(50),
    @Часы INT,
    @Стоимость DECIMAL(10, 2)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Тариф WHERE Наименование = @Наименование)
    BEGIN
        RAISERROR('Тариф с таким названием уже существует.', 16, 1);
        RETURN;
    END;

    IF @Часы <= 0 OR @Стоимость <= 0
    BEGIN
        RAISERROR('Продолжительность и стоимость должны быть больше нуля.', 16, 1);
        RETURN;
    END;

    INSERT INTO Тариф (Наименование, Продолжительность_часов, Стоимость)
    VALUES (@Наименование, @Часы, @Стоимость);
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteTariff
    @Наименование VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID_тарифа INT;

    SELECT @ID_тарифа = ID_тарифа
    FROM Тариф
    WHERE Наименование = @Наименование;

    IF @ID_тарифа IS NULL
    BEGIN
        RAISERROR('Тариф с таким названием не найден.', 16, 1);
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM Платёж WHERE ID_тарифа = @ID_тарифа)
    BEGIN
        RAISERROR('Нельзя удалить этот тариф: по нему существуют платежи.', 16, 1);
        RETURN;
    END;

    DELETE FROM Тариф WHERE ID_тарифа = @ID_тарифа;
END;
GO

CREATE OR ALTER PROCEDURE sp_AddClientAndVehicle
    @ФИО_владельца VARCHAR(100),
    @Телефон VARCHAR(18),
    @Почта_владельца VARCHAR(100),

    @Гос_номер VARCHAR(12),
    @Тип_ТС VARCHAR(50),
    @Марка_ТС VARCHAR(50),
    @Модель_ТС VARCHAR(50),
    
    @Результат NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ClientID INT;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        SELECT @ClientID = ID_клиента 
        FROM Клиент 
        WHERE Телефон = @Телефон;

        IF @ClientID IS NULL
        BEGIN
            INSERT INTO Клиент (ФИО, Телефон, Электронная_почта)
            VALUES (@ФИО_владельца, @Телефон, @Почта_владельца);
            
            SET @ClientID = SCOPE_IDENTITY();
            
            SET @Результат = 'Успех: Создан новый клиент (ID: ' + CAST(@ClientID AS VARCHAR) + ') и добавлено ТС.';
        END
        ELSE
        BEGIN
            UPDATE Клиент
            SET ФИО = @ФИО_владельца, Электронная_почта = @Почта_владельца
            WHERE ID_клиента = @ClientID;
            
            SET @Результат = 'Успех: Найден существующий клиент (ID: ' + CAST(@ClientID AS VARCHAR) + ') и добавлено ТС.';
        END

        IF EXISTS (SELECT 1 FROM ТС WHERE Гос_номер = @Гос_номер)
        BEGIN
            SET @Результат = 'Ошибка: Автомобиль с госномером ' + @Гос_номер + ' уже зарегистрирован.';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO ТС (Гос_номер, ID_клиента, Тип, Марка, Модель)
        VALUES (@Гос_номер, @ClientID, @Тип_ТС, @Марка_ТС, @Модель_ТС);
        
        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Результат = 'Критическая ошибка: ' + ERROR_MESSAGE();
    END CATCH
END;
GO