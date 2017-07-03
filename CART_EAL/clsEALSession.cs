using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CART_EAL
{
    public class clsEALSession
    {
        private static string currentUser = "CurrentUser";

        public static string CurrentUser
        {
            get { return clsEALSession.currentUser; }
            set { clsEALSession.currentUser = value; }
        }
        private static string groupMapping = "GroupMapping";

        public static string GroupMapping
        {
            get { return clsEALSession.groupMapping; }
            set { clsEALSession.groupMapping = value; }
        }
        private static string userRole = "Role";

        public static string UserRole
        {
            get { return clsEALSession.userRole; }
            set { clsEALSession.userRole = value; }
        }

        private static string servers = "Servers";

        public static string Servers
        {
            get { return clsEALSession.servers; }
            set { clsEALSession.servers = value; }
        }


        private static string appSelected = "SelectedAppplication";

        public static string SelectedAppplication
        {
            get { return clsEALSession.appSelected; }
            set { clsEALSession.appSelected = value; }
        }

        private static string databaseMapping = "DatabaseMapping";

        public static string DatabaseMapping
        {
            get { return clsEALSession.databaseMapping; }
            set { clsEALSession.databaseMapping = value; }
        }
        private static string reportUsers = "ReportUsers";

        public static string ReportUsers
        {
            get { return clsEALSession.reportUsers; }
            set { clsEALSession.reportUsers = value; }
        }
        //Initiatives

        private static string initiatives = "Initiatives";

        public static string Initiatives
        {
            get { return clsEALSession.initiatives; }
            set { clsEALSession.initiatives = value; }
        }


        private static string userDetails = "UserDetails";

        public static string UserDetails
        {
            get { return clsEALSession.userDetails; }
            set { clsEALSession.userDetails = value; }
        }

        private static string applications = "Applications";

        public static string Applications
        {
            get { return clsEALSession.applications; }
            set { clsEALSession.applications = value; }
        }


        private static string shares = "Shares";

        public static string Shares
        {
            get { return clsEALSession.shares; }
            set { clsEALSession.shares = value; }
        }


        private static string appApprovers = "Approvers";

        public static string Approvers
        {
            get { return clsEALSession.appApprovers; }
            set { clsEALSession.appApprovers = value; }
        }
        private static string applicationID = "AppID";

        public static string ApplicationID
        {
            get { return clsEALSession.applicationID; }
            set { clsEALSession.applicationID = value; }
        }
        private static string reportID;

        public static string ReportID
        {
            get { return clsEALSession.reportID; }
            set { clsEALSession.reportID = value; }
        }
        private static string searchUserID = "SUserID";

        public static string SearchUserID
        {
            get { return clsEALSession.searchUserID; }
            set { clsEALSession.searchUserID = value; }
        }

        private static string previousPage = "PreviousPage";

        public static string PreviousPage 
        {
            get { return clsEALSession.previousPage; }
            set { clsEALSession.previousPage = value; }
        }

       
        private static string valuePath = "ValuePath";

        public static string ValuePath
        {
            get { return clsEALSession.valuePath; }
            set { clsEALSession.valuePath = value; }
        }
        private static string selectedQuarter = "SelectedQuarter";

        public static string SelectedQuarter
        {
            get { return clsEALSession.selectedQuarter; }
            set { clsEALSession.selectedQuarter = value; }
        }


        private static string userSID = "UserSID";

        public static string UserSID
        {
            get { return clsEALSession.userSID; }
            set { clsEALSession.userSID = value; }
        }
        private static string reportData = "ReportData";

        public static string ReportData
        {
            get { return clsEALSession.reportData; }
           
        }

        private static string display = "Display";

        public static string Display
        {
            get { return clsEALSession.display; }

        }
        private static string accounts = "Accounts";

        public static string Accounts
        {
            get { return clsEALSession.accounts; }

        }
        private static string globalApprovers = "GlobalApprovers";

        public static string GlobalApprovers
        {
            get { return clsEALSession.globalApprovers; }

        }

        private static string sqlaccounts = "SQLAccounts";

        public static string SQLAccounts
        {
            get { return clsEALSession.sqlaccounts; }

        }
        private static string oracleaccounts = "ORACLEAccounts";

        public static string ORACLEAccounts
        {
            get { return clsEALSession.oracleaccounts; }
        }

        private static string psiAccounts = "PSIAccounts";

        public static string PSIAccounts
        {
            get { return clsEALSession.psiAccounts; }
        }

        private static string lastRemoved = "LastRemoved";

        public static string LastRemoved
        {
            get { return clsEALSession.lastRemoved; }
            set { clsEALSession.lastRemoved = value; }
        }

        private static string linuxaccounts = "LinuxAccounts";

        public static string LinuxAccounts
        {
            get { return clsEALSession.linuxaccounts; }

        }
        private static string secgrpaccounts = "SecGrpAccounts";

        public static string SecGrpAccounts
        {
            get { return clsEALSession.secgrpaccounts; }

        }
    }
}
