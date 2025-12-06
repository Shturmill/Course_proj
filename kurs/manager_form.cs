using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kurs
{
    public partial class manager_form : Form
    {

        private int _currentEmployeeId;

        private readonly string _connectionString = @"Server=SHTORKA\MSSQLSERVER01;Database=Park_spot;Integrated Security=True;";

        private int _selectedTariffId = -1;



        public manager_form(string fioString, int ID)
        {
            InitializeComponent();

            NameEmpl.Text = "Менеджер: " + fioString;

            _currentEmployeeId = ID;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT 
                        s.ФИО, 
                        s.Телефон, 
                        s.Электронная_почта, 
                        u.Логин 
                    FROM Сотрудник s
                    JOIN УчетныеЗаписи u ON s.ID_сотрудника = u.ID_сотрудника
                    WHERE s.ID_сотрудника = @ID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", _currentEmployeeId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxLogin.Text = reader["Логин"].ToString();
                            textBoxPhoneProfile.Text = reader["Телефон"].ToString();
                            textBoxMailProfile.Text = reader["Электронная_почта"].ToString();

                            string fullFio = reader["ФИО"].ToString();
                            string[] fioParts = fullFio.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (fioParts.Length > 0) textBoxFamProfile.Text = fioParts[0];
                            if (fioParts.Length > 1) textBoxNameProfile.Text = fioParts[1];
                            if (fioParts.Length > 2) textBoxSurProfile.Text = fioParts[2];
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных профиля: " + ex.Message);
                }
            }
        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {
            string fam = textBoxFamProfile.Text.Trim();
            string name = textBoxNameProfile.Text.Trim();
            string sur = textBoxSurProfile.Text.Trim();

            string fullFio = $"{fam} {name} {sur}".Trim();

            string phone = textBoxPhone.Text.Trim();
            string mail = textBoxMail.Text.Trim();

            if (string.IsNullOrEmpty(fullFio) || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("ФИО и Телефон обязательны для заполнения!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_UpdateEmployeeProfile", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID_сотрудника", _currentEmployeeId);
                    command.Parameters.AddWithValue("@ФИО", fullFio);
                    command.Parameters.AddWithValue("@Телефон", phone);

                    command.Parameters.AddWithValue("@Электронная_почта", mail);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные профиля успешно обновлены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении профиля: " + ex.Message);
                }
            }
        }

        private void buttonPassword_Click(object sender, EventArgs e)
        {
            string passwordNew = textBoxPassword0.Text;
            string passwordConfirm = textBoxPassword1.Text;
            string login = textBoxLogin.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(passwordNew))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми.");
                return;
            }

            if (passwordNew != passwordConfirm)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_UpdateEmployeeCredentials", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID_сотрудника", _currentEmployeeId);
                    command.Parameters.AddWithValue("@НовыйЛогин", login);
                    command.Parameters.AddWithValue("@НовыйПароль", passwordNew);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        MessageBox.Show("Логин и пароль успешно изменены!");
                    else
                        MessageBox.Show("Не удалось найти запись сотрудника.");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ошибка базы данных: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void manager_form_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.Платёж". При необходимости она может быть перемещена или удалена.
            this.платёжTableAdapter.Fill(this.park_spotDataSet.Платёж);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.park_spotDataSet.Сотрудник);
            // данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.Тариф"
            this.тарифTableAdapter.Fill(this.park_spotDataSet.Тариф);

        }

        private void тарифDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void сотрудникDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Name_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Log_in Log_in = new Log_in();
            Log_in.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Trim();
            string fam = textBoxFam.Text.Trim();
            string sur = textBoxSurname.Text.Trim();

            string phone = textBoxPhone.Text.Trim();
            string mail = textBoxMail.Text.Trim();
            string position = emplVal.Text.Trim();

            if (string.IsNullOrEmpty(fam) || string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(mail) ||
                string.IsNullOrEmpty(position))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля:\nФамилия, Имя, Телефон, Почта, Должность.");
                return;
            }

            string fullFio = $"{fam} {name} {sur}".Trim();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_AddEmployee", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ФИО", fullFio);
                    command.Parameters.AddWithValue("@Должность", position);
                    command.Parameters.AddWithValue("@Телефон", phone);
                    command.Parameters.AddWithValue("@Электронная_почта", mail);

                    command.ExecuteNonQuery();

                    MessageBox.Show($"Сотрудник успешно добавлен!\n\nЛогин для входа: {mail}\nПароль по умолчанию: 12345");

                    textBoxName.Clear(); textBoxFam.Clear(); textBoxSurname.Clear();
                    textBoxPhone.Clear(); textBoxMail.Clear();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ошибка базы данных: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string phone = textBoxPhone.Text.Trim();

            string inputFam = textBoxFam.Text.Trim();
            string inputName = textBoxName.Text.Trim();
            string inputSur = textBoxSurname.Text.Trim();

            string inputFullFio = $"{inputFam} {inputName} {inputSur}".Trim();

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Введите номер телефона сотрудника для поиска.");
                return;
            }

            if (string.IsNullOrEmpty(inputFullFio))
            {
                MessageBox.Show("Для безопасности удаления необходимо ввести ФИО сотрудника.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string checkQuery = "SELECT ID_сотрудника, ФИО, Должность FROM Сотрудник WHERE Телефон = @Phone";

                    string dbFio = "";
                    string dbPosition = "";
                    int dbId = 0; 
                    bool employeeFound = false;

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Phone", phone);

                        using (SqlDataReader reader = checkCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dbFio = reader["ФИО"].ToString();
                                dbPosition = reader["Должность"].ToString();
                                dbId = Convert.ToInt32(reader["ID_сотрудника"]);
                                employeeFound = true;
                            }
                        }
                    }

                    if (!employeeFound)
                    {
                        MessageBox.Show("Сотрудник с таким номером телефона не найден в базе данных.");
                        return;
                    }

                    if (dbId == _currentEmployeeId)
                    {
                        MessageBox.Show("Вы не можете удалить свою собственную учетную запись!",
                                        "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!string.Equals(inputFullFio, dbFio, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            $"Ошибка проверки данных!\n\n" +
                            $"Вы ввели: {inputFullFio}\n" +
                            $"В базе данных (по этому телефону): {dbFio}\n\n" +
                            $"Удаление отменено. Данные не совпадают.",
                            "Ошибка безопасности", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DialogResult dialogResult = MessageBox.Show(
                        $"Вы действительно хотите удалить следующего сотрудника?\n\n" +
                        $"ФИО: {dbFio}\n" +
                        $"Должность: {dbPosition}\n" +
                        $"Телефон: {phone}\n\n" +
                        $"Это действие нельзя отменить.",
                        "Подтверждение удаления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                    {
                        SqlCommand deleteCommand = new SqlCommand("sp_DeleteEmployee", connection);
                        deleteCommand.CommandType = CommandType.StoredProcedure;
                        deleteCommand.Parameters.AddWithValue("@Телефон", phone);

                        deleteCommand.ExecuteNonQuery();

                        MessageBox.Show("Сотрудник успешно удален.");

                        textBoxName.Clear(); textBoxFam.Clear(); textBoxSurname.Clear();
                        textBoxPhone.Clear(); textBoxMail.Clear();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ошибка базы данных: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBoxNameTarif.Text.Trim();

            if (!int.TryParse(textBoxHours.Text, out int hours) || hours <= 0)
            {
                MessageBox.Show("Введите корректное количество часов (целое число > 0).");
                return;
            }

            if (!decimal.TryParse(textBoxCash.Text, out decimal cost) || cost <= 0)
            {
                MessageBox.Show("Введите корректную стоимость (число > 0).");
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название тарифа.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_AddTariff", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Наименование", name);
                    command.Parameters.AddWithValue("@Часы", hours);
                    command.Parameters.AddWithValue("@Стоимость", cost);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Тариф успешно добавлен!");
                    ClearFields();

                    this.тарифTableAdapter.Fill(this.park_spotDataSet.Тариф);
                    тарифBindingSource.Sort = "Продолжительность_часов ASC";
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ошибка БД: " + ex.Message);
                }
            }
        }


        private void ClearFields()
        {
            textBoxNameTarif.Clear();
            textBoxHours.Clear();
            textBoxCash.Clear();
        }

        private void buttonRedact_Click(object sender, EventArgs e)
        {
            string name = textBoxNameTarif.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название тарифа, который нужно изменить.");
                return;
            }

            if (!int.TryParse(textBoxHours.Text, out int hours) || hours <= 0)
            {
                MessageBox.Show("Некорректные часы.");
                return;
            }

            if (!decimal.TryParse(textBoxCash.Text, out decimal cost) || cost <= 0)
            {
                MessageBox.Show("Некорректная стоимость.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_UpdateTariff", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Наименование", name); // теперь поиск по имени!
                    command.Parameters.AddWithValue("@Часы", hours);
                    command.Parameters.AddWithValue("@Стоимость", cost);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Тариф обновлен!");
                    ClearFields();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ошибка БД: " + ex.Message);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string name = textBoxNameTarif.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название тарифа для удаления.");
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить тариф '{name}'?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_DeleteTariff", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Наименование", name);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Тариф удален.");
                        ClearFields();


                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 547 || ex.Message.Contains("FK"))
                            MessageBox.Show("Невозможно удалить тариф: он используется в платежах.");
                        else
                            MessageBox.Show("Ошибка БД: " + ex.Message);
                    }
                }
            }
        }


        private void UpdateProfile_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
            SELECT 
                s.ФИО, 
                s.Телефон, 
                s.Электронная_почта, 
                u.Логин 
            FROM Сотрудник s
            JOIN УчетныеЗаписи u ON s.ID_сотрудника = u.ID_сотрудника
            WHERE s.ID_сотрудника = @ID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", _currentEmployeeId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxLogin.Text = reader["Логин"].ToString();
                            textBoxPhoneProfile.Text = reader["Телефон"].ToString();
                            textBoxMailProfile.Text = reader["Электронная_почта"].ToString();

                            string fullFio = reader["ФИО"].ToString();
                            string[] fioParts = fullFio.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            textBoxFamProfile.Text = fioParts.Length > 0 ? fioParts[0] : "";
                            textBoxNameProfile.Text = fioParts.Length > 1 ? fioParts[1] : "";
                            textBoxSurProfile.Text = fioParts.Length > 2 ? fioParts[2] : "";
                        }
                        else
                        {
                            MessageBox.Show("Данные сотрудника не найдены.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении данных профиля: " + ex.Message);
                }
            }
        }

        private void UpdatePersonal_Click(object sender, EventArgs e)
        {
            try
            {
                this.сотрудникTableAdapter.Fill(this.park_spotDataSet.Сотрудник);
                сотрудникBindingSource.Sort = "Должность ASC";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении списка сотрудников: " + ex.Message);
            }
        }

        private void Updateclient_Click(object sender, EventArgs e)
        {
            try
            {
                this.тарифTableAdapter.Fill(this.park_spotDataSet.Тариф);
                тарифBindingSource.Sort = "Продолжительность_часов ASC";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении тарифов: " + ex.Message);
            }
        }

    }
}
