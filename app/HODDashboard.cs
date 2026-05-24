using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CompanyManagement.Models;

namespace CompanyManagement.Forms
{
    public class HODDashboard : Form
    {
        private HeadOfDeptModel currentHOD;
        private Panel    sidebar;
        private Panel    contentArea;
        private Label    lblPageTitle;
        private Button   btnActive;   // track highlighted nav button

        // Nav buttons
        private Button btnHome;
        private Button btnDepts;
        private Button btnEmployees;
        private Button btnProjects;
        private Button btnTasks;
        private Button btnReports;
        private Button btnLogout;

        public HODDashboard(HeadOfDeptModel hod)
        {
            currentHOD           = hod;
            this.Text            = "HOD Dashboard — " + hod.Name;
            this.Size            = new Size(1300, 800);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.BackColor       = UITheme.BgDark;
            BuildLayout();
            ShowHome();
        }

        // ── Layout shell ──────────────────────────────────────────────────────
        private void BuildLayout()
        {
            // ─ Sidebar ────────────────────────────────────────────────────────
            sidebar              = new Panel();
            sidebar.Size         = new Size(240, this.Height);
            sidebar.Location     = new Point(0, 0);
            sidebar.BackColor    = UITheme.BgDark;
            this.Controls.Add(sidebar);

            // Gold top accent
            Panel topAccent      = new Panel();
            topAccent.Size       = new Size(240, 4);
            topAccent.Location   = new Point(0, 0);
            topAccent.BackColor  = UITheme.AccentGold;
            sidebar.Controls.Add(topAccent);

            // Company logo area
            Panel logoArea       = new Panel();
            logoArea.Size        = new Size(240, 90);
            logoArea.Location    = new Point(0, 4);
            logoArea.BackColor   = Color.FromArgb(10, 20, 32);
            sidebar.Controls.Add(logoArea);

            Label lblLogo        = UITheme.MakeLabel("DevTrack", new Font("Segoe UI", 16, FontStyle.Bold), UITheme.AccentGold);
            lblLogo.Location     = new Point(24, 20);
            logoArea.Controls.Add(lblLogo);

            Label lblPortal      = UITheme.MakeLabel("Management Portal", UITheme.FontSmall, UITheme.TextMuted);
            lblPortal.Location   = new Point(24, 54);
            logoArea.Controls.Add(lblPortal);

            // HOD info card
            Panel hodCard        = new Panel();
            hodCard.Size         = new Size(210, 80);
            hodCard.Location     = new Point(15, 104);
            hodCard.BackColor    = UITheme.BgPanel;
            sidebar.Controls.Add(hodCard);

            Label lblHodName     = UITheme.MakeLabel(currentHOD.Name, UITheme.FontSubhead, UITheme.AccentTeal);
            lblHodName.Location  = new Point(12, 10);
            lblHodName.MaximumSize = new Size(185, 0);
            hodCard.Controls.Add(lblHodName);

            Label lblHodRole     = UITheme.MakeLabel("Head of Department", UITheme.FontSmall, UITheme.TextMuted);
            lblHodRole.Location  = new Point(12, 32);
            hodCard.Controls.Add(lblHodRole);

            Label lblHodDept     = UITheme.MakeLabel("Dept: " + currentHOD.DeptName, UITheme.FontSmall, UITheme.TextSilver);
            lblHodDept.Location  = new Point(12, 52);
            hodCard.Controls.Add(lblHodDept);

            // Separator
            sidebar.Controls.Add(UITheme.MakeSeparator(20, 200, 200));

            Label lblNav         = UITheme.MakeLabel("NAVIGATION", UITheme.FontSmall, UITheme.TextMuted);
            lblNav.Location      = new Point(24, 212);
            sidebar.Controls.Add(lblNav);

            // Nav buttons
            btnHome      = MakeNavBtn("🏠  Home",       230);
            btnDepts     = MakeNavBtn("🏢  Departments", 272);
            btnEmployees = MakeNavBtn("👤  Employees",   314);
            btnProjects  = MakeNavBtn("📋  Projects",    356);
            btnTasks     = MakeNavBtn("✅  Tasks",        398);
            btnReports   = MakeNavBtn("📄  Reports",     440);

            btnHome.Click      += new EventHandler(OnHomeClick);
            btnDepts.Click     += new EventHandler(OnDeptsClick);
            btnEmployees.Click += new EventHandler(OnEmployeesClick);
            btnProjects.Click  += new EventHandler(OnProjectsClick);
            btnTasks.Click     += new EventHandler(OnTasksClick);
            btnReports.Click   += new EventHandler(OnReportsClick);

            // Logout at bottom
            sidebar.Controls.Add(UITheme.MakeSeparator(20, 508, 200));
            btnLogout          = MakeNavBtn("🚪  Logout",   520);
            btnLogout.ForeColor = Color.FromArgb(248, 113, 113);
            btnLogout.Click   += new EventHandler(OnLogoutClick);

            // ─ Top bar ────────────────────────────────────────────────────────
            Panel topBar         = new Panel();
            topBar.Size          = new Size(this.Width - 240, 60);
            topBar.Location      = new Point(240, 0);
            topBar.BackColor     = UITheme.BgMid;
            this.Controls.Add(topBar);

            lblPageTitle         = UITheme.MakeLabel("Dashboard", UITheme.FontHeading, UITheme.TextWhite);
            lblPageTitle.Location = new Point(30, 16);
            topBar.Controls.Add(lblPageTitle);

            Label lblRight       = UITheme.MakeLabel("Office No: " + currentHOD.OfficeNo + "   |   " + DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                                    UITheme.FontSmall, UITheme.TextMuted);
            lblRight.Location    = new Point(topBar.Width - 460, 20);
            lblRight.AutoSize    = false;
            lblRight.Size        = new Size(440, 20);
            lblRight.TextAlign   = ContentAlignment.MiddleRight;
            topBar.Controls.Add(lblRight);

            // Teal bottom accent on topbar
            Panel barAccent      = new Panel();
            barAccent.Size       = new Size(topBar.Width, 2);
            barAccent.Location   = new Point(0, 58);
            barAccent.BackColor  = UITheme.AccentTeal;
            topBar.Controls.Add(barAccent);

            // ─ Content Area ───────────────────────────────────────────────────
            contentArea          = new Panel();
            contentArea.Size     = new Size(this.Width - 240, this.Height - 60);
            contentArea.Location = new Point(240, 60);
            contentArea.BackColor = UITheme.BgMid;
            contentArea.AutoScroll = true;
            this.Controls.Add(contentArea);
        }

        private Button MakeNavBtn(string text, int top)
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
            btn.Cursor          = Cursors.Hand;
            btn.Padding         = new Padding(14, 0, 0, 0);
            sidebar.Controls.Add(btn);
            return btn;
        }

        private void HighlightNav(Button btn)
        {
            Button[] all = { btnHome, btnDepts, btnEmployees, btnProjects, btnTasks, btnReports, btnLogout };
            foreach (Button b in all)
            {
                b.BackColor = Color.Transparent;
                b.ForeColor = UITheme.TextSilver;
            }
            btn.BackColor = UITheme.BgPanel;
            btn.ForeColor = UITheme.AccentTeal;
            btnActive = btn;
        }

        private void LoadContent(Control ctrl)
        {
            contentArea.Controls.Clear();
            ctrl.Dock = DockStyle.Fill;
            contentArea.Controls.Add(ctrl);
        }

        // ── Nav handlers ──────────────────────────────────────────────────────
        private void ShowHome() { HighlightNav(btnHome); lblPageTitle.Text = "Dashboard"; LoadContent(new HomePanel(currentHOD)); }
        private void OnHomeClick(object s, EventArgs e)      { ShowHome(); }
        private void OnDeptsClick(object s, EventArgs e)     { HighlightNav(btnDepts);     lblPageTitle.Text = "Departments"; LoadContent(new ManageDepartmentsPanel()); }
        private void OnEmployeesClick(object s, EventArgs e) { HighlightNav(btnEmployees); lblPageTitle.Text = "Employees";   LoadContent(new ManageEmployeesPanel()); }
        private void OnProjectsClick(object s, EventArgs e)  { HighlightNav(btnProjects);  lblPageTitle.Text = "Projects";   LoadContent(new ManageProjectsPanel()); }
        private void OnTasksClick(object s, EventArgs e)     { HighlightNav(btnTasks);     lblPageTitle.Text = "Tasks";      LoadContent(new ManageTasksPanel()); }
        private void OnReportsClick(object s, EventArgs e)   { HighlightNav(btnReports);   lblPageTitle.Text = "Reports";    LoadContent(new ManageReportsPanel(true)); }
        private void OnLogoutClick(object s, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Are you sure you want to logout?",
                "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
                this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HODDashboard
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "HODDashboard";
            this.Load += new System.EventHandler(this.HODDashboard_Load);
            this.ResumeLayout(false);

        }

        private void HODDashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
