using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class PlayerOnlineGetService(IGameApiClient client) : IGetService<PlayerOnlineQuery, bool>
{
	public async ValueTask<bool> GetAsync(PlayerOnlineQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.SearchMemberAsync(
			apiUrl: query.ApiUrl,
			agentCode: query.AgentCode,
			agentKey: query.AgentKey,
			currency: query.Currency,
			memberAccount: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return result.Online;
	}
}
