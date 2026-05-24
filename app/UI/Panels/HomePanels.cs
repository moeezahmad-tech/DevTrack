using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevTrack.BusinessLogic.Managers;
using DevTrack.DataAccess.Models;
using DevTrack.UI.Utilities;

namespace DevTrack.UI.Panels
{
    // ── HOD Home Panel ────────────────────────────────────────────────────────
    public class HomePanel : Panel
    {
        HeadOfDeptModel hod;

        public HomePanel(HeadOfDeptModel hod)
        {
            this.hod       = hod;
            this.BackColor = UITheme.BgMid;
            this.Paint    += new PaintEventHandler(OnPaint);
            BuildUI();
        }

        private void OnPaint(object s, PaintEventArgs e)
        {
            UITheme.PaintGradientBg(s, e);
        }

        private void BuildUI()
        {
            // Welcome banner
            Label lblWelcome     = UITheme.MakeLabel("Welcome back, " + hod.Name + " 👋",
                                    new Font("Segoe UI", 20, FontStyle.Bold), UITheme.TextWhite);
            lblWelcome.Location  = new Point(40, 40);
            this.Controls.Add(lblWelcome);

            Label lblSub         = UITheme.MakeLabel("Here is a summary of your office today - " + DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                                    UITheme.FontBody, UITheme.TextMuted);
            lblSub.Location      = new Point(40, 78);
            this.Controls.Add(lblSub);

            this.Controls.Add(UITheme.MakeSeparator(40, 108, 860));

            // Stat cards row
            try
            {
                int empCount  = new EmployeeManager().GetAll().Count;
                int projCount = new ProjectManager().GetAll().Count;
                int taskCount = new TaskManager().GetAll().Count;
                int repCount  = new ReportManager().GetAll().Count;

                AddStatCard("👤 Employees",  empCount.ToString(),  UITheme.AccentTeal,  60,  140);
                AddStatCard("📋 Projects",   projCount.ToString(), UITheme.AccentGold,  260, 140);
                AddStatCard("✅ Tasks",       taskCount.ToString(), UITheme.BtnBlue,     460, 140);
                AddStatCard("📄 Reports",    repCount.ToString(),  UITheme.BtnSuccess,  660, 140);
            }
            catch { }

            // Info block
            Panel infoCard        = UITheme.MakeCard(60, 310, 860, 130);
            this.Controls.Add(infoCard);

            Label lblInfoHead     = UITheme.MakeLabel("Your Profile", UITheme.FontSubhead, UITheme.AccentGold);
            lblInfoHead.Location  = new Point(24, 16);
            infoCard.Controls.Add(lblInfoHead);

            infoCard.Controls.Add(UITheme.MakeSeparator(0, 44, 860));

            string[] keys   = { "Full Name", "Email", "Department", "Office No", "Role" };
            string[] values = { hod.Name, hod.Email, hod.DeptName, hod.OfficeNo, "Head of Department" };

            for (int i = 0; i < keys.Length; i++)
            {
                int col = (i < 3) ? 0 : 430;
                int row = (i < 3) ? i : (i - 3);

                Label lKey   = UITheme.MakeLabel(keys[i] + ":", UITheme.FontSmall, UITheme.TextMuted);
                lKey.Location = new Point(24 + col, 56 + row * 22);
                infoCard.Controls.Add(lKey);

                Label lVal   = UITheme.MakeLabel(values[i], UITheme.FontSmall, UITheme.TextWhite);
                lVal.Location = new Point(140 + col, 56 + row * 22);
                infoCard.Controls.Add(lVal);
            }

            // Quick tip
            Panel tipCard        = UITheme.MakeCard(60, 460, 860, 60);
            this.Controls.Add(tipCard);

            Label lblTip         = UITheme.MakeLabel(
                "💡  Tip: Use the Employees menu to register new staff, and Tasks to assign work to team members.",
                UITheme.FontBody, UITheme.TextSilver);
            lblTip.Location      = new Point(20, 18);
            tipCard.Controls.Add(lblTip);
        }

        private void AddStatCard(string label, string value, Color accent, int x, int y)
        {
            Panel card        = UITheme.MakeCard(x, y, 170, 100);
            card.BackColor    = UITheme.BgPanel;

            Panel stripe      = new Panel();
            stripe.Size       = new Size(4, 100);
            stripe.Location   = new Point(0, 0);
            stripe.BackColor  = accent;
            card.Controls.Add(stripe);

            Label lblVal      = new Label();
            lblVal.Text       = value;
            lblVal.Font       = new Font("Segoe UI", 28, FontStyle.Bold);
            lblVal.ForeColor  = accent;
            lblVal.AutoSize   = true;
            lblVal.Location   = new Point(20, 14);
            card.Controls.Add(lblVal);

            Label lblLbl      = UITheme.MakeLabel(label, UITheme.FontSmall, UITheme.TextMuted);
            lblLbl.Location   = new Point(20, 68);
            card.Controls.Add(lblLbl);

            this.Controls.Add(card);
        }
    }

    // ── Employee Home Panel ───────────────────────────────────────────────────
    public class EmployeeHomePanel : Panel
    {
        EmployeeModel emp;

        public EmployeeHomePanel(EmployeeModel emp)
        {
            this.emp       = emp;
            this.BackColor = UITheme.BgMid;
            this.Paint    += new PaintEventHandler(OnPaint);
            BuildUI();
        }

        private void OnPaint(object s, PaintEventArgs e)
        {
            UITheme.PaintGradientBg(s, e);
        }

        private void BuildUI()
        {
            Label lblWelcome    = UITheme.MakeLabel("Hello, " + emp.Name + " 👋",
                                   new Font("Segoe UI", 20, FontStyle.Bold), UITheme.TextWhite);
            lblWelcome.Location = new Point(40, 40);
            this.Controls.Add(lblWelcome);

            Label lblSub        = UITheme.MakeLabel(emp.Role + "  ·  " + emp.DeptName + "  ·  " + DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                                   UITheme.FontBody, UITheme.TextMuted);
            lblSub.Location     = new Point(40, 78);
            this.Controls.Add(lblSub);

            this.Controls.Add(UITheme.MakeSeparator(40, 108, 860));

            // My Tasks count
            try
            {
                List<TaskItemModel> tasks = new TaskManager().GetByEmployee(emp.ID);
                AddStatCard("✅ My Tasks",   tasks.Count.ToString(), UITheme.AccentTeal, 60,  140);
            }
            catch { }

            // Profile card
            Panel card        = UITheme.MakeCard(60, 280, 860, 130);
            this.Controls.Add(card);

            Label lblHead     = UITheme.MakeLabel("My Profile", UITheme.FontSubhead, UITheme.AccentGold);
            lblHead.Location  = new Point(24, 16);
            card.Controls.Add(lblHead);

            card.Controls.Add(UITheme.MakeSeparator(0, 44, 860));

            string[] keys   = { "Employee ID", "Full Name", "Email", "Role", "Department" };
            string[] values = { emp.ID.ToString(), emp.Name, emp.Email, emp.Role, emp.DeptName };
            for (int i = 0; i < keys.Length; i++)
            {
                int col = (i < 3) ? 0 : 430;
                int row = (i < 3) ? i : (i - 3);
                Label lk = UITheme.MakeLabel(keys[i] + ":", UITheme.FontSmall, UITheme.TextMuted);
                lk.Location = new Point(24 + col, 56 + row * 22);
                card.Controls.Add(lk);
                Label lv = UITheme.MakeLabel(values[i], UITheme.FontSmall, UITheme.TextWhite);
                lv.Location = new Point(150 + col, 56 + row * 22);
                card.Controls.Add(lv);
            }

            Panel tipCard     = UITheme.MakeCard(60, 430, 860, 60);
            this.Controls.Add(tipCard);
            Label lblTip      = UITheme.MakeLabel("💡  Use My Tasks to view assigned work and Submit Report to send updates to your HOD.",
                                 UITheme.FontBody, UITheme.TextSilver);
            lblTip.Location   = new Point(20, 18);
            tipCard.Controls.Add(lblTip);
        }

        private void AddStatCard(string label, string value, Color accent, int x, int y)
        {
            Panel card       = UITheme.MakeCard(x, y, 170, 100);
            Panel stripe     = new Panel();
            stripe.Size      = new Size(4, 100);
            stripe.Location  = new Point(0, 0);
            stripe.BackColor = accent;
            card.Controls.Add(stripe);
            Label lblVal     = new Label();
            lblVal.Text      = value;
            lblVal.Font      = new Font("Segoe UI", 28, FontStyle.Bold);
            lblVal.ForeColor = accent;
            lblVal.AutoSize  = true;
            lblVal.Location  = new Point(20, 14);
            card.Controls.Add(lblVal);
            Label lblLbl     = UITheme.MakeLabel(label, UITheme.FontSmall, UITheme.TextMuted);
            lblLbl.Location  = new Point(20, 68);
            card.Controls.Add(lblLbl);
            this.Controls.Add(card);
        }
    }

    // ── Employee View Projects (read-only) ────────────────────────────────────
    public class ViewProjectsPanel : Panel
    {
        public ViewProjectsPanel()
        {
            this.BackColor = UITheme.BgMid;
            BuildUI();
        }

        private void BuildUI()
        {
            DataGridView grid = UITheme.MakeGrid();
            grid.Location     = new Point(30, 30);
            grid.Size         = new Size(890, 500);

            try
            {
                List<ProjectModel> list = new ProjectManager().GetAll();
                grid.Columns.Add("ID",     "Project ID");
                grid.Columns.Add("Name",   "Project Name");
                grid.Columns.Add("Dept",   "Department");
                grid.Columns.Add("Start",  "Start Date");
                grid.Columns.Add("Status", "Status");
                foreach (ProjectModel p in list)
                    grid.Rows.Add(p.ProjectID, p.ProjectName, p.DeptName,
                                  p.StartDate.ToString("dd MMM yyyy"), p.Status);
            }
            catch (Exception ex)
            {
                grid.Columns.Add("e", "Error");
                grid.Rows.Add(ex.Message);
            }
            this.Controls.Add(grid);
        }
    }

    // ── Employee My Tasks (read-only) ─────────────────────────────────────────
    public class MyTasksPanel : Panel
    {
        int empID;

        public MyTasksPanel(int empID)
        {
            this.empID     = empID;
            this.BackColor = UITheme.BgMid;
            BuildUI();
        }

        private void BuildUI()
        {
            Label lbl     = UITheme.MakeLabel("Tasks Assigned to Me", UITheme.FontHeading, UITheme.AccentGold);
            lbl.Location  = new Point(30, 20);
            this.Controls.Add(lbl);

            DataGridView grid = UITheme.MakeGrid();
            grid.Location     = new Point(30, 60);
            grid.Size         = new Size(890, 480);

            try
            {
                List<TaskItemModel> list = new TaskManager().GetByEmployee(empID);
                grid.Columns.Add("TID",   "Task ID");
                grid.Columns.Add("Title", "Title");
                grid.Columns.Add("Prio",  "Priority");
                grid.Columns.Add("DL",    "Deadline");
                grid.Columns.Add("Proj",  "Project");
                grid.Columns.Add("Desc",  "Description");
                foreach (TaskItemModel t in list)
                    grid.Rows.Add(t.TaskID, t.Title, t.Priority,
                                  t.Deadline.ToString("dd MMM yyyy"), t.ProjectName, t.TaskDescription);
            }
            catch (Exception ex)
            {
                grid.Columns.Add("e", "Error");
                grid.Rows.Add(ex.Message);
            }
            this.Controls.Add(grid);
        }
    }
}
