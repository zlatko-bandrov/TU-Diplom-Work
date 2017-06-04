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
        private readonly decimal InitialUserBalance = Decimal.Parse(ConfigurationManager.AppSettings["InitialUserBalance"]);


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


        public void AddNewLotteryTickets(string username, AddToCartModel addToCartModel)
        {
            var loggedUser = this.FindUser(username);
            if (loggedUser == null)
            {
                throw new ArgumentNullException("loggedUser", "There is no one logged in currently.");
            }
            if (addToCartModel.LottoDrawId <= 0)
            {
                return;
            }

            try
            {
                LottoTicket ticket = null;
                var gameUnitOfWork = new LottoGameUnitOfWork();
                var lottoGameKey = gameUnitOfWork.DrawRepository.Get(g => g.ID == addToCartModel.LottoDrawId).LotteryGame.GameKey;
                decimal gameTicketPrice = GetGameTicketPrice(lottoGameKey);

                // Add ticket one
                if (addToCartModel.DrawBallsTicket1.Count() > 0 && addToCartModel.BonusBallsTicket1.Count() > 0)
                {
                    ticket = CreateNewLotteryTicket(addToCartModel.LottoDrawId, loggedUser.ID, addToCartModel.DrawBallsTicket1, addToCartModel.BonusBallsTicket1);
                    this.UnitOfWork.TicketsRepository.Add(ticket);

                    // Update user money balance, it is one ticket so the change in the balance is one ticket price
                    loggedUser.Balance.Value += (-1) * gameTicketPrice;
                }
                // Add the second ticket
                if (addToCartModel.DrawBallsTicket2.Count() > 0 && addToCartModel.BonusBallsTicket2.Count() > 0)
                {
                    ticket = CreateNewLotteryTicket(addToCartModel.LottoDrawId, loggedUser.ID, addToCartModel.DrawBallsTicket2, addToCartModel.BonusBallsTicket2);
                    this.UnitOfWork.TicketsRepository.Add(ticket);

                    // Update user money balance, it is one ticket so the change in the balance is one ticket price
                    loggedUser.Balance.Value += (-1) * gameTicketPrice;
                }
                // Add third ticket
                if (addToCartModel.DrawBallsTicket3.Count() > 0 && addToCartModel.BonusBallsTicket3.Count() > 0)
                {
                    ticket = CreateNewLotteryTicket(addToCartModel.LottoDrawId, loggedUser.ID, addToCartModel.DrawBallsTicket3, addToCartModel.BonusBallsTicket3);
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

                var lotteryTickets = loggedUser.LottoTickets.Where(t => t.LottoDrawing.LotteryGameID == lotteryGame.Id && !t.IsCalculated);
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
                        LottoTicketId = ticket.ID,
                        LottoDrawId = ticket.LottoDrawID
                    });
                }
                cartItemsList.Add(gameCartItem);
            }
            return cartItemsList;
        }

        public List<ShoppingCartItem> GetAllShoppingCartTickets(string username, out decimal userBalance, out decimal totalCartValue)
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
                var allUserGames = loggedUser.LottoTickets.Where(t => !t.IsCalculated).GroupBy(t => t.LottoDrawing.LotteryGameID).ToList();
                var gameService = new LotteryGameService();
                var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);

                foreach (var g in allUserGames)
                {
                    Guid gameKey = g.FirstOrDefault().LottoDrawing.LotteryGame.GameKey;
                    var publishedContent = umbracoHelper.TypedContent(gameKey);
                    var gameContent = gameService.GetLottoGameModelByKey(gameKey, publishedContent);
                    var cartItem = new ShoppingCartItem
                    {
                        LottoGameId = g.Key,
                        TicketsCount = g.Count(),
                        TicketPrice = gameContent.GameSettings.TicketPrice,
                        LottoGameName = gameContent.LottoGameName,
                        LotteryGameUrl = gameContent.LottoGameUrl
                    };
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
                var user = FindUser(username);
                var userTickets = user.LottoTickets.Where(t => !t.IsCalculated && t.LottoDrawing.LotteryGameID == lottoGameId);
                if (userTickets != null && userTickets.Any())
                {
                    foreach (var lottoTicket in userTickets)
                    {
                        if (lottoTicket.LottoTicketBalls != null && lottoTicket.LottoTicketBalls.Any())
                        {
                            foreach (var ticketRelation in lottoTicket.LottoTicketBalls)
                            {
                                UnitOfWork.LotteryBallRepository.Remove(ticketRelation.LotteryBall);
                            }
                            UnitOfWork.LottoTicketBallRepository.Remove(lottoTicket.LottoTicketBalls.ToArray());
                        }
                    }

                    // Update user money balance 
                    decimal ticketPrice = GetGameTicketPrice(userTickets.FirstOrDefault().LottoDrawing.LotteryGame.GameKey);
                    decimal balanceChange = userTickets.Count() * ticketPrice;
                    UpdateUserBalance(user, balanceChange, false);

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
                var user = FindUser(username);
                var userTicket = user.LottoTickets.FirstOrDefault(t => !t.IsCalculated && t.ID == lotteryTicketId);
                if (userTicket != null)
                {
                    if (userTicket.LottoTicketBalls != null && userTicket.LottoTicketBalls.Any())
                    {
                        var lottoBallsList = userTicket.LottoTicketBalls.Select(b => b.LotteryBall).ToArray();
                        UnitOfWork.LotteryBallRepository.Remove(lottoBallsList);
                        UnitOfWork.LottoTicketBallRepository.Remove(userTicket.LottoTicketBalls.ToArray());
                    }

                    // Update user money balance, it is one ticket so the change in the balance is one ticket price
                    decimal balanceChange = GetGameTicketPrice(userTicket.LottoDrawing.LotteryGame.GameKey);
                    UpdateUserBalance(user, balanceChange, false);

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

        public List<LotteryTicket> GetAllTicketsByDraw(int gameDrawId)
        {
            var allTickets = UnitOfWork.TicketsRepository.AsList(t => t.LottoDrawID == gameDrawId).Select(t => LotteryTicketConverter.AssignFrom(t)).ToList();
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
                                // Update user balance because of winning ticket
                                UpdateUserBalance(lottoTicket.User, entry.Value.WinningTier.WinningPerPerson);
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


        private LottoTicket CreateNewLotteryTicket(int gameDrawId, int userId, IEnumerable<byte> ballsList, IEnumerable<byte> bonusBallsList)
        {
            DateTime creationTime = DateTime.Now;
            var newTicket = new LottoTicket
            {
                UserID = userId,
                LottoDrawID = gameDrawId,
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
                UnitOfWork.LottoTicketBallRepository.Add(newLottoTicketBall);
                UnitOfWork.LotteryBallRepository.Add(newLotteryBall);
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
                UnitOfWork.LottoTicketBallRepository.Add(newLottoTicketBall);
                UnitOfWork.LotteryBallRepository.Add(newLotteryBall);
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