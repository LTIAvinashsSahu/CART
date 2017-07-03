using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CART_EAL
{
    public class clsEALUser
    {
        private string strUserName = String.Empty;

        public string StrUserName
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        private string strUserADID = String.Empty;

        public string StrUserADID
        {
            get { return strUserADID; }
            set { strUserADID = value; }
        }
        private string strUserEmailID = String.Empty;

        public string StrUserEmailID
        {
            get { return strUserEmailID; }
            set { strUserEmailID = value; }
        }
        private string strUserSID = String.Empty;

        public string StrUserSID
        {
            get { return strUserSID; }
            set { strUserSID = value; }
        }
        private string strFname;

        public string StrFname
        {
            get { return strFname; }
            set { strFname = value; }
        }
        private string strLname;

        public string StrLname
        {
            get { return strLname; }
            set { strLname = value; }
        }

        private clsEALRoles[] userRole = null;

        public clsEALRoles[] UserRole
        {
            get { return userRole; }
            set { userRole = value; }
        }
    }
}
