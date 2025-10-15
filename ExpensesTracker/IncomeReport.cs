using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class IncomeReport : Form
    {
        private int _userId;

        public IncomeReport(int userId)
        {
            InitializeComponent();
            _userId = userId; // Logged-in user's ID
        }

        private void IncomeReport_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDocument report = new ReportDocument();

                // Load Crystal Report file using centralized path
                report.Load(DatabaseConfig.IncomeReportPath);

                // Set Database Login using centralized configuration
                report.SetDatabaseLogon("", "", DatabaseConfig.ServerName, DatabaseConfig.DatabaseName);

                // Set UserId parameter
                report.SetParameterValue("UserId", _userId);

                // Bind to viewer
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message + "\nReport Path: " + DatabaseConfig.IncomeReportPath);
            }
        }
    }
}
