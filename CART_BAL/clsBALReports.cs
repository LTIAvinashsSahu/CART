using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CART_EAL;
using CART_DAL;

namespace CART_BAL
{
    public class clsBALReports
    {

        #region DataMembers
        DataSet objds;
        clsDALReports objclsDALReports;
        
        #endregion

        #region Search
        public DataSet SearchAllReport(string Type, string Quarter, int AppID, string SecurityGroup, string LastApproved, string AccountName, string signoffStatus, string Accountstatus, string LoggedInUser, string PreviousQuartertoSelected)
        {
            clsDALReports objclsDALReports = new clsDALReports();
            DataSet ds = objclsDALReports.SearchAllReport(Type, Quarter, AppID, SecurityGroup, LastApproved, AccountName, signoffStatus, Accountstatus, LoggedInUser, PreviousQuartertoSelected);
            return ds;
        }
        #endregion

        #region Search
        public DataSet GetAllReport(int ApplicationID, string[] role, string Quarter)
        {
            clsDALReports objclsDALReports =  new clsDALReports();
            DataSet ds = objclsDALReports.GetAllReport(ApplicationID, role, Quarter);
            return ds;

        }
        #endregion
        public DataSet GetApplicationLinuxUsers(int AppID, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetApplicationLinuxUsers(AppID, strquarter);
        }
        public DataSet GetApplicationSQLUsers(int AppID, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetApplicationSQLUsers(AppID, strquarter);
        }
        public DataSet GetApplicationSecGrpUsers(int AppID, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetApplicationSecGrpUsers(AppID, strquarter);
        }


        public DataSet GetApplicationPSIUsers(int AppID, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetApplicationPSILUsers(AppID, strquarter);
        }

        public DataSet GetApplicationOracleUsers(int AppID, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetApplicationOracleUsers(AppID, strquarter);
        }
        public DataSet GetSQLReportsForUser(int AppID, string strUserNm, string strRole, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetDistinctSQLReportsForUser(AppID, strUserNm, strRole, strquarter);
        }
        public DataSet GetLinuxReportsForUser(int AppID, string strUserId, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetDistinctLinuxReportsForUser(AppID, strUserId, strquarter);
        }
        public DataSet GetSecGrpReportsForUser(int AppID, string strUserId, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetDistinctSecGrpReportsForUser(AppID, strUserId, strquarter);
        }
        public DataSet GetOracleReportsForUser(int AppID, string strUserNm, string strRole, string strquarter)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetDistinctOracleReportsForUser(AppID, strUserNm, strRole, strquarter);
        }
        #region GetReportDetails
        public DataSet GetReportDetails(int ReportID, string strReportType, string SelectedQuarter)
        {
            try
            {
                clsDALReports objclsDALReports = new clsDALReports();
                objds = objclsDALReports.GetReportDetails(ReportID, strReportType, SelectedQuarter);
                return objds;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        #endregion


        public DataSet GetAllRecievedReports(string Quarter)
        {
            objclsDALReports = new clsDALReports();
            DataSet ds = objclsDALReports.GetAllRecievedReports(Quarter);
            return ds;
        }

        #region Get Users of a Report
        public DataSet GetReportUsers(int ReportID, string strRepType, string strQuarter, string PreviousQuarter, int ApplicationID, bool IsGlobalApprover, string LoggeInUser, string[] role)
        {
            try
            {
                clsDALReports objclsDALReports = new clsDALReports();
                objds = objclsDALReports.GetReportUsers(ReportID, strRepType, strQuarter, PreviousQuarter, ApplicationID, IsGlobalApprover, LoggeInUser,role);
                return objds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Search Details
        public DataSet GetSearchDetails(int ReportID, string strRepType, string strQuarter, string PreviousQuarter, int ApplicationID, bool IsGlobalApprover, string LoggeInUser, string[] role, Int32 UserID)
        {
            try
            {
                clsDALReports objclsDALReports = new clsDALReports();
                objds = objclsDALReports.GetSearchDetails(ReportID, strRepType, strQuarter, PreviousQuarter, ApplicationID, IsGlobalApprover, LoggeInUser, role, UserID);
                return objds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SignoffUser by Global
        public int  SignOffUsersByGlobalReport(string usersid,string strGroupsid, string signoffstatus, clsEALUser objclsEALApprover, string Quarter,bool IsAdminUpdate,bool IsAdmin,int ReportID,string scope)
        {
            try
            {
                objclsDALReports = new clsDALReports();
              return  objclsDALReports.SignOffUsersByGlobalReport(usersid,strGroupsid, signoffstatus, objclsEALApprover, Quarter, IsAdminUpdate, IsAdmin,ReportID,scope);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SignOffUsersByGlobalReport_SQL(string userNm, string strRole, string strSA, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, int ReportID, string scope)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                return objclsDALReports.SignOffUsersByGlobalReport_SQL(userNm, strRole, strSA, signoffstatus, objclsEALApprover, Quarter, ReportID, scope);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SignOffUsersByGlobalReport_Linux(string userNm, string strRole, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, int ReportID, string scope)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                return objclsDALReports.SignOffUsersByGlobalReport_Linux(userNm, strRole, signoffstatus, objclsEALApprover, Quarter, ReportID, scope);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SignOffUsersByGlobalReport_SecGrp(string userNm, string strRole, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, int ReportID, string scope)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                return objclsDALReports.SignOffUsersByGlobalReport_SecGrp(userNm, strRole, signoffstatus, objclsEALApprover, Quarter, ReportID, scope);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int SignOffUsersByGlobalReport_Ora(string userNm, string strRole, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, int ReportID, string scope)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                return objclsDALReports.SignOffUsersByGlobalReport_Ora(userNm, strRole, signoffstatus, objclsEALApprover, Quarter, ReportID, scope);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SignOffUsersByGlobal(string usersid, string strgroupsid, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, bool IsAdminUpdate, bool IsAdmin)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                objclsDALReports.SignOffUsersByGlobal(usersid, strgroupsid, signoffstatus, objclsEALApprover, Quarter, IsAdminUpdate, IsAdmin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


         #endregion

        public void UpdateISAdmin(string usersid, Int32 ReportID, bool IsAdmin)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                objclsDALReports.UpdateISAdmin(usersid,ReportID,IsAdmin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region SignoffUsersByOther
        public int SignOffUsersByOthers(string scope, Int32 ReportID, string strUserSID,string strGroupSID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID,bool IsAdminUpdate,bool IsAdmin)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                return objclsDALReports.SignOffUsersByOthers(scope, ReportID, strUserSID,strGroupSID, signoffstatus, objclsEALApprover, Quarter, ApplicationID, IsAdminUpdate,IsAdmin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public int SignOffDBUsersByOthers(string scope, Int32 ReportID, string strUserNm, string strDatabase, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string IsSA, string strRepType)
        //{
        //    try
        //    {
        //        objclsDALReports = new clsDALReports();
        //        return objclsDALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, signoffstatus, objclsEALApprover, Quarter, ApplicationID, IsSA, strRepType);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public int SignOffDBUsersByOthers(string scope, Int32 ReportID, string strUserNm, string strDatabase, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string IsSA, string strRepType,string strDBUser_ID)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                return objclsDALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, signoffstatus, objclsEALApprover, Quarter, ApplicationID, IsSA, strRepType, strDBUser_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPrevDBSignoffByAproverName_Reports(string PreviousQuarter, string userNm, string strSA, int RepId, string RepType)
        {
            objclsDALReports = new clsDALReports();
            return objclsDALReports.GetPrevDBSignoffByAproverName_Reports(PreviousQuarter, userNm, strSA, RepId, RepType);
        }
        public string LastStatus_DB(string strUserNm, string strCurrQuarter, string strDBRepType)
        {
            clsDALReports objclsDALReports = new clsDALReports();
            string strLastStatus = objclsDALReports.LastStatus_DB(strUserNm, strCurrQuarter, strDBRepType);
            return strLastStatus;
        }

        //public DataSet UpdateDBResetToPending(string scope, Int32 ReportID, string usernm, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strSA, string strRepType)
        //{
        //    objclsDALReports = new clsDALReports();
        //    return objclsDALReports.UpdateDBResetToPending(scope, ReportID, usernm, objclsEALApprover, Quarter, ApplicationID, strSA, strRepType);
        //}
        public void UpdateDBResetToPending(string scope, Int32 ReportID, string usernm, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strSA, string strRepType, string strDBUser_ID, string ResetBy)
        {
            objclsDALReports = new clsDALReports();
            objclsDALReports.UpdateDBResetToPending(scope, ReportID, usernm, objclsEALApprover, Quarter, ApplicationID, strSA, strRepType, strDBUser_ID, ResetBy);
        }
        public void SignOffUsersByOthersAllAcc(string scope, Int32 ReportID, string strUserSID, string strgroupsid, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                objclsDALReports.SignOffUsersByOthersAllAcc(scope, ReportID, strUserSID, strgroupsid, signoffstatus, objclsEALApprover, Quarter, ApplicationID, IsAdminUpdate, IsAdmin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region to check if user exists for current quarter

        public DataSet CheckIfUserExistsForCurrentQuarter(string strUserSID, string strGroupSID, string scope, int Appid, string strquarter)
        {
            
            clsDALReports objclsDALReports = new clsDALReports();
            DataSet ds = objclsDALReports.CheckIfUserExistsForCurrentQuarter(strUserSID, strGroupSID, scope, Appid, strquarter);
            return ds;
        }

        #endregion
        public void SignOffUsersByOthersAllAppScope(string scope, Int32 ReportID, string strUserSID, string strGroupSID, string signoffstatus, clsEALUser objclsEALApprover,string strCOSID, string Quarter, Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                objclsDALReports.SignOffUsersByOthersAllAppScope(scope, ReportID, strUserSID, strGroupSID, signoffstatus, objclsEALApprover,strCOSID, Quarter, ApplicationID, IsAdminUpdate, IsAdmin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public bool CheckIfReportSubmitted(int repID)
        {
            objclsDALReports = new clsDALReports();
            bool bln = objclsDALReports.CheckIfReportSubmitted(repID);
            return bln;
        }
        public bool CheckIfDBReportSubmitted(int repID, string repType,string strQuarter)
        {
            objclsDALReports = new clsDALReports();
            bool bln = objclsDALReports.CheckIfDBReportSubmitted(repID, repType, strQuarter);
            return bln;
        }

        //Added by Nag
        public DataSet GetPendingApprovalReports(string approverName, string reportType)
        {
            objclsDALReports = new clsDALReports();
            DataSet pr = objclsDALReports.GetPendingApprovalUsers(approverName, reportType);
            return pr;
        }


        //Added by Nag
        public DataSet GetLoadExceptionReports(string reportType)
        {
            objclsDALReports = new clsDALReports();
            DataSet pr = objclsDALReports.GetLoadExceptionReports(reportType);
            return pr;
        }



         //Added by Nag
        public DataSet GetUnmappedSecGrpReport()
        {
            objclsDALReports = new clsDALReports();
            DataSet pr = objclsDALReports.GetUnmappedSecGrpReport();
            return pr;
        }

        //Added by Nag
        public DataSet GetTicketReports(string strQuarter, string reportType)
        {
            objclsDALReports = new clsDALReports();
            DataSet pr = objclsDALReports.GetTicketReports(strQuarter, reportType);
            return pr;
        }



        #region SubmitReport
        public void SubmitReport(Int32 ReportID, clsEALUser objApprover)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                objclsDALReports.SubmitReport(ReportID, objApprover);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SubmitDBReport(Int32 ReportID, clsEALUser objApprover, string quarter, string strrepType)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                objclsDALReports.SubmitDBReport(ReportID, objApprover,quarter,strrepType);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataSet GetApplication(Int32 AppID)
        {
            try
            {
                objclsDALReports = new clsDALReports();
                DataSet ds = objclsDALReports.GetApplication(AppID);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CheckAllDBReports(int AppiD, string strCurrQuarter, string strRepType)
        {
            objclsDALReports = new clsDALReports();
            DataSet ds = objclsDALReports.CheckAllDBReports(AppiD, strCurrQuarter, strRepType);
            return ds;
        }


        #region UpdateComments
        public void UpdateComment(string comment,string scope, string strQuarter,int AppID,string ApproverSID,Int32 ReportID, string SID, string GroupSID)
        {
            try
            {
                clsDALReports objclsDALReports = new clsDALReports();
                objclsDALReports.UpdateComment(comment, scope, strQuarter, AppID, ApproverSID, ReportID, SID, GroupSID);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateDBComment(string comment, string scope, string strQuarter, int AppID, string ApproverAdid, Int32 ReportID, string UserNm, string reportType, string strDBnm)
        //{
        //    try
        //    {
        //        clsDALReports objclsDALReports = new clsDALReports();
        //        objclsDALReports.UpdateDBComment(comment, scope, strQuarter, AppID, ApproverAdid, ReportID, UserNm, reportType, strDBnm);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateDBComment(string comment, string scope, string strQuarter, int AppID, string ApproverAdid, Int32 ReportID, string UserNm, string reportType, string strDBnm,string strDBUser_ID)
        {
            try
            {
                clsDALReports objclsDALReports = new clsDALReports();
                objclsDALReports.UpdateDBComment(comment, scope, strQuarter, AppID, ApproverAdid, ReportID, UserNm, reportType, strDBnm, strDBUser_ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLinuxComment(string comment, string scope, string strQuarter, int AppID, string ApproverAdid, Int32 ServerID, string strUser_ID)
        {
            try
            {
                clsDALReports objclsDALReports = new clsDALReports();
                objclsDALReports.UpdateLinuxComment(comment, scope, strQuarter, AppID, ApproverAdid, ServerID, strUser_ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateSecGrpComment(string comment, string scope, string strQuarter, int AppID, string ApproverAdid, Int32 ServerID, string strUser_ID)
        {
            try
            {
                clsDALReports objclsDALReports = new clsDALReports();
                objclsDALReports.UpdateSecGrpComment(comment, scope, strQuarter, AppID, ApproverAdid, ServerID, strUser_ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetComments
        public string GetComment(Int32 ReportID, string SID, string GroupSID)
        {

                string comment = string.Empty;
                clsDALReports objclsDALReports = new clsDALReports();
                comment = objclsDALReports.GetComment(ReportID, SID, GroupSID);
                return comment;

            
        }
        public string GetDBComment(Int32 ReportID, string UserNm, string strRepType)
        {

            string comment = string.Empty;
            clsDALReports objclsDALReports = new clsDALReports();
            comment = objclsDALReports.GetDBComment(ReportID, UserNm, strRepType);
            return comment;


        }
        public string GetLinuxComment(Int32 RowID)
        {
            string comment = string.Empty;
            clsDALReports objclsDALReports = new clsDALReports();
            comment = objclsDALReports.GetLinuxComment(RowID);
            return comment;
        }
        public string GetSecGrpComment(Int32 RowID)
        {
            string comment = string.Empty;
            clsDALReports objclsDALReports = new clsDALReports();
            comment = objclsDALReports.GetSecGrpComment(RowID);
            return comment;
        }
        #endregion
        public DataSet GetAllAccounts(clsEALRoles objRole, string ApplicationID, string Quarter)
        {
            return new DataSet();
        }
    }
}
