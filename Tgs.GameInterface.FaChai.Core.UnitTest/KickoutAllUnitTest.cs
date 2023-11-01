using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class KickoutAllUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "PG";
        var agentKey = "iQ1mFWdzTh5zPO9r";
        var currency = "CNY";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new { };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "PAPG68m0yGPa0kYB9nndPQ==";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "67c1890df55a3fda9bfa59ed880bdd0c";
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
            Result = 0
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/KickoutAll")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        await client.KickoutAllAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            currency: currency);
    }

    [Fact]
    public async Task Account_does_not_exist()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "PG";
        var agentKey = "iQ1mFWdzTh5zPO9r";
        var currency = "CNY";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new { };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "PAPG68m0yGPa0kYB9nndPQ==";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "67c1890df55a3fda9bfa59ed880bdd0c";
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
        _ = handler.Expect(HttpMethod.Post, "/KickoutAll")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        await client.KickoutAllAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            currency: currency);
    }

    [Fact]
    public async Task Account_is_not_online()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "PG";
        var agentKey = "iQ1mFWdzTh5zPO9r";
        var currency = "CNY";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new { };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "PAPG68m0yGPa0kYB9nndPQ==";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "67c1890df55a3fda9bfa59ed880bdd0c";
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
            Result = 504
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/KickoutAll")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        await client.KickoutAllAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            currency: currency);
    }
}
