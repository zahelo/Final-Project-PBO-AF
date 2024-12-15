using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Final_Project_PBO_AF
{
    public partial class HighScoreForm : Form
    {
        private const string HighScoreFile = "highscores.txt";

        public HighScoreForm()
        {
            InitializeComponent();
            LoadHighScores();
        }

        private void InitializeComponent()
        {
            this.Text = "High Scores";
            this.WindowState = FormWindowState.Maximized;
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

            var closeButton = new Button
            {
                Text = "Back to Menu",
                Location = new System.Drawing.Point(150, 100),
                Size = new System.Drawing.Size(300, 90)
            };
            closeButton.Click += (s, e) => this.Close();

            this.Controls.Add(closeButton);

            closeButton.Location = new System.Drawing.Point((this.ClientSize.Width - closeButton.Width) / 2, 800);
            this.Resize += (s, e) =>
            {
                closeButton.Location = new System.Drawing.Point((this.ClientSize.Width - closeButton.Width) / 2, closeButton.Location.Y);
            };
        }

        private void LoadHighScores()
        {
            var highScoreLabel = new Label
            {
                Text = "High Scores:\n",
                Font = new System.Drawing.Font("Arial", 20),
                AutoSize = true,
                Location = new System.Drawing.Point(50, 50),
                ForeColor = System.Drawing.Color.Black
            };

            List<(string name, int score)> highScores = ReadHighScores();

            int rank = 1;
            foreach (var (name, score) in highScores)
            {
                highScoreLabel.Text += $"{rank}. {name}: {score}\n";
                rank++;
            }

            this.Controls.Add(highScoreLabel);
        }

        public static void SaveHighScore(string name, int score)
        {
            List<(string name, int score)> highScores = ReadHighScores();
            highScores.Add((name, score));

            highScores = highScores
                .OrderByDescending(h => h.score)
                .Take(10) // Only keep the top 10 scores
                .ToList();

            File.WriteAllLines(HighScoreFile, highScores.Select(h => $"{h.name}:{h.score}"));
        }

        private static List<(string name, int score)> ReadHighScores()
        {
            if (!File.Exists(HighScoreFile))
                return new List<(string name, int score)>();

            return File.ReadAllLines(HighScoreFile)
                .Select(line => line.Split(':'))
                .Where(parts => parts.Length == 2 && int.TryParse(parts[1], out _))
                .Select(parts => (parts[0], int.Parse(parts[1])))
                .OrderByDescending(h => h.Item2)
                .ToList();
        }

        public static int GetHighestScore()
        {
            var highScores = ReadHighScores();
            return highScores.Any() ? highScores.First().score : 0;
        }

        public static string GetHighestScorer()
        {
            var highScores = ReadHighScores();
            return highScores.Any() ? highScores.First().name : "N/A";
        }
    }
}
