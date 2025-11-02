import random
from faker import Faker
from datetime import datetime, timedelta

OUTPUT_FILENAME = "INSERT.sql"
NUM_CLIENTS = 20
NUM_EMPLOYEES = 20
NUM_CARS = 25
NUM_TARIFFS = 20
NUM_SPOTS = 50 
NUM_SESSIONS_PAYMENTS = 120

LICENSE_PLATE_CHARS = 'ABCEXHKMOPT'

fake = Faker('ru_RU')
sql_queries = []

# Списки для хранения ID и данных для связей
client_ids = list(range(1, NUM_CLIENTS + 1))
employee_ids = list(range(1, NUM_EMPLOYEES + 1))
spot_numbers = []
car_numbers = []
# Словарь для тарифов: {id: (продолжительность_в_часах, стоимость)}
tariffs_data = {}

print(f"Генерация {OUTPUT_FILENAME} началась...")

sql_queries.append("\n-- 1. Заполнение таблицы «Клиент»")
for i in client_ids:
    name = fake.name().replace("'", "''")
    email = fake.email()
    phone = f"+79{fake.random_number(digits=9, fix_len=True)}"
    sql_queries.append(
        f"INSERT INTO Клиент (ID_клиента, ФИО, Электронная_почта, Телефон) "
        f"VALUES ({i}, '{name}', '{email}', '{phone}');"
    )

sql_queries.append("\n-- 2. Заполнение таблицы «Сотрудник»")
for i in employee_ids:
    name = fake.name().replace("'", "''")
    email = fake.email()
    phone = f"+79{fake.random_number(digits=9, fix_len=True)}"
    position = random.choice(['Оператор парковки', 'Менеджер'])
    sql_queries.append(
        f"INSERT INTO Сотрудник (ID_сотрудника, ФИО, Должность, Электронная_почта, Телефон) "
        f"VALUES ({i}, '{name}', '{position}', '{email}', '{phone}');"
    )

sql_queries.append("\n-- 3. Заполнение таблицы «Тариф»")
base_tariffs = [
    (1, 'Почасовой', 1, 150.00),
    (2, '3 часа', 3, 400.00),
    (3, 'Дневной (8 часов)', 8, 800.00),
    (4, 'Суточный', 24, 1200.00),
    (5, 'Недельный', 168, 5000.00)
]

for i, name, duration, cost in base_tariffs:
    tariffs_data[i] = (duration, cost)
    sql_queries.append(
        f"INSERT INTO Тариф (ID_тарифа, Наименование, Продолжительность_часов, Стоимость) "
        f"VALUES ({i}, '{name}', {duration}, {cost});"
    )
#TO DO: расписать больше тарифов
for i in range(len(base_tariffs) + 1, NUM_TARIFFS + 1):
    duration = random.randint(2, 48)
    cost = duration * random.randint(100, 130) # Расчетная стоимость
    name = f"Спец-тариф {duration}ч. (ID {i})"
    tariffs_data[i] = (duration, cost)
    sql_queries.append(
        f"INSERT INTO Тариф (ID_тарифа, Наименование, Продолжительность_часов, Стоимость) "
        f"VALUES ({i}, '{name}', {duration}, {cost});"
    )

sql_queries.append("\n-- 4. Заполнение таблицы «Парковочное место»")
floor = 1
for i in range(1, NUM_SPOTS + 1):
    if i > 25:
        floor = 2 
    
    spot_num = (floor * 100) + (i % 25 if i % 25 != 0 else 25)
    spot_numbers.append(spot_num)
    sql_queries.append(
        f"INSERT INTO Парковочное_место (Номер_места, Этаж, Статус) "
        f"VALUES ({spot_num}, {floor}, 'Свободно');"
    )


sql_queries.append("\n-- 5. Заполнение таблицы «ТС»")
sql_queries.append("-- Гарантируется, что каждому клиенту принадлежит хотя бы один автомобиль")
car_brands_models = {
    'Lada': ['Vesta', 'Granta'],
    'Toyota': ['Camry', 'RAV4'],
    'Mercedes': ['E200', 'GLC'],
    'BMW': ['X5', '520i'],
    'KAMAZ': ['5490', '65115'], 
    'MAN': ['TGS', 'TGX']      
}
light_brands = ['Lada', 'Toyota', 'Mercedes', 'BMW']
heavy_brands = ['KAMAZ', 'MAN']

if NUM_CARS < NUM_CLIENTS:
    print(f"ПРЕДУПРЕЖДЕНИЕ: Машин ({NUM_CARS}) меньше, чем клиентов ({NUM_CLIENTS}). Невозможно каждому клиенту дать машину.")
    for i in range(1, NUM_CARS + 1):
        num = f"{fake.random_number(digits=3, fix_len=True)}"
        region = f"{random.randint(100, 999)}".zfill(3)
        plate = f"{random.choice(LICENSE_PLATE_CHARS)}{num}{random.choice(LICENSE_PLATE_CHARS)}{random.choice(LICENSE_PLATE_CHARS)}{region}"
        car_numbers.append(plate)
        car_type = random.choice(['Легковая', 'Грузовая'])
        brand = random.choice(light_brands) if car_type == 'Легковая' else random.choice(heavy_brands)
        model = random.choice(car_brands_models[brand])
        client_id = random.choice(client_ids) 
        sql_queries.append(
            f"INSERT INTO ТС (Гос_номер, ID_клиента, Тип, Марка, Модель) "
            f"VALUES ('{plate}', {client_id}, '{car_type}', '{brand}', '{model}');"
        )
else:
    for i in range(1, NUM_CLIENTS + 1):
        num = f"{fake.random_number(digits=3, fix_len=True)}"
        region = f"{random.randint(100, 999)}".zfill(3)
        plate = f"{random.choice(LICENSE_PLATE_CHARS)}{num}{random.choice(LICENSE_PLATE_CHARS)}{random.choice(LICENSE_PLATE_CHARS)}{region}"
        car_numbers.append(plate)
        
        car_type = random.choice(['Легковая', 'Грузовая'])
        brand = random.choice(light_brands) if car_type == 'Легковая' else random.choice(heavy_brands)
        model = random.choice(car_brands_models[brand])
        
        client_id = i # Прямое присвоение: i-я машина -> i-му клиенту
        
        sql_queries.append(
            f"INSERT INTO ТС (Гос_номер, ID_клиента, Тип, Марка, Модель) "
            f"VALUES ('{plate}', {client_id}, '{car_type}', '{brand}', '{model}');"
        )
        
    # Раздаем оставшиеся машины (если NUM_CARS > NUM_CLIENTS) случайным клиентам
    for i in range(NUM_CLIENTS + 1, NUM_CARS + 1):
        num = f"{fake.random_number(digits=3, fix_len=True)}"
        region = f"{random.randint(100, 999)}".zfill(3)
        plate = f"{random.choice(LICENSE_PLATE_CHARS)}{num}{random.choice(LICENSE_PLATE_CHARS)}{random.choice(LICENSE_PLATE_CHARS)}{region}"
        car_numbers.append(plate)
        
        car_type = random.choice(['Легковая', 'Грузовая'])
        brand = random.choice(light_brands) if car_type == 'Легковая' else random.choice(heavy_brands)
        model = random.choice(car_brands_models[brand])
        
        client_id = random.choice(client_ids) # Случайный клиент для лишних машин
        
        sql_queries.append(
            f"INSERT INTO ТС (Гос_номер, ID_клиента, Тип, Марка, Модель) "
            f"VALUES ('{plate}', {client_id}, '{car_type}', '{brand}', '{model}');"
        )


sql_queries.append("\n-- 6. и 7. Заполнение таблиц «Парковочная сессия» и «Платёж»")

now = datetime.now()
session_payment_ids = list(range(1, NUM_SESSIONS_PAYMENTS + 1))
tariff_ids = list(tariffs_data.keys())

for i in session_payment_ids:
    # 1. Выбираем данные для связи
    session_id = i
    payment_id = i
    
    # 2. Выбираем тариф -> из него берем длительность и стоимость
    tariff_id = random.choice(tariff_ids)
    duration_hours, cost = tariffs_data[tariff_id]

    # 3. Дата
    zaezd_datetime = now - timedelta(
        days=random.randint(1, 365),
        hours=random.randint(0, 23),
        minutes=random.randint(0, 59)
    )
    # Время выезда = время заезда + длительность тарифа
    vyezd_datetime = zaezd_datetime + timedelta(hours=duration_hours)
    
    # Форматируем в SQL SMALLDATETIME (YYYY-MM-DD HH:MM:SS)
    zaezd_str = zaezd_datetime.strftime('%Y-%m-%d %H:%M:%S')
    vyezd_str = vyezd_datetime.strftime('%Y-%m-%d %H:%M:%S') # Дата платежа = время выезда
    
    # 4. Выбираем остальные FK
    gos_nomer = random.choice(car_numbers)
    nomer_mesta = random.choice(spot_numbers)
    id_sotrudnika = random.choice(employee_ids)
    
    # 5. Генерируем INSERT для сессии
    sql_queries.append(
        f"INSERT INTO Парковочная_сессия (ID_сессии, Гос_номер, Номер_места, ID_сотрудника, Время_заезда, Время_выезда) "
        f"VALUES ({session_id}, '{gos_nomer}', {nomer_mesta}, {id_sotrudnika}, '{zaezd_str}', '{vyezd_str}');"
    )
    
    # 6. Генерируем INSERT для платежа
    # (ID_сессии = UNIQUE, Сумма = стоимость тарифа, Дата_платежа = время выезда)
    sql_queries.append(
        f"INSERT INTO Платёж (ID_платежа, ID_сессии, ID_тарифа, Сумма, Дата_платежа) "
        f"VALUES ({payment_id}, {session_id}, {tariff_id}, {cost:.2f}, '{vyezd_str}');"
    )
    sql_queries.append("") # Пустая строка для читаемости

try:
    with open(OUTPUT_FILENAME, 'w', encoding='utf-8') as f:
       
        for query in sql_queries:
            f.write(query + '\n')
    
    print(f"\nУспешно. Файл '{OUTPUT_FILENAME}' создан и содержит {len(sql_queries)} запросов.")

except Exception as e:
    print(f"Ошибка при записи в файл: {e}")