using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CART_EAL
{
    public class clsEALRoles
    {
        private static string complianceadmin = "Compliance Administrator";
        private static string globalApprover = "Global Approver";
        private static string approver = "Approver";
        private static string complianceTester = "Compliance Tester";
        private static string complianceAuditor = "Compliance Auditor";
        private static string controlOwner = "Control Owner";

        public static string ControlOwner
        {
            get { return clsEALRoles.controlOwner; }
            set { clsEALRoles.controlOwner = value; }
        }

        public static string ComplianceAdmin
        {
            get { return clsEALRoles.complianceadmin; }
            
        }
        public static string GlobalApprover
        {
            get { return clsEALRoles.globalApprover; }
           
        }
        public static string Approver
        {
            get { return clsEALRoles.approver; }
           
        }
        public static string ComplianceTester
        {
            get { return clsEALRoles.complianceTester; }
           
        }
        public static string ComplianceAuditor
        {
            get { return clsEALRoles.complianceAuditor; }

        }
    }
}
