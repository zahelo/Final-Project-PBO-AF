using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public class Poop
    {
        private PictureBox _poopPictureBox;

        public Poop(Point position)
        {
            _poopPictureBox = new PictureBox
            {
                Size = new Size(30, 30), // Ukuran poop
                Location = position,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            try
            {
                using (MemoryStream ms = new MemoryStream(Resource.poop))
                {
                    _poopPictureBox.Image = Image.FromStream(ms);
                }
            }
            catch (Exception e)
            {
                _poopPictureBox.BackColor = Color.White;
                Console.WriteLine("Gagal load image bone: " + e.Message);
            }

        }
        public PictureBox GetPictureBox()
        {
            return _poopPictureBox;
        }
    }
}
