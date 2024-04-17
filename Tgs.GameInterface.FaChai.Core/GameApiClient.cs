using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.FaChai.Core.Extensions;
using Tgs.GameInterface.FaChai.Core.Models;

namespace Tgs.GameInterface.FaChai.Core;

internal class GameApiClient(
	ILogger<GameApiClient> logger,
	ICryptographyUtensil cryptography,
	HttpClient client) : IGameApiClient
{
	public async ValueTask AddMemberAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		string memberAccount,
		CancellationToken cancellationToken = default)
	{
		var parameters = new
		{
			MemberAccount = memberAccount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/AddMember",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(parameters),
			logPrefix: "AddMember",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() is not 0 and not 502)
		{
			throw new Exception($"This create player failed.({memberAccount})");
		}
	}

	public async ValueTask<DepositResult> DepositAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		string memberAccount,
		string trsId,
		decimal amount,
		CancellationToken cancellationToken = default)
	{
		var request = new
		{
			MemberAccount = memberAccount,
			TrsID = trsId,
			AllOut = 0,
			Points = amount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/SetPoints",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(request),
			logPrefix: "SetPoints_Deposit",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() != 0
			? throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})")
			: new DepositResult(
				BankId: jsonNode["BankID"]!.GetValue<long>(),
				TrsId: jsonNode["TrsID"]!.GetValue<string>(),
				Balance: jsonNode["AfterPoint"]!.GetValue<decimal>(),
				Amount: jsonNode["Points"]!.GetValue<decimal>(),
				Res: jsonNode.ToJsonString());
	}

	public async IAsyncEnumerable<RecordResult> GetHistoryRecordListAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		DateTimeOffset startDate,
		DateTimeOffset endDate,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new
		{
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString()
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/GetHistoryRecordList",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(request),
			logPrefix: "GetHistoryRecordList",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() is not 0 and not 227)
		{
			throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})");
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

	public async IAsyncEnumerable<RecordResult> GetRecordListAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		DateTimeOffset startDate,
		DateTimeOffset endDate,
	   [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new
		{
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString()
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/GetRecordList",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(request),
			logPrefix: "GetRecordList",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() != 0)
		{
			throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})");
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

	public async ValueTask<bool> GetSingleBillAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		string trsId,
		CancellationToken cancellationToken = default)
	{
		var parameters = new
		{
			TrsID = trsId
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/GetSingleBill",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(parameters),
			logPrefix: "GetSingleBill",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() is not 0 and not 709
			? throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})")
			: jsonNode["Result"]!.GetValue<int>() != 709;
	}

	public async ValueTask KickoutAllAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		CancellationToken cancellationToken = default)

	{
		var parameters = new { };

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/KickoutAll",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(parameters),
			logPrefix: "KickoutAll",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() is not 0)
		{
			throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})");
		}
	}

	public async ValueTask KickOutAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		string memberAccount,
		CancellationToken cancellationToken = default)
	{
		var parameters = new
		{
			MemberAccount = memberAccount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/KickOut",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(parameters),
			logPrefix: "KickOut",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["Result"]!.GetValue<int>() is not 0 and not 504 and not 500)
		{
			throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})");
		}
	}

	public async ValueTask<string> LoginAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		string memberAccount,
		int gameId,
		int languageId,
		CancellationToken cancellationToken = default)
	{
		var request = new
		{
			MemberAccount = memberAccount,
			GameID = gameId,
			LanguageID = languageId
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/Login",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(request),
			logPrefix: "Login",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() != 0
			? throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})")
			: jsonNode["Url"]!.ToString();
	}

	public async ValueTask<SearchMemberResult> SearchMemberAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		string memberAccount,
		CancellationToken cancellationToken = default)
	{
		var request = new
		{
			MemberAccount = memberAccount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/SearchMember",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(request),
			logPrefix: "SearchMember",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() != 0
			? jsonNode["Result"]!.GetValue<int>() != 500
				? throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})")
				: new SearchMemberResult(false, false, 0)
			: new SearchMemberResult(
				IsPlayerExist: true,
				Online: jsonNode["OnlineType"]!.GetValue<int>() == 1,
				Balance: jsonNode["Points"]!.GetValue<decimal>());
	}

	public async ValueTask<WithdrawalResult> WithdrawalAsync(
		Uri apiUrl,
		string agentCode,
		string agentKey,
		string currency,
		string memberAccount,
		string trsId,
		decimal amount,
		CancellationToken cancellationToken = default)
	{
		var request = new
		{
			MemberAccount = memberAccount,
			TrsID = trsId,
			AllOut = 0,
			Points = -amount
		};

		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			path: "/SetPoints",
			agentCode: agentCode,
			agentKey: agentKey,
			currency: currency,
			parameters: JsonSerializer.Serialize(request),
			logPrefix: "SetPoints_Withdrawal",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["Result"]!.GetValue<int>() != 0
			? throw new Exception($"The game provider responded with an error.({jsonNode["Result"]!.GetValue<int>()})")
			: new WithdrawalResult(
				BankId: jsonNode["BankID"]!.GetValue<long>(),
				TrsId: jsonNode["TrsID"]!.GetValue<string>(),
				Balance: jsonNode["AfterPoint"]!.GetValue<decimal>(),
				Amount: jsonNode["Points"]!.GetValue<decimal>(),
				Res: jsonNode.ToJsonString());
	}

	private async ValueTask<JsonNode> PostAsync(
		Uri apiUrl,
		string path,
		string agentCode,
		string agentKey,
		string currency,
		string parameters,
		string logPrefix,
		CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

		var sw = Stopwatch.StartNew();
		var url = new Uri(apiUrl, path);

		var request = new KeyValuePair<string, string>[]
		{
			new ("AgentCode",agentCode ),
			new ("Currency", currency),
			new ("Params",cryptography.AesEncrypt(parameters, agentKey) ),
			new ("Sign",cryptography.Md5Encrypt(parameters) )
		};

		HttpContent content = new FormUrlEncodedContent(request);

		var httpResponseMessage = await client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
		}

		var apiLogEvent = new GameApiLogEvent
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
