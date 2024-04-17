using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class KickOutUnitTest
{
	[Fact]
	public async Task Account_does_not_exist()
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
			Result = 500
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/KickOut")
				.WithExactFormData(request)
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

		await client.KickOutAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			memberAccount: memberAccount);
	}

	[Fact]
	public async Task Account_is_not_online()
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
			Result = 504
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/KickOut")
				.WithExactFormData(request)
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

		await client.KickOutAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			memberAccount: memberAccount);
	}

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
			Result = 0
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/KickOut")
				.WithExactFormData(request)
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

		await client.KickOutAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			memberAccount: memberAccount);
	}
}
