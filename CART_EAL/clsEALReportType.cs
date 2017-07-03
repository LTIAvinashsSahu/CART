using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CART_EAL
{
    public class clsEALReportType
    {
        private static string serverReport = "ServerReport";

        public static string ServerReport
        {
            get { return clsEALReportType.serverReport; }
           
        }
        private static string shareReport = "ShareReport";

        public static string ShareReport
        {
            get { return clsEALReportType.shareReport; }
           
        }
        private static string sqlReport = "SQLReport";

        public static string SQLReport
        {
            get { return clsEALReportType.sqlReport; }

        }
        private static string orcaleReport = "OracleReport";

        public static string OracleReport
        {
            get { return clsEALReportType.orcaleReport; }

        }
        private static string psiReport = "PSI Online";

        public static string PSIReport
        {
            get { return clsEALReportType.psiReport; }

        }
        private static string linuxReport = "LinuxReport";

        public static string LinuxReport
        {
            get { return clsEALReportType.linuxReport; }

        }
        private static string securityGroupReport = "SecurityGroupReport";

        public static string SecurityGroupReport
        {
            get { return clsEALReportType.securityGroupReport; }

        }
    }
}
