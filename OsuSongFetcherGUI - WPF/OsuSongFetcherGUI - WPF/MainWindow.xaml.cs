using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace OsuSongFetcherGUI___WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string osuDest = @"";
        static string fout = @"";
        static string[] LogicalDrives = System.Environment.GetLogicalDrives();
        static string exeLoc = Environment.CurrentDirectory; // Get the location of EXE.
        public MainWindow()
        {
            InitializeComponent();
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
            if (fout == "")
            {
                return false;
            }
            else if (fout != "")
            {
                for (int i = 0; i < LogicalDrives.Length; i++)
                {
                    if (fout == LogicalDrives[i])
                    {
                        MessageBox.Show("We dont recommend outputting there", "Error !", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            return true;
        }
        private void Form1_Loaded(object sender, RoutedEventArgs e)
        {
            Form1.Title = "Osu Song Fetcher GUI - Echelon";
            textBox1.IsReadOnly = true;
            textBox2.IsReadOnly = true;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }        
        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
