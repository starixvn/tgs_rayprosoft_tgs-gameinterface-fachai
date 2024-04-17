using Tgs.GameInterface.FaChai.Core.Models;

namespace Tgs.GameInterface.FaChai.Core;

public interface IGameApiClient
{
	public ValueTask AddMemberAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, CancellationToken cancellationToken = default);

	public ValueTask<DepositResult> DepositAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, string trsId, decimal amount, CancellationToken cancellationToken = default);

	public IAsyncEnumerable<RecordResult> GetHistoryRecordListAsync(Uri apiUrl, string agentCode, string agentKey, string currency, DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);

	public IAsyncEnumerable<RecordResult> GetRecordListAsync(Uri apiUrl, string agentCode, string agentKey, string currency, DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);

	public ValueTask<bool> GetSingleBillAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string trsId, CancellationToken cancellationToken = default);

	public ValueTask KickoutAllAsync(Uri apiUrl, string agentCode, string agentKey, string currency, CancellationToken cancellationToken = default);

	public ValueTask KickOutAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, CancellationToken cancellationToken = default);

	public ValueTask<string> LoginAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, int gameId, int languageId, CancellationToken cancellationToken = default);

	public ValueTask<SearchMemberResult> SearchMemberAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, CancellationToken cancellationToken = default);

	public ValueTask<WithdrawalResult> WithdrawalAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, string trsId, decimal amount, CancellationToken cancellationToken = default);
}
