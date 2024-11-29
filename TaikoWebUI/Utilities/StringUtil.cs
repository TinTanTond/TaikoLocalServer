namespace TaikoWebUI.Utilities;

public partial class StringUtil
{
    public static List<string> SplitIntoGroups(string str, int groupSize)
    {
        List<string> groups = [];
        for (int i = 0; i < str.Length; i += groupSize)
        {
            groups.Add(str.Substring(i, Math.Min(groupSize, str.Length - i)));
        }
        return groups;
    }

    public static bool OnlyHexInString(string test)
    {
        return HexRegex().IsMatch(test);
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"\A\b[0-9a-fA-F]+\b\Z")]
    private static partial System.Text.RegularExpressions.Regex HexRegex();
}