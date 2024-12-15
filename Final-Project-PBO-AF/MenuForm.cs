using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeForm();
            InitializeControls();
        }

        private void InitializeForm()
        {
            this.Text = "Game Menu";
            this.WindowState = FormWindowState.Maximized; //set full screen
            this.StartPosition = FormStartPosition.CenterScreen;

            try
            {
                using (MemoryStream ms = new MemoryStream(Resource.background)) 
                {
                    this.BackgroundImage = Image.FromStream(ms);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Gagal memuat background: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void InitializeControls()
        {
            var startGameButton = new Button
            {
                Text = "Start Game",
                Location = new Point(150, 50),
                Size = new Size(300, 90)
            };
            startGameButton.Click += StartGameButton_Click;

            var exitButton = new Button
            {
                Text = "Exit",
                Location = new Point(150, 100),
                Size = new Size(300, 90)
            };
            exitButton.Click += (s, e) => this.Close();

            var highScoreButton = new Button
            {
                Text = "High Scores",
                Location = new Point(150, 150),
                Size = new Size(300, 90)
            };
            highScoreButton.Click += (s, e) =>
            {
                HighScoreForm highScoreForm = new HighScoreForm();
                highScoreForm.ShowDialog();
            };

            startGameButton.Location = new Point((this.ClientSize.Width - startGameButton.Width) / 2, 300);
            highScoreButton.Location = new Point((this.ClientSize.Width - highScoreButton.Width) / 2, 400);
            exitButton.Location = new Point((this.ClientSize.Width - exitButton.Width) / 2, 500);

            this.Controls.Add(startGameButton);
            this.Controls.Add(highScoreButton);
            this.Controls.Add(exitButton);

            // set button to stay in the middle of the form
            this.Resize += (s, e) =>
            {
                startGameButton.Location = new Point((this.ClientSize.Width - startGameButton.Width) / 2, startGameButton.Location.Y);
                highScoreButton.Location = new Point((this.ClientSize.Width - highScoreButton.Width) / 2, highScoreButton.Location.Y);
                exitButton.Location = new Point((this.ClientSize.Width - exitButton.Width) / 2, exitButton.Location.Y);
            };
        }
        private void StartGameButton_Click(object sender, EventArgs e)
        {
            // after click game start, it will navigate to main form 
            MainForm gameForm = new MainForm();
            gameForm.FormClosed += (s, args) => this.Show(); // show menu form if main form closed
            gameForm.Show();
            this.Hide();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.WindowState = FormWindowState.Normal; // Kembali ke ukuran normal
        this.FormBorderStyle = FormBorderStyle.Sizable; // Tampilkan kembali border
        this.StartPosition = FormStartPosition.CenterScreen; // Pusatkan form
        this.Size = new Size(800, 600); // Atur ukuran default
        return true; // Beri tahu sistem bahwa tombol sudah ditangani
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
