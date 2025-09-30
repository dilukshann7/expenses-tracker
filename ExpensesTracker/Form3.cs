using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class Form3 : Form
    {
        private int _userId;

        public Form3(int userId)
        {
            InitializeComponent();
            _userId = userId; // Pass the current logged-in user's ID
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDocument report = new ReportDocument();

                // Load Crystal Report file using centralized path
                report.Load(DatabaseConfig.ExpenseReportPath);

                // Set database login using centralized configuration
                report.SetDatabaseLogon("", "", DatabaseConfig.ServerName, DatabaseConfig.DatabaseName);

                // Set the UserId parameter in the report
                report.SetParameterValue("UserId", _userId);

                // Bind report to viewer
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message + "\nReport Path: " + DatabaseConfig.ExpenseReportPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
