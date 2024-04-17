using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class FundOutGetService(
	IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		if (query.OrderId.Length is < 0 or > 30)
		{
			throw new ArgumentOutOfRangeException($"The order identification code cannot be longer than 30 characters.({query.OrderId.Length})");
		}

		var result = await client.DepositAsync(
			apiUrl: query.ApiUrl,
			agentCode: query.AgentCode,
			agentKey: query.AgentKey,
			currency: query.Currency,
			memberAccount: query.PlayerCode,
			trsId: query.OrderId,
			amount: query.Amount,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new FundResult(
			TrsId: query.OrderId,
			BankId: result.BankId,
			Balance: result.Balance,
			Amount: result.Amount,
			Res: result.Res);
	}
}
