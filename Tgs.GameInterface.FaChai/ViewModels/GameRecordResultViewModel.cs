namespace Tgs.GameInterface.FaChai.ViewModels;

public record GameRecordResultViewModel(
	string ExternalRoundId,
	string ExternalGameId,
	string PlayerCode,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal Settle,
	decimal WinLoss,
	decimal Jppoints,
	string AdditionalData);
