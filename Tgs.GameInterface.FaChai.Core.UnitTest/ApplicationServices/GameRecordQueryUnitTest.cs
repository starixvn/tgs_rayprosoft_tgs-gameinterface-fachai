using NSubstitute;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;
using Tgs.GameInterface.FaChai.Core.Models;

namespace Tgs.GameInterface.FaChai.Core.UnitTest.ApplicationServices;

public class GameRecordQueryUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            AgentCode: "GPICN",
            AgentKey: "EOJC9q31Y7AnPb9i",
            Currency: "CNY",
            StartDate: DateTimeOffset.Parse("2023-03-13T15:45:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-03-13T16:00:00+08:00"));

        var client = new GameRecordQueryService(
            fakeGameApiClient);

        _ = await client.QueryAsync(
            query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameRecordResult>();

        _ = await fakeGameApiClient
            .Received(1)
            .GetRecordListAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.AgentCode),
            Arg.Is(query.AgentKey),
            Arg.Is(query.Currency),
            Arg.Is(query.StartDate),
            Arg.Is(query.EndDate),
            Arg.Any<CancellationToken>())
            .ToArrayAsync()
            ?? Enumerable.Empty<RecordResult>();
    }
}
