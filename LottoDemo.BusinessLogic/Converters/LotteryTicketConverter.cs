using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using System.Linq;

namespace LottoDemo.BusinessLogic.Converters
{
    public static class LotteryTicketConverter
    {
        public static LotteryTicket AssignFrom(LottoTicket dbLottoTicket)
        {
            var ballNumbers = dbLottoTicket.LottoTicketBalls.Where(b => b.LotteryBall.IsBonusBall == false).Select(b => b.LotteryBall.BallNumber).ToList();
            var bonusBallNumbers = dbLottoTicket.LottoTicketBalls.Where(b => b.LotteryBall.IsBonusBall == true).Select(b => b.LotteryBall.BallNumber).ToList();
            var ticket = new LotteryTicket(ballNumbers, bonusBallNumbers);
            ticket.ID = dbLottoTicket.ID;
            
            return ticket;
        }
    }
}