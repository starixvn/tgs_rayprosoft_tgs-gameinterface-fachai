using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;
using Tgs.GameInterface.FaChai.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.Controllers
{
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
        ///             "amount": 1
        ///         }
        /// </remarks>
        [HttpPost("fund/in", Name = "FundIn")]
        public async ValueTask<FundResultViewModel> FundInAsync(
            [FromServices] IGetService<FundInQuery, FundResult> service,
            FundViewModel source)
        {
            var query = new FundInQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                Currency: source.Currency,
                PlayerCode: source.PlayerCode,
                Amount: source.Amount);

            var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return new FundResultViewModel(
                TrsId: result.TrsId,
                BankId: result.BankId,
                Balance: result.Balance,
                Amount: result.Amount,
                Res: result.Res);
        }

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
        ///             "amount": 1
        ///         }
        /// </remarks>
        [HttpPost("fund/out", Name = "FundOut")]
        public async ValueTask<FundResultViewModel> FundOutAsync(
            [FromServices] IGetService<FundOutQuery, FundResult> service,
            FundViewModel source)
        {
            var query = new FundOutQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                Currency: source.Currency,
                PlayerCode: source.PlayerCode,
                Amount: source.Amount);

            var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return new FundResultViewModel(
                TrsId: result.TrsId,
                BankId: result.BankId,
                Balance: result.Balance,
                Amount: result.Amount,
                Res: result.Res);
        }

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
        ///             "trsId": "1635165954039484416"
        ///         }
        /// </remarks>
        [HttpPost("fund/Status", Name = "FundStatus")]
        public ValueTask<bool> FundStatusAsync(
            [FromServices] IGetService<FundStatusQuery, bool> service,
            FundStatusViewModel source)
        {
            var query = new FundStatusQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                Currency: source.Currency,
                TrsId: source.TrsId);

            return service.GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted);
        }

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
        public async IAsyncEnumerable<GameRecordResultViewModel> GameRecordAsync(
            [FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
            GameRecordViewModel source)
        {
            var query = new GameRecordQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                Currency: source.Currency,
                StartDate: source.StartDate,
                EndDate: source.EndDate);

            var results = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordResult>();

            if (!results.Any())
            {
                yield break;
            }

            foreach (var result in results)
            {
                yield return new GameRecordResultViewModel(
                    ExternalRoundId: result.ExternalRoundId,
                    ExternalGameId: result.ExternalGameId,
                    PlayerCode: result.PlayerCode,
                    ExternalCreatedOn: result.ExternalCreatedOn,
                    ExternalClosedOn: result.ExternalClosedOn,
                    Bet: result.Bet,
                    Settle: result.Settle,
                    WinLoss: result.WinLoss,
                    Jppoints: result.Jppoints,
                    AdditionalData: result.AdditionalData);
            }
        }

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
        public async IAsyncEnumerable<GameRecordResultViewModel> GameRecordHistoryAsync(
            [FromServices] IQueryService<GameRecordHistoryQuery, GameRecordResult> service,
            GameRecordViewModel source)
        {
            var query = new GameRecordHistoryQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                Currency: source.Currency,
                StartDate: source.StartDate,
                EndDate: source.EndDate);

            var results = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordResult>();

            if (!results.Any())
            {
                yield break;
            }

            foreach (var result in results)
            {
                yield return new GameRecordResultViewModel(
                    ExternalRoundId: result.ExternalRoundId,
                    ExternalGameId: result.ExternalGameId,
                    PlayerCode: result.PlayerCode,
                    ExternalCreatedOn: result.ExternalCreatedOn,
                    ExternalClosedOn: result.ExternalClosedOn,
                    Bet: result.Bet,
                    Settle: result.Settle,
                    WinLoss: result.WinLoss,
                    Jppoints: result.Jppoints,
                    AdditionalData: result.AdditionalData);
            }
        }

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
            GameUrlQueryViewModel source)
        {
            var query = new GameUrlQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                Currency: source.Currency,
                PlayerCode: source.PlayerCode,
                GameId: source.GameID,
                LanguageId: source.LanguageID);

            return service
             .GetAsync(
                 query: query,
                 cancellationToken: HttpContext.RequestAborted);
        }

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
        public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
            [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
            PlayerBalanceViewModel source)
        {
            var query = new PlayerBalanceQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                Currency: source.Currency,
                PlayerCode: source.PlayerCode);

            var result = await service
                    .GetAsync(
                        query: query,
                        cancellationToken: HttpContext.RequestAborted)
                    .ConfigureAwait(false);

            return new PlayerBalanceResultViewModel(
                result.IsPlayerExist,
                result.Balance);
        }

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
            PlayerCreateViewModel source)
        {
            var command = new PlayerCreateCommand(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                Currency: source.Currency,
                PlayerCode: source.PlayerCode);
            return service.ExecuteAsync(
                    command: command,
                    cancellationToken: HttpContext.RequestAborted);
        }

        /// <summary>
        /// Players the kick asynchronous.
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
        public ValueTask PlayerKickAsync(
            [FromServices] ICommandService<PlayerLogoutCommand> service,
            PlayerLogoutViewModel source)
            => service
                .ExecuteAsync(
                    command: new PlayerLogoutCommand(
                        ApiUrl: source.ApiUrl,
                        AgentCode: source.AgentCode,
                        AgentKey: source.AgentKey,
                        Currency: source.Currency,
                        PlayerCode: source.PlayerCode),
                    cancellationToken: HttpContext.RequestAborted);
    }
}
