using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class GameUrlGetService(IGameApiClient client) : IGetService<GameUrlQuery, string>
{
    public ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => client.LoginAsync(
            apiUrl: query.ApiUrl,
            agentCode: query.AgentCode,
            agentKey: query.AgentKey,
            currency: query.Currency,
            memberAccount: query.PlayerCode,
            gameId: query.GameId,
            languageId: query.LanguageId,
            cancellationToken: cancellationToken);
}
