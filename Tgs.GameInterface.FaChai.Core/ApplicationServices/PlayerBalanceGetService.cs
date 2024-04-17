using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client
			.SearchMemberAsync(
				apiUrl: query.ApiUrl,
				agentCode: query.AgentCode,
				agentKey: query.AgentKey,
				currency: query.Currency,
				memberAccount: query.PlayerCode,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceResult(
			result.IsPlayerExist,
			result.Balance);
	}
}
