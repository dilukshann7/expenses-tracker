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
using System.IO;

namespace ExpensesTracker
{
    public partial class Dashboard: UserControl
    {

        public Dashboard()
        {
            InitializeComponent();
            LoadIncomeAndExpenseData();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView2.CellFormatting += dataGridView2_CellFormatting;
            displayExpenseData();
            StyleDataGridViewDark();
        }

        public void LoadIncomeAndExpenseData()
        {
            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();

                // Income: This Week
                using (SqlCommand cmd = new SqlCommand("SELECT SUM(income) FROM income WHERE DATEPART(week, date_income) = DATEPART(week, GETDATE()) AND YEAR(date_income) = YEAR(GETDATE()) AND user_id = @user_id", connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);
                    object result = cmd.ExecuteScalar();
                    income_thisweek.Text = result != DBNull.Value ? "$" + Convert.ToDecimal(result).ToString("0.00") : "$0.00";
                }

                // Income: This Month
                using (SqlCommand cmd = new SqlCommand("SELECT SUM(income) FROM income WHERE MONTH(date_income) = MONTH(GETDATE()) AND YEAR(date_income) = YEAR(GETDATE()) AND user_id = @user_id", connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);
                    object result = cmd.ExecuteScalar();
                    income_thismonth.Text = result != DBNull.Value ? "$" + Convert.ToDecimal(result).ToString("0.00") : "$0.00";
                }

                // Income: This Year
                using (SqlCommand cmd = new SqlCommand("SELECT SUM(income) FROM income WHERE YEAR(date_income) = YEAR(GETDATE()) AND user_id = @user_id", connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);
                    object result = cmd.ExecuteScalar();
                    income_thisyear.Text = result != DBNull.Value ? "$" + Convert.ToDecimal(result).ToString("0.00") : "$0.00";
                }

                // Expense: This Week
                using (SqlCommand cmd = new SqlCommand("SELECT SUM(cost) FROM expenses WHERE DATEPART(week, date_expense) = DATEPART(week, GETDATE()) AND YEAR(date_expense) = YEAR(GETDATE()) AND user_id = @user_id", connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);
                    object result = cmd.ExecuteScalar();
                    expense_thisweek.Text = result != DBNull.Value ? "$" + Convert.ToDecimal(result).ToString("0.00") : "$0.00";
                }

                // Expense: This Month
                using (SqlCommand cmd = new SqlCommand("SELECT SUM(cost) FROM expenses WHERE MONTH(date_expense) = MONTH(GETDATE()) AND YEAR(date_expense) = YEAR(GETDATE()) AND user_id = @user_id", connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);
                    object result = cmd.ExecuteScalar();
                    expense_thismonth.Text = result != DBNull.Value ? "$" + Convert.ToDecimal(result).ToString("0.00") : "$0.00";
                }

                // Expense: This Year
                using (SqlCommand cmd = new SqlCommand("SELECT SUM(cost) FROM expenses WHERE YEAR(date_expense) = YEAR(GETDATE()) AND user_id = @user_id", connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);
                    object result = cmd.ExecuteScalar();
                    expense_thisyear.Text = result != DBNull.Value ? "$" + Convert.ToDecimal(result).ToString("0.00") : "$0.00";
                }
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM expenses WHERE user_id = @user_id";

            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);

                    connect.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            Filter = "CSV file (*.csv)|*.csv",
                            FileName = "export.csv"
                        };

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    sw.Write(reader.GetName(i));
                                    if (i < reader.FieldCount - 1)
                                        sw.Write(",");
                                }
                                sw.WriteLine();

                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string value = reader[i].ToString().Replace("\"", "\"\"");
                                        sw.Write($"\"{value}\"");
                                        if (i < reader.FieldCount - 1)
                                            sw.Write(",");
                                    }
                                    sw.WriteLine();
                                }

                                MessageBox.Show("CSV Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }  
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void expense_thisweek_Click(object sender, EventArgs e)
        {

        }

        private void expense_thismonth_Click(object sender, EventArgs e)
        {

        }

        private void income_thisweek_Click(object sender, EventArgs e)
        {

        }

        private void income_thismonth_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void displayExpenseData()
        {
            ExpenseDataDashboard Data = new ExpenseDataDashboard();
            List<ExpenseDataDashboard> listData = Data.expenseListData();

            dataGridView1.DataSource = listData;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            IncomeDataDashboard iData = new IncomeDataDashboard();
            List<IncomeDataDashboard> listiData = iData.incomeListData();

            dataGridView2.DataSource = listiData;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private int getID = 0;
        private void panel13_Paint(object sender, PaintEventArgs e)
        {

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
                    e.CellStyle.Font = new Font("Poppins", 9, FontStyle.Bold);
                }
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
                    e.CellStyle.Font = new Font("Poppins", 9, FontStyle.Bold);
                }
            }
        }


        private void StyleDataGridViewDark()
        {
            void SetupDataGridView(DataGridView dgv)
            {
                // Appearance
                dgv.BorderStyle = BorderStyle.None;
                dgv.BackgroundColor = Color.FromArgb(30, 30, 30);
                dgv.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                dgv.DefaultCellStyle.ForeColor = Color.White;
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 38);
                dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;

                // Grid & Selection
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.ReadOnly = true;
                dgv.RowHeadersVisible = false;
                dgv.RowTemplate.Height = 40;
                dgv.CurrentCell = null;

                // Fonts
                dgv.DefaultCellStyle.Font = new Font("Poppins", 10);
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Poppins", 10, FontStyle.Bold);

                // Header styles
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 65);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                // Disable selection visual feedback
                dgv.DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.BackColor;
                dgv.DefaultCellStyle.SelectionForeColor = dgv.DefaultCellStyle.ForeColor;

                // Prevent selection behavior
                dgv.SelectionChanged += (s, e) => dgv.ClearSelection();
                dgv.CurrentCell = null;

                // Hide unwanted columns if they exist
                if (dgv.Columns.Contains("ID")) dgv.Columns["ID"].Visible = false;
                if (dgv.Columns.Contains("Description")) dgv.Columns["Description"].Visible = false;

                // Add $ to Cost column
                if (dgv.Columns.Contains("Cost")) dgv.Columns["Cost"].DefaultCellStyle.Format = "C2";
            }
            SetupDataGridView(dataGridView1); 
            SetupDataGridView(dataGridView2);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
