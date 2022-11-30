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
}