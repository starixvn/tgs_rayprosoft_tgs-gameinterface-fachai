namespace Tgs.GameInterface.FaChai.Core.ApplicationServices;

public record FundResult(
	string TrsId,
	long BankId,
	decimal Balance,
	decimal Amount,
	string Resp);
