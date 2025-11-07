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
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.v_ParkingLotOverview". При необходимости она может быть перемещена или удалена.
            this.v_ParkingLotOverviewTableAdapter.Fill(this.park_spotDataSet.v_ParkingLotOverview);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.v_PaymentHistory". При необходимости она может быть перемещена или удалена.
            this.v_PaymentHistoryTableAdapter.Fill(this.park_spotDataSet.v_PaymentHistory);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.Парковочное_место". При необходимости она может быть перемещена или удалена.
            this.парковочное_местоTableAdapter.Fill(this.park_spotDataSet.Парковочное_место);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.ТС". При необходимости она может быть перемещена или удалена.
            this.тСTableAdapter.Fill(this.park_spotDataSet.ТС);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.Клиент". При необходимости она может быть перемещена или удалена.
            this.клиентTableAdapter.Fill(this.park_spotDataSet.Клиент);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "park_spotDataSet.Парковочная_сессия". При необходимости она может быть перемещена или удалена.
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
    }
}
