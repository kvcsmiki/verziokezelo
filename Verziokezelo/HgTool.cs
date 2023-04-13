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
                @"  1>" + this.Destination + @"\objects.txt";
            return command;
        }

        public void CopyFiles()
        {
            String[] file = File.ReadAllLines(this.Destination + @"\objects.txt");

            for(int i=0;i<file.Length;i++)
            {
                if (!file[0].Equals("R"))
                {
                    File.Copy(file[2], this.Destination + @"\" + file[2]);
                }
            }
        }

    }
}
