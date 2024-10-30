using Joosh.DotnetInstallHelper.Console.Extensions.System.Collections.Generic;
using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console.Runtimes;
internal class ListInstalledRuntimes
{
    public static void Execute()
    {
        var raw = CommandProcessor.RunCommand("dotnet", ["--list-runtimes"]);

        var lines = raw.Split('\n').Select(x => new DotnetSdkOrRuntime(x));

        var table = lines.Tabulate().Title("Installed .NET runtimes");

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
    }
}
