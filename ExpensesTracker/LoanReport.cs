using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public partial class LoanReport : Form
    {
        private int _loanId;

        public LoanReport(int loanId )
        {
            InitializeComponent();
            _loanId = loanId;
        }

        private void LoanReport_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();

                // Load Crystal Report file using centralized path
                rptDoc.Load(DatabaseConfig.LoanReportPath);

                // Set database login using centralized configuration
                rptDoc.SetDatabaseLogon("", "", DatabaseConfig.ServerName, DatabaseConfig.DatabaseName);

                // Pass parameter to report
                rptDoc.SetParameterValue("LoanId", _loanId);

                crystalReportViewer1.ReportSource = rptDoc;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message + "\nReport Path: " + DatabaseConfig.LoanReportPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
