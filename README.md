# SimpleINI
SimpleINI is, as the name suggests, a simple INI file parser. It is designed to be easy to use and to be fast.

## Features
* Easy-to-use API
* Supports comments

## Usage
### Parsing
```cs
var data = INIParser.Parse(myIniDataAsString);
```
### Reading values
```cs
if(!data.TryGetValue("MyGroup", "Value1", out string value1)) return;
if(!data.TryGetValue("MyGroup", "Value1", out int value1)) return;
if(!data.TryGetValue("MyGroup", "Value1", out bool value1)) return;
```
### Writing values
```cs
data.TryAddGroup("MyGroup"); // Create the group, returns true if it didn't exist
data.TrySetValue("MyGroup", "Value1", "Hello world!"); // Sets a value, returns false if the group does not exist
data.ForceSetValue("MyGroup", "Value1", "Hello World!"); // Sets a value and creates the group if required

data.ForceSetValue("MyGroup", "Value1", true); // Automatically invokes ToString()
```
### Saving
```cs
var iniDataAsString = data.Stringify();
```