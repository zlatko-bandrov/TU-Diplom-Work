using LottoDemo.BusinessLogic.Converters;
using LottoDemo.BusinessLogic.Extensions.LotteryGame;
using LottoDemo.BusinessLogic.Services;
using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Models;

namespace LottoDemo.BusinessLogic.Games
{
    public class LotteryGameService
    {
        public LottoUserService UserServices = new LottoUserService();

        public LottoGameUnitOfWork GameUnitOfWork = new LottoGameUnitOfWork();

        public LotteryGame GetLotteryGameByKey(Guid gameKey)
        {
            var lottoGame = GameUnitOfWork.GameRepository.GetLottoGameByKey(gameKey);
            return lottoGame;
        }

        public LottoGameModel GetLottoGameModelByKey(Guid gameUniqueId, IPublishedContent contentItem = null)
        {
            var lottoGame = this.GetLotteryGameByKey(gameUniqueId);
            return lottoGame != null ? lottoGame.ToLottoGameModel(contentItem) : null;
        }

        public LottoDrawing CreateNewLottoDraw(DateTime drawingDate, Guid lottoGameKey, List<byte> ballNumbers = null, List<byte> bonusBallsNumbers = null)
        {
            LottoDrawing newDrawing = null;
            try
            {
                var lottoGame = this.GameUnitOfWork.GameRepository.Get(g => g.GameKey.Equals(lottoGameKey));
                newDrawing = new LottoDrawing
                {
                    LotteryGameID = lottoGame.ID,
                    IsCalculated = false,
                    DrawTime = drawingDate
                };

                GameUnitOfWork.DrawRepository.Add(newDrawing);
                GameUnitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                GameUnitOfWork.Rollback();
                throw ex;
            }

            return newDrawing;
        }

        public void SaveNewDrawToDatabase(int lottoGameId, DateTime drawTime, out int nextDrawId)
        {
            try
            {
                if (lottoGameId <= 0 || drawTime <= DateTime.Now)
                {
                    throw new ArgumentNullException(
                        string.Format("Saving new draw failed. One of the parameters is not valid: LottoGameId = '{0}'; DrawTime = '{1}'.",
                            lottoGameId, drawTime));
                }
                var newLottoDraw = new LottoDrawing
                {
                    LotteryGameID = lottoGameId,
                    DrawTime = drawTime,
                    IsCalculated = false
                };
                // Save the changes to the database
                GameUnitOfWork.DrawRepository.Add(newLottoDraw);
                GameUnitOfWork.CommitChanges();

                // Get last draw data item
                var nextDraw =
                    GameUnitOfWork.DrawRepository
                        .AsQuery(d => d.LotteryGameID == lottoGameId && d.IsCalculated == false)
                        .OrderByDescending(d => d.ID)
                        .Take(1)
                        .FirstOrDefault();
                nextDrawId = nextDraw != null ? nextDraw.ID : -1;
            }
            catch (Exception ex)
            {
                GameUnitOfWork.Rollback();
                throw ex;
            }
        }

        public LottoDrawing UpdateLottoDraw(int lottoDrawId, List<byte> ballNumbers, List<byte> bonusBallNumbers)
        {
            var lottoDraw = GameUnitOfWork.DrawRepository.Get(d => d.ID == lottoDrawId);
            if (lottoDraw != null)
            {
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
                            LottoDrawingID = lottoDrawId,
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
                            LottoDrawingID = lottoDrawId,
                            LotteryBall = newLotteryBall
                        };
                        GameUnitOfWork.BallsRepository.Add(newLotteryBall);
                        GameUnitOfWork.DrawBallsRelationsRepository.Add(newLottoDrawBalls);
                    }
                    GameUnitOfWork.CommitChanges();
                }
                catch (Exception ex)
                {
                    GameUnitOfWork.Rollback();
                    throw ex;
                }

            }
            else
            {
                throw new NullReferenceException(
                    string.Format("{0}: LottoDraw with ID={1} wasn't found.",
                        MethodBase.GetCurrentMethod().Name, lottoDrawId));
            }

            return lottoDraw;
        }

        public DateTime GetTheNextDrawTime(TimeSpan drawInterval, LottoDrawing draw = null)
        {
            if (drawInterval == null || drawInterval == TimeSpan.Zero)
            {
                drawInterval = TimeSpan.FromMinutes(5);
            }
            return draw != null ? draw.DrawTime : DateTime.Now.Add(drawInterval);
        }

        public LottoDrawing TakeTheNextDraw(int gameId)
        {
            var lastDraw =
                this.GameUnitOfWork.DrawRepository
                    .AsQuery(g => g.LotteryGameID == gameId && DateTime.Now <= g.DrawTime && g.IsCalculated == false)
                    .OrderByDescending(d => d.DrawTime)
                    .FirstOrDefault();
            return lastDraw;
        }

        public DateTime GetNextAvailableDraw(Guid gameKey)
        {
            var game = this.GetLotteryGameByKey(gameKey);
            var nextDraw = this.TakeTheNextDraw(game.ID);
            return nextDraw != null ? nextDraw.DrawTime : DateTime.MaxValue;
        }

        public DateTime GetLastCompletedDrawTime(Guid gameKey)
        {
            var game = this.GetLotteryGameByKey(gameKey);
            DateTime drawTime = GetLastCompletedDrawTime(game.ID);
            return drawTime;
        }

        public DateTime GetLastCompletedDrawTime(int lottoGameId)
        {
            LottoDrawing lastDraw =
                GameUnitOfWork.DrawRepository
                    .AsQuery(d => d.LotteryGameID == lottoGameId && d.IsCalculated == true && d.DrawTime <= DateTime.Now)
                    .OrderByDescending(d => d.ID)
                    .FirstOrDefault();
            if (lastDraw != null)
            {
                return lastDraw.DrawTime;
            }
            return DateTime.MinValue;
        }

        public DateTime GetPreviousDrawTime(int lottoGameId, int lottoDrawId)
        {
            LottoDrawing lastDraw =
                GameUnitOfWork.DrawRepository
                    .AsQuery(d => d.LotteryGameID == lottoGameId && d.ID < lottoDrawId && d.IsCalculated && d.DrawTime <= DateTime.Now)
                    .OrderByDescending(d => d.ID)
                    .Take(1)
                    .FirstOrDefault();
            if (lastDraw != null)
            {
                return lastDraw.DrawTime;
            }
            return DateTime.MinValue;
        }

        public LottoDrawStatistic DoExecutedDrawWinnings(LottoDrawing executedDraw, decimal jackpot)
        {
            DateTime previousDrawTime = GetPreviousDrawTime(executedDraw.LotteryGameID, executedDraw.ID);
            LottoDrawStatistic drawStatistics = this.CreateDrawStatistics(executedDraw.LotteryGameID);
            LottoDrawModel drawModel = LottoDrawModelConverter.AssignFrom(executedDraw);

            var allTicketsByDraw = UserServices.GetAllTicketsByDraw(executedDraw.LotteryGameID, previousDrawTime, executedDraw.DrawTime);
            this.DoWinningsCalculation(drawModel, allTicketsByDraw, drawStatistics, jackpot);

            return drawStatistics;
        }

        private void DoWinningsCalculation(LottoDrawModel executedDraw, List<LotteryTicket> lotteryTickets, LottoDrawStatistic drawStatistics, decimal jackpot)
        {
            byte ballsCount = 0;
            byte bonusBallsCount = 0;
            foreach (var ticket in lotteryTickets)
            {
                ballsCount = (byte)ticket.BallNumbers.Intersect(executedDraw.BallsList).Count();
                bonusBallsCount = (byte)ticket.BonnusBallNumbers.Intersect(executedDraw.BonusBallsList).Count();

                DrawStatisticItem statisticsItem = null;
                var dictionaryKey = Tuple.Create<byte, byte>(ballsCount, bonusBallsCount);
                if (drawStatistics.Table.TryGetValue(dictionaryKey, out statisticsItem))
                {
                    statisticsItem.Tickets.Add(ticket);
                }
            }

            // Calculate winnings per tier
            CalculatePricesPerWinning(drawStatistics, jackpot);

            // Remove winning tickets from the looser tickets
            this.UpdateTicketList(lotteryTickets, drawStatistics);

            // Save all calculations to the database
            UserServices.UpdateTicketsAtLost(lotteryTickets);
            UserServices.UpdateWinningTickets(drawStatistics);
            SaveDrawStatisticsToDatabase(drawStatistics, executedDraw.ID);
        }

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

        // Remove the winning tickets from the list
        private void UpdateTicketList(List<LotteryTicket> tickets, LottoDrawStatistic statistics)
        {
            foreach (var key in statistics.Table)
            {
                List<int> listOfIDs = key.Value.Tickets.Select(d => d.ID).ToList();
                tickets.RemoveAll(t => listOfIDs.Contains(t.ID));
            }
        }

        // Save draw statistics by winning tier in the database, if there are any winning tickets per tier
        private void SaveDrawStatisticsToDatabase(LottoDrawStatistic drawStatistics, int executedDrawId)
        {
            try
            {
                foreach (var entry in drawStatistics.Table)
                {
                    if (entry.Value.Tickets.Any())
                    {
                        DrawStatistic newDrawStatistic = new DrawStatistic
                        {
                            LottoDrawingID = executedDrawId,
                            WinningsTierID = entry.Value.WinningTier.ID,
                            WinningPerPerson = entry.Value.WinningTier.WinningPerPerson
                        };
                        GameUnitOfWork.DrawStatisticsRepository.Add(newDrawStatistic);

                        foreach (var ticket in entry.Value.Tickets)
                        {
                            WinningTicket winTicket = new WinningTicket
                            {
                                LottoTicketID = ticket.ID,
                                DrawStatistic = newDrawStatistic
                            };
                            GameUnitOfWork.WinningTicketsRepository.Add(winTicket);
                        }
                    }
                }

                var executedDraw = GameUnitOfWork.DrawRepository.Get(d => d.ID == executedDrawId);
                if (executedDraw != null)
                {
                    executedDraw.IsCalculated = true;
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

        private void CalculatePricesPerWinning(LottoDrawStatistic drawStatistics, decimal jackpot)
        {
            foreach (var entry in drawStatistics.Table)
            {
                int ticketsCount = entry.Value.Tickets.Count;
                decimal tierPrice = jackpot / ((decimal)entry.Value.WinningTier.TierPercent / 100);
                entry.Value.WinningTier.WinningPerPerson = ticketsCount != 0 ? tierPrice / ticketsCount : tierPrice;
            }
        }
    }
}