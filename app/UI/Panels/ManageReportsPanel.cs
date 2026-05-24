using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevTrack.BusinessLogic.Managers;
using DevTrack.DataAccess.Models;
using DevTrack.UI.Utilities;

namespace DevTrack.UI.Panels
{
    // isHOD = true   -> HOD view: see all reports, approve/reject, delete
    // isHOD = false  -> Employee view: submit new report only
    public class ManageReportsPanel : Panel
    {
        private bool        isHOD;
        private DataGridView grid;
        private TextBox     txtRepID, txtTitle, txtDesc;
        private Label       lblMsg;
        private Button      btnSubmit, btnApprove, btnReject, btnDelete, btnClear;

        private ReportManager bll = new ReportManager();

        public ManageReportsPanel(bool isHOD)
        {
            this.isHOD     = isHOD;
            this.BackColor = UITheme.BgMid;
            BuildUI();
            LoadGrid();
        }

        private void BuildUI()
        {
            string heading = isHOD ? "Manage Reports - Approve / Reject" : "Submit a Report";
            Label lbl     = UITheme.MakeLabel(heading, UITheme.FontHeading, UITheme.AccentGold);
            lbl.Location  = new Point(30, 18);
            this.Controls.Add(lbl);

            Panel card    = UITheme.MakeCard(30, 58, 920, 210);
            this.Controls.Add(card);

            int c1 = 20, c2 = 280;
            int r1 = 16, r2 = 68, r3 = 120, r4 = 165;

            // Report ID
            AL(card, "Report ID *", c1, r1);
            txtRepID = ATB(card, c1, r1 + 18, 230);

            // Report Title
            AL(card, "Report Title *", c2, r1);
            txtTitle = ATB(card, c2, r1 + 18, 600);

            // Description
            AL(card, "Report Description", c1, r2);
            txtDesc = new TextBox();
            txtDesc.Multiline   = true;
            txtDesc.BackColor   = UITheme.BgCard;
            txtDesc.ForeColor   = UITheme.TextWhite;
            txtDesc.Font        = UITheme.FontBody;
            txtDesc.BorderStyle = BorderStyle.FixedSingle;
            txtDesc.Location    = new Point(c1, r2 + 18);
            txtDesc.Size        = new Size(880, 46);
            card.Controls.Add(txtDesc);

            // Message
            lblMsg          = UITheme.MakeLabel("", UITheme.FontSmall, UITheme.AccentTeal);
            lblMsg.Location = new Point(c1, r3 + 4);
            lblMsg.Size     = new Size(550, 20);
            lblMsg.AutoSize = false;
            card.Controls.Add(lblMsg);

            // ── Buttons depend on role ─────────────────────────────────────────
            if (isHOD)
            {
                btnApprove = UITheme.MakeButton("✔  Approve",  UITheme.BtnSuccess);
                btnReject  = UITheme.MakeButton("✘  Reject",   UITheme.BtnDanger);
                btnDelete  = UITheme.MakeButton("🗑  Delete",   Color.FromArgb(80, 40, 40));
                btnClear   = UITheme.MakeButton("✖  Clear",    UITheme.BgCard);

                int bw = 160;
                btnApprove.Size = btnReject.Size = btnDelete.Size = btnClear.Size = new Size(bw, 38);
                btnApprove.Location = new Point(c1,       r4);
                btnReject.Location  = new Point(c1 + 170, r4);
                btnDelete.Location  = new Point(c1 + 340, r4);
                btnClear.Location   = new Point(c1 + 510, r4);

                btnApprove.Click += new EventHandler(OnApprove);
                btnReject.Click  += new EventHandler(OnReject);
                btnDelete.Click  += new EventHandler(OnDelete);
                btnClear.Click   += new EventHandler(OnClear);

                card.Controls.Add(btnApprove);
                card.Controls.Add(btnReject);
                card.Controls.Add(btnDelete);
                card.Controls.Add(btnClear);
            }
            else
            {
                btnSubmit = UITheme.MakeButton("📤  Submit Report", UITheme.AccentTeal);
                btnClear  = UITheme.MakeButton("✖  Clear",          UITheme.BgCard);

                btnSubmit.Size  = btnClear.Size  = new Size(200, 38);
                btnSubmit.Location = new Point(c1,       r4);
                btnClear.Location  = new Point(c1 + 210, r4);
                btnSubmit.ForeColor = UITheme.BgDark;

                btnSubmit.Click += new EventHandler(OnSubmit);
                btnClear.Click  += new EventHandler(OnClear);

                card.Controls.Add(btnSubmit);
                card.Controls.Add(btnClear);
            }

            // ── Grid ──────────────────────────────────────────────────────────
            grid           = UITheme.MakeGrid();
            grid.Location  = new Point(30, 280);
            grid.Size      = new Size(920, 380);
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

        private void LoadGrid()
        {
            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.Columns.Add("ID",     "Report ID");
            grid.Columns.Add("Title",  "Title");
            grid.Columns.Add("Date",   "Submission Date");
            grid.Columns.Add("Status", "Status");
            grid.Columns.Add("Desc",   "Description");
            try
            {
                foreach (ReportModel r in bll.GetAll())
                    grid.Rows.Add(r.ReportID, r.ReportTitle,
                                  r.SubmissionDate.ToString("dd MMM yyyy"),
                                  r.StatusUpdate, r.ReportDescription);

                // Colour-code status
                foreach (DataGridViewRow row in grid.Rows)
                {
                    string st = row.Cells["Status"].Value.ToString();
                    if (st == "Approved")
                        row.Cells["Status"].Style.ForeColor = UITheme.AccentTeal;
                    else if (st == "Rejected")
                        row.Cells["Status"].Style.ForeColor = Color.FromArgb(239, 68, 68);
                    else
                        row.Cells["Status"].Style.ForeColor = UITheme.AccentGold;
                }
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnGridClick(object s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            txtRepID.Text     = row.Cells["ID"].Value.ToString();
            txtTitle.Text     = row.Cells["Title"].Value.ToString();
            txtDesc.Text      = row.Cells["Desc"].Value.ToString();
            txtRepID.ReadOnly = true;
        }

        private void OnSubmit(object s, EventArgs e)
        {
            try
            {
                ReportModel r = new ReportModel();
                r.ReportID          = txtRepID.Text.Trim();
                r.ReportTitle       = txtTitle.Text.Trim();
                r.ReportDescription = txtDesc.Text.Trim();
                r.SubmissionDate    = DateTime.Now;
                bll.Submit(r);
                ShowMsg("✓  Report submitted successfully.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnApprove(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRepID.Text)) { ShowMsg("✗  Select a report first.", false); return; }
            try
            {
                bll.Approve(txtRepID.Text.Trim());
                ShowMsg("✓  Report approved.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnReject(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRepID.Text)) { ShowMsg("✗  Select a report first.", false); return; }
            try
            {
                bll.Reject(txtRepID.Text.Trim());
                ShowMsg("✓  Report rejected.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnDelete(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRepID.Text)) { ShowMsg("✗  Select a report first.", false); return; }
            if (MessageBox.Show("Delete report " + txtRepID.Text + "?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                bll.Delete(txtRepID.Text.Trim());
                ShowMsg("✓  Report deleted.", true);
                ClearFields();
                LoadGrid();
            }
            catch (Exception ex) { ShowMsg("✗  " + ex.Message, false); }
        }

        private void OnClear(object s, EventArgs e) { ClearFields(); lblMsg.Text = ""; }

        private void ClearFields()
        {
            txtRepID.Text     = "";
            txtTitle.Text     = "";
            txtDesc.Text      = "";
            txtRepID.ReadOnly = false;
        }

        private void ShowMsg(string text, bool ok)
        {
            lblMsg.Text      = text;
            lblMsg.ForeColor = ok ? UITheme.AccentTeal : Color.FromArgb(239, 68, 68);
        }
    }
}
