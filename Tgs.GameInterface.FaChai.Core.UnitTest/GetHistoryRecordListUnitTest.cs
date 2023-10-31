using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.FaChai.Core.Extensions;
using Tgs.GameInterface.FaChai.Core.Models;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class GetHistoryRecordListUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "GPICN";
        var agentKey = "EOJC9q31Y7AnPb9i";
        var currency = "CNY";
        var startDate = DateTimeOffset.Parse("2023-03-13T15:45:00+08:00");
        var endDate = DateTimeOffset.Parse("2023-03-13T16:00:00+08:00");

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            StartDate = startDate.DatetimeConvertToString(),
            EndDate = endDate.DatetimeConvertToString()
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "RUpxEa8hwgTTYs/hhmfOQyZg5sldtCpGLeHZLgFK/4KrxnQ0d4iK78KAHqq9FAqVY+gEO9L091rZVr84ktPEhdJvVr1QiKIl7oyEaLocA3U=";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "102d32a0fff4664ae197078b9886b1da";
        _ = fakeCryptographyUtensil
             .Md5Encrypt(parameters)
             .Returns(sign);

        var request = new KeyValuePair<string, string>[]
        {
            new ("AgentCode",agentCode ),
            new ("Currency", currency),
            new ("Params",parametersEncrypt),
            new ("Sign",sign )
        };
        var response = new
        {
            Result = 0,
            Records = new[] {
                new {
                    bet= 0.5,
                    prize= 0.0,
                    winlose= -0.5,
                    before= 100.0,
                    after = 99.5,
                    jptax= 0.0,
                    jppoints= 0.0,
                    recordID= "640ed4b2eb1a1e6f43f4e7e4",
                    account= "4ucmqplxb029",
                    gameID= 22018,
                    gametype= 2,
                    jpmode= 0,
                    bdate= "2023-03-13 03:45:54",
                    isBuyFeature= false},
                new {
                    bet= 0.5,
                    prize= 0.2,
                    winlose= -0.3,
                    before= 99.5,
                    after = 99.2,
                    jptax= 0.0,
                    jppoints= 0.0,
                    recordID= "640ed4b5eb1a1e6f43f4e7e5",
                    account= "4ucmqplxb029",
                    gameID= 22018,
                    gametype= 2,
                    jpmode= 0,
                    bdate= "2023-03-13 03:45:57",
                    isBuyFeature= false},
                new {
                    bet= 0.5,
                    prize= 0.0,
                    winlose= -0.5,
                    before= 99.2,
                    after = 98.7,
                    jptax= 0.0,
                    jppoints= 0.0,
                    recordID= "640ed4b9eb1a1e6f43f4e7e7",
                    account= "4ucmqplxb029",
                    gameID= 22018,
                    gametype= 2,
                    jpmode= 0,
                    bdate= "2023-03-13 03:46:01",
                    isBuyFeature= false}
            }
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/GetHistoryRecordList")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        var actual = await client.GetHistoryRecordListAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            currency: currency,
            startDate: startDate,
            endDate: endDate)
            .ToArrayAsync()
            ?? Enumerable.Empty<RecordResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }

    [Fact]
    public async Task Result_Is_227()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "GPICN";
        var agentKey = "EOJC9q31Y7AnPb9i";
        var currency = "CNY";
        var startDate = DateTimeOffset.Parse("2023-03-13T15:45:00+08:00");
        var endDate = DateTimeOffset.Parse("2023-03-13T16:00:00+08:00");

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            StartDate = startDate.DatetimeConvertToString(),
            EndDate = endDate.DatetimeConvertToString()
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "RUpxEa8hwgTTYs/hhmfOQyZg5sldtCpGLeHZLgFK/4KrxnQ0d4iK78KAHqq9FAqVY+gEO9L091rZVr84ktPEhdJvVr1QiKIl7oyEaLocA3U=";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "102d32a0fff4664ae197078b9886b1da";
        _ = fakeCryptographyUtensil
             .Md5Encrypt(parameters)
             .Returns(sign);

        var request = new KeyValuePair<string, string>[]
        {
            new ("AgentCode",agentCode ),
            new ("Currency", currency),
            new ("Params",parametersEncrypt),
            new ("Sign",sign )
        };
        var response = new
        {
            Result = 227
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/GetHistoryRecordList")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        var actual = await client.GetHistoryRecordListAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            currency: currency,
            startDate: startDate,
            endDate: endDate)
            .ToArrayAsync()
            ?? Enumerable.Empty<RecordResult>();

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }
}
