using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joosh.DotnetInstallHelper.Console.Workloads;
internal class RemoveWorkloads
{
    public static void Execute()
    {
        var installedTools = CommandProcessor.RunCommand("dotnet", ["workload", "list"])
            .Split('\n')
            .Skip(2)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(s => s.Split(' ').First());

        AnsiConsole.Clear();
        var workloadsToRemove = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
            .Title("Select workload(s) to remove.")
            .NotRequired()
            .AddChoices(installedTools));

        foreach (var workload in workloadsToRemove)
        {
            AnsiConsole.WriteLine($"Uninstalling {workload}...");
            CommandProcessor.RunCommand("dotnet", ["workload", "uninstall", workload]);
            AnsiConsole.WriteLine($"Uninstalled {workload}");
        }
    }
}
