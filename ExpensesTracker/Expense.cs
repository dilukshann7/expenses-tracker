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
    public partial class Expense: UserControl
    {
        public Expense()
        {
            InitializeComponent();

            displayCategories();
            displayExpenseData();
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

        public void displayExpenseData()
        {
            ExpenseData Data = new ExpenseData();
            List<ExpenseData> listData = Data.expenseListData(LoginInfo.ID); 

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
                    cmd.Parameters.AddWithValue("@type", "Expense");
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

     

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void clearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
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
                    string insertData = "INSERT INTO expenses (category, item, cost, description, date_expense, date_insert, user_id)" +
                        "VALUES(@cat, @item, @cost, @desc, @date_expense, @date_insert, @user_id);";
                    using (SqlCommand cmd = new SqlCommand(insertData, connect))
                    {
                        cmd.Parameters.AddWithValue("@cat", comboBox1.SelectedItem);
                        cmd.Parameters.AddWithValue("@item", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@cost", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@desc", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@date_expense", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@date_insert", DateTime.Now);
                        cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID); // Add the current user's ID
                        cmd.ExecuteNonQuery();
                        clearFields();
                        MessageBox.Show("Added successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    connect.Close();
                }
            }
            displayExpenseData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            displayCategories();
            displayExpenseData();
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

                    string updateData = "UPDATE expenses SET category = @category, item = @item, cost = @cost, description = @description, date_expense = @date_expense, date_insert = @date_insert WHERE id = @id AND user_id = @user_id";

                    using (SqlCommand cmd = new SqlCommand(updateData, connect))
                    {
                        cmd.Parameters.AddWithValue("@category", comboBox1.SelectedItem);
                        cmd.Parameters.AddWithValue("@item", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@cost", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@description", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@date_expense", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@date_insert", DateTime.Now);
                        cmd.Parameters.AddWithValue("@id", getID);
                        cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID); // Ensure user_id is included

                        cmd.ExecuteNonQuery();
                        clearFields();

                        MessageBox.Show("Updated successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    connect.Close();
                }
            }
            displayExpenseData();
        }


        private int getID = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

                    string deleteData = "DELETE FROM expenses WHERE id = @id";

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
            displayExpenseData();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
