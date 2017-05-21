using LottoDemo.BusinessLogic.Converters;
using LottoDemo.BusinessLogic.Games;
using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.Cart;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace LottoDemo.BusinessLogic.Services
{
    public class LottoUserService
    {
        private decimal InitialUserBalance
        {
            get
            {
                decimal temp = 0;
                if (Decimal.TryParse(ConfigurationManager.AppSettings["InitialUserBalance"], out temp))
                {
                    return temp;
                }

                return 1000;
            }
        }

        public UserUnitOfWork UnitOfWork = new UserUnitOfWork();

        public bool CreateNewUser(string umbracoUsername)
        {
            try
            {
                var newUser = this.GetNewUser(umbracoUsername);
                UnitOfWork.UserRepository.Add(newUser);
                UnitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                Log.Error(typeof(LottoUserService), string.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                UnitOfWork.Rollback();
                return false;
            }
            return true;
        }

        public User FindUser(string username)
        {
            var user = this.UnitOfWork.UserRepository.Get(u => u.Username.Equals(username));
            return user;
        }

        public void AddNewLotteryTickets(string username, AddToCartModel tickets)
        {
            var loggedUser = this.FindUser(username);
            if (loggedUser != null)
            {
                try
                {
                    LottoTicket ticket = null;
                    var gameUnitOfWork = new LottoGameUnitOfWork();
                    var lottoGameKey = gameUnitOfWork.GameRepository.Get(g => g.ID == tickets.LotteryGameId).GameKey;
                    decimal gameTicketPrice = this.GetGameTicketPrice(lottoGameKey);

                    // Add ticket one
                    if (tickets.DrawBallsTicket1.Count() > 0 && tickets.BonusBallsTicket1.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.LotteryGameId, loggedUser.ID, tickets.DrawBallsTicket1, tickets.BonusBallsTicket1);
                        this.UnitOfWork.TicketsRepository.Add(ticket);

                        // Update user money balance, it is one ticket so the change in the balance is one ticket price
                        loggedUser.Balance.Value += (-1) * gameTicketPrice;
                    }

                    // Add the second ticket
                    if (tickets.DrawBallsTicket2.Count() > 0 && tickets.BonusBallsTicket2.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.LotteryGameId, loggedUser.ID, tickets.DrawBallsTicket2, tickets.BonusBallsTicket2);
                        this.UnitOfWork.TicketsRepository.Add(ticket);

                        // Update user money balance, it is one ticket so the change in the balance is one ticket price
                        loggedUser.Balance.Value += (-1) * gameTicketPrice;
                    }

                    // Add third ticket
                    if (tickets.DrawBallsTicket3.Count() > 0 && tickets.BonusBallsTicket3.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.LotteryGameId, loggedUser.ID, tickets.DrawBallsTicket3, tickets.BonusBallsTicket3);
                        this.UnitOfWork.TicketsRepository.Add(ticket);

                        // Update user money balance, it is one ticket so the change in the balance is one ticket price
                        loggedUser.Balance.Value += (-1) * gameTicketPrice;
                    }
                    UnitOfWork.BalanceRepository.Update(loggedUser.Balance);

                    // Save the changes to the database
                    UnitOfWork.CommitChanges();
                }
                catch (Exception ex)
                {
                    UnitOfWork.Rollback();
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("loggedUser", "There is no one logged in currently.");
            }
        }

        public List<LotteryGameCartItem> GetAllCartTickets(string username, LottoGameModel lotteryGame, string lotteryGameName)
        {
            var cartItemsList = new List<LotteryGameCartItem>();
            var loggedUser = this.FindUser(username);
            if (loggedUser != null)
            {
                LotteryGameCartItem gameCartItem = new LotteryGameCartItem
                {
                    LottoGameName = lotteryGameName,
                    TicketPrice = lotteryGame.GameSettings.TicketPrice
                };

                var lotteryTickets =
                    loggedUser.LottoTickets
                        .Where(t => t.LotteryGameID == lotteryGame.Id
                            && !t.IsCalculated
                            && t.InputTime > lotteryGame.PreviousDrawingTime
                            && t.InputTime < lotteryGame.NextDrawingTime)
                        .ToList();

                // Return if there are no tickets
                if (lotteryTickets == null || !lotteryTickets.Any())
                {
                    return cartItemsList;
                }

                foreach (var ticket in lotteryTickets)
                {
                    var ballsNumbers = ticket.LottoTicketBalls.Select(r => r.LotteryBall).Where(b => !b.IsBonusBall).OrderBy(b => b.Position).Select(b => b.BallNumber).ToList();
                    var bonusBallsNumbers = ticket.LottoTicketBalls.Select(r => r.LotteryBall).Where(b => b.IsBonusBall).OrderBy(b => b.Position).Select(b => b.BallNumber).ToList();

                    gameCartItem.Tickets.Add(new LotteryTicketCartItem
                    {
                        GameDrawTime = lotteryGame.NextDrawingTime,
                        BallsNumbers = ballsNumbers,
                        BonusBallsNumbers = bonusBallsNumbers,
                        LottoTicketId = ticket.ID
                    });
                }

                cartItemsList.Add(gameCartItem);
            }

            return cartItemsList;
        }

        public List<ShoppingCartItem> GetAllCartTickets(string username, out decimal userBalance, out decimal totalCartValue)
        {
            var cartItems = new List<ShoppingCartItem>();
            var loggedUser = this.FindUser(username);

            totalCartValue = 0;
            userBalance = 0;

            if (loggedUser == null)
            {
                return cartItems;
            }

            // Get user balance value 
            userBalance = loggedUser.Balance.Value;

            if (loggedUser.LottoTickets != null && loggedUser.LottoTickets.Where(t => !t.IsCalculated).Any())
            {
                var allUserGames = loggedUser.LottoTickets.Where(t => !t.IsCalculated).GroupBy(t => t.LotteryGameID).ToList();
                var gameService = new LotteryGameService();
                var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);

                foreach (var g in allUserGames)
                {
                    Guid gameKey = g.FirstOrDefault().LotteryGame.GameKey;
                    var publishedContent = umbracoHelper.TypedContent(gameKey);
                    var gameContent = gameService.GetLottoGameModelByKey(gameKey, publishedContent);
                    var cartItem = new ShoppingCartItem();

                    cartItem.LottoGameId = g.Key;
                    cartItem.TicketsCount = g.Count();
                    cartItem.TicketPrice = gameContent.GameSettings.TicketPrice;
                    cartItem.LottoGameName = gameContent.LottoGameName;
                    cartItem.LotteryGameUrl = gameContent.LottoGameUrl;

                    cartItems.Add(cartItem);
                }
            }

            if (cartItems.Any())
            {
                foreach (var c in cartItems)
                {
                    totalCartValue += (decimal)(c.TicketPrice * c.TicketsCount);
                }
            }

            return cartItems;
        }

        public void DeleteAllTicketsByGame(string username, int lottoGameId)
        {
            try
            {
                var user = this.FindUser(username);
                var userTickets = user.LottoTickets.Where(t => !t.IsCalculated && t.LotteryGameID == lottoGameId).ToList();

                if (userTickets != null && userTickets.Any())
                {
                    foreach (var lottoTicket in userTickets)
                    {
                        if (lottoTicket.LottoTicketBalls != null && lottoTicket.LottoTicketBalls.Any())
                        {
                            foreach (var ticketRelation in lottoTicket.LottoTicketBalls)
                            {
                                this.UnitOfWork.LotteryBallRepository.Remove(ticketRelation.LotteryBall);
                            }
                            this.UnitOfWork.LottoTicketBallRepository.Remove(lottoTicket.LottoTicketBalls.ToArray());
                        }
                    }

                    // Update user money balance 
                    decimal ticketPrice = this.GetGameTicketPrice(userTickets.FirstOrDefault().LotteryGame.GameKey);
                    decimal balanceChange = userTickets.Count * ticketPrice;
                    this.UpdateUserBalance(user, balanceChange, false);

                    UnitOfWork.TicketsRepository.Remove(userTickets.ToArray());
                    UnitOfWork.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw ex;
            }
        }

        public void DeleteSingleTicketByGame(string username, int lotteryTicketId)
        {
            try
            {
                var user = this.FindUser(username);
                var userTicket = user.LottoTickets.FirstOrDefault(t => !t.IsCalculated && t.ID == lotteryTicketId);

                if (userTicket != null)
                {
                    if (userTicket.LottoTicketBalls != null && userTicket.LottoTicketBalls.Any())
                    {
                        foreach (var ticketRelation in userTicket.LottoTicketBalls)
                        {
                            this.UnitOfWork.LotteryBallRepository.Remove(ticketRelation.LotteryBall);
                        }
                        this.UnitOfWork.LottoTicketBallRepository.Remove(userTicket.LottoTicketBalls.ToArray());
                    }

                    // Update user money balance, it is one ticket so the change in the balance is one ticket price
                    decimal balanceChange = this.GetGameTicketPrice(userTicket.LotteryGame.GameKey);
                    this.UpdateUserBalance(user, balanceChange, false);

                    UnitOfWork.TicketsRepository.Remove(userTicket);
                    UnitOfWork.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw ex;
            }
        }

        public void UpdateUserBalance(string username, decimal balanceChange, bool commitChanges = false)
        {
            var user = this.FindUser(username);
            this.UpdateUserBalance(user, balanceChange, commitChanges);
        }

        public void UpdateUserBalance(User user, decimal balanceChange, bool commitChanges = false)
        {
            try
            {
                user.Balance.Value += balanceChange;
                this.UnitOfWork.BalanceRepository.Update(user.Balance);
                if (commitChanges)
                {
                    this.UnitOfWork.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw ex;
            }
        }

        public List<LotteryTicket> GetAllTicketsByDraw(int lottoGameId, DateTime previousDrawTime, DateTime currentDrawTime)
        {
            var allTickets =
                UnitOfWork.TicketsRepository
                    .AsQuery(t => t.LotteryGameID == lottoGameId && t.InputTime > previousDrawTime && t.InputTime < currentDrawTime)
                    .ToList()
                    .Select(t => LotteryTicketConverter.AssignFrom(t))
                    .ToList();
            return allTickets;
        }

        public void UpdateTicketsAtLost(List<LotteryTicket> tickets)
        {
            try
            {
                foreach (var ticket in tickets)
                {
                    var lottoTicket = UnitOfWork.TicketsRepository.Get(t => t.ID == ticket.ID);
                    if (lottoTicket != null)
                    {
                        lottoTicket.IsCalculated = true;
                        lottoTicket.IsWinning = false;
                        UnitOfWork.TicketsRepository.Update(lottoTicket);
                    }
                }
                UnitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw ex;
            }
        }

        public void UpdateWinningTickets(LottoDrawStatistic statistics)
        {
            try
            {
                foreach (var entry in statistics.Table)
                {
                    if (entry.Value.Tickets.Any())
                    {
                        foreach (var ticket in entry.Value.Tickets)
                        {
                            var lottoTicket = UnitOfWork.TicketsRepository.Get(t => t.ID == ticket.ID);
                            if (lottoTicket != null)
                            {
                                this.UpdateUserBalance(lottoTicket.User, entry.Value.WinningTier.WinningPerPerson);

                                lottoTicket.IsWinning = true;
                                lottoTicket.IsCalculated = true;
                                UnitOfWork.TicketsRepository.Update(lottoTicket);
                            }
                        }
                    }
                }
                UnitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw ex;
            }
        }

        private LottoTicket CreateNewLotteryTicket(int gameId, int userId, IEnumerable<byte> ballsList, IEnumerable<byte> bonusBallsList)
        {
            DateTime creationTime = DateTime.Now;
            var newTicket = new LottoTicket
            {
                UserID = userId,
                LotteryGameID = gameId,
                InputTime = creationTime
            };

            // Add all drawing balls to the ticket
            byte postion = 1;
            foreach (var ballNumber in ballsList)
            {
                var newLotteryBall = new LotteryBall
                {
                    IsBonusBall = false,
                    BallNumber = ballNumber,
                    Position = postion++
                };

                // Create new ticket - ball connection
                var newLottoTicketBall = new LottoTicketBall
                {
                    LottoTicket = newTicket,
                    LotteryBall = newLotteryBall
                };

                this.UnitOfWork.LottoTicketBallRepository.Add(newLottoTicketBall);
                this.UnitOfWork.LotteryBallRepository.Add(newLotteryBall);
            }

            // Add all bonus balls to the ticket
            postion = 1;
            foreach (var ballNumber in bonusBallsList)
            {
                var newLotteryBall = new LotteryBall
                {
                    IsBonusBall = true,
                    BallNumber = ballNumber,
                    Position = postion++
                };

                var newLottoTicketBall = new LottoTicketBall
                {
                    LotteryBall = newLotteryBall,
                    LottoTicket = newTicket
                };

                this.UnitOfWork.LottoTicketBallRepository.Add(newLottoTicketBall);
                this.UnitOfWork.LotteryBallRepository.Add(newLotteryBall);
            }

            return newTicket;
        }

        private User GetNewUser(string umbracoUsername)
        {
            var balanceCurrency = this.UnitOfWork.CurrencyRepository.AsQuery().FirstOrDefault();
            if (balanceCurrency == null)
            {
                throw new ArgumentNullException("User Balance Currency", "The default lottery system currency could not be found!");
            }
            User newUser = new User
            {
                Username = umbracoUsername
            };
            newUser.Balance = new Balance
            {
                Value = this.InitialUserBalance,
                CurrencyID = balanceCurrency.ID
            };
            return newUser;
        }

        private decimal GetGameTicketPrice(Guid lottoGameKey)
        {
            var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
            var content = umbracoHelper.TypedContent(lottoGameKey);

            return decimal.Parse(content.GetProperty("TicketPrice").Value.ToString());
        }
    }
}
