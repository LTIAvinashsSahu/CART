using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CART_EAL;
using System.Collections;
using CART_DAL;
using System.Data.SqlClient;
namespace CART_BAL
{
    public class clsBALApplication
  {
      #region DataFields
      clsDALApplication objclsDALApplication;
      #endregion
      public ArrayList GetAllAppHavingAccess(clsEALUser objclsEalUser, clsEALRoles objclsEALRol)
        {
            return new ArrayList();
        }
      public DataSet GETGlobalApprovers(string strCurrQuarter)
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.GETGlobalApprovers(strCurrQuarter);
          return ds;
      }
      public DataSet GetAdminCOSelectiveApproval(string COSID, string strReportType, string  strRole)
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.GetAdminCOSelectiveApproval(COSID, strReportType, strRole);
          return ds;
      }
      public DataSet CheckAllReports(int AppiD, string strCurrQuarter)
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.CheckAllReports(AppiD, strCurrQuarter);
          return ds;
      }

      public DataTable GetInitiatives()
      {
          objclsDALApplication = new clsDALApplication();
          DataTable ds = objclsDALApplication.GetInitiatives();
          return ds;
      }

      //GetCOFromSelectiveApproval
      public DataSet GetCOFromSelectiveApproval(string strrblCO_GA)
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.GetCOFromSelectiveApproval(strrblCO_GA);
          return ds;
      }

      public DataSet GetOwner(string strrblCO_GA)
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.GetOwner(strrblCO_GA);
          return ds;
      }
      public DataSet GetSecurityGroupMapping()
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.GetSecurityGroupMapping();
          return ds;
      }
      public DataSet GetSecurityGroup()
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.GetSecurityGroup();
          return ds;
      }
      public bool SaveDBApplicationMapping(int AppID, string Database, int serverid)
      {
          objclsDALApplication = new clsDALApplication();
          bool bln = objclsDALApplication.SaveDBApplicationMapping(AppID, Database, serverid);
          return bln;
      }
      public void DeleteAppDatabaseMapping(int Appid, int DatabaseMappingID)
      {
          objclsDALApplication = new clsDALApplication();
          objclsDALApplication.DeleteAppDatabaseMapping(Appid, DatabaseMappingID);

      }

      //public bool CheckIfDBMappingExists(int AppID, int intDBID)
      //{
      //    objclsDALApplication = new clsDALApplication();
      //    bool bln = objclsDALApplication.CheckIfDBMappingExists(AppID, intDBID);
      //    return bln;
      //}
      public bool CheckIfDBServerExists(string strServerNm, int servertypeID)
      {
          objclsDALApplication = new clsDALApplication();
          bool bln = objclsDALApplication.CheckIfDBServerExists(strServerNm, servertypeID); 
          return bln;
      }
      public bool CheckIfDBServerExists1(string strServerNm, int servertypeID)
      {
          objclsDALApplication = new clsDALApplication();
          bool bln = objclsDALApplication.CheckIfDBServerExists1(strServerNm, servertypeID);
          return bln;
      }

      public bool GetCOApplicationCompletion(int AppID, string strCurrQuarter)
      {
          objclsDALApplication = new clsDALApplication();
          bool blnComp = objclsDALApplication.GetApplicationCOCompletion(AppID, strCurrQuarter);
          return blnComp;
      }
      public bool GetCOSignOff(int AppID,string strCOSID, string strCurrQuarter)
      {
          objclsDALApplication = new clsDALApplication();
          bool blnSignoff = objclsDALApplication.GetCOSignOff(AppID,strCOSID, strCurrQuarter);
          return blnSignoff;
      }

      public void UpdateCOSignOff(int AppID, string strCurrQuarter)
      {
          objclsDALApplication = new clsDALApplication();
         objclsDALApplication.UpdateCOSignOff(AppID, strCurrQuarter);
          //return blnSignoff;
      }
      public DataTable GetInitiativeDetails()
      {
          objclsDALApplication = new clsDALApplication();
          DataTable ds = objclsDALApplication.GetInitiativesDetails();
          return ds;
      }
      public DataTable GetAppInitiatives()
      {
          objclsDALApplication = new clsDALApplication();
          DataTable ds = objclsDALApplication.GetAppInitiatives();
          return ds;
      }
      public DataSet GetApproverByApplication(string[] role, clsEALUser objclsEALLoggedInUser, string strCurrentQuarter)
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.GetApproverByApplication(role, objclsEALLoggedInUser, strCurrentQuarter);
          return ds;
      }
      public bool UnlockApprover(string ApprovertoUnlock, int AppID, string strCurrentQuarter,string loggedInUser)
      {
          objclsDALApplication = new clsDALApplication();
          objclsDALApplication.UnlockApprover(ApprovertoUnlock, AppID, strCurrentQuarter, loggedInUser);
          return true;
      }

      public bool UnlockControlOwner(string COTOUnlock, int AppID, string strCurrentQuarter, bool blnCOSignOff, string strLoggedInUser)
      {
          objclsDALApplication = new clsDALApplication();
          objclsDALApplication.UnlockControlOwner(COTOUnlock, AppID, strCurrentQuarter, blnCOSignOff, strLoggedInUser);
          return true;
      }

      

      public bool UnlockAGlobalApprover(string Admin, string strCurrQuarter, string loggedInUser)
      {
          objclsDALApplication = new clsDALApplication();
          objclsDALApplication.UnlockGlobalApprover(Admin, strCurrQuarter, loggedInUser);
          return true;
      }


      public DataSet GetAllApproversByApplication(string[] role,clsEALUser objclsEALUserApp)
      {
          objclsDALApplication = new clsDALApplication();
          DataSet ds = objclsDALApplication.GetAllApproversByApplication(role, objclsEALUserApp);
          return ds;
      }
      #region GetApplicationCompletionStatus
      public bool GetApplicationCompletionStatus(string role, clsEALUser objclsEALUser, string Quarter,Int32 ApplicationID)
        {
            try
            {
                objclsDALApplication = new clsDALApplication();
                bool status = objclsDALApplication.GetApplicationCompletionStatus(role, objclsEALUser, Quarter, ApplicationID);
                return status;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
      #endregion


      #region
      public void UpdateCompletionStatus(string role, clsEALUser objclsEALApprover, Int32 ApplicationiD, string Quarter, bool status )
      {
          try
          {
              objclsDALApplication = new clsDALApplication();
              objclsDALApplication.updateCompletionStatus(role, objclsEALApprover, ApplicationiD, Quarter, status);
            
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public void UpdateCOCompletionStatus(string role, clsEALUser objclsEALApprover, Int32 ApplicationiD, string Quarter, bool status, bool COSignOff)
      {
          try
          {
              objclsDALApplication = new clsDALApplication();
              objclsDALApplication.updateCOCompletionStatus(role, objclsEALApprover, ApplicationiD, Quarter, status, COSignOff);

          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      
      #endregion
      public DataSet GetAllApproversByApplication(clsEALUser objClsEALUser)
        {
            return new DataSet();
        }
        public bool GetAppCompletionStatus(clsEALUser objClsEALUser, string Quarter)
        {
            return true;
        }
        #region ApplicationMApping
        public bool CheckIfMappingExists(int AppID,int intShare)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.CheckIfMappingExists(AppID, intShare);
            return bln;
        }

        public DataSet GetServerLists()
        {
            objclsDALApplication = new clsDALApplication();
            DataSet ds = objclsDALApplication.GetServerLists();
            return ds;
        }

        public DataSet GetDBServerLists()
        {
            objclsDALApplication = new clsDALApplication();
            DataSet ds = objclsDALApplication.GetDBServerLists();
            return ds;
        }
        public DataSet GetDBServerTypes()
        {
            objclsDALApplication = new clsDALApplication();
            DataSet ds = objclsDALApplication.GetDBServerTypes();
            return ds;
        }

        public bool SaveApplicationMapping(int AppID, int Share, string StrUserADID)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveApplicationMapping(AppID, Share, StrUserADID);
            return bln;
        }

        public bool  SaveLinuxMapping(int AppID,string strServerName,string StrUserADID)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveLinuxMapping(AppID, strServerName, StrUserADID);
            return bln;
        }
        public bool SaveGroupMapping(int AppID, string strGroupName, string strdomain, string StrUserADID)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveGroupMapping(AppID, strGroupName, strdomain, StrUserADID);
            return bln;
        }

        public DataSet GetAppShareMapping()
        {
            objclsDALApplication = new clsDALApplication();
            DataSet ds = objclsDALApplication.GetAppShareMapping();
            return ds;
        }
        public DataSet GetAllApplications()
        {
            objclsDALApplication = new clsDALApplication();
           DataSet ds= objclsDALApplication.GetAllApplications();
           return ds;
        }
        public DataSet GetServerTypes()
        {
            objclsDALApplication = new clsDALApplication();
            DataSet ds = objclsDALApplication.GetServerTypes();
            return ds;
        }

        public DataSet GetApplicationsByAppName( string AppName)
        {
            objclsDALApplication = new clsDALApplication();
            DataSet ds = objclsDALApplication.GetApplicationsByAppName(AppName);
           return ds;
        }

      
        public DataSet GetAllShares()
        {
            objclsDALApplication = new clsDALApplication();
            DataSet ds = objclsDALApplication.GetAllShares();
            return ds;
        }
        #endregion
        public bool SaveApplicationApproverMapping(ArrayList arrclsEALApprover, int AppID, string strCurrentQuarter)
        {
            clsDALApplication objclsDALApp = new clsDALApplication();
            bool bln = objclsDALApp.SaveApplicationApproverMapping(arrclsEALApprover, AppID, strCurrentQuarter);
            return bln;
        }

        public bool SaveApplicationDetails(string role, clsEALUser objclsAppOwner, string AppName, string strInitiatives, bool admin, bool MultipleApprovals, bool ExcludeGA, bool UnlockApp, string StrUserADID,string procycle, string quarters)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveApplicationDetails(role, objclsAppOwner, AppName, strInitiatives, admin, MultipleApprovals, ExcludeGA, UnlockApp, StrUserADID, procycle, quarters);
            
            return bln;
        }
        public bool SaveGroupDetails(string GroupName, string OwnerType, string OwnerName, string UserADID, bool NoRestrictions, bool AllControlOwners, string CreatedBy)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveGroupDetails( GroupName,  OwnerType,  OwnerName,  UserADID,  NoRestrictions,  AllControlOwners,  CreatedBy);
            return bln;
        }
        public bool CheckGroupMapping(string GroupName)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.CheckGroupMapping(GroupName);
            return bln;
        }
        public bool CheckAllCO(string GroupName)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.CheckAllCO(GroupName);
            return bln;
        }
        public bool CheckNoRestrictions(string GroupName)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.CheckNoRestrictions(GroupName);
            return bln;
        }
        public bool CheckOwnerMapping(string GroupName, string strOwner, string strOwnerType)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.CheckOwnerMapping(GroupName, strOwner, strOwnerType);
            return bln;
        }
        public bool SaveApplicationStatus(clsEALUser objclsAppOwner, int AppId, string strQuarter)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveApplicationStatus(objclsAppOwner, AppId, strQuarter);
            
            return bln;
        }
      

        public DataSet GetAllApplicationList(string strUsersid, string[] strRole, string strQuarter)
        {
            DataSet ds = null;

            try
            {
                clsDALApplication objclsDALApplication = new clsDALApplication();
                ds = objclsDALApplication.GetAllApplicationList(strUsersid,strRole,strQuarter);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertAppShareMapping(string Appid, string ShareID)
        {
            return true;
        }
        public bool UpdateAppShareMapping(int Appid, int ShareID, int rowid)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.UpdateAppShareMapping(Appid, ShareID, rowid);
            return bln;
        }


        public bool UpdateServerShare(string strShareNm, int serverid,  int ShareID)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.UpdateServerShare(strShareNm, serverid, ShareID);
            return bln;
        }

        public bool UpdateAppDetails(string strRole, int Appid, string AppNm, string Initiative, clsEALUser objControlOwner, bool Admin, bool MultipleApprovals, bool flagExclude, bool flagUnlockApp, string Proccycle, string quarter)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.UpdateApplicationDetails(strRole, Appid, AppNm, Initiative, objControlOwner, Admin, MultipleApprovals, flagExclude, flagUnlockApp, Proccycle, quarter);
            return bln;
        }
        public void DeleteAppShareMapping(int Appid, int ShareID,string StrUserADID)
        {
            objclsDALApplication = new clsDALApplication();
            objclsDALApplication.DeleteAppShareMapping(Appid, ShareID, StrUserADID);
          
        }

        public void UpdateLinuxServerMapping(int serverid, int appid, string StrUserADID, string strservername)
        {
            objclsDALApplication = new clsDALApplication();
            objclsDALApplication.UpdateLinuxServerMapping(serverid, appid, StrUserADID,strservername);

        }
        public void UpdateGroupMapping(int serverid, int appid, string StrUserADID, string strservername, string domain)
        {
            objclsDALApplication = new clsDALApplication();
            objclsDALApplication.UpdateGroupMapping(serverid, appid, StrUserADID, strservername, domain);

        }

        public void DeleteGroupMapping(int GroupID, string strOwnerType, string strOwnerNm, string strNoRestrictions, string strAllControlOwners)
        {
            objclsDALApplication = new clsDALApplication();
            objclsDALApplication.DeleteGroupMapping(GroupID, strOwnerType, strOwnerNm, strNoRestrictions, strAllControlOwners);

        }
        public void DeleteApproverMapping(ArrayList arrApprover,int Appid)
        {
            objclsDALApplication = new clsDALApplication();
            objclsDALApplication.DeleteApproverMapping(arrApprover,Appid);

        }
        public bool InsertApplicationDetails(string AppName, string Initiative, clsEALUser objControlOwner)
        {
            return true;
        }
        public bool UpdateApplicationDetails(string AppID, string AppName, string Initiative, clsEALUser objControlOwner)
        {
            return true;
        }
        public bool CheckIfApplicationExists(string AppNm)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.CheckIfApplicationExists(AppNm);
            return bln;
        }
        public bool CheckIfApplicationExistsForUpdate(string AppNm, string Initiatives, clsEALUser objControlOwner)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.CheckIfApplicationExistsForUpdate(AppNm, Initiatives, objControlOwner);
            return bln;
        }

        public bool CheckIfServerExists(string strServer)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.CheckIfServerExists(strServer);
            return bln;
        }

        public bool DeleteApplicationDetails(int AppID, string ModifiedBy)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.DeleteApplicationDetails(AppID, ModifiedBy);
            return bln;
        }

        public bool DeleteInitiative(int Initiative)
        {
            objclsDALApplication = new clsDALApplication();
            bool flag = objclsDALApplication.DeleteInitiative(Initiative);
            return flag;
        }
        public void DeleteServerDB(int ServerID)
        {
            objclsDALApplication = new clsDALApplication();
            objclsDALApplication.DeleteServerDB(ServerID);
            //return flag;
        }
        public bool DeleteServer(int ServerID, string ModifiedBy)
        {
            objclsDALApplication = new clsDALApplication();
            bool flag = objclsDALApplication.DeleteServer(ServerID, ModifiedBy);
            return flag;
        }
        public bool UpdateInitiatives(int InitiaveID, bool blnUpdate, bool blnVisible)
        {
            objclsDALApplication = new clsDALApplication();
           bool bln= objclsDALApplication.UpdateInitiatives(InitiaveID, blnUpdate, blnVisible);
            return bln;
        }
        public bool UpdateServer(int ServerID, string strServerName)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.UpdateServer(ServerID, strServerName);
            return bln;
        }
        public bool UpdateServerStatus( string strServerName)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.UpdateServerStatus( strServerName);
            return bln;
        }

        public bool UpdateDBServer(int ServerID, string strServerName,int intServerType_ID)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.UpdateDBServer(ServerID, strServerName, intServerType_ID);
            return bln;
        }

        public bool UpdateDBServerStatus(string strServerName,int intServerType_ID)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.UpdateDBServerStatus(strServerName,intServerType_ID);
            return bln;
        }

        public bool SaveInitiative(string InitiativeNm, bool blnUpdate, bool blnVisible)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveInitiative(InitiativeNm, blnUpdate, blnVisible);
            return bln;
        }

        public bool SaveServer(string strServer)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveServer(strServer);
            return bln;
        }
        public bool SaveDBServer(string strServer, int servertypeid)
        {
            objclsDALApplication = new clsDALApplication();
            bool bln = objclsDALApplication.SaveDBServer(strServer, servertypeid);
            return bln;
        }
        public string GetAllApplicationStatus(string[] strRole, string strQaurter, int intAppId)
        {
            objclsDALApplication = new clsDALApplication();
            string strStatus = objclsDALApplication.GetAllApplicationStatus(strRole, strQaurter, intAppId);
            return strStatus;
        }


        public DataTable GetUnlockApprover(string ApproverADID)
        {
            clsDALApplication objclsDALApplication = new clsDALApplication();
            return (objclsDALApplication.GetUnlockApprover(ApproverADID));
        }
        public DataTable GetUnlockCO(string COADID) 
        {
            clsDALApplication objclsDALApplication = new clsDALApplication();
            return (objclsDALApplication.GetUnlockCO(COADID));
        }
        public bool CheckApprover(int AppID, string strName)
        {
            objclsDALApplication = new clsDALApplication();
            bool flag=objclsDALApplication.CheckApprover(AppID,strName);
            return flag;
        }

         
    }
}
