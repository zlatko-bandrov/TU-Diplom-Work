using LottoDemo.BusinessLogic.LotteryLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            NumbersGenerator generator = new NumbersGenerator();
            generator.Next(1, 49);
        }
    }
}
