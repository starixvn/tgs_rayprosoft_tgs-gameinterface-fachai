namespace Tgs.GameInterface.FaChai.ViewModels;

public record GameRecordViewModel(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string Currency,
	DateTimeOffset StartDate,
	DateTimeOffset EndDate);
