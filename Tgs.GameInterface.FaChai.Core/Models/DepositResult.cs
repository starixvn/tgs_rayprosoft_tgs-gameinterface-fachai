namespace Tgs.GameInterface.FaChai.Core.Models;

public record DepositResult(
	long BankId,
	string TrsId,
	decimal Balance,
	decimal Amount,
	string Resp);
