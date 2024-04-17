namespace Tgs.GameInterface.FaChai.ViewModels;

public record FundResultViewModel(
	string TrsId,
	long BankId,
	decimal Balance,
	decimal Amount,
	string Res);
