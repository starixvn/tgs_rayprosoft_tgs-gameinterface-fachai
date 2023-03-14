namespace Tgs.GameInterface.FaChai.ViewModels;

public record GameHistoryMarkViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    IEnumerable<string> ExternalRoundIds);
