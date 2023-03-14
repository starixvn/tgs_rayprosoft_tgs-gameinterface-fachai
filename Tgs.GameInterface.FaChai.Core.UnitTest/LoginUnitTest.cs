using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class LoginUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://api.fcg666.net/");
        var agentCode = "PG";
        var agentKey = "iQ1mFWdzTh5zPO9r";
        var currency = "CNY";
        var memberAccount = "test0310";
        var gameID = 21003;
        var languageID = 2;

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var source = new
        {
            MemberAccount = memberAccount,
            GameID = gameID,
            LanguageID = languageID
        };

        var parameters = JsonSerializer.Serialize(source);

        var parametersEncrypt = "/lrqY3SMqUTDU8Q4I6mhUlDdrG791b2WDG4wRtDRMhtvAjAjGNdJqo6ehQfAhhbVKnuPfp5DKcvhNDkF4CGcsw==";
        _ = fakeCryptographyUtensil
             .AesEncrypt(parameters, agentKey)
             .Returns(parametersEncrypt);

        var sign = "7703a8dce82f296df710be1bad33f091";
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
            Url = "https://web.fcg666.net/index?Params=eyJpdiI6IlEzelZJYlk1TmlsVnM4QTRlQUZ5Nmc9PSIsInZhbHVlIjoiV1h3YUFmOC9JNHEyWE9GL1ZqVlpNMEhTOGlMd09ZU1VXMWcxMmtiTjNONThDRFRsRlVubDd0WGxpeTJwcUZIeUVpNFRlNk44QjhyUkpLNGdpRjFLd3ZPS1ppYWRZNU5SWktPTjVta1RQSWdGdVZkT3hKa1JGaUNEMUw2cUo2Q0YiLCJtYWMiOiJjMzUwNWI4MDZlYTQ5ODFiMjIwODMwMGE0NzZjZjIyYmYyMDRkOWQ0YTYwMzI5ZjQxZTBhMzFmNTU2NTAwOTJlIn0=&languageID=2"
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Login")
                .WithExactFormData(request)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

        var atual = await client
            .LoginAsync(
                apiUrl,
                agentCode,
                agentKey,
                currency,
                memberAccount,
                gameID,
                languageID)
            .ConfigureAwait(false);

        Assert.NotNull(atual);
        //Assert.True(atual.IsPlayerExist);
        //Assert.False(atual.Online);
        //Assert.Equal(1.0m, atual.Balance);
    }
}
