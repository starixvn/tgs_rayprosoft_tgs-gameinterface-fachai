using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
        => _client
        .GetSingleBillAsync(
            apiUrl: query.ApiUrl,
            agentCode: query.AgentCode,
            agentKey: query.AgentKey,
            currency: query.Currency,
            trsId: query.TrsId,
            cancellationToken: cancellationToken);
}
