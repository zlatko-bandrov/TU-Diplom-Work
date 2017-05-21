using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.BusinessLogic.Converters
{
    public static class LottoDrawModelConverter
    {
        public static LottoDrawModel AssignFrom(LottoDrawing lottoDraw)
        {
            LottoDrawModel model = new LottoDrawModel();
            model.ID = lottoDraw.ID;
            model.BallsList.AddRange(lottoDraw.LottoDrawingBalls.Where(b => b.LotteryBall.IsBonusBall == false).Select(b => b.LotteryBall.BallNumber));
            model.BonusBallsList.AddRange(lottoDraw.LottoDrawingBalls.Where(b => b.LotteryBall.IsBonusBall == true).Select(b => b.LotteryBall.BallNumber));
            model.LottoGameID = lottoDraw.LotteryGameID;

            return model;
        }
    }
}
