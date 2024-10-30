using System.Diagnostics;

namespace Joosh.DotnetInstallHelper.Console;
internal static class CommandProcessor
{
    public static string RunCommand(string command, string[] args)
    {
        Process pProcess = new();

        pProcess.StartInfo.FileName = command;
        pProcess.StartInfo.Arguments = string.Join(' ', args);
        pProcess.StartInfo.UseShellExecute = false;
        pProcess.StartInfo.RedirectStandardOutput = true;

        pProcess.Start();
        string strOutput = pProcess.StandardOutput.ReadToEnd();
        pProcess.WaitForExit();

        return strOutput;
    }
}
