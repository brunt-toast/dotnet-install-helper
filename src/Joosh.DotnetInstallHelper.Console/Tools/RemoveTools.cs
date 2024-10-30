using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console.Tools;
internal class RemoveTools
{
    public static void Execute()
    {
        var installedTools = CommandProcessor.RunCommand("dotnet", ["tool", "list", "-g"])
            .Split('\n')
            .Skip(2)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(s => s.Split(' ').First());

        AnsiConsole.Clear();
        var toolsToRemove = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
            .Title("Select tool(s) to remove.")
            .NotRequired()
            .AddChoices(installedTools));

        foreach (var tool in toolsToRemove)
        {
            AnsiConsole.WriteLine($"Uninstalling {tool}...");
            CommandProcessor.RunCommand("dotnet", ["tool", "uninstall", "-g", tool]);
            AnsiConsole.WriteLine($"Uninstalled {tool}");
        }
    }
}
