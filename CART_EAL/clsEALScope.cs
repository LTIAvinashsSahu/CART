using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CART_EAL
{
    public class clsEALScope
    {
        private static string thisReport = "ThisReport";

        public static string ThisReport
        {
            get { return clsEALScope.thisReport; }
            set { clsEALScope.thisReport = value; }
        }
        private static string thisApp = "ThisApplication";

        public static string ThisApp
        {
            get { return clsEALScope.thisApp; }
            set { clsEALScope.thisApp = value; }
        }
        private static string allMyApp = "MyAllApps";

        public static string AllMyApp
        {
            get { return clsEALScope.allMyApp; }
            set { clsEALScope.allMyApp = value; }
        }

        private static string allReports = "AllReports";

        public static string AllReports
        {
            get { return clsEALScope.allReports; }
            set { clsEALScope.allReports= value; }
        }

    }
}
