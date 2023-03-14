namespace Tgs.GameInterface.FaChai.Core.Models;

public record SearchMemberResult(
    bool IsPlayerExist,
    bool Online,
    decimal Balance);
