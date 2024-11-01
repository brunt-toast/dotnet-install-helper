using Spectre.Console;
using System.Diagnostics;

namespace Joosh.DotnetInstallHelper.Console.Build; 

internal class BuildProject
{
    public static void Execute() 
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Finding projects...");
        string cwd = Directory.GetCurrentDirectory();
        string[] projects = Directory.GetFiles(cwd, "*.csproj", SearchOption.AllDirectories);

        string projectToBuild = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Choose a project:")
            .AddChoices(projects));

        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{projectToBuild}\"", 
                RedirectStandardOutput = true,          
                RedirectStandardError = true,
                UseShellExecute = false,                
                CreateNoWindow = true                   
            }
        };

        process.OutputDataReceived += (sender, args) => {
            if (args.Data is null) return;
            AnsiConsole.WriteLine(args.Data);
        };
        process.ErrorDataReceived += (sender, args) => AnsiConsole.WriteLine("ERROR: " + args.Data);

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine(process.ExitCode == 0
                ? "Build succeeded."
                : "Build failed with exit code: " + process.ExitCode);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine("An error occurred: " + ex.Message);
        }
        finally
        {
            process.Close();
        }
    }
}