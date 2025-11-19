using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kurs
{
    public partial class manager_form : Form
    {
        public manager_form()
        {
            InitializeComponent();
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
    }
}
