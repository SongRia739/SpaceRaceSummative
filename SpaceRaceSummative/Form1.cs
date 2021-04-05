using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

//Ria Das
//Two player Space race game where who ever gets 3 points first, wins
namespace SpaceRaceSummative
{
    public partial class Form1 : Form
    {
        SoundPlayer bells = new SoundPlayer(Properties.Resources.bells);

        string gameState = "waiting";

        int p1X = 260;
        int p1Y = 510;
        int p1Score = 0;

        int p2X = 320;
        int p2Y = 510;
        int p2Score = 0;

        int playerHeight = 15;
        int playerWidth = 15;
        int playerSpeed = 10;

        List<int> leftObsticalX = new List<int>();
        List<int> rightObsticalX = new List<int>();
        List<int> leftObsticalY = new List<int>();
        List<int> rightObsticalY = new List<int>();

        int obsticalWidth = 10;
        int obsticalHeight = 35;
        int obsticalSpeed = 10;

        //keys
        bool wDown = false;
        bool sDown = false;
        bool aDown = false;
        bool dDown = false;

        bool upArrow = false;
        bool downArrow = false;
        bool rightArrow = false;
        bool leftArrow = false;

        SolidBrush purpleBrush = new SolidBrush(Color.MediumPurple);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        int obsticalCounter = 0;

        Random ranGen = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrow = false;
                    break;
                case Keys.Down:
                    downArrow = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrow = true;
                    break;
                case Keys.Down:
                    downArrow = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over" || gameState == "gameWon" || gameState == "gameWon2")
                    {
                        GameInitialize();
                    }
                    break;
                //exit
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over" || gameState == "gameWon" || gameState == "gameWon2")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //game start
            if (gameState == "waiting")
            {
                titleLabel.Text = "SPACE RACE";
                subtitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "running")
            {
                //player 1 and 2
                e.Graphics.FillEllipse(purpleBrush, p1X, p1Y, playerWidth, playerHeight);
                e.Graphics.FillEllipse(purpleBrush, p2X, p2Y, playerWidth, playerHeight);

                //draw obsticals
                for (int i = 0; i < leftObsticalX.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, leftObsticalX[i], leftObsticalY[i], obsticalWidth, obsticalHeight);
                }
                for (int i = 0; i < rightObsticalX.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, rightObsticalX[i], rightObsticalY[i], obsticalWidth, obsticalHeight);
                }
            }
            else if (gameState == "gameWon")
            {
                titleLabel.Text = "RACE COMPLETE";

                subtitleLabel.Text = $"Player 1 Wins!";
                subtitleLabel.Text += "\nPress Space Bar to Play Again or Escape to Exit";
            }
            else if (gameState == "gameWon2")
            {
                titleLabel.Text = "RACE COMPLETE";

                subtitleLabel.Text = $"Player 2 Wins!";
                subtitleLabel.Text += "\nPress Space Bar to Play Again or Escape to Exit";
            }
        }

        public void GameInitialize()
        {
            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameTimer.Enabled = true;
            gameState = "running";

            p1X = 260;
            p1Y = 510;

            p2X = 320;
            p2Y = 510;
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
         
            obsticalCounter++;

            //move player 1
            if (wDown == true && p1Y > 0)
            {
                p1Y -= playerSpeed;
            }
            if (sDown == true && p1Y < this.Height - playerHeight)
            {
                p1Y += playerSpeed;
            }

            //mover player 2
            if (upArrow == true && p2Y > 0)
            {
                p2Y -= playerSpeed;
            }
            if (downArrow == true && p2Y < this.Height)
            {
                p2Y += playerSpeed;
            }

            //move obsticals
            if (obsticalCounter == 3)
            {
                leftObsticalY.Add(ranGen.Next(10, this.Height - 50));
                leftObsticalX.Add(10);

                rightObsticalY.Add(ranGen.Next(10, this.Height - 50));
                rightObsticalX.Add(this.Width - obsticalWidth);

                obsticalCounter = 0;
            }
            for (int i = 0; i < leftObsticalX.Count(); i++)
            {
                leftObsticalX[i] += obsticalSpeed;
            }
            for (int i = 0; i < leftObsticalX.Count(); i++)
            {
                if (leftObsticalX[i] > this.Width)
                {
                    leftObsticalX.RemoveAt(i);
                    leftObsticalY.RemoveAt(i);
                }
            }
            for (int i = 0; i < rightObsticalX.Count(); i++)
            {
                rightObsticalX[i] += obsticalSpeed;

            }
            for (int i = 0; i < rightObsticalX.Count(); i++)
            {

                if (rightObsticalX[i] > this.Width)
                {
                    rightObsticalX.RemoveAt(i);
                    rightObsticalY.RemoveAt(i);
                }
            }
            //collision 
            Rectangle p1Rec = new Rectangle(p1X, p1Y, playerWidth, playerHeight);
            Rectangle p2Rec = new Rectangle(p2X, p2Y, playerWidth, playerHeight);

            for (int i = 0; i < leftObsticalX.Count(); i++)
            {
                Rectangle leftObsticalRec = new Rectangle(leftObsticalX[i], leftObsticalY[i], obsticalWidth, obsticalHeight);

                if (p1Rec.IntersectsWith(leftObsticalRec))
                {
                    bells.Play();
                    p1Y = 510;
                }
                if (p2Rec.IntersectsWith(leftObsticalRec))
                {
                    bells.Play();
                    p2Y = 510;
                }
            }
            for (int i = 0; i < rightObsticalX.Count(); i++)
            {
                Rectangle rightObsticalRec = new Rectangle(rightObsticalX[i], rightObsticalY[i], obsticalWidth, obsticalHeight);

                if (p1Rec.IntersectsWith(rightObsticalRec))
                {
                    bells.Play();
                    p1Y = 510;
                }
                if (p2Rec.IntersectsWith(rightObsticalRec))
                {
                    bells.Play();
                    p2Y = 510;
                }
            }

            //score
            if (p1Y < 3)
            {
                p1Score++;
                p1ScoreLabel.Text = $"{p1Score}";

                p1Y = 510;
            }
            else if (p2Y < 3)
            {
                p2Score++;
                p2ScoreLabel.Text = $"{p2Score}";

                p2Y = 510;
            }

            //win
            if (p1Score == 3)
            {
                gameTimer.Enabled = false;
                gameState = "gameWon";
            }
            else if (p2Score == 3)
            {
                gameTimer.Enabled = false;
                gameState = "gameWon2";
            }

            Refresh();
        }
    }
}
