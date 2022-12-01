# SVL.Process
This is a library that will let you run a process as an asyncronous task and await its output as a string.

```csharp
var whoami = await Process.Run("whoami");
```

It's that easy!  

You can also use arguments and environment variables!
```csharp
var ls = await Process.Run("ls", ".");

var myVar = await Process.Run("printenv", "MY_VAR", new Dictionary<string, string>()
{
    { "MY_VAR", myStr }
});
```
If you think I can add any cool or useful features to it, please add an issue.