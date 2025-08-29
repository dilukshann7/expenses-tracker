using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpensesTracker
{
    class IncomeDataDashboard
    {
        public int ID { set; get; }
        public string Category { set; get; }
        public string Item { set; get; }
        public string Cost { set; get; }
        public string Description { set; get; }
        public string DateIncome { set; get; }

        public List<IncomeDataDashboard> incomeListData()
        {
            List<IncomeDataDashboard> listData = new List<IncomeDataDashboard>();

            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();

                string selectData = "SELECT * FROM income WHERE user_id = @user_id";

                using (SqlCommand cmd = new SqlCommand(selectData, connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        IncomeDataDashboard data = new IncomeDataDashboard();
                        data.Category = reader["category"].ToString();
                        data.Item = reader["item"].ToString();
                        data.Cost = reader["income"].ToString();
                        data.DateIncome = reader["date_income"].ToString();
                        listData.Add(data);
                    }
                }
            }

            return listData;
        }

    }
}
