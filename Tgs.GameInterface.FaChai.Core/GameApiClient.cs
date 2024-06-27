using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.FaChai.Core.Exceptions;
using Tgs.GameInterface.FaChai.Core.Extensions;
using Tgs.GameInterface.FaChai.Core.Models;

namespace Tgs.GameInterface.FaChai.Core;

internal class GameApiClient(
	ILogger<GameApiClient> logger,
	ICryptographyUtensil cryptography,
	HttpClient client) : IGameApiClient
{
	public async ValueTask AddMemberAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["MemberAccount"] = memberAccount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/AddMember",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(AddMemberAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() is not 0 and not 502)
		{
			throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>());
		}
	}

	public async ValueTask<DepositResult> DepositAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, string trsId, decimal amount, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["MemberAccount"] = memberAccount,
			["TrsID"] = trsId,
			["AllOut"] = 0,
			["Points"] = amount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/SetPoints",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(DepositAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() != 0
			? throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>())
			: new DepositResult(
				BankId: jsonNode["BankID"]!.GetValue<long>(),
				TrsId: jsonNode["TrsID"]!.GetValue<string>(),
				Balance: jsonNode["AfterPoint"]!.GetValue<decimal>(),
				Amount: jsonNode["Points"]!.GetValue<decimal>(),
				Resp: jsonNode.ToJsonString());
	}

	public async IAsyncEnumerable<RecordResult> GetHistoryRecordListAsync(Uri apiUrl, string agentCode, string agentKey, string currency, DateTimeOffset startDate, DateTimeOffset endDate, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["StartDate"] = startDate.DatetimeConvertToString(),
			["EndDate"] = endDate.DatetimeConvertToString()
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/GetHistoryRecordList",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(GetHistoryRecordListAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() is not 0 and not 227)
		{
			throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>());
		}

		if (jsonNode["Result"]!.GetValue<int>() == 227)
		{
			yield break;
		}

		var records = jsonNode["Records"]?.AsArray();

		if (records is null || records.Count == 0)
		{
			yield break;
		}

		foreach (var record in records)
		{
			if (record is null)
			{
				continue;
			}

			yield return new RecordResult(
				ExternalRoundId: record["recordID"]!.GetValue<string>(),
				ExternalGameId: record["gameID"]!.ToString(),
				PlayerCode: record["account"]!.GetValue<string>(),
				ExternalCreatedOn: record["bdate"]!.GetValue<string>().StringConvertToDatetime(),
				Bet: record["bet"]!.GetValue<decimal>(),
				Settle: record["prize"]!.GetValue<decimal>(),
				WinLoss: record["winlose"]!.GetValue<decimal>(),
				Jppoints: record["jppoints"]!.GetValue<decimal>(),
				AdditionalData: record.ToJsonString());
		}
	}

	public async IAsyncEnumerable<string> GetOnlineMemberAsync(Uri apiUrl, string agentCode, string agentKey, string currency, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/GetOnlineMember",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: new JsonObject(),
			logPrefix: nameof(SearchMemberAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() != 0)
		{
			throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>());
		}

		var memberList = jsonNode["MemberList"]!.AsArray();

		if (memberList is null || memberList.Count == 0)
		{
			yield break;
		}

		foreach (var member in memberList)
		{
			if (member is null)
			{
				continue;
			}

			var values = JsonSerializer.Deserialize<Dictionary<string, long>>(member.ToJsonString());

			if (values is null)
			{
				continue;
			}

			foreach (var value in values)
			{
				yield return value.Key;
			}
		}
	}

	public async IAsyncEnumerable<RecordResult> GetRecordListAsync(Uri apiUrl, string agentCode, string agentKey, string currency, DateTimeOffset startDate, DateTimeOffset endDate, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["StartDate"] = startDate.DatetimeConvertToString(),
			["EndDate"] = endDate.DatetimeConvertToString()
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/GetRecordList",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(GetRecordListAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() != 0)
		{
			throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>());
		}

		var records = jsonNode["Records"]?.AsArray();

		if (records is null || records.Count == 0)
		{
			yield break;
		}

		foreach (var record in records)
		{
			if (record is null)
			{
				continue;
			}

			yield return new RecordResult(
				ExternalRoundId: record["recordID"]!.GetValue<string>(),
				ExternalGameId: record["gameID"]!.ToString(),
				PlayerCode: record["account"]!.GetValue<string>(),
				ExternalCreatedOn: record["bdate"]!.GetValue<string>().StringConvertToDatetime(),
				Bet: record["bet"]!.GetValue<decimal>(),
				Settle: record["prize"]!.GetValue<decimal>(),
				WinLoss: record["winlose"]!.GetValue<decimal>(),
				Jppoints: record["jppoints"]!.GetValue<decimal>(),
				AdditionalData: record.ToJsonString());
		}
	}

	public async ValueTask<bool> GetSingleBillAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string trsId, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["TrsID"] = trsId
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/GetSingleBill",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(GetSingleBillAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() is not 0 and not 709
			? throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>())
			: jsonNode["Result"]!.GetValue<int>() != 709;
	}

	public async ValueTask KickoutAllAsync(Uri apiUrl, string agentCode, string agentKey, string currency, CancellationToken cancellationToken = default)
	{

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/KickoutAll",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: new JsonObject { },
			logPrefix: nameof(KickoutAllAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() is not 0)
		{
			throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>());
		}
	}

	public async ValueTask KickOutAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["MemberAccount"] = memberAccount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/KickOut",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(KickOutAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() is not 0 and not 504 and not 500)
		{
			throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>());
		}
	}

	public async ValueTask<string> LoginAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, int gameId, int languageId, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["MemberAccount"] = memberAccount,
			["GameID"] = gameId,
			["LanguageID"] = languageId
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/Login",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(LoginAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() != 0
			? throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>())
			: jsonNode["Url"]!.ToString();
	}

	public async ValueTask<SearchMemberResult> SearchMemberAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["MemberAccount"] = memberAccount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/SearchMember",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(SearchMemberAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() != 0
			? jsonNode["Result"]!.GetValue<int>() != 500
				? throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>())
				: new SearchMemberResult(false, false, 0)
			: new SearchMemberResult(
				IsPlayerExist: true,
				Online: jsonNode["OnlineType"]!.GetValue<int>() == 1,
				Balance: jsonNode["Points"]!.GetValue<decimal>());
	}

	public async ValueTask<WithdrawalResult> WithdrawalAsync(Uri apiUrl, string agentCode, string agentKey, string currency, string memberAccount, string trsId, decimal amount, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["MemberAccount"] = memberAccount,
			["TrsID"] = trsId,
			["AllOut"] = 0,
			["Points"] = -amount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/SetPoints",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: parameters,
			logPrefix: nameof(WithdrawalAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() != 0
			? throw new GameInterfaceException(jsonNode["Result"]!.GetValue<int>())
			: new WithdrawalResult(
				BankId: jsonNode["BankID"]!.GetValue<long>(),
				TrsId: jsonNode["TrsID"]!.GetValue<string>(),
				Balance: jsonNode["AfterPoint"]!.GetValue<decimal>(),
				Amount: jsonNode["Points"]!.GetValue<decimal>(),
				Resp: jsonNode.ToJsonString());
	}

	private async ValueTask<JsonNode> PostAsync(Uri apiUrl, string path, string agentCode, string agentKey, string currency, JsonNode parameters, string logPrefix, CancellationToken cancellationToken = default)
	{

		var url = new Uri(apiUrl, path);

		var request = new KeyValuePair<string, string>[]
		{
			new ("AgentCode",agentCode ),
			new ("Currency", currency),
			new ("Params",cryptography.AesEncrypt(parameters.ToJsonString(), agentKey) ),
			new ("Sign",cryptography.Md5Encrypt(parameters.ToJsonString()) )
		};

		HttpContent content = new FormUrlEncodedContent(request);

		var sw = Stopwatch.StartNew();

		var httpResponseMessage = await client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
		}

		var apiLogEvent = new
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return JsonNode.Parse(responseMessage)!;
	}
}
