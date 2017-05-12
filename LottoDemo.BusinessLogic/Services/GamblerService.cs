using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Models.Cart;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace LottoDemo.BusinessLogic.Services
{
    public class GamblerService
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

        private UserUnitOfWork UnitOfWork = new UserUnitOfWork();

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
                Log.Error(typeof(GamblerService), string.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, ex.Message), ex);
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

                    // Add ticket one
                    if (tickets.DrawBallsTicket1.Count() > 0 && tickets.BonusBallsTicket1.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.DrawBallsTicket1, tickets.BonusBallsTicket1, tickets.LotteryGameId, loggedUser.ID);
                        this.UnitOfWork.LottoTicketRepository.Add(ticket);
                    }

                    // Add the second ticket
                    if (tickets.DrawBallsTicket2.Count() > 0 && tickets.BonusBallsTicket2.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.DrawBallsTicket2, tickets.BonusBallsTicket2, tickets.LotteryGameId, loggedUser.ID);
                        this.UnitOfWork.LottoTicketRepository.Add(ticket);
                    }

                    // Add third ticket
                    if (tickets.DrawBallsTicket3.Count() > 0 && tickets.BonusBallsTicket3.Count() > 0)
                    {
                        ticket = CreateNewLotteryTicket(tickets.DrawBallsTicket3, tickets.BonusBallsTicket3, tickets.LotteryGameId, loggedUser.ID);
                        this.UnitOfWork.LottoTicketRepository.Add(ticket);
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
    }
}
