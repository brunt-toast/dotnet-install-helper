using Joosh.DotnetInstallHelper.Console.Nuget;
using Joosh.DotnetInstallHelper.Console.Runtimes;
using Joosh.DotnetInstallHelper.Console.Sdks;
using Joosh.DotnetInstallHelper.Console.Tools;
using Joosh.DotnetInstallHelper.Console.Workloads;
using Joosh.DotnetInstallHelper.Console.Build;
using Joosh.DotnetInstallHelper.Console.Run;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Joosh.DotnetInstallHelper.Console;

class Program
{
    public static void Main(string[] args)
    {
        if (!args.Contains("--skipSplashScreen")) ShowSplashScreen();

        CheckDotnetInstalled();

        const string listInstalledSdks = "List installed SDKs";
        const string listInstalledRuntimes = "List installed runtimes";
        const string manageSource = "Manage sources";
        const string buildProject = "Build project";
        const string runProject = "Run project";
        const string runDotnetInstaller = "Run .NET Installer";
        const string showToolsMenu = "+ Tools";
        const string showWorkloadsMenu = "+ Workloads";
        const string exitProgram = "Quit";

        var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Function:")
            .AddChoices([
                listInstalledSdks,
                listInstalledRuntimes,
                manageSource,
                buildProject,
                runProject,
                runDotnetInstaller,
                showToolsMenu,
                showWorkloadsMenu,
                exitProgram
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
            case buildProject:
                BuildProject.Execute();
                break;
            case runProject:
                RunProject.Execute();
                break;
            case runDotnetInstaller:
                InstallDotNet();
                break;
            case showToolsMenu:
                ShowToolsMenu();
                break;
            case showWorkloadsMenu:
                ShowWorkloadsMenu();
                break;
            case exitProgram:
                Environment.Exit(0);
                break;
        }
    }

    private static void ShowWorkloadsMenu()
    {
        const string searchWorkloads = "Search workloads";
        const string removeWorkloads = "Remove workloads";
        const string updateWorkloads = "Update workloads";
        const string goBack = "< Back";

        AnsiConsole.Clear();
        var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Function:")
            .AddChoices([
                searchWorkloads,
                removeWorkloads,
                updateWorkloads,
                goBack
            ]));

        switch (fn)
        {
            case searchWorkloads:
                SearchWorkloads.Execute();
                break;
            case removeWorkloads:
                RemoveWorkloads.Execute();
                break;
            case updateWorkloads:
                UpdateWorkloads.Execute();
                break;
            case goBack:
                Main(["--skipSplashScreen"]);
                break;
        }
    }

    private static void ShowToolsMenu()
    {
        const string searchTools = "Search tools";
        const string removeTools = "Remove tools";
        const string goBack = "< Back";

        AnsiConsole.Clear();
        var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Function:")
            .AddChoices([
                searchTools,
                removeTools,
                goBack
            ]));

        switch (fn)
        {
            case searchTools:
                SearchTools.Execute();
                break;
            case removeTools:
                RemoveTools.Execute();
                break;
            case goBack:
                Main(["--skipSplashScreen"]);
                break;
        }
    }

    private static void ShowSplashScreen(int delay = 1000)
    {
        var content = new Layout("Root").SplitRows(new Layout("TopHeader") { Size = 14 }.SplitRows(new Layout("HeaderLine1") { Size = 6 }, new Layout("HeaderLine2") { Size = 6 }), new Layout("BottomDetails"));
        content["TopHeader"]["HeaderLine1"].Update(new FigletText(".NET Install").Centered().Color(Color.Purple));
        content["TopHeader"]["HeaderLine2"].Update(new FigletText("Helper").Centered().Color(Color.Purple));
        content["BottomDetails"].Update(new Markup($"\ueb29 {GetBuildNumber()}\n\uf09b brunt-toast/dotnet-install-helper").Centered());

        var panel = new Panel(content);
        panel.Height = 20;
        panel.Expand = true;
        panel.Header = new PanelHeader("Loading...").SetAlignment(Justify.Center);
        panel.Border = BoxBorder.Rounded;

        AnsiConsole.Write(panel);

        Thread.Sleep(delay);
        AnsiConsole.Clear();
    }

    private static string GetBuildNumber()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        return Regex.Replace(fileVersionInfo.ProductVersion ?? "Unknown", @"\+.*", "+...");
    }

    private static void CheckDotnetInstalled() 
    {
        Process process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = "--version";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        try
        {
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            
            if (process.ExitCode == 0) 
            {
                AnsiConsole.WriteLine($"dotnet {output}");
            }
            else 
            {
                AnsiConsole.Write(new Markup("[red]dotnet is not installed or was not found in PATH.[/]\n"));
                AnsiConsole.WriteLine("Error");

                string installDotnet = "Install .NET";
                string exitProgram = "Quit";
                var fn = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Function:")
                    .AddChoices([
                        installDotnet,
                        exitProgram
                    ]));

                if (fn == installDotnet) InstallDotNet();
                
                Environment.Exit(0);
            } 

        }
        catch
        {
            AnsiConsole.Write(new Markup("[yellow]Warning: Could not determine dotnet CLI version[/]"));
        }
    }

    private static void InstallDotNet() 
    {
#if WINDOWS
        AnsiConsole.WriteLine("Detected Windows (.ps1 install script)");
        string path = Path.Combine(Path.GetTempPath(), $"dotnet-install-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.ps1");
        string webLocation = @"https://raw.githubusercontent.com/dotnet/install-scripts/main/src/dotnet-install.ps1";
#else 
        AnsiConsole.WriteLine("Detected linux/macOS (.sh install script)");
        string path = Path.Combine(Path.GetTempPath(), $"dotnet-install-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.sh");
        string webLocation = @"https://github.com/dotnet/install-scripts/blob/main/src/dotnet-install.sh";
#endif

        AnsiConsole.WriteLine("Downloading install script...");
        using (HttpClient client = new())
        {
            try 
            {
                byte[] fileData = client.GetByteArrayAsync(webLocation).Result;
                File.WriteAllBytes(path, fileData);
            }
            catch 
            {
                AnsiConsole.Write(new Markup("[red]Failed to download install script[/]"));
                Environment.Exit(1);
            }
        }

        AnsiConsole.WriteLine("Installing...");
#if WINDOWS
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pwsh",
                Arguments = $"\"{path}\"",
                RedirectStandardOutput = true,          
                RedirectStandardError = true,
                UseShellExecute = false,                
                CreateNoWindow = true                   
            }
        };
#else 
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = path,
                RedirectStandardOutput = true,          
                RedirectStandardError = true,
                UseShellExecute = false,                
                CreateNoWindow = true                   
            }
        };
#endif
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
            AnsiConsole.Write(new Markup(process.ExitCode == 0
                ? "Install Successful"
                : $"Run exited with non-0 exit code: {process.ExitCode}"));
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
