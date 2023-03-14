using System.Runtime.CompilerServices;
using Tgs.GameInterface.FaChai.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class GameRecordQueryService : IQueryService<GameRecordQuery, GameRecordResult>
{
    private readonly IGameApiClient _client;

    public GameRecordQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordResult> QueryAsync(
        GameRecordQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var results = await _client
            .GetRecordListAsync(
                apiUrl: query.ApiUrl,
                agentCode: query.AgentCode,
                agentKey: query.AgentKey,
                currency: query.Currency,
                startDate: query.StartDate,
                endDate: query.EndDate,
                cancellationToken: cancellationToken)
            .ToArrayAsync(
                cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<RecordResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new GameRecordResult(
                ExternalRoundId: result.ExternalRoundId,
                ExternalGameId: result.ExternalGameId,
                PlayerCode: result.PlayerCode,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalCreatedOn,
                Bet: result.Bet,
                Settle: result.Settle,
                WinLoss: result.WinLoss,
                Jppoints: result.Jppoints,
                AdditionalData: result.AdditionalData);
        }
    }
}
