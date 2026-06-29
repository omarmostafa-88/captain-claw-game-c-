using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using NAudio.Wave;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class user
    {
        public int x, y;
        public int width, height;
        public Bitmap hp;
    }
    public class person
    {
        public int x, y;
        public int width, height;
        public List<Bitmap> photo;
    }
    public class img
    {
        public Bitmap photo;
        public Rectangle rdes;
        public Rectangle rsrc;
    }
    public class bullet
    {
        public int x, y, width, height, speed;
        public Bitmap photo;
        public bool active;
        public bool movingRight;

    }

    public partial class Form1 : Form
    {
        int stage = 0, index = 1, current_state = 0, index2 = 11, f_jump = 0, index3 = 21, index4 = 28, current_officer1 = 0, current_officer2 = 0;
        bool pressright = false, pressleft = false, fire = false, magic_bullet = false;
        bool jumping = false, maskk = false, climb_up = false, RIP = false, bullet_fired = false, key_is_earned = false, press_k = false, stop_lasser = false, on_elevator = false;
        int jumpspeed = 10;
        int index5 = 35, index6 = 41, index7 = 47, index8 = 59;
        int ct1 = 0, f_s = 0, check = 0, ct_tick = 0, ct_lasser = 0, point = 0;
        bool magic_is_earned = false;
        int index_officer2 = 1, ct_officer2 = 0, health_officer2 = 0, health_hero = 0;

        bool bg_should_play = false;
        WaveOutEvent bgPlayer = null;
        AudioFileReader bgReader = null;
        WaveOutEvent loadingPlayer = null;
        AudioFileReader loadingReader = null;

        WaveOutEvent stage2Player = null;
        AudioFileReader stage2Reader = null;

        bool loading_should_play = false;
        bool stage2_should_play = false;

        void PlayMagicSound()
        {
            var magicReader = new AudioFileReader("magicclaw.wav");
            var magicPlayer = new WaveOutEvent();
            magicPlayer.Init(magicReader);
            magicPlayer.Play();
            magicPlayer.PlaybackStopped += (s, e) => {
                magicReader.Dispose();
                magicPlayer.Dispose();
            };
        }
        void PlayPistolSound()
        {
            var pistolReader = new AudioFileReader("pistol.wav");
            var pistolPlayer = new WaveOutEvent();
            pistolPlayer.Init(pistolReader);
            pistolPlayer.Play();
            pistolPlayer.PlaybackStopped += (s, e) =>
            {
                pistolReader.Dispose();
                pistolPlayer.Dispose();
            };
        }
        Bitmap background;
        Bitmap off;

        Timer tt = new Timer();

        public List<user> users = new List<user>();
        public List<user> enmy1 = new List<user>();
        public List<user> logos = new List<user>();
        public List<user> start = new List<user>();
        public List<user> hero_with_crown = new List<user>();

        public List<img> bg = new List<img>();
        public List<img> thistle = new List<img>();
        public List<img> claw_photo = new List<img>();
        public List<img> single_bullet = new List<img>();
        public List<img> multi_bullet = new List<img>();
        public List<img> off_bullet = new List<img>();
        public List<img> ladder = new List<img>();
        public List<img> stand = new List<img>();
        public List<img> lassermachine = new List<img>();
        public List<img> downlasser = new List<img>();
        public List<img> elevator = new List<img>();
        public List<img> magic_key = new List<img>();
        public List<img> floor = new List<img>();
        public List<img> magic = new List<img>();
        public List<img> logo = new List<img>();
        public List<img> menu = new List<img>();

        public List<person> checkpoint = new List<person>();
        public List<person> hero = new List<person>();
        public List<person> officer1 = new List<person>();
        public List<person> officer2 = new List<person>();
        public List<bullet> bullets = new List<bullet>();

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.MouseDown += Form1_MouseDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);
            loading_screen();
            tt.Interval = 30;
            tt.Tick += Tt_Tick;
            tt.Start();
        }
        void PlayBackgroundMusic()
        {
            bg_should_play = true;

            if (bgReader == null)
            {
                bgReader = new AudioFileReader("Captain Claw - Level 01 Music.wav");
            }

            if (bgPlayer == null)
            {
                bgPlayer = new WaveOutEvent();
                bgPlayer.Init(bgReader);
                bgPlayer.PlaybackStopped += (s, e) =>
                {
                    if (bg_should_play)
                    {
                        bgReader.Position = 0;
                        bgPlayer.Play();
                    }
                };
            }

            if (bgPlayer.PlaybackState != PlaybackState.Playing)
            {
                bgPlayer.Play();
            }
        }
        void PlayLoadingMusic()
        {
            loading_should_play = true;

            if (loadingReader == null)
            {
                loadingReader = new AudioFileReader("loadingscreen.wav"); 
            }

            if (loadingPlayer == null)
            {
                loadingPlayer = new WaveOutEvent();
                loadingPlayer.Init(loadingReader);
                loadingPlayer.PlaybackStopped += (s, e) =>
                {
                    if (loading_should_play)
                    {
                        loadingReader.Position = 0;
                        loadingPlayer.Play();
                    }
                };
            }

            if (loadingPlayer.PlaybackState != PlaybackState.Playing)
            {
                loadingPlayer.Play();
            }
        }
        void PlayStage2Music()
        {
            stage2_should_play = true;

            if (stage2Reader == null)
            {
                stage2Reader = new AudioFileReader("win.wav"); 
            }

            if (stage2Player == null)
            {
                stage2Player = new WaveOutEvent();
                stage2Player.Init(stage2Reader);
                stage2Player.PlaybackStopped += (s, e) =>
                {
                    if (stage2_should_play)
                    {
                        stage2Reader.Position = 0;
                        stage2Player.Play();
                    }
                };
            }

            if (stage2Player.PlaybackState != PlaybackState.Playing)
            {
                stage2Player.Play();
            }
        }

        private void ResetGame()
        {
            index = 1;
            current_state = 0;
            index2 = 11;
            f_jump = 0;
            index3 = 21;
            index4 = 28;
            current_officer1 = 0;
            current_officer2 = 0;
            pressright = false;
            pressleft = false;
            fire = false;
            magic_bullet = false;
            jumping = false;
            maskk = false;
            climb_up = false;
            RIP = false;
            bullet_fired = false;
            key_is_earned = false;
            press_k = false;
            stop_lasser = false;
            on_elevator = false;
            jumpspeed = 10;
            index5 = 35;
            index6 = 41;
            index7 = 47;
            index8 = 59;
            ct1 = 0;
            f_s = 0;
            check = 0;
            ct_tick = 0;
            ct_lasser = 0;
            point = 0;
            magic_is_earned = false;
            index_officer2 = 1;
            ct_officer2 = 0;
            health_officer2 = 0;
            health_hero = 0;

            users.Clear();
            enmy1.Clear();
            logos.Clear();
            start.Clear();
            hero_with_crown.Clear();
            menu.Clear();
            logo.Clear();
            bg.Clear();
            thistle.Clear();
            claw_photo.Clear();
            single_bullet.Clear();
            multi_bullet.Clear();
            off_bullet.Clear();
            ladder.Clear();
            stand.Clear();
            lassermachine.Clear();
            downlasser.Clear();
            elevator.Clear();
            magic_key.Clear();
            floor.Clear();
            magic.Clear();
            checkpoint.Clear();
            hero.Clear();
            officer1.Clear();
            officer2.Clear();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (stage == 2)
            {
                if (e.X >= logo[1].rdes.X && e.X <= logo[1].rdes.X + logo[1].rdes.Width && e.Y >= logo[1].rdes.Y + 20 && e.Y <= logo[1].rdes.Y + logo[1].rdes.Height - 20)
                {
                    stage = 0;
                    ResetGame();
                    loading_screen();
                    bg_should_play = false;
                    if (bgPlayer != null)
                    {
                        bgPlayer.Stop();
                        bgPlayer.Dispose();
                        bgPlayer = null;
                    }
                    if (bgReader != null)
                    {
                        bgReader.Dispose();
                        bgReader = null;
                    }
                    stage2_should_play = false;
                    if (stage2Player != null) { stage2Player.Stop(); stage2Player.Dispose(); stage2Player = null; }
                    if (stage2Reader != null) { stage2Reader.Dispose(); stage2Reader = null; }
                }
                else if (e.X >= logo[0].rdes.X && e.X <= logo[0].rdes.X + logo[0].rdes.Width && e.Y >= logo[0].rdes.Y + 20 && e.Y <= logo[0].rdes.Y + logo[0].rdes.Height - 20)
                {
                    stage = 1;
                    ResetGame();
                    tt.Start();
                    stage2_should_play = false;
                    if (stage2Player != null) { stage2Player.Stop(); stage2Player.Dispose(); stage2Player = null; }
                    if (stage2Reader != null) { stage2Reader.Dispose(); stage2Reader = null; }
                    stage1();
                    bgPlayer.Stop();           
                }
            }

            else if (RIP)
            {
                if (e.X >= logo[2].rdes.X && e.X <= logo[2].rdes.X + logo[2].rdes.Width && e.Y >= logo[2].rdes.Y + 20 && e.Y <= logo[2].rdes.Y + logo[2].rdes.Height - 20)
                {
                    stage = 0;
                    ResetGame();
                    loading_screen();
                    bg_should_play = false;
                    if (bgPlayer != null)
                    {
                        bgPlayer.Stop();
                        bgPlayer.Dispose();
                        bgPlayer = null;
                    }
                    if (bgReader != null)
                    {
                        bgReader.Dispose();
                        bgReader = null;
                    }
                    loading_should_play = true;
                    PlayLoadingMusic();
                }
                else if (e.X >= logo[1].rdes.X && e.X <= logo[1].rdes.X + logo[1].rdes.Width && e.Y >= logo[1].rdes.Y + 20 && e.Y <= logo[1].rdes.Y + logo[1].rdes.Height - 20)
                {
                    stage = 1;
                    ResetGame();
                    tt.Start();    
                    stage1();
                    bgPlayer.Stop();
                }
            }
            drawbuff(this.CreateGraphics());

        }
        private void Tt_Tick(object sender, EventArgs e)
        {
            if (stage == 1)
            {
                ct_tick++;
                ct_lasser++;
                if (pressright)
                {
                    index++;
                    if (index >= 11)
                    {
                        index = 1;
                    }
                }
                if (pressleft)
                {
                    index2++;
                    if (index2 >= 21)
                    {
                        index2 = 11;
                    }
                }
                if (f_jump == 1)
                {
                    if (current_state == 3)
                    {
                        index3++;
                        if (index3 >= 28)
                        {
                            index3 = 21;
                        }
                    }
                    if (current_state == 4 && pressleft)
                    {
                        index4++;
                        if (index4 >= 35)
                        {
                            index4 = 28;
                        }
                    }

                }
                if (fire && ct_tick > 3)
                {
                    index5++;
                    if (index5 >= 41)
                    {
                        index5 = 35;
                        fire = false;
                        current_state = 0;
                    }
                    ct_tick = 0;
                }


                if (magic_bullet)
                {
                    index6++;
                    if (index6 >= 47)
                    {
                        index6 = 41;
                        magic_bullet = false;
                        current_state = 0;
                    }
                }
                if (ct_lasser > 3)
                {
                    create_lasser();
                    ct_lasser = 0;
                }
                if (press_k)
                {
                    stoplasser();
                }
                getkey();
                getmagic();
                jump();
                gravity();
                moveelevator();
                scroll();
                shot();
                shot_magic();
                climb();
                move_officer1();
                lasser_down();
                officer2_attack();
                dead();
                stage2();
                drawbuff(this.CreateGraphics());
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (stage == 0)
            {
                ResetGame();
                stage = 1;
                background = null;
                loading_should_play = false;
                if (loadingPlayer != null) { loadingPlayer.Stop(); loadingPlayer.Dispose(); loadingPlayer = null; }
                if (loadingReader != null) { loadingReader.Dispose(); loadingReader = null; }
                stage1();
                tt.Start();      
            }
            if (stage == 1 && !RIP)
            {
                if (e.KeyCode == Keys.Right)////////////move right
                {
                    pressright = true;
                    current_state = 1;

                }
                if (e.KeyCode == Keys.Left)////////////move left
                {
                    pressleft = true;
                    current_state = 2;
                }
                if (e.KeyCode == Keys.Space && f_jump == 0)///////////// jump 
                {
                    jumping = true;
                    f_jump = 1;
                    current_state = 3;
                    if (pressleft)
                    {
                        current_state = 4;
                    }
                }
                if (e.KeyCode == Keys.X)////////////////// fire with pistol single bullet
                {
                    fire = true;
                    current_state = 5;
                }
                if (e.KeyCode == Keys.Q)////////////////// fire with magic multi bullet
                {
                    magic_bullet = true;
                    current_state = 6;
                }
                if (e.KeyCode == Keys.Up)//////////////// go up after climb
                {
                    climb_up = true;
                }
                if (e.KeyCode == Keys.K)
                {
                    press_k = true;
                }

            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (stage == 1 && !RIP)
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
                {
                    pressright = false;
                    pressleft = false;
                    index = 1;
                    index2 = 11;
                    current_state = 0;
                }
            }
        }   
        void loading_screen()
        {
            if(stage == 0)
            {
                background = new Bitmap("castle.png");
                ///////////////////////////////////////////////
                user logo = new user();
                logo.x = ClientSize.Width / 2 - 220;
                logo.y = 0;
                logo.width = 400;
                logo.height = 150;
                logo.hp = new Bitmap("logo.bmp");
                logo.hp.MakeTransparent(logo.hp.GetPixel(0, 0));
                logos.Add(logo);
                /////////////////////////////////////////////////
                logo = new user();
                logo.x = ClientSize.Width / 2 - 290;
                logo.y = ClientSize.Height - 220;
                logo.width = 550;
                logo.height = 150;
                logo.hp = new Bitmap("start.png");
                logo.hp.MakeTransparent(logo.hp.GetPixel(0, 0));
                start.Add(logo);
                /////////////////////////////////////////////////
                user enmy = new user();
                enmy.x = ClientSize.Width / 2 + 210;
                enmy.y = 270;
                enmy.width = 300;
                enmy.height = 300;
                enmy.hp = new Bitmap("PC Computer - Claw - Officer.png");
                enmy1.Add(enmy);
                ///////////////////////////////////////////////////
                user hero = new user();
                hero.x = 200;
                hero.y = 270;
                hero.width = 300;
                hero.height = 300;
                hero.hp = new Bitmap("claw2-bg.png");
                users.Add(hero);
                loading_should_play = true;
                PlayLoadingMusic();
            }

        }
        void stage1()
        {
            if (stage == 1)
            {
                img pnn = new img();
                pnn.photo = new Bitmap("map.png");
                pnn.rdes = new Rectangle(0, 0, this.Width, this.Height);
                pnn.rsrc = new Rectangle(0, 0, 520, 515);
                bg.Add(pnn);
                /////////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("newthistle.png");
                pnn.photo.MakeTransparent(pnn.photo.GetPixel(0, 0));
                pnn.rdes = new Rectangle(1250, 595, 900, 100);
                pnn.rsrc = new Rectangle(0, 0, 700, 100);
                thistle.Add(pnn);
                /////////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("ladder.png");
                pnn.rdes = new Rectangle(1000, 280, 150, 210);
                pnn.rsrc = new Rectangle(0, 0, 65, 138);
                ladder.Add(pnn);
                ////////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("stand.png");
                pnn.rdes = new Rectangle(1150, 280, 100, 33);
                pnn.rsrc = new Rectangle(0, 0, 86, 33);
                stand.Add(pnn);
                //////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("stand.png");
                pnn.rdes = new Rectangle(1400, 230, 100, 33);
                pnn.rsrc = new Rectangle(0, 0, 86, 33);
                stand.Add(pnn);
                ////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("stand.png");
                pnn.rdes = new Rectangle(1650, 180, 100, 33);
                pnn.rsrc = new Rectangle(0, 0, 86, 33);
                stand.Add(pnn);
                //////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("stand.png");
                pnn.rdes = new Rectangle(1900, 230, 100, 33);
                pnn.rsrc = new Rectangle(0, 0, 86, 33);
                stand.Add(pnn);
                //////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("stand.png");
                pnn.rdes = new Rectangle(2150, 280, 100, 33);
                pnn.rsrc = new Rectangle(0, 0, 86, 33);
                stand.Add(pnn);
                //////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("lasser.png");
                pnn.rdes = new Rectangle(3300, 0, 105, 45);
                pnn.rsrc = new Rectangle(0, 0, 101, 41);
                lassermachine.Add(pnn);
                //////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("elevator.png");
                pnn.rdes = new Rectangle(3800, 595, 265, 35);
                pnn.rsrc = new Rectangle(0, 0, 261, 32);
                elevator.Add(pnn);
                //////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("floor.png");
                pnn.rdes = new Rectangle(4066, 160, 680, 50);
                pnn.rsrc = new Rectangle(0, 0, 673, 46);
                floor.Add(pnn);
                //////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("magic.png");
                pnn.rdes = new Rectangle(4400, 120, 40, 40);
                pnn.rsrc = new Rectangle(0, 0, 37, 37);
                magic.Add(pnn);
                ////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("photo-health.png");
                pnn.rdes = new Rectangle(0, 20, 60, 60);
                pnn.rsrc = new Rectangle(0, 0, 59, 57);
                claw_photo.Add(pnn);
                ////////////////////////////////////////////////

                person ptrav = new person();
                ptrav.x = lassermachine[0].rdes.X - 100;
                ptrav.y = 550;
                ptrav.width = 50;
                ptrav.height = 126;
                ptrav.photo = new List<Bitmap>();
                for (int i = 0; i < 2; i++)
                {
                    Bitmap ph = new Bitmap($"{i}gate.png");
                    ptrav.photo.Add(ph);
                }
                checkpoint.Add(ptrav);

                createofficer1();
                createofficer2();
                createhero();
                bg_should_play = true;
                PlayBackgroundMusic();
            }
        }
        void createofficer1()
        {
            person pnn = new person();
            pnn.x = 2800;
            pnn.y = 515;
            pnn.width = 180;
            pnn.height = 180;
            pnn.photo = new List<Bitmap>();
            for (int i = 0; i < 4; i++)
            {
                Bitmap ph = new Bitmap($"officer1/{i}officer.PNG");
                pnn.photo.Add(ph);
            }
            officer1.Add(pnn);
        }
        void move_officer1()
        {
            if (hero[0].y >= 400 && current_officer1 != 3)
            {
                if (officer1[0].x > hero[0].x + hero[0].width - 20 && hero[0].x > thistle[0].rdes.X + thistle[0].rdes.Width)
                {
                    officer1[0].x -= 8;
                    current_officer1 = 1;
                }

                else if (officer1[0].x + officer1[0].width < hero[0].x)
                {
                    officer1[0].x += 5;
                    current_officer1 = 2;
                }


                if (
                    officer1[0].x > hero[0].x &&
                    officer1[0].x < hero[0].x + hero[0].width - 19 &&
                    officer1[0].y + officer1[0].height > hero[0].y &&
                    officer1[0].y < hero[0].y + hero[0].height
                )
                {
                    RIP = true;
                    current_state = 8;
                    current_officer1 = 0;
                }
            }
        }
        void createofficer2()
        {
            person pnn = new person();
            pnn.x = 5900;
            pnn.y = 515;
            pnn.width = 180;
            pnn.height = 180;
            pnn.photo = new List<Bitmap>();
            for (int i = 0; i < 6; i++)
            {
                Bitmap ph = new Bitmap($"officer2/{i}officer.PNG");
                pnn.photo.Add(ph);
            }
            officer2.Add(pnn);
        }
        void createhero()
        {
            person pnn = new person();
            pnn.x = 150;
            pnn.y = 530;
            pnn.width = 130;
            pnn.height = 160;
            pnn.photo = new List<Bitmap>();
            for (int i = 1; i <= 60; i++)
            {
                Bitmap ph = new Bitmap($"hero/{i}claw.PNG");
                pnn.photo.Add(ph);
            }
            hero.Add(pnn);
        }
        void scroll()
        {
            if (!RIP)
            {
                if (pressright)
                {
                    if (bg[0].rsrc.X + bg[0].rsrc.Width < bg[0].photo.Width)
                    {
                        if (hero[0].x < 250)
                        {
                            hero[0].x += 10;
                        }
                        else
                        {
                            bg[0].rsrc.X += 5;
                            ladder[0].rdes.X -= 15;
                            thistle[0].rdes.X -= 15;
                            for (int i = 0; i < stand.Count; i++)
                            {
                                stand[i].rdes.X -= 15;
                            }
                            officer1[0].x -= 15;
                            lassermachine[0].rdes.X -= 15;
                            elevator[0].rdes.X -= 15;
                            floor[0].rdes.X -= 15;
                            officer2[0].x -= 15;
                            for (int i = 0; i < single_bullet.Count; i++)
                            {
                                single_bullet[i].rdes.X -= 15;
                            }
                            for (int i = 0; i < downlasser.Count; i++)
                            {
                                downlasser[i].rdes.X -= 15;
                            }
                            if (!key_is_earned)
                            {
                                for (int i = 0; i < magic_key.Count; i++)
                                {
                                    magic_key[i].rdes.X -= 15;
                                }
                            }

                            if (!magic_is_earned)
                            {
                                magic[0].rdes.X -= 15;
                            }

                            checkpoint[0].x -= 15;
                        }
                    }
                    else
                    {

                        hero[0].x += 10;

                    }
                    check = 0;
                }

                if (pressleft)
                {
                    if (bg[0].rsrc.X > 0)
                    {
                        if (hero[0].x > 150)
                        {
                            hero[0].x -= 10;
                        }
                        else
                        {
                            bg[0].rsrc.X -= 5;
                            ladder[0].rdes.X += 15;
                            thistle[0].rdes.X += 15;
                            for (int i = 0; i < stand.Count; i++)
                            {
                                stand[i].rdes.X += 15;
                            }
                            officer1[0].x += 15;
                            lassermachine[0].rdes.X += 15;
                            elevator[0].rdes.X += 15;
                            floor[0].rdes.X += 15;
                            officer2[0].x += 15;
                            for (int i = 0; i < single_bullet.Count; i++)
                            {
                                single_bullet[i].rdes.X += 15;
                            }
                            for (int i = 0; i < downlasser.Count; i++)
                            {
                                downlasser[i].rdes.X += 15;
                            }
                            if (!key_is_earned)
                            {
                                for (int i = 0; i < magic_key.Count; i++)
                                {
                                    magic_key[i].rdes.X += 15;
                                }
                            }
                            if (!magic_is_earned)
                            {
                                magic[0].rdes.X += 15;
                            }

                            checkpoint[0].x += 15;
                            //claw_photo[0].rdes.X -= 15;
                        }
                    }
                    else
                    {
                        if (hero[0].x > 70)
                        {
                            hero[0].x -= 10;
                        }
                    }
                    check = 0;
                }
            }

        }
        void jump()
        {
            if (jumping && f_s != 1)
            {
                hero[0].y -= jumpspeed;
                jumpspeed--;
                if (jumpspeed <= 0)
                {
                    jumping = false;
                    jumpspeed = 15;
                    check = 0;
                }
            }
        }
        void gravity()
        {
            if (!jumping && f_s != 1)
            {
                for (int i = 0; i < ladder.Count; i++)
                {
                    if (hero[0].y + hero[0].height >= ladder[i].rdes.Y && hero[0].y + hero[0].height < ladder[i].rdes.Y + 50 && hero[0].x + hero[0].width - 15 >= ladder[i].rdes.X && hero[0].x + 40 <= ladder[i].rdes.X + ladder[i].rdes.Width)
                    {
                        hero[0].y = ladder[i].rdes.Y - hero[0].height + 15;
                        jumpspeed = 15;
                        f_jump = 0;
                        jumping = false;
                        check = 1;
                        if (current_state != 5 && current_state != 6)
                        {
                            if (pressright)
                            {
                                current_state = 1;
                            }
                            else if (pressleft)
                            {
                                current_state = 2;
                            }
                            else
                            {
                                current_state = 0;
                            }
                        }
                        break;
                    }
                }
                for (int i = 0; i < stand.Count; i++)
                {
                    if (hero[0].y + hero[0].height >= stand[i].rdes.Y && hero[0].y + hero[0].height <= stand[i].rdes.Y + stand[i].rdes.Height && hero[0].x + hero[0].width - 15 >= stand[i].rdes.X && hero[0].x + 40 <= stand[i].rdes.X + stand[i].rdes.Width)
                    {
                        hero[0].y = stand[i].rdes.Y - hero[0].height + 15;
                        jumpspeed = 15;
                        f_jump = 0;
                        jumping = false;
                        if (current_state != 5 && current_state != 6)
                        {
                            if (pressright)
                            {
                                current_state = 1;
                            }
                            else if (pressleft)
                            {
                                current_state = 2;
                            }
                            else
                            {
                                current_state = 0;
                            }
                        }
                        //MessageBox.Show("fjtnfjntj");
                        check = 1;
                        break;
                    }
                }
             
                for (int i = 0; i < elevator.Count; i++)
                {
                    if (hero[0].y + hero[0].height >= elevator[i].rdes.Y && hero[0].x + hero[0].width - 15 >= elevator[i].rdes.X && hero[0].x + 40 <= elevator[i].rdes.X + elevator[i].rdes.Width)
                    {
                        hero[0].y = elevator[i].rdes.Y - hero[0].height + 15;
                        jumpspeed = 15;
                        f_jump = 0;
                        jumping = false;
                        check = 1;
                        on_elevator = true;
                        if (current_state != 5 && current_state != 6)
                        {
                            if (pressright)
                            {
                                current_state = 1;
                            }
                            else if (pressleft)
                            {
                                current_state = 2;
                            }
                            else
                            {
                                current_state = 0;
                            }
                        }
                        break;
                    }
                }
                for (int i = 0; i < floor.Count; i++)
                {
                    if (hero[0].y + hero[0].height >= floor[i].rdes.Y && hero[0].y + hero[0].height < floor[i].rdes.Y + 20 && hero[0].x + hero[0].width - 15 >= floor[i].rdes.X && hero[0].x + 40 <= floor[i].rdes.X + floor[i].rdes.Width)
                    {
                        hero[0].y = floor[i].rdes.Y - hero[0].height + 15;
                        jumpspeed = 15;
                        f_jump = 0;
                        jumping = false;
                        on_elevator = false;
                        check = 1;
                        if (current_state != 5 && current_state != 6)
                        {
                            if (pressright)
                            {
                                current_state = 1;
                            }
                            else if (pressleft)
                            {
                                current_state = 2;
                            }
                            else
                            {
                                current_state = 0;
                            }
                        }
                        break;
                    }
                }
                if (check == 0)
                {
                    hero[0].y += jumpspeed;
                    jumpspeed++;
                    if (hero[0].y + hero[0].height >= 670)
                    {
                        hero[0].y = 530;
                        jumpspeed = 15;
                        f_jump = 0;
                        ct1 = 0;
                        jumping = false;
                        on_elevator = false;
                        f_s = 0;
                        //check = 0;
                        if (current_state != 5 && current_state != 6)
                        {
                            if (pressright)
                            {
                                current_state = 1;
                            }
                            else if (pressleft)
                            {
                                current_state = 2;
                            }
                            else
                            {
                                current_state = 0;
                            }
                        }
                    }
                }
            }
        }
        void climb()
        {
            if (hero[0].x + hero[0].photo[0].Width > ladder[0].rdes.X && hero[0].x < ladder[0].rdes.X + ladder[0].rdes.Width && jumping && hero[0].y < ladder[0].rdes.Y + ladder[0].rdes.Height && f_s != 2) ////////// bet43l2 3la el slm
            {
                maskk = true;
                current_state = 7;
                if (ct1 == 0)
                {
                    hero[0].y = 400;
                    ct1 = 1;
                }
                hero[0].x = ladder[0].rdes.X + 10;
                f_s = 1;
            }

            if (climb_up && maskk)//////////////////// betl3 lfo2 
            {
                index7++;
                hero[0].y -= 5;
                if (index7 >= 59)
                {
                    index7 = 47;
                }
                climb_up = false;
            }
            if (hero[0].y + hero[0].height < ladder[0].rdes.Y + 30 && maskk) ///// wsl la5r el slm
            {
                current_state = 0;
                maskk = false;
                jumping = false;
                f_s = 2;
                check = 1;
                hero[0].y = 140;
                jumpspeed = 15;
                f_jump = 0;
            }
        }
        void shot()
        {
            if (fire && index5 == 38 && !bullet_fired)
            {
                img pnn = new img();
                pnn.photo = new Bitmap("pistol-bullet.png");
                pnn.rdes = new Rectangle(hero[0].x + hero[0].width + 5, hero[0].y + hero[0].height / 2 - 40, 11, 11);
                pnn.rsrc = new Rectangle(0, 0, 11, 11);
                single_bullet.Add(pnn);
                bullet_fired = true;
                PlayPistolSound();
                bgPlayer.Volume = 0.3f;

              
                Task.Delay(1500).ContinueWith(_ => {
                    bgPlayer.Volume = 1.0f; 
                });
            }

            if (!fire)
            {
                bullet_fired = false;
            }

            for (int i = 0; i < single_bullet.Count; i++)
            {
                single_bullet[i].rdes.X += 35;

                if (single_bullet[i].rdes.X > this.Width)
                {
                    single_bullet.RemoveAt(i);
                    i--;
                }
                else if (single_bullet[i].rdes.X >= officer1[0].x && hero[0].x + hero[0].width < officer1[0].x && single_bullet[i].rdes.Y > officer1[0].y && current_officer1 != 3)
                {
                    current_officer1 = 3;
                    img pnn = new img();
                    pnn.photo = new Bitmap("magic_key.png");
                    pnn.rdes = new Rectangle(officer1[0].x + officer1[0].width / 2, officer1[0].y - 10, 60, 30);
                    pnn.rsrc = new Rectangle(0, 0, 60, 30);
                    magic_key.Add(pnn);
                    bullet_fired = true;
                    officer1[0].y = thistle[0].rdes.Y - 50;
                    single_bullet.RemoveAt(i);
                    i--;
                }
                else if (single_bullet[i].rdes.X >= officer2[0].x + 30 && hero[0].x + hero[0].width < officer2[0].x && single_bullet[i].rdes.Y > officer2[0].y && current_officer2 != 2)
                {
                    health_officer2++;
                    if (health_officer2 == 10)
                    {
                        current_officer2 = 2;
                    }
                    single_bullet.RemoveAt(i);
                    bullet_fired = false;
                    i--;
                }
            }
        }
        void create_lasser()
        {
            if (!stop_lasser)
            {
                img pnn = new img();
                pnn.photo = new Bitmap("lasser_down.png");
                pnn.rdes = new Rectangle(lassermachine[0].rdes.X + lassermachine[0].rdes.Width / 2 - 12, lassermachine[0].rdes.Y + lassermachine[0].rdes.Height + 5, 61, 120);
                pnn.rsrc = new Rectangle(0, 0, 61, 120);
                downlasser.Add(pnn);
            }

        }
        void lasser_down()
        {
            if (!stop_lasser)
            {
                for (int i = 0; i < downlasser.Count; i++)
                {
                    if (downlasser[i].rdes.Y + downlasser[i].rdes.Height < 695)
                    {
                        downlasser[i].rdes.Y += 15;
                    }
                    else
                    {
                        downlasser.RemoveAt(i);
                        i--;
                    }
                }
            }

        }
        void stoplasser()
        {
            if (hero[0].x + hero[0].width >= checkpoint[0].x)
            {
                stop_lasser = true;
                magic_key.Clear();
                downlasser.Clear();
                point = 1;
            }
            else
            {
                press_k = false;
            }
        }
        void getkey()
        {
            for (int i = 0; i < magic_key.Count; i++)
            {
                if (hero[0].x + hero[0].width >= magic_key[0].rdes.X && !key_is_earned)
                {
                    magic_key.Clear();
                    img pnn = new img();
                    pnn.photo = new Bitmap("magic_key.png");
                    pnn.rdes = new Rectangle(0, 80, 60, 30);
                    pnn.rsrc = new Rectangle(0, 0, 60, 30);
                    magic_key.Add(pnn);
                    key_is_earned = true;
                    break;
                }
            }
        }
        void moveelevator()
        {
            if (on_elevator)
            {
                if (elevator[0].rdes.Y >= floor[0].rdes.Y)
                {
                    elevator[0].rdes.Y -= 5;
                }
                else
                {
                    hero[0].x = floor[0].rdes.X;
                }

            }
            else
            {
                if (elevator[0].rdes.Y <= 595)
                {
                    elevator[0].rdes.Y += 5;
                }
            }
        }
        void getmagic()
        {
            for (int i = 0; i < magic.Count; i++)
            {
                if (hero[0].x + hero[0].width >= magic[i].rdes.X && !magic_is_earned && hero[0].y < magic[i].rdes.Y)
                {
                    magic.Clear();
                    img pnn = new img();
                    pnn.photo = new Bitmap("magic.png");
                    pnn.rdes = new Rectangle(0, 80, 37, 37);
                    pnn.rsrc = new Rectangle(0, 0, 37, 37);
                    magic.Add(pnn);
                    magic_is_earned = true;
                    break;
                }
            }
        }
        void officer2_attack()
        {
            if (current_officer2 != 2 && !RIP)
            {
                if (hero[0].x > floor[0].rdes.X + floor[0].rdes.Width + 250)
                {
                    ct_officer2++;
                    if (ct_officer2 > 5)
                    {
                        current_officer2 = 1;
                        index_officer2++;
                        if (index_officer2 == 4)
                        {
                            index_officer2 = 1;
                        }
                        if(index_officer2 == 2)
                        {
                            img pnn = new img();
                            pnn.photo = new Bitmap("pistol-bullet.png");
                            pnn.rdes = new Rectangle(officer2[0].x , officer2[0].y + officer2[0].height / 2 - 45, 11, 11);
                            pnn.rsrc = new Rectangle(0, 0, 11, 11);
                            off_bullet.Add(pnn);
                            PlayPistolSound();
                        }
                        ct_officer2 = 0;
                    }
                }
                else
                {
                    current_officer2 = 0;
                }
                for (int i = 0; i < off_bullet.Count; i++)
                {
                    off_bullet[i].rdes.X -= 35;
                    if (off_bullet[i].rdes.X < 0)
                    {
                        off_bullet.RemoveAt(i);
                        i--;
                    }
                    else if (off_bullet[i].rdes.X < hero[0].x + hero[0].width && off_bullet[i].rdes.Y > hero[0].y)
                    {
                        health_hero++;
                        off_bullet.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        void shot_magic()
        {
            if (magic_is_earned && magic_bullet && index6 == 46)
            {
                img pnn = new img();
                pnn.photo = new Bitmap("magic-claw.png");
                pnn.rdes = new Rectangle(hero[0].x + hero[0].width , hero[0].y + hero[0].height / 2 - 40, 90, 37);
                pnn.rsrc = new Rectangle(0, 0, 90, 37);
                multi_bullet.Add(pnn);
                PlayMagicSound();

            }
            for (int i = 0; i < multi_bullet.Count; i++)
            {
                multi_bullet[i].rdes.X += 45;

                if (multi_bullet[i].rdes.X > this.Width)
                {
                    multi_bullet.RemoveAt(i);
                    i--;
                }
                else if (multi_bullet[i].rdes.X >= officer2[0].x + 30 && hero[0].x + hero[0].width < officer2[0].x && multi_bullet[i].rdes.Y > officer2[0].y && current_officer2 != 2)
                {
                    health_officer2++;
                    if (health_officer2 == 5)
                    {
                        current_officer2 = 2;
                        off_bullet.Clear();
                    }
                    multi_bullet.RemoveAt(i);
                    bullet_fired = false;
                    i--;
                }

            }
        }
        void stage2()
        {
            if (hero[0].x > officer2[0].x + officer2[0].width)
            {
                stage = 2;
                bg_should_play = false;
                if (bgPlayer != null)
                {
                    bgPlayer.Stop();
                }
                stage2_should_play = true;
                PlayStage2Music();

                tt.Stop();
                background = new Bitmap("throne_room.png");
                img pnn = new img();
                pnn.photo = new Bitmap("legend.png");
                pnn.photo.MakeTransparent(pnn.photo.GetPixel(0, 0));
                pnn.rdes = new Rectangle(this.Width/2 - 365, 15, 630, 154);
                pnn.rsrc = new Rectangle(0, 0, 730, 234);
                menu.Add(pnn);
                ////////////////////////////////////////////////////////
                user hero = new user();
                hero.x = this.Width / 2 - 100;
                hero.y = this.Height / 2 - 100;
                hero.width = 120;
                hero.height = 150;
                hero.hp = new Bitmap("claw-won.png");
                hero.hp.MakeTransparent(hero.hp.GetPixel(0, 0));
                hero_with_crown.Add(hero);
                //////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("restart.png");
                pnn.photo.MakeTransparent(pnn.photo.GetPixel(0, 0));
                pnn.rdes = new Rectangle(this.Width / 2 - 365, this.Height / 2 + 100, 220, 154);
                pnn.rsrc = new Rectangle(0, 0, 395, 174);
                logo.Add(pnn);
                /////////////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("end.png");
                pnn.photo.MakeTransparent(pnn.photo.GetPixel(0, 0));
                pnn.rdes = new Rectangle(this.Width / 2, this.Height / 2 + 100, 220, 154);
                pnn.rsrc = new Rectangle(0, 0, 478, 174);
                logo.Add(pnn);
            }
        }
        void dead()
        {
            if (hero[0].x + hero[0].width > thistle[0].rdes.X && hero[0].x < thistle[0].rdes.X + thistle[0].rdes.Width && hero[0].y + hero[0].height > thistle[0].rdes.Y)
            {
                current_state = 8;
                hero[0].y = thistle[0].rdes.Y - 50;
                claw_photo.Clear();
                RIP = true;
            }
            if (!stop_lasser)
            {
                for (int i = 0; i < downlasser.Count; i++)
                {
                    if (hero[0].x + hero[0].width >= downlasser[i].rdes.X)
                    {
                        current_state = 8;
                        hero[0].y = thistle[0].rdes.Y - 50;
                        claw_photo.Clear();
                        magic_key.Clear();
                        RIP = true;
                    }
                }
            }
            if (health_hero >= 10)
            {
                current_state = 8;
                hero[0].y = thistle[0].rdes.Y - 50;
                claw_photo.Clear();
                magic.Clear();
                current_officer2 = 0;
                off_bullet.Clear();
                RIP = true;
            }
            if (RIP)
            {
                img pnn = new img();
                pnn.photo = new Bitmap("game-over.png");
                pnn.photo.MakeTransparent(pnn.photo.GetPixel(0, 0));
                pnn.rdes = new Rectangle(this.Width / 2 - 365, 15, 630, 154);
                pnn.rsrc = new Rectangle(0, 0, 761, 214);
                logo.Add(pnn);
                /////////////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("restart.png");
                pnn.photo.MakeTransparent(pnn.photo.GetPixel(0, 0));
                pnn.rdes = new Rectangle(this.Width / 2 - 365, this.Height / 2, 220, 154);
                pnn.rsrc = new Rectangle(0, 0, 395, 174);
                logo.Add(pnn);
                /////////////////////////////////////////////////////////////
                pnn = new img();
                pnn.photo = new Bitmap("end.png");
                pnn.photo.MakeTransparent(pnn.photo.GetPixel(0, 0));
                pnn.rdes = new Rectangle(this.Width / 2, this.Height / 2, 220, 154);
                pnn.rsrc = new Rectangle(0, 0, 478, 174);
                logo.Add(pnn);

            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawbuff(e.Graphics);
        }
        void drawbuff(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawscene(g2);
            g.DrawImage(off, 0, 0);
        }
        void drawscene(Graphics g2)
        {
            g2.Clear(Color.Black);
            Pen p = new Pen(Color.Red, 3);
            if (stage == 0)
            {
                g2.DrawImage(background, 0, 0, ClientSize.Width, ClientSize.Height);
                for (int i = 0; i < users.Count; i++)
                {
                    g2.DrawImage(users[i].hp, users[i].x, users[i].y, users[i].width, users[i].height);

                }
                for (int i = 0; i < enmy1.Count; i++)
                {
                    g2.DrawImage(enmy1[i].hp, enmy1[i].x, enmy1[i].y, enmy1[i].width, enmy1[i].height);

                }
                for (int i = 0; i < logos.Count; i++)
                {
                    g2.DrawImage(logos[i].hp, logos[i].x, logos[i].y, logos[i].width, logos[i].height);

                }
                for (int i = 0; i < start.Count; i++)
                {
                    g2.DrawImage(start[i].hp, start[i].x, start[i].y, start[i].width, start[i].height);

                }
            }
            else if (stage == 1)
            {
                for (int i = 0; i < bg.Count; i++)
                {
                    img ptrav = bg[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < ladder.Count; i++)
                {
                    img ptrav = ladder[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < thistle.Count; i++)
                {
                    img ptrav = thistle[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < stand.Count; i++)
                {
                    img ptrav = stand[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < lassermachine.Count; i++)
                {
                    img ptrav = lassermachine[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < checkpoint.Count; i++)
                {
                    person ptrav = checkpoint[i];
                    g2.DrawImage(ptrav.photo[point], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                }
                for (int i = 0; i < elevator.Count; i++)
                {
                    img ptrav = elevator[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < magic_key.Count; i++)
                {
                    img ptrav = magic_key[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < floor.Count; i++)
                {
                    img ptrav = floor[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < downlasser.Count; i++)
                {
                    img ptrav = downlasser[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < magic.Count; i++)
                {
                    img ptrav = magic[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < hero.Count; i++)
                {
                    person ptrav = hero[i];
                    //g2.DrawRectangle(p, hero[0].x, hero[0].y, hero[0].width, hero[0].height);
                    if (current_state == 0)
                    {
                        g2.DrawImage(ptrav.photo[0], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_state == 1)
                    {
                        g2.DrawImage(ptrav.photo[index], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_state == 2)
                    {
                        g2.DrawImage(ptrav.photo[index2], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_state == 3)
                    {
                        g2.DrawImage(ptrav.photo[index3], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_state == 4)
                    {
                        g2.DrawImage(ptrav.photo[index4], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_state == 5)
                    {
                        g2.DrawImage(ptrav.photo[index5], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_state == 6)
                    {
                        g2.DrawImage(ptrav.photo[index6], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_state == 7)
                    {
                        g2.DrawImage(ptrav.photo[index7], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_state == 8)
                    {
                        g2.DrawImage(ptrav.photo[index8], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                }
                for (int i = 0; i < officer1.Count; i++)
                {
                    person ptrav = officer1[i];
                    if (current_officer1 == 0)
                    {
                        g2.DrawImage(ptrav.photo[0], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_officer1 == 1)
                    {
                        g2.DrawImage(ptrav.photo[1], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_officer1 == 2)
                    {
                        g2.DrawImage(ptrav.photo[2], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_officer1 == 3)
                    {
                        g2.DrawImage(ptrav.photo[3], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                }
                for (int i = 0; i < officer2.Count; i++)
                {
                    person ptrav = officer2[i];
                    if (current_officer2 == 0)
                    {
                        g2.DrawImage(ptrav.photo[0], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_officer2 == 1)
                    {
                        g2.DrawImage(ptrav.photo[index_officer2], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                    if (current_officer2 == 2)
                    {
                        g2.DrawImage(ptrav.photo[5], ptrav.x, ptrav.y, ptrav.width, ptrav.height);
                    }
                }

                for (int i = 0; i < claw_photo.Count; i++)
                {
                    img ptrav = claw_photo[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < single_bullet.Count; i++)
                {
                    img ptrav = single_bullet[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < multi_bullet.Count; i++)
                {
                    img ptrav = multi_bullet[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
                for (int i = 0; i < off_bullet.Count; i++)
                {
                    img ptrav = off_bullet[i];
                    g2.DrawImage(ptrav.photo, ptrav.rdes, ptrav.rsrc, GraphicsUnit.Pixel);
                }
              
                for (int i = 0; i < logo.Count; i++)
                {
                    g2.DrawImage(logo[i].photo, logo[i].rdes, logo[i].rsrc, GraphicsUnit.Pixel);
                }
            }
            else if (stage == 2) 
            {
                g2.DrawImage(background, 0, 0, ClientSize.Width, ClientSize.Height);
                //for (int i = 0; i < menu.Count; i++)
                //{
                //    g2.DrawImage(menu[i].photo,menu[i].rdes,menu[i].rsrc,GraphicsUnit.Pixel);
                //}
                for (int i = 0; i < hero_with_crown.Count; i++)
                {
                    g2.DrawImage(hero_with_crown[i].hp, hero_with_crown[i].x, hero_with_crown[i].y, hero_with_crown[i].width, hero_with_crown[i].height);
                }
                for (int i = 0; i < logo.Count; i++)
                {
                    g2.DrawImage(logo[i].photo, logo[i].rdes, logo[i].rsrc, GraphicsUnit.Pixel);
                }
            }
        }
    }
}
