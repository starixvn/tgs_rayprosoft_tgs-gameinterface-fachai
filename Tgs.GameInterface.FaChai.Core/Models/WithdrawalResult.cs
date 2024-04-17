namespace Tgs.GameInterface.FaChai.Core.Models;

public record WithdrawalResult(
	long BankId,
	string TrsId,
	decimal Balance,
	decimal Amount,
	string Res);
