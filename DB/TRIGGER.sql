CREATE   TRIGGER trg_CreatePaymentOnSessionEnd
ON Парковочная_сессия
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Время_выезда)
    BEGIN
        INSERT INTO Платёж (ID_сессии, ID_тарифа, Сумма, Дата_платежа)
        SELECT
            i.ID_сессии,
            COALESCE(
                (SELECT TOP 1 ID_тарифа
                 FROM Тариф
                 WHERE Продолжительность_часов <= DATEDIFF(MINUTE, i.Время_заезда, i.Время_выезда) / 60.0
                 ORDER BY Продолжительность_часов DESC),
                (SELECT TOP 1 ID_тарифа FROM Тариф ORDER BY Продолжительность_часов ASC)
            ) AS ID_тарифа,
            COALESCE(
                 (SELECT TOP 1
                    CEILING((DATEDIFF(MINUTE, i.Время_заезда, i.Время_выезда) / 60.0) / t.Продолжительность_часов) * t.Стоимость
                  FROM Тариф t
                  WHERE t.ID_тарифа = COALESCE(
                        (SELECT TOP 1 ID_тарифа
                         FROM Тариф
                         WHERE Продолжительность_часов <= DATEDIFF(MINUTE, i.Время_заезда, i.Время_выезда) / 60.0
                         ORDER BY Продолжительность_часов DESC),
                        (SELECT TOP 1 ID_тарифа FROM Тариф ORDER BY Продолжительность_часов ASC)
                  )
                 ),
                 0
            ) AS Сумма,
            i.Время_выезда
        FROM inserted i
        INNER JOIN deleted d ON i.ID_сессии = d.ID_сессии
        WHERE
            d.Время_выезда IS NULL
            AND i.Время_выезда IS NOT NULL
            AND NOT EXISTS (
                SELECT 1 FROM Платёж WHERE ID_сессии = i.ID_сессии
            );
    END
END;
GO

CREATE OR ALTER TRIGGER trg_UpdateParkingSpotStatus
ON Парковочная_сессия
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Парковочное_место
    SET Статус = 'Занято'
    WHERE Номер_места IN (
        SELECT i.Номер_места
        FROM inserted i
        WHERE i.Время_выезда IS NULL
    );
    
    IF UPDATE(Время_выезда)
    BEGIN
        UPDATE Парковочное_место
        SET Статус = 'Свободно'
        WHERE Номер_места IN (
            SELECT i.Номер_места
            FROM inserted i
            INNER JOIN deleted d ON i.ID_сессии = d.ID_сессии
            WHERE d.Время_выезда IS NULL 
                AND i.Время_выезда IS NOT NULL
        );
    END
END;
GO

USE [Park_spot]
GO

CREATE TRIGGER trg_ClientCheckContacts
ON Клиент
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (
        SELECT 1 
        FROM inserted i
        INNER JOIN Сотрудник с ON i.Телефон = с.Телефон
    )
    BEGIN
        RAISERROR('Ошибка: телефон клиента совпадает с телефоном сотрудника', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    IF EXISTS (
        SELECT 1 
        FROM inserted i
        INNER JOIN Сотрудник с ON i.Электронная_почта = с.Электронная_почта
        WHERE i.Электронная_почта IS NOT NULL
    )
    BEGIN
        RAISERROR('Ошибка: электронная почта клиента совпадает с почтой сотрудника', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

CREATE TRIGGER trg_EmployeeCheckContacts
ON Сотрудник
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM inserted i
        INNER JOIN Клиент к ON i.Телефон = к.Телефон
    )
    BEGIN
        RAISERROR('Ошибка: телефон сотрудника совпадает с телефоном клиента', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS (
        SELECT 1 
        FROM inserted i
        INNER JOIN Клиент к ON i.Электронная_почта = к.Электронная_почта
        WHERE i.Электронная_почта IS NOT NULL
    )
    BEGIN
        RAISERROR('Ошибка: электронная почта сотрудника совпадает с почтой клиента', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO