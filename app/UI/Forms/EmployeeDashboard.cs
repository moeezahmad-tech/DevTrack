using System;
using System.Drawing;
using System.Windows.Forms;
using DevTrack.DataAccess.Models;
using DevTrack.UI.Panels;
using DevTrack.UI.Utilities;

namespace DevTrack.UI.Forms
{
    public class EmployeeDashboard : Form
    {
        private EmployeeModel currentEmp;
        private Panel contentArea;
        private Label lblPageTitle;

        private Button btnHome;
        private Button btnMyTasks;
        private Button btnSubmitReport;
        private Button btnViewProjects;
        private Button btnLogout;

        public EmployeeDashboard(EmployeeModel emp)
        {
            currentEmp           = emp;
            this.Text            = "Employee Portal - " + emp.Name;
            this.Size            = new Size(1300, 800);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.BackColor       = UITheme.BgDark;
            BuildLayout();
            ShowHome();
        }

        private void BuildLayout()
        {
            // Sidebar
            Panel sidebar         = new Panel();
            sidebar.Size          = new Size(240, this.Height);
            sidebar.Location      = new Point(0, 0);
            sidebar.BackColor     = UITheme.BgDark;
            this.Controls.Add(sidebar);

            Panel topAccent       = new Panel();
            topAccent.Size        = new Size(240, 4);
            topAccent.Location    = new Point(0, 0);
            topAccent.BackColor   = UITheme.AccentTeal;
            sidebar.Controls.Add(topAccent);

            Panel logoArea        = new Panel();
            logoArea.Size         = new Size(240, 90);
            logoArea.Location     = new Point(0, 4);
            logoArea.BackColor    = Color.FromArgb(10, 20, 32);
            sidebar.Controls.Add(logoArea);

            Label lblLogo         = UITheme.MakeLabel("DevTrack", new Font("Segoe UI", 16, FontStyle.Bold), UITheme.AccentTeal);
            lblLogo.Location      = new Point(24, 20);
            logoArea.Controls.Add(lblLogo);

            Label lblPortal       = UITheme.MakeLabel("Employee Portal", UITheme.FontSmall, UITheme.TextMuted);
            lblPortal.Location    = new Point(24, 54);
            logoArea.Controls.Add(lblPortal);

            // Employee info card
            Panel empCard         = new Panel();
            empCard.Size          = new Size(210, 90);
            empCard.Location      = new Point(15, 104);
            empCard.BackColor     = UITheme.BgPanel;
            sidebar.Controls.Add(empCard);

            Label lblEmpName      = UITheme.MakeLabel(currentEmp.Name, UITheme.FontSubhead, UITheme.AccentGold);
            lblEmpName.Location   = new Point(12, 10);
            lblEmpName.MaximumSize = new Size(185, 0);
            empCard.Controls.Add(lblEmpName);

            Label lblRole         = UITheme.MakeLabel(currentEmp.Role, UITheme.FontSmall, UITheme.TextSilver);
            lblRole.Location      = new Point(12, 32);
            empCard.Controls.Add(lblRole);

            Label lblDept         = UITheme.MakeLabel("Dept: " + currentEmp.DeptName, UITheme.FontSmall, UITheme.TextMuted);
            lblDept.Location      = new Point(12, 52);
            empCard.Controls.Add(lblDept);

            Label lblID           = UITheme.MakeLabel("ID: " + currentEmp.ID, UITheme.FontSmall, UITheme.TextMuted);
            lblID.Location        = new Point(12, 70);
            empCard.Controls.Add(lblID);

            sidebar.Controls.Add(UITheme.MakeSeparator(20, 210, 200));

            Label lblNav          = UITheme.MakeLabel("NAVIGATION", UITheme.FontSmall, UITheme.TextMuted);
            lblNav.Location       = new Point(24, 222);
            sidebar.Controls.Add(lblNav);

            btnHome         = MakeNavBtn(sidebar, "🏠  Home",          240);
            btnMyTasks      = MakeNavBtn(sidebar, "✅  My Tasks",       282);
            btnSubmitReport = MakeNavBtn(sidebar, "📄  Submit Report",  324);
            btnViewProjects = MakeNavBtn(sidebar, "📋  View Projects",  366);

            btnHome.Click         += new EventHandler(OnHomeClick);
            btnMyTasks.Click      += new EventHandler(OnMyTasksClick);
            btnSubmitReport.Click += new EventHandler(OnSubmitReportClick);
            btnViewProjects.Click += new EventHandler(OnViewProjectsClick);

            sidebar.Controls.Add(UITheme.MakeSeparator(20, 420, 200));
            btnLogout              = MakeNavBtn(sidebar, "🚪  Logout", 432);
            btnLogout.ForeColor    = Color.FromArgb(248, 113, 113);
            btnLogout.Click       += new EventHandler(OnLogoutClick);

            // Top bar
            Panel topBar          = new Panel();
            topBar.Size           = new Size(this.Width - 240, 60);
            topBar.Location       = new Point(240, 0);
            topBar.BackColor      = UITheme.BgMid;
            this.Controls.Add(topBar);

            lblPageTitle          = UITheme.MakeLabel("Dashboard", UITheme.FontHeading, UITheme.TextWhite);
            lblPageTitle.Location = new Point(30, 16);
            topBar.Controls.Add(lblPageTitle);

            Label lblDate         = UITheme.MakeLabel(DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                                     UITheme.FontSmall, UITheme.TextMuted);
            lblDate.Location      = new Point(topBar.Width - 300, 20);
            lblDate.Size          = new Size(280, 20);
            lblDate.TextAlign     = ContentAlignment.MiddleRight;
            lblDate.AutoSize      = false;
            topBar.Controls.Add(lblDate);

            Panel barAccent       = new Panel();
            barAccent.Size        = new Size(topBar.Width, 2);
            barAccent.Location    = new Point(0, 58);
            barAccent.BackColor   = UITheme.AccentGold;
            topBar.Controls.Add(barAccent);

            // Content area
            contentArea           = new Panel();
            contentArea.Size      = new Size(this.Width - 240, this.Height - 60);
            contentArea.Location  = new Point(240, 60);
            contentArea.BackColor = UITheme.BgMid;
            contentArea.AutoScroll = true;
            this.Controls.Add(contentArea);
        }

        private Button MakeNavBtn(Panel sidebar, string text, int top)
        {
            Button btn          = new Button();
            btn.Text            = text;
            btn.Size            = new Size(210, 38);
            btn.Location        = new Point(15, top);
            btn.BackColor       = Color.Transparent;
            btn.ForeColor       = UITheme.TextSilver;
            btn.Font            = UITheme.FontNav;
            btn.FlatStyle       = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = UITheme.BgPanel;
            btn.TextAlign       = ContentAlignment.MiddleLeft;
            btn.Padding         = new Padding(14, 0, 0, 0);
            btn.Cursor          = Cursors.Hand;
            sidebar.Controls.Add(btn);
            return btn;
        }

        private void HighlightNav(Button active)
        {
            Button[] all = { btnHome, btnMyTasks, btnSubmitReport, btnViewProjects, btnLogout };
            foreach (Button b in all)
            {
                b.BackColor = Color.Transparent;
                b.ForeColor = UITheme.TextSilver;
            }
            active.BackColor = UITheme.BgPanel;
            active.ForeColor = UITheme.AccentGold;
        }

        private void LoadContent(Control ctrl)
        {
            contentArea.Controls.Clear();
            ctrl.Dock = DockStyle.Fill;
            contentArea.Controls.Add(ctrl);
        }

        private void ShowHome()
        {
            HighlightNav(btnHome);
            lblPageTitle.Text = "My Dashboard";
            LoadContent(new EmployeeHomePanel(currentEmp));
        }

        private void OnHomeClick(object s, EventArgs e)
        {
            ShowHome();
        }
        private void OnMyTasksClick(object s, EventArgs e)
        {
            HighlightNav(btnMyTasks);
            lblPageTitle.Text = "My Tasks";
            LoadContent(new MyTasksPanel(currentEmp.ID));
        }
        private void OnSubmitReportClick(object s, EventArgs e)
        {
            HighlightNav(btnSubmitReport);
            lblPageTitle.Text = "Submit Report";
            LoadContent(new ManageReportsPanel(false));
        }
        private void OnViewProjectsClick(object s, EventArgs e)
        {
            HighlightNav(btnViewProjects);
            lblPageTitle.Text = "Projects";
            LoadContent(new ViewProjectsPanel());
        }
        private void OnLogoutClick(object s, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // EmployeeDashboard
            //
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "EmployeeDashboard";
            this.Load += new System.EventHandler(this.EmployeeDashboard_Load);
            this.ResumeLayout(false);

        }

        private void EmployeeDashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
