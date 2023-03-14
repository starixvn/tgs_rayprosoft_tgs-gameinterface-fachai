namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

public record PlayerLogoutCommand(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    string PlayerCode);
