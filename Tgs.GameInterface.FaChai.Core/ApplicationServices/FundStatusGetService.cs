using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
		=> client.GetSingleBillAsync(
			apiUrl: query.ApiUrl,
			agentCode: query.AgentCode,
			agentKey: query.AgentKey,
			currency: query.Currency,
			trsId: query.OrderId,
			cancellationToken: cancellationToken);
}
