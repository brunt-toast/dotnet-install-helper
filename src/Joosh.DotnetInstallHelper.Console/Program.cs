using Joosh.DotnetInstallHelper.Console.Nuget;
using Joosh.DotnetInstallHelper.Console.Runtimes;
using Joosh.DotnetInstallHelper.Console.Sdks;
using Joosh.DotnetInstallHelper.Console.Tools;
using Joosh.DotnetInstallHelper.Console.Workloads;
using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console;

class Program
{
    public static void Main(string[] args)
    {
        ShowSplashScreen();

        const string listInstalledSdks = "List installed SDKs";
        const string listInstalledRuntimes = "List installed runtimes";
        const string manageSource = "Manage sources";
        const string showToolsMenu = "+ Tools";
        const string showWorkloadsMenu = "+ Workloads";

        var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Function:")
            .AddChoices([
                listInstalledSdks,
                listInstalledRuntimes,
                manageSource,
                showToolsMenu,
                showWorkloadsMenu
            ]));

        switch (fn)
        {
            case listInstalledSdks:
                ListInstalledSdks.Execute();
                break;
            case listInstalledRuntimes:
                ListInstalledRuntimes.Execute();
                break;
            case manageSource:
                ManageSources.Execute();
                break;
            case showToolsMenu:
                ShowToolsMenu();
                break;
            case showWorkloadsMenu:
                ShowWorkloadsMenu();
                break;
        }
    }

    private static void ShowWorkloadsMenu()
    {
        const string searchWorkloads = "Search workloads";
        const string removeWorkloads = "Remove workloads";

        AnsiConsole.Clear();
        var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Function:")
            .AddChoices([
                searchWorkloads,
                removeWorkloads
            ]));

        switch (fn)
        {
            case searchWorkloads:
                SearchWorkloads.Execute();
                break;
            case removeWorkloads:
                RemoveWorkloads.Execute();
                break;
        }
    }

    private static void ShowToolsMenu()
    {
        const string searchTools = "Search tools";
        const string removeTools = "Remove tools";

        AnsiConsole.Clear();
        var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Function:")
            .AddChoices([
                searchTools,
                removeTools,
            ]));

        switch (fn)
        {
            case searchTools:
                SearchTools.Execute();
                break;
            case removeTools:
                RemoveTools.Execute();
                break;
        }
    }

    private static void ShowSplashScreen(int delay = 1000)
    {
        var content = new Layout("Root").SplitRows(new Layout("TopHeader") { Size = 14 }.SplitRows(new Layout("HeaderLine1") { Size = 6 }, new Layout("HeaderLine2") { Size = 6 }), new Layout("BottomDetails"));
        content["TopHeader"]["HeaderLine1"].Update(new FigletText(".NET Install").Centered().Color(Color.Purple));
        content["TopHeader"]["HeaderLine2"].Update(new FigletText("Helper").Centered().Color(Color.Purple));
        content["BottomDetails"].Update(new Markup("\ueb29 0.0.0\n\uf09b brunt-toast/dotnet-install-helper").Centered());

        var panel = new Panel(content);
        panel.Height = 20;
        panel.Expand = true;
        panel.Header = new PanelHeader("Loading...").SetAlignment(Justify.Center);
        panel.Border = BoxBorder.Rounded;

        AnsiConsole.Write(panel);

        Thread.Sleep(delay);
        AnsiConsole.Clear();
    }
}