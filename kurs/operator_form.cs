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
    public partial class operator_form : Form
    {
        public operator_form()
        {
            InitializeComponent();
        }

        private void operator_form_Load(object sender, EventArgs e)
        {
            
            this.v_ActiveSessionsTableAdapter.Fill(this.park_spotDataSet.v_ActiveSessions);
            this.v_ParkingLotOverviewTableAdapter.Fill(this.park_spotDataSet.v_ParkingLotOverview);            
            this.v_PaymentHistoryTableAdapter.Fill(this.park_spotDataSet.v_PaymentHistory);            
            this.парковочное_местоTableAdapter.Fill(this.park_spotDataSet.Парковочное_место);            
            this.тСTableAdapter.Fill(this.park_spotDataSet.ТС);            
            this.клиентTableAdapter.Fill(this.park_spotDataSet.Клиент);
            this.парковочная_сессияTableAdapter.Fill(this.park_spotDataSet.Парковочная_сессия);

        }

        private void парковочная_сессияBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.парковочная_сессияBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.park_spotDataSet);

        }

        private void парковочная_сессияDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
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

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.тСTableAdapter.FillBy(this.park_spotDataSet.ТС);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void v_ActiveSessionsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void Update_Click(object sender, EventArgs e)
        {

        }
    }
}
