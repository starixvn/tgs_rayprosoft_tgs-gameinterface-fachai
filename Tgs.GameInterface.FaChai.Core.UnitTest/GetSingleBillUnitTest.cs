using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.FaChai.Core.Exceptions;

namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class GetSingleBillUnitTest
{
	[Fact]
	public async Task No_Information_Found()
	{
		var apiUrl = new Uri("http://api.fcg666.net/");
		var agentCode = "GPICN";
		var agentKey = "EOJC9q31Y7AnPb9i";
		var currency = "CNY";
		var trsId = "1635165954039484416";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var source = new
		{
			TrsID = trsId
		};

		var parameters = JsonSerializer.Serialize(source);

		var parametersEncrypt = "EsLcduKWXVzPqkJbyvrijXX4O68BVQliZILhwwko+jM=";
		_ = fakeCryptographyUtensil
			 .AesEncrypt(parameters, agentKey)
			 .Returns(parametersEncrypt);

		var sign = "32f1f25df20be091b142de4c6c6f4262";
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
			Result = 709
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/GetSingleBill")
				.WithExactFormData(request)
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

		var actual = await client.GetSingleBillAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			trsId: trsId);

		Assert.False(actual);
	}

	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://api.fcg666.net/");
		var agentCode = "GPICN";
		var agentKey = "EOJC9q31Y7AnPb9i";
		var currency = "CNY";
		var trsId = "1635165954039484416";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var source = new
		{
			TrsID = trsId
		};

		var parameters = JsonSerializer.Serialize(source);

		var parametersEncrypt = "EsLcduKWXVzPqkJbyvrijXX4O68BVQliZILhwwko+jM=";
		_ = fakeCryptographyUtensil
			 .AesEncrypt(parameters, agentKey)
			 .Returns(parametersEncrypt);

		var sign = "32f1f25df20be091b142de4c6c6f4262";
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
			bankID = 14,
			trsID = trsId,
			dction = "deposit",
			points = 100,
			account = "test0310",
			status = 1,
			beforepoints = 101071,
			afterpoints = 101171,
			cdate = "2019-12-05 11:41:13",
			bdate = "2019-12-05 11:41:13",
			eventID = "62f3402670c737176421ec2c",
			currency = "CNY"
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/GetSingleBill")
				.WithExactFormData(request)
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

		var actual = await client.GetSingleBillAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			trsId: trsId);

		Assert.True(actual);
	}

	[Fact]
	public async Task Other_Errors()
	{
		var apiUrl = new Uri("http://api.fcg666.net/");
		var agentCode = "GPICN";
		var agentKey = "EOJC9q31Y7AnPb9i";
		var currency = "CNY";
		var trsId = "1635165954039484416";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var source = new
		{
			TrsID = trsId
		};

		var parameters = JsonSerializer.Serialize(source);

		var parametersEncrypt = "EsLcduKWXVzPqkJbyvrijXX4O68BVQliZILhwwko+jM=";
		_ = fakeCryptographyUtensil
			 .AesEncrypt(parameters, agentKey)
			 .Returns(parametersEncrypt);

		var sign = "32f1f25df20be091b142de4c6c6f4262";
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
			Result = 303
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/GetSingleBill")
				.WithExactFormData(request)
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(fakeLog, fakeCryptographyUtensil, fakeHttpClient);

		_ = await Assert.ThrowsAsync<GameInterfaceException>(() => client.GetSingleBillAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			trsId: trsId).AsTask());
	}
}
