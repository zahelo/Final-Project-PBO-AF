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
        }
    }
}
