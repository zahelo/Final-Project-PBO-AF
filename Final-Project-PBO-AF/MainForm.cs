using System.Drawing;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public partial class MainForm : Form
    {
        private const int PlayerInitialPositionX = 50;
        private const int PlayerInitialPositionY = 50;
        private const int AnimationInterval = 100;
        private System.Windows.Forms.Timer _animationTimer;

        private Player _player;
        private Label _frameLabel;
        private long _totalFrameCount;
        private List<Bone> _bones;
        private int _score;
        private Label _scoreLabel;


        public MainForm()
        {
            InitializeLevel();
            this.DoubleBuffered = true;
        }

        private void InitializeLevel()
        {
            this.Text = "Bone Hunt";
            this.Size = new Size(800, 600);
            this.BackColor = Color.LightGray;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;



            _animationTimer = new System.Windows.Forms.Timer { Interval = AnimationInterval };
            _animationTimer.Tick += (sender, e) => Render();
            _animationTimer.Start();
            _bones = new List<Bone>();
            _score = 0;


            _player = new Player(new Point(PlayerInitialPositionX, PlayerInitialPositionY));
            this.Controls.Add(_player.GetPictureBox());

            _frameLabel = new Label
            {
                Text = "Total frames: ",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Arial", 16),
                ForeColor = Color.Black
            };
            this.Controls.Add(_frameLabel);

            _scoreLabel = new Label
            {
                Text = "Score: 0",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Arial", 16),
                ForeColor = Color.Black
            };
            this.Controls.Add(_scoreLabel);

            SpawnRandomBones(5); // Memunculkan 5 tulang di awal

        }
        private void SpawnRandomBones(int count)
        {
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                int x = rand.Next(50, this.ClientSize.Width - 50); // Hindari spawn terlalu dekat dengan tepi
                int y = rand.Next(50, this.ClientSize.Height - 50);

                Bone bone = new Bone(new Point(x, y));
                _bones.Add(bone);
                this.Controls.Add(bone.GetPictureBox());
            }
        }

        private void CheckCollisions()
        {
            foreach (var bone in _bones.ToList())
            {
                if (_player.GetPictureBox().Bounds.IntersectsWith(bone.GetPictureBox().Bounds))
                {
                    // Hapus tulang dari layar dan daftar
                    this.Controls.Remove(bone.GetPictureBox());
                    _bones.Remove(bone);

                    // Tambah skor
                    UpdateScore(10);
                }
            }

            // Jika semua tulang telah dikumpulkan, spawn lagi
            if (_bones.Count == 0)
            {
                SpawnRandomBones(5);
            }

        }
        private void UpdateScore(int points)
        {
            _score += points;
            _scoreLabel.Text = "Score: " + _score;
        }


        private void UpdateFrameCount()
        {
            _totalFrameCount++;
            _frameLabel.Text = "Total frames: " + _totalFrameCount;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _player.Walk(e.KeyCode, this.ClientSize);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _player.StopWalking();
        }

        private void Render()
        {
            UpdateFrameCount();
            _player.Animate();
            CheckCollisions();
        }
    }
}
