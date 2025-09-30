using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class Form2 : Form
    {
        public void DisplayIncomeData()
        {
            IncomeData Data = new IncomeData();
            List<IncomeData> listData = Data.incomeListData(LoginInfo.ID);
        }

        public void DisplayCategories()
        {
            CategoryData Data = new CategoryData();
            List<CategoryData> listData = Data.CategoryListData();
        }


        public void DisplayExpenseData()
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
            button5.Click += SidebarButton_Click;

            dashboard3.Visible = true;
            income3.Visible = false;
            expense1.Visible = false;
            profile1.Visible = false;
            loanUI1.Visible = false;
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
            dashboard3.Visible = false;
            income3.Visible = true;
            expense1.Visible = false;
            profile1.Visible = false;
            loanUI1.Visible = false;

            // Refresh the income form data
            income3.RefreshData();

            DisplayIncomeData();
            DisplayCategories();
            DisplayExpenseData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dashboard3.Visible = true;
            income3.Visible = false;
            expense1.Visible = false;
            profile1.Visible = false;
            loanUI1.Visible = false;

            // Refresh the dashboard data
            dashboard3.LoadIncomeAndExpenseData();
            dashboard3.displayExpenseData();

            DisplayIncomeData();
            DisplayCategories();
            DisplayExpenseData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dashboard3.Visible = false;
            income3.Visible = false;
            expense1.Visible = true;
            profile1.Visible = false;
            loanUI1.Visible = false;

            // Refresh the expense form data
            expense1.RefreshData();

            DisplayIncomeData();
            DisplayCategories();
            DisplayExpenseData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dashboard3.Visible = false;
            income3.Visible = false;
            expense1.Visible = false;
            profile1.Visible = true;
            loanUI1.Visible = false;

            // Refresh the profile form data
            profile1.RefreshData();

            DisplayIncomeData();
            DisplayCategories();
            DisplayExpenseData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dashboard3.Visible = false;
            income3.Visible = false;
            expense1.Visible = false;
            profile1.Visible = false;
            loanUI1.Visible = true;

            // Refresh the loan form data
            loanUI1.RefreshData();

            DisplayIncomeData();
            DisplayCategories();
            DisplayExpenseData();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dashboard3.Visible = true;
            income3.Visible = false;
            expense1.Visible = false;
            profile1.Visible = false;
            loanUI1.Visible = false;

            // Refresh the dashboard data
            dashboard3.LoadIncomeAndExpenseData();
            dashboard3.displayExpenseData();

            DisplayIncomeData();
            DisplayCategories();
            DisplayExpenseData();
        }

        private void loanUI1_Load(object sender, EventArgs e)
        {

        }
    }
}
