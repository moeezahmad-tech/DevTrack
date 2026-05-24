using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevTrack.BusinessLogic.Managers;
using DevTrack.DataAccess.Models;
using DevTrack.UI.Utilities;

namespace DevTrack.UI.Forms
{
    public class LoginForm : Form
    {
        private TextBox  txtEmail;
        private TextBox  txtPassword;
        private Button   btnLogin;
        private Label    lblError;
        private CheckBox chkShowPass;
        private Panel    cardPanel;

        public LoginForm()
        {
            this.Text            = "Company Portal — Sign In";
            this.Size            = new Size(1100, 700);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.BackColor       = UITheme.BgDark;
            this.Paint          += new PaintEventHandler(OnFormPaint);

            BuildUI();
        }

        // ── Paint gradient + texture background ───────────────────────────────
        private void OnFormPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Left-side dark gradient
            Rectangle leftRect = new Rectangle(0, 0, this.Width / 2, this.Height);
            LinearGradientBrush leftBrush = new LinearGradientBrush(
                leftRect, UITheme.BgDark, UITheme.BgMid, LinearGradientMode.BackwardDiagonal);
            g.FillRectangle(leftBrush, leftRect);
            leftBrush.Dispose();

            // Right-side slightly lighter
            Rectangle rightRect = new Rectangle(this.Width / 2, 0, this.Width / 2, this.Height);
            LinearGradientBrush rightBrush = new LinearGradientBrush(
                rightRect, UITheme.BgMid, UITheme.BgPanel, LinearGradientMode.ForwardDiagonal);
            g.FillRectangle(rightBrush, rightRect);
            rightBrush.Dispose();

            // Dot texture
            using (SolidBrush dot = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            {
                for (int x = 0; x < this.Width; x += 28)
                    for (int y = 0; y < this.Height; y += 28)
                        g.FillEllipse(dot, x, y, 2, 2);
            }

            // Decorative teal vertical bar on left side
            using (SolidBrush bar = new SolidBrush(Color.FromArgb(40, UITheme.AccentTeal)))
                g.FillRectangle(bar, 0, 0, 8, this.Height);

            // Gold accent line
            using (Pen pen = new Pen(UITheme.AccentGold, 2))
                g.DrawLine(pen, this.Width / 2 - 1, 60, this.Width / 2 - 1, this.Height - 60);

            // Brand name on left side
            using (Font bigFont = new Font("Segoe UI", 30, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(UITheme.TextWhite))
            {
                g.DrawString("COMPANY", bigFont, textBrush, 60, 200);
            }
            using (Font bigFont = new Font("Segoe UI", 30, FontStyle.Regular))
            using (SolidBrush teal = new SolidBrush(UITheme.AccentTeal))
            {
                g.DrawString("MANAGEMENT", bigFont, teal, 60, 250);
            }
            using (Font subFont = new Font("Segoe UI", 11, FontStyle.Regular))
            using (SolidBrush muted = new SolidBrush(UITheme.TextMuted))
            {
                g.DrawString("Office Portal  ·  Secure Sign In", subFont, muted, 60, 310);
            }

            // Bottom tagline left side
            using (Font tagFont = new Font("Segoe UI", 9, FontStyle.Italic))
            using (SolidBrush muted = new SolidBrush(UITheme.TextMuted))
            {
                g.DrawString("Streamlining departments, projects & teams.", tagFont, muted, 60, this.Height - 80);
            }
        }

        // ── Build all controls ────────────────────────────────────────────────
        private void BuildUI()
        {
            // Card on right side
            cardPanel           = new Panel();
            cardPanel.Size      = new Size(400, 460);
            cardPanel.Location  = new Point(this.Width / 2 + (this.Width / 2 - 400) / 2, (this.Height - 460) / 2);
            cardPanel.BackColor = UITheme.BgPanel;
            this.Controls.Add(cardPanel);

            // Gold top border stripe on card
            Panel topStripe      = new Panel();
            topStripe.Size       = new Size(400, 4);
            topStripe.Location   = new Point(0, 0);
            topStripe.BackColor  = UITheme.AccentGold;
            cardPanel.Controls.Add(topStripe);

            // Sign In heading
            Label lblHeading       = UITheme.MakeLabel("Sign In", UITheme.FontTitle);
            lblHeading.Location    = new Point(40, 30);
            lblHeading.ForeColor   = UITheme.TextWhite;
            cardPanel.Controls.Add(lblHeading);

            Label lblSub           = UITheme.MakeLabel("Enter your credentials to access the portal", UITheme.FontSmall);
            lblSub.Location        = new Point(40, 80);
            lblSub.ForeColor       = UITheme.TextMuted;
            cardPanel.Controls.Add(lblSub);

            // Separator
            Panel sep              = UITheme.MakeSeparator(40, 108, 320);
            cardPanel.Controls.Add(sep);

            // Email
            Label lblEmail         = UITheme.MakeLabel("EMAIL ADDRESS", UITheme.FontSmall, UITheme.AccentTeal);
            lblEmail.Location      = new Point(40, 126);
            cardPanel.Controls.Add(lblEmail);

            txtEmail               = UITheme.MakeTextBox();
            txtEmail.Location      = new Point(40, 148);
            txtEmail.Size          = new Size(320, 34);
            cardPanel.Controls.Add(txtEmail);

            // Password
            Label lblPwd           = UITheme.MakeLabel("PASSWORD", UITheme.FontSmall, UITheme.AccentTeal);
            lblPwd.Location        = new Point(40, 196);
            cardPanel.Controls.Add(lblPwd);

            txtPassword            = UITheme.MakeTextBox(true);
            txtPassword.Location   = new Point(40, 218);
            txtPassword.Size       = new Size(320, 34);
            cardPanel.Controls.Add(txtPassword);

            // Show password checkbox
            chkShowPass            = new CheckBox();
            chkShowPass.Text       = "Show Password";
            chkShowPass.ForeColor  = UITheme.TextMuted;
            chkShowPass.BackColor  = Color.Transparent;
            chkShowPass.Font       = UITheme.FontSmall;
            chkShowPass.Location   = new Point(40, 262);
            chkShowPass.AutoSize   = true;
            chkShowPass.Cursor     = Cursors.Hand;
            chkShowPass.CheckedChanged += new EventHandler(OnShowPassChanged);
            cardPanel.Controls.Add(chkShowPass);

            // Error label
            lblError               = UITheme.MakeLabel("", UITheme.FontSmall, Color.FromArgb(239, 68, 68));
            lblError.Location      = new Point(40, 292);
            lblError.Size          = new Size(320, 20);
            lblError.AutoSize      = false;
            cardPanel.Controls.Add(lblError);

            // Login button
            btnLogin               = UITheme.MakeButton("  SIGN IN  →", UITheme.AccentTeal);
            btnLogin.Location      = new Point(40, 322);
            btnLogin.Size          = new Size(320, 46);
            btnLogin.ForeColor     = UITheme.BgDark;
            btnLogin.Font          = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLogin.Click        += new EventHandler(OnLoginClick);
            cardPanel.Controls.Add(btnLogin);

            // Footer note
            Label lblNote          = UITheme.MakeLabel(
                "New employees are registered by the Head of Department.",
                UITheme.FontSmall, UITheme.TextMuted);
            lblNote.Location       = new Point(40, 386);
            lblNote.Size           = new Size(320, 40);
            lblNote.AutoSize       = false;
            cardPanel.Controls.Add(lblNote);

            // Version
            Label lblVer           = UITheme.MakeLabel("v1.0.0  ·  Company Management System", UITheme.FontSmall, UITheme.TextMuted);
            lblVer.Location        = new Point(40, 426);
            cardPanel.Controls.Add(lblVer);

            // Allow Enter key to trigger login
            this.AcceptButton = btnLogin;
        }

        private void OnShowPassChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked)
                txtPassword.PasswordChar = '\0';
            else
                txtPassword.PasswordChar = '●';
        }

        private void OnLoginClick(object sender, EventArgs e)
        {
            lblError.Text   = "";
            string email    = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                PersonManager   personMgr = new PersonManager();
                PersonModel person        = personMgr.Login(email, password);

                if (person == null)
                {
                    lblError.Text = "✗  Invalid email or password.";
                    return;
                }

                HeadOfDeptManager hodMgr = new HeadOfDeptManager();
                EmployeeManager   empMgr = new EmployeeManager();

                if (hodMgr.IsHOD(person.ID))
                {
                    HeadOfDeptModel hod = hodMgr.GetByID(person.ID);
                    HODDashboard dashboard = new HODDashboard(hod);
                    this.Hide();
                    dashboard.FormClosed += new FormClosedEventHandler(OnChildClosed);
                    dashboard.Show();
                }
                else if (empMgr.IsEmployee(person.ID))
                {
                    EmployeeModel emp = empMgr.GetByID(person.ID);
                    EmployeeDashboard dashboard = new EmployeeDashboard(emp);
                    this.Hide();
                    dashboard.FormClosed += new FormClosedEventHandler(OnChildClosed);
                    dashboard.Show();
                }
                else
                {
                    lblError.Text = "✗  Account not assigned a role. Contact your HOD.";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "✗  " + ex.Message;
            }
        }

        private void OnChildClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
            txtPassword.Clear();
            lblError.Text = "";
        }
    }
}
