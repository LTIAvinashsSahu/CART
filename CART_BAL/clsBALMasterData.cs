using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CART_DAL;
using System.Data;
using System.Data.SqlClient;

namespace CART_BAL
{
    public class clsBALMasterData
    {

        clsDALMasterData objclsDALMasterData;
        public ArrayList FetchMasterData(string Key)
        {
            return new ArrayList();
        }
        public bool InsertNewInitiative(string InitiativeName, bool update, bool VisibleToAuditor)
        {
            return true;
        }
        public DataSet GetShareLists()
        {
            objclsDALMasterData = new clsDALMasterData();
            DataSet ds = objclsDALMasterData.GetShareLists();
            return ds;

        }

        public DataSet GetServerLists()
        {
            objclsDALMasterData = new clsDALMasterData();
            DataSet ds = objclsDALMasterData.GetServerLists();
            return ds;

        }
        public DataSet GetGroupList()
        {
            objclsDALMasterData = new clsDALMasterData();
            DataSet ds = objclsDALMasterData.GetGroupList();
            return ds;

        }


        public string GetServerForShare(string ShareName)
        {
            objclsDALMasterData = new clsDALMasterData();
            string strServerName = objclsDALMasterData.GeTServerForShare(ShareName);
            return strServerName;
        }
        public int GeTDB_ID(string DatabaseName)
        {
            objclsDALMasterData = new clsDALMasterData();
            int strDB_ID = objclsDALMasterData.GeTDB_ID(DatabaseName);
            return strDB_ID;
        }
        public DataSet GetDatabaseLists()
        {
            objclsDALMasterData = new clsDALMasterData();
            DataSet ds = objclsDALMasterData.GetDatabaseLists();
            return ds;

        }

        public bool SaveDBMapping(string strDatabaseName, int ServerID)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool bln = objclsDALMasterData.SaveDBMapping(strDatabaseName, ServerID);
            return bln;
        }

        public bool SaveDBMapping(string strDBName, int ServerID, int DB_ID,string CreatedBy)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool bln = objclsDALMasterData.SaveDBMapping(strDBName, ServerID, DB_ID,CreatedBy);
            return bln;
        }
        public int GetDatabaseID(string strDatabaseName)
        {
            objclsDALMasterData = new clsDALMasterData();
            return objclsDALMasterData.GetDatabaseID(strDatabaseName);

        }

        public bool CheckIfServerDBMapping(string strDBNM, int ServerID)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagServerShare = objclsDALMasterData.CheckIfServerDBMapping(strDBNM, ServerID);
            return blnflagServerShare;
        }


        public DataSet CheckIfDatabaseExists(string strDatabaseNm, int strServerID, int strAppID)
        {
            objclsDALMasterData = new clsDALMasterData();
            DataSet blnflagShare = objclsDALMasterData.CheckIfDatabaseExists(strDatabaseNm, strServerID, strAppID);
            return blnflagShare;
        }

        public string GetServerForReport(int repID)
        {
            objclsDALMasterData = new clsDALMasterData();
            string strServerName = objclsDALMasterData.GetServerForReport(repID);
            return strServerName;
        }
        public void DeleteShare(int ShareID)
        {
            objclsDALMasterData = new clsDALMasterData();
            objclsDALMasterData.DeleteShare(ShareID);

        }

        public void DeleteLinuxServerMapping(int serverid)
        {
            objclsDALMasterData = new clsDALMasterData();
            objclsDALMasterData.DeleteLinuxServerMapping(serverid);

        }
        public void DeleteGroupMapping(int groupid)
        {
            objclsDALMasterData = new clsDALMasterData();
            objclsDALMasterData.DeleteGroupMapping(groupid);

        }
        public int GetShareID(string ShareName)
        {
            objclsDALMasterData = new clsDALMasterData();
            return objclsDALMasterData.GetShareID(ShareName);

        }
        public bool SaveShare(string strShare, int ServerID)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool bln = objclsDALMasterData.SaveShare(strShare, ServerID);
            return bln;
        }
        public bool UpdateShareName(int ShareID, string strShare, int ServerID)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool bln = objclsDALMasterData.UpdateshareName(ShareID, strShare, ServerID);
            return bln;
        }
        public bool CheckIfGroupMappingExists(string strServerName, string strdomain, int appid)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagShare = objclsDALMasterData.CheckIfGroupMappingExists(strServerName, strdomain, appid);
            return blnflagShare;
        }
        public bool CheckIfSecGrpMappingExists(string strServerName, string strdomain, int appid)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagShare = objclsDALMasterData.CheckIfSecGrpMappingExists(strServerName, strdomain, appid);
            return blnflagShare;
        }
        public bool CheckIfShareExists(string strShareName)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagShare = objclsDALMasterData.CheckIfShareExists(strShareName);
            return blnflagShare;
        }

        public bool CheckIfServerExists(string strServerName)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagShare = objclsDALMasterData.CheckIfServerExists(strServerName);
            return blnflagShare;
        }

        public bool CheckIfLinuxMappingExists(string strShareName,int intAppID)
            {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagShare = objclsDALMasterData.CheckIfLinuxMappingExists(strShareName, intAppID);
            return blnflagShare;
        }
        public bool CheckIfGroupIDExists(int groupid)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagServerShare = objclsDALMasterData.CheckIfGroupIDExists(groupid);
            return blnflagServerShare;
 
        }
        public bool CheckIfServerShareMapExists(string strShareName, int ServerID)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagServerShare = objclsDALMasterData.CheckIfServerShareExists(strShareName, ServerID);
            return blnflagServerShare;
        }
        public bool CheckIfServerShareMapping(string strShareName, int ServerID)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool blnflagServerShare = objclsDALMasterData.CheckIfServerShareMapping(strShareName, ServerID);
            return blnflagServerShare;
        }

        public bool UpdateInitiative(string InitiativeID, string InitiativeName, bool update, bool VisibleToAuditor)
        {
            return true;
        }
        public bool DeleteInitiatives(string InitiativeID)
        {
            return true;
        }
        public bool InsertServerName(string servername)
        {
            return true;
        }
        public bool DeleteServerName(string serverID)
        {
            return true;
        }
        public bool UpdateServerName(string serverID, string servername)
        {
            return true;
        }
        public bool InsertShareName(string shareName, string serverID)
        {
            return true;
        }
        public bool DeleteShareName(string shareID)
        {
            return true;
        }
        public bool UpdateshareName(string shareID, string shareName, string serverID)
        {
            return true;
        }
        public bool UpdateDBMapping(string Dbname, int AppId, int ServerId, int DbId, string ModifiedBy)
        {
            objclsDALMasterData = new clsDALMasterData();
            bool obj = objclsDALMasterData.UpdateDBMapping(Dbname, AppId, ServerId, DbId,ModifiedBy);
            return obj;
        }
        public bool DeleteDBMapping(int DBID, string ModifiedBy)
        {
            objclsDALMasterData=new clsDALMasterData ();
            bool obj = objclsDALMasterData.DeleteDBMapping(DBID,ModifiedBy);
            return obj;
        }
    }//Class Ends
}//Namespace Ends