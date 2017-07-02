using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Common
{
    public class Constants
    {
        public static readonly string SettingsItemId = ConfigurationManager.AppSettings["SettingsItemID"];

        public static readonly string DefaultGameJackpot = ConfigurationManager.AppSettings["InitialLotteryJackpot"];

        public static readonly int DefaultDrawInterval = 10;

        #region Errors Keys

        public static readonly string ERR_USERREG_USEREXISTS_KEY = "REG_USEREXISTS";
        public static readonly string ERR_USERREG_REQUIREDNAME_KEY = "REG_REQUIREDNAME";
        public static readonly string ERR_USERREG_REQUIREDFAMILY_KEY = "REG_REQUIREDFAMILY";
        public static readonly string ERR_USERREG_REQUIREDEMAIL_KEY = "REG_REQUIREDEMAIL";
        public static readonly string ERR_USERREG_NONEVALIDDATE_KEY = "REG_NONEVALIDDATE";
        public static readonly string ERR_USERREG_DATE18YEARS_KEY = "REG_DATE18YEARS";
        public static readonly string ERR_USERREG_PASSREQUIRED_KEY = "REG_PASSREQUIRED";
        public static readonly string ERR_USERREG_PHONEREQUIRED_KEY = "REG_PHONEREQUIRED";

        public static readonly string ERR_USERLOG_REQUIREDEMAIL_KEY = "LOG_REQUIREDEMAIL";
        public static readonly string ERR_USERLOG_PASSREQUIRED_KEY = "LOG_PASSREQUIRED";
        public static readonly string ERR_USERLOG_NOTVALIDCRED_KEY = "LOG_NOTVALIDCRED";

        public static readonly string ERR_ACCOUNT_OLDPASSREQUIRED_KEY = "ACCOUNT_OLDPASSREQUIRED";
        public static readonly string ERR_ACCOUNT_NEWPASSREQUIRED_KEY = "ACCOUNT_NEWPASSREQUIRED";
        public static readonly string ERR_ACCOUNT_CONFIRMPASSREQUIRED_KEY = "ACCOUNT_CONFIRMPASSREQUIRED";
        public static readonly string ERR_ACCOUNT_PASSNOTMATCH_KEY = "ACCOUNT_PASSNOTMATCH";

        #endregion

        #region Constants

        public static readonly int HOME_PAGE_ID = 1050;
        public static readonly int LOGIN_PAGE_ID = 1265;

        #endregion
    }
}