using FormsTimer = System.Windows.Forms.Timer;

namespace Final_Project_PBO_AF
{
    public class Player
    {
        private const int PlayerWidth = 120;
        private const int PlayerHeight = 90;
        private const int TotalFrames = 3;
        private PictureBox _playerPictureBox;
        private Image _spriteSheet;
        private int _currentFrame;
        private int _currentRow;
        private bool _isMoving;
        private int _speed;

        public Player(Point startPosition)
        {
            using (MemoryStream ms = new MemoryStream(Resource.shibainu))
            {
                _spriteSheet = Image.FromStream(ms);
            }

            _currentFrame = 0;
            _currentRow = 0;
            _speed = 10;

            _playerPictureBox = new PictureBox
            {
                Size = new Size(PlayerWidth, PlayerHeight),
                Location = startPosition,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage

            };

            UpdateSprite();
        }

        public PictureBox GetPictureBox() => _playerPictureBox;

        public void Walk(Keys key, Size boundary)
        {
            //int speed = 10;
            _isMoving = true;

            // _currentRow menunjukkan baris yang sesuai pada resource sprite (bisa dilihat pada folder Resource -> shibainu.png)
            switch (key) // Memakai arrow key dan WSAD
            {
                case Keys.Down: // Baris untuk Down 
                case Keys.S:
                    _currentRow = 0;
                    if (_playerPictureBox.Bottom < boundary.Height)
                        _playerPictureBox.Top += _speed;
                    break;
                case Keys.Left:
                case Keys.A:
                    _currentRow = 1; // Baris untuk Left 
                    if (_playerPictureBox.Left > 0)
                        _playerPictureBox.Left -= _speed;
                    break;
                case Keys.Right:
                case Keys.D:
                    _currentRow = 2; // Baris untuk Right
                    if (_playerPictureBox.Right < boundary.Width)
                        _playerPictureBox.Left += _speed;
                    break;
                case Keys.Up:
                case Keys.W:
                    _currentRow = 3; // Baris untuk Up
                    if (_playerPictureBox.Top > 0)
                        _playerPictureBox.Top -= _speed;
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
                _currentFrame = (_currentFrame + 1) % 3; // Total 3 frame per baris
                UpdateSprite();
            }
        }

        public void IncreaseSpeedTemporary(int additionalSpeed, int durationInMilliseconds)
        {
            _speed += additionalSpeed; // Tambah kecepatan

            // Timer untuk mengembalikan kecepatan ke normal
            FormsTimer resetSpeedTimer = new FormsTimer { Interval = durationInMilliseconds };
            resetSpeedTimer.Tick += (s, e) =>
            {
                _speed -= additionalSpeed; // Kembalikan kecepatan normal
                resetSpeedTimer.Stop();
            };
            resetSpeedTimer.Start();
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
