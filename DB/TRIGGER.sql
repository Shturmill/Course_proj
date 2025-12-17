CREATE OR ALTER TRIGGER trg_CreatePaymentOnSessionEnd
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
            (SELECT TOP 1 ID_тарифа 
             FROM Тариф 
             WHERE Продолжительность_часов <= DATEDIFF(HOUR, i.Время_заезда, i.Время_выезда)
             ORDER BY Продолжительность_часов DESC),
            (SELECT TOP 1 
                CEILING(DATEDIFF(MINUTE, i.Время_заезда, i.Время_выезда) / 60.0 / t.Продолжительность_часов) * t.Стоимость
             FROM Тариф t
             WHERE t.Продолжительность_часов <= DATEDIFF(HOUR, i.Время_заезда, i.Время_выезда)
             ORDER BY t.Продолжительность_часов DESC),
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

CREATE TRIGGER [dbo].[trg_CheckClientPhone]
ON [dbo].[Клиент]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    

    IF UPDATE(Телефон) OR EXISTS(SELECT 1 FROM inserted)
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM inserted i
            INNER JOIN Сотрудник s ON i.Телефон = s.Телефон
        )
        BEGIN
            RAISERROR('Ошибка: Номер телефона клиента совпадает с номером телефона сотрудника. Операция отменена.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END;
GO

CREATE TRIGGER [dbo].[trg_CheckEmployeePhone]
ON [dbo].[Сотрудник]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(Телефон) OR EXISTS(SELECT 1 FROM inserted)
    BEGIN

        IF EXISTS (
            SELECT 1
            FROM inserted i
            INNER JOIN Клиент k ON i.Телефон = k.Телефон
        )
        BEGIN
            RAISERROR('Ошибка: Номер телефона сотрудника совпадает с номером телефона клиента. Операция отменена.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END;
GO