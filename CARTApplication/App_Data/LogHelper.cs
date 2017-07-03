using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CARTApplication
{
    public static class LogHelper
    {
        public static void LogError(Exception exc)
        {
            HttpContext Context = HttpContext.Current;
            LogException objLogException = new LogException();
            objLogException.LogErrorInDataBase(exc, Context);
            HttpContext.Current.Server.Transfer("wfrmErrorPage.aspx", false);
        }
    }
}
