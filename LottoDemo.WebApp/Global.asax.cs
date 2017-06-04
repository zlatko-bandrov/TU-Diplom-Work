using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LottoDemo.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
// TODO: Fix the login popup
// TODO: Fix the login page
// TODO: Forgot password Page
// TODO: Fix the user account changes
// TODO: Fix the user results and winnings page
// TODO: Test the winings tickets calculation