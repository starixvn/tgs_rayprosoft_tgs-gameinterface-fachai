using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.FaChai.Core.ApplicationServices;
using Tgs.GameInterface.FaChai.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.FaChai.GrpcServices
{
    public class GameInterfaceFaChaiGrpcService : GameInterfaceFaChai.GameInterfaceFaChaiBase
    {
        private readonly IGetService<FundInQuery, FundResult> _fundInService;
        private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
        private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
        private readonly IQueryService<GameRecordHistoryQuery, GameRecordResult> _gameRecordHistoryService;
        private readonly IQueryService<GameRecordQuery, GameRecordResult> _gameRecordService;
        private readonly IGetService<GameUrlQuery, string> _gameUrlService;
        private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
        private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
        private readonly ICommandService<PlayerLogoutCommand> _playerLogoutService;

        public GameInterfaceFaChaiGrpcService(
            IGetService<FundInQuery, FundResult> fundInService,
            IGetService<FundOutQuery, FundResult> fundOutService,
            IGetService<FundStatusQuery, bool> fundStatusService,
            IQueryService<GameRecordHistoryQuery, GameRecordResult> gameRecordHistoryService,
            IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
            IGetService<GameUrlQuery, string> gameUrlService,
            IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
            ICommandService<PlayerCreateCommand> playerCreateService,
            ICommandService<PlayerLogoutCommand> PlayerLogoutService)
        {
            _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
            _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
            _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
            _gameRecordHistoryService = gameRecordHistoryService ?? throw new ArgumentNullException(nameof(gameRecordHistoryService));
            _gameRecordService = gameRecordService ?? throw new ArgumentNullException(nameof(gameRecordService));
            _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
            _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
            _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
            _playerLogoutService = PlayerLogoutService ?? throw new ArgumentNullException(nameof(PlayerLogoutService));
        }

        public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
        {
            var query = new FundInQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                PlayerCode: request.PlayerCode,
                Amount: request.Amount);

            var result = await _fundInService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                TrsId = result.TrsId,
                BankId = result.BankId,
                Balance = result.Balance,
                Amount = result.Amount,
                Res = result.Res
            };
        }

        public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
        {
            var query = new FundOutQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                PlayerCode: request.PlayerCode,
                Amount: request.Amount);

            var result = await _fundOutService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                TrsId = result.TrsId,
                BankId = result.BankId,
                Balance = result.Balance,
                Amount = result.Amount,
                Res = result.Res
            };
        }

        public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
        {
            var query = new FundStatusQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                TrsId: request.TrsId);

            var result = await _fundStatusService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new BaseReply
            {
                State = result
            };
        }

        public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
        {
            var query = new GameRecordQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                StartDate: request.StartDate.ToDateTimeOffset(),
                EndDate: request.EndDate.ToDateTimeOffset());

            var results = await _gameRecordService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordResult>();

            var reply = new GameRecordReply();

            if (!results.Any())
            {
                foreach (var result in results)
                {
                    reply.Items.Add(new GameRecord
                    {
                        ExternalRoundId = result.ExternalRoundId,
                        ExternalGameId = result.ExternalGameId,
                        PlayerCode = result.PlayerCode,
                        ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                        Bet = result.Bet,
                        Settle = result.Settle,
                        WinLoss = result.WinLoss,
                        Jppoints = result.Jppoints,
                        AdditionalData = result.AdditionalData
                    });
                }
            }

            return reply;
        }

        public override async Task<GameRecordReply> GameRecordHistory(GameRecordRequest request, ServerCallContext context)
        {
            var query = new GameRecordHistoryQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                StartDate: request.StartDate.ToDateTimeOffset(),
                EndDate: request.EndDate.ToDateTimeOffset());

            var results = await _gameRecordHistoryService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordResult>();

            var reply = new GameRecordReply();

            if (!results.Any())
            {
                foreach (var result in results)
                {
                    reply.Items.Add(new GameRecord
                    {
                        ExternalRoundId = result.ExternalRoundId,
                        ExternalGameId = result.ExternalGameId,
                        PlayerCode = result.PlayerCode,
                        ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                        Bet = result.Bet,
                        Settle = result.Settle,
                        WinLoss = result.WinLoss,
                        Jppoints = result.Jppoints,
                        AdditionalData = result.AdditionalData
                    });
                }
            }

            return reply;
        }

        public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
        {
            var query = new GameUrlQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                PlayerCode: request.PlayerCode,
                GameId: request.GameId,
                LanguageId: request.LanguageId);

            var result = await _gameUrlService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new GameUrlReply
            {
                GameUrl = result
            };
        }

        public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
        {
            var query = new PlayerBalanceQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                PlayerCode: request.PlayerCode);

            var result = await _playerBalanceService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new PlayerBalanceReply
            {
                IsPlayerExist = result.IsPlayerExist,
                Balance = result.Balance
            };
        }

        public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
        {
            var command = new PlayerCreateCommand(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                PlayerCode: request.PlayerCode);

            await _playerCreateService
                .ExecuteAsync(
                    command: command,
                   cancellationToken: context.CancellationToken)
               .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }

        public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
        {
            var command = new PlayerLogoutCommand(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                Currency: request.Currency,
                PlayerCode: request.PlayerCode);

            await _playerLogoutService
                .ExecuteAsync(
                    command: command,
                   cancellationToken: context.CancellationToken)
               .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }
    }
}
