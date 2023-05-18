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

        public string Path { get { return _path; } }
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
            String command = @"cd " + this.Path +
                @" & hg status --rev " + this.OldRev + @" --rev " + this.NewRev +
                @"  1>" + this.Destination + @"\objects.txt & exit";
            return command;
        }

        public string CreateRpCommand()
        {
            return @"cd " + this.Destination +
                @" & rputil -ir FullExport -op teszt.ipj & exit";
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
                        writer.Write(line);
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
                    File.Copy(this.Path + @"\" + line[1], this.Destination + @"\" + line[1]);
                }
            }
        }

    }
}
