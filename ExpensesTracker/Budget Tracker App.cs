using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class Form2: Form
    {
        public void displayIncomeData()
        {
            IncomeData Data = new IncomeData();
            List<IncomeData> listData = Data.incomeListData(LoginInfo.ID);

        }

        public void displayCategories()
        {
            CategoryData Data = new CategoryData();
            List<CategoryData> listData = Data.CategoryListData();

        }

        public void displayExpenseData()
        {
            ExpenseData Data = new ExpenseData();
            List<ExpenseData> listData = Data.expenseListData(LoginInfo.ID);

        }
        public Form2()
        {
            InitializeComponent();

            button1.Click += SidebarButton_Click;
            button2.Click += SidebarButton_Click;
            button3.Click += SidebarButton_Click;
            button4.Click += SidebarButton_Click;

            dashboard1.Visible = true;
            income1.Visible = false;
            expense1.Visible = false;
            profile1.Visible = false;
        }

        private Button selectedButton;

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            // Reset previous selection
            if (selectedButton != null)
                selectedButton.BackColor = Color.FromArgb(28, 28, 28);

            // Highlight the clicked button
            selectedButton = sender as Button;
            selectedButton.BackColor = Color.FromArgb(40, 40, 40); // Dark gray
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dashboard1.Visible = false;
            income1.Visible = true;
            expense1.Visible = false;
            profile1.Visible = false;
            displayIncomeData();
            displayCategories();
            displayExpenseData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dashboard1.Visible = true;
            income1.Visible = false;
            expense1.Visible = false;
            profile1.Visible = false;
            displayIncomeData();
            displayCategories();
            displayExpenseData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dashboard1.Visible = false;
            income1.Visible = false;
            expense1.Visible = true;
            profile1.Visible = false;
            displayIncomeData();
            displayCategories();
            displayExpenseData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dashboard1.Visible = false;
            income1.Visible = false;
            expense1.Visible = false;
            profile1.Visible = true;
            displayIncomeData();
            displayCategories();
            displayExpenseData();
        }

        private void income1_Load(object sender, EventArgs e)
        {

        }

        private void dashboard1_Load(object sender, EventArgs e)
        {

        }

        private void profile1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
