CREATE OR ALTER VIEW vw_SessionDetails AS
SELECT 
    ps.Гос_номер AS 'Гос номер',
    ps.Номер_места AS 'Номер места',
    ps.Время_заезда AS 'Время заезда',
    (t.Марка + ' ' + t.Модель) AS Автомобиль,
    k.Телефон AS 'Телефон владельца',
    k.ФИО AS 'ФИО владельца'
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
    k.Телефон AS 'Телефон владельца'
FROM ТС t
JOIN Клиент k ON t.ID_клиента = k.ID_клиента;
GO

CREATE OR ALTER VIEW vw_PaymentReport AS
SELECT 
    p.Дата_платежа AS 'Дата платежа',
    p.Сумма AS 'Сумма платежа',
    DATEDIFF(MINUTE, ps.Время_заезда, ps.Время_выезда) AS 'Длительность мин',
    t.Тип,
    (t.Марка + ' ' + t.Модель) AS Автомобиль,
    k.ФИО AS Владелец
FROM Платёж p
JOIN Парковочная_сессия ps ON p.ID_сессии = ps.ID_сессии
JOIN ТС t ON ps.Гос_номер = t.Гос_номер
JOIN Клиент k ON t.ID_клиента = k.ID_клиента;
GO