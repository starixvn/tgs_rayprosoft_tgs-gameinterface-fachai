using NSubstitute;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;

namespace Tgs.GameInterface.FaChai.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            AgentCode: "GPICN",
            AgentKey: "EOJC9q31Y7AnPb9i",
            Currency: "CNY",
            TrsId: "1635165246208741376;");

        _ = fakeGameApiClient
             .GetSingleBillAsync(
                 Arg.Is(query.ApiUrl),
                 Arg.Is(query.AgentCode),
                 Arg.Is(query.AgentKey),
                 Arg.Is(query.Currency),
                 Arg.Is(query.TrsId.ToString()),
                 Arg.Any<CancellationToken>())
             .Returns(true);

        var client = new FundStatusGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.True(actual);
    }
}
