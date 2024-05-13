using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace brealoutdls
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

        // تحديد العمق الأقصى للبحث
        private const int MAX_DEPTH = 5;

        public Form1()
        {
            InitializeComponent();
            PlaceBlocks();
        }

        private void setupGame()
        {
            isGameOver = false;
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 12;
            gameTimer.Interval = 100; // تغيير فترة التحديث إلى 50 مللي ثانية

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
            setupGame();
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

            ball.Left += ballx;
            ball.Top += bally;
            if (ball.Left < 0 || ball.Left > 775)
            {
                ballx = -ballx;
            }
            if (ball.Top < 0)
            {
                bally = -bally;
            }
            if (ball.Bounds.IntersectsWith(player.Bounds))
            {
                ball.Top = 463;
                bally = rnd.Next(5, 12) * -1;
                if (ballx < 0)
                {
                    ballx = rnd.Next(5, 12) * -1;
                }
            }
            else
            {
                ballx = rnd.Next(5, 12);
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;
                        bally = -bally;
                        this.Controls.Remove(x);
                    }
                }
            }
            if (score == 15)
            {
                gameOver("You Win!! Press Enter to Play Again");
            }
            if (ball.Top > 580)
            {
                gameOver("You Lose!! Press Enter to try again");
            }





            // تنفيذ خوارزمية DLS
            DLS(ballx, bally, 0);
        }

        private void DLS(int ballSpeedX, int ballSpeedY, int depth)
        {
            if (depth >= MAX_DEPTH) // التحقق من الوصول إلى العمق المسموح به
            {
                return;
            }

            // قم بتقييم الحالة الحالية لتحديد الحركة الأمثل
            int bestMove = EvaluateGameState(ballSpeedX, ballSpeedY);

            // اتخاذ الحركة الأمثل
            switch (bestMove)
            {
                case 0: // الحركة إلى اليسار
                    ball.Left -= ballSpeedX;
                    break;
                case 1: // الحركة إلى اليمين
                    ball.Left += ballSpeedX;
                    break;
                default:
                    break;
            }

            // تحديث حركة الكرة
            ball.Left += ballSpeedX;
            ball.Top += ballSpeedY;

            // استمرار البحث بعمق أقل
            DLS(ballSpeedX, ballSpeedY, depth + 1);
        }

        private int EvaluateGameState(int ballSpeedX, int ballSpeedY)
        {
            // تحديد موقع الكرة واللاعب
            int ballX = ball.Left + ball.Width / 2;
            int playerX = player.Left + player.Width / 2;

            // تحديد الحركة المثلى بناءً على موقع الكرة بالنسبة لللاعب
            if (ballX < playerX)
            {
                return 0; // الحركة إلى اليسار
            }
            else
            {
                return 1; // الحركة إلى اليمين
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

        private void keyisup(object sender, KeyEventArgs e)
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
            }
        }


    }
}

