using System.Text.RegularExpressions;

namespace Joosh.DotnetInstallHelper.Console;

internal class DotnetSdkOrRuntime
{
    public string Name { get; }
    public string Path { get; }

    public DotnetSdkOrRuntime(string rawCommandOutput)
    {
        Name = Regex.Match(rawCommandOutput, @"^[^\[]*").Value.TrimEnd();
        Path = Regex.Match(rawCommandOutput, @"\[(.*)\]").Groups[1].Value;
    }
}