using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class GetOnlineMemberUnitTest
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

		var parameters = new JsonObject().ToJsonString();

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
		var response = new JsonObject
		{
			["Result"] = 0,
			["Count"] = 2,
			["MemberList"] = new JsonArray
			{
				new JsonObject{
					["demo001"] = 27001
				},
				new JsonObject{
					["demo002"] = 21003
				}
			}
		};
		var content = response.ToJsonString();

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/GetOnlineMember")
				.WithExactFormData(request)
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

		var actual = await client.GetOnlineMemberAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency)
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal("demo001", actual[0]);
		Assert.Equal("demo002", actual[1]);
	}

	[Fact]
	public async Task No_Data()
	{
		var apiUrl = new Uri("http://api.fcg666.net/");
		var agentCode = "PG";
		var agentKey = "iQ1mFWdzTh5zPO9r";
		var currency = "CNY";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var parameters = new JsonObject().ToJsonString();

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
		var response = new JsonObject
		{
			["Result"] = 0,
			["Count"] = 0,
			["MemberList"] = new JsonArray { }
		};
		var content = response.ToJsonString();

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/GetOnlineMember")
				.WithExactFormData(request)
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

		var actual = await client.GetOnlineMemberAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency)
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Empty(actual);
	}
}
