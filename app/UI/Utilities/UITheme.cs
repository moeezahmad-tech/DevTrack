using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevTrack.UI.Utilities
{
    public static class UITheme
    {
        // ── Colour Palette ──────────────────────────────────────────────
        public static Color BgDark      = Color.FromArgb(13, 27, 42);
        public static Color BgMid       = Color.FromArgb(22, 44, 66);
        public static Color BgPanel     = Color.FromArgb(27, 55, 82);
        public static Color BgCard      = Color.FromArgb(34, 67, 98);
        public static Color AccentTeal  = Color.FromArgb(20, 184, 166);
        public static Color AccentGold  = Color.FromArgb(212, 175, 55);
        public static Color BtnBlue     = Color.FromArgb(37, 99, 235);
        public static Color BtnDanger   = Color.FromArgb(185, 28, 28);
        public static Color BtnSuccess  = Color.FromArgb(21, 128, 61);
        public static Color TextWhite   = Color.FromArgb(248, 250, 252);
        public static Color TextSilver  = Color.FromArgb(180, 196, 212);
        public static Color TextMuted   = Color.FromArgb(100, 130, 158);
        public static Color Separator   = Color.FromArgb(40, 80, 110);

        // ── Fonts ────────────────────────────────────────────────────────
        public static Font FontTitle    = new Font("Segoe UI", 22, FontStyle.Bold);
        public static Font FontHeading  = new Font("Segoe UI", 14, FontStyle.Bold);
        public static Font FontSubhead  = new Font("Segoe UI", 11, FontStyle.Bold);
        public static Font FontBody     = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font FontSmall    = new Font("Segoe UI", 9,  FontStyle.Regular);
        public static Font FontButton   = new Font("Segoe UI", 10, FontStyle.Bold);
        public static Font FontNav      = new Font("Segoe UI", 11, FontStyle.Regular);

        // ── Helpers ──────────────────────────────────────────────────────
        public static Button MakeButton(string text, Color backColor)
        {
            Button btn = new Button();
            btn.Text      = text;
            btn.BackColor = backColor;
            btn.ForeColor = TextWhite;
            btn.Font      = FontButton;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize  = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.2f);
            btn.Cursor    = Cursors.Hand;
            btn.Height    = 40;
            return btn;
        }

        public static TextBox MakeTextBox(bool isPassword = false)
        {
            TextBox tb = new TextBox();
            tb.BackColor    = BgCard;
            tb.ForeColor    = TextWhite;
            tb.Font         = FontBody;
            tb.BorderStyle  = BorderStyle.FixedSingle;
            tb.Height       = 34;
            if (isPassword)
                tb.PasswordChar = '●';
            return tb;
        }

        public static ComboBox MakeComboBox()
        {
            ComboBox cb = new ComboBox();
            cb.BackColor   = BgCard;
            cb.ForeColor   = TextWhite;
            cb.Font        = FontBody;
            cb.FlatStyle   = FlatStyle.Flat;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            return cb;
        }

        public static Label MakeLabel(string text, Font font = null, Color? color = null)
        {
            Label lbl = new Label();
            lbl.Text      = text;
            lbl.Font      = font ?? FontBody;
            lbl.ForeColor = color ?? TextWhite;
            lbl.AutoSize  = true;
            lbl.BackColor = Color.Transparent;
            return lbl;
        }

        public static DateTimePicker MakeDatePicker()
        {
            DateTimePicker dtp = new DateTimePicker();
            dtp.Format     = DateTimePickerFormat.Short;
            dtp.Font       = FontBody;
            dtp.CalendarForeColor    = TextWhite;
            dtp.CalendarMonthBackground = BgCard;
            return dtp;
        }

        public static DataGridView MakeGrid()
        {
            DataGridView dgv = new DataGridView();
            dgv.BackgroundColor         = BgMid;
            dgv.ForeColor               = TextWhite;
            dgv.BorderStyle             = BorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor  = BgDark;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor  = AccentGold;
            dgv.ColumnHeadersDefaultCellStyle.Font       = FontSubhead;
            dgv.DefaultCellStyle.BackColor               = BgPanel;
            dgv.DefaultCellStyle.ForeColor               = TextWhite;
            dgv.DefaultCellStyle.Font                    = FontBody;
            dgv.DefaultCellStyle.SelectionBackColor      = BgCard;
            dgv.DefaultCellStyle.SelectionForeColor      = AccentTeal;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = BgMid;
            dgv.GridColor                                = Separator;
            dgv.RowHeadersVisible                        = false;
            dgv.ReadOnly                                 = true;
            dgv.SelectionMode                            = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode                      = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows                       = false;
            dgv.AllowUserToDeleteRows                    = false;
            dgv.ColumnHeadersHeightSizeMode              = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.EnableHeadersVisualStyles                = false;
            return dgv;
        }

        // Paint gradient background onto a form or panel
        public static void PaintGradientBg(object sender, PaintEventArgs e)
        {
            Control ctrl = (Control)sender;
            Rectangle rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
            LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                BgDark,
                BgMid,
                LinearGradientMode.ForwardDiagonal);
            e.Graphics.FillRectangle(brush, rect);
            brush.Dispose();

            // Subtle dot pattern for texture
            using (SolidBrush dot = new SolidBrush(Color.FromArgb(18, 255, 255, 255)))
            {
                for (int x = 0; x < ctrl.Width; x += 30)
                {
                    for (int y = 0; y < ctrl.Height; y += 30)
                    {
                        e.Graphics.FillEllipse(dot, x, y, 2, 2);
                    }
                }
            }
        }

        // Styled panel with rounded look
        public static Panel MakeCard(int x, int y, int w, int h)
        {
            Panel card = new Panel();
            card.Location   = new Point(x, y);
            card.Size       = new Size(w, h);
            card.BackColor  = BgCard;
            card.BorderStyle = BorderStyle.None;
            return card;
        }

        // Separator line
        public static Panel MakeSeparator(int x, int y, int width)
        {
            Panel sep = new Panel();
            sep.Location   = new Point(x, y);
            sep.Size       = new Size(width, 1);
            sep.BackColor  = Separator;
            return sep;
        }
    }
}
