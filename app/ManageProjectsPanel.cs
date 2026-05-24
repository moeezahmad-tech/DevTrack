using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CompanyManagement.BLL;
using CompanyManagement.Models;

namespace CompanyManagement.Forms
{
    public class ManageProjectsPanel : Panel
    {
        private DataGridView grid;
        private TextBox      txtProjID, txtProjName;
        private ComboBox     cbDept, cbStatus;
        private DateTimePicker dtpStart;
        private Label        lblMsg;
        private Button       btnAdd, btnUpdate, btnDelete, btnClear;

        private ProjectBLL    bll     = new ProjectBLL();
        private DepartmentBLL deptBll = new DepartmentBLL();

        public ManageProjectsPanel()
        {
            this.BackColor = UITheme.BgMid;
            BuildUI();
            LoadDepts();
            LoadGrid();
        }

        private void BuildUI()
        {
            Label lbl     = UITheme.MakeLabel("Manage Projects", UITheme.FontHeading, UITheme.AccentGold);
            lbl.Location  = new Point(30, 18);
            this.Controls.Add(lbl);

            Panel card    = UITheme.MakeCard(30, 58, 920, 220);
            this.Controls.Add(card);

            int row1 = 16, row2 = 68, row3 = 120, row4 = 172;
            int c1 = 20, c2 = 280, c3 = 540, c4 = 740;

            // Row 1
            AddLabel(card, "Project ID *",   c1, row1);
            txtProjID     = AddTB(card, c1, row1 + 18, 230);

            AddLabel(card, "Project Name *", c2, row1);
            txtProjName   = AddTB(card, c2, row1 + 18, 230);

            AddLabel(card, "Department",     c3, row1);
            cbDept        = UITheme.MakeComboBox();
            cbDept.Location = new Point(c3, row1 + 18);
            cbDept.Width  = 340;
            card.Controls.Add(cbDept);

            // Row 2
            AddLabel(card, "Start Date *",   c1, row2);
            dtpStart      = UITheme.MakeDatePicker();
            dtpStart.Location = new Point(c1, row2 + 18);
            dtpStart.Width = 230;
            card.Controls.Add(dtpStart);

            AddLabel(card, "Status *",       c2, row2);
            cbStatus      = UITheme.MakeComboBox();
            cbStatus.Location = new Point(c2, row2 + 18);
            cbStatus.Width = 230;
            cbStatus.Items.AddRange(new string[] { "Planning", "Active", "On Hold", "Completed", "Cancelled" });
            cbStatus.SelectedIndex = 0;
            card.Controls.Add(cbStatus);

            // Message
            lblMsg          = UITheme.MakeLabel("", UITheme.FontSmall, UITheme.AccentTeal);
            lblMsg.Location = new Point(c1, row3);
            lblMsg.Size     = new Size(600, 20);
            lblMsg.AutoSize = false;
            card.Controls.Add(lblMsg);

            // Buttons
            btnAdd    = UITheme.MakeButton("➕ Add",    UITheme.BtnSuccess);
            btnUpdate = UITheme.MakeButton("✏  Update", UITheme.BtnBlue);
            btnDelete = UITheme.MakeButton("🗑  Delete", UITheme.BtnDanger);
            btnClear  = UITheme.MakeButton("✖  Clear",  UITheme.BgCard);

            int bw = 160;
            btnAdd.Size  = btnUpdate.Size = btnDelete.Size = btnClear.Size = new Size(bw, 38);
            btnAdd.Location    = new Point(c1,       row4);
            btnUpdate.Location = new Point(c1 + 170, row4);
            btnDelete.Location = new Point(c1 + 340, row4);
            btnClear.Location  = new Point(c1 + 510, row4);

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
            grid.Location  = new Point(30, 290);
            grid.Size      = new Size(920, 380);
            grid.CellClick += new DataGridViewCellEventHandler(OnGridClick);
            this.Controls.Add(grid);
        }

        private void AddLabel(Panel p, string t, int x, int y)
        {
            Label l    = UITheme.MakeLabel(t, UITheme.FontSmall, UITheme.TextMuted);
            l.Location = new Point(x, y);
            p.Controls.Add(l);
        }

        private TextBox AddTB(Panel p, int x, int y, int w)
        {
            TextBox tb = UITheme.MakeTextBox();
            tb.Location = new Point(x, y);
            tb.Width    = w;
            p.Controls.Add(tb);
            return tb;
        }

        private void LoadDepts()
        {
            cbDept.Items.Clear();
            cbDept.Items.Add("(None)");
            try
            {
                foreach (DepartmentModel d in deptBll.GetAll())
                    cbDept.Items.Add(d.DeptID + " — " + d.DeptName);
            }
            catch { }
            cbDept.SelectedIndex = 0;
        }

        private void LoadGrid()
        {
            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.Columns.Add("ID",     "Project ID");
            grid.Columns.Add("Name",   "Name");
            grid.Columns.Add("Dept",   "Department");
            grid.Columns.Add("Start",  "Start Date");
            grid.Columns.Add("Status", "Status");
            try
            {
                foreach (ProjectModel p in bll.GetAll())
                    grid.Rows.Add(p.ProjectID, p.ProjectName, p.DeptName,
                                  p.StartDate.ToString("dd MMM yyyy"), p.Status);
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnGridClick(object s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            txtProjID.Text     = row.Cells["ID"].Value.ToString();
            txtProjName.Text   = row.Cells["Name"].Value.ToString();
            txtProjID.ReadOnly = true;
            // Set status combo
            string st = row.Cells["Status"].Value.ToString();
            for (int i = 0; i < cbStatus.Items.Count; i++)
                if (cbStatus.Items[i].ToString() == st)
                    cbStatus.SelectedIndex = i;
        }

        private void OnAdd(object s, EventArgs e)
        {
            try
            {
                ProjectModel pm = new ProjectModel();
                pm.ProjectID   = txtProjID.Text.Trim();
                pm.ProjectName = txtProjName.Text.Trim();
                pm.DeptID      = GetDeptID();
                pm.StartDate   = dtpStart.Value;
                pm.Status      = cbStatus.SelectedItem.ToString();
                bll.Add(pm);
                ShowMsg("✓  Project added.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnUpdate(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjID.Text)) { ShowMsg("✗  Select a project first.", false); return; }
            try
            {
                ProjectModel pm = new ProjectModel();
                pm.ProjectID   = txtProjID.Text.Trim();
                pm.ProjectName = txtProjName.Text.Trim();
                pm.DeptID      = GetDeptID();
                pm.StartDate   = dtpStart.Value;
                pm.Status      = cbStatus.SelectedItem.ToString();
                bll.Update(pm);
                ShowMsg("✓  Project updated.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnDelete(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjID.Text)) { ShowMsg("✗  Select a project first.", false); return; }
            if (MessageBox.Show("Delete project " + txtProjID.Text + "? All linked tasks will also be deleted.",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                bll.Delete(txtProjID.Text.Trim());
                ShowMsg("✓  Project deleted.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnClear(object s, EventArgs e) { ClearFields(); lblMsg.Text = ""; }

        private void ClearFields()
        {
            txtProjID.Text     = "";
            txtProjName.Text   = "";
            txtProjID.ReadOnly = false;
            cbDept.SelectedIndex  = 0;
            cbStatus.SelectedIndex = 0;
        }

        private string GetDeptID()
        {
            if (cbDept.SelectedIndex <= 0) return "";
            return cbDept.SelectedItem.ToString().Split('—')[0].Trim();
        }

        private void ShowMsg(string text, bool ok)
        {
            lblMsg.Text      = text;
            lblMsg.ForeColor = ok ? UITheme.AccentTeal : Color.FromArgb(239, 68, 68);
        }
    }
}
