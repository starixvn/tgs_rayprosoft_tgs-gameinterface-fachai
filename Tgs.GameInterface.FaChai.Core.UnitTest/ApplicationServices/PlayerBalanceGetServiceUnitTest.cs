using NSubstitute;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;
using Tgs.GameInterface.FaChai.Core.Models;

namespace Tgs.GameInterface.FaChai.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri("http://api.fcg666.net/"),
			AgentCode: "GPICN",
			AgentKey: "EOJC9q31Y7AnPb9i",
			Currency: "CNY",
			PlayerCode: "4ucmqplxb029");

		var result = new SearchMemberResult(
			IsPlayerExist: true,
			Online: false,
			Balance: 1m);
		_ = fakeGameApiClient
			 .SearchMemberAsync(
				 Arg.Is(query.ApiUrl),
				 Arg.Is(query.AgentCode),
				 Arg.Is(query.AgentKey),
				 Arg.Is(query.Currency),
				 Arg.Is(query.PlayerCode),
				 Arg.Any<CancellationToken>())
			 .Returns(result);

		var client = new PlayerBalanceGetService(
			fakeGameApiClient);

		var actual = await client.GetAsync(query);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
	}
}
