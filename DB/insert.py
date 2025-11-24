import random
from faker import Faker
from datetime import datetime, timedelta

OUTPUT_FILENAME = "INSERT_DATA.sql"
NUM_CLIENTS = 20
NUM_EMPLOYEES = 10
NUM_SPOTS = 40
NUM_CARS = 30
NUM_SESSIONS = 100
UNFINISHED_PERCENTAGE = 0.1

fake = Faker('ru_RU')

def escape_sql(text):
    return str(text).replace("'", "''")

def get_random_date(start_date, end_date):
    delta = end_date - start_date
    int_delta = (delta.days * 24 * 60 * 60) + delta.seconds
    random_second = random.randrange(int_delta)
    return start_date + timedelta(seconds=random_second)

print("Генерация данных началась...")

queries = []
queries.append(f"USE Park_spot;\nGO\n")

# Клиенты 
queries.append("-- Клиенты")
queries.append("SET IDENTITY_INSERT Клиент ON;")
client_ids = []
for i in range(1, NUM_CLIENTS + 1):
    name = escape_sql(fake.name())
    email = fake.unique.email()
    phone = f"+79{random.randint(100000000, 999999999)}"
    queries.append(f"INSERT INTO Клиент (ID_клиента, ФИО, Электронная_почта, Телефон) VALUES ({i}, '{name}', '{email}', '{phone}');")
    client_ids.append(i)
queries.append("SET IDENTITY_INSERT Клиент OFF;\nGO\n")

# Сотрудники и Учетки
queries.append("-- Сотрудники и Учетные записи")
queries.append("SET IDENTITY_INSERT Сотрудник ON;")
employee_ids = []
account_queries = []

for i in range(1, NUM_EMPLOYEES + 1):
    name = escape_sql(fake.name())
    email = fake.unique.email()
    phone = f"+79{random.randint(100000000, 999999999)}"
    position = 'Менеджер' if i <= 2 else 'Оператор парковки'
    
    queries.append(f"INSERT INTO Сотрудник (ID_сотрудника, ФИО, Должность, Электронная_почта, Телефон) VALUES ({i}, '{name}', '{position}', '{email}', '{phone}');")
    employee_ids.append(i)
    
    login = f"user_{i}"
    account_queries.append(f"INSERT INTO УчетныеЗаписи (ID_сотрудника, Логин, Хеш_пароля) VALUES ({i}, '{login}', HASHBYTES('SHA2_256', 'password123'));")

queries.append("SET IDENTITY_INSERT Сотрудник OFF;")
queries.extend(account_queries)
queries.append("GO\n")

# Тарифы
queries.append("-- Тарифы")
queries.append("SET IDENTITY_INSERT Тариф ON;")
tariffs = [
    (1, 'Почасовой', 1, 100.00),
    (2, 'Льготный 3ч', 3, 250.00),
    (3, 'Ночной', 12, 500.00),
    (4, 'Суточный', 24, 1000.00),
    (5, 'Недельный', 168, 5000.00)
]
tariff_dict = {t[0]: {'dur': t[2], 'cost': t[3]} for t in tariffs}

for t in tariffs:
    queries.append(f"INSERT INTO Тариф (ID_тарифа, Наименование, Продолжительность_часов, Стоимость) VALUES ({t[0]}, '{t[1]}', {t[2]}, {t[3]});")
queries.append("SET IDENTITY_INSERT Тариф OFF;\nGO\n")

# Парковочные места
queries.append("-- Парковочные места")
spot_numbers = []
for i in range(1, NUM_SPOTS + 1):
    floor = 1 if i <= 20 else 2
    spot_num = (floor * 100) + (i if floor == 1 else i - 20)
    spot_numbers.append(spot_num)
    queries.append(f"INSERT INTO Парковочное_место (Номер_места, Этаж, Статус) VALUES ({spot_num}, {floor}, 'Свободно');")
queries.append("GO\n")

# Транспортные средства 
queries.append("-- ТС")
car_brands = {
    'Легковая': ['Toyota Camry', 'BMW X5', 'Kia Rio', 'Lada Vesta', 'Ford Focus'],
    'Грузовая': ['Volvo FH', 'MAN TGS', 'KAMAZ 5490']
}
plate_chars = 'ABCEHKMOPTXY'
cars_list = []

for _ in range(NUM_CARS):
    c_id = random.choice(client_ids)
    
    p1 = random.choice(plate_chars)
    p3 = "".join(random.choices(plate_chars, k=2))
    nums = f"{random.randint(1, 999):03d}"
    reg = f"{random.randint(77, 199)}"
    plate = f"{p1}{nums}{p3}{reg}"
    
    c_type = 'Легковая' if random.random() > 0.15 else 'Грузовая'
    full_brand = random.choice(car_brands[c_type]).split()
    brand, model = full_brand[0], full_brand[1]
    
    cars_list.append(plate)
    queries.append(f"INSERT INTO ТС (Гос_номер, ID_клиента, Тип, Марка, Модель) VALUES ('{plate}', {c_id}, '{c_type}', '{brand}', '{model}');")
queries.append("GO\n")

queries.append("-- Сессии и Платежи")
queries.append("SET IDENTITY_INSERT Парковочная_сессия ON;")
queries.append("SET IDENTITY_INSERT Платёж ON;")

occupied_spots = []
payment_id_counter = 1

start_simulation = datetime.now() - timedelta(days=60)
end_simulation = datetime.now()

for sess_id in range(1, NUM_SESSIONS + 1):
    car = random.choice(cars_list)
    emp = random.choice(employee_ids)
    
    # Время заезда
    time_in = get_random_date(start_simulation, end_simulation)
    
    is_active = False
    if sess_id > (NUM_SESSIONS * (1 - UNFINISHED_PERCENTAGE)):
        available_spots = list(set(spot_numbers) - set(occupied_spots))
        if not available_spots:
            continue
        spot = random.choice(available_spots)
        occupied_spots.append(spot)
        is_active = True
        time_out_sql = "NULL"
    else:
        spot = random.choice(spot_numbers)
        
        duration_minutes = random.randint(30, 2880) 
        time_out = time_in + timedelta(minutes=duration_minutes)
        
        if time_out > datetime.now():
            time_out = datetime.now() 
            
        time_out_sql = f"'{time_out.strftime('%Y-%m-%d %H:%M:%S')}'"
        
        duration_hours = (time_out - time_in).total_seconds() / 3600
        
        if duration_hours >= 24:
            t_id = 4
            count = duration_hours / 24
            cost = count * tariff_dict[4]['cost']
        else:
            t_id = 1
            import math
            hours_billed = math.ceil(duration_hours)
            cost = hours_billed * tariff_dict[1]['cost']

    time_in_str = time_in.strftime('%Y-%m-%d %H:%M:%S')
    
    queries.append(f"INSERT INTO Парковочная_сессия (ID_сессии, Гос_номер, Номер_места, ID_сотрудника, Время_заезда, Время_выезда) VALUES ({sess_id}, '{car}', {spot}, {emp}, '{time_in_str}', {time_out_sql});")
    
    if not is_active:
        queries.append(f"INSERT INTO Платёж (ID_платежа, ID_сессии, ID_тарифа, Сумма, Дата_платежа) VALUES ({payment_id_counter}, {sess_id}, {t_id}, {cost:.2f}, {time_out_sql});")
        payment_id_counter += 1

# Обновление статусов занятых мест
for spot in occupied_spots:
    queries.append(f"UPDATE Парковочное_место SET Статус = 'Занято' WHERE Номер_места = {spot};")

queries.append("SET IDENTITY_INSERT Парковочная_сессия OFF;")
queries.append("SET IDENTITY_INSERT Платёж OFF;")
queries.append("GO")

try:
    with open(OUTPUT_FILENAME, 'w', encoding='utf-8') as f:
        for q in queries:
            f.write(q + "\n")
    print(f"Готово! Файл {OUTPUT_FILENAME} создан.")
except Exception as e:
    print(f"Ошибка: {e}")