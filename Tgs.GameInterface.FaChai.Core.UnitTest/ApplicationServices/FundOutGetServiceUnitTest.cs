using NSubstitute;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;
using Tgs.GameInterface.FaChai.Core.Models;

namespace Tgs.GameInterface.FaChai.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new FundOutQuery(
			ApiUrl: new Uri("http://api.fcg666.net/"),
			AgentCode: "GPICN",
			AgentKey: "EOJC9q31Y7AnPb9i",
			Currency: "CNY",
			PlayerCode: "4ucmqplxb029",
			OrderId: "1635165246208741376",
			Amount: 1m);

		var result = new DepositResult(
			BankId: 123456,
			TrsId: query.OrderId,
			Balance: 1,
			Amount: 1m,
			Resp: "{}");
		_ = fakeGameApiClient
			 .DepositAsync(
				 Arg.Is(query.ApiUrl),
				 Arg.Is(query.AgentCode),
				 Arg.Is(query.AgentKey),
				 Arg.Is(query.Currency),
				 Arg.Is(query.PlayerCode),
				 Arg.Is(query.OrderId),
				 Arg.Is(query.Amount),
				 Arg.Any<CancellationToken>())
			 .Returns(result);

		var client = new FundOutGetService(
			fakeGameApiClient);

		var actual = await client.GetAsync(
			query);

		Assert.NotNull(actual);
		Assert.Equal(query.OrderId, actual.TrsId);
	}
}
