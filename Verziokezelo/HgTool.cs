using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verziokezelo
{
    public class HgTool
    {
        string _path;
        long _oldRev;
        long _newRev;
        string _destination;

        public string MyPath { get { return _path; } }
        public long OldRev { get { return _oldRev; } }
        public long NewRev { get { return _newRev; } }

        public string Destination { get { return _destination; } }

        public HgTool(string path, long oldrev, long newrev, string destination)
        {
            this._path = path;
            this._oldRev = oldrev;
            this._newRev = newrev;
            this._destination = destination;
        }

        public string CreateHGCommand()
        {
            String command = @"cd " + this.MyPath +
                @" & hg status --rev " + this.OldRev + @" --rev " + this.NewRev +
                @"  1>" + this.Destination + @"\objects.txt & exit";
            return command;
        }

        public string CreateRpCommand()
        {
            return @"cd " + this.Destination +
                @" & rputil -ir FullExport -op teszt.ipj & exit";
        }

        public string CreateSolution()
        {
            return @"cd " + this.Destination +
                @" & rputil -ir Standard -ob SolutionPack/delta/rp/Solution & exit";
        }

        public void ModifyVersionNumbers()
        {
            StreamReader reader = new StreamReader(this.Destination + @"\teszt.ipj");
            string? line;
            using (StreamWriter writer = new StreamWriter(this.Destination + @"\modified.ipj"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#Version"))
                    {
                        string version = line.Substring(line.IndexOf('"'));
                        if (int.Parse(version.Split(".")[6].Replace('"'.ToString(),"")) == 0) {
                            int number = int.Parse(version.Split(".")[5]);
                            int newNumber = number - 1;
                            line = line.Replace(number.ToString(), newNumber.ToString());
                        }
                    }
                        writer.WriteLine(line);
                }
                reader.Close();
                writer.Close();
            }
        }

        public void CopyFiles()
        {
            String[] file = File.ReadAllLines(this.Destination + @"\objects.txt");

            for(int i=0;i<file.Length;i++)
            {
                string[] line = file[i].Split(" ", 2);
                if (!line[0].Equals("R") && !line[1].StartsWith("."))
                {
                    string[] splitted = line[1].Split(@"\");
                    if (!Directory.Exists(this.Destination + @"\" + line[1].Replace(@"\" + splitted[splitted.Length - 1], ""))){
                        Directory.CreateDirectory(this.Destination + @"\" + line[1].Replace(@"\" + splitted[splitted.Length - 1], ""));
                    }
                    File.Copy(this.MyPath + @"\" + line[1], this.Destination + @"\" + line[1]);
                    CopyFileIfStandard(line[1]);

                }
            }
        }

        public void CopyFileIfStandard(string filePath)
        {
            if (File.Exists(this.Destination + @"\" + filePath))
            {
                StreamReader reader = new StreamReader(this.Destination + @"\" + filePath);
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#Version"))
                    {
                        string version = line.Substring(line.IndexOf('"'));
                        if (int.Parse(version.Split(".")[6].Replace('"'.ToString(), "")) == 0)
                        {
                            string[] splitted = filePath.Split(@"\");
                            if (!Directory.Exists(this.Destination + @"\Standard\" + filePath.Replace(@"\" + splitted[splitted.Length -1], "")))
                            {
                                Directory.CreateDirectory(this.Destination + @"\Standard\" + filePath.Replace(@"\" + splitted[splitted.Length - 1], ""));
                            }
                            File.Copy(this.Destination + @"\" + filePath, this.Destination + @"\Standard\" + filePath);
                        }
                    }
                }
                reader.Close();
            }
        }

        public void CreateSolutionStructure()
        {
            Directory.CreateDirectory(this.Destination + @"\SolutionPack\delta\rp");
            Directory.CreateDirectory(this.Destination + @"\SolutionPack\inforCom");
        }

        public void CopyDirectory(string sourceDir, string destinationDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir);
            }
        }

        public void CopySysbox()
        {
            if (Directory.Exists(this.Destination + @"\Sysbox"))
            {
                CopyDirectory(this.Destination + @"\Sysbox", this.Destination + @"SolutionPack\delta\sysbox");
            }
        }

        public void CleanUp()
        {
            DeleteIfExists(this.Destination + @"\FullExport");
            DeleteIfExists(this.Destination + @"\InforCOM");
            DeleteIfExists(this.Destination + @"\SolutionPack");
            DeleteIfExists(this.Destination + @"\Standard");
            DeleteIfExists(this.Destination + @"\Sysbox");
            DeleteIfExists(this.Destination + @"\objects.txt");
            DeleteIfExists(this.Destination + @"\teszt.ipj");
        }

        public void DeleteIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            } else if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

    }
}
