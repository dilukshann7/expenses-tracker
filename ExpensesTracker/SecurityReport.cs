using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class SecurityReport : Form
    {
        public SecurityReport()
        {
            InitializeComponent();
        }

        private void SecurityReport_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDocument report = new ReportDocument();
                
                // Load Crystal Report file using centralized path
                report.Load(DatabaseConfig.SecurityReportPath);
                
                // Set database login using centralized configuration  
                report.SetDatabaseLogon("", "", DatabaseConfig.ServerName, DatabaseConfig.DatabaseName);
                
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message + "\nReport Path: " + DatabaseConfig.SecurityReportPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
