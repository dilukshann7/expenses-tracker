using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Collections.Specialized.BitVector32;

namespace ExpensesTracker
{
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();

            StyleDataGridViewDark();

            displayCategories();

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
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(28, 28, 28); // Slight dark highlight
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
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.CurrentCell = null;

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void displayCategories()
        {
            CategoryData Data = new CategoryData();
            List<CategoryData> listData = Data.CategoryListData();

            dataGridView1.DataSource = listData;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                string query = "INSERT INTO categories (category, type, user_id, date_insert) VALUES (@category, @type, @user_id, @date_insert)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@category", textBox3.Text);
                    command.Parameters.AddWithValue("@type", comboBox1.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@date_insert", DateTime.Now);
                    command.Parameters.AddWithValue("@user_id", LoginInfo.ID);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            MessageBox.Show("Category saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            displayCategories();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string query = "UPDATE users SET username = @username, password = @password WHERE id = 2";

            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            SqlConnection conn = new SqlConnection(connStr);
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@password", textBox2.Text.Trim());

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Profile updated!");
                    }
                    else
                    {
                        MessageBox.Show("Nothing changed.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private int getID = 0;
        private void button5_Click_1(object sender, EventArgs e)
        {
            if (getID == 0)
            {
                MessageBox.Show("Please select a category to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this category?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();

                string deleteData = "DELETE FROM categories WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(deleteData, connect))
                {
                    cmd.Parameters.AddWithValue("@id", getID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Category deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                connect.Close();
            }

            // Refresh grid
            displayCategories();
            getID = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                getID = (int)row.Cells[0].Value;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
        }
    }
}
