using System.Text.RegularExpressions;

namespace Common.Utilities;

public static partial class Formatter
{
    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex PascalCaseRegex();

    public static string FormatePascalToKebab(string str)
    {
        return PascalCaseRegex().Replace(str, "$1-$2");
    }
}