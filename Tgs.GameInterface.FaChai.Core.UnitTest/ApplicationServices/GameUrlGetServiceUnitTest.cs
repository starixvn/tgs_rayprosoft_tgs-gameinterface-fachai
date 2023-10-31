using NSubstitute;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;

namespace Tgs.GameInterface.FaChai.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            AgentCode: "GPICN",
            AgentKey: "EOJC9q31Y7AnPb9i",
            Currency: "CNY",
            PlayerCode: "4ucmqplxb029",
            GameId: 22018,
            LanguageId: 2);

        var client = new GameUrlGetService(
            fakeGameApiClient);

        _ = await client.GetAsync(query);

        _ = await fakeGameApiClient
            .Received(1)
            .LoginAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.AgentCode),
            Arg.Is(query.AgentKey),
            Arg.Is(query.Currency),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.GameId),
            Arg.Is(query.LanguageId),
            Arg.Any<CancellationToken>());
    }
}
