using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public class OutlinedLabel : Control
    {
        public string DisplayText { get; set; } = "BONE HUNT";
        public Font DisplayFont { get; set; } = new Font("Arial", 48, FontStyle.Bold);
        public Color OutlineColor { get; set; } = Color.Black;
        public Color TextColor { get; set; } = ColorTranslator.FromHtml("#EFC482");
        public int OutlineWidth { get; set; } = 3;

        public OutlinedLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.DoubleBuffered = true; // Mengurangi flicker
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Hitung ukuran teks
            SizeF textSize = g.MeasureString(DisplayText, DisplayFont);
            int padding = OutlineWidth * 2; // Tambahkan padding untuk outline

            // Atur ukuran kontrol jika diperlukan
            this.Size = new Size((int)textSize.Width + padding, (int)textSize.Height + padding);

            // Hitung posisi teks di tengah kontrol
            float x = (this.Width - textSize.Width) / 2;
            float y = (this.Height - textSize.Height) / 2;

            // Gambar outline
            using (var outlineBrush = new SolidBrush(OutlineColor))
            {
                for (int dx = -OutlineWidth; dx <= OutlineWidth; dx++)
                {
                    for (int dy = -OutlineWidth; dy <= OutlineWidth; dy++)
                    {
                        if (dx != 0 || dy != 0)
                            g.DrawString(DisplayText, DisplayFont, outlineBrush, x + dx, y + dy);
                    }
                }
            }

            // Gambar teks utama
            using (var textBrush = new SolidBrush(TextColor))
            {
                g.DrawString(DisplayText, DisplayFont, textBrush, x, y);
            }
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }
    }
}
