using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console.Workloads;
internal class UpdateWorkloads
{
    public static void Execute()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Working...");
        CommandProcessor.RunCommand("dotnet", ["workload", "update"]);
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Updated workloads");
    }
}
