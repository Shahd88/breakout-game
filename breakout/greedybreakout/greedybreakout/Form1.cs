using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace greedybreakout
{
    public partial class Form1 : Form
    {
        bool goLeft;
        bool goRight;
        bool isGameOver;
        int score;
        int ballx;
        int bally;
        int playerSpeed;
        Random rnd = new Random();
        PictureBox[] blockArray;

        public Form1()
        {
            InitializeComponent();
            PlaceBlocks();
            setupGame();
        }

        private void setupGame()
        {
            isGameOver = false;
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 12;
            txtScore.Text = "Score: " + score;
            ball.Left = 376;
            ball.Top = 328;
            player.Left = 347;
            gameTimer.Start();
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }
        }

        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Score: " + score + " " + message;
        }

        private void PlaceBlocks()
        {
            blockArray = new PictureBox[15];
            int a = 0;
            int top = 50;
            int left = 100;
            for (int i = 0; i < blockArray.Length; i++)
            {
                blockArray[i] = new PictureBox();
                blockArray[i].Height = 32;
                blockArray[i].Width = 100;
                blockArray[i].Tag = "blocks";
                blockArray[i].BackColor = Color.White;
                if (a == 5)
                {
                    top = top + 50;
                    left = 100;
                    a = 0;
                }
                if (a < 5)
                {
                    a++;
                    blockArray[i].Left = left;
                    blockArray[i].Top = top;
                    this.Controls.Add(blockArray[i]);
                    left = left + 130;
                }
            }
        }

        private void removeBlocks()
        {
            foreach (PictureBox x in blockArray)
            {
                this.Controls.Remove(x);
            }
        }

        private void mainGameTimer(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            if (goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true && player.Left < 700)
            {
                player.Left += playerSpeed;
            }

            // تنفيذ البحث بخوارزمية Greedy Search
            MovePlayerGreedy();

            ball.Left += ballx;
            ball.Top += bally;

            // تحقق من نهاية اللعبة
            CheckGameOver();
        }

        private void MovePlayerGreedy()
        {
            // تحديد موقع الكرة واللاعب
            int ballX = ball.Left + ball.Width / 2;
            int playerX = player.Left + player.Width / 2;

            // تحديد الحركة المناسبة بناءً على موقع الكرة بالنسبة لللاعب
            if (ballX < playerX && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }
            else if (ballX > playerX && player.Left < 700)
            {
                player.Left += playerSpeed;
            }
        }

        private void CheckGameOver()
        {
            // تحقق من نهاية اللعبة
            if (score == 15)
            {
                gameOver("You Win!! Press Enter to Play Again");
            }
            else if (ball.Top > 580)
            {
                gameOver("You Lose!! Press Enter to try again");
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
        }

        private void keeyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeBlocks();
                PlaceBlocks();
                setupGame();
            }
        }
    }
}
