using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Controllers;

[Route("fc")]
[ApiController]
public class FaChaiController : ControllerBase
{
	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "playerCode": "4ucmqplxb029",
	///             "orderId": "1635165954039484416",
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public ValueTask<FundResult> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		FundInQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "playerCode": "4ucmqplxb029",
	///             "orderId": "1635165246208741376",
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public ValueTask<FundResult> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		FundOutQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "orderId": "1635165954039484416"
	///         }
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "startDate": "2023-03-13T15:45:00+08:00",
	///             "endDate": "2023-03-13T16:00:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/record")]
	public IAsyncEnumerable<GameRecordResult> GameRecordAsync(
		[FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
		GameRecordQuery source)
		=> service.QueryAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record history asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "startDate": "2023-03-13T15:45:00+08:00",
	///             "endDate": "2023-03-13T16:00:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/record/history")]
	public IAsyncEnumerable<GameRecordResult> GameRecordHistoryAsync(
		[FromServices] IQueryService<GameRecordHistoryQuery, GameRecordResult> service,
		GameRecordHistoryQuery source)
		=> service.QueryAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "playerCode": "4ucmqplxb029",
	///             "gameID": 22018,
	///             "languageID": 2
	///         }
	/// </remarks>
	[HttpPost("gameurl")]
	public ValueTask<string> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, string> service,
		GameUrlQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public ValueTask<PlayerBalanceResult> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		PlayerBalanceQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the create asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/create", Name = "PlayerCreate")]
	public ValueTask PlayerCreateAsync(
		[FromServices] ICommandService<PlayerCreateCommand> service,
		PlayerCreateCommand source)
		=> service.ExecuteAsync(
			command: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the online.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "http://api.fcg666.net",
	///             "agentCode": "GPICN",
	///             "agentKey": "EOJC9q31Y7AnPb9i",
	///             "currency": "CNY",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/online", Name = "PlayerOnline")]
	public ValueTask<bool> PlayerOnline(
		[FromServices] IGetService<PlayerOnlineQuery, bool> service,
		[FromBody] PlayerOnlineQuery query)
		=> service.GetAsync(query);

	/// <summary>
	/// Players the logout asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.dg0.co/",
	///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
	///             "agentName": "DGTE0105IH",
	///             "memberId": 94030972
	///         }
	/// </remarks>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutCommand source)
		=> service.ExecuteAsync(
			command: source,
			cancellationToken: HttpContext.RequestAborted);
}
