using LottoDemo.Entities.Models;

namespace LottoDemo.BusinessLogic.Extensions.Currency
{
    public static class CurrencyExtensions
    {
        public static CurrencyModel ToCurrencyModel(this LottoDemo.DataAccess.Currency currency)
        {
            CurrencyModel model = new CurrencyModel
            {
                Id = currency.ID,
                Code = currency.Code,
                Name = currency.Name,
                Symbol = currency.Symbol
            };

            return model;
        }
    }
}
