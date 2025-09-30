using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace ExpensesTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (username.Text == "" || password.Text == "")
            {
                MessageBox.Show("Invalid Credentials", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(DatabaseConfig.ConnectionString))
                    {
                        connect.Open();

                        string selectUsername = "SELECT * FROM users WHERE username = @usern";
                        using (SqlCommand checkUser = new SqlCommand(selectUsername, connect))
                        {
                            checkUser.Parameters.AddWithValue("@usern", username.Text.Trim());
                            SqlDataAdapter adapter = new SqlDataAdapter(checkUser);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count != 0)
                            {
                                MessageBox.Show("Username already exists!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string insertData = "INSERT INTO users (username, password, date_create) OUTPUT INSERTED.id VALUES(@usern, @pass, @date)";
                                using (SqlCommand insertUser = new SqlCommand(insertData, connect))
                                {
                                    insertUser.Parameters.AddWithValue("@usern", username.Text.Trim());
                                    insertUser.Parameters.AddWithValue("@pass", password.Text.Trim());
                                    insertUser.Parameters.AddWithValue("@date", DateTime.Now);

                                    int userId = (int)insertUser.ExecuteScalar(); // Get inserted ID

                                    // Log registration
                                    string logRegister = "INSERT INTO UserActivityLog (UserId, Username, ActionType, Notes) VALUES (@userId, @username, 'REGISTER', 'New account created')";
                                    using (SqlCommand logCmd = new SqlCommand(logRegister, connect))
                                    {
                                        logCmd.Parameters.AddWithValue("@userId", userId);
                                        logCmd.Parameters.AddWithValue("@username", username.Text.Trim());
                                        logCmd.ExecuteNonQuery();
                                    }

                                    MessageBox.Show("Account Created Successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    username.Clear();
                                    password.Clear();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Account Creation Failed: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                connect.Open();
                string selectUsername = "SELECT id, username FROM users WHERE username = @usern AND password = @pass";
                using (SqlCommand checkUser = new SqlCommand(selectUsername, connect))
                {
                    checkUser.Parameters.AddWithValue("@usern", username.Text.Trim());
                    checkUser.Parameters.AddWithValue("@pass", password.Text.Trim());
                    SqlDataAdapter adapter = new SqlDataAdapter(checkUser);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count != 0)
                    {
                        int userId = Convert.ToInt32(table.Rows[0]["id"]);
                        string usernameValue = table.Rows[0]["username"].ToString();

                        // Log Successful Login
                        string logLogin = "INSERT INTO UserActivityLog (UserId, Username, ActionType, Notes) VALUES (@userId, @username, 'LOGIN', 'Successful login')";
                        using (SqlCommand logCmd = new SqlCommand(logLogin, connect))
                        {
                            logCmd.Parameters.AddWithValue("@userId", userId);
                            logCmd.Parameters.AddWithValue("@username", usernameValue);
                            logCmd.ExecuteNonQuery();
                        }

                        LoginInfo.ID = userId;
                        LoginInfo.Username = usernameValue;
                        LoginInfo.IsLoggedIn = true;

                        this.Hide();
                        Form2 form2 = new Form2();
                        form2.Show();
                    }
                    else
                    {
                        // Log Failed Login Attempt
                        string logFailed = "INSERT INTO UserActivityLog (Username, ActionType, Notes) VALUES (@username, 'LOGIN_FAILED', 'Invalid credentials')";
                        using (SqlCommand logFailedCmd = new SqlCommand(logFailed, connect))
                        {
                            logFailedCmd.Parameters.AddWithValue("@username", username.Text.Trim());
                            logFailedCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Invalid Credentials", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SecurityReport securityReport = new SecurityReport();
            securityReport.Show();
        }
    }
}
