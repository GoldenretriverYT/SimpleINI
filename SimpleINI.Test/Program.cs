namespace SimpleINI.Test;

/// <summary>
/// Yes. I know this isnt using a test framework. I just wanted to make sure it worked.
/// </summary>
internal class Program
{
    private static void Main(string[] args)
    {
        NoGroupTest();
        GroupTest();
        MultiGroupTest();
        OtherTypesTest();
        CreateTest();
        CreateMultiGroupTest();
        Console.WriteLine("All tests passed!");
    }
    
    private static void NoGroupTest()
    {
        string iniData = @"val1=yes
val2=ok";

        var data = INIParser.Parse(iniData);

        if (data.TryGetGroup("val1", out var _)) throw new Exception("NoGroupTest: First value was parsed as group!");
        if (!data.TryGetValue("", "val1", out string val1)) throw new Exception("NoGroupTest: Val1 not found!");
        if (!data.TryGetValue("", "val2", out string val2)) throw new Exception("NoGroupTest: Val2 not found!");

        if (val1 != "yes") throw new Exception("NoGroupTest: Val1 was not parsed correctly!");
        if (val2 != "ok") throw new Exception("NoGroupTest: Val2 was not parsed correctly!");

        Console.WriteLine("NoGroupTest: passed");
    }

    private static void GroupTest()
    {
        string iniData = @"[MyGroup]
val1=yes
val2=ok";

        var data = INIParser.Parse(iniData);

        if (!data.TryGetGroup("MyGroup", out var _)) throw new Exception("GroupTest: Group not found!");
        if (!data.TryGetValue("MyGroup", "val1", out string val1)) throw new Exception("GroupTest: Val1 not found!");
        if (!data.TryGetValue("MyGroup", "val2", out string val2)) throw new Exception("GroupTest: Val2 not found!");

        if (val1 != "yes") throw new Exception("GroupTest: Val1 was not parsed correctly!");
        if (val2 != "ok") throw new Exception("GroupTest: Val2 was not parsed correctly!");

        Console.WriteLine("GroupTest: passed");
    }

    private static void MultiGroupTest()
    {
        string iniData = @"[MyGroup]
val1=yes

[AnotherGroup]
val1=ok";

        var data = INIParser.Parse(iniData);

        if (!data.TryGetGroup("MyGroup", out var _)) throw new Exception("MultiGroupTest: Group 1 not found!");
        if (!data.TryGetGroup("AnotherGroup", out var _)) throw new Exception("MultiGroupTest: Group 2 not found!");

        if (!data.TryGetValue("MyGroup", "val1", out string val1)) throw new Exception("MultiGroupTest: Val1 in MyGroup not found!");
        if (!data.TryGetValue("AnotherGroup", "val1", out string val2)) throw new Exception("MultiGroupTest: Val1 in AnotherGroup not found!");

        if (val1 != "yes") throw new Exception("MultiGroupTest: Val1 was not parsed correctly!");
        if (val2 != "ok") throw new Exception("MultiGroupTest: Val2 was not parsed correctly!");

        Console.WriteLine("MultiGroupTest: passed");
    }

    private static void OtherTypesTest()
    {
        string iniData = @"[TestGroup]
IsTest=true
TestRating=10";

        var data = INIParser.Parse(iniData);

        if (!data.TryGetValue("TestGroup", "IsTest", out string isTestString)) throw new Exception();
        if (!data.TryGetValue("TestGroup", "IsTest", out bool isTest)) throw new Exception();

        if (isTestString != "true") throw new Exception();
        if (!isTest) throw new Exception();

        if (!data.TryGetValue("TestGroup", "TestRating", out string ratingString)) throw new Exception();
        if (!data.TryGetValue("TestGroup", "TestRating", out int rating)) throw new Exception();

        if (ratingString != "10") throw new Exception();
        if (rating != 10) throw new Exception();
        
        Console.WriteLine("OtherTypesTest: passed");
    }

    private static void CreateTest()
    {
        var data = new INIGroupsHolder();
        data.ForceSetValue("TestGroup", "IsTest", "yup");

        var iniData = data.Stringify();
        
        if (iniData != @"[TestGroup]
IsTest=yup

") throw new Exception();

        Console.WriteLine("CreateTest: passed");
    }

    private static void CreateMultiGroupTest()
    {
        var data = new INIGroupsHolder();
        data.ForceSetValue("TestGroup", "IsTest", "yup");
        data.ForceSetValue("NotTestGroup", "IsTest", "nah");


        var iniData = data.Stringify();

        if (iniData != @"[TestGroup]
IsTest=yup

[NotTestGroup]
IsTest=nah

") throw new Exception();

        Console.WriteLine("CreateMultiGroupTest: passed");
    }
}