using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public partial class MainForm : Form
    {
        private const int PlayerInitialPositionX = 900;
        private const int PlayerInitialPositionY = 450;
        private const int AnimationInterval = 100;
        private System.Windows.Forms.Timer _animationTimer;

        private Player _player;
        private Label _frameLabel;
        private long _totalFrameCount;
        private List<Bone> _bones;
        private List<Poop> _poop;
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
            this.FormBorderStyle = FormBorderStyle.Sizable; // Start in maximized
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.LightGray;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;

            _animationTimer = new System.Windows.Forms.Timer { Interval = AnimationInterval };
            _animationTimer.Tick += (sender, e) => Render();
            _animationTimer.Start();

            _bones = new List<Bone>();
            _poop = new List<Poop>();
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
                Location = new Point(10, 40),
                AutoSize = true,
                Font = new Font("Arial", 16),
                ForeColor = Color.Black
            };
            this.Controls.Add(_scoreLabel);
        }

        protected override void OnShown(EventArgs e) //spawn bones and poop after canvas is fully form
        {
            base.OnShown(e);

            SpawnRandomBones(3);
            SpawnRandomPoop(2);
        }


        private void SpawnRandomBones(int count)
        {
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                Point position;
                bool validPosition;

                do
                {
                    validPosition = true;

                    int x = rand.Next(50, this.ClientSize.Width - 50);
                    int y = rand.Next(50, this.ClientSize.Height - 50);
                    position = new Point(x, y);

                    foreach (var bones in _bones)
                    {
                        double distance = Math.Sqrt(
                            Math.Pow(position.X - bones.GetPictureBox().Location.X, 2) +
                            Math.Pow(position.Y - bones.GetPictureBox().Location.Y, 2)
                        );

                        if (distance < 100)
                        {
                            validPosition = false;
                            break;
                        }
                    }
                }
                while (!validPosition);

                Bone bone = new Bone(position);
                _bones.Add(bone);
                this.Controls.Add(bone.GetPictureBox());
            }
        }


        private void SpawnRandomPoop(int count)
        {
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                int x = rand.Next(50, this.ClientSize.Width - 50); // Hindari spawn terlalu dekat dengan tepi
                int y = rand.Next(50, this.ClientSize.Height - 50);

                Poop poop = new Poop(new Point(x, y));
                _poop.Add(poop);
                this.Controls.Add(poop.GetPictureBox());
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

                    SpawnRandomBones(1);

                    foreach (var poop in _poop.ToList())
                    {
                        this.Controls.Remove(poop.GetPictureBox());
                        _poop.Remove(poop);
                    }

                    SpawnRandomPoop(2);

                    // Tambah skor
                    UpdateScore(10);
                }
            }

            foreach (var poop in _poop.ToList())
            {
                if (_player.GetPictureBox().Bounds.IntersectsWith(poop.GetPictureBox().Bounds))
                {
                    this.Controls.Remove(poop.GetPictureBox());
                    _poop.Remove(poop);

                    SpawnRandomPoop(1);

                    UpdateScore(-5);
                }
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
