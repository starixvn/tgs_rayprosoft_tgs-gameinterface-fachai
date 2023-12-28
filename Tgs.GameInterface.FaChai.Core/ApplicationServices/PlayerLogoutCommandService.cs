using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class PlayerLogoutCommandService(IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
    public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
        => client.KickOutAsync(
                apiUrl: command.ApiUrl,
                agentCode: command.AgentCode,
                agentKey: command.AgentKey,
                currency: command.Currency,
                memberAccount: command.PlayerCode,
                cancellationToken: cancellationToken);
}
