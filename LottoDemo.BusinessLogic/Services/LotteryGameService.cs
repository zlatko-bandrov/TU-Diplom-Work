using LottoDemo.BusinessLogic.Converters;
using LottoDemo.BusinessLogic.Extensions.LotteryGame;
using LottoDemo.BusinessLogic.Services;
using LottoDemo.Common;
using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.ResultsAndWinnings;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Models;

namespace LottoDemo.BusinessLogic.Games
{
    public class LotteryGameService
    {
        public LottoUserService UserServices = new LottoUserService();

        public LottoGameUnitOfWork GameUnitOfWork = new LottoGameUnitOfWork();

        public static readonly TimeSpan DefaultDrawInterval = TimeSpan.FromMinutes(Constants.DefaultDrawInterval);

        //public static TimeSpan GetGameDrawInterval(Guid gameKey)
        //{
        //    TimeSpan timeInterval = DefaultDrawInterval;
        //    var gameSettings = Umbraco.Core.ApplicationContext.Current.Services.ContentService.GetById(gameKey);
        //    if (gameSettings != null)
        //    {
        //        var value = gameSettings.GetValue<double>("DrawingTimeInterval");
        //        timeInterval = TimeSpan.FromMinutes(value);
        //    }
        //    return timeInterval;
        //}


        public LotteryGame GetLotteryGameByKey(Guid gameKey)
        {
            var lottoGame = GameUnitOfWork.GameRepository.GetLottoGameByKey(gameKey);
            return lottoGame;
        }

        public LottoGameModel GetLottoGameModelByKey(Guid gameUniqueId, IPublishedContent contentItem = null)
        {
            var lottoGame = this.GetLotteryGameByKey(gameUniqueId);
            return lottoGame?.ToLottoGameModel(contentItem);
        }


        public LottoDrawing SaveNewDrawToDatabase(int lottoGameId, DateTime drawTime)
        {
            try
            {
                if (lottoGameId <= 0 || drawTime <= DateTime.Now)
                {
                    throw new ArgumentNullException(
                        string.Format("Saving new draw failed. One of the parameters is not valid: LottoGameId = '{0}'; DrawTime = '{1}'.",
                            lottoGameId, drawTime));
                }

                LottoDrawing newLottoDraw = new LottoDrawing
                {
                    LotteryGameID = lottoGameId,
                    DrawTime = drawTime,
                    WasCompleted = false,
                    WasExecuted = false
                };
                // Save the changes to the database
                GameUnitOfWork.DrawRepository.Add(newLottoDraw);
                GameUnitOfWork.CommitChanges();

                // Get last draw data item
                newLottoDraw = TakeTheNextDraw(lottoGameId);
                return newLottoDraw;
            }
            catch (Exception ex)
            {
                GameUnitOfWork.Rollback();
                throw ex;
            }
        }

        public int UpdateGameDraw(int lottodrawId, List<byte> ballNumbers, List<byte> bonusBallNumbers)
        {
            var lottoDraw = GameUnitOfWork.DrawRepository.Get(d => d.ID == lottodrawId);
            try
            {
                // Save the balls numbers list to the database
                byte position = 1;
                foreach (byte ballNumber in ballNumbers)
                {
                    var newLotteryBall = new LotteryBall
                    {
                        BallNumber = ballNumber,
                        IsBonusBall = false,
                        Position = position++
                    };
                    var newLottoDrawBalls = new LottoDrawingBall
                    {
                        LottoDrawingID = lottoDraw.ID,
                        LotteryBall = newLotteryBall
                    };
                    GameUnitOfWork.BallsRepository.Add(newLotteryBall);
                    GameUnitOfWork.DrawBallsRelationsRepository.Add(newLottoDrawBalls);
                }
                // Save the bonus balls numbers list to the database
                position = 1;
                foreach (byte ballNumber in bonusBallNumbers)
                {
                    var newLotteryBall = new LotteryBall
                    {
                        BallNumber = ballNumber,
                        IsBonusBall = true,
                        Position = position++
                    };
                    var newLottoDrawBalls = new LottoDrawingBall
                    {
                        LottoDrawingID = lottoDraw.ID,
                        LotteryBall = newLotteryBall
                    };
                    GameUnitOfWork.BallsRepository.Add(newLotteryBall);
                    GameUnitOfWork.DrawBallsRelationsRepository.Add(newLottoDrawBalls);
                }

                lottoDraw.WasExecuted = true;
                GameUnitOfWork.DrawRepository.Update(lottoDraw);
                GameUnitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                GameUnitOfWork.Rollback();
                throw ex;
            }

            return lottoDraw.ID;
        }

        public LottoDrawing TakeTheNextDraw(int gameId)
        {
            var lastDraw =
                this.GameUnitOfWork.DrawRepository
                    .AsQuery(g => g.LotteryGameID == gameId && g.DrawTime >= DateTime.Now)
                    .OrderByDescending(d => d.DrawTime)
                    .FirstOrDefault();
            return lastDraw;
        }

        public int? TakeTheNextDrawID(int gameId)
        {
            return TakeTheNextDraw(gameId)?.ID;
        }


        public LottoDrawing GetLastCompletedDraw(int lottoGameId)
        {
            LottoDrawing lastDraw =
                GameUnitOfWork.DrawRepository
                    .AsQuery(d => d.LotteryGameID == lottoGameId && d.WasCompleted && d.DrawTime <= DateTime.Now)
                    .OrderByDescending(d => d.DrawTime)
                    .Take(1)
                    .FirstOrDefault();
            return lastDraw;
        }

        public List<LottoDrawResult> GetAllCompletedDrawings(int gameId, int drawId = -1)
        {
            var results = new List<LottoDrawResult>();
            var lottoDraws =
                GameUnitOfWork.DrawRepository
                    .AsQuery(d => d.LotteryGameID == gameId && (drawId == -1 || d.ID == drawId) && d.WasCompleted)
                    .OrderByDescending(d => d.DrawTime);

            foreach (var lottoDraw in lottoDraws)
            {
                var allBalls = lottoDraw.LottoDrawingBalls.Select(ball => ball.LotteryBall).ToList();
                var drawResult = new LottoDrawResult
                {
                    DrawID = lottoDraw.ID,
                    DrawDate = lottoDraw.DrawTime,
                    BallsBumbers = allBalls.Where(ball => !ball.IsBonusBall).OrderBy(b => b.Position).Select(ball => ball.BallNumber).ToList(),
                    BonusBallsNumbers = allBalls.Where(ball => ball.IsBonusBall).OrderBy(b => b.Position).Select(ball => ball.BallNumber).ToList(),
                };
                drawResult.WinningTiers = lottoDraw.DrawStatistics.Select(statistics =>
                    new DrawWinningTier
                    {
                        TierName = statistics.GameWinningsTier.TierName,
                        WinningPerPerson = statistics.WinningPerPerson,
                        NumberOfWinners = statistics.WinningTickets.Count
                    }).ToList();

                results.Add(drawResult);
            }
            return results;
        }

        public Tuple<List<byte>, List<byte>> GetLastDrawNumbers(int gameId)
        {
            var result = new Tuple<List<byte>, List<byte>>(new List<byte>(), new List<byte>());
            var lastDraw = GetLastCompletedDraw(gameId);
            if (lastDraw != null)
            {
                result.Item1.AddRange(lastDraw.LottoDrawingBalls.Where(ball => !ball.LotteryBall.IsBonusBall).Select(ball => ball.LotteryBall.BallNumber));
                result.Item2.AddRange(lastDraw.LottoDrawingBalls.Where(ball => ball.LotteryBall.IsBonusBall).Select(ball => ball.LotteryBall.BallNumber));
            }
            return result;
        }


        public DateTime GetGameNextDrawTime(TimeSpan drawInterval, DateTime? nextDrawTime = null)
        {
            if (drawInterval == null || drawInterval == TimeSpan.Zero)
            {
                drawInterval = DefaultDrawInterval;
            }
            return nextDrawTime.HasValue ? nextDrawTime.Value : DateTime.Now.Add(drawInterval);
        }

        public DateTime GetGameNextDrawTime(int gameId, TimeSpan? drawInterval = null)
        {
            var drawTime = TakeNextDrawTime(gameId);
            var nextDrawTime = GetGameNextDrawTime((drawInterval.HasValue ? drawInterval.Value : TimeSpan.Zero), drawTime);
            return nextDrawTime;
        }

        public DateTime GetLastCompletedDrawTime(int lottoGameId)
        {
            LottoDrawing lastDraw = GetLastCompletedDraw(lottoGameId);
            return lastDraw != null ? lastDraw.DrawTime : DateTime.MinValue;
        }

        public DateTime GetLastCompletedDrawTime(Guid gameKey)
        {
            var game = this.GetLotteryGameByKey(gameKey);
            DateTime drawTime = GetLastCompletedDrawTime(game.ID);
            return drawTime;
        }

        public LottoDrawStatistic CalculateDrawWinnings(int currentDrawId, LottoGameSettings gameSettings, out decimal newJackpot)
        {
            var currentDraw = GameUnitOfWork.DrawRepository.Get(d => d.ID == currentDrawId);
            LottoDrawStatistic drawStatistics = CreateDrawStatistics(currentDraw.LotteryGameID);
            LottoDrawModel drawModel = LottoDrawModelConverter.AssignFrom(currentDraw);
            List<LotteryTicket> allTicketsOfTheDraw = UserServices.GetAllTicketsByDraw(currentDraw.ID);

            decimal jackpot = gameSettings.Jackpot;
            double gamePayout = gameSettings.GamePayout;
            double ticketPrice = gameSettings.TicketPrice;

            byte ballsCount = 0;
            byte bonusBallsCount = 0;

            // Calculate all winning tickets from the winning tiers
            foreach (var ticket in allTicketsOfTheDraw)
            {
                ballsCount = (byte)ticket.BallNumbers.Intersect(drawModel.BallsList).Count();
                bonusBallsCount = (byte)ticket.BonnusBallNumbers.Intersect(drawModel.BonusBallsList).Count();

                DrawStatisticItem statisticsItem = null;
                var dictionaryKey = Tuple.Create<byte, byte>(ballsCount, bonusBallsCount);
                if (drawStatistics.Table.TryGetValue(dictionaryKey, out statisticsItem))
                {
                    statisticsItem.Tickets.Add(ticket);
                }
            }

            // Calculate winnings tickets per tier
            decimal payout = (jackpot * (decimal)(gamePayout / 100));
            foreach (var entry in drawStatistics.Table)
            {
                int ticketsCount = entry.Value.Tickets.Count;
                decimal tierPrice = payout / ((decimal)entry.Value.WinningTier.TierPercent / 100);
                entry.Value.WinningTier.WinningPerPerson = ticketsCount != 0 ? (tierPrice / ticketsCount) : tierPrice;
            }

            // Remove winning tickets from the common tickets list; the rest tickets are losing
            foreach (var key in drawStatistics.Table)
            {
                List<int> listOfIDs = key.Value.Tickets.Select(d => d.ID).ToList();
                allTicketsOfTheDraw.RemoveAll(t => listOfIDs.Contains(t.ID));
            }

            // Calculate and update the new jackpot
            newJackpot = UpdateTheGameJackpot(drawModel.LottoGameID, jackpot, drawStatistics, (decimal)(allTicketsOfTheDraw.Count * ticketPrice));

            // Save all calculations to the database
            UserServices.UpdateTicketsAtLost(allTicketsOfTheDraw);
            UserServices.UpdateWinningTickets(drawStatistics);
            SaveDrawStatisticsToDatabase(drawStatistics, drawModel.ID);

            return drawStatistics;
        }


        //private void DoWinningsCalculation(
        //    LottoDrawModel executedDraw,
        //    List<LotteryTicket> lotteryTickets,
        //    LottoDrawStatistic drawStatistics,
        //    decimal jackpot,
        //    double gamePayout)
        //{
        //    byte ballsCount = 0;
        //    byte bonusBallsCount = 0;
        //    foreach (var ticket in lotteryTickets)
        //    {
        //        ballsCount = (byte)ticket.BallNumbers.Intersect(executedDraw.BallsList).Count();
        //        bonusBallsCount = (byte)ticket.BonnusBallNumbers.Intersect(executedDraw.BonusBallsList).Count();

        //        DrawStatisticItem statisticsItem = null;
        //        var dictionaryKey = Tuple.Create<byte, byte>(ballsCount, bonusBallsCount);
        //        if (drawStatistics.Table.TryGetValue(dictionaryKey, out statisticsItem))
        //        {
        //            statisticsItem.Tickets.Add(ticket);
        //        }
        //    }

        //    // Calculate winnings per tier
        //    foreach (var entry in drawStatistics.Table)
        //    {
        //        int ticketsCount = entry.Value.Tickets.Count;
        //        decimal tierPrice = jackpot / ((decimal)entry.Value.WinningTier.TierPercent / 100);
        //        entry.Value.WinningTier.WinningPerPerson = ticketsCount != 0 ? tierPrice / ticketsCount : tierPrice;
        //    }

        //    // Remove winning tickets from the common tickets list; the rest tickets are losing
        //    foreach (var key in drawStatistics.Table)
        //    {
        //        List<int> listOfIDs = key.Value.Tickets.Select(d => d.ID).ToList();
        //        lotteryTickets.RemoveAll(t => listOfIDs.Contains(t.ID));
        //    }

        //    // Save all calculations to the database
        //    UserServices.UpdateTicketsAtLost(lotteryTickets);
        //    UserServices.UpdateWinningTickets(drawStatistics);
        //    SaveDrawStatisticsToDatabase(drawStatistics, executedDraw.ID);
        //}

        // Create initial statistics data
        private LottoDrawStatistic CreateDrawStatistics(int lottoGameId)
        {
            LottoDrawStatistic statistics = new LottoDrawStatistic();
            List<WinningTier> tiers =
                GameUnitOfWork.WinningsTiersRepository
                    .AsQuery(w => w.LottoGameID == lottoGameId)
                    .OrderBy(w => w.ID)
                    .ToList()
                    .Select(t => WinningTierConverter.AssignFrom(t))
                    .ToList();

            tiers.Select(t => new DrawStatisticItem { WinningTier = t, Tickets = new List<LotteryTicket>() }).ToList()
                .ForEach(t => statistics.Table.Add(Tuple.Create<byte, byte>(t.WinningTier.BallsCount, t.WinningTier.BonnusBallCount), t));

            return statistics;
        }

        private decimal UpdateTheGameJackpot(int gameId, decimal jackpot, LottoDrawStatistic drawStatistics, decimal losingTicketsValue)
        {
            decimal newJackpot = jackpot;
            try
            {
                // First take the winnings from the jackpot
                foreach (var entry in drawStatistics.Table)
                {
                    if (entry.Value.Tickets != null && entry.Value.Tickets.Any())
                    {
                        newJackpot -= entry.Value.Tickets.Count * entry.Value.WinningTier.WinningPerPerson;
                    }
                }
                // then add the losing tickets to the jackpot
                newJackpot += losingTicketsValue;
            }
            catch (Exception ex)
            {
                GameUnitOfWork.Rollback();
                throw ex;
            }

            return newJackpot;
        }

        // Save draw statistics by winning tier in the database, if there are any winning tickets per tier
        private void SaveDrawStatisticsToDatabase(LottoDrawStatistic drawStatistics, int executedDrawId)
        {
            try
            {
                foreach (var entry in drawStatistics.Table)
                {
                    DrawStatistic newDrawStatistic = new DrawStatistic
                    {
                        LottoDrawingID = executedDrawId,
                        WinningsTierID = entry.Value.WinningTier.ID,
                        WinningPerPerson = entry.Value.WinningTier.WinningPerPerson
                    };
                    GameUnitOfWork.DrawStatisticsRepository.Add(newDrawStatistic);

                    // Add winning tickets to the repository
                    if (entry.Value.Tickets.Any())
                    {
                        var winTickets = entry.Value.Tickets.Select(ticket => new WinningTicket { LottoTicketID = ticket.ID, DrawStatistic = newDrawStatistic }).ToArray();
                        GameUnitOfWork.WinningTicketsRepository.Add(winTickets);
                    }
                }
                // Updating the current game draw  property to be IsCalculated = true
                var executedDraw = GameUnitOfWork.DrawRepository.Get(d => d.ID == executedDrawId);
                if (executedDraw != null)
                {
                    executedDraw.WasCompleted = true;
                    GameUnitOfWork.DrawRepository.Update(executedDraw);
                }
                GameUnitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                GameUnitOfWork.Rollback();
                throw ex;
            }
        }

        private DateTime? TakeNextDrawTime(int gameId)
        {
            var nextDraw = TakeTheNextDraw(gameId);
            return nextDraw?.DrawTime;
        }
    }
}