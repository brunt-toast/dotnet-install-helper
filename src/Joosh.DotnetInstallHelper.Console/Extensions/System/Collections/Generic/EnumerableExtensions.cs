using Spectre.Console;
using System.Reflection;

namespace Joosh.DotnetInstallHelper.Console.Extensions.System.Collections.Generic;

internal static class EnumerableExtensions
{
    public static Table Tabulate<T>(this IEnumerable<T> data) where T : class
    {
        Table table = new Table();

        var publicMembers = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.GetMethod is not null && x.GetMethod.IsPublic)
            .ToArray();

        foreach (var member in publicMembers)
        {
            table.AddColumn(member.Name);
        }

        foreach (T row in data)
        {
            string[] rowData = publicMembers.Select(member => typeof(T).GetProperty(member.Name)?.GetValue(row, null)?.ToString() ?? "").ToArray();
            table.AddRow(rowData);
        }

        return table;
    }
}
