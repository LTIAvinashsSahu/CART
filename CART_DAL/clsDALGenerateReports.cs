using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CART_DAL
{
    class clsDALGenerateReports:clsDBConnection 
    {
        public DataSet GetAllReceivedReports(string Quarter)
        {
            return new DataSet();
        }
        public DataSet NewUserReport(string CurrentQuarter,string PrevQuarter)
        {
            return new DataSet();
        }
    }
}
