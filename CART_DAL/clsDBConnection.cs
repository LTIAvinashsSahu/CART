using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CART_DAL
{
    public class clsDBConnection
    {
        public static string strconnectionString = "";
        public static string strConnStringBMC = "";
        public static clsDBConnection objDBConnection = new clsDBConnection();

        public clsDBConnection()
         {
             strconnectionString = ConfigurationManager.AppSettings["CARTconnection"].ToString();
             strConnStringBMC = ConfigurationManager.AppSettings["BMCMailBox"].ToString();
         }
    }
}
