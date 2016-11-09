using LottoDemo.BusinessLogic.LotteryLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.LottoGeneratorService
{
    public partial class LottoGenerator : ServiceBase
    {
        public LottoGenerator()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.eventLog.WriteEntry("LottoDemo Service OnStart");

            NumbersGenerator generator = new NumbersGenerator();
            generator.Next(1, 49);
        }

        protected override void OnStop()
        {
            this.eventLog.WriteEntry("LottoDemo Service OnStop");
        }
    }
}
