DROP TABLE IF EXISTS Платёж;
DROP TABLE IF EXISTS Парковочная_сессия;
DROP TABLE IF EXISTS Тариф;
DROP TABLE IF EXISTS Сотрудник;
DROP TABLE IF EXISTS Парковочное_место;
DROP TABLE IF EXISTS ТС;
DROP TABLE IF EXISTS Клиент;
DROP TABLE IF EXISTS УчетныеЗаписи;

CREATE TABLE Клиент (
    ID_клиента INT IDENTITY(1,1) PRIMARY KEY,
    ФИО VARCHAR(100) NOT NULL,
    Электронная_почта VARCHAR(100) UNIQUE,
    Телефон VARCHAR(20) NOT NULL UNIQUE,
    CHECK (Электронная_почта LIKE '%_@__%.__%'),
    CHECK (Телефон LIKE '+7%')
);

CREATE TABLE ТС (
    Гос_номер VARCHAR(9) PRIMARY KEY,
    ID_клиента INT NOT NULL,
    Тип VARCHAR(8) NOT NULL DEFAULT 'Легковая',
    Марка VARCHAR(31) NOT NULL,
    Модель VARCHAR(31) NOT NULL,
    FOREIGN KEY (ID_клиента) REFERENCES Клиент(ID_клиента) ON DELETE CASCADE,
    CHECK (Тип IN ('Легковая', 'Грузовая')),
    CHECK (Гос_номер NOT LIKE '%[^A-Z0-9]%')
);

CREATE TABLE Парковочное_место (
    Номер_места INT PRIMARY KEY,
    Этаж INT NOT NULL,
    Статус VARCHAR(8) NOT NULL DEFAULT 'Свободно',
    CHECK (Статус IN ('Свободно', 'Занято')),
    CHECK (Этаж >= 0)
);

CREATE TABLE Сотрудник (
    ID_сотрудника INT IDENTITY(1,1) PRIMARY KEY,
    ФИО VARCHAR(100) NOT NULL,
    Должность VARCHAR(18) NOT NULL DEFAULT 'Оператор парковки',
    Электронная_почта VARCHAR(100) UNIQUE,
    Телефон VARCHAR(20) NOT NULL UNIQUE,
    CHECK (Должность IN ('Оператор парковки', 'Менеджер')),
    CHECK (Электронная_почта LIKE '%_@_%._%'),
    CHECK (Телефон LIKE '+7%')
);

CREATE TABLE Тариф (
    ID_тарифа INT IDENTITY(1,1) PRIMARY KEY,
    Наименование VARCHAR(50) NOT NULL UNIQUE,
    Продолжительность_часов INT NOT NULL,
    Стоимость DECIMAL(10, 2) NOT NULL,
    CHECK (Продолжительность_часов > 0),
    CHECK (Стоимость > 0)
);

CREATE TABLE Парковочная_сессия (
    ID_сессии INT IDENTITY(1,1) PRIMARY KEY,
    Гос_номер VARCHAR(9) NOT NULL,
    Номер_места INT NOT NULL,
    ID_сотрудника INT NOT NULL,
    Время_заезда SMALLDATETIME NOT NULL DEFAULT GETDATE(),
    Время_выезда SMALLDATETIME NULL,
    FOREIGN KEY (Гос_номер) REFERENCES ТС(Гос_номер) ON DELETE CASCADE,
    FOREIGN KEY (Номер_места) REFERENCES Парковочное_место(Номер_места) ON DELETE NO ACTION,
    FOREIGN KEY (ID_сотрудника) REFERENCES Сотрудник(ID_сотрудника) ON DELETE NO ACTION,
    CHECK (Время_выезда IS NULL OR Время_выезда > Время_заезда)
);

CREATE TABLE Платёж (
    ID_платежа INT IDENTITY(1,1) PRIMARY KEY,
    ID_сессии INT NOT NULL UNIQUE,
    ID_тарифа INT NOT NULL,
    Сумма DECIMAL(10, 2) NOT NULL,
    Дата_платежа SMALLDATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (ID_сессии) REFERENCES Парковочная_сессия(ID_сессии) ON DELETE CASCADE,
    FOREIGN KEY (ID_тарифа) REFERENCES Тариф(ID_тарифа) ON DELETE NO ACTION,
    CHECK (Сумма > 0)
);

CREATE TABLE УчетныеЗаписи (
    ID_учетной_записи INT IDENTITY(1,1) PRIMARY KEY,
    ID_сотрудника INT NOT NULL UNIQUE,
    Логин NVARCHAR(50) NOT NULL UNIQUE,
    Хеш_пароля VARBINARY(32) NOT NULL, 
    Дата_создания SMALLDATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ID_сотрудника) REFERENCES Сотрудник(ID_сотрудника) ON DELETE NO ACTION
);