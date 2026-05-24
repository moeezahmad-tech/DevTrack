using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevTrack.BusinessLogic.Managers;
using DevTrack.DataAccess.Models;
using DevTrack.UI.Utilities;

namespace DevTrack.UI.Panels
{
    public class ManageTasksPanel : Panel
    {
        private DataGridView grid;
        private TextBox      txtTaskID, txtTitle, txtDesc;
        private ComboBox     cbPriority, cbProject, cbEmployee;
        private DateTimePicker dtpDeadline;
        private Label        lblMsg;
        private Button       btnAdd, btnUpdate, btnDelete, btnClear;

        private TaskManager     bll     = new TaskManager();
        private ProjectManager  projBll = new ProjectManager();
        private EmployeeManager empBll  = new EmployeeManager();

        public ManageTasksPanel()
        {
            this.BackColor = UITheme.BgMid;
            BuildUI();
            LoadCombos();
            LoadGrid();
        }

        private void BuildUI()
        {
            Label lbl     = UITheme.MakeLabel("Manage Tasks", UITheme.FontHeading, UITheme.AccentGold);
            lbl.Location  = new Point(30, 18);
            this.Controls.Add(lbl);

            Panel card    = UITheme.MakeCard(30, 58, 920, 270);
            this.Controls.Add(card);

            int c1 = 20, c2 = 260, c3 = 540, c4 = 740;
            int r1 = 16, r2 = 68, r3 = 120, r4 = 174, r5 = 220;

            // Row 1
            AL(card, "Task ID *",    c1, r1); txtTaskID = ATB(card, c1, r1+18, 210);
            AL(card, "Title *",      c2, r1); txtTitle  = ATB(card, c2, r1+18, 250);
            AL(card, "Priority *",   c3, r1);
            cbPriority = UITheme.MakeComboBox();
            cbPriority.Location = new Point(c3, r1+18); cbPriority.Width = 180;
            cbPriority.Items.AddRange(new string[] { "Low", "Medium", "High", "Critical" });
            cbPriority.SelectedIndex = 1;
            card.Controls.Add(cbPriority);

            // Row 2
            AL(card, "Deadline *",   c1, r2);
            dtpDeadline = UITheme.MakeDatePicker();
            dtpDeadline.Location = new Point(c1, r2+18); dtpDeadline.Width = 210;
            card.Controls.Add(dtpDeadline);

            AL(card, "Project",      c2, r2);
            cbProject = UITheme.MakeComboBox();
            cbProject.Location = new Point(c2, r2+18); cbProject.Width = 250;
            card.Controls.Add(cbProject);

            AL(card, "Assign To Employee", c3, r2);
            cbEmployee = UITheme.MakeComboBox();
            cbEmployee.Location = new Point(c3, r2+18); cbEmployee.Width = 350;
            card.Controls.Add(cbEmployee);

            // Row 3 - Description
            AL(card, "Task Description", c1, r3);
            txtDesc = new TextBox();
            txtDesc.Multiline   = true;
            txtDesc.BackColor   = UITheme.BgCard;
            txtDesc.ForeColor   = UITheme.TextWhite;
            txtDesc.Font        = UITheme.FontBody;
            txtDesc.BorderStyle = BorderStyle.FixedSingle;
            txtDesc.Location    = new Point(c1, r3+18);
            txtDesc.Size        = new Size(880, 46);
            card.Controls.Add(txtDesc);

            // Message
            lblMsg          = UITheme.MakeLabel("", UITheme.FontSmall, UITheme.AccentTeal);
            lblMsg.Location = new Point(c1, r4 + 4);
            lblMsg.Size     = new Size(550, 20);
            lblMsg.AutoSize = false;
            card.Controls.Add(lblMsg);

            // Buttons
            btnAdd    = UITheme.MakeButton("➕ Add",    UITheme.BtnSuccess);
            btnUpdate = UITheme.MakeButton("✏  Update", UITheme.BtnBlue);
            btnDelete = UITheme.MakeButton("🗑  Delete", UITheme.BtnDanger);
            btnClear  = UITheme.MakeButton("✖  Clear",  UITheme.BgCard);

            int bw = 155;
            btnAdd.Size  = btnUpdate.Size = btnDelete.Size = btnClear.Size = new Size(bw, 38);
            btnAdd.Location    = new Point(c1,       r5);
            btnUpdate.Location = new Point(c1 + 165, r5);
            btnDelete.Location = new Point(c1 + 330, r5);
            btnClear.Location  = new Point(c1 + 495, r5);

            btnAdd.Click    += new EventHandler(OnAdd);
            btnUpdate.Click += new EventHandler(OnUpdate);
            btnDelete.Click += new EventHandler(OnDelete);
            btnClear.Click  += new EventHandler(OnClear);

            card.Controls.Add(btnAdd);
            card.Controls.Add(btnUpdate);
            card.Controls.Add(btnDelete);
            card.Controls.Add(btnClear);

            // Grid
            grid           = UITheme.MakeGrid();
            grid.Location  = new Point(30, 340);
            grid.Size      = new Size(920, 340);
            grid.CellClick += new DataGridViewCellEventHandler(OnGridClick);
            this.Controls.Add(grid);
        }

        private void AL(Panel p, string t, int x, int y)
        {
            Label l = UITheme.MakeLabel(t, UITheme.FontSmall, UITheme.TextMuted);
            l.Location = new Point(x, y); p.Controls.Add(l);
        }
        private TextBox ATB(Panel p, int x, int y, int w)
        {
            TextBox tb = UITheme.MakeTextBox();
            tb.Location = new Point(x, y); tb.Width = w;
            p.Controls.Add(tb); return tb;
        }

        private void LoadCombos()
        {
            // Projects
            cbProject.Items.Clear();
            cbProject.Items.Add("(None)");
            try
            {
                foreach (ProjectModel p in projBll.GetAll())
                    cbProject.Items.Add(p.ProjectID + " | " + p.ProjectName);
            }
            catch { }
            cbProject.SelectedIndex = 0;

            // Employees
            cbEmployee.Items.Clear();
            cbEmployee.Items.Add("(None)");
            try
            {
                foreach (EmployeeModel e in empBll.GetAll())
                    cbEmployee.Items.Add(e.ID + " | " + e.Name + " (" + e.Role + ")");
            }
            catch { }
            cbEmployee.SelectedIndex = 0;
        }

        private void LoadGrid()
        {
            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.Columns.Add("ID",   "Task ID");
            grid.Columns.Add("Title","Title");
            grid.Columns.Add("Prio", "Priority");
            grid.Columns.Add("DL",   "Deadline");
            grid.Columns.Add("Proj", "Project");
            grid.Columns.Add("Emp",  "Assigned To");
            try
            {
                foreach (TaskItemModel t in bll.GetAll())
                    grid.Rows.Add(t.TaskID, t.Title, t.Priority,
                                  t.Deadline.ToString("dd MMM yyyy"), t.ProjectName, t.EmployeeName);
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnGridClick(object s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            txtTaskID.Text     = row.Cells["ID"].Value.ToString();
            txtTitle.Text      = row.Cells["Title"].Value.ToString();
            txtTaskID.ReadOnly = true;

            string prio = row.Cells["Prio"].Value.ToString();
            for (int i = 0; i < cbPriority.Items.Count; i++)
                if (cbPriority.Items[i].ToString() == prio) { cbPriority.SelectedIndex = i; break; }
        }

        private void OnAdd(object s, EventArgs e)
        {
            try
            {
                TaskItemModel tm = BuildModel();
                bll.Add(tm);
                ShowMsg("✓  Task added.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnUpdate(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskID.Text)) { ShowMsg("✗  Select a task first.", false); return; }
            try
            {
                TaskItemModel tm = BuildModel();
                bll.Update(tm);
                ShowMsg("✓  Task updated.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnDelete(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskID.Text)) { ShowMsg("✗  Select a task first.", false); return; }
            if (MessageBox.Show("Delete task " + txtTaskID.Text + "?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                bll.Delete(txtTaskID.Text.Trim());
                ShowMsg("✓  Task deleted.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnClear(object s, EventArgs e) { ClearFields(); lblMsg.Text = ""; }

        private TaskItemModel BuildModel()
        {
            TaskItemModel tm = new TaskItemModel();
            tm.TaskID          = txtTaskID.Text.Trim();
            tm.Title           = txtTitle.Text.Trim();
            tm.TaskDescription = txtDesc.Text.Trim();
            tm.Priority        = cbPriority.SelectedItem.ToString();
            tm.Deadline        = dtpDeadline.Value;
            tm.ProjectID       = GetProjectID();
            tm.EmployeeID      = GetEmployeeID();
            return tm;
        }

        private string GetProjectID()
        {
            if (cbProject.SelectedIndex <= 0) return "";
            return cbProject.SelectedItem.ToString().Split('|')[0].Trim();
        }

        private int GetEmployeeID()
        {
            if (cbEmployee.SelectedIndex <= 0) return 0;
            string raw = cbEmployee.SelectedItem.ToString().Split('|')[0].Trim();
            int id = 0;
            int.TryParse(raw, out id);
            return id;
        }

        private void ClearFields()
        {
            txtTaskID.Text     = "";
            txtTitle.Text      = "";
            txtDesc.Text       = "";
            txtTaskID.ReadOnly = false;
            cbPriority.SelectedIndex = 1;
            cbProject.SelectedIndex  = 0;
            cbEmployee.SelectedIndex = 0;
        }

        private void ShowMsg(string text, bool ok)
        {
            lblMsg.Text      = text;
            lblMsg.ForeColor = ok ? UITheme.AccentTeal : Color.FromArgb(239, 68, 68);
        }
    }
}
