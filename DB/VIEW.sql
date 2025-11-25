CREATE OR ALTER VIEW vw_SessionDetails AS
SELECT 
    ps.Гос_номер,
    ps.Номер_места,
    ps.Время_заезда,
    (t.Марка + ' ' + t.Модель) AS Автомобиль,
    k.Телефон AS Телефон_владельца,
    k.ФИО AS ФИО_владельца
FROM Парковочная_сессия ps
JOIN ТС t ON ps.Гос_номер = t.Гос_номер
JOIN Клиент k ON t.ID_клиента = k.ID_клиента
WHERE ps.Время_выезда IS NULL;
GO

CREATE OR ALTER VIEW vw_VehicleRegistry AS
SELECT 
    t.Гос_номер,
    t.Тип,
    (t.Марка + ' ' + t.Модель) AS Автомобиль,
    k.Телефон AS Телефон_владельца
FROM ТС t
JOIN Клиент k ON t.ID_клиента = k.ID_клиента;
GO

CREATE OR ALTER VIEW vw_PaymentReport AS
SELECT 
    p.Дата_платежа,
    DATEDIFF(MINUTE, ps.Время_заезда, ps.Время_выезда) AS Длительность_мин,
    t.Тип,
    (t.Марка + ' ' + t.Модель) AS Автомобиль,
    k.ФИО AS Владелец
FROM Платёж p
JOIN Парковочная_сессия ps ON p.ID_сессии = ps.ID_сессии
JOIN ТС t ON ps.Гос_номер = t.Гос_номер
JOIN Клиент k ON t.ID_клиента = k.ID_клиента;
GO