CREATE TRIGGER trg_ManageParkingStatus
ON Парковочная_сессия
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
    BEGIN
        DECLARE @InsertedSpot INT, @InsertedCar VARCHAR(9);
        SELECT @InsertedSpot = Номер_места, @InsertedCar = Гос_номер FROM inserted;

        IF (SELECT Статус FROM Парковочное_место WHERE Номер_места = @InsertedSpot) = 'Занято'
        BEGIN
            RAISERROR('Ошибка: Парковочное место %d уже занято.', 16, 1, @InsertedSpot);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF (SELECT COUNT(*) FROM Парковочная_сессия WHERE Гос_номер = @InsertedCar AND Время_выезда IS NULL) > 1
        BEGIN
            RAISERROR('Ошибка: Транспортное средство %s уже находится на парковке.', 16, 1, @InsertedCar);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        UPDATE Парковочное_место
        SET Статус = 'Занято'
        WHERE Номер_места = @InsertedSpot;
        
    END;

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        IF (SELECT Время_выезда FROM inserted) IS NOT NULL AND (SELECT Время_выезда FROM deleted) IS NULL
        BEGIN
            DECLARE @FreedSpot INT;
            SELECT @FreedSpot = Номер_места FROM inserted;

            UPDATE Парковочное_место
            SET Статус = 'Свободно'
            WHERE Номер_места = @FreedSpot;
        END;
    END;
    
END;


