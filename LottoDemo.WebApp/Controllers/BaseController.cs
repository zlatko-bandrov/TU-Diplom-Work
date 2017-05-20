using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class BaseController : SurfaceController
    {
        protected virtual void WriteErrorMessage(Exception ex)
        {
            this.Logger.Error(typeof(GameDetailsController), ex.Message, ex);
        }
    }
}