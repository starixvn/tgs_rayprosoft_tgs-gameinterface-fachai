namespace Tgs.GameInterface.FaChai.Core.Models;

public record RecordResult(
    string ExternalRoundId,
    string ExternalGameId,
    string PlayerCode,
    DateTimeOffset ExternalCreatedOn,
    decimal Bet,
    decimal Settle,
    decimal WinLoss,
    decimal Jppoints,
    string AdditionalData);
