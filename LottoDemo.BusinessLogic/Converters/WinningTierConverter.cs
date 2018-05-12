using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;

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
