using Joosh.DotnetInstallHelper.Console.Runtimes;
using Joosh.DotnetInstallHelper.Console.Sdks;
using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console;

class Program
{
    public static void Main(string[] args)
    {
        const string listInstalledSdks = "List installed SDKs";
        const string listInstalledRuntimes = "List installed runtimes";

        var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Function:")
            .AddChoices([
                listInstalledSdks,
                listInstalledRuntimes,
            ]));

        switch (fn)
        {
            case listInstalledSdks:
                ListInstalledSdks.Execute();
                break;
            case listInstalledRuntimes:
                ListInstalledRuntimes.Execute();
                break;
        }
    }
}