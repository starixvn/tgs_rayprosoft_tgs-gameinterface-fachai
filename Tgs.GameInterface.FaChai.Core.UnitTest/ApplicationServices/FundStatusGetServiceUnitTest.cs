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
			OrderId: "1635165246208741376;");

		_ = fakeGameApiClient.GetSingleBillAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			agentCode: Arg.Is(query.AgentCode),
			agentKey: Arg.Is(query.AgentKey),
			currency: Arg.Is(query.Currency),
			trsId: Arg.Is(query.OrderId),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(true);

		var client = new FundStatusGetService(
			fakeGameApiClient);

		var actual = await client.GetAsync(
			query);

		Assert.True(actual);
	}
}
