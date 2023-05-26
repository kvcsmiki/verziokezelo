using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verziokezelo
{
    public class CommandTool
    {
        public CommandTool() { }

        public void ExecuteCommand(string command)
        {

            ProcessStartInfo ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;
            ProcessInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process? Process = Process.Start(ProcessInfo);
            Process.WaitForExit();

        }

        public void CreateZip(string source, string target)
        {
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = @"C:\Program Files\7-Zip\7z.exe";
            p.Arguments = "a -t7z \"" + target + "\" \"" + source + "\\*\" -mx=9";
            p.WindowStyle = ProcessWindowStyle.Hidden;
            Process x = Process.Start(p);
            x.WaitForExit();
        }
    }
}
