namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

public record PlayerBalanceQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    string PlayerCode);
