using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kurs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            string login = textBox1.Text;
            string password = textBox2.Text;

            //TO DO: Сделать через БД с проверкой на совпадение и т.д.
            if (login == "administrator" && password == "admin") {
                Form Form2 = new Form2();
                Form2.Show();
            }

            else if (login == "operator" && password == "operator"){
                Form Form3 = new Form3();
                Form3.Show();
            }

            else{
                MessageBox.Show(
                    "Неверный логин или пароль!",
                    "Ошибка входа",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled= true;
        }

        private void label1_Click(object sender, EventArgs e)
        {


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
