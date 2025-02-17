using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPong
{
    public partial class Form1 : Form
    {
        int ballXspeed = 4;
        int ballYspeed = 4;
        int speed = 2;
        Random rand = new Random();
        bool goDown, goUp;
        int comp_speed_change = 50;
        int playerScore = 0;
        int compScore = 0;
        int playerSpeed = 8;
        int[] i = { 5, 6, 8, 9 }; //comp speed {need to use rand to select data and will assign to speed variable}
        int[] j = { 10, 9, 8, 11, 12}; // will use to assign the diff speed to ballX and ballY

        public Form1()
        {
            InitializeComponent();
        }

        private void GameTimeEvent(object sender, EventArgs e)
        {
            ball.Top -= ballYspeed;                 //Note: Kaya -= mag Move yung ball depends kung positive or negative yung value
            ball.Left -= ballXspeed;

            this.Text = "Player Score:" + playerScore + "-----" +"Computer Score:" + compScore;

            if(ball.Top < 0 || ball.Bottom > this.ClientSize.Height)              //clientSize is height ng screen kaya din may top and buttom para sa height ng screen
            {
                ballYspeed =-ballYspeed;
            }

            //for comp score
            if (ball.Left < -2)              //pag na reach yung left border or na miss yung ball
            {                                          
                ball.Left = 300;                //mag rerest yung ball sa middle kaya may 300
                ballXspeed =-ballXspeed;            //ball will move to opposite direction
                compScore++;                        //matic mag add dito yung score ng comp
            }

            //for player score
            if(ball.Right > this.ClientSize.Width + 2)
            {
                ball.Left = 300;
                ballXspeed =-ballXspeed;
                playerScore++;
            }

            //for comp movement 
            if(computer.Top <= 1)
            {
                computer.Top = 0;
            }
            else if(computer.Bottom >= this.ClientSize.Height)
            {
                computer.Top = this.ClientSize.Height - computer.Height;
            }                                             //eto ay height ng comp picture box

            if(ball.Top < computer.Top +(computer.Height /2) && ball.Left >= 300)    //computer.Top +(computer.Height /2) will make the comp follow the ball from the center of the comp paddle
            {                                                                        
                computer.Top -= speed;
            }
            if (ball.Top > computer.Top + (computer.Height / 2) && ball.Left >= 300)     //if the ball is moving down
            {
                computer.Top += speed;
            }

            comp_speed_change -= 1;              //default speed
            if(comp_speed_change < 0)               //kapag nag reach ng 0 yung speed mamimili ng random number sa array
            {                                       //tapos mag rereset ang integer sa 50 sa default speed
                speed = i[rand.Next(i.Length)];
                comp_speed_change = 50;
            }

            if(goDown && Player.Top + Player.Height < this.ClientSize.Height)
            {
                Player.Top += playerSpeed;
            }
            if(goUp && Player.Top > 0)               //if the player goes up it goes true and changes the speed
            {
                Player.Top -= playerSpeed;
            }

            CheckCollision(ball, Player, Player.Right + 5);               //note kaya may oofset para hindi mag click sa picture box and para walang glitch
            CheckCollision(ball, computer, computer.Left - 35);        //minus kase yung comp nasa right, plus naman sa player kasi nasa left

            if(compScore > 5)
            {
                GameOver("Sorry you loss");
            }
            else if(playerScore > 5)
            {
                GameOver("You win the game");
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if(e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;             //pag narelease ung key up or down automatic false
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
        }

        private void CheckCollision(PictureBox PicOne, PictureBox PicTwo, int offset) 
        { 
            if(PicOne.Bounds.IntersectsWith(PicTwo.Bounds))
            {
                PicOne.Left = offset;

                int x = j[rand.Next(j.Length)];  //will only select one random value sa array
                int y = j[rand.Next(j.Length)];

                if(ballXspeed < 0)          //kapag yung ball ay moving sa left magiging less than 0 
                {                           //pero kapag right magiging greater than 0 kaya -x yung else
                    ballXspeed = x;
                }
                else
                {
                    ballXspeed = -x;
                }

                if (ballYspeed < 0)          //kapag yung ball ay moving up the new value ay magiging negative
                {                            //mag change yung spped naman depende sa value ng array
                    ballYspeed = -y;
                }
                else 
                {
                    ballYspeed = y;           //for moving down
                }
            }
        }

        private void GameOver(string message)
        {
            GameTimer.Stop();
            MessageBox.Show(message, "Ano na");
            compScore = 0;
            playerScore = 0;
            ballXspeed = ballYspeed = 4;
            comp_speed_change = 50;
            GameTimer.Start();
        }

    }
}
