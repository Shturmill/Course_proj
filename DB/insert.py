import random
from faker import Faker
from datetime import datetime, timedelta

OUTPUT_FILENAME = "INSERT_DATA.sql"

NUM_CLIENTS = 20
NUM_EMPLOYEES = 10
NUM_CARS = 25
NUM_TARIFFS = 5
NUM_SPOTS = 40

LICENSE_PLATE_CHARS = 'ABCEXHKMOPT'

fake = Faker('ru_RU')
sql_queries = []

client_ids = list(range(1, NUM_CLIENTS + 1))
employee_ids = list(range(1, NUM_EMPLOYEES + 1))
spot_numbers = []
car_numbers = []
tariffs_data = {}

print(f"Генерация {OUTPUT_FILENAME} началась...")

def wrap_identity_insert(table_name, queries_list):
    return [f"SET IDENTITY_INSERT {table_name} ON;"] + queries_list + [f"SET IDENTITY_INSERT {table_name} OFF;", "GO"]

client_queries = []
used_emails = set()
used_phones = set()

for i in client_ids:
    name = fake.name().replace("'", "''")

    while True:
        email = fake.unique.email()
        if email not in used_emails:
            used_emails.add(email)
            break
            
    phone = f"+79{fake.random_number(digits=9, fix_len=True)}"
    
    client_queries.append(
        f"INSERT INTO Клиент (ID_клиента, ФИО, Электронная_почта, Телефон) "
        f"VALUES ({i}, '{name}', '{email}', '{phone}');"
    )

sql_queries.extend(wrap_identity_insert("Клиент", client_queries))
sql_queries.append("\n")


emp_queries = []
account_queries = []

for i in employee_ids:
    name = fake.name().replace("'", "''")
    email = fake.unique.email()
    phone = f"+79{fake.random_number(digits=9, fix_len=True)}"
    position = 'Менеджер' if i <= 2 else 'Оператор парковки'
    
    emp_queries.append(
        f"INSERT INTO Сотрудник (ID_сотрудника, ФИО, Должность, Электронная_почта, Телефон) "
        f"VALUES ({i}, '{name}', '{position}', '{email}', '{phone}');"
    )
    
    # Учетная запись
    login = email.split('@')[0].replace("'", "")[:30] + str(i) # Добавляем ID для уникальности логина
    default_pass = 'admin123'
    # В SQL Server HASHBYTES возвращает varbinary, insert корректен
    account_queries.append(
        f"INSERT INTO УчетныеЗаписи (ID_сотрудника, Логин, Хеш_пароля) "
        f"VALUES ({i}, '{login}', HASHBYTES('SHA2_256', '{default_pass}'));"
    )

sql_queries.extend(wrap_identity_insert("Сотрудник", emp_queries))
sql_queries.append("GO")
sql_queries.extend(account_queries)
sql_queries.append("GO\n")

tariff_queries = []
base_tariffs = [
    (1, 'Почасовой', 1, 100.00),
    (2, '3 часа', 3, 250.00),
    (3, 'Ночной', 12, 500.00),
    (4, 'Суточный', 24, 1000.00),
    (5, 'Недельный', 168, 5000.00)
]

for t_id, name, dur, cost in base_tariffs:
    tariffs_data[t_id] = (dur, cost)
    tariff_queries.append(
        f"INSERT INTO Тариф (ID_тарифа, Наименование, Продолжительность_часов, Стоимость) "
        f"VALUES ({t_id}, '{name}', {dur}, {cost});"
    )

sql_queries.extend(wrap_identity_insert("Тариф", tariff_queries))
sql_queries.append("\n")


spot_queries = []
for i in range(1, NUM_SPOTS + 1):
    floor = 1 if i <= 20 else 2
    # Формат номера 101..120, 201..220
    spot_num = (floor * 100) + (i if floor == 1 else i - 20)
    spot_numbers.append(spot_num)
    
    spot_queries.append(
        f"INSERT INTO Парковочное_место (Номер_места, Этаж, Статус) "
        f"VALUES ({spot_num}, {floor}, 'Свободно');"
    )

sql_queries.extend(spot_queries)
sql_queries.append("GO\n")

ts_queries = []
car_brands = {
    'Легковая': {'Toyota': ['Camry', 'Corolla'], 'BMW': ['X5', '320i'], 'Kia': ['Rio', 'Sportage']},
    'Грузовая': {'MAN': ['TGS', 'TGX'], 'Volvo': ['FH', 'FM'], 'KAMAZ': ['5490']}
}

current_client_idx = 0
for i in range(NUM_CARS):
    if current_client_idx < NUM_CLIENTS:
        c_id = client_ids[current_client_idx]
        current_client_idx += 1
    else:
        c_id = random.choice(client_ids)

    n1 = random.choice(LICENSE_PLATE_CHARS)
    n2 = random.choice(LICENSE_PLATE_CHARS)
    n3 = random.choice(LICENSE_PLATE_CHARS)
    digits = f"{random.randint(1, 999):03d}"
    region = f"{random.randint(77, 999)}"
    
    plate = f"{n1}{digits}{n2}{n3}{region}"
    car_numbers.append(plate)
    
    c_type = 'Легковая' if random.random() > 0.1 else 'Грузовая'
    brand = random.choice(list(car_brands[c_type].keys()))
    model = random.choice(car_brands[c_type][brand])
    
    ts_queries.append(
        f"INSERT INTO ТС (Гос_номер, ID_клиента, Тип, Марка, Модель) "
        f"VALUES ('{plate}', {c_id}, '{c_type}', '{brand}', '{model}');"
    )

sql_queries.extend(ts_queries)
sql_queries.append("GO\n")


session_queries = []
payment_queries = []

global_session_id = 1
global_payment_id = 1

start_history = datetime.now() - timedelta(days=30)
now = datetime.now()

for spot in spot_numbers:
    # Для каждого места начинаем историю 30 дней назад
    current_time = start_history + timedelta(hours=random.randint(0, 48))
    
    while current_time < now:
        # 1. Будет ли машина парковаться? (вероятность 90%)
        if random.random() > 0.1:
            car = random.choice(car_numbers)
            emp = random.choice(employee_ids)
            
            # Выбираем тариф
            t_id = random.choice(list(tariffs_data.keys()))
            duration, cost = tariffs_data[t_id]
            
            actual_duration_sec = duration * 3600 + random.randint(-600, 1800)
            
            time_in = current_time
            time_out = time_in + timedelta(seconds=actual_duration_sec)
            
            time_in_str = time_in.strftime('%Y-%m-%d %H:%M:%S')
            
            if time_out > now:
                session_queries.append(
                    f"INSERT INTO Парковочная_сессия (ID_сессии, Гос_номер, Номер_места, ID_сотрудника, Время_заезда, Время_выезда) "
                    f"VALUES ({global_session_id}, '{car}', {spot}, {emp}, '{time_in_str}', NULL);"
                )

                
                global_session_id += 1
                break 
            else:
                time_out_str = time_out.strftime('%Y-%m-%d %H:%M:%S')
                
                session_queries.append(
                    f"INSERT INTO Парковочная_сессия (ID_сессии, Гос_номер, Номер_места, ID_сотрудника, Время_заезда, Время_выезда) "
                    f"VALUES ({global_session_id}, '{car}', {spot}, {emp}, '{time_in_str}', '{time_out_str}');"
                )
                
                # Генерируем платеж
                payment_queries.append(
                    f"INSERT INTO Платёж (ID_платежа, ID_сессии, ID_тарифа, Сумма, Дата_платежа) "
                    f"VALUES ({global_payment_id}, {global_session_id}, {t_id}, {cost}, '{time_out_str}');"
                )
                
                global_session_id += 1
                global_payment_id += 1
                
                # Передвигаем время вперед: время выезда + случайный простой места (от 10 мин до 5 часов)
                current_time = time_out + timedelta(minutes=random.randint(10, 300))
        else:
            current_time += timedelta(hours=random.randint(4, 12))

sql_queries.extend(wrap_identity_insert("Парковочная_сессия", session_queries))
sql_queries.append("\n")
sql_queries.extend(wrap_identity_insert("Платёж", payment_queries))

try:
    with open(OUTPUT_FILENAME, 'w', encoding='utf-8') as f:
        f.write("-- Generated by Python script\n")
        f.write("USE Parkspot \nGO\n\n")
        
        for query in sql_queries:
            f.write(query + '\n')
    
    print(f"Готово! Сгенерировано {global_session_id-1} сессий.")
    print(f"Файл: {OUTPUT_FILENAME}")

except Exception as e:
    print(f"Ошибка записи: {e}")