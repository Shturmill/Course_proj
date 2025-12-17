namespace kurs
{
    partial class manager_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.park_spotDataSet = new kurs.Park_spotDataSet();
            this.parkspotDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.тарифBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.тарифTableAdapter = new kurs.Park_spotDataSetTableAdapters.ТарифTableAdapter();
            this.tableAdapterManager = new kurs.Park_spotDataSetTableAdapters.TableAdapterManager();
            this.сотрудникTableAdapter = new kurs.Park_spotDataSetTableAdapters.СотрудникTableAdapter();
            this.тарифDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonRedact = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Updateclient = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCash = new System.Windows.Forms.TextBox();
            this.textBoxHours = new System.Windows.Forms.TextBox();
            this.textBoxNameTarif = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.emplVal = new System.Windows.Forms.ComboBox();
            this.deleteEmpl = new System.Windows.Forms.Button();
            this.addEmpl = new System.Windows.Forms.Button();
            this.textBoxMail = new System.Windows.Forms.TextBox();
            this.textBoxPhone = new System.Windows.Forms.TextBox();
            this.textBoxSurname = new System.Windows.Forms.TextBox();
            this.textBoxFam = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.MailProfile = new System.Windows.Forms.Label();
            this.PhoneProfile = new System.Windows.Forms.Label();
            this.SurnameProfile = new System.Windows.Forms.Label();
            this.FamProfile = new System.Windows.Forms.Label();
            this.NameProfile = new System.Windows.Forms.Label();
            this.UpdatePersonal = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.сотрудникDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сотрудникBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.UpdateProfile = new System.Windows.Forms.Button();
            this.buttonPassword = new System.Windows.Forms.Button();
            this.textBoxPassword1 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.textBoxPassword0 = new System.Windows.Forms.TextBox();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonProfile = new System.Windows.Forms.Button();
            this.textBoxMailProfile = new System.Windows.Forms.TextBox();
            this.textBoxPhoneProfile = new System.Windows.Forms.TextBox();
            this.textBoxSurProfile = new System.Windows.Forms.TextBox();
            this.textBoxFamProfile = new System.Windows.Forms.TextBox();
            this.textBoxNameProfile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.NameEmpl = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fKПлатёжIDтариф4A23E96ABindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.платёжTableAdapter = new kurs.Park_spotDataSetTableAdapters.ПлатёжTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.park_spotDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parkspotDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.тарифBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.тарифDataGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.сотрудникDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.сотрудникBindingSource)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKПлатёжIDтариф4A23E96ABindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // park_spotDataSet
            // 
            this.park_spotDataSet.DataSetName = "Park_spotDataSet";
            this.park_spotDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // parkspotDataSetBindingSource
            // 
            this.parkspotDataSetBindingSource.DataSource = this.park_spotDataSet;
            this.parkspotDataSetBindingSource.Position = 0;
            // 
            // тарифBindingSource
            // 
            this.тарифBindingSource.DataMember = "Тариф";
            this.тарифBindingSource.DataSource = this.park_spotDataSet;
            this.тарифBindingSource.Sort = "Продолжительность_часов ASC";
            // 
            // тарифTableAdapter
            // 
            this.тарифTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.UpdateOrder = kurs.Park_spotDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.КлиентTableAdapter = null;
            this.tableAdapterManager.Парковочная_сессияTableAdapter = null;
            this.tableAdapterManager.Парковочное_местоTableAdapter = null;
            this.tableAdapterManager.ПлатёжTableAdapter = null;
            this.tableAdapterManager.СотрудникTableAdapter = this.сотрудникTableAdapter;
            this.tableAdapterManager.ТарифTableAdapter = this.тарифTableAdapter;
            this.tableAdapterManager.ТСTableAdapter = null;
            this.tableAdapterManager.УчетныеЗаписиTableAdapter = null;
            // 
            // сотрудникTableAdapter
            // 
            this.сотрудникTableAdapter.ClearBeforeFill = true;
            // 
            // тарифDataGridView
            // 
            this.тарифDataGridView.AllowUserToAddRows = false;
            this.тарифDataGridView.AllowUserToDeleteRows = false;
            this.тарифDataGridView.AutoGenerateColumns = false;
            this.тарифDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.тарифDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.тарифDataGridView.DataSource = this.тарифBindingSource;
            this.тарифDataGridView.Location = new System.Drawing.Point(506, 8);
            this.тарифDataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.тарифDataGridView.Name = "тарифDataGridView";
            this.тарифDataGridView.ReadOnly = true;
            this.тарифDataGridView.Size = new System.Drawing.Size(499, 313);
            this.тарифDataGridView.TabIndex = 0;
            this.тарифDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.тарифDataGridView_CellContentClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Наименование";
            this.dataGridViewTextBoxColumn2.HeaderText = "Наименование";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Продолжительность_часов";
            this.dataGridViewTextBoxColumn3.HeaderText = "Продолжительность часов";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 180;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Стоимость";
            this.dataGridViewTextBoxColumn4.HeaderText = "Стоимость";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 120;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(36, 91);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1218, 400);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonDelete);
            this.tabPage1.Controls.Add(this.buttonRedact);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.Updateclient);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxCash);
            this.tabPage1.Controls.Add(this.textBoxHours);
            this.tabPage1.Controls.Add(this.textBoxNameTarif);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.тарифDataGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1210, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Тарифы";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(282, 257);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(148, 32);
            this.buttonDelete.TabIndex = 53;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonRedact
            // 
            this.buttonRedact.Location = new System.Drawing.Point(145, 257);
            this.buttonRedact.Name = "buttonRedact";
            this.buttonRedact.Size = new System.Drawing.Size(131, 32);
            this.buttonRedact.TabIndex = 52;
            this.buttonRedact.Text = "Изменить";
            this.buttonRedact.UseVisualStyleBackColor = true;
            this.buttonRedact.Click += new System.EventHandler(this.buttonRedact_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 257);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 32);
            this.button2.TabIndex = 51;
            this.button2.Text = "Добавить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Updateclient
            // 
            this.Updateclient.BackgroundImage = global::kurs.Properties.Resources.update_icon;
            this.Updateclient.Location = new System.Drawing.Point(448, 281);
            this.Updateclient.Name = "Updateclient";
            this.Updateclient.Size = new System.Drawing.Size(40, 40);
            this.Updateclient.TabIndex = 50;
            this.Updateclient.UseVisualStyleBackColor = true;
            this.Updateclient.Click += new System.EventHandler(this.Updateclient_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 185);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Стоимость";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 140);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Количество часов для тарифа";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 96);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "Имя тарифа";
            // 
            // textBoxCash
            // 
            this.textBoxCash.Location = new System.Drawing.Point(282, 182);
            this.textBoxCash.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCash.Name = "textBoxCash";
            this.textBoxCash.Size = new System.Drawing.Size(148, 26);
            this.textBoxCash.TabIndex = 4;
            // 
            // textBoxHours
            // 
            this.textBoxHours.Location = new System.Drawing.Point(282, 135);
            this.textBoxHours.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxHours.Name = "textBoxHours";
            this.textBoxHours.Size = new System.Drawing.Size(148, 26);
            this.textBoxHours.TabIndex = 3;
            // 
            // textBoxNameTarif
            // 
            this.textBoxNameTarif.Location = new System.Drawing.Point(282, 85);
            this.textBoxNameTarif.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxNameTarif.Name = "textBoxNameTarif";
            this.textBoxNameTarif.Size = new System.Drawing.Size(148, 26);
            this.textBoxNameTarif.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(34, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Управление тарифами ";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.emplVal);
            this.tabPage2.Controls.Add(this.deleteEmpl);
            this.tabPage2.Controls.Add(this.addEmpl);
            this.tabPage2.Controls.Add(this.textBoxMail);
            this.tabPage2.Controls.Add(this.textBoxPhone);
            this.tabPage2.Controls.Add(this.textBoxSurname);
            this.tabPage2.Controls.Add(this.textBoxFam);
            this.tabPage2.Controls.Add(this.textBoxName);
            this.tabPage2.Controls.Add(this.MailProfile);
            this.tabPage2.Controls.Add(this.PhoneProfile);
            this.tabPage2.Controls.Add(this.SurnameProfile);
            this.tabPage2.Controls.Add(this.FamProfile);
            this.tabPage2.Controls.Add(this.NameProfile);
            this.tabPage2.Controls.Add(this.UpdatePersonal);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.сотрудникDataGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1210, 368);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Персонал";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(56, 199);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 19);
            this.label12.TabIndex = 64;
            this.label12.Text = "Должность";
            // 
            // emplVal
            // 
            this.emplVal.FormattingEnabled = true;
            this.emplVal.Items.AddRange(new object[] {
            "Оператор парковки",
            "Менеджер"});
            this.emplVal.Location = new System.Drawing.Point(147, 196);
            this.emplVal.Name = "emplVal";
            this.emplVal.Size = new System.Drawing.Size(227, 27);
            this.emplVal.TabIndex = 63;
            // 
            // deleteEmpl
            // 
            this.deleteEmpl.Location = new System.Drawing.Point(224, 312);
            this.deleteEmpl.Name = "deleteEmpl";
            this.deleteEmpl.Size = new System.Drawing.Size(150, 28);
            this.deleteEmpl.TabIndex = 62;
            this.deleteEmpl.Text = "Удалить";
            this.deleteEmpl.UseVisualStyleBackColor = true;
            this.deleteEmpl.Click += new System.EventHandler(this.button4_Click);
            // 
            // addEmpl
            // 
            this.addEmpl.Location = new System.Drawing.Point(62, 312);
            this.addEmpl.Name = "addEmpl";
            this.addEmpl.Size = new System.Drawing.Size(156, 28);
            this.addEmpl.TabIndex = 61;
            this.addEmpl.Text = "Добавить";
            this.addEmpl.UseVisualStyleBackColor = true;
            this.addEmpl.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxMail
            // 
            this.textBoxMail.Location = new System.Drawing.Point(147, 269);
            this.textBoxMail.Name = "textBoxMail";
            this.textBoxMail.Size = new System.Drawing.Size(227, 26);
            this.textBoxMail.TabIndex = 60;
            // 
            // textBoxPhone
            // 
            this.textBoxPhone.Location = new System.Drawing.Point(147, 233);
            this.textBoxPhone.Name = "textBoxPhone";
            this.textBoxPhone.Size = new System.Drawing.Size(227, 26);
            this.textBoxPhone.TabIndex = 59;
            // 
            // textBoxSurname
            // 
            this.textBoxSurname.Location = new System.Drawing.Point(147, 155);
            this.textBoxSurname.Name = "textBoxSurname";
            this.textBoxSurname.Size = new System.Drawing.Size(227, 26);
            this.textBoxSurname.TabIndex = 58;
            // 
            // textBoxFam
            // 
            this.textBoxFam.Location = new System.Drawing.Point(147, 91);
            this.textBoxFam.Name = "textBoxFam";
            this.textBoxFam.Size = new System.Drawing.Size(227, 26);
            this.textBoxFam.TabIndex = 57;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(147, 123);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(227, 26);
            this.textBoxName.TabIndex = 56;
            // 
            // MailProfile
            // 
            this.MailProfile.AutoSize = true;
            this.MailProfile.Location = new System.Drawing.Point(63, 275);
            this.MailProfile.Name = "MailProfile";
            this.MailProfile.Size = new System.Drawing.Size(48, 19);
            this.MailProfile.TabIndex = 55;
            this.MailProfile.Text = "E-mail";
            // 
            // PhoneProfile
            // 
            this.PhoneProfile.AutoSize = true;
            this.PhoneProfile.Location = new System.Drawing.Point(58, 233);
            this.PhoneProfile.Name = "PhoneProfile";
            this.PhoneProfile.Size = new System.Drawing.Size(65, 19);
            this.PhoneProfile.TabIndex = 54;
            this.PhoneProfile.Text = "Телефон";
            // 
            // SurnameProfile
            // 
            this.SurnameProfile.AutoSize = true;
            this.SurnameProfile.Location = new System.Drawing.Point(56, 158);
            this.SurnameProfile.Name = "SurnameProfile";
            this.SurnameProfile.Size = new System.Drawing.Size(73, 19);
            this.SurnameProfile.TabIndex = 53;
            this.SurnameProfile.Text = "Отчество";
            // 
            // FamProfile
            // 
            this.FamProfile.AutoSize = true;
            this.FamProfile.Location = new System.Drawing.Point(58, 93);
            this.FamProfile.Name = "FamProfile";
            this.FamProfile.Size = new System.Drawing.Size(72, 19);
            this.FamProfile.TabIndex = 52;
            this.FamProfile.Text = "Фамилия";
            // 
            // NameProfile
            // 
            this.NameProfile.AutoSize = true;
            this.NameProfile.Location = new System.Drawing.Point(58, 126);
            this.NameProfile.Name = "NameProfile";
            this.NameProfile.Size = new System.Drawing.Size(37, 19);
            this.NameProfile.TabIndex = 51;
            this.NameProfile.Text = "Имя";
            // 
            // UpdatePersonal
            // 
            this.UpdatePersonal.BackgroundImage = global::kurs.Properties.Resources.update_icon;
            this.UpdatePersonal.Location = new System.Drawing.Point(411, 306);
            this.UpdatePersonal.Name = "UpdatePersonal";
            this.UpdatePersonal.Size = new System.Drawing.Size(40, 40);
            this.UpdatePersonal.TabIndex = 50;
            this.UpdatePersonal.UseVisualStyleBackColor = true;
            this.UpdatePersonal.Click += new System.EventHandler(this.UpdatePersonal_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label5.Location = new System.Drawing.Point(56, 48);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 19);
            this.label5.TabIndex = 1;
            this.label5.Text = "Данные о сотрудниках";
            // 
            // сотрудникDataGridView
            // 
            this.сотрудникDataGridView.AutoGenerateColumns = false;
            this.сотрудникDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.сотрудникDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.сотрудникDataGridView.DataSource = this.сотрудникBindingSource;
            this.сотрудникDataGridView.Location = new System.Drawing.Point(458, 24);
            this.сотрудникDataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.сотрудникDataGridView.Name = "сотрудникDataGridView";
            this.сотрудникDataGridView.Size = new System.Drawing.Size(730, 322);
            this.сотрудникDataGridView.TabIndex = 0;
            this.сотрудникDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.сотрудникDataGridView_CellContentClick);
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "ФИО";
            this.dataGridViewTextBoxColumn5.HeaderText = "ФИО";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 300;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Должность";
            this.dataGridViewTextBoxColumn6.HeaderText = "Должность";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Электронная_почта";
            this.dataGridViewTextBoxColumn7.HeaderText = "Электронная_почта";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 150;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Телефон";
            this.dataGridViewTextBoxColumn8.HeaderText = "Телефон";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 120;
            // 
            // сотрудникBindingSource
            // 
            this.сотрудникBindingSource.DataMember = "Сотрудник";
            this.сотрудникBindingSource.DataSource = this.park_spotDataSet;
            this.сотрудникBindingSource.Sort = "Должность ASC";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.UpdateProfile);
            this.tabPage3.Controls.Add(this.buttonPassword);
            this.tabPage3.Controls.Add(this.textBoxPassword1);
            this.tabPage3.Controls.Add(this.label24);
            this.tabPage3.Controls.Add(this.textBoxPassword0);
            this.tabPage3.Controls.Add(this.textBoxLogin);
            this.tabPage3.Controls.Add(this.label23);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.buttonProfile);
            this.tabPage3.Controls.Add(this.textBoxMailProfile);
            this.tabPage3.Controls.Add(this.textBoxPhoneProfile);
            this.tabPage3.Controls.Add(this.textBoxSurProfile);
            this.tabPage3.Controls.Add(this.textBoxFamProfile);
            this.tabPage3.Controls.Add(this.textBoxNameProfile);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1210, 368);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Учётная запись";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // UpdateProfile
            // 
            this.UpdateProfile.BackgroundImage = global::kurs.Properties.Resources.update_icon;
            this.UpdateProfile.Location = new System.Drawing.Point(830, 240);
            this.UpdateProfile.Name = "UpdateProfile";
            this.UpdateProfile.Size = new System.Drawing.Size(40, 40);
            this.UpdateProfile.TabIndex = 51;
            this.UpdateProfile.UseVisualStyleBackColor = true;
            this.UpdateProfile.Click += new System.EventHandler(this.UpdateProfile_Click);
            // 
            // buttonPassword
            // 
            this.buttonPassword.Location = new System.Drawing.Point(489, 254);
            this.buttonPassword.Name = "buttonPassword";
            this.buttonPassword.Size = new System.Drawing.Size(316, 28);
            this.buttonPassword.TabIndex = 35;
            this.buttonPassword.Text = "Редактировать";
            this.buttonPassword.UseVisualStyleBackColor = true;
            this.buttonPassword.Click += new System.EventHandler(this.buttonPassword_Click);
            // 
            // textBoxPassword1
            // 
            this.textBoxPassword1.Location = new System.Drawing.Point(578, 203);
            this.textBoxPassword1.Name = "textBoxPassword1";
            this.textBoxPassword1.Size = new System.Drawing.Size(227, 26);
            this.textBoxPassword1.TabIndex = 34;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(439, 206);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(133, 19);
            this.label24.TabIndex = 33;
            this.label24.Text = "Повторите пароль";
            // 
            // textBoxPassword0
            // 
            this.textBoxPassword0.Location = new System.Drawing.Point(578, 162);
            this.textBoxPassword0.Name = "textBoxPassword0";
            this.textBoxPassword0.Size = new System.Drawing.Size(227, 26);
            this.textBoxPassword0.TabIndex = 32;
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(578, 128);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(227, 26);
            this.textBoxLogin.TabIndex = 31;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(520, 131);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(52, 19);
            this.label23.TabIndex = 30;
            this.label23.Text = "Логин";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(514, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 19);
            this.label8.TabIndex = 29;
            this.label8.Text = "Пароль";
            // 
            // buttonProfile
            // 
            this.buttonProfile.Location = new System.Drawing.Point(85, 252);
            this.buttonProfile.Name = "buttonProfile";
            this.buttonProfile.Size = new System.Drawing.Size(316, 28);
            this.buttonProfile.TabIndex = 28;
            this.buttonProfile.Text = "Редактировать";
            this.buttonProfile.UseVisualStyleBackColor = true;
            this.buttonProfile.Click += new System.EventHandler(this.buttonProfile_Click);
            // 
            // textBoxMailProfile
            // 
            this.textBoxMailProfile.Location = new System.Drawing.Point(174, 201);
            this.textBoxMailProfile.Name = "textBoxMailProfile";
            this.textBoxMailProfile.Size = new System.Drawing.Size(227, 26);
            this.textBoxMailProfile.TabIndex = 27;
            // 
            // textBoxPhoneProfile
            // 
            this.textBoxPhoneProfile.Location = new System.Drawing.Point(174, 165);
            this.textBoxPhoneProfile.Name = "textBoxPhoneProfile";
            this.textBoxPhoneProfile.Size = new System.Drawing.Size(227, 26);
            this.textBoxPhoneProfile.TabIndex = 26;
            // 
            // textBoxSurProfile
            // 
            this.textBoxSurProfile.Location = new System.Drawing.Point(174, 128);
            this.textBoxSurProfile.Name = "textBoxSurProfile";
            this.textBoxSurProfile.Size = new System.Drawing.Size(227, 26);
            this.textBoxSurProfile.TabIndex = 25;
            // 
            // textBoxFamProfile
            // 
            this.textBoxFamProfile.Location = new System.Drawing.Point(174, 88);
            this.textBoxFamProfile.Name = "textBoxFamProfile";
            this.textBoxFamProfile.Size = new System.Drawing.Size(227, 26);
            this.textBoxFamProfile.TabIndex = 24;
            // 
            // textBoxNameProfile
            // 
            this.textBoxNameProfile.Location = new System.Drawing.Point(174, 52);
            this.textBoxNameProfile.Name = "textBoxNameProfile";
            this.textBoxNameProfile.Size = new System.Drawing.Size(227, 26);
            this.textBoxNameProfile.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(90, 207);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 19);
            this.label6.TabIndex = 22;
            this.label6.Text = "E-mail";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(85, 165);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 19);
            this.label7.TabIndex = 21;
            this.label7.Text = "Телефон";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(85, 128);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 19);
            this.label9.TabIndex = 20;
            this.label9.Text = "Отчество";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(85, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 19);
            this.label10.TabIndex = 19;
            this.label10.Text = "Фамилия";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(85, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 19);
            this.label11.TabIndex = 18;
            this.label11.Text = "Имя";
            // 
            // NameEmpl
            // 
            this.NameEmpl.AutoSize = true;
            this.NameEmpl.Location = new System.Drawing.Point(136, 24);
            this.NameEmpl.Name = "NameEmpl";
            this.NameEmpl.Size = new System.Drawing.Size(96, 19);
            this.NameEmpl.TabIndex = 5;
            this.NameEmpl.Text = "manager_form";
            this.NameEmpl.Click += new System.EventHandler(this.Name_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(140, 56);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(93, 25);
            this.ExitButton.TabIndex = 52;
            this.ExitButton.Text = "Выход";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::kurs.Properties.Resources.admin_icon;
            this.pictureBox1.Location = new System.Drawing.Point(40, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(72, 72);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // fKПлатёжIDтариф4A23E96ABindingSource
            // 
            this.fKПлатёжIDтариф4A23E96ABindingSource.DataMember = "FK__Платёж__ID_тариф__4A23E96A";
            this.fKПлатёжIDтариф4A23E96ABindingSource.DataSource = this.тарифBindingSource;
            // 
            // платёжTableAdapter
            // 
            this.платёжTableAdapter.ClearBeforeFill = true;
            // 
            // manager_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1273, 507);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.NameEmpl);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "manager_form";
            this.Text = "Окно менеджера";
            this.Load += new System.EventHandler(this.manager_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.park_spotDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parkspotDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.тарифBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.тарифDataGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.сотрудникDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.сотрудникBindingSource)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKПлатёжIDтариф4A23E96ABindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Park_spotDataSet park_spotDataSet;
        private System.Windows.Forms.BindingSource parkspotDataSetBindingSource;
        private System.Windows.Forms.BindingSource тарифBindingSource;
        private Park_spotDataSetTableAdapters.ТарифTableAdapter тарифTableAdapter;
        private Park_spotDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridView тарифDataGridView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Park_spotDataSetTableAdapters.СотрудникTableAdapter сотрудникTableAdapter;
        private System.Windows.Forms.BindingSource сотрудникBindingSource;
        private System.Windows.Forms.DataGridView сотрудникDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCash;
        private System.Windows.Forms.TextBox textBoxHours;
        private System.Windows.Forms.TextBox textBoxNameTarif;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label NameEmpl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Updateclient;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.Button UpdatePersonal;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBoxMail;
        private System.Windows.Forms.TextBox textBoxPhone;
        private System.Windows.Forms.TextBox textBoxSurname;
        private System.Windows.Forms.TextBox textBoxFam;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label MailProfile;
        private System.Windows.Forms.Label PhoneProfile;
        private System.Windows.Forms.Label SurnameProfile;
        private System.Windows.Forms.Label FamProfile;
        private System.Windows.Forms.Label NameProfile;
        private System.Windows.Forms.Button buttonPassword;
        private System.Windows.Forms.TextBox textBoxPassword1;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox textBoxPassword0;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonProfile;
        private System.Windows.Forms.TextBox textBoxMailProfile;
        private System.Windows.Forms.TextBox textBoxPhoneProfile;
        private System.Windows.Forms.TextBox textBoxSurProfile;
        private System.Windows.Forms.TextBox textBoxFamProfile;
        private System.Windows.Forms.TextBox textBoxNameProfile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonRedact;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button UpdateProfile;
        private System.Windows.Forms.Button deleteEmpl;
        private System.Windows.Forms.Button addEmpl;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox emplVal;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.BindingSource fKПлатёжIDтариф4A23E96ABindingSource;
        private Park_spotDataSetTableAdapters.ПлатёжTableAdapter платёжTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}