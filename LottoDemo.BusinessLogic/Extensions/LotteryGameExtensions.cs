using LottoDemo.Entities.Models;
using Umbraco.Core.Models;

namespace LottoDemo.BusinessLogic.Extensions.LotteryGame
{
    public static class LotteryGameExtensions
    {
        public static LottoGameModel ToLottoGameModel(this LottoDemo.DataAccess.LotteryGame lottoGame, IPublishedContent contentItem = null)
        {
            LottoGameModel model = new LottoGameModel(contentItem)
            {
                Id = lottoGame.ID,
                GameKey = lottoGame.GameKey
            };
            return model;
        }
    }
}
