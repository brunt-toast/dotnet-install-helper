using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joosh.DotnetInstallHelper.Console.Workloads;
internal class SearchWorkloads
{
    public static void Execute()
    {
        // Get query from user
        var query = AnsiConsole.Prompt(new TextPrompt<string>("Search term:"));

        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Working...");

        // Find matching results, excluding those already installed 
        var installedWorkloads = CommandProcessor.RunCommand("dotnet", ["workload", "list"])
            .Split('\n')
            .Skip(2)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(s => s.Split(' ').First());
        var searchResults = CommandProcessor.RunCommand("dotnet", ["workload", "search", query])
            .Split('\n')
            .Skip(2)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Split(' ').First());

        // Prompt the user to select tools to install
        AnsiConsole.Clear();
        var workloadsToInstall = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
            .Title("Select workload(s) to install.")
            .NotRequired()
            .AddChoices(searchResults));

        // Install newly requested tools
        foreach (var workload in searchResults.Where(x => workloadsToInstall.Contains(x)))
        {
            AnsiConsole.WriteLine($"Installing {workload}...");
            CommandProcessor.RunCommand("dotnet", ["workload", "install", workload]);
            AnsiConsole.WriteLine($"Installed {workload}");
        }

        // Uninstall no longer requested tools
        foreach (var workload in installedWorkloads.Where(x => !workloadsToInstall.Contains(x)))
        {
            AnsiConsole.WriteLine($"Uninstalling {workload}...");
            CommandProcessor.RunCommand("dotnet", ["workload", "uninstall", workload]);
            AnsiConsole.WriteLine($"Uninstalled {workload}");
        }
    }
}
