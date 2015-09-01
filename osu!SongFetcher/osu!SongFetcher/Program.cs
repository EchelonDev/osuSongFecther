using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using nonficLib;

namespace osu_SongFetcher
{
    class Program
    {
        static string fsongs = "", fout = "";
        static string exeLoc = Environment.CurrentDirectory; // Get the location of EXE.
        static int FetchMode = 0;
        static bool checkMode(string value)
        {
            if (value == "-f")
            {
                FetchMode = 2;
            }
            else if (value == "-l")
            {
                FetchMode = 3;
            }
            else
                return false;

            return true;
        }
        static bool checkArg(string option, string value)
        {
            if (option == "-s")
            {
                fsongs = @value;
            }
            else if (option == "-o")
            {
                fout = @value;
            }
            else
                return false;

            return true;
        }
        static bool handleArguments(string[] args)
        {
            if (args.Length == 1)
            {
                return checkMode(args[0]);
            }
            else if (args.Length == 3)
            {
                return (checkMode(args[0]) && checkArg(args[1], args[2]));
            }
            else if (args.Length == 5)
            {
                return (checkMode(args[0]) && checkArg(args[1], args[2]) && checkArg(args[3], args[4]));
            }
            else
                return false;
        }
        public static string StringStyle(string arg)
        {
            int reqIndex = 0;
            arg = arg.Substring(arg.LastIndexOf("\\") + 1);
            char[] argChar = arg.ToCharArray();
            if (argChar[0] == '0' || argChar[0] == '1' || argChar[0] == '2' || argChar[0] == '3' || argChar[0] == '4' || argChar[0] == '5' || argChar[0] == '6' || argChar[0] == '7' || argChar[0] == '8' || argChar[0] == '9')
            {
                for (int i = 0; i < argChar.Length; i++)
                {
                    if (argChar[i] == ' ')
                    {
                        reqIndex = i;
                        break;
                    }
                }
                arg = arg.Substring(reqIndex + 1);
            }
            return arg + ".mp3";
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
        public static void FetchSong(int mode)
        {
            FetchSong(mode, "","");
        }
        public static void FetchSong(int mode,string osuDest)
        {
            FetchSong(mode, osuDest, "");
        }
        public static void FetchSong(int mode,string osuDest,string fout)
        {
            if (mode == 1)
            {
                Console.Write("Please give the osu! path : ");
                while (checkOsuDest(osuDest) == false)
                {
                    DialogResult result = new DialogResult();
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    result = fbd.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        osuDest = fbd.SelectedPath;
                    }
                    else if(result == DialogResult.Cancel)
                    {
                        System.Environment.Exit(1);
                    }
                }
                string fsongs = osuDest + "\\Songs";
                try
                {
                    var folders = Directory.GetDirectories(fsongs);
                    foreach (var folder in folders)
                    {
                        string songName = StringStyle(folder.ToString());
                        if (songName != "Failed")
                        {
                            Console.WriteLine(songName); // console de satır limiti var heralde .s .s son 200 felan gösteriyor
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
                return;
            } else if(mode == 0)
            {
                Console.WriteLine("Please give the osu! path : ");
                while (checkOsuDest(osuDest) == false)
                {
                    DialogResult result = new DialogResult();
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    result = fbd.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        osuDest = fbd.SelectedPath;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        System.Environment.Exit(1);
                    }
                }
                Console.WriteLine("Please give the path for output : ");
                DialogResult result2 = new DialogResult();
                FolderBrowserDialog fbd2 = new FolderBrowserDialog();
                result2 = fbd2.ShowDialog();
                if (result2 == DialogResult.OK)
                {
                    fout = fbd2.SelectedPath;
                }
                else if (result2 == DialogResult.Cancel)
                {
                    System.Environment.Exit(1);
                }
                int curCount = 0;
                int topCount = 0;
                int realCount = 0;
                try
                {
                    fsongs = osuDest + "\\Songs";
                    var folders = Directory.GetDirectories(fsongs);
                    foreach (var folder in folders)
                    {
                        var filenames = Directory.GetFiles(folder, "*.mp3").Select(Path.GetFileName);
                        foreach (var f in filenames)
                        {
                            if (HitSoundControl(f.ToString()) == true)
                            {
                                topCount++;
                            }
                        }
                    }
                    foreach (var folder in folders)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("In\t: " + folder.ToString().Substring(folder.LastIndexOf("\\") + 1));
                        var filenames = Directory.GetFiles(folder, "*.mp3").Select(Path.GetFileName);
                        foreach (var f in filenames)
                        {
                            if (HitSoundControl(f.ToString()) == true)
                            {
                                try
                                {
                                    string fname = StringStyle(folder.ToString());
                                    File.Copy(Path.Combine(folder, f), Path.Combine(fout, fname));
                                    Console.Write("Out\t: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine(fname + "\n");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    curCount++;
                                    realCount++;
                                }
                                catch (Exception e)
                                {
                                    Console.Write("Error\t: ");
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine(e.Message + "\n");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    curCount++;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("\nSkipping hitsound.\n");
                            }
                            Console.Title = "Fetching Songs - " + curCount + "/" + topCount + " " + TitleLoad(curCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Fetching is done... {0} Songs is Fetched. Press any button to exit !", realCount);
                Console.ReadKey();
                Console.Title = "Osu Song Fetcher - Echelon";
                System.Environment.Exit(1);
            }
            else if(mode == 2)
            {
                if (CreateFolder(@fout))
                    Console.WriteLine("Folder created.\n");
                else
                {
                    Console.WriteLine("Folder could not be created.");
                    return;
                }
                int curCount = 0;
                int topCount = 0;
                int realCount = 0;
                try
                {
                    fsongs = osuDest + "\\Songs";
                    var folders = Directory.GetDirectories(fsongs);
                    foreach (var folder in folders)
                    {
                        var filenames = Directory.GetFiles(folder, "*.mp3").Select(Path.GetFileName);
                        foreach (var f in filenames)
                        {
                            if (HitSoundControl(f.ToString()) == true)
                            {
                                topCount++;
                            }
                        }
                    }
                    foreach (var folder in folders)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("In\t: " + folder.ToString().Substring(folder.LastIndexOf("\\") + 1));
                        var filenames = Directory.GetFiles(folder, "*.mp3").Select(Path.GetFileName);
                        foreach (var f in filenames)
                        {
                            if (HitSoundControl(f.ToString()) == true)
                            {
                                try
                                {
                                    string fname = StringStyle(folder.ToString());
                                    File.Copy(Path.Combine(folder, f), Path.Combine(fout, fname));
                                    Console.Write("Out\t: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine(fname + "\n");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    curCount++;
                                    realCount++;
                                }
                                catch (Exception e)
                                {
                                    Console.Write("Error\t: ");
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine(e.Message + "\n");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    curCount++;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("\nSkipping hitsound.\n");
                            }
                            Console.Title = "Fetching Songs - " + curCount + "/" + topCount + " " + TitleLoad(curCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Fetching is done... {0} Songs is Fetched. Press any button to exit !", realCount);
                Console.ReadKey();
                Console.Title = "Osu Song Fetcher - Echelon";
                System.Environment.Exit(1);
            }
            else if (mode == 3)
            {
                string fsongs = osuDest + "\\Songs";
                try
                {
                    var folders = Directory.GetDirectories(fsongs);
                    foreach (var folder in folders)
                    {
                        string songName = StringStyle(folder.ToString());
                        if (songName != "Failed")
                        {
                            Console.WriteLine(songName); // console de satır limiti var heralde .s .s son 200 felan gösteriyor
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
                return;
            }
        }
        public static string TitleLoad(int count)
        {
            switch(count%8)
            {
                case 0: return "ǀ";
                case 1: return "/";
                case 2: return "―";
                case 3: return "\\";
                case 4: return "|";
                case 5: return "/";
                case 6: return "―";
                case 7: return "\\";
                default: return "ǀ";
            }
        }
        public static bool CreateFolder(string fout)
        {
            try
            {
                System.IO.Directory.CreateDirectory(fout);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static string folderName(string directory)
        {
            int lastBackSlash = directory.LastIndexOf('\\');
            if (lastBackSlash == -1)
            { return directory; }
            else
            { return directory.Substring(lastBackSlash + 1); }
        }
        public static bool HitSoundControl(string fileName)
        {
            string[] HitSounds = { "clap", "hitsound", "hitfinish", "slider", "whistle", "hitclap", "tutorial" };
            bool flag = true;
            for (int i = 0; i < HitSounds.Length; i++)
            {
                if (fileName.Contains(HitSounds[i]))
                { flag = false; }
            }
            return flag;
        }
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Osu Song Fetcher - Echelon";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WindowWidth = 95;
            Console.BufferWidth = 95;
            Console.BackgroundColor = ConsoleColor.Black;            
            if (args.Length > 0)
            {
                if (!handleArguments(args))
                {
                    Console.WriteLine("There is an error in your parameters.\n");
                    return;
                }
                else
                {
                    if (FetchMode == 2)
                    {
                        FetchSong(2, fsongs, fout);
                    }
                    else if (FetchMode == 3)
                    {
                        FetchSong(3, fsongs);
                    }
                }
            }
            else
            {
                while (true)
                {
                    Console.Clear();
                    nonficLib.Menu MyMenu = new nonficLib.Menu("Fetch Songs", "List Songs", "Exit");
                    MyMenu.SetColors(ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Cyan, ConsoleColor.Black);
                    int MenuSelectedIndex = MyMenu.Display(0, 2);
                    switch (MenuSelectedIndex)
                    {
                        case 0: FetchSong(0); break;
                        case 1: FetchSong(1); break;
                        case 2: System.Environment.Exit(1); return;
                    }
                }
            }
            
        }
    }
}
