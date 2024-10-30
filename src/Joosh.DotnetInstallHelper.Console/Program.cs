using Joosh.DotnetInstallHelper.Console.Runtimes;
using Joosh.DotnetInstallHelper.Console.Sdks;
using Joosh.DotnetInstallHelper.Console.Tools;
using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console;

class Program
{
    public static void Main(string[] args)
    {
        const string listInstalledSdks = "List installed SDKs";
        const string listInstalledRuntimes = "List installed runtimes";
        const string searchTools = "Search tools";
        const string removeTools = "Remove tools";

        var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Function:")
            .AddChoices([
                listInstalledSdks,
                listInstalledRuntimes,
                searchTools,
                removeTools
            ]));

        switch (fn)
        {
            case listInstalledSdks:
                ListInstalledSdks.Execute();
                break;
            case listInstalledRuntimes:
                ListInstalledRuntimes.Execute();
                break;
            case searchTools:
                SearchTools.Execute();
                break;
            case removeTools:
                RemoveTools.Execute();
                break;
        }
    }
}