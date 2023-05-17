namespace Verziokezelo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CommandTool commandTool= new CommandTool();
            HgTool hgtool = new HgTool(args[0], long.Parse(args[1]), long.Parse(args[2]), args[3]);

            Console.WriteLine(hgtool.CreateHGCommand());
            commandTool.ExecuteCommand(hgtool.CreateHGCommand());

            Thread.Sleep(5000);
            hgtool.CopyFiles();

            Thread.Sleep(5000);
            commandTool.ExecuteCommand(hgtool.CreateRpCommand());
        }
    }
}