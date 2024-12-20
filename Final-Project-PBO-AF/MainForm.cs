using System.Drawing;
using System.Drawing.Text;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using NAudio.Wave;

namespace Final_Project_PBO_AF
{
    public partial class MainForm : Form
    {
        private const int PlayerInitialPositionX = 900;
        private const int PlayerInitialPositionY = 450;
        private const int AnimationInterval = 100;
        private const int GameDuration = 60;
        private IWavePlayer _bgmPlayer;         // Player untuk BGM
        private AudioFileReader _bgmFile;       // File reader untuk BGM


        private System.Windows.Forms.Timer _animationTimer;
        private System.Windows.Forms.Timer _gameTimer;

        private Player _player;
        private Label _frameLabel;
        private Label _scoreLabel;
        private Label _timerLabel;
        private Label _highestScoreLabel;

        private long _totalFrameCount;
        private List<Bone> _bones;
        private List<Gold> _gold;
        private List<Poop> _poop;
        private int _score;
        private int _remainingTime;
        private SoundPlayer _bgmSound;

        public MainForm(IWavePlayer bgmPlayer)
        {
            InitializeLevel();
            this.DoubleBuffered = true;
            //PlayBackgroundMusic();
            _bgmPlayer = bgmPlayer;
            //_bgmSound = new SoundPlayer(new MemoryStream(Resource.bgm));
            //_bgmSound.PlayLooping();
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

            _highestScoreLabel = new Label
            {
                Text = "High Score: " + HighScoreForm.GetHighestScore(),
                Location = new Point(10, 100),
                AutoSize = true,
                Font = new Font("Arial", 30),
                ForeColor = Color.SaddleBrown,
                BackColor = Color.Transparent
            };
            this.Controls.Add(_highestScoreLabel);

            //_frameLabel = new Label
            //{
            //    Text = "Total frames: ",
            //    Location = new Point(10, 10),
            //    AutoSize = true,
            //    Font = new Font("Arial", 16),
            //    ForeColor = Color.SaddleBrown
            //};
            //this.Controls.Add(_frameLabel);

            _scoreLabel = new Label
            {
                Text = "Score: 0",
                Location = new Point(10, 40),
                AutoSize = true,
                Font = new Font("Arial", 30),
                ForeColor = Color.SaddleBrown,
                BackColor = Color.Transparent
            };
            this.Controls.Add(_scoreLabel);

            _timerLabel = new Label
            {
                Text = "Time: 60",
                Location = new Point(10, 160),
                AutoSize = true,
                Font = new Font("Arial", 30),
                ForeColor = Color.SaddleBrown,
                BackColor = Color.Transparent
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

        private void EndGame()  //timers end, add name, show score
        {
            _animationTimer.Stop();
            _gameTimer.Stop();

            var playerName = Microsoft.VisualBasic.Interaction.InputBox("Enter your name:", "Game Over", "Player");
            HighScoreForm.SaveHighScore(playerName, _score);

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
                Point position;
                bool validPosition;

                do
                {
                    validPosition = true;

                    int x = rand.Next(50, this.ClientSize.Width - 50);
                    int y = rand.Next(50, this.ClientSize.Height - 50);
                    position = new Point(x, y);

                    double playerDistance = Math.Sqrt(
                        Math.Pow(position.X - _player.GetPictureBox().Location.X, 2) +
                        Math.Pow(position.Y - _player.GetPictureBox().Location.Y, 2)
                    );

                    if (playerDistance < 100)
                    {
                        validPosition = false;
                        continue;
                    }

                    foreach (var bone in _bones)
                    {
                        double boneDistance = Math.Sqrt(
                            Math.Pow(position.X - bone.GetPictureBox().Location.X, 2) +
                            Math.Pow(position.Y - bone.GetPictureBox().Location.Y, 2)
                        );

                        if (boneDistance < 100)
                        {
                            validPosition = false;
                            break;
                        }
                    }
                }
                while (!validPosition);

                Poop poop = new Poop(position);
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

        private Rectangle GetPlayerCollisionBox()
        {
            // Shrink the player's collision box by 10 pixels on each side
            Rectangle bounds = _player.GetPictureBox().Bounds;
            bounds.Inflate(-10, -10); // Negative values shrink the rectangle
            return bounds;
        }

        private void CheckCollisions()
        {
            Random rand = new Random();
            Rectangle playerCollisionBox = GetPlayerCollisionBox(); // Shrunk player's collision box

            foreach (var bone in _bones.ToList())
            {
                if (playerCollisionBox.IntersectsWith(bone.GetPictureBox().Bounds))
                {
                    this.Controls.Remove(bone.GetPictureBox());
                    _bones.Remove(bone);
                    PlayBoneEffectSound(new MemoryStream(Resource.boneSound));

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
                if (playerCollisionBox.IntersectsWith(gold.GetPictureBox().Bounds))
                {
                    this.Controls.Remove(gold.GetPictureBox());
                    _gold.Remove(gold);
                    PlayGoldEffectSound(new MemoryStream(Resource.boneSound));

                    _remainingTime += 5;
                    _timerLabel.Text = "Time: " + _remainingTime;

                    _player.IncreaseSpeedTemporary(5, 15000); // Increase speed by 5 for 15 seconds

                    SpawnRandomBones(1);
                }
            }

            foreach (var poop in _poop.ToList())
            {
                if (playerCollisionBox.IntersectsWith(poop.GetPictureBox().Bounds))
                {
                    this.Controls.Remove(poop.GetPictureBox());
                    _poop.Remove(poop);
                    PlayBoneEffectSound(new MemoryStream(Resource.boneSound));

                    SpawnRandomPoop(1);

                    UpdateScore(-5);
                }
            }
        }
        private void PlayBackgroundMusic()
        {
            try
            {
                // Path ke file BGM
                string bgmPath = Path.Combine(Application.StartupPath, "Resources", "bgm.wav");
                _bgmFile = new AudioFileReader(bgmPath);  
                _bgmPlayer = new WaveOutEvent();          
                _bgmPlayer.Init(_bgmFile);                
                _bgmPlayer.Play();                       
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saat memutar BGM: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void PlayBoneEffectSound(Stream soundStream)
        {
            using (SoundPlayer effectPlayer = new SoundPlayer(soundStream))
            {
                effectPlayer.Play();
            }
        }

        private void PlayGoldEffectSound(Stream soundStream)
        {
            using (SoundPlayer effectPlayer = new SoundPlayer(soundStream))
            {
                effectPlayer.Play();
            }
        }

        private void PlayPoopEffectSound(Stream soundStream)
        {
            using (SoundPlayer effectPlayer = new SoundPlayer(soundStream))
            {
                effectPlayer.Play();
            }
        }
        private void UpdateScore(int points)
        {
            _score += points;
            _scoreLabel.Text = "Score: " + _score;
        }

        //private void UpdateFrameCount()
        //{
        //    _totalFrameCount++;
        //    _frameLabel.Text = "Total frames: " + _totalFrameCount;
        //}

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
            //UpdateFrameCount();
            _player.Animate();
            CheckCollisions();
        }
    }
}
