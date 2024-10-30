using System.Text.RegularExpressions;

namespace Joosh.DotnetInstallHelper.Console.Nuget;

internal class NugetSource
{
    public string Name { get; }
    public string Url { get; }
    public bool IsEnabled { get; }

    public NugetSource(string line1, string line2)
    {
        Name = Regex.Match(line1, @"\s+?\d+?\.?\s+?(.*)\[").Groups[1].Value;
        IsEnabled = Regex.Match(line1, @"\[(Enabled|Disabled)\]").Value == "[Enabled]";
        Url = line2;
    }
}