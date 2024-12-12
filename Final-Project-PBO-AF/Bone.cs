using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public class Bone
    {
        private PictureBox _bonePictureBox;

        public Bone(Point position)
        {
            _bonePictureBox = new PictureBox
            {
                Size = new Size(32, 32), // Ukuran tulang
                Location = position,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            try
            {
                using (MemoryStream ms = new MemoryStream(Resource.bone)) 
                {
                    _bonePictureBox.Image = Image.FromStream(ms);
                }
            }
            catch (Exception e)
            {
                _bonePictureBox.BackColor = Color.White; 
                Console.WriteLine("Gagal load image bone: " + e.Message);
            }

        }
        public PictureBox GetPictureBox()
        {
            return _bonePictureBox;
        }
    }
}
