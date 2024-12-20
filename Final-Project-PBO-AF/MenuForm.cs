﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace Final_Project_PBO_AF
{
    public partial class MenuForm : Form
    {
        private IWavePlayer _bgmPlayer;
        private AudioFileReader _bgmFile;
        private Label _gameTitleLabel;
        public MenuForm()
        {
            InitializeForm();
            AddGameTitle();
            InitializeControls();
            PlayBackgroundMusic();

            Application.ApplicationExit += OnApplicationExit;
        }

        private void AddGameTitle()
        {
            var gameTitle = new OutlinedLabel
            {
                DisplayText = "BONE HUNT",
                DisplayFont = LoadCustomFont("Resources/Fonts/CuteDog-d94AK.ttf", 100, FontStyle.Regular),
                OutlineColor = Color.SaddleBrown, // Warna outline
                TextColor = ColorTranslator.FromHtml("#EFC482"), // Warna teks Shiba Inu
                OutlineWidth = 4, // Ketebalan outline
                AutoSize = false,
                Size = new Size(800, 120),
                Location = new Point((this.ClientSize.Width - 800) / 2, this.ClientSize.Height / 6),
                BackColor = Color.Transparent // Mengaktifkan transparansi
            };

            this.Controls.Add(gameTitle);

            this.Resize += (s, e) =>
            {
                gameTitle.Location = new Point((this.ClientSize.Width - 1500) / 2, this.ClientSize.Height / 6);
            };
        }

            private Font LoadCustomFont(string fontFilePath, float size, FontStyle style)
        {
            PrivateFontCollection pfc = new PrivateFontCollection();

            string fullPath = Path.Combine(Application.StartupPath, "Resources", "Fonts", "CuteDog-d94AK.ttf");
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("Font file not found", fullPath);
            }

            pfc.AddFontFile(fullPath);
            return new Font(pfc.Families[0], size, style);
        }
        private void PlayBackgroundMusic()
        {
            try
            {
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

            int formCenterX = (this.ClientSize.Width - startGameButton.Width) / 2;
            int verticalOffset = this.ClientSize.Height / 2; // Pusat layar untuk menu
            int spacing = 20; // Jarak antar tombol

            startGameButton.Location = new Point((this.ClientSize.Width - startGameButton.Width) / 2, 300);
            highScoreButton.Location = new Point((this.ClientSize.Width - highScoreButton.Width) / 2, 400);
            exitButton.Location = new Point((this.ClientSize.Width - exitButton.Width) / 2, 500);

            this.Controls.Add(startGameButton);
            this.Controls.Add(highScoreButton);
            this.Controls.Add(exitButton);

            // set button to stay in the middle of the form
            this.Resize += (s, e) =>
            {
                formCenterX = (this.ClientSize.Width - startGameButton.Width) / 2;
                verticalOffset = this.ClientSize.Height / 2;

                startGameButton.Location = new Point(formCenterX, verticalOffset);
                highScoreButton.Location = new Point(formCenterX, startGameButton.Bottom + spacing);
                exitButton.Location = new Point(formCenterX, highScoreButton.Bottom + spacing);
            };
        }
        private void StartGameButton_Click(object sender, EventArgs e)
        {
            // after click game start, it will navigate to main form 
            MainForm gameForm = new MainForm(_bgmPlayer);
            gameForm.FormClosed += (s, args) => this.Show(); // show menu form if main form closed
            gameForm.Show();
            this.Hide();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            // Hentikan musik dan bersihkan resource saat aplikasi ditutup
            _bgmPlayer?.Stop();
            _bgmPlayer?.Dispose();
            _bgmFile?.Dispose();
        }
        protected override void OnClosed(EventArgs e)
        {
            // Hentikan musik dan bersihkan resource saat MenuForm ditutup
            base.OnClosed(e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.WindowState = FormWindowState.Normal; 
        this.FormBorderStyle = FormBorderStyle.Sizable; 
        this.StartPosition = FormStartPosition.CenterScreen; 
        this.Size = new Size(800, 600); 
        return true; 
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
