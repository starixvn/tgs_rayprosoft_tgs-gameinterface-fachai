using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class AddMemberUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "GPICN";
        var agentKey = "EOJC9q31Y7AnPb9i";
        var currency = "CNY";
        var memberAccount = "4ucmqplxb029";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            MemberAccount = memberAccount
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "VwZ4kWvs5kMwKR//dPe5YNXc4dUt91rceOiISZkVT8S24ADG5mZfYJBGcsirFqt7";
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
            Result = 0
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/AddMember")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        await client
            .AddMemberAsync(
                apiUrl,
                agentCode,
                agentKey,
                currency,
                memberAccount)
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Player_Already_Exists()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "GPICN";
        var agentKey = "EOJC9q31Y7AnPb9i";
        var currency = "CNY";
        var memberAccount = "4ucmqplxb029";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            MemberAccount = memberAccount
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "VwZ4kWvs5kMwKR//dPe5YNXc4dUt91rceOiISZkVT8S24ADG5mZfYJBGcsirFqt7";
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
            Result = 502
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/AddMember")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        await client
            .AddMemberAsync(
                apiUrl,
                agentCode,
                agentKey,
                currency,
                memberAccount)
            .ConfigureAwait(false);
    }
}
