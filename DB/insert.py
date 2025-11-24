import random
from faker import Faker
from datetime import datetime, timedelta

# Конфигурация
OUTPUT_FILENAME = "INSERT_DATA.sql"
NUM_CLIENTS = 20
NUM_EMPLOYEES = 10
NUM_SPOTS = 40
NUM_CARS = 30
NUM_SESSIONS = 100
UNFINISHED_PERCENTAGE = 0.10

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

# Клиенты (с автоинкрементом)
queries.append("-- Клиенты")
client_ids = []
for i in range(1, NUM_CLIENTS + 1):
    name = escape_sql(fake.name())
    email = fake.unique.email()
    phone = f"+79{random.randint(100000000, 999999999)}"
    queries.append(f"INSERT INTO Клиент (ФИО, Электронная_почта, Телефон) VALUES ('{name}', '{email}', '{phone}');")
    client_ids.append(i)
queries.append("GO\n")

# Сотрудники (с автоинкрементом)
queries.append("-- Сотрудники")
employee_ids = []
for i in range(1, NUM_EMPLOYEES + 1):
    name = escape_sql(fake.name())
    email = fake.unique.email()
    phone = f"+79{random.randint(100000000, 999999999)}"
    position = 'Менеджер' if i <= 2 else 'Оператор парковки'
    
    queries.append(f"INSERT INTO Сотрудник (ФИО, Должность, Электронная_почта, Телефон) VALUES ('{name}', '{position}', '{email}', '{phone}');")
    employee_ids.append(i)
queries.append("GO\n")

# Учетные записи
queries.append("-- Учетные записи")
for i in range(1, NUM_EMPLOYEES + 1):
    login = f"user_{i}"
    queries.append(f"INSERT INTO УчетныеЗаписи (ID_сотрудника, Логин, Хеш_пароля) VALUES ({i}, '{login}', HASHBYTES('SHA2_256', 'password123'));")
queries.append("GO\n")

# Тарифы (с автоинкрементом)
queries.append("-- Тарифы")
tariffs = [
    ('Почасовой', 1, 100.00),
    ('Льготный 3ч', 3, 250.00),
    ('Ночной', 12, 500.00),
    ('Суточный', 24, 1000.00),
    ('Недельный', 168, 5000.00)
]

for t in tariffs:
    queries.append(f"INSERT INTO Тариф (Наименование, Продолжительность_часов, Стоимость) VALUES ('{t[0]}', {t[1]}, {t[2]});")
queries.append("GO\n")

# Парковочные места
queries.append("-- Парковочные места")
spot_numbers = []
for i in range(1, NUM_SPOTS + 1):
    floor = 1 if i <= 20 else 2
    spot_num = (floor * 100) + (i if floor == 1 else i - 20)
    spot_numbers.append(spot_num)
    queries.append(f"INSERT INTO Парковочное_место (Номер_места, Этаж, Статус) VALUES ({spot_num}, {floor}, 'Свободно');")
queries.append("GO\n")

# Транспортные средства (ТС)
queries.append("-- ТС")
car_brands = {
    'Легковая': ['Toyota Camry', 'BMW X5', 'Kia Rio', 'Lada Vesta', 'Ford Focus'],
    'Грузовая': ['Volvo FH', 'MAN TGS', 'KAMAZ 5490']
}
plate_chars = 'ABCEHKMOPTXY' 
cars_list = []

for _ in range(NUM_CARS):
    c_id = random.choice(client_ids)
    
    # Генерация номера
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

# Сессии - сортируем по времени заезда
queries.append("-- Сессии (отсортированы по времени заезда)")

occupied_spots = []
start_simulation = datetime.now() - timedelta(days=60)
end_simulation = datetime.now()

# Создаем список сессий с временем
sessions_data = []

for sess_id in range(1, NUM_SESSIONS + 1):
    car = random.choice(cars_list)
    emp = random.choice(employee_ids)
    
    time_in = get_random_date(start_simulation, end_simulation)
    
    # Определяем активность сессии
    is_active = sess_id > (NUM_SESSIONS * (1 - UNFINISHED_PERCENTAGE))
    
    if is_active:
        available_spots = list(set(spot_numbers) - set(occupied_spots))
        if not available_spots:
            continue
        spot = random.choice(available_spots)
        occupied_spots.append(spot)
        time_out = None
    else:
        spot = random.choice(spot_numbers)
        duration_minutes = random.randint(60, 2880)
        time_out = time_in + timedelta(minutes=duration_minutes)
        
        if time_out > datetime.now():
            time_out = datetime.now()
    
    sessions_data.append({
        'car': car,
        'spot': spot,
        'emp': emp,
        'time_in': time_in,
        'time_out': time_out,
        'is_active': is_active
    })

# Сортируем по времени заезда
sessions_data.sort(key=lambda x: x['time_in'])

# Вставляем отсортированные сессии
for session in sessions_data:
    time_in_str = session['time_in'].strftime('%Y-%m-%d %H:%M:%S')
    time_out_sql = "NULL" if session['time_out'] is None else f"'{session['time_out'].strftime('%Y-%m-%d %H:%M:%S')}'"
    
    queries.append(f"INSERT INTO Парковочная_сессия (Гос_номер, Номер_места, ID_сотрудника, Время_заезда, Время_выезда) VALUES ('{session['car']}', {session['spot']}, {session['emp']}, '{time_in_str}', {time_out_sql});")

queries.append("GO")

try:
    with open(OUTPUT_FILENAME, 'w', encoding='utf-8') as f:
        for q in queries:
            f.write(q + "\n")
    print(f"Готово! Файл {OUTPUT_FILENAME} создан.")
    print(f"Создано сессий: {len(sessions_data)}")
    print(f"Активных сессий: {len(occupied_spots)}")
except Exception as e:
    print(f"Ошибка: {e}")