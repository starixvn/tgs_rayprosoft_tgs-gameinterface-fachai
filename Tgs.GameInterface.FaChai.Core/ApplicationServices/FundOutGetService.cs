using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class FundOutGetService(
    ILogger<FundOutGetService> logger,
    ISnowflakeUtensil snowflakeUtensil,
    IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var trsId = snowflakeUtensil.NextId();
        try
        {
            var result = await client
             .DepositAsync(
                 query.ApiUrl,
                 query.AgentCode,
                 query.AgentKey,
                 query.Currency,
                 query.PlayerCode,
                 trsId.ToString(),
                 query.Amount,
                 cancellationToken)
             .ConfigureAwait(false);

            return new FundResult(
                TrsId: trsId.ToString(),
                BankId: result.BankId,
                Balance: result.Balance,
                Amount: result.Amount,
                Res: result.Res);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred with this fund.");
            throw;
        }
    }
}
