using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ExpensesTracker
{
    class IncomeData
    {
        public int ID { set; get; }
        public string Category { set; get; }
        public string Item { set; get; }
        public string Cost { set; get; }
        public string Description { set; get; }
        public string DateIncome { set; get; }

        public List<IncomeData> incomeListData(int userID)
        {
            List<IncomeData> listData = new List<IncomeData>();

            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();

                string selectData = "SELECT * FROM income WHERE user_id = @userID";

                using (SqlCommand cmd = new SqlCommand(selectData, connect))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        IncomeData data = new IncomeData();
                        data.ID = Convert.ToInt32(reader["ID"]);
                        data.Category = reader["category"].ToString();
                        data.Item = reader["item"].ToString();
                        data.Cost = reader["income"].ToString();
                        data.Description = reader["description"].ToString();
                        data.DateIncome = reader["date_income"].ToString();
                        listData.Add(data);
                    }
                }
            }
            return listData;
        }

    }
}
