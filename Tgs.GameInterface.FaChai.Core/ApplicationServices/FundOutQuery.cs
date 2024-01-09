namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    string PlayerCode,
    string OrderId,
    decimal Amount);
