namespace Tgs.GameInterface.FaChai.ViewModels;

public record PlayerTableLimitViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    string Username,
    string TableLimit);
