namespace Tgs.GameInterface.FaChai.ViewModels;

public record PlayerCreateViewModel(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string Currency,
	string PlayerCode);
