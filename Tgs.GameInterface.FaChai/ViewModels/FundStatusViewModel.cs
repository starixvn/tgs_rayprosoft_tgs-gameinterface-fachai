namespace Tgs.GameInterface.FaChai.ViewModels;

public record FundStatusViewModel(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    string TrsId);
