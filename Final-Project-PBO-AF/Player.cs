using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public class Player
    {
        private const int PlayerWidth = 64;
        private const int PlayerHeight = 48;
        private const int TotalFrames = 4;
      
        private PictureBox _playerPictureBox;
        private Image _spriteSheet;
        private int _currentFrame;
        private int _currentRow;
        private bool _isMoving;

        public Player(Point startPosition)
        {
            using (MemoryStream ms = new MemoryStream(Resource.shibainu))
            {
                _spriteSheet = Image.FromStream(ms);
            }

            _currentFrame = 0;
            _currentRow = 0;

            _playerPictureBox = new PictureBox
            {
                Size = new Size(PlayerWidth, PlayerHeight),
                Location = startPosition,
                BackColor = Color.Transparent
            };

            UpdateSprite();
        }

        public PictureBox GetPictureBox() => _playerPictureBox;

        public void Walk(Keys key, Size boundary)
        {
            int speed = 10;
            _isMoving = true;

            // _currentRow menunjukkan baris yang sesuai pada resource sprite (bisa dilihat pada folder Resource -> shibainu.png)
            switch (key)
            {
                case Keys.Down: // Baris untuk Down dengan menggunakan panah atau 'S'
                case Keys.S:
                    _currentRow = 0;
                    if (_playerPictureBox.Bottom < boundary.Height)
                        _playerPictureBox.Top += speed;
                    break;
                case Keys.Left:
                case Keys.A:
                    _currentRow = 1; // Baris untuk Left
                    if (_playerPictureBox.Left > 0)
                        _playerPictureBox.Left -= speed;
                    break;
                case Keys.Right:
                case Keys.D:
                    _currentRow = 2; // Baris untuk Right
                    if (_playerPictureBox.Right < boundary.Width)
                        _playerPictureBox.Left += speed;
                    break;
                case Keys.Up:
                case Keys.W:
                    _currentRow = 3; // Baris untuk Up
                    if (_playerPictureBox.Top > 0)
                        _playerPictureBox.Top -= speed;
                    break;
                default:
                    _isMoving = false;
                    break;
            }
        }


        public void StopWalking()
        {
            _isMoving = false;
            _currentFrame = 0;
            UpdateSprite();
        }

        public void Animate()
        {
            if (_isMoving)
            {
                _currentFrame = (_currentFrame + 1) % 4; // Total 4 frame per baris
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            int frameWidth = 64;
            int frameHeight = 48;

            Rectangle srcRect = new Rectangle(_currentFrame * frameWidth, _currentRow * frameHeight, frameWidth, frameHeight);
            Bitmap currentFrameImage = new Bitmap(frameWidth, frameHeight);

            using (Graphics g = Graphics.FromImage(currentFrameImage))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(_spriteSheet, new Rectangle(0, 0, frameWidth, frameHeight), srcRect, GraphicsUnit.Pixel);
            }
            _playerPictureBox.Image = currentFrameImage;
        }
    }
}
