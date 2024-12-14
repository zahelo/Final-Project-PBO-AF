using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public class Gold
    {
        private PictureBox _goldPictureBox;

        public Gold(Point position)
        {
            _goldPictureBox = new PictureBox
            {
                Size = new Size(42, 28), // Ukuran tulang
                Location = position,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            try
            {
                using (MemoryStream ms = new MemoryStream(Resource.goldenbone))
                {
                    _goldPictureBox.Image = Image.FromStream(ms);
                }
            }
            catch (Exception e)
            {
                _goldPictureBox.BackColor = Color.White;
                Console.WriteLine("Gagal load image bone: " + e.Message);
            }

        }
        public PictureBox GetPictureBox()
        {
            return _goldPictureBox;
        }
    }
}
