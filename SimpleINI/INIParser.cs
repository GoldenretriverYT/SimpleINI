using System.Text;

namespace SimpleINI
{
    public class INIParser
    {
        public static INIGroupsHolder Parse(string iniData)
        {
            INIGroupsHolder groups = new();
            string[] lines = iniData.Split(new string[] { "\r\n", "\n\r", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string currentGroup = "";
            
            foreach(var line in lines) {
                string trimmed = line.Trim();
                if (trimmed.StartsWith(";")) continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]")) {
                    string groupName = trimmed.Substring(1, trimmed.Length - 2);
                    groups.TryAddGroup(groupName);
                    currentGroup = groupName;
                } else {
                    if (line.IndexOf("=") == -1) throw new Exception("Invalid INI: Value without equals found");

                    string[] parts = new string[] { line.Substring(0, line.IndexOf("=")), line.Substring(line.IndexOf("=") + 1)};
                    groups.ForceSetValue(currentGroup, parts[0], parts[1]);
                }
            }

            return groups;
        }
    }

    public class INIGroupsHolder
    {
        private Dictionary<string, Dictionary<string, string>> groups = new() { { "", new() } };

        public bool TryGetGroup(string name, out Dictionary<string, string> group)
        {
            if (groups.ContainsKey(name)) {
                group = groups[name];
                return true;
            }

            group = null;
            return false;
        }

        public bool TryAddGroup(string name)
        {
            if (groups.ContainsKey(name)) return false;
            groups.Add(name, new());
            return true;
        }

        public bool TryGetValue(string groupName, string valueName, out string value)
        {
            if (!groups.TryGetValue(groupName, out var group)) {
                value = "";
                return false;
            }
            
            if (!group.TryGetValue(valueName, out value)) return false;
            return true;
        }

        public bool TryGetValue(string groupName, string valueName, out int value)
        {
            value = -1;
            
            if (!groups.TryGetValue(groupName, out var group)) {
                return false;
            }

            if (!group.TryGetValue(valueName, out var stringValue)) return false;
            if (!int.TryParse(stringValue, out value)) return false;
            
            return true;
        }

        public bool TryGetValue(string groupName, string valueName, out bool value)
        {
            value = false;

            if (!groups.TryGetValue(groupName, out var group)) {
                return false;
            }

            if (!group.TryGetValue(valueName, out var stringValue)) return false;
            if (!bool.TryParse(stringValue, out value)) return false;

            return true;
        }

        public bool TrySetValue(string groupName, string valueName, string value)
        {
            if (!groups.TryGetValue(groupName, out var group)) return false;
            group[valueName] = value;
            return true;
        }
        
        public bool TrySetValue(string groupName, string valueName, object value) => TrySetValue(groupName, valueName, value.ToString());

        public void ForceSetValue(string groupName, string valueName, string value)
        {
            if (!groups.ContainsKey(groupName)) groups.Add(groupName, new());
            groups[groupName][valueName] = value;
        }

        public void ForceSetValue(string groupName, string valueName, object value) => ForceSetValue(groupName, valueName, value.ToString());

        public string Stringify()
        {
            StringBuilder sb = new("");
            
            foreach((string groupName, Dictionary<string, string> data) in groups) {
                if(groupName != "") sb.Append("[" + groupName + "]" + Environment.NewLine); // group name "" is the root
                
                foreach((string valueKey, string value) in data) {
                    sb.AppendLine(valueKey + "=" + value);
                }

                if(data.Count != 0) sb.AppendLine("");
            }

            return sb.ToString();
        }
    }
}