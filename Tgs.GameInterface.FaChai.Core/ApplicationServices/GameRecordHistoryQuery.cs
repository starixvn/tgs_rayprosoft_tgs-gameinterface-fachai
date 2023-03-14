namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

public record GameRecordHistoryQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);
