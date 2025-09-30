using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ExpensesTracker
{
    public class ExpenseData
    {
        public int ID { set; get; }
        public string Category { set; get; }
        public string Item { set; get; }
        public string Cost { set; get; }
        public string Description { set; get; }
        public string DateExpense { set; get; }

        public List<ExpenseData> expenseListData(int userID)
        {
            List<ExpenseData> listData = new List<ExpenseData>();

            using (SqlConnection connect = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                connect.Open();

                string selectData = "SELECT * FROM expenses WHERE user_id = @userID";

                using (SqlCommand cmd = new SqlCommand(selectData, connect))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ExpenseData edata = new ExpenseData();
                        edata.ID = Convert.ToInt32(reader["ID"]);
                        edata.Category = reader["category"].ToString();
                        edata.Item = reader["item"].ToString();
                        edata.Cost = reader["cost"].ToString();
                        edata.Description = reader["description"].ToString();
                        edata.DateExpense = reader["date_expense"].ToString();
                        listData.Add(edata);
                    }
                }
            }
            return listData;
        }
    }
}
