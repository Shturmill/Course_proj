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

            // Загрузка данных из БД
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

        }

        private void Update_Click(object sender, EventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {

        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {

            string fam = textBoxFam.Text.Trim();    
            string name = textBoxName.Text.Trim();  
            string sur = textBoxSurname.Text.Trim(); 

            // Собираем полное ФИО для базы данных
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

            // Валидация
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

        private void Updateclient_Click(object sender, EventArgs e)
        {

        }


        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
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
    }
}
