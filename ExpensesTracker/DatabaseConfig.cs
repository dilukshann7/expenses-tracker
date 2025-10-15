using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace ExpensesTracker
{
    public static class DatabaseConfig
    {
        // Centralized connection string - change this one line to affect the entire application
        public static string ConnectionString => "Data Source=NITRO_5\\SQLEXPRESS;Initial Catalog=ExpenseTracker;Integrated Security=True";
        
        // Individual components for Crystal Reports and other uses
        public static string ServerName => @"NITRO_5\SQLEXPRESS";
        public static string DatabaseName => "ExpenseTracker";
        
        // Crystal Report Paths - Centralized and relative to application directory
        private static string GetReportPath(string reportFileName)
        {
            // Try different possible locations for the report files
            string appDirectory = Application.StartupPath;
            
            // First try: Same directory as executable
            string path1 = Path.Combine(appDirectory, reportFileName);
            if (File.Exists(path1)) return path1;
            
            // Second try: One level up from bin directory (for debug/release builds)
            string path2 = Path.Combine(Directory.GetParent(appDirectory).FullName, reportFileName);
            if (File.Exists(path2)) return path2;
            
            // Third try: Two levels up from bin directory (common structure)
            string path3 = Path.Combine(Directory.GetParent(Directory.GetParent(appDirectory).FullName).FullName, reportFileName);
            if (File.Exists(path3)) return path3;
            
            // Fourth try: In the project root directory
            string projectRoot = appDirectory;
            while (Directory.GetParent(projectRoot) != null)
            {
                string testPath = Path.Combine(projectRoot, reportFileName);
                if (File.Exists(testPath)) return testPath;
                projectRoot = Directory.GetParent(projectRoot).FullName;
            }
            
            // If not found, return the first path (will cause an error but with a clear location)
            return path1;
        }
        
        // Crystal Report file paths
        public static string ExpenseReportPath => GetReportPath("CrystalReport1.rpt");
        public static string IncomeReportPath => GetReportPath("CrystalReport2.rpt");
        public static string LoanReportPath => GetReportPath("CrystalReport3.rpt");
        public static string SecurityReportPath => GetReportPath("CrystalReport4.rpt");
        public static string OverallReportPath => GetReportPath("CrystalReport5.rpt");
        
    }
}