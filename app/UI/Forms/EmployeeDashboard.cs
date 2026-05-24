using System;
using System.Windows.Forms;
using DevTrack.DataAccess.Models;
using DevTrack.UI.Utilities;

namespace DevTrack.UI.Forms
{
    public class EmployeeDashboard : Form
    {
        private EmployeeModel employee;

        public EmployeeDashboard(EmployeeModel emp)
        {
            employee = emp;
            this.Text = $"Employee Dashboard - {emp.Name}";
            this.Size = new System.Drawing.Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UITheme.BgDark;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Label lblWelcome = UITheme.MakeLabel($"Welcome, {employee.Name}", UITheme.FontHeading);
            lblWelcome.Location = new System.Drawing.Point(20, 20);
            this.Controls.Add(lblWelcome);

            Label lblInfo = UITheme.MakeLabel($"Role: {employee.Role} | Department: {employee.DeptName}", UITheme.FontBody);
            lblInfo.Location = new System.Drawing.Point(20, 60);
            this.Controls.Add(lblInfo);
        }
    }
}
