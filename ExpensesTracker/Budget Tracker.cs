using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ExpensesTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                        string dbPath = Application.StartupPath + @"\expense.mdf";
                        string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

                        SqlConnection connect = new SqlConnection(connStr);
                        
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
                                string tempUsername = username.Text.Substring(0, 1).ToUpper() + username.Text.Substring(1);
                                MessageBox.Show(tempUsername + " is existing already", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string insertData = "INSERT INTO users (username, password, date_create) VALUES(@usern, @pass, @date)";

                                using (SqlCommand insertUser = new SqlCommand(insertData, connect))
                                {
                                    insertUser.Parameters.AddWithValue("@usern", username.Text.Trim());
                                    insertUser.Parameters.AddWithValue("@pass", password.Text.Trim());
                                    insertUser.Parameters.AddWithValue("@date", DateTime.Now);
                                    int rows = insertUser.ExecuteNonQuery();
                                    if (rows > 0)
                                    {
                                        MessageBox.Show("Account Created Successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        username.Clear();
                                        password.Clear();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Account Creation Failed", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                        connect.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Account Creation Failed", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private void password_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connect = new SqlConnection(connStr))
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
                        // Get the user ID from the first row
                        int userId = Convert.ToInt32(table.Rows[0]["id"]);
                        string usernameValue = table.Rows[0]["username"].ToString();

                        // Store login info
                        LoginInfo.ID = userId;
                        LoginInfo.Username = usernameValue;
                        LoginInfo.IsLoggedIn = true;

                        MessageBox.Show("Login Successful", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Hide();
                        Form2 form2 = new Form2();
                        form2.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Credentials", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
