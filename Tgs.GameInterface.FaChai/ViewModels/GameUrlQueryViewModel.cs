namespace Tgs.GameInterface.FaChai.ViewModels;

public record GameUrlQueryViewModel(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string Currency,
	string PlayerCode,
	int GameID,
	int LanguageID);
