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
using Umbraco.Core;

namespace LottoDemo.BusinessLogic.Services
{
    public class LottoUserService
    {
        #region Singleton Pattern

        private LottoUserService() { }

        private static readonly LottoUserService _instance = new LottoUserService();

        public static LottoUserService Instance { get { return _instance; } }

        #endregion

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

        public UserUnitOfWork UnitOfWork { get { return UserUnitOfWork.Instance; } }

        public bool CreateNewUser(string umbracoUsername)
        {
            try
            {
                var newUser = this.GetNewUser(umbracoUsername);
                this.UnitOfWork.UserRepository.Add(newUser);
                this.UnitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                Log.Error(typeof(LottoUserService), string.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                this.UnitOfWork.Rollback();
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
                    var gameUnitOfWork = LottoGameUnitOfWork.Instance;
                    var lottoGameKey = gameUnitOfWork.GameRepository.Get(g => g.ID == tickets.LotteryGameId).GameKey;
                    decimal gameTicketPrice = this.GetGameTicketPrice(lottoGameKey);

                    // Add ticket one
                    if (tickets.DrawBallsTicket1.Count() > 0 && tickets.BonusBallsTicket1.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.DrawBallsTicket1, tickets.BonusBallsTicket1, tickets.LotteryGameId, loggedUser.ID);
                        this.UnitOfWork.LottoTicketRepository.Add(ticket);

                        // Update user money balance, it is one ticket so the change in the balance is one ticket price
                        this.UpdateUserBalance(loggedUser, (-1) * gameTicketPrice, false);
                    }

                    // Add the second ticket
                    if (tickets.DrawBallsTicket2.Count() > 0 && tickets.BonusBallsTicket2.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.DrawBallsTicket2, tickets.BonusBallsTicket2, tickets.LotteryGameId, loggedUser.ID);
                        this.UnitOfWork.LottoTicketRepository.Add(ticket);

                        // Update user money balance, it is one ticket so the change in the balance is one ticket price
                        this.UpdateUserBalance(loggedUser, (-1) * gameTicketPrice, false);
                    }

                    // Add third ticket
                    if (tickets.DrawBallsTicket3.Count() > 0 && tickets.BonusBallsTicket3.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.DrawBallsTicket3, tickets.BonusBallsTicket3, tickets.LotteryGameId, loggedUser.ID);
                        this.UnitOfWork.LottoTicketRepository.Add(ticket);

                        // Update user money balance, it is one ticket so the change in the balance is one ticket price
                        this.UpdateUserBalance(loggedUser, (-1) * gameTicketPrice, false);
                    }

                    // Save the changes to the database
                    this.UnitOfWork.CommitChanges();
                }
                catch (Exception ex)
                {
                    this.UnitOfWork.Rollback();
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
                var gameService = LotteryGameService.Instance;
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

                this.UnitOfWork.LottoTicketRepository.Remove(userTickets.ToArray());
                this.UnitOfWork.CommitChanges();
            }
        }

        public void DeleteSingleTicketByGame(string username, int lotteryTicketId)
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

                this.UnitOfWork.LottoTicketRepository.Remove(userTicket);
                this.UnitOfWork.CommitChanges();
            }
        }

        public void UpdateUserBalance(string username, decimal balanceChange, bool commitChanges = false)
        {
            var user = this.FindUser(username);
            this.UpdateUserBalance(user, balanceChange, commitChanges);
        }

        public void UpdateUserBalance(User user, decimal balanceChange, bool commitChanges = false)
        {
            user.Balance.Value += balanceChange;
            this.UnitOfWork.BalanceRepository.Update(user.Balance);
            if (commitChanges)
            {
                this.UnitOfWork.CommitChanges();
            }
        }

        private LottoTicket CreateNewLotteryTicket(IEnumerable<byte> ballsList, IEnumerable<byte> bonusBallsList, int gameId, int userId)
        {
            DateTime creationTime = DateTime.Now;
            var newTicket = new LottoTicket
            {
                UserID = userId,
                LotteryGameID = gameId,
                InputTime = creationTime,
                ModifiedDate = creationTime,
                CreationDate = creationTime
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
                Username = umbracoUsername,
                CreationDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            newUser.Balance = new Balance
            {
                Value = this.InitialUserBalance,
                CurrencyID = balanceCurrency.ID,
                CreationDate = DateTime.Now,
                ModifiedDate = DateTime.Now
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
