using Spectre.Console;

namespace Joosh.DotnetInstallHelper.Console.Nuget;
internal class ManageSources
{
    /// <summary>
    ///     Enable or disable NuGet sources.
    /// </summary>
    public static void Execute()
    {
        var allSourcesStrings = CommandProcessor.RunCommand("dotnet", ["nuget", "list", "source"]).Split("\n").Skip(1).SkipLast(1).ToArray();
        List<NugetSource> allSources = [];

        for (int i = 0; i < allSourcesStrings.Length; i += 2)
        {
            allSources.Add(new NugetSource(allSourcesStrings[i], allSourcesStrings[i + 1]));
        }

        var prompt = new MultiSelectionPrompt<NugetSource>()
            .Title("Enable/disable sources")
            .NotRequired()
            .AddChoices(allSources)
            .UseConverter(x => x.Name);
        foreach (var source in allSources.Where(x => x.IsEnabled))
        {
            prompt.Select(source);
        }
        var result = AnsiConsole.Prompt(prompt);

        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Working...");

        Table table = new Table().Title("Sources").AddColumns(["source", "status"]);
        foreach (var source in allSources)
        {
            string operation = result.Contains(source) ? "enable" : "disable";
            table.AddRow(new Markup(source.Name), new Markup(operation == "enable" ? "[green]Enabled[/]" : "[red]Disabled[/]"));
            CommandProcessor.RunCommand("dotnet", ["nuget", operation, "source", source.Name]);
        }

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
    }
}