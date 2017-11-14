using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Transparent_Moorhuhn
{
    public partial class Form1 : Form
    {
        //Löcher beim danebenschiessen/Punkte/
        struct Huhn
        {
            public String art;
            public int speed;
            public bool lebendig;
            public int frame;
            public Rectangle positionsize;
        }
        Image sprites = Properties.Resources.Sprites_Moorhuhn;
        List<Huhn> Hühner;
        List<Rectangle> grosseshuhn;
        List<Rectangle> mittlereshuhn;
        List<Rectangle> kleineshuhn;
        List<Rectangle> grossertod;
        List<Rectangle> mittlerertod;
        List<Rectangle> kleinertod;
        Rectangle screenrectangle;
        Graphics screengraphics;
        BufferedGraphicsContext currentContext;
        BufferedGraphics myBuffer;
        int frameslebendig;
        int framestod;
        int nächsteshuhn;
        Random myrandom = new Random();
        int score = 0;
        public Form1()
        {
            InitializeComponent();

            this.ShowInTaskbar = false;
            this.BackColor = Color.Lime;
            this.TransparencyKey = this.BackColor;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            screenrectangle = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Size.Width, Screen.PrimaryScreen.Bounds.Size.Height);
            screengraphics = this.CreateGraphics();
            frameslebendig = 13;
            framestod = 8;
            nächsteshuhn = 0;
            init_Rectangle_list();
            timer1.Interval = 100;
            timer1.Enabled = true;
        }
        private void init_Rectangle_list()
        {
            grosseshuhn = new List<Rectangle>();
            mittlereshuhn = new List<Rectangle>();
            kleineshuhn = new List<Rectangle>();
            grossertod = new List<Rectangle>();
            mittlerertod = new List<Rectangle>();
            kleinertod = new List<Rectangle>();
            Hühner = new List<Huhn>();
            for (int i = 0; i < frameslebendig; i++)
            {
                grosseshuhn.Add(new Rectangle(0, i * 152, 140, 152));
                mittlereshuhn.Add(new Rectangle(275, i * 120, 110, 120));
                kleineshuhn.Add(new Rectangle(530, i * 32, 45, 32));
            }
            for (int i = 0; i < framestod; i++)
            {
                grossertod.Add(new Rectangle(140, i * 150, 140, 150));
                mittlerertod.Add(new Rectangle(395, i * 120, 120, 120));
                kleinertod.Add(new Rectangle(575, i * 33, 40, 33));
            }
        }
        private void randomHuhn()
        {
            switch (myrandom.Next(3))
            {
                case 0:
                    NeuesHuhn(screenrectangle.Width, myrandom.Next(screenrectangle.Height - grosseshuhn[0].Height), "gross", grosseshuhn[0]);
                    break;
                case 1:
                    NeuesHuhn(screenrectangle.Width, myrandom.Next(screenrectangle.Height - mittlereshuhn[0].Height), "mittel", mittlereshuhn[0]);
                    break;
                case 2:
                    NeuesHuhn(screenrectangle.Width, myrandom.Next(screenrectangle.Height - kleineshuhn[0].Height), "klein", kleineshuhn[0]);
                    break;
            }
        }
        private void NeuesHuhn(int x, int y, string art, Rectangle grösse)
        {
            Huhn neuhuhn = new Huhn();
            neuhuhn.speed = myrandom.Next(1, 10);
            neuhuhn.frame = 0;
            neuhuhn.art = art;
            neuhuhn.lebendig = true;
            neuhuhn.positionsize = new Rectangle(x, y, grösse.Width, grösse.Height);
            Hühner.Add(neuhuhn);
        }
        private void Getroffen(int index)
        {
            Huhn ramhuhn = Hühner[index];
            ramhuhn.frame = 0;
            ramhuhn.lebendig = false;
            switch (ramhuhn.art)
            {
                case "gross":
                    ramhuhn.positionsize = new Rectangle(ramhuhn.positionsize.X, ramhuhn.positionsize.Y, grossertod[0].Width, grossertod[0].Height);
                    break;
                case "mittel":
                    ramhuhn.positionsize = new Rectangle(ramhuhn.positionsize.X, ramhuhn.positionsize.Y, mittlerertod[0].Width, mittlerertod[0].Height);
                    break;
                case "klein":
                    ramhuhn.positionsize = new Rectangle(ramhuhn.positionsize.X, ramhuhn.positionsize.Y, kleinertod[0].Width, kleinertod[0].Height);
                    break;
            }
            Hühner[index] = ramhuhn;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            myBuffer.Graphics.Clear(Color.Lime);
            myBuffer.Graphics.DrawString("Score: " + score.ToString(),DefaultFont , new SolidBrush(Color.Black), 10, 10);
            for (int i = 0; i < Hühner.Count; i++)
            {
                if (Hühner[i].lebendig == true)
                {
                    switch (Hühner[i].art)
                    {
                        case "gross":
                            myBuffer.Graphics.DrawImage(sprites, Hühner[i].positionsize, grosseshuhn[Hühner[i].frame], GraphicsUnit.Pixel);
                            break;
                        case "mittel":
                            myBuffer.Graphics.DrawImage(sprites, Hühner[i].positionsize, mittlereshuhn[Hühner[i].frame], GraphicsUnit.Pixel);
                            break;
                        case "klein":
                            myBuffer.Graphics.DrawImage(sprites, Hühner[i].positionsize, kleineshuhn[Hühner[i].frame], GraphicsUnit.Pixel);
                            break;
                    }
                }
                else
                {
                    switch (Hühner[i].art)
                    {
                        case "gross":
                            myBuffer.Graphics.DrawImage(sprites, Hühner[i].positionsize, grossertod[Hühner[i].frame], GraphicsUnit.Pixel);
                            break;
                        case "mittel":
                            myBuffer.Graphics.DrawImage(sprites, Hühner[i].positionsize, mittlerertod[Hühner[i].frame], GraphicsUnit.Pixel);
                            break;
                        case "klein":
                            myBuffer.Graphics.DrawImage(sprites, Hühner[i].positionsize, kleinertod[Hühner[i].frame], GraphicsUnit.Pixel);
                            break;
                    }
                }
            }
            myBuffer.Render();
            base.OnPaint(e);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            nächsteshuhn--;
            if (nächsteshuhn <= 0)
            {
                randomHuhn();
                nächsteshuhn = myrandom.Next(20,200);
            }
            Huhn ramhuhn;
            for (int i = 0; i < Hühner.Count; i++)
            {
                ramhuhn = Hühner[i];
                if (ramhuhn.lebendig == true)
                {
                    ramhuhn.positionsize.X -= ramhuhn.speed;
                    ramhuhn.frame++;
                    if (ramhuhn.frame >= frameslebendig) ramhuhn.frame = 0;
                }
                else
                {
                    ramhuhn.positionsize.Y += 15;
                    ramhuhn.frame++;
                    if (ramhuhn.frame >= framestod) ramhuhn.frame = 0;
                }
                Hühner[i] = ramhuhn;
            }
            OnPaint(new PaintEventArgs(screengraphics, screenrectangle));
            for (int i = 0; i < Hühner.Count; i++)
            {
                if ((Hühner[i].positionsize.X + Hühner[i].positionsize.Width < 0) || (Hühner[i].lebendig == false && Hühner[i].frame == framestod - 1))
                {
                    Hühner.RemoveAt(i);
                    i--;
                }
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Hühner.Count; i++)
            {
                if (MousePosition.X >= Hühner[i].positionsize.X && MousePosition.X <= Hühner[i].positionsize.X + Hühner[i].positionsize.Width)
                {
                    if (MousePosition.Y >= Hühner[i].positionsize.Y && MousePosition.Y <= Hühner[i].positionsize.Y + Hühner[i].positionsize.Height)
                    {
                        Getroffen(i);
                        score++;
                    }
                }
            }
        }
    }
}
