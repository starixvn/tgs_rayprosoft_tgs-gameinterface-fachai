namespace Tgs.GameInterface.FaChai.ViewModels;

public record PlayerBalanceViewModel(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    string PlayerCode);
