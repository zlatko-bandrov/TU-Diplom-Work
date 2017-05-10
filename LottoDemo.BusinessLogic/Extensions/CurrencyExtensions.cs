using LottoDemo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
