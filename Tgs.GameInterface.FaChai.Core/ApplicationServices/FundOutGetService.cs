using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;
    private readonly ILogger<FundOutGetService> _logger;
    private readonly ISnowflakeUtensil _snowflakeUtensil;

    public FundOutGetService(
        ILogger<FundOutGetService> logger,
        ISnowflakeUtensil snowflakeUtensil,
        IGameApiClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _snowflakeUtensil = snowflakeUtensil ?? throw new ArgumentNullException(nameof(snowflakeUtensil));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var trsId = _snowflakeUtensil.NextId();
        try
        {
            var result = await _client
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
            _logger.LogError(ex, "An exception occurred with this fund.");
            throw;
        }
    }
}
