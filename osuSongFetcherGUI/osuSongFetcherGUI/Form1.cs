using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osuSongFetcherGUI
{
    public partial class Form1 : Form
    {
        static string osuDest = @"";
        static string fout = @"";
        static string[] LogicalDrives = System.Environment.GetLogicalDrives();
        static string exeLoc = Environment.CurrentDirectory; // Get the location of EXE.
        public Form1()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "OsuSongFetcher GUI - Echelon";
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }
        static bool checkOsuDest(string osuDest)
        {
            if (File.Exists(osuDest + "\\osu!.exe"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static bool checkOutputLocation(string fout)
        {
            if(fout == "")
            {
                return false;
            }
            else if (fout != "")
            { 
                for (int i = 0; i < LogicalDrives.Length; i++)
                {
                    if (fout == LogicalDrives[i])
                    {
                        MessageBox.Show("We dont recommend outputting there","Error !",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = new DialogResult();
            do
            {
                if (osuDest != "" && checkOsuDest(osuDest) == false)
                {
                    MessageBox.Show("Please select correct osu!.exe location", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    osuDest = fbd.SelectedPath;
                    textBox1.Text = osuDest;
                }
                else if(result == DialogResult.Cancel)
                {
                    textBox1.Text = null;
                    break;
                }
            } while (checkOsuDest(osuDest) == false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = new DialogResult();
            do
            {
                result = fbd2.ShowDialog();
                if (result == DialogResult.OK)
                {
                    fout = fbd2.SelectedPath;
                    textBox2.Text = fout;
                }
                else if (result == DialogResult.Cancel)
                {
                    textBox2.Text = null;
                    break;
                }
            } while (checkOutputLocation(fout) == false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (osuDest != "" && fout != "")
            {
                ProcessStartInfo psi = new ProcessStartInfo(@"osf.exe", "-f -s \"" + osuDest + "\" -o \"" + fout + "\" ");
                Process.Start(psi);
            }
            else if (osuDest == "" && fout == "")
            {
                MessageBox.Show("Please select osu!.exe and output location", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (osuDest == "")
            {
                MessageBox.Show("Please select osu!.exe location", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (fout == "")
            {
                MessageBox.Show("Please select output location", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(osuDest != "")
            {
                ProcessStartInfo psi2 = new ProcessStartInfo(@"osf.exe", "-l -s \"" + osuDest + "\"");
                Process.Start(psi2);
            }
            else if (osuDest == "")
            {
                MessageBox.Show("Please select osu!.exe location", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
