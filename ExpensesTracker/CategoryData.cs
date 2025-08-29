using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ExpensesTracker
{
    class CategoryData
    {
        public int ID { set; get; }
        public string Category { set; get; }
        public string Type { set; get; }
        public string DateInsert { set; get; }

        public List<CategoryData> CategoryListData()
        {
            List<CategoryData> listData = new List<CategoryData>();
            string dbPath = Application.StartupPath + @"\expense.mdf";
            string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;";

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();
                string selectData = "SELECT * FROM categories WHERE user_id = @user_id";
                using (SqlCommand cmd = new SqlCommand(selectData, connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", LoginInfo.ID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CategoryData category = new CategoryData();
                        category.ID = Convert.ToInt32(reader["ID"]);
                        category.Category = reader["category"].ToString();
                        category.Type = reader["type"].ToString();
                        category.DateInsert = reader["date_insert"].ToString();
                        listData.Add(category);
                    }
                }
                return listData;
            }
        }
    }
}