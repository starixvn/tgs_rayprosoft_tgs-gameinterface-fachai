namespace Tgs.GameInterface.FaChai.ViewModels;

public record PlayerLogoutViewModel(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    string PlayerCode);
