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

namespace Taxi_Helper
{
    public partial class Menu : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Table");
                dataGridView1.DataSource = dataSet.Tables["Table"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Table"].Clear();
                sqlDataAdapter.Fill(dataSet, "Table");
                dataGridView1.DataSource = dataSet.Tables["Table"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Menu()
        {
            InitializeComponent();
        }

        private async void Add_button_Click(object sender, EventArgs e)
        {
            int cash = 0;
            SqlCommand command = new SqlCommand("INSERT INTO [Table] (Дата, ПП, ЗЛ, ЗЦ, ЗП, Направление, Сумма)VALUES(@Дата, @ПП, @ЗЛ, @ЗЦ, @ЗП, @Направление, @Сумма)", sqlConnection);
            command.Parameters.AddWithValue("Дата", dateTimePicker1.Value);
            command.Parameters.AddWithValue("ПП", comboBox1.SelectedItem.ToString());
            command.Parameters.AddWithValue("ЗЛ", comboBox2.SelectedItem.ToString());
            command.Parameters.AddWithValue("ЗЦ", comboBox3.SelectedItem.ToString());
            command.Parameters.AddWithValue("ЗП", comboBox4.SelectedItem.ToString());
            switch (radioButton1.Checked)
            {
                case true:
                    command.Parameters.AddWithValue("Направление", radioButton1.Text);
                    break;
                case false:
                    command.Parameters.AddWithValue("Направление", radioButton2.Text);
                    break;
            }
            switch (Convert.ToString(comboBox1.SelectedItem.ToString()))
            {
                case "Матвей":
                    cash += 50;
                    break;
                case "Пусто":
                    cash += 0;
                    break;
                default:
                    cash += 100;
                    break;
            }
            switch (Convert.ToString(comboBox2.SelectedItem.ToString()))
            {
                case "Матвей":
                    cash += 50;
                    break;
                case "Пусто":
                    cash += 0;
                    break;
                default:
                    cash += 100;
                    break;
            }
            switch (Convert.ToString(comboBox3.SelectedItem.ToString()))
            {
                case "Матвей":
                    cash += 50;
                    break;
                case "Пусто":
                    cash += 0;
                    break;
                default:
                    cash += 100;
                    break;
            }
            switch (Convert.ToString(comboBox4.SelectedItem.ToString()))
            {
                case "Матвей":
                    cash += 50;
                    break;
                case "Пусто":
                    cash += 0;
                    break;
                default:
                    cash += 100;
                    break;
            }
            switch (checkBox1.Checked)
            {
                case true:
                    cash -= Convert.ToInt32(textBox1.Text);
                    break;
                case false:
                    break;
            }
            switch (checkBox2.Checked)
            {
                case true:
                    cash -= Convert.ToInt32(textBox2.Text);
                    break;
                case false:
                    break;
            }
            command.Parameters.AddWithValue("Сумма", cash);
            await command.ExecuteNonQueryAsync();
            ReloadData();
            Check_sum();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alexander\source\repos\Taxi_Helper\Taxi_Helper\Database.mdf;Integrated Security=True");
            sqlConnection.Open();
            LoadData();
            try
            {
                Check_sum();
            }
            catch
            {
                
            }
            
        }

        private async void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

            DialogResult dr = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            SqlCommand command = new SqlCommand("DELETE FROM [Table] WHERE[Id] = @Id", sqlConnection);
            command.Parameters.AddWithValue("Id", dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value);
            await command.ExecuteNonQueryAsync();
            ReloadData();
            Check_sum();
        }

        private void Clean_button_Click(object sender, EventArgs e)
        {
            ClearData();
            richTextBox1.Text = "0";
            richTextBox1.BackColor = Color.Green;
        }

        public async void ClearData()
        {
            SqlCommand command = new SqlCommand("TRUNCATE TABLE [Table]", sqlConnection);
            await command.ExecuteNonQueryAsync();
            ReloadData();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            switch (textBox1.Enabled)
            {
                case true:
                    textBox1.Enabled = false;
                    textBox1.Text = "";
                    break;
                case false:
                    textBox1.Enabled = true;
                    break;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            switch (textBox2.Enabled)
            {
                case true:
                    textBox2.Enabled = false;
                    textBox2.Text = "";
                    break;
                case false:
                    textBox2.Enabled = true;
                    break;
            }
        }

        public void Check_sum()
        {
            int sum = 0;
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                sum += Convert.ToInt32(dataGridView1[7, j].Value);
            }
            switch (sum >= 0)
            {
                case true:
                    richTextBox1.BackColor = Color.Green;
                    break;
                case false:
                    richTextBox1.BackColor = Color.Red;
                    break;
            }
            richTextBox1.Text = Convert.ToString(sum);
        }
    }
    }
