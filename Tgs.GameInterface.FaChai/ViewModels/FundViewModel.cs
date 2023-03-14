namespace Tgs.GameInterface.FaChai.ViewModels;

public record FundViewModel(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    string PlayerCode,
    decimal Amount);
