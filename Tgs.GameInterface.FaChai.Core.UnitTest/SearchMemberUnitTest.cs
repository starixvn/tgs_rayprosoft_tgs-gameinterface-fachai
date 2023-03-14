using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class SearchMemberUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "PG";
        var agentKey = "iQ1mFWdzTh5zPO9r";
        var currency = "CNY";
        var memberAccount = "test0310";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            MemberAccount = memberAccount
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "/lrqY3SMqUTDU8Q4I6mhUhKzW82nofYGZ3G68NF9MKo=";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "d68c0e614db6b9d79ba95dd547a11a88";
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
            OnlineType = 0,
            Points = 1,
            LanguageID = 2
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/SearchMember")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        var actual = await client
            .SearchMemberAsync(
                apiUrl,
                agentCode,
                agentKey,
                currency,
                memberAccount)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
        Assert.False(actual.Online);
        Assert.Equal(1.0m, actual.Balance);
    }

    [Fact]
    public async Task This_Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "PG";
        var agentKey = "iQ1mFWdzTh5zPO9r";
        var currency = "CNY";
        var memberAccount = "test0310";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            MemberAccount = memberAccount
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "/lrqY3SMqUTDU8Q4I6mhUg1MJwecqMt1FxciO4XYuUag1fx0sjGJmpFa3wHC384L";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "422e1936ec134b526b322a1c1374a207";
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
            Result = 500
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/SearchMember")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        var actual = await client
            .SearchMemberAsync(
                apiUrl,
                agentCode,
                agentKey,
                currency,
                memberAccount)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.IsPlayerExist);
    }

    [Fact]
    public async Task This_Player_Is_Online()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "PG";
        var agentKey = "iQ1mFWdzTh5zPO9r";
        var currency = "CNY";
        var memberAccount = "test0310";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            MemberAccount = memberAccount
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "/lrqY3SMqUTDU8Q4I6mhUhKzW82nofYGZ3G68NF9MKo=";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "d68c0e614db6b9d79ba95dd547a11a88";
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
            OnlineType = 1,
            Points = 1,
            LanguageID = 2
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/SearchMember")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        var actual = await client
            .SearchMemberAsync(
                apiUrl,
                agentCode,
                agentKey,
                currency,
                memberAccount)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
        Assert.True(actual.Online);
        Assert.Equal(1.0m, actual.Balance);
    }
}
