using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LottoDemo.BusinessLogic.Extensions.Currency;

namespace LottoDemo.BusinessLogic.Extensions.LotteryGame
{
    public static class LotteryGameExtensions
    {
        public static LottoGameModel ToLottoGameModel(this LottoDemo.DataAccess.LotteryGame lottoGame)
        {
            LottoGameModel model = new LottoGameModel
            {
                Id = lottoGame.ID,
                GameKey = lottoGame.GameKey
            };

            if (lottoGame.Jackpot != null && lottoGame.Jackpot.Balance != null)
            {
                model.Jackpot = lottoGame.Jackpot.Balance.Value;
                if (lottoGame.Jackpot.Balance.Currency != null)
                {
                    model.JackpotCurrency = lottoGame.Jackpot.Balance.Currency.ToCurrencyModel();
                }
            }

            return model;
        }
    }
}
