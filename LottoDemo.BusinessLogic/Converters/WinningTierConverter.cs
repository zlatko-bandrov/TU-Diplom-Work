using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.BusinessLogic.Converters
{
    public static class WinningTierConverter
    {
        public static WinningTier AssignFrom(GameWinningsTier dbWinningTier)
        {
            var newWinningTier = new WinningTier
            {
                ID = dbWinningTier.ID,
                BallsCount = dbWinningTier.BallsCount,
                BonnusBallCount = dbWinningTier.BonnusBallCount,
                TierPercent = dbWinningTier.TierPercent
            };
            return newWinningTier;
        }
    }
}
