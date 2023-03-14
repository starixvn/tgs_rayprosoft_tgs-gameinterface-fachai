using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, string>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => _client.LoginAsync(
            apiUrl: query.ApiUrl,
            agentCode: query.AgentCode,
            agentKey: query.AgentKey,
            currency: query.Currency,
            memberAccount: query.PlayerCode,
            gameId: query.GameId,
            languageId: query.LanguageId,
            cancellationToken: cancellationToken);
}
