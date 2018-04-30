using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace snake_new
{
    public partial class Form1 : Form
    {
        Graphics dArea;
        static string dir;
        //colors
        SolidBrush sB = new SolidBrush(Color.Silver);       //snake body
        SolidBrush sH = new SolidBrush(Color.Gray);         //snake head
        SolidBrush f = new SolidBrush(Color.Green);         //food
        SolidBrush B = new SolidBrush(Color.Black);         //borders
        SolidBrush GO = new SolidBrush(Color.Red);          //opps

        struct position
        {
            public int x;
            public int y;
        }
        position food, temp2, sHead;

        Queue<position> snake = new Queue<position>();
        int wait = 250;

        private void play_Click(object sender, EventArgs e)
        {
            play.Enabled = false;
            Task.Factory.StartNew(()=> {
                Random r = new Random();

                sHead.x = (r.Next(2, 36)) * 10;
                sHead.y = (r.Next(2, 31)) * 10;
                snake.Enqueue(sHead);
                dArea.Clear(Color.WhiteSmoke);
                foreach (position t in snake)
                {
                    dArea.FillRectangle(sB, t.x, t.y, 10, 10);
                }
                dArea.FillRectangle(sH, snake.Peek().x, snake.Peek().y, 10, 10);

                food.x = 300;
                food.y = 250;
                dArea.FillEllipse(f, food.x, food.y, 10, 10);
                while (true) {
                    dArea.Clear(Form1.DefaultBackColor);
                    dArea.FillRectangle(B, 0, 0, 10, 320);
                    dArea.FillRectangle(B, 0, 0, 370, 10);
                    dArea.FillRectangle(B, 0, 320, 370, 10);
                    dArea.FillRectangle(B,370, 0, 10, 330);

                    dArea.FillRectangle(sH, sHead.x, sHead.y, 10, 10);
                    foreach (position i in snake)
                    {
                        dArea.FillRectangle(sB, i.x, i.y, 10, 10);
                    }
                    dArea.FillEllipse(f, food.x, food.y, 10, 10);
                    snake.Enqueue(sHead);

                    switch (dir)
                    {
                        case "up":
                            sHead.y -= 10;
                            break;
                        case "down":
                            sHead.y += 10;
                            break;
                        case "right":
                            sHead.x += 10;
                            break;
                        case "left":
                            sHead.x -= 10;
                            break;
                    }
                    //game over
                    if ((sHead.x < 10)||(sHead.x > 360)||(sHead.y < 10) || (sHead.y > 310))
                    {
                        dArea.Clear(Form1.DefaultBackColor);
                        dArea.FillRectangle(B, 0, 0, 10, 320);
                        dArea.FillRectangle(B, 0, 0, 370, 10);
                        dArea.FillRectangle(B, 0, 320, 370, 10);
                        dArea.FillRectangle(B, 370, 0, 10, 330);
                        foreach (position i in snake)
                        {
                            dArea.FillRectangle(sB, i.x, i.y, 10, 10);
                        }
                        dArea.FillRectangle(GO, sHead.x, sHead.y, 10, 10);
                        break;
                    }
                    else if (snake.Contains(sHead))
                    {
                        dArea.Clear(Form1.DefaultBackColor);
                        dArea.FillRectangle(B, 0, 0, 10, 320);
                        dArea.FillRectangle(B, 0, 0, 370, 10);
                        dArea.FillRectangle(B, 0, 320, 370, 10);
                        dArea.FillRectangle(B, 370, 0, 10, 330);
                        foreach (position i in snake)
                        {
                            dArea.FillRectangle(sB, i.x, i.y, 10, 10);
                        }
                        dArea.FillRectangle(GO, sHead.x, sHead.y, 10, 10);
                        break;
                    }

                    //eating
                    if ((sHead.x == food.x) && (sHead.y == food.y))
                    {
                        snake.Enqueue(food);
                        while(snake.Contains(food))
                        {
                            food.x = (r.Next(2, 36)) * 10;
                            food.y = (r.Next(2, 31)) * 10;
                        }
                        //go faster
                        if (wait > 100)
                        {
                            wait -= 5;
                        }

                    }
                    else
                    {
                             temp2 = snake.Dequeue();
                    }

                    Thread.Sleep(wait);
                }
                snake.Clear();
            });
            play.Enabled = true;

        }

        public Form1()
        {
            InitializeComponent();
            dArea = drawingArea.CreateGraphics();
            dir = "right";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            char st = (char) e.KeyData;
            switch (st)
            {
                case 'W':
                    if (dir != "down")
                    { dir = "up"; }
                    break;
                case 'S':
                    if (dir != "up")
                    { dir = "down"; }
                    break;
                case 'A':
                    if (dir != "right")
                    { dir = "left"; }
                    break;
                case 'D':
                    if (dir != "left")
                    { dir = "right"; }
                    break;
            }
            Thread.Sleep(wait);
        }
    }
}
