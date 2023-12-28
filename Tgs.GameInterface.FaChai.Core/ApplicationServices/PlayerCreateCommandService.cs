using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => client.AddMemberAsync(
                apiUrl: command.ApiUrl,
                agentCode: command.AgentCode,
                agentKey: command.AgentKey,
                currency: command.Currency,
                memberAccount: command.PlayerCode,
                cancellationToken: cancellationToken);
}
