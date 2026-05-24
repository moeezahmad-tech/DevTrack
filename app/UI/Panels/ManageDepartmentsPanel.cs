using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevTrack.BusinessLogic.Managers;
using DevTrack.DataAccess.Models;
using DevTrack.UI.Utilities;

namespace DevTrack.UI.Panels
{
    public class ManageDepartmentsPanel : Panel
    {
        private DataGridView grid;
        private TextBox txtDeptID, txtDeptName;
        private Label   lblMsg;
        private Button  btnAdd, btnUpdate, btnDelete, btnClear;
        private DepartmentManager bll = new DepartmentManager();

        public ManageDepartmentsPanel()
        {
            this.BackColor = UITheme.BgMid;
            BuildUI();
            LoadGrid();
        }

        private void BuildUI()
        {
            Label lbl     = UITheme.MakeLabel("Manage Departments", UITheme.FontHeading, UITheme.AccentGold);
            lbl.Location  = new Point(30, 18);
            this.Controls.Add(lbl);

            Panel card    = UITheme.MakeCard(30, 58, 920, 160);
            this.Controls.Add(card);

            // Dept ID
            Label l1      = UITheme.MakeLabel("Department ID *", UITheme.FontSmall, UITheme.TextMuted);
            l1.Location   = new Point(20, 20);
            card.Controls.Add(l1);
            txtDeptID     = UITheme.MakeTextBox();
            txtDeptID.Location = new Point(20, 38);
            txtDeptID.Width    = 200;
            card.Controls.Add(txtDeptID);

            // Dept Name
            Label l2      = UITheme.MakeLabel("Department Name *", UITheme.FontSmall, UITheme.TextMuted);
            l2.Location   = new Point(240, 20);
            card.Controls.Add(l2);
            txtDeptName   = UITheme.MakeTextBox();
            txtDeptName.Location = new Point(240, 38);
            txtDeptName.Width    = 300;
            card.Controls.Add(txtDeptName);

            // Message
            lblMsg          = UITheme.MakeLabel("", UITheme.FontSmall, UITheme.AccentTeal);
            lblMsg.Location = new Point(20, 80);
            lblMsg.Size     = new Size(500, 20);
            lblMsg.AutoSize = false;
            card.Controls.Add(lblMsg);

            // Buttons
            btnAdd    = UITheme.MakeButton("➕ Add",    UITheme.BtnSuccess);
            btnUpdate = UITheme.MakeButton("✏  Update", UITheme.BtnBlue);
            btnDelete = UITheme.MakeButton("🗑  Delete", UITheme.BtnDanger);
            btnClear  = UITheme.MakeButton("✖  Clear",  UITheme.BgCard);

            int bw = 150;
            btnAdd.Size  = btnUpdate.Size = btnDelete.Size = btnClear.Size = new Size(bw, 38);
            btnAdd.Location    = new Point(20,  106);
            btnUpdate.Location = new Point(180, 106);
            btnDelete.Location = new Point(340, 106);
            btnClear.Location  = new Point(500, 106);

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
            grid.Location  = new Point(30, 232);
            grid.Size      = new Size(920, 420);
            grid.CellClick += new DataGridViewCellEventHandler(OnGridClick);
            this.Controls.Add(grid);
        }

        private void LoadGrid()
        {
            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.Columns.Add("ID",   "Department ID");
            grid.Columns.Add("Name", "Department Name");
            try
            {
                List<DepartmentModel> list = bll.GetAll();
                foreach (DepartmentModel d in list)
                    grid.Rows.Add(d.DeptID, d.DeptName);
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnGridClick(object s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            txtDeptID.Text     = row.Cells["ID"].Value.ToString();
            txtDeptName.Text   = row.Cells["Name"].Value.ToString();
            txtDeptID.ReadOnly = true;
        }

        private void OnAdd(object s, EventArgs e)
        {
            try
            {
                DepartmentModel d = new DepartmentModel();
                d.DeptID   = txtDeptID.Text.Trim().ToUpper();
                d.DeptName = txtDeptName.Text.Trim();
                bll.Add(d);
                ShowMsg("✓  Department added.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnUpdate(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeptID.Text)) { ShowMsg("✗  Select a department first.", false); return; }
            try
            {
                DepartmentModel d = new DepartmentModel();
                d.DeptID   = txtDeptID.Text.Trim();
                d.DeptName = txtDeptName.Text.Trim();
                bll.Update(d);
                ShowMsg("✓  Department updated.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnDelete(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeptID.Text)) { ShowMsg("✗  Select a department first.", false); return; }
            if (MessageBox.Show("Delete department " + txtDeptID.Text + "? This may affect linked employees/projects.",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                bll.Delete(txtDeptID.Text.Trim());
                ShowMsg("✓  Department deleted.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnClear(object s, EventArgs e) { ClearFields(); lblMsg.Text = ""; }

        private void ClearFields()
        {
            txtDeptID.Text     = "";
            txtDeptName.Text   = "";
            txtDeptID.ReadOnly = false;
        }

        private void ShowMsg(string text, bool ok)
        {
            lblMsg.Text      = text;
            lblMsg.ForeColor = ok ? UITheme.AccentTeal : Color.FromArgb(239, 68, 68);
        }
    }
}
