using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ExpensesTracker
{
    public partial class Income: UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\diluk\OneDrive\Documents\expense.mdf;Integrated Security=True;Connect Timeout=30");
        public Income()
        {
            InitializeComponent();

            displayCategories();
            displayIncomeData();
            StyleDataGridViewDark();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "status")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status == "Submitted")
                    {
                        e.CellStyle.BackColor = Color.MediumSlateBlue;
                        e.CellStyle.ForeColor = Color.White;
                    }
                    else if (status == "Not Submitted")
                    {
                        e.CellStyle.BackColor = Color.MediumVioletRed;
                        e.CellStyle.ForeColor = Color.White;
                    }

                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            }
        }


        private void StyleDataGridViewDark()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 38);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 65);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Poppins", 10, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new Font("Poppins", 10);
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.CurrentCell = null;
        }

        public void displayIncomeData()
        {
            IncomeData Data = new IncomeData();
            List<IncomeData> listData = Data.incomeListData(LoginInfo.ID); 

            dataGridView1.DataSource = listData;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        public void displayCategories()
        {
            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();

                string selectData = "SELECT category FROM categories WHERE type = @type AND user_id = @user_id";

                using (SqlCommand cmd = new SqlCommand(selectData, connect))
                {
                    cmd.Parameters.AddWithValue("@type", "Income");
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);

                    comboBox1.Items.Clear();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["category"].ToString());
                    }
                }
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string dbPath = Application.StartupPath + @"\expense.mdf";
                string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

                using (SqlConnection connect = new SqlConnection(connStr))
                {
                    connect.Open();
                    string insertData = "INSERT INTO income (category, item, income, description, date_income, date_insert, user_id)" +
                        "VALUES(@cat, @item, @income, @desc, @date_income, @date_insert, @user_id);";
                    using (SqlCommand cmd = new SqlCommand(insertData, connect))
                    {
                        cmd.Parameters.AddWithValue("@cat", comboBox1.SelectedItem);
                        cmd.Parameters.AddWithValue("@item", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@income", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@desc", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@date_income", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@date_insert", DateTime.Now);
                        cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID); // Add the user ID
                        cmd.ExecuteNonQuery();
                        clearFields();
                        MessageBox.Show("Added successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    connect.Close();
                }
            }
            displayIncomeData();
        }

        public void clearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;  
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            displayCategories();
            displayIncomeData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string dbPath = Application.StartupPath + @"\expense.mdf";
                string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

                using (SqlConnection connect = new SqlConnection(connStr))
                {
                    connect.Open();

                    string updateData = "UPDATE income SET category = @category, item = @item, income = @income, description = @description, date_income = @date_income, date_insert = @date_insert WHERE id = @id AND user_id = @user_id";

                    using (SqlCommand cmd = new SqlCommand(updateData, connect))
                    {
                        cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);  // Pass the correct user ID

                        cmd.Parameters.AddWithValue("@category", comboBox1.SelectedItem);
                        cmd.Parameters.AddWithValue("@item", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@income", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@description", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@date_income", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@date_insert", DateTime.Now);
                        cmd.Parameters.AddWithValue("@id", getID);

                        cmd.ExecuteNonQuery();
                        clearFields();

                        MessageBox.Show("Updated successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    connect.Close();
                }
            }
            displayIncomeData();
        }

        private int getID = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                getID = (int)row.Cells[0].Value;
                comboBox1.SelectedItem = row.Cells[1].Value.ToString();
                textBox1.Text = row.Cells[2].Value.ToString();
                textBox2.Text = row.Cells[3].Value.ToString();
                textBox3.Text = row.Cells[4].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells[5].Value.ToString());

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string dbPath = Application.StartupPath + @"\expense.mdf";
                string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

                using (SqlConnection connect = new SqlConnection(connStr))
                {
                    connect.Open();

                    string deleteData = "DELETE FROM income WHERE id = @id";

                    using (SqlCommand cmd = new SqlCommand(deleteData, connect))
                    {
                        cmd.Parameters.AddWithValue("@id", getID);

                        cmd.ExecuteNonQuery();
                        clearFields();

                        MessageBox.Show("Updated successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    connect.Close();
                }
            }
            displayIncomeData();
        }
    }
}