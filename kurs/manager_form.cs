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

        private void manager_form_Load(object sender, EventArgs e)
        {
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

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void Name_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Log_in Log_in = new Log_in();
            Log_in.Show();
            this.Hide();
        }
    }
}
