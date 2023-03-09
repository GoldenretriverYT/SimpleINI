namespace SimpleINI;

public class LightParser
{
    public static Dictionary<string, Dictionary<string, string>> Parse(string iniData)
    {
        Dictionary<string, Dictionary<string, string>> groups = new() { { "", new() } };
        string[] lines = iniData.Split(new string[] { "\r\n", "\n\r", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        string currentGroup = "";

        foreach (var line in lines) {
            string trimmed = line.Trim();
            if (trimmed.StartsWith(";")) continue;

            if (trimmed.StartsWith("[") && trimmed.EndsWith("]")) {
                string groupName = trimmed.Substring(1, trimmed.Length - 2);
                groups.Add(groupName, new());
                currentGroup = groupName;
            } else {
                if (line.IndexOf("=") == -1) throw new Exception("Invalid INI!");

                string[] parts = new string[] { line.Substring(0, line.IndexOf("=")), line.Substring(line.IndexOf("=") + 1) };
                groups[currentGroup][parts[0]] = parts[1];
            }
        }

        return groups;
    }
}