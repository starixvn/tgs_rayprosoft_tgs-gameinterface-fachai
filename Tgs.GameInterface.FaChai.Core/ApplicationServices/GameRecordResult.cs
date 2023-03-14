namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

public record GameRecordResult(
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
