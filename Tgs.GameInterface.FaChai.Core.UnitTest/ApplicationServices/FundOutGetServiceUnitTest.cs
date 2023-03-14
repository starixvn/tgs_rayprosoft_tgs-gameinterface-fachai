using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;
using Tgs.GameInterface.FaChai.Core.Models;

namespace Tgs.GameInterface.FaChai.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLog = NullLogger<FundOutGetService>.Instance;
        var fakeSnowflakeUtensil = Substitute.For<ISnowflakeUtensil>();
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundOutQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            AgentCode: "GPICN",
            AgentKey: "EOJC9q31Y7AnPb9i",
            Currency: "CNY",
            PlayerCode: "4ucmqplxb029",
            Amount: 1m);

        var trsId = 1635165246208741376;
        _ = fakeSnowflakeUtensil
            .NextId()
            .Returns(trsId);

        var result = new DepositResult(
            BankId: 123456,
            TrsId: trsId.ToString(),
            Balance: 1,
            Amount: 1m,
            Res: "{}");
        _ = fakeGameApiClient
             .DepositAsync(
                 Arg.Is(query.ApiUrl),
                 Arg.Is(query.AgentCode),
                 Arg.Is(query.AgentKey),
                 Arg.Is(query.Currency),
                 Arg.Is(query.PlayerCode),
                 Arg.Is(trsId.ToString()),
                 Arg.Is(query.Amount),
                 Arg.Any<CancellationToken>())
             .Returns(result);

        var client = new FundOutGetService(
            fakeLog,
            fakeSnowflakeUtensil,
            fakeGameApiClient);

        var atual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(atual);
        Assert.Equal(trsId.ToString(), atual.TrsId);
    }
}
