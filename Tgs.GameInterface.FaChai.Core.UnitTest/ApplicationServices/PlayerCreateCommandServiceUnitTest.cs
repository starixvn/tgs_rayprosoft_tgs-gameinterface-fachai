using NSubstitute;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;

namespace Tgs.GameInterface.FaChai.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var command = new PlayerCreateCommand(
			ApiUrl: new Uri("http://api.fcg666.net/"),
			AgentCode: "GPICN",
			AgentKey: "EOJC9q31Y7AnPb9i",
			Currency: "CNY",
			PlayerCode: "4ucmqplxb029");

		var client = new PlayerCreateCommandService(
			fakeGameApiClient);

		await client.ExecuteAsync(command);

		await fakeGameApiClient
		   .Received(1)
		   .AddMemberAsync(
			Arg.Is(command.ApiUrl),
			Arg.Is(command.AgentCode),
			Arg.Is(command.AgentKey),
			Arg.Is(command.Currency),
			Arg.Is(command.PlayerCode),
			Arg.Any<CancellationToken>());
	}
}
