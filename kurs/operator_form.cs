using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting.Contexts;

namespace kurs
{

    public partial class operator_form : Form
    {

        private int _currentEmployeeId;

        private readonly string _connectionString = @"Server=SHTORKA\MSSQLSERVER01;Database=Park_spot;Integrated Security=True;";

        public operator_form(string fioString, int ID)
        {
            InitializeComponent();

            _currentEmployeeId = ID;
            NameEmpl.Text = "Оператор: " + fioString;

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
                            textBoxPhone.Text = reader["Телефон"].ToString();
                            textBoxMail.Text = reader["Электронная_почта"].ToString();

                            string fullFio = reader["ФИО"].ToString();
                            string[] fioParts = fullFio.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (fioParts.Length > 0) textBoxFam.Text = fioParts[0];
                            if (fioParts.Length > 1) textBoxName.Text = fioParts[1];
                            if (fioParts.Length > 2) textBoxSurname.Text = fioParts[2];
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных профиля: " + ex.Message);
                }
            }
        }


        private void ExitButton_Click(object sender, EventArgs e)
        {
            Log_in Log_in = new Log_in();
            Log_in.Show();
            this.Hide();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string carPlate = string.Empty;

            if (ForAdd.SelectedItem != null)
            {
                if (ForAdd.SelectedValue != null)
                {
                    carPlate = ForAdd.SelectedValue.ToString().Trim();
                }
                else
                {
                    carPlate = ForAdd.SelectedItem.ToString().Trim();
                }
            }

            if (string.IsNullOrEmpty(carPlate))
            {
                carPlate = ForAdd.Text.Trim();
            }


            if (string.IsNullOrEmpty(carPlate))
            {
                MessageBox.Show("Выберите или введите госномер автомобиля для въезда!");
                return;
            }

            int? spotNumber = null;


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_CreateParkingSession", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Гос_номер", carPlate);
                    command.Parameters.AddWithValue("@ID_сотрудника", _currentEmployeeId);

                    if (spotNumber.HasValue)
                        command.Parameters.AddWithValue("@Номер_места", spotNumber.Value);
                    else
                        command.Parameters.AddWithValue("@Номер_места", DBNull.Value);

                    SqlParameter resultParam = new SqlParameter("@Результат", SqlDbType.NVarChar, 500);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();

                    string resultMessage = resultParam.Value.ToString();

                    if (resultMessage.StartsWith("Ошибка"))
                        MessageBox.Show(resultMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show(resultMessage, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!resultMessage.StartsWith("Ошибка"))
                    {
                        ForAdd.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Системная ошибка: " + ex.Message);
                }
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            string carPlate = EndForSession.Text.Trim();

            if (string.IsNullOrEmpty(carPlate))
            {
                MessageBox.Show("Выберите автомобиль для выезда (завершения сессии)!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_CloseParkingSession", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Гос_номер", carPlate);

                    SqlParameter resultParam = new SqlParameter("@Результат", SqlDbType.VarChar, 500);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();

                    string resultMessage = resultParam.Value.ToString();

                    if (resultMessage.StartsWith("Ошибка"))
                    {
                        MessageBox.Show(resultMessage, "Ошибка выезда", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(resultMessage, "Успешный выезд", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        EndForSession.Text = "";
                        LoadActiveSessions();
                        LoadAvailableCars();

                        RefreshActiveSessionsComboBox();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Системная ошибка: " + ex.Message);
                }
            }
        }

        private void LoadAvailableCars()
        {
            ForAdd.Items.Clear(); 

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT Гос_номер FROM ТС 
                             WHERE Гос_номер NOT IN (
                                 SELECT Гос_номер 
                                 FROM Парковочная_сессия 
                                 WHERE Время_выезда IS NULL
                             )";

                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ForAdd.Items.Add(reader["Гос_номер"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке списка доступных машин: " + ex.Message);
                }
            }
        }

        private void LoadActiveSessions()
        {
            EndForSession.Items.Clear();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT DISTINCT Гос_номер 
                             FROM Парковочная_сессия 
                             WHERE Время_выезда IS NULL";

                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EndForSession.Items.Add(reader["Гос_номер"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке списка припаркованных машин: " + ex.Message);
                }
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            const string activeSessionsViewName = "v_ActiveSessions";

            DataTable activeSessionsData = GetViewData(activeSessionsViewName);

            if (activeSessionsData != null)
            {
                v_ActiveSessionsBindingSource2.DataSource = activeSessionsData;

                MessageBox.Show($"Таблица активных сессий ({activeSessionsViewName}) обновлена. Найдено записей: {activeSessionsData.Rows.Count}",
                                "Обновление завершено",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            RefreshActiveSessionsComboBox();
        }

        private DataTable GetViewData(string viewName)
        {
            DataTable dt = new DataTable();
            string query = $"SELECT * FROM {viewName}";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении данных из представления: " + ex.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {

            string fam = textBoxFam.Text.Trim();    
            string name = textBoxName.Text.Trim();  
            string sur = textBoxSurname.Text.Trim(); 

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

        private void operator_form_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet31.v_ParkingLotOverview". При необходимости она может быть перемещена или удалена.
            this.v_ParkingLotOverviewTableAdapter.Fill(this.park_spotDataSet31.v_ParkingLotOverview);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet31.v_PaymentHistory". При необходимости она может быть перемещена или удалена.
            this.v_PaymentHistoryTableAdapter.Fill(this.park_spotDataSet31.v_PaymentHistory);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet31.vw_PaymentReport". При необходимости она может быть перемещена или удалена.
            this.vw_PaymentReportTableAdapter.Fill(this.park_spotDataSet31.vw_PaymentReport);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet31.vw_VehicleRegistry". При необходимости она может быть перемещена или удалена.
            this.vw_VehicleRegistryTableAdapter2.Fill(this.park_spotDataSet31.vw_VehicleRegistry);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet4.Парковочная_сессия". При необходимости она может быть перемещена или удалена.
            this.парковочная_сессияTableAdapter.Fill(this.park_spotDataSet4.Парковочная_сессия);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.v_ActiveSessions". При необходимости она может быть перемещена или удалена.
            this.v_ActiveSessionsTableAdapter.Fill(this.park_spotDataSet.v_ActiveSessions);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.park_spotDataSet.Сотрудник);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet2.vw_SessionDetails". При необходимости она может быть перемещена или удалена.      
            this.парковочное_местоTableAdapter.Fill(this.park_spotDataSet.Парковочное_место);            
            this.тСTableAdapter.Fill(this.park_spotDataSet.ТС);            
            this.клиентTableAdapter.Fill(this.park_spotDataSet.Клиент);
            this.парковочная_сессияTableAdapter.Fill(this.park_spotDataSet.Парковочная_сессия);

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }




        private void label12_Click(object sender, EventArgs e)
        {

        }



        private void textBoxPassword1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ForAdd_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void v_PaymentHistoryDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e) // Удаление ТС
        {
            string govNumber = GosText.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(govNumber))
            {
                MessageBox.Show("Введите госномер автомобиля для удаления.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (false)
            {
                MessageBox.Show($"Невозможно удалить ТС {govNumber}. Сначала необходимо закрыть активную парковочную сессию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Вы уверены, что хотите удалить ТС с госномером {govNumber}?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();
                        string deleteQuery = "DELETE FROM ТС WHERE Гос_номер = @GovNumber";

                        SqlCommand command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@GovNumber", govNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"ТС {govNumber} успешно удалено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearVehicleAndClientFields();
                        }
                        else
                        {
                            MessageBox.Show($"ТС с госномером {govNumber} не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Ошибка базы данных: " + ex.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) 
        {
            string ownerFio = textBox2.Text.Trim();
            string ownerPhone = textBox1.Text.Trim();
            string ownerMail = textBox3.Text.Trim();

            string govNumber = GosText.Text.Trim().ToUpper(); 
            string vehicleType = VehicleType.SelectedItem?.ToString() ?? ""; 
            string vehicleMark = MarkVeh.Text.Trim();
            string vehicleModel = ModelVeh.Text.Trim();

            if (string.IsNullOrEmpty(ownerFio) || string.IsNullOrEmpty(ownerPhone) || string.IsNullOrEmpty(govNumber))
            {
                MessageBox.Show("Пожалуйста, заполните ФИО владельца, телефон и госномер автомобиля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(vehicleType))
            {
                MessageBox.Show("Пожалуйста, выберите тип транспортного средства.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_AddClientAndVehicle", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ФИО_владельца", ownerFio);
                    command.Parameters.AddWithValue("@Телефон", ownerPhone);
                    command.Parameters.AddWithValue("@Почта_владельца", string.IsNullOrEmpty(ownerMail) ? (object)DBNull.Value : ownerMail); 

                    command.Parameters.AddWithValue("@Гос_номер", govNumber);
                    command.Parameters.AddWithValue("@Тип_ТС", vehicleType);
                    command.Parameters.AddWithValue("@Марка_ТС", vehicleMark);
                    command.Parameters.AddWithValue("@Модель_ТС", vehicleModel);

                    SqlParameter resultParam = new SqlParameter("@Результат", SqlDbType.NVarChar, 500);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();

                    string resultMessage = resultParam.Value.ToString();

                    if (resultMessage.StartsWith("Ошибка"))
                    {
                        MessageBox.Show(resultMessage, "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (resultMessage.StartsWith("Критическая ошибка"))
                    {
                        MessageBox.Show("Ошибка базы данных: " + resultMessage, "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(resultMessage, "Успешно добавлено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearVehicleAndClientFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Системная ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool overviewSuccess = false;
            bool paymentSuccess = false;

            const string parkingOverviewViewName = "v_ParkingLotOverview"; 
            DataTable parkingData = GetViewData(parkingOverviewViewName);

            if (parkingData != null)
            {
                v_ParkingLotOverviewBindingSource1.DataSource = parkingData;
                overviewSuccess = true;
            }

            const string paymentHistoryViewName = "v_PaymentHistory";
            DataTable paymentData = GetViewData(paymentHistoryViewName);

            if (paymentData != null)
            {
                v_PaymentHistoryBindingSource1.DataSource = paymentData;
                paymentSuccess = true;
            }

            if (overviewSuccess && paymentSuccess)
            {
                MessageBox.Show("Обзор парковки и история платежей успешно обновлены.", "Обновление завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (overviewSuccess || paymentSuccess)
            {
                MessageBox.Show("Обновление завершено, но возникли проблемы с одним из представлений. Проверьте ошибки.",
                                "Частичное обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Не удалось обновить ни одну из таблиц. Проверьте подключение к базе данных.",
                                "Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearVehicleAndClientFields()
        {
            textBox2.Clear();
            textBox1.Clear();
            textBox3.Clear();
            GosText.Clear();
            VehicleType.SelectedIndex = -1; 
            MarkVeh.Clear();
            ModelVeh.Clear();
        }

        private void Updateclient_Click(object sender, EventArgs e)
        {
            bool clientUpdateSuccess = false;
            bool vehicleUpdateSuccess = false;

            const string clientDataSourceName = "Клиент"; 
            DataTable clientData = GetViewData(clientDataSourceName);

            if (clientData != null)
            {
                клиентBindingSource.DataSource = clientData;
                clientUpdateSuccess = true;
            }

            const string vehicleDataSourceName = "vw_VehicleRegistry";
            DataTable vehicleData = GetViewData(vehicleDataSourceName);

            if (vehicleData != null)
            {
                vw_VehicleRegistryBindingSource1.DataSource = vehicleData;
                vehicleUpdateSuccess = true;
            }

            if (clientUpdateSuccess && vehicleUpdateSuccess)
            {
                MessageBox.Show("Данные клиентов и реестра ТС успешно обновлены.", "Обновление завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (clientUpdateSuccess || vehicleUpdateSuccess)
            {
                MessageBox.Show("Обновление завершено, но возникли проблемы с одним из источников данных. Проверьте ошибки.",
                                "Частичное обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Не удалось обновить ни одну из таблиц. Проверьте подключение к базе данных.",
                                "Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
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
                            textBoxPhone.Text = reader["Телефон"].ToString();
                            textBoxMail.Text = reader["Электронная_почта"].ToString();

                            string fullFio = reader["ФИО"].ToString();
                            string[] fioParts = fullFio.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            textBoxFam.Text = fioParts.Length > 0 ? fioParts[0] : "";
                            textBoxName.Text = fioParts.Length > 1 ? fioParts[1] : "";
                            textBoxSurname.Text = fioParts.Length > 2 ? fioParts[2] : "";
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

        private void RefreshActiveSessionsComboBox()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Гос_номер FROM Парковочная_сессия WHERE Время_выезда IS NULL";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    EndForSession.DataSource = null;

                    EndForSession.DataSource = dt;
                    EndForSession.DisplayMember = "Гос_номер";
                    EndForSession.ValueMember = "Гос_номер"; 

                    EndForSession.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления списка машин: " + ex.Message);
                }
            }
        }
    }
}
