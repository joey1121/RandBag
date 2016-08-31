using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandBag
{
    public partial class Form1 : Form
    {
        private int min=0;
        private int max=0;
        private int countdown_t = 0;
        private int countdown_n = 0;
        private int draw_size = 0;
        private int _grid = 20;
        private string soundsource = null;
        Timer timerflash = new Timer();
        Timer timerprogress = new Timer();
        int progressvalue = 0;
        Random random = new Random();
        List<string> drawBag;
        ToolTip toolBarTip;

        public Form1()
        {
            InitializeComponent();
            timerflash.Tick += timerflash_Tick;
            timerprogress.Tick += timerprogress_Tick;
            timerflash.Interval = (57) * (1);
            int.TryParse(textBox5.Text, out countdown_t);
            int.TryParse(textBox2.Text, out countdown_n);
            progressBar1.Visible = false;
            button1.Visible = true;
            button1.Select();
            toolBarTip = new ToolTip();
            drawBag = new List<string>();
            toolBarTip.ShowAlways = true;            
            toolBarTip.SetToolTip(textBox4, "Copy/Paste your Number(s)");
            toolBarTip.SetToolTip(progressBar1, "Click to Pick a Number");
            toolBarTip.SetToolTip(button1, "Click to Pick Number(s)");
        }

        void timerflash_Tick(object sender, EventArgs e)
        {            
            int ran;

            ran = random.Next(min, max + 1);

            label4.Text = ran.ToString();

            if (draw_size <= max - min + 1 )
            while (drawBag.Contains(label4.Text) )
            {
                ran = random.Next(min, max + 1);
                label4.Text = ran.ToString();

            };


        }

        void timerprogress_Tick(object sender, EventArgs e)
        {
            progressvalue++;
            if ( (progressvalue%_grid) ==0 )
            {
                if (progressvalue > progressBar1.Maximum)
                {
                    timerprogress.Enabled = false;
                }
                else if (progressBar1.Value > progressvalue)
                    progressvalue = progressBar1.Value;
                else
                {
                    progressBar1.Value = progressvalue;
                    if (!picknum())
                    {
                        progressvalue = 0;
                        timerprogress.Enabled = false;
                    }
                }
            }
            else if (progressvalue <= progressBar1.Maximum)
            {
                if (progressBar1.Value > progressvalue)
                    progressvalue = progressBar1.Value;
                else progressBar1.Value = progressvalue;
            }
        }

        private bool picknum()
        {
            if (timerflash.Enabled)
            {
                if (countdown_n > 0)
                {
                    SoundPlayer soundPlayer = new SoundPlayer(soundsource);
                    soundPlayer.Play();

                       
                    if (textBox4.Text.Length == 0)
                    {
                        textBox4.Text = label4.Text;
                    }
                    else
                        textBox4.Text += ", " + label4.Text;
                    textBox4.SelectionStart = textBox4.TextLength;
                    textBox4.ScrollToCaret();
                    countdown_n--;
                    progressBar1.Value = progressBar1.Maximum - countdown_n*_grid;
                    drawBag.Add(label4.Text);
                }

                if (countdown_n <= 0)
                {
                    timerflash.Enabled = false;
                    timerflash.Stop();
                    timerprogress.Enabled = false;
                    timerprogress.Stop();
                    progressBar1.Visible = false;
                    button1.Visible = true;
                    button1.Select();
                }

            }
            else
            {

                    textBox4.Text = null;


                timerflash.Enabled = true;
                timerprogress.Enabled = true;
                timerflash.Start();
                timerprogress.Start();
            }
            if (countdown_n <= 0) return false;
            else return true;
        }

        private void range_validate() {
            int org_countdown_n = countdown_n;
            int org_countdown_t = countdown_t;
            if (!int.TryParse(textBox1.Text, out min))
                min = 0;

            if (!int.TryParse(textBox3.Text, out max))
                max = 0;

            if (!int.TryParse(textBox5.Text, out countdown_t))
                countdown_t = org_countdown_t;

            if (!int.TryParse(textBox2.Text, out countdown_n))
                countdown_n = org_countdown_n;


            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }
            textBox1.Text = min.ToString();
            textBox3.Text = max.ToString();
            textBox5.Text = countdown_t.ToString();
            textBox2.Text = countdown_n.ToString();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = countdown_n*_grid;
            progressBar1.Value = 0;
            timerprogress.Interval = countdown_t*1000/countdown_n/_grid;
            progressvalue = 0;
            draw_size = countdown_n;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            textBox3.Text = Properties.Settings.Default.Maximum;
            textBox1.Text = Properties.Settings.Default.Minimum;
            soundsource = Properties.Settings.Default.SoundSource;
            textBox5.Text = Properties.Settings.Default.TotalTime;
            textBox2.Text = Properties.Settings.Default.Number;
            _grid = Properties.Settings.Default.Grid;

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            picknum();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Maximum = textBox3.Text;
            Properties.Settings.Default.Minimum = textBox1.Text;
            Properties.Settings.Default.TotalTime = textBox5.Text;
            Properties.Settings.Default.Number = textBox2.Text;
            
            Properties.Settings.Default.Save();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            range_validate();
            if (max != min)
            {
                drawBag.Clear();
                button1.Visible = false;
                progressBar1.Visible = true;
                progressBar1.Select();
                picknum();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {

        }

    }
}
