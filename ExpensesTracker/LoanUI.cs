using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class LoanUI : UserControl
    {
        public LoanUI()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NewLoan newLoan = new NewLoan();
            if (newLoan.ShowDialog() == DialogResult.OK)
            {
                // Refresh data after new loan is added
                RefreshData();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void LoanUI_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                con.Open();

                string query = "SELECT DISTINCT bank FROM Loans WHERE user_id = @UserId ORDER BY bank";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", LoginInfo.ID); // ✅ Filter by logged user

                    SqlDataReader reader = cmd.ExecuteReader();

                    comboBox1.Items.Clear(); // Clear old items before reloading

                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["bank"].ToString());
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            string selectedBank = comboBox1.SelectedItem.ToString();
            int userId = LoginInfo.ID; // replace with current user ID

            SqlConnection con = new SqlConnection(DatabaseConfig.ConnectionString);
            
            con.Open();

            // Get loan details for selected bank and user
            string query = @"SELECT TOP 1 id, category, amount, start_date, end_date
                            FROM Loans
                            WHERE bank = @Bank AND user_id = @UserId
                            ORDER BY id DESC"; // get latest loan if multiple

            SqlCommand cmd = new SqlCommand(query, con);
            
            cmd.Parameters.AddWithValue("@Bank", selectedBank);
            cmd.Parameters.AddWithValue("@UserId", userId);

            SqlDataReader reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                category.Text = reader["category"].ToString();
                amount.Text = reader["amount"].ToString();
                startDate.Text = Convert.ToDateTime(reader["start_date"]).ToString("yyyy-MM-dd");
                dueDate.Text = Convert.ToDateTime(reader["end_date"]).ToString("yyyy-MM-dd");

                // Optionally calculate lastPaid and dueAmount
                lastPaid.Text = GetLastPaymentAmount((int)reader["id"], userId).ToString();
                dueAmount.Text = CalculateDueAmount((int)reader["id"], userId, (decimal)reader["amount"]).ToString();
            }
            else
            {
                // Clear fields if no loan found
                category.Clear();
                amount.Clear();
                startDate.Clear();
                dueDate.Clear();
                lastPaid.Clear();
                dueAmount.Clear();
            }     
        }

        private decimal GetLastPaymentAmount(int loanId, int userId)
        {
            decimal lastPaidAmount = 0;
            SqlConnection con = new SqlConnection(DatabaseConfig.ConnectionString);
            
            con.Open();
            string query = @"SELECT TOP 1 amount 
                        FROM LoanPayments 
                        WHERE loan_id = @LoanId AND user_id = @UserId
                        ORDER BY payment_date DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            var result = cmd.ExecuteScalar();
            if (result != null) lastPaidAmount = Convert.ToDecimal(result);
            
            return lastPaidAmount;
        }

        private decimal CalculateDueAmount(int loanId, int userId, decimal totalAmount)
        {
            decimal paid = 0;
            SqlConnection con = new SqlConnection(DatabaseConfig.ConnectionString);
            
            con.Open();
            string query = @"SELECT ISNULL(SUM(amount),0) 
                        FROM LoanPayments 
                        WHERE loan_id = @LoanId AND user_id = @UserId";

            SqlCommand cmd = new SqlCommand(query, con);
            
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            paid = Convert.ToDecimal(cmd.ExecuteScalar());
                    
            return totalAmount - paid;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a bank first.");
                return;
            }

            string selectedBank = comboBox1.SelectedItem.ToString();
            int userId = LoginInfo.ID; // current user id
            decimal paymentAmount;
            if (!decimal.TryParse(payAmount.Text, out paymentAmount) || paymentAmount <= 0)
            {
                MessageBox.Show("Enter a valid payment amount.");
                return;
            }

            string paymentMethod = payMethod.Text;
            DateTime paymentDate = payDate.Value;

            using (SqlConnection con = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                con.Open();

                // Get loan ID for the selected bank
                string getLoanQuery = @"SELECT TOP 1 id, amount 
                                FROM Loans 
                                WHERE bank = @Bank AND user_id = @UserId 
                                ORDER BY id DESC";

                int loanId = 0;
                decimal totalLoanAmount = 0;

                using (SqlCommand cmd = new SqlCommand(getLoanQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Bank", selectedBank);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            loanId = Convert.ToInt32(reader["id"]);
                            totalLoanAmount = Convert.ToDecimal(reader["amount"]);
                        }
                        else
                        {
                            MessageBox.Show("No loan found for this bank.");
                            return;
                        }
                    }
                }

                // Insert payment record
                string insertPayment = @"INSERT INTO LoanPayments (loan_id, user_id, amount, payment_date, method)
                                 VALUES (@LoanId, @UserId, @Amount, @Date, @Method)";
                using (SqlCommand cmd = new SqlCommand(insertPayment, con))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                    cmd.Parameters.AddWithValue("@Date", paymentDate);
                    cmd.Parameters.AddWithValue("@Method", paymentMethod);
                    cmd.ExecuteNonQuery();
                }

                // Get total payments made
                string getTotalPaid = @"SELECT ISNULL(SUM(amount),0) 
                                FROM LoanPayments 
                                WHERE loan_id = @LoanId AND user_id = @UserId";
                decimal totalPaid = 0;
                using (SqlCommand cmd = new SqlCommand(getTotalPaid, con))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    totalPaid = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                decimal remaining = totalLoanAmount - totalPaid;

                // Update UI fields
                dueAmount.Text = remaining.ToString("0.00");
                lastPaid.Text = paymentAmount.ToString("0.00");

                MessageBox.Show($"Payment recorded successfully!\nRemaining Balance: {remaining:0.00}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a bank first.");
                return;
            }

            string selectedBank = comboBox1.SelectedItem.ToString();
            int userId = LoginInfo.ID;

            using (SqlConnection con = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                con.Open();
                string query = @"SELECT TOP 1 id 
                         FROM Loans 
                         WHERE bank = @Bank AND user_id = @UserId
                         ORDER BY id DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Bank", selectedBank);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int loanId = Convert.ToInt32(result);
                        LoanReport reportForm = new LoanReport(loanId);
                        reportForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No loan found for this bank.");
                    }
                }
            }
        }

        // Add this method to refresh data when switching between forms
        public void RefreshData()
        {
            LoanUI_Load(null, null); // Refresh the bank dropdown
        }
    }
}
