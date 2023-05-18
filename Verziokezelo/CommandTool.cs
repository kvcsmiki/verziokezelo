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

            Process? Process = Process.Start(ProcessInfo);
            Process.WaitForExit();

        }
    }
}
