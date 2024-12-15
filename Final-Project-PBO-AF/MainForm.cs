using System.Drawing;
using System.Media;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Final_Project_PBO_AF
{
    public partial class MainForm : Form
    {
        private const int PlayerInitialPositionX = 900;
        private const int PlayerInitialPositionY = 450;
        private const int AnimationInterval = 100;
        private const int GameDuration = 60;

        private System.Windows.Forms.Timer _animationTimer;
        private System.Windows.Forms.Timer _gameTimer;

        private Player _player;
        private Label _frameLabel;
        private Label _scoreLabel;
        private Label _timerLabel;

        private long _totalFrameCount;
        private List<Bone> _bones;
        private List<Gold> _gold;
        private List<Poop> _poop;
        private int _score;
        private int _remainingTime;
        private SoundPlayer _GoldSound;
        private SoundPlayer _poopSound;
        private SoundPlayer _boneSound;

        public MainForm()
        {
            InitializeLevel();
            this.DoubleBuffered = true;

            _boneSound = new SoundPlayer(new MemoryStream(Resource.boneSound));
            _GoldSound = new SoundPlayer(new MemoryStream(Resource.goldBoneSound));
            _poopSound = new SoundPlayer(new MemoryStream(Resource.poopSound));
        }

        private void InitializeLevel()
        {
            this.Text = "Bone Hunt";
            this.FormBorderStyle = FormBorderStyle.Sizable; // Start in maximized
            this.WindowState = FormWindowState.Maximized;
            this.BackgroundImage = Image.FromStream(new MemoryStream(Resource.grass)); 

            this.BackColor = Color.LightGray;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;

            _animationTimer = new System.Windows.Forms.Timer { Interval = AnimationInterval };
            _animationTimer.Tick += (sender, e) => Render();
            _animationTimer.Start();

            _gameTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _gameTimer.Tick += UpdateGameTimer;
            _remainingTime = GameDuration;

            _bones = new List<Bone>();
            _poop = new List<Poop>();
            _gold = new List<Gold>();
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

            _timerLabel = new Label
            {
                Text = "Time: 60",
                Location = new Point(10, 70),
                AutoSize = true,
                Font = new Font("Arial", 16),
                ForeColor = Color.Black
            };
            this.Controls.Add(_timerLabel);
        }

        protected override void OnShown(EventArgs e) //spawn after canvas is fully loaded
        {
            base.OnShown(e);

            SpawnRandomBones(3);
            SpawnRandomPoop(2);

            _gameTimer.Start();
        }

        private void UpdateGameTimer(object sender, EventArgs e)
        {
            _remainingTime--;
            _timerLabel.Text = "Time: " + _remainingTime;

            if (_remainingTime <= 0)
            {
                EndGame();
            }
        }

        private void EndGame() // game end if timer stop
        {
            _animationTimer.Stop();
            _gameTimer.Stop();

            MessageBox.Show($"Game Over! Your final score is: {_score}", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void SpawnRandomBones(int count) // bones doesnt overlapped
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
                int x = rand.Next(50, this.ClientSize.Width - 50);
                int y = rand.Next(50, this.ClientSize.Height - 50);

                Poop poop = new Poop(new Point(x, y));
                _poop.Add(poop);
                this.Controls.Add(poop.GetPictureBox());
            }
        }

        private void SpawnRandomGold(int count)
        {
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                int x = rand.Next(50, this.ClientSize.Width - 50);
                int y = rand.Next(50, this.ClientSize.Height - 50);

                Gold gold = new Gold(new Point(x, y));
                _gold.Add(gold);
                this.Controls.Add(gold.GetPictureBox());
            }
        }

        private void CheckCollisions()
        {
            Random rand = new Random();

            foreach (var bone in _bones.ToList())
            {
                if (_player.GetPictureBox().Bounds.IntersectsWith(bone.GetPictureBox().Bounds))
                {
                    _boneSound.Play();
                    this.Controls.Remove(bone.GetPictureBox());
                    _bones.Remove(bone);

                    if (rand.Next(0, 100) < 10) // 5% chance of getting golden bone
                    {
                        SpawnRandomGold(1);
                    }
                    else
                    {
                        SpawnRandomBones(1);
                    }


                    foreach (var poop in _poop.ToList())
                    {
                        this.Controls.Remove(poop.GetPictureBox());
                        _poop.Remove(poop);
                    }

                    SpawnRandomPoop(2);

                    UpdateScore(10);
                }
            }

            foreach (var gold in _gold.ToList())
            {
                if (_player.GetPictureBox().Bounds.IntersectsWith(gold.GetPictureBox().Bounds))
                {
                    _GoldSound.Play();
                    this.Controls.Remove(gold.GetPictureBox());
                    _gold.Remove(gold);

                    _remainingTime += 5;
                    _timerLabel.Text = "Time: " + _remainingTime;

                    _player.IncreaseSpeedTemporary(5, 5000); // Tambah kecepatan 5 selama 5 detik

                    SpawnRandomBones(1);
                }
            }

            foreach (var poop in _poop.ToList())
            {
                if (_player.GetPictureBox().Bounds.IntersectsWith(poop.GetPictureBox().Bounds))
                {
                    _poopSound.Play();
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
