/* Esther Bae
 * Mr.T
 * June 14, 2021
 * Two player game where the players try to get to the other side without colliding with the asteroids.
 * Whoever gets 3 points first wins
 */

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

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        //global variables
        Rectangle player1 = new Rectangle(280, 500, 10, 30);
        Rectangle player2 = new Rectangle(500, 500, 10, 30);

        Rectangle end = new Rectangle(0, 1, 800, 1);

        int playerSpeed = 8;
        int p1Score = 0;
        int p2Score = 0;

        List<int> asteroidRY = new List<int>();
        List<int> asteroidRX = new List<int>();
        List<int> asteroidLY = new List<int>();
        List<int> asteroidLX = new List<int>();
        int asteroidRightSpeed = 7;
        int asteroidLeftSpeed = 6;
        int asteroidHeight = 10;
        int asteroidLength = 10;

        bool upArrowDown = false;
        bool downArrowDown = false;
        bool sDown = false;
        bool wDown = false;

        string gameState = "waiting";
        string win;

        Random randGen = new Random();
        int randValue = 0;

        SolidBrush pinkBrush = new SolidBrush(Color.LightCoral);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        SoundPlayer collision = new SoundPlayer(Properties.Resources.collision);
        SoundPlayer score = new SoundPlayer(Properties.Resources.score);
        SoundPlayer gameOver = new SoundPlayer(Properties.Resources.gameOver);

        public Form1()
        {
            InitializeComponent();
        }

        //when space bar is pressed
        public void GameInitialize()
        {
            titleLabel.Text = "";
            subtitleLabel.Text = "";
            p1ScoreOutput.Text = "0";
            p2ScoreOutput.Text = "0";

            gameTimer.Enabled = true;
            gameState = "running";

            p1Score = 0;
            p2Score = 0;
            player1.X = 280;
            player1.Y = 500;
            player2.X = 500;
            player2.Y = 500;

            asteroidLX.Clear();
            asteroidLY.Clear();
            asteroidRX.Clear();
            asteroidRY.Clear();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move players
            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
            }

            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }

            //create asteroids
            randValue = randGen.Next(0, 101);

            if (randValue < 7)
            {
                asteroidRX.Add(800);
                asteroidRY.Add(randGen.Next(10, this.Height - 50));
            }
            else if (randValue < 14)
            {
                asteroidLX.Add(0);
                asteroidLY.Add(randGen.Next(10, this.Height - 50));
            }

            //move asteroids
            for (int i = 0; i < asteroidRX.Count(); i++)
            {
                asteroidRX[i] -= asteroidRightSpeed;
            }

            for (int i = 0; i < asteroidLX.Count(); i++)
            {
                asteroidLX[i] += asteroidLeftSpeed;
            }

            //collision with player
            for (int i = 0; i < asteroidRX.Count(); i++)
            {
                Rectangle asteroid1 = new Rectangle(asteroidRX[i], asteroidRY[i], asteroidLength, asteroidHeight);
                asteroidRX[i] -= asteroidRightSpeed;

                if (player1.IntersectsWith(asteroid1))
                {
                    collision.Play();
                    player1.X = 280;
                    player1.Y = 500;
                }
                else if (player2.IntersectsWith(asteroid1))
                {
                    collision.Play();
                    player2.X = 500;
                    player2.Y = 500;
                }
            }

            for (int i = 0; i < asteroidLX.Count(); i++)
            {
                Rectangle asteroid2 = new Rectangle(asteroidLX[i], asteroidLY[i], asteroidLength, asteroidHeight);
                asteroidLX[i] += asteroidLeftSpeed;

                if (player1.IntersectsWith(asteroid2))
                {
                    collision.Play();
                    player1.X = 280;
                    player1.Y = 500;
                }
                else if (player2.IntersectsWith(asteroid2))
                {
                    collision.Play();
                    player2.X = 500;
                    player2.Y = 500;
                }
            }

            //add point when player makes it to the end
            if (player1.IntersectsWith(end))
            {
                score.Play();
                player1.X = 280;
                player1.Y = 500;

                p1Score++;
                p1ScoreOutput.Text = $"{p1Score}";
            }

            if (player2.IntersectsWith(end))
            {
                score.Play();
                player2.X = 500;
                player2.Y = 500;

                p2Score++;
                p2ScoreOutput.Text = $"{p2Score}";
            }

            //end game if a player gets to 3 points first
            if (p1Score == 3)
            {
                gameState = "over";
                win = "player 1 wins!";
            }
            else if (p2Score == 3)
            {
                gameState = "over";
                win = "player 2 wins!";
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                titleLabel.Text = "space race 3000!";
                subtitleLabel.Text = "press space to start or escape to exit";
            }
            else if (gameState == "running")
            {
                //draw players
                e.Graphics.FillRectangle(pinkBrush, player1);
                e.Graphics.FillRectangle(pinkBrush, player2);

                //draw asteroids
                for (int i = 0; i < asteroidRX.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidRX[i], asteroidRY[i], asteroidLength, asteroidHeight);
                }

                for (int i = 0; i < asteroidLX.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidLX[i], asteroidLY[i], asteroidLength, asteroidHeight);
                }
            }

            //if a player reaches 3 points
            if (gameState == "over")
            {
                gameOver.Play();

                titleLabel.Text = "play again?";
                subtitleLabel.Text = $"{win} \n\n press space to play again or escape to exit";
                p1ScoreOutput.Text = "";
                p2ScoreOutput.Text = "";
                gameTimer.Enabled = false;
            }
        }
    }
}