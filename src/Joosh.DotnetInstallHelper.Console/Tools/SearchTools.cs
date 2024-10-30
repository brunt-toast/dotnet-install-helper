using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console.Tools;

internal class SearchTools
{
    public static void Execute()
    {
        // Get query from user
        var query = AnsiConsole.Prompt(new TextPrompt<string>("Search term:"));

        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Working...");

        // Find matching results, excluding those already installed 
        var installedTools = CommandProcessor.RunCommand("dotnet", ["tool", "list", "-g"])
            .Split('\n')
            .Skip(2)
            .Select(s => s.Split(' ').First());
        var searchResults = CommandProcessor.RunCommand("dotnet", ["tool", "search", query])
            .Split('\n')
            .Skip(2)
            .Select(x => x.Split(' ').First());

        // Prompt the user to select tools to install
        AnsiConsole.Clear();
        var toolsToInstall = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
            .Title("Select tool(s) to install.")
            .NotRequired()
            .AddChoices(searchResults));

        // Install newly requested tools
        foreach (var tool in searchResults.Where(x => toolsToInstall.Contains(x)))
        {
            AnsiConsole.WriteLine($"Installing {tool}...");
            CommandProcessor.RunCommand("dotnet", ["tool", "install", "-g", tool]);
            AnsiConsole.WriteLine($"Installed {tool}");
        }

        // Uninstall no longer requested tools
        foreach (var tool in installedTools.Where(x => !toolsToInstall.Contains(x)))
        {
            AnsiConsole.WriteLine($"Uninstalling {tool}...");
            CommandProcessor.RunCommand("dotnet", ["tool", "uninstall", "-g", tool]);
            AnsiConsole.WriteLine($"Uninstalled {tool}");
        }
    }
}
