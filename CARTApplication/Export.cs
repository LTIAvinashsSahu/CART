using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;


namespace CARTApplication
{
    public class Export
    {
        public void ExportGridView(GridView gdExport, string filename)
        {
            //iFlag = 1;

            string attachment = "attachment; filename=" + filename + ".xls";
            HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gdExport.RenderControl(htw);
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
            //iFlag = 0;
        }
    }
}
