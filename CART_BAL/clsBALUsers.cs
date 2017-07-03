using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CART_EAL;
using CART_DAL;
using System.Data;

namespace CART_BAL
{
    public class clsBALUsers
    {
        #region Data Members
        clsDALUsers objclsDALUsers;
        #endregion

        #region GetCurrentUser

        //code added by suman
        public DataSet GetLockOut()
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetLockOut();
        }

        public DataSet GetApplicationUsers(int AppID, string strquarter)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetApplicationUsers(AppID,strquarter);
        }

        public DataSet FetchAllLinuxAccountUser(string strQuarter, string strPreviousQuarter, int intAppID, string[] role)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAllLinuxAccountUser(strQuarter, strPreviousQuarter, intAppID, role);
            return ds;
        }
        public DataSet FetchAllSecGrpAccountUser(string strQuarter, string strPreviousQuarter, int intAppID, string[] role)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAllSecGrpAccountUser(strQuarter, strPreviousQuarter, intAppID, role);
            return ds;
        }

        public DataSet FetchAllSQLAccountUser(string strQuarter, string strPreviousQuarter, int intAppID, string[] role)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAllSQLAccountUser(strQuarter, strPreviousQuarter, intAppID, role);
            return ds;
        }
        public void UpdateAccountComment(string comment, string quarter, string loggedinAdid, string strUserName, string strRole, string strReptype, string strDB, string Server)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateAccountComment(comment, quarter, loggedinAdid, strUserName, strRole,strReptype, strDB,Server);
        }
        public void UpdatelinuxAccountComment(string comment, string quarter, string loggedinAdid, string strUserID, string Server)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdatelinuxAccountComment(comment, quarter, loggedinAdid, strUserID, Server);
        }
        public void UpdateSecGrpAccountComment(string comment, string quarter, string loggedinAdid, string strUserID, string Server)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateSecGrpAccountComment(comment, quarter, loggedinAdid, strUserID, Server);
        }
        public void UpdatePSIAccountComment(string comment, string quarter, string strUserName, string strUserID )
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdatePSIAccountComment(comment, quarter, strUserName, strUserID);
        }
        public DataSet FetchAllORACLEAccountUser(string strQuarter, string strPreviousQuarter, int intAppID, string[] role)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAllORACLEAccountUser(strQuarter, strPreviousQuarter, intAppID, role);
            return ds;
        }
        public string GetAccountComment(string strUserNm, string strRole, string strRepType, string strQuarter, string strDB, string strServer)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strComment = objclsDALUsers.GetAccountComment(strUserNm,strRole, strRepType, strQuarter, strDB, strServer);
            return strComment;
        }
        public string GetAccountCommentLinux(string strUserID, string strRepType, string strQuarter, string strServer)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strComment = objclsDALUsers.GetAccountCommentLinux(strUserID, strRepType, strQuarter, strServer);
            return strComment;
        }
        public string GetPSIComment(string strUserNm , string strQuarter  )
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strComment = objclsDALUsers.GetPSIComment(strUserNm,   strQuarter);
            return strComment;
        }
        public DataSet GetReportsForUser(int AppID, string strUserSID,string strGroupSID, string strquarter)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetDistinctReportsForUser(AppID, strUserSID, strGroupSID, strquarter);
        }

        public void SignOffUsersByOthersAllAcc_SQL(string scope, Int32 ReportID, string strUserName, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin, string strDBType, string strRole, string strDB, string strSever)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByOthersAllAcc_SQL(scope, ReportID, strUserName, signoffstatus, objclsEALApprover, Quarter, ApplicationID, IsAdminUpdate, IsAdmin, strDBType, strRole,strDB,strSever);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SignOffUsersByOthersAllAcc_SQL_AllApp(string scope, Int32 ReportID, string strUserSID, string strUserName, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin, string strDBType, string strCOSID,string strRole, string strDB, string strServer)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, ReportID, strUserSID, strUserName, signoffstatus, objclsEALApprover, Quarter, ApplicationID, IsAdminUpdate, IsAdmin, strDBType, strCOSID, strRole,strDB,strServer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SignOffUsersByOthersAllAcc_Linux_AllApp(string scope, string strUserID,  string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID,  string strCOSID,  string strServer)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByOthersAllAcc_Linux_AllApp(scope, strUserID, signoffstatus, objclsEALApprover, Quarter, ApplicationID, strCOSID, strServer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByOthersAllAcc_Linux(string scope, Int32 Reportid, string strUserID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strServer)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByOthersAllAcc_Linux(scope, Reportid, strUserID, signoffstatus, objclsEALApprover, Quarter, ApplicationID, strServer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByOthersAllAcc_Linux_New(string scope, Int32 Reportid, string strUserID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strServer , string loginstatus)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByOthersAllAcc_Linux_New(scope, Reportid, strUserID, signoffstatus, objclsEALApprover, Quarter, ApplicationID, strServer, loginstatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByOthersAllAcc_SecGrp_AllApp(string scope, string strUserID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strCOSID, string strServer)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByOthersAllAcc_SecGrp_AllApp(scope, strUserID, signoffstatus, objclsEALApprover, Quarter, ApplicationID, strCOSID, strServer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SignOffUsersByOthersAllAcc_SecGrp(string scope, Int32 Reportid, string strUserID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strServer)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByOthersAllAcc_SecGrp(scope, Reportid, strUserID, signoffstatus, objclsEALApprover, Quarter, ApplicationID, strServer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByGlobal_SQL(string uName, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, bool IsAdminUpdate, bool IsAdmin, string strDBType, string strRole, string strDB, string strServer)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByGlobal_SQL(uName, signoffstatus, objclsEALApprover, Quarter, IsAdminUpdate, IsAdmin, strDBType, strRole, strDB, strServer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByGlobal_Linux(string uID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, string strServer, string loginstatus, string strgroup)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByGlobal_Linux(uID, signoffstatus, objclsEALApprover, Quarter, strServer, loginstatus, strgroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByGlobal_SecGrp(string uID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, string strServer)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffUsersByGlobal_SecGrp(uID, signoffstatus, objclsEALApprover, Quarter, strServer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffPSIUsers(string uName, string strStatus_PSI, clsEALUser objclsEALApprover, string strQuarter, string strUserID)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                objclsDALUsers.SignOffPSIUsers(uName, strStatus_PSI, objclsEALApprover, strQuarter, strUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSignoffByAproverName(string PreviousQuarter, string UserSID, string GroupSID)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetSignoffByAproverName(PreviousQuarter,UserSID,GroupSID);
        }

        public DataSet GetSignoffByAproverNameDB(string PreviousQuarter, string UserSID, string strReportType)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetSignoffByAproverNameDB(PreviousQuarter, UserSID, strReportType);
        }

        public DataSet GetSignoffByAproverNamelinux(string PreviousQuarter, string UserID, string loginstatus)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetSignoffByAproverNameLinux(PreviousQuarter, UserID, loginstatus);
        }
        public DataSet GetSignoffByAproverNameSecGrp(string PreviousQuarter, string UserID)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetSignoffByAproverNameSecGrp(PreviousQuarter, UserID);
        }

        public DataSet GetSignoffByAproverNamePSI(string PreviousQuarter, string UserName, string strCurrQuarter )
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetSignoffByAproverNamePSI(PreviousQuarter, UserName, strCurrQuarter);
        }

        public DataSet GetSignoffByAproverName_Reports(string PreviousQuarter,string userSID, string groupsid, int RepId)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetSignoffByAproverName_Reports(PreviousQuarter,userSID, groupsid, RepId);
        }
        public DataSet UpdateLockout(string sid, string status)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateLockout(sid, status);

        }
        public DataSet UpdateEmailBody(string GroupID, string Body)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateEmailBody(GroupID,Body);

        }

        /// <All Reports>

        public DataSet UpdateIsReportSubmitted(int ReportId)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateIsReportSubmitted(ReportId);
        }
        public DataSet GetReportStatusPending(int ReportId, string repType,string strQuarter)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetReportStatusPending(ReportId, repType, strQuarter);
        }

        public DataSet GetAccountStatusNoAdmin(int ReportId)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetAccountStatusNoAdmin(ReportId);
        }
        /// </All Reports>
        

        /// <Report and AllAccounts>

        public DataSet GetMultipleApprovals(int ApplicationID, clsEALUser objclsEALApprover)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.GetMultipleApprovals(ApplicationID,objclsEALApprover );
        }

        public DataSet UpdateResetToPendingAllAccounts(string scope, Int32 ReportID, string usersid, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strRight, string GroupSID, string ResetBy)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateResetToPendingAllAccounts(scope, ReportID, usersid, objclsEALApprover, Quarter, ApplicationID, strRight, GroupSID, ResetBy);
        }

        public DataSet UpdateResetToPendingAllSQLAccounts(string scopereset_SQL, clsEALUser objclsEALApprover, string strQuarter, int intAppId, string strSQLUserName, string strDBType, string strDatabase, string strRole, string strServer)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateResetToPendingAllSQLAccounts(scopereset_SQL, objclsEALApprover, strQuarter, intAppId, strSQLUserName, strDBType, strDatabase, strRole, strServer);
        }

        public DataSet UpdateResetToPendingAllLinuxAccounts(string scopereset_Linux, clsEALUser objclsEALApprover, string strQuarter, int intAppId, string strUserID, string strServer, string strloginstatus, string strGroup)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateResetToPendingAllLinuxAccounts(scopereset_Linux, objclsEALApprover, strQuarter, intAppId, strUserID, strServer, strloginstatus, strGroup);
        }
        public DataSet UpdateResetToPendingAllSecGrpAccounts(string scopereset_Linux, clsEALUser objclsEALApprover, string strQuarter, int intAppId, string strUserID, string strServer)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateResetToPendingAllSecGrpAccounts(scopereset_Linux, objclsEALApprover, strQuarter, intAppId, strUserID, strServer);
        }
        public DataSet UpdateResetToPendingPSIAccounts(string strQuarter,  clsEALUser objclsEALApprover, int intAppId, string strPSIUser, string strUserID)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateResetToPendingPSIAccounts(strQuarter, objclsEALApprover, intAppId, strPSIUser, strUserID);
        }

        public DataSet UpdateResetToPending(string scope, Int32 ReportID, string usersid, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string groupSid, string strRight, string ResetBy)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateResetToPending(scope, ReportID, usersid, objclsEALApprover, Quarter, ApplicationID, groupSid, strRight, ResetBy);
        }
        public DataSet UpdateReset_AdminRights(string scope, Int32 ReportID, string usersid, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string groupSid, string strRight)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateReset_AdminRights(scope, ReportID, usersid, objclsEALApprover, Quarter, ApplicationID, groupSid, strRight);
        }
        /// </Report>
       /// <returns></returns>

        public DataSet DeleteScheduleMail(string groupID, string Track)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.DeleteScheduleMail(groupID, Track);
        }
        public DataSet UpdateScheduleMail(string groupID, DateTime Date, string Subject, string Body, string Track)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateScheduleMail( groupID,  Date,  Subject,  Body,  Track);
        }
        public DataSet UpdateUserScheduleMail(string UserName, string Track, string groupID)
        {
            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.UpdateUserScheduleMail(UserName, Track, groupID);
        }
        public DataTable GroupSchedule()
        {
              objclsDALUsers = new clsDALUsers();
              return objclsDALUsers.GroupSchedule();
        }
        public DataSet Delete_RoleWiseUser(int RoleMappingId, string Track)
        {

            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.Delete_RoleWiseUser(RoleMappingId, Track);
        }
        public DataSet InsertDelete_RoleWiseUser(int roleID, string UserName, string UserADID, string UserEmail, string Track)
        {

            objclsDALUsers = new clsDALUsers();
            return objclsDALUsers.InsertDelete_RoleWiseUser(roleID,UserName,UserADID,UserEmail,Track);

        }

        public DataSet InsertDeleteScheduleMail(int RoleID,string groupID , string UserName, string UserADID, string UserEmail, string Track, DateTime Date, string Subject, string Body)
        {
             objclsDALUsers = new clsDALUsers();
             return objclsDALUsers.InsertDeleteScheduleMail(RoleID,groupID, UserName, UserADID, UserEmail, Track, Date, Subject, Body);
        }
        //code end here
        public string[] GetCurrentUserRole(clsEALUser objclsEALUser)
        {
             objclsDALUsers = new clsDALUsers();
                string[] role = objclsDALUsers.GetCurrentUserRole(objclsEALUser);
                return role;
            
        }

        public void UpdateTicketStatus(int intAppID, string RITMNo, string strUSID, string strGroupSID, string strUName, string strQuarterSelected)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatus(intAppID, RITMNo, strUSID, strGroupSID, strUName, strQuarterSelected);
        }

        

        public void UpdateTicketStatusSQL(int intAppID, string RITMNo, string strUName, string strURole, string strQuarterSelected)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusSQL(intAppID, RITMNo, strUName, strURole, strQuarterSelected);
        }

        public void UpdateTicketStatusOracle(int intAppID, string RITMNo, string strUName, string strURole, string strQuarterSelected)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusOracle(intAppID, RITMNo, strUName, strURole, strQuarterSelected);
        }

        public void UpdateTicketStatusLinux(int intAppID, string RITMNo, string strUName, string strQuarterSelected)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusLinux(intAppID, RITMNo, strUName, strQuarterSelected);
        }

        public void UpdateTicketStatusSecGrp(int intAppID, string RITMNo, string strUName, string strQuarterSelected)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusSecGrp(intAppID, RITMNo, strUName, strQuarterSelected);
        }

        public void UpdateTicketStatusPSI(string strUName, string RITMNo, string strQuarterSelected)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusPSI(strUName, RITMNo, strQuarterSelected);
        }

        public void UpdateTicketStatusSecGrpIndividualRep(string lblServer, string RITMNo, string lblUserName, string strSelectedQuarter)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusSecGrpIndividualRep(lblServer, RITMNo, lblUserName, strSelectedQuarter);
        }

        public void UpdateTicketStatusIndividualRep(int ReportID, string lblAccountName, string lblUserSID, string strSelectedQuarter, string RITMNo)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusIndividualRep(ReportID, lblAccountName, lblUserSID, strSelectedQuarter, RITMNo);
        }

        public void UpdateTicketStatusSQLIndividualRep(int ReportID, string lblAccountName, string lblServer, string strSelectedQuarter, string RITMNo)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusSQLIndividualRep(ReportID, lblAccountName,  lblServer, strSelectedQuarter, RITMNo);
        }

        public void UpdateTicketStatusOracleIndividualRep(int ReportID,  string lblServerNm, string lblUserName, string strSelectedQuarter, string RITMNo)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusOracleIndividualRep(ReportID, lblServerNm, lblUserName, strSelectedQuarter, RITMNo);
        }

        public void UpdateTicketStatusLinuxIndividualRep(int ReportID,string lblUserName,string lblServer,string strSelectedQuarter,string RITMNo)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateTicketStatusLinuxIndividualRep(ReportID, lblUserName, lblServer, strSelectedQuarter, RITMNo);
        }

        public string[] ApproverOrCO(clsEALUser objclsEALUser)
        {
            objclsDALUsers = new clsDALUsers();
            string[] role = objclsDALUsers.ApproverOrCO(objclsEALUser);
            return role;

        }
        public bool CheckIfCo(string loggedInUserSID)
        {
            objclsDALUsers = new clsDALUsers();
            bool IsCO = objclsDALUsers.CheckIFCO(loggedInUserSID);
            return IsCO;

        }

        #endregion

       

        #region GetUserRoleBYApplication
        public string GetUserRoleBYApplication(clsEALUser objclsEALUser,int ApplicationID)
        {
            try
            {
                objclsDALUsers = new clsDALUsers();
                string role = objclsDALUsers.GetUserRoleByApplication(objclsEALUser, ApplicationID);
                return role;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public DataSet FetchAllUser(string[] role, int intAppID, string strQuarter, string strPreviousQuarter, string strLoggedInUserName)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAllUser(role, intAppID, strQuarter, strPreviousQuarter, strLoggedInUserName);
            return ds;
        }
        public DataSet FetchPSIUser(string[] role, int intAppID, string strQuarter, string strPreviousQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchPSIUser(role, intAppID, strQuarter, strPreviousQuarter);
            return ds;
        }

        public void UpdateStatus(string strUserSID, int repid, string scope, string strQuarter, int ApplicationID, clsEALUser objclsEALLoggedInUser, string strSignoffStatus)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateStatus(strUserSID, repid, scope, strQuarter, ApplicationID, objclsEALLoggedInUser, strSignoffStatus);
        }

        public void UpdateSQLStatus(string strUserSID, int repid, string scope, string strQuarter, int ApplicationID, clsEALUser objclsEALLoggedInUser, string strSignoffStatus, string strDBType,string strUserName,string strDatabase)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateSQLStatus(strUserSID, repid, scope, strQuarter, ApplicationID, objclsEALLoggedInUser, strSignoffStatus, strDBType, strUserName, strDatabase);
        }

        public void InsertBMC(string scope, string strQuarter, Int32 ReportID, clsEALUser objclsEALoggedInUser, Int32 ApplicationID, string usersid, string strUserNm, string groupSid, bool IsRemoved)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.InsertBMCTable( scope, strQuarter, ReportID, objclsEALoggedInUser, ApplicationID, usersid, strUserNm, groupSid, IsRemoved);
        }

        public void UpdateStatus_Reports(string strUserSID, int repid, string scope, string strQuarter, int ApplicationID, clsEALUser objclsEALLoggedInUser, string strSignoffStatus, string strGroupSID)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateStatus_Reports(strUserSID, repid, scope, strQuarter, ApplicationID, objclsEALLoggedInUser, strSignoffStatus, strGroupSID);
        }
        public DataSet FetchAssignedUser(string strApproverAdid, int intAppID, string strQuarter,string strprevquarter,string strCOSID,string strScope,string strRepType)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAssignedUser(strApproverAdid, intAppID, strQuarter, strprevquarter,strCOSID, strScope,strRepType);
            return ds;
        }

        public DataSet FetchAssignedUser_DB(string strApproverAdid, int intAppID, string strQuarter, string strprevquarter, string strCOSID, string strScope,string strReportType)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAssignedUser_DB(strApproverAdid, intAppID, strQuarter, strprevquarter, strCOSID, strScope, strReportType);
            return ds;
        }

        public DataSet FetchAssignedUser_Linux(string strApproverAdid, int intAppID, string strQuarter, string strprevquarter, string strCOSID, string strScope, string strReportType)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAssignedUser_Linux(strApproverAdid, intAppID, strQuarter, strprevquarter, strCOSID, strScope, strReportType);
            return ds;
        }
        public DataSet FetchAssignedUser_PSI_DB(string strApproverAdid, int intAppID, string strQuarter, string strprevquarter, string strCOSID, string strScope, string strReportType)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchAssignedUser_PSI_DB(strApproverAdid, intAppID, strQuarter, strprevquarter, strCOSID, strScope, strReportType);
            return ds;
        }
        public string LastApprovers(string strUserSID,string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastApprover = objclsDALUsers.LastApprovedBy(strUserSID, strCurrQuarter);
            return strLastApprover;
        }
        public DataSet LastApprovedByDetails(string strUserSID, string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.LastApprovedByDetails(strUserSID, strCurrQuarter);
            return ds;
        }

        public string LastSQLApprovers(string strUserSID, string strCurrQuarter, string strDBType)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastApprover = objclsDALUsers.LastSQLApprovedBy(strUserSID, strCurrQuarter, strDBType);
            return strLastApprover;
        }

        public DataSet LastSQLApproversDetails(string strUserSID, string strCurrQuarter, string strDBType)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.LastSQLApprovedByDetails(strUserSID, strCurrQuarter, strDBType);
            return ds;
        }

        public string LastLinuxApprovers(string strUserSID, string strCurrQuarter, string strDBType, string loginstatus)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastApprover = objclsDALUsers.LastLinuxApprovedBy(strUserSID, strCurrQuarter, strDBType, loginstatus);
            return strLastApprover;
        }

        public string LastPSIApprovers(string strUserName, string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastApprover = objclsDALUsers.LastPSIApprovedBy(strUserName, strCurrQuarter);
            return strLastApprover;
        }

        public string LastApprovers_ADSec(string strUSerSID, string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastApprover = objclsDALUsers.LastApprovedBy_ADSec(strUSerSID, strCurrQuarter);
            return strLastApprover;
        }

        public DataSet LastApprovers_ADSecDetails(string strUSerSID, string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.LastApprovedBy_ADSecDetails(strUSerSID, strCurrQuarter);
            return ds;
        }

        public string LastStatus(string strUserSID, string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastStatus = objclsDALUsers.LastStatus(strUserSID, strCurrQuarter);
            return strLastStatus;
        }

        public string LastStatus_SQL(string strUserSID, string strRole,string strDB, string strServer, string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastStatus = objclsDALUsers.LastStatus_SQL(strUserSID,strRole,strDB,strServer, strCurrQuarter);
            return strLastStatus;
        }

        public string LastStatus_Linux(string strUserID, string strServer, string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastStatus = objclsDALUsers.LastStatus_Linux(strUserID, strServer, strCurrQuarter);
            return strLastStatus;
        }
        public string LastStatus_SecGrp(string strUserID, string strServer, string strCurrQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastStatus = objclsDALUsers.LastStatus_SecGrp(strUserID, strServer, strCurrQuarter);
            return strLastStatus;
        }
        public string LastStatus_PSI(string LoggedInUserName, string strQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            string strLastStatus = objclsDALUsers.LastStatus_PSI(LoggedInUserName, strQuarter);
            return strLastStatus;
        }
        public DataSet GetNewUsers(string strCurrentQuarter, string strPreviousQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.GetNewUsers(strCurrentQuarter, strPreviousQuarter);
            return ds;
        }

        public bool CheckRole(string strRole)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            bool blnExists = objclsDALUsers.CheckRole(strRole);
            return blnExists;
        }


        public DataSet GetRoles()
        {
            objclsDALUsers = new clsDALUsers();
            DataSet ds= objclsDALUsers.GetRoles();
            return ds;
        }

        public string GetAdminMailIDs()
        {
            objclsDALUsers = new clsDALUsers();
            string strMailID = objclsDALUsers.GetAdminMailIDs();
            return strMailID;
        }

        //GetUserDetails
        public DataSet GetUserDetails()
        {
            objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.GetUserDetails();
            return ds;
        }
        public bool SaveUserRole(clsEALUser objclsUser, string userRole, string StrUserADID)
        {
            objclsDALUsers = new clsDALUsers();
            bool bln = objclsDALUsers.SaveUserRole(objclsUser, userRole, StrUserADID);
            return bln;
        }
        public bool InsertNewUser(clsEALUser user, clsEALRoles role)
        {
            return true;
        }
        public bool UpdateUser(clsEALUser user, string strUserRole,string modifiedby)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.UpdateUser(user, strUserRole, modifiedby);
            return true;
        }
        public bool DeleteUser(clsEALUser user,string modifiedby)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.DeleteUser(user, modifiedby);
            return true;
        }
        public bool DeleteUserFromDb(string userADID)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.DeleteUserFromDb(userADID);
            return true;
        }
        public bool DeleteApproverFromDb(string userADID)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.DeleteApproverFromDb(userADID);
            return true;
        }
        public bool DeleteCOFromDb(string userADID)
        {
            objclsDALUsers = new clsDALUsers();
            objclsDALUsers.DeleteCOFromDb(userADID);
            return true;
        }
        public DataSet FetchReportData(string Type, string strPreviousQuarter, string strQuarter)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            DataSet ds = objclsDALUsers.FetchReportData(Type, strPreviousQuarter, strQuarter);
            return ds;
        }

        public void ADUsersAdd(string mygroup, string childitems)
        {
            clsDALUsers objclsDALUsers = new clsDALUsers();
            objclsDALUsers.ADUsersAdd(mygroup, childitems);
        }
    }
}
