using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace CARTApplication
{
    public class LogException
    {
        //This method logs the messag into the database
        public void LogErrorInDataBase(Exception ex, HttpContext Context) 
        {
            //Context = HttpContext.Current;
            LogEntry logEntry = new LogEntry();
            logEntry.Severity = System.Diagnostics.TraceEventType.Information;
            logEntry.Title = ex.Message;
            logEntry.Categories.Add("General");
            logEntry.Message = string.Format(ex.StackTrace);
            logEntry.TimeStamp = DateTime.Now;
            logEntry.Win32ThreadId = Context.User.Identity.Name;
            logEntry.ManagedThreadName = Context.Request.Url.ToString();
            Logger.Write(logEntry);
            
        }
    }
}
