using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kurs
{
    //модель данных сотрудника
    public class СотрудникModel
    {
        public int ID { get; set; }
        public string ФИО { get; set; }
        public string Должность { get; set; }
        public string Email { get; set; }
        public string Телефон { get; set; }
    }


    //класс базы данных
    public class DatabaseService
    {
        private readonly string _connectionString = @"Server=SHTORKA\MSSQLSERVER01;Database=Park_spot;Integrated Security=True;";

        public async Task<СотрудникModel> LoginUserAsync(string login, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_LoginUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Логин", login);
                    command.Parameters.AddWithValue("@Пароль", password);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Логин и пароль верны
                            return new СотрудникModel
                            {
                                ID = (int)reader["ID_сотрудника"],
                                ФИО = reader["ФИО"].ToString(),
                                Должность = reader["Должность"].ToString(),
                                Email = reader["Электронная_почта"]?.ToString(), // ?. для NULL полей
                                Телефон = reader["Телефон"].ToString()
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        /// Регистрирует нового пользователя с логином и паролем.
        public async Task<bool> RegisterUserAsync(int employeeId, string login, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_RegisterUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID_сотрудника", employeeId);
                    command.Parameters.AddWithValue("@Логин", login);
                    command.Parameters.AddWithValue("@Пароль", password);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return true;
                    }
                    catch (SqlException)
                    {
                        return false;
                    }
                }
            }
        }
    }



    public partial class Log_in : Form
    {
        // Создаем экземпляр сервиса
        private readonly DatabaseService _databaseService;

        public Log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // Инициализируем сервис
            _databaseService = new DatabaseService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxLogin.MaxLength = 31;
            textBoxPassword.MaxLength = 31;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                button1.Enabled = false;

                СотрудникModel user = await _databaseService.LoginUserAsync(login, password);

                if (user != null)
                {
                    MessageBox.Show($"Добро пожаловать, {user.ФИО}!", "Успешный вход", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    switch (user.Должность)
                    {
                        case "Оператор парковки":
                           
                            operator_form operatorForm = new operator_form(user.ФИО, user.ID);
                            operatorForm.Show();
                            break;

                        case "Менеджер":
                            manager_form manager_form = new manager_form(user.ФИО, user.ID); 
                            manager_form.Show();
                            break;

                        default:
                            MessageBox.Show("Ваша должность не позволяет войти.", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                    }

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {sqlEx.Message}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}