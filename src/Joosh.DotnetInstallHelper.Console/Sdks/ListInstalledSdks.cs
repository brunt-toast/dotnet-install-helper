using Joosh.DotnetInstallHelper.Console.Extensions.System.Collections.Generic;
using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console.Sdks;
internal class ListInstalledSdks
{
    public static void Execute()
    {
        var raw = CommandProcessor.RunCommand("dotnet", ["--list-sdks"]);

        var lines = raw.Split('\n').Select(x => new DotnetSdkOrRuntime(x));

        var table = lines.Tabulate().Title("Installed .NET SDKs");

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
    }
}
