using Tgs.GameInterface.FaChai.Protos;

namespace Tgs.GameInterface.FaChai.Client;

public interface IGameInterfaceFaChaiClient
{
    ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string orderId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string agentCode, string agentKey, string currency, DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameRecordInfo> GameRecordHistoryAsync(Uri apiUrl, string agentCode, string agentKey, string currency, DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);

    ValueTask<string> GameUrlAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string playerCode, int gameId, int languageId, CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string playerCode, CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string playerCode, CancellationToken cancellationToken = default);

    ValueTask PlayerLogoutAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string playerCode, CancellationToken cancellationToken = default);
}
