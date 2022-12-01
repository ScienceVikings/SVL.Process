using Shouldly;

namespace SVL.Process.Tests;

public class ProcessShould
{
    [Fact]
    public async Task ShouldGiveProcessOutput()
    {
        var whoami = await Process.Run("whoami");
        whoami.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task ShouldAllowArguments()
    {
        var ls = "";
        if (OperatingSystem.IsWindows())
        {
            ls=await Process.Run("dir", ".");
        }else
        {
            ls=await Process.Run("ls", ".");
        }
        ls.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task ShouldAllowEnvironmentVariables()
    {
        var myStr = "Huzzah!";
        var myVar = "";
        if (OperatingSystem.IsWindows())
        {
            myVar=await Process.Run("SET", "MY_VAR", new Dictionary<string, string>()
            {
                { "MY_VAR", myStr }
            });
        }else
        {
            myVar=await Process.Run("printenv", "MY_VAR", new Dictionary<string, string>()
            {
                { "MY_VAR", myStr }
            });
        }
        myVar.ShouldBe(myStr);
    }
    
    [Fact]
    public async Task ShouldThrowWhenItCannotFindTheFile()
    {
        await Should.ThrowAsync<Exception>(async () =>
        {
            var _ = await Process.Run("not-a-process-at-all-actually");
        });
    }
}