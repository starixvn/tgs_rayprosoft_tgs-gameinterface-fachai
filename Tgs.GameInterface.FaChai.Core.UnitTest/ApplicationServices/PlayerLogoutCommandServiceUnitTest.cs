using NSubstitute;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;

namespace Tgs.GameInterface.FaChai.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerLogoutCommand(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            AgentCode: "GPICN",
            AgentKey: "EOJC9q31Y7AnPb9i",
            Currency: "CNY",
            PlayerCode: "4ucmqplxb029");

        var client = new PlayerLogoutCommandService(
            fakeGameApiClient);

        await client.ExecuteAsync(command);

        await fakeGameApiClient
           .Received(1)
           .KickOutAsync(
            Arg.Is(command.ApiUrl),
            Arg.Is(command.AgentCode),
            Arg.Is(command.AgentKey),
            Arg.Is(command.Currency),
            Arg.Is(command.PlayerCode),
            Arg.Any<CancellationToken>());
    }
}
