using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class NewLoan : Form
    {
        public NewLoan()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(category.Text) ||
                string.IsNullOrWhiteSpace(amount.Text) ||
                bankComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (!decimal.TryParse(amount.Text, out decimal loanAmount))
            {
                MessageBox.Show("Please enter a valid number for amount.");
                return;
            }

            string selectedBank = bankComboBox.SelectedItem.ToString();
            string loanCategory = category.Text;
            DateTime startDate = startingDate.Value.Date;
            DateTime endDate = finalDate.Value.Date;

            int userId = LoginInfo.ID;

            using (SqlConnection con = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = @"INSERT INTO Loans (user_id, category, amount, bank, start_date, end_date)
                                    VALUES (@UserId, @Category, @Amount, @Bank, @StartDate, @EndDate)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@Category", loanCategory);
                        cmd.Parameters.AddWithValue("@Amount", loanAmount);
                        cmd.Parameters.AddWithValue("@Bank", selectedBank);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Loan added successfully!");
                            category.Clear();
                            amount.Clear();
                            bankComboBox.SelectedIndex = -1;
                            startingDate.Value = DateTime.Now;
                            finalDate.Value = DateTime.Now;
                            
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

    }
}
