namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    string OrderId);
