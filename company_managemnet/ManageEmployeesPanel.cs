using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CompanyManagement.BLL;
using CompanyManagement.Models;

namespace CompanyManagement.Forms
{
    public class ManageEmployeesPanel : Panel
    {
        private DataGridView grid;
        private TextBox  txtID, txtName, txtEmail, txtSalary, txtPassword, txtRole;
        private ComboBox cbDept;
        private DateTimePicker dtpJoining;
        private Label    lblMsg;
        private Button   btnAdd, btnUpdate, btnDelete, btnClear;

        private EmployeeBLL   empBLL  = new EmployeeBLL();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private PersonBLL     persBLL = new PersonBLL();

        public ManageEmployeesPanel()
        {
            this.BackColor = UITheme.BgMid;
            BuildUI();
            LoadGrid();
            LoadDepts();
        }

        private void BuildUI()
        {
            // ── Section heading ───────────────────────────────────────────────
            Label lbl     = UITheme.MakeLabel("Register / Manage Employees", UITheme.FontHeading, UITheme.AccentGold);
            lbl.Location  = new Point(30, 18);
            this.Controls.Add(lbl);

            Label lblNote = UITheme.MakeLabel(
                "Only the Head of Department can add new employees. Deleting removes them from Employee records.",
                UITheme.FontSmall, UITheme.TextMuted);
            lblNote.Location = new Point(30, 50);
            this.Controls.Add(lblNote);

            // ── Input form card ───────────────────────────────────────────────
            Panel card    = UITheme.MakeCard(30, 76, 920, 270);
            this.Controls.Add(card);

            int lw = 120, fw = 180, row1 = 20, row2 = 72, row3 = 124, row4 = 176;
            int col1 = 20, col2 = 330, col3 = 640;

            // Row 1
            card.Controls.Add(Field(card, "Person ID *",    col1, row1, lw, out txtID,       fw, false));
            card.Controls.Add(Field(card, "Full Name *",    col2, row1, lw, out txtName,     fw, false));
            card.Controls.Add(Field(card, "Email *",        col3, row1, lw, out txtEmail,    fw, false));

            // Row 2
            card.Controls.Add(Field(card, "Salary *",       col1, row2, lw, out txtSalary,   fw, false));
            card.Controls.Add(Field(card, "Password *",     col2, row2, lw, out txtPassword, fw, true));
            card.Controls.Add(FieldDate(card, "Joining Date *", col3, row2, lw, out dtpJoining, fw));

            // Row 3
            card.Controls.Add(Field(card, "Role *",         col1, row3, lw, out txtRole,     fw, false));
            card.Controls.Add(FieldCombo(card, "Department", col2, row3, lw, out cbDept,     fw));

            // Message
            lblMsg          = UITheme.MakeLabel("", UITheme.FontSmall, UITheme.AccentTeal);
            lblMsg.Location = new Point(col1, row4);
            lblMsg.Size     = new Size(500, 20);
            lblMsg.AutoSize = false;
            card.Controls.Add(lblMsg);

            // Buttons
            btnAdd    = UITheme.MakeButton("➕ Add Employee",  UITheme.BtnSuccess);
            btnUpdate = UITheme.MakeButton("✏  Update Role/Dept", UITheme.BtnBlue);
            btnDelete = UITheme.MakeButton("🗑  Delete",        UITheme.BtnDanger);
            btnClear  = UITheme.MakeButton("✖  Clear",          UITheme.BgCard);

            btnAdd.Size  = btnUpdate.Size = btnDelete.Size = btnClear.Size = new Size(192, 38);
            btnAdd.Location    = new Point(col1,       row4 + 26);
            btnUpdate.Location = new Point(col1 + 202, row4 + 26);
            btnDelete.Location = new Point(col1 + 404, row4 + 26);
            btnClear.Location  = new Point(col1 + 606, row4 + 26);

            btnAdd.Click    += new EventHandler(OnAdd);
            btnUpdate.Click += new EventHandler(OnUpdate);
            btnDelete.Click += new EventHandler(OnDelete);
            btnClear.Click  += new EventHandler(OnClear);

            card.Controls.Add(btnAdd);
            card.Controls.Add(btnUpdate);
            card.Controls.Add(btnDelete);
            card.Controls.Add(btnClear);

            // ── Grid ──────────────────────────────────────────────────────────
            grid           = UITheme.MakeGrid();
            grid.Location  = new Point(30, 358);
            grid.Size      = new Size(920, 340);
            grid.CellClick += new DataGridViewCellEventHandler(OnGridClick);
            this.Controls.Add(grid);
        }

        // Helper to place a label+textbox in card
        private Panel Field(Panel card, string lbl, int x, int y, int lw, out TextBox tb, int fw, bool pwd)
        {
            Label l    = UITheme.MakeLabel(lbl, UITheme.FontSmall, UITheme.TextMuted);
            l.Location = new Point(x, y);
            card.Controls.Add(l);

            tb          = UITheme.MakeTextBox(pwd);
            tb.Location = new Point(x, y + 18);
            tb.Width    = fw;
            card.Controls.Add(tb);
            return new Panel(); // dummy return – controls added directly
        }

        private Panel FieldDate(Panel card, string lbl, int x, int y, int lw, out DateTimePicker dtp, int fw)
        {
            Label l    = UITheme.MakeLabel(lbl, UITheme.FontSmall, UITheme.TextMuted);
            l.Location = new Point(x, y);
            card.Controls.Add(l);

            dtp          = UITheme.MakeDatePicker();
            dtp.Location = new Point(x, y + 18);
            dtp.Width    = fw;
            card.Controls.Add(dtp);
            return new Panel();
        }

        private Panel FieldCombo(Panel card, string lbl, int x, int y, int lw, out ComboBox cb, int fw)
        {
            Label l    = UITheme.MakeLabel(lbl, UITheme.FontSmall, UITheme.TextMuted);
            l.Location = new Point(x, y);
            card.Controls.Add(l);

            cb          = UITheme.MakeComboBox();
            cb.Location = new Point(x, y + 18);
            cb.Width    = fw;
            card.Controls.Add(cb);
            return new Panel();
        }

        private void LoadDepts()
        {
            cbDept.Items.Clear();
            cbDept.Items.Add("(None)");
            try
            {
                List<DepartmentModel> depts = deptBLL.GetAll();
                foreach (DepartmentModel d in depts)
                    cbDept.Items.Add(d.DeptID + " — " + d.DeptName);
            }
            catch { }
            cbDept.SelectedIndex = 0;
        }

        private void LoadGrid()
        {
            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.Columns.Add("ID",     "ID");
            grid.Columns.Add("Name",   "Name");
            grid.Columns.Add("Email",  "Email");
            grid.Columns.Add("Role",   "Role");
            grid.Columns.Add("Dept",   "Department");
            try
            {
                List<EmployeeModel> list = empBLL.GetAll();
                foreach (EmployeeModel e in list)
                    grid.Rows.Add(e.ID, e.Name, e.Email, e.Role, e.DeptName);
            }
            catch (Exception ex)
            {
                ShowMsg("Error loading: " + ex.Message, false);
            }
        }

        private void OnGridClick(object s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            txtID.Text       = row.Cells["ID"].Value.ToString();
            txtName.Text     = row.Cells["Name"].Value.ToString();
            txtEmail.Text    = row.Cells["Email"].Value.ToString();
            txtRole.Text     = row.Cells["Role"].Value.ToString();
            txtID.ReadOnly   = true;
        }

        private void OnAdd(object s, EventArgs e)
        {
            try
            {
                PersonModel person = new PersonModel();
                person.ID          = int.Parse(txtID.Text.Trim());
                person.Name        = txtName.Text.Trim();
                person.Email       = txtEmail.Text.Trim();
                person.Salary      = decimal.Parse(txtSalary.Text.Trim());
                person.Password    = txtPassword.Text.Trim();
                person.JoiningDate = dtpJoining.Value;

                EmployeeModel emp = new EmployeeModel();
                emp.Role          = txtRole.Text.Trim();
                emp.DeptID        = GetSelectedDeptID();

                empBLL.RegisterNewEmployee(person, emp);
                ShowMsg("✓  Employee registered successfully.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex)
            {
                ShowMsg("✗  " + ex.Message, false);
            }
        }

        private void OnUpdate(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                ShowMsg("✗  Select an employee from the grid first.", false);
                return;
            }
            try
            {
                EmployeeModel emp = new EmployeeModel();
                emp.ID     = int.Parse(txtID.Text);
                emp.Role   = txtRole.Text.Trim();
                emp.DeptID = GetSelectedDeptID();
                empBLL.Update(emp);
                ShowMsg("✓  Employee updated.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex)
            {
                ShowMsg("✗  " + ex.Message, false);
            }
        }

        private void OnDelete(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                ShowMsg("✗  Select an employee from the grid first.", false);
                return;
            }
            if (MessageBox.Show("Delete employee ID " + txtID.Text + "?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            try
            {
                empBLL.Delete(int.Parse(txtID.Text));
                ShowMsg("✓  Employee deleted.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex)
            {
                ShowMsg("✗  " + ex.Message, false);
            }
        }

        private void OnClear(object s, EventArgs e) { ClearFields(); lblMsg.Text = ""; }

        private void ClearFields()
        {
            txtID.Text       = "";
            txtName.Text     = "";
            txtEmail.Text    = "";
            txtSalary.Text   = "";
            txtPassword.Text = "";
            txtRole.Text     = "";
            txtID.ReadOnly   = false;
            cbDept.SelectedIndex = 0;
        }

        private string GetSelectedDeptID()
        {
            if (cbDept.SelectedIndex <= 0) return "";
            string item = cbDept.SelectedItem.ToString();
            return item.Split('—')[0].Trim();
        }

        private void ShowMsg(string text, bool ok)
        {
            lblMsg.Text      = text;
            lblMsg.ForeColor = ok ? UITheme.AccentTeal : Color.FromArgb(239, 68, 68);
        }
    }
}
