using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class DepositUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "GPICN";
        var agentKey = "EOJC9q31Y7AnPb9i";
        var currency = "CNY";
        var memberAccount = "4ucmqplxb029";
        var trsId = "1635165246208741376";
        var amount = 1m;

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            MemberAccount = memberAccount,
            TrsID = trsId,
            AllOut = 0,
            Points = amount
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "VwZ4kWvs5kMwKR//dPe5YGxsprU2fH6sFxgAH7pp7R3CtOz8vLkk2a2/MPdbdYDFFDWH7iqAUh/OPkoap49KPJhr397bBxlvWG9d8dVO+HkNBREiDKBuNpd8aMyUT7qw";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "f2bca43183fdf7aea37b1116a9ee5c15";
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
            BankID = 183698,
            TrsID = trsId,
            AfterPoint = 2m,
            Points = 1m
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/SetPoints")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        var actual = await client.DepositAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            currency: currency,
            memberAccount: memberAccount,
            trsId: trsId,
            amount: amount);

        Assert.NotNull(actual);
        Assert.Equal(trsId, actual.TrsId);
    }
}
