using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class OverallReport : Form
    {
        private int _userId;

        public OverallReport(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void OverallReport_Load(object sender, EventArgs e)
        {
            try
            {
                // Load Crystal Report (.rpt)
                ReportDocument report = new ReportDocument();
                report.Load(DatabaseConfig.OverallReportPath);

                // Set database login using centralized configuration
                report.SetDatabaseLogon("", "", DatabaseConfig.ServerName, DatabaseConfig.DatabaseName);

                // Pass user parameter
                report.SetParameterValue("UserId", _userId);

                // Set report source
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message + "\nReport Path: " + DatabaseConfig.OverallReportPath, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
