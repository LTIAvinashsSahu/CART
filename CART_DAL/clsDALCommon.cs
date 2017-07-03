using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CART_EAL;
using System.Data;
using System.Data.SqlClient;

namespace CART_DAL
{
    public class clsDALCommon:clsDBConnection
    {
        #region DataMemebers
        SqlCommand cmd = null;
        //SqlConnection con = null;
        DataSet objds = null;
        #endregion

        //code added by Suman

        public DataTable GetCommonFields(string Field)
        {
            try
            {
                return (clsDAL_SqlHelper.ExecuteDataset
                        (
                            strconnectionString,
                            CommandType.StoredProcedure,
                            "sp_GetCommonFields",
                            new SqlParameter("@Field", Field)
                         ).Tables[0]);
           

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FetchApproval(string strLoggedinSID)
        {

            clsEALSession objclsEALSession = new clsEALSession();

            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetSelectiveApproval", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[1];

                FetchReportParam[0] = new SqlParameter("@loggedInSID", SqlDbType.VarChar, 50);
                FetchReportParam[0].Value = strLoggedinSID;

                

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daFetchAppReports.SelectCommand.Parameters.Add(p);
                }
                daFetchAppReports.Fill(objds);
                return objds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objds.Dispose();
            }
        }

        public DataSet FetchApproval_DB(string strLoggedinSID,string strDBType)
        {

            clsEALSession objclsEALSession = new clsEALSession();

            //SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetSelectiveApproval_DB", strconnectionString);
            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetSelectiveApproval_DB_1", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@loggedInSID", SqlDbType.VarChar, 50);
                FetchReportParam[0].Value = strLoggedinSID;

                FetchReportParam[1] = new SqlParameter("@DBType", SqlDbType.VarChar, 50);
                FetchReportParam[1].Value = strDBType;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daFetchAppReports.SelectCommand.Parameters.Add(p);
                }
                daFetchAppReports.Fill(objds);
                return objds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objds.Dispose();
            }
        }

        public DataSet FetchApproval_PSI(string strLoggedinSID)
        {

            clsEALSession objclsEALSession = new clsEALSession();

            //SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetSelectiveApproval_DB", strconnectionString);
            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetSelectiveApproval_PSI", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[1];

                FetchReportParam[0] = new SqlParameter("@loggedInSID", SqlDbType.VarChar, 50);
                FetchReportParam[0].Value = strLoggedinSID;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daFetchAppReports.SelectCommand.Parameters.Add(p);
                }
                daFetchAppReports.Fill(objds);
                return objds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objds.Dispose();
            }
        }


        public void SaveSelectiveApproval(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverNAme, string strApproverEmail, string strUserSID, string strGroupSID, string strUserName, string strGroupNm, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_SaveSelectiveApproval", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[13];
            para[0] = new SqlParameter("@CoName", SqlDbType.VarChar, 100);
            para[0].Value = strLoggedInUserName;

            para[1] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
            para[1].Value = strloggedInUserSID;

            para[2] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
            para[2].Value = strApproverSID;

            para[3] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
            para[3].Value = strUserSID;

            para[4] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
            para[4].Value = strUserName;

            para[5] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar, 50);
            para[5].Value = strCurrQuarter;
            para[6] = new SqlParameter("@AppID", SqlDbType.Int);
            para[6].Value = intAppId;

            para[7] = new SqlParameter("@IsGlobal", SqlDbType.Bit);
            para[7].Value = IsGlobal;

            para[8] = new SqlParameter("@Scope", SqlDbType.VarChar, 20);
            para[8].Value = strScope;

            para[9] = new SqlParameter("@ApproverNm", SqlDbType.VarChar, 100);
            para[9].Value = strApproverNAme;

            para[10] = new SqlParameter("@ApproverMail", SqlDbType.VarChar, 500);
            para[10].Value = strApproverEmail;

            para[11] = new SqlParameter("@GroupNm", SqlDbType.VarChar, 500);
            para[11].Value = strGroupNm;

            if (strGroupSID == "")
            {
                strGroupSID = null;
            }

            para[12] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
            para[12].Value = strGroupSID;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            conn.Close();
        }

        public void DeleteSelectiveApproval(string strloggedInUserSID, string strApproverSID,
            string strCurrQuarter, int intAppId, string strScope, string sReportType)
        {
            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("SP_DeleteSelectiveApproval", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure; 
            cmd.Parameters.AddWithValue("@COSID", strloggedInUserSID);
            cmd.Parameters.AddWithValue("@ApproverSID", strApproverSID); 
            cmd.Parameters.AddWithValue("@Quarter", strCurrQuarter);
            cmd.Parameters.AddWithValue("@AppID", intAppId); 
            cmd.Parameters.AddWithValue("@Scope", strScope);
            cmd.Parameters.AddWithValue("@ReportType", sReportType);
            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void SaveSelectiveApproval_DB(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverNAme, string strApproverEmail, string strUserSID, string strServerName, string strUserName, string strDatabasename, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope,string strDBType,string strSignOffStatus, string strRole)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_SaveSelectiveApproval_DBAcc_new", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[16];
            para[0] = new SqlParameter("@CoName", SqlDbType.VarChar, 100);
            para[0].Value = strLoggedInUserName;

            para[1] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
            para[1].Value = strloggedInUserSID;

            para[2] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
            para[2].Value = strApproverSID;

            para[3] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
            para[3].Value = strUserSID;

            para[4] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
            para[4].Value = strUserName;

            para[5] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar, 50);
            para[5].Value = strCurrQuarter;

            para[6] = new SqlParameter("@AppID", SqlDbType.Int);
            para[6].Value = intAppId;

            para[7] = new SqlParameter("@IsGlobal", SqlDbType.Bit);
            para[7].Value = IsGlobal;

            para[8] = new SqlParameter("@Scope", SqlDbType.VarChar, 20);
            para[8].Value = strScope;

            para[9] = new SqlParameter("@ApproverNm", SqlDbType.VarChar, 100);
            para[9].Value = strApproverNAme;

            para[10] = new SqlParameter("@ApproverMail", SqlDbType.VarChar, 500);
            para[10].Value = strApproverEmail;

            para[11] = new SqlParameter("@DataBaseName", SqlDbType.VarChar, 500);
            para[11].Value = strDatabasename;

            para[12] = new SqlParameter("@ServerName", SqlDbType.VarChar, 100);
            para[12].Value = strServerName;

            para[13] = new SqlParameter("@DBType", SqlDbType.VarChar, 100);
            para[13].Value = strDBType;

            para[14] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 100);
            para[14].Value = strSignOffStatus;

            para[15] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
            para[15].Value = strRole;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            conn.Close();
        }
       
        public void SaveSelectiveApproval_linux(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverNAme, string strApproverEmail, string strUserSID, string strServerName, string strUserName, string strDatabasename, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope, string strDBType, string strSignOffStatus, string strRole)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_SaveSelectiveApproval_Linux", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[16];
            para[0] = new SqlParameter("@CoName", SqlDbType.VarChar, 100);
            para[0].Value = strLoggedInUserName;

            para[1] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
            para[1].Value = strloggedInUserSID;

            para[2] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
            para[2].Value = strApproverSID;

            para[3] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
            para[3].Value = strUserSID;

            para[4] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
            para[4].Value = strUserName;

            para[5] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar, 50);
            para[5].Value = strCurrQuarter;

            para[6] = new SqlParameter("@AppID", SqlDbType.Int);
            para[6].Value = intAppId;

            para[7] = new SqlParameter("@IsGlobal", SqlDbType.Bit);
            para[7].Value = IsGlobal;

            para[8] = new SqlParameter("@Scope", SqlDbType.VarChar, 20);
            para[8].Value = strScope;

            para[9] = new SqlParameter("@ApproverNm", SqlDbType.VarChar, 100);
            para[9].Value = strApproverNAme;

            para[10] = new SqlParameter("@ApproverMail", SqlDbType.VarChar, 500);
            para[10].Value = strApproverEmail;

            para[11] = new SqlParameter("@DataBaseName", SqlDbType.VarChar, 500);
            para[11].Value = strDatabasename;

            para[12] = new SqlParameter("@ServerName", SqlDbType.VarChar, 100);
            para[12].Value = strServerName;

            para[13] = new SqlParameter("@DBType", SqlDbType.VarChar, 100);
            para[13].Value = strDBType;

            para[14] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 100);
            para[14].Value = strSignOffStatus;

            para[15] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
            para[15].Value = strRole;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            conn.Close();
        }
        
        public void ModifyAdminRights_SQL(string strUserSID, int intAppID, string scope, string strLoggedInUserSID, string strSelectedQuarter, string strDBType)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_ModifyAdminRight_SQL", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[6];


            para[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
            para[0].Value = strUserSID;

            para[1] = new SqlParameter("@ApplicationID", SqlDbType.Int);
            para[1].Value = intAppID;

            para[2] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
            para[2].Value = scope;

            para[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
            para[3].Value = strSelectedQuarter;

            para[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
            para[4].Value = strLoggedInUserSID;

            para[5] = new SqlParameter("@DBType", SqlDbType.VarChar, 50);
            para[5].Value = strLoggedInUserSID;


            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);
            conn.Close();

        }

        public void UpdateSelectiveQuarter(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverName, string strApproverEmail, string strUserSID, string strGroupSID, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope, string strUserName, string strGroupNm)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_UpdateSelectiveQuarter", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[13];
            para[0] = new SqlParameter("@CoName", SqlDbType.VarChar, 100);
            para[0].Value = strLoggedInUserName;

            para[1] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
            para[1].Value = strloggedInUserSID;

            para[2] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
            para[2].Value = strApproverSID;

            para[3] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar, 50);
            para[3].Value = strCurrQuarter;

            para[4] = new SqlParameter("@AppID", SqlDbType.Int);
            para[4].Value = intAppId;

            para[5] = new SqlParameter("@IsGlobal", SqlDbType.Bit);
            para[5].Value = IsGlobal;

            para[6] = new SqlParameter("@Scope", SqlDbType.VarChar, 20);
            para[6].Value = strScope;

            para[7] = new SqlParameter("@ApproverNm", SqlDbType.VarChar, 100);
            para[7].Value = strApproverName;

            para[8] = new SqlParameter("@ApproverMail", SqlDbType.VarChar, 500);
            para[8].Value = strApproverEmail;

            para[9] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
            para[9].Value = strUserSID;

            if (strGroupSID == "")
            {
                strGroupSID = null;
            }


            para[10] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
            para[10].Value = strGroupSID;

            para[11] = new SqlParameter("@UserName", SqlDbType.VarChar, 500);
            para[11].Value = strUserName;

            para[12] = new SqlParameter("@GroupNm", SqlDbType.VarChar, 500);
            para[12].Value = strGroupNm;
            

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            conn.Close();
        }

        public void UpdateSelectiveQuarter_DB(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverName, string strApproverEmail, string strFetchedQuarter, string strLatestQuarter, int intAppId, bool IsGlobal, string strScope, string strDBType)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_UpdateSelectiveQuarter_DB_new", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[11];
            para[0] = new SqlParameter("@CoName", SqlDbType.VarChar, 100);
            para[0].Value = strLoggedInUserName;

            para[1] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
            para[1].Value = strloggedInUserSID;

            para[2] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
            para[2].Value = strApproverSID;

            para[3] = new SqlParameter("@LatestQuarter", SqlDbType.VarChar, 50);
            para[3].Value = strLatestQuarter;

            para[4] = new SqlParameter("@AppID", SqlDbType.Int);
            para[4].Value = intAppId;

            para[5] = new SqlParameter("@IsGlobal", SqlDbType.Bit);
            para[5].Value = IsGlobal;

            para[6] = new SqlParameter("@Scope", SqlDbType.VarChar, 20);
            para[6].Value = strScope;

            para[7] = new SqlParameter("@ApproverNm", SqlDbType.VarChar, 100);
            para[7].Value = strApproverName;

            para[8] = new SqlParameter("@ApproverMail", SqlDbType.VarChar, 500);
            para[8].Value = strApproverEmail;

            para[9] = new SqlParameter("@FetchedQuarter", SqlDbType.VarChar, 100);
            para[9].Value = strFetchedQuarter;

            para[10] = new SqlParameter("@DBType", SqlDbType.VarChar, 500);
            para[10].Value = strDBType;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            conn.Close();
        }


        public void ModifyAdminRights(string strUserSID, int intAppID, string scope, string strLoggedInUserSID, string strSelectedQuarter)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_ModifyAdminRight", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[5];
           

            para[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
            para[0].Value = strUserSID;

            para[1] = new SqlParameter("@ApplicationID", SqlDbType.Int);
            para[1].Value = intAppID;

            para[2] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
            para[2].Value = scope;

            para[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
            para[3].Value = strSelectedQuarter;

            para[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
            para[4].Value = strLoggedInUserSID;



            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);
            conn.Close();

        }
        public void ModifyReportAdminRights(string strUserSID, string strGroupSID, int AppID, int RepID, string scope, string strLoggedInUserSID, string strSelectedQuarter)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_ModifyReportAdminRight", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[7];


            para[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
            para[0].Value = strUserSID;

            para[1] = new SqlParameter("@RepID", SqlDbType.Int);
            para[1].Value = RepID;

            para[2] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
            para[2].Value = strGroupSID;

            para[3] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
            para[3].Value = scope;

            para[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
            para[4].Value = strLoggedInUserSID;

            para[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
            para[5].Value = strSelectedQuarter;


            para[6] = new SqlParameter("@ApplicationID", SqlDbType.Int);
            para[6].Value = AppID;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);
            conn.Close();

        }

        public DataTable GetRoleWiseUser(int roleID)
        {
            try
            {
                return (clsDAL_SqlHelper.ExecuteDataset
                        (
                            strconnectionString,
                            CommandType.StoredProcedure,
                            "sp_GetRoleWiseUser",
                            new SqlParameter("@roleID", roleID)
                         ).Tables[0]);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //code end here

        public string GetHomepageText()
        {
            string strText="";
            //SqlDataAdapter daText = new SqlDataAdapter("SP_GetHomePageData", strconnectionString);
            SqlDataAdapter daText = new SqlDataAdapter("SP_GetHomePageData", strconnectionString);
            daText.SelectCommand.CommandType = CommandType.StoredProcedure;
            daText.SelectCommand.CommandTimeout = 36000;
            DataSet ds_Text = new DataSet();

            daText.Fill(ds_Text);
           
            strText = ds_Text.Tables[0].Rows[0][0].ToString() ;
            return strText;
        
        }
        public string GetCurrentQuarter()
        {
            string strQuarter = "";
            SqlDataAdapter daQ = new SqlDataAdapter("SP_GetCurrentQuarter", strconnectionString);
            daQ.SelectCommand.CommandType = CommandType.StoredProcedure;
            daQ.SelectCommand.CommandTimeout = 36000;
            DataSet ds_Q = new DataSet();

            daQ.Fill(ds_Q);

            strQuarter = ds_Q.Tables[0].Rows[0][0].ToString();
            return strQuarter;

        }

        public string GetLatestQuarter()
        {
            string strQuarter = "";
            SqlDataAdapter daQ = new SqlDataAdapter("select  quarter from tblsoxreport where reportid=(select max(reportid) from tblsoxReport )", strconnectionString);
            daQ.SelectCommand.CommandType = CommandType.Text;
            daQ.SelectCommand.CommandTimeout = 36000;
            DataSet ds_Q = new DataSet();

            daQ.Fill(ds_Q);

            strQuarter = ds_Q.Tables[0].Rows[0][0].ToString();
            return strQuarter;

        }

        #region to check if user exists in current quarter

        public DataSet CheckIfUserExistsForCurrentQuarter_DB(string strUserSID,string scope, int Appid, string strquarter,string strDBType)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckUserForNextQuarter_DB1", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                // string strControlOwnerSID = objControlOwner.StrUserSID;

                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(strconnectionString);
                conn.Open();

                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                para[0].Value = scope;

                para[1] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
                para[1].Value = strUserSID;

                para[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                para[2].Value = Appid;

                para[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                para[3].Value = strquarter;

                para[4] = new SqlParameter("@DBType", SqlDbType.VarChar, 100);
                para[4].Value = strDBType;


                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);

                }
                da.Fill(ds);


                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        public string GetLatestQuarter_DB(string strDBType)
        {
            string strQuarter = "";
            SqlDataAdapter daQ = new SqlDataAdapter();
            if (strDBType == "SQLReport")
            {
                daQ = new SqlDataAdapter("SELECT DISTINCT Quarter from tblSOXDBReportUpload WHERE DBRepID IN (SELECT Max(DBRepID) FROM tblSOXDBReportUpload)", strconnectionString);
            }
            else if (strDBType == "OracleReport")
            {
                daQ = new SqlDataAdapter("SELECT DISTINCT Quarter from tblSOXDBReportOracleUpload WHERE DBRepID IN (SELECT Max(DBRepID) FROM tblSOXDBReportOracleUpload)", strconnectionString);
            }
            else if (strDBType == clsEALReportType.PSIReport)
            {
                daQ = new SqlDataAdapter("SELECT distinct quarter from tblsoxpsiusers WHERE rowid IN (SELECT Max(rowid) FROM tblsoxPSIUsers)", strconnectionString);
            }
            if (strDBType == "LinuxReport")
            {
                daQ = new SqlDataAdapter("SELECT DISTINCT Quarter from tblSOXlinux WHERE ID IN (SELECT Max(ID) FROM tblSOXlinux)", strconnectionString);
            }
            daQ.SelectCommand.CommandType = CommandType.Text;
            daQ.SelectCommand.CommandTimeout = 36000;
            DataSet ds_Q = new DataSet();

            daQ.Fill(ds_Q);

            strQuarter = ds_Q.Tables[0].Rows[0][0].ToString();
            return strQuarter;

        }

        public string GetNextQuarter(string strcurrQuarter)
        {
            string strQuarter = "";
            SqlDataAdapter daQ = new SqlDataAdapter("SP_GetNextQuarter", strconnectionString);
            daQ.SelectCommand.CommandType = CommandType.StoredProcedure;
            daQ.SelectCommand.CommandTimeout = 36000;
            DataSet ds_Q = new DataSet();

            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter("@currquar", SqlDbType.VarChar,50);
            para[0].Value = strcurrQuarter;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                daQ.SelectCommand.Parameters.Add(p);
            }
            daQ.Fill(ds_Q);

            strQuarter = ds_Q.Tables[0].Rows[0][0].ToString();
            return strQuarter;

        }

        public bool CheckIfNextQuarterReportExists(string strCurrQuarter)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_ChkNextQuarterReport", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter("@currquar", SqlDbType.VarChar, 50);
            para[0].Value = strCurrQuarter;

            para[1] = new SqlParameter("@nextquarterExists", SqlDbType.Bit);
            para[1].Direction = ParameterDirection.Output;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            string strDs = ds.Tables[0].Rows[0][0].ToString();
            bool flag;
            if (strDs == "True")
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            conn.Close();
            return flag;
        }

        public void CheckIFUserExistsInCurrentQuarter(string strUserSID, string strCurrQuarter, string PreviousQuartertoSelected, int intAppId,string role)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_ChkSelectUserExistsCurrQuarter", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            // string strControlOwnerSID = objControlOwner.StrUserSID;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar, 50);
            para[0].Value = strCurrQuarter;

            para[1] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
            para[1].Value = strUserSID;

            para[2] = new SqlParameter("@PrevQuarter", SqlDbType.VarChar, 50);
            para[2].Value = PreviousQuartertoSelected;

            para[3] = new SqlParameter("@AppID", SqlDbType.Int);
            para[3].Value = intAppId;

            para[4] = new SqlParameter("@Role", SqlDbType.VarChar,50);
            para[4].Value = role;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);
            conn.Close();
            //string strDs = ds.Tables[0].Rows[0][0].ToString();
            //bool flag;
            //if (strDs == "True")
            //{
            //    flag = true;
            //}
            //else
            //{
            //    flag = false;
            //}
          
        }

        public bool SetHomepageText(string strText)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "SP_SetHomePageData";
                SqlParameter[] pTicket = new SqlParameter[1];
               

                pTicket[0] = new SqlParameter("@IN_Text", SqlDbType.Text);
                pTicket[0].Value = strText;
                foreach (SqlParameter p in pTicket)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                con.Close();
                return true;
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
            
        }

        public DataTable GetSignOffStatus()
        {
            SqlDataAdapter daQ = new SqlDataAdapter();
            daQ = new SqlDataAdapter("SP_GetSignOffStatus", strconnectionString);
            daQ.SelectCommand.CommandType = CommandType.StoredProcedure;
            daQ.SelectCommand.CommandTimeout = 36000;

            DataTable ds_CrtMgr = new DataTable();
            try
            {
                SqlParameter[] para = new SqlParameter[1]; 
                daQ.Fill(ds_CrtMgr);
                return ds_CrtMgr;
            }
            catch (Exception Ex)
            {
                throw Ex;
            } 
        }

        public DataTable GetAccountStatus()
        {
            SqlDataAdapter daQ = new SqlDataAdapter();
            daQ = new SqlDataAdapter("Select Row_Number() over (Order by Userstatus ) As Row, A.Userstatus from "+
                "(Select Distinct Case when Userstatus='True' Then 'Enabled' Else 'Disabled' end Userstatus from tblsoxusers "+
                "Union Select distinct Account_status from tblSOXDBReportOracleUpload ) A", strconnectionString);
            daQ.SelectCommand.CommandType = CommandType.Text;
            daQ.SelectCommand.CommandTimeout = 36000;
            DataTable ds_status = new DataTable();

            daQ.Fill(ds_status);
            return ds_status;
        }

        public DataTable GetAccountStatusPSI()
        {
            SqlDataAdapter daQ = new SqlDataAdapter();
            daQ = new SqlDataAdapter("Select Row_Number() over (Order by Userstatus ) As Row, A.Userstatus from " +
                "(Select Distinct Case when Userstatus='True' Then 'Enabled' Else 'Disabled' end Userstatus from tblsoxusers " +
                "Union Select distinct Account_status from tblSOXDBReportOracleUpload Union Select distinct user_status from tblSOXPSIUsers) A", strconnectionString);
            daQ.SelectCommand.CommandType = CommandType.Text;
            daQ.SelectCommand.CommandTimeout = 36000;
            DataTable ds_status = new DataTable();

            daQ.Fill(ds_status);
            return ds_status;
        }

        public DataTable GetCurrentManagers(string quarter)
        {
            SqlDataAdapter daQ = new SqlDataAdapter();
            daQ = new SqlDataAdapter("Select Row_Number() over (Order by Current_Manager ) As Row, A.Current_Manager from " +
                "(Select distinct Current_Manager from tblSOXPSIUsers where Current_Manager is not null and Quarter = '" + quarter + "') A", strconnectionString);
            daQ.SelectCommand.CommandType = CommandType.Text;
            daQ.SelectCommand.CommandTimeout = 36000;
            DataTable ds_CrtMgr = new DataTable();

            daQ.Fill(ds_CrtMgr);
            return ds_CrtMgr;
        }

        //Added by Nag
        public DataTable GetAllApprovers(string ReportType)
        {
            SqlDataAdapter daQ = new SqlDataAdapter();
            daQ = new SqlDataAdapter("sp_GetAllApproversDD", strconnectionString);
            daQ.SelectCommand.CommandType = CommandType.StoredProcedure;
            daQ.SelectCommand.CommandTimeout = 36000;

            DataTable ds_CrtMgr = new DataTable();
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@ReportType", SqlDbType.Text);
                para[0].Value = ReportType;
                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daQ.SelectCommand.Parameters.Add(p);
                }
                daQ.Fill(ds_CrtMgr);
                return ds_CrtMgr;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //daQ.Fill(ds_CrtMgr);
            //return ds_CrtMgr;
        }

        public DataTable GetAvailableQuarters()
        {
            string strQuarter = "";
            SqlDataAdapter da = new SqlDataAdapter("SP_GetAvialabeQuarter", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 36000;
            DataTable dt = new DataTable();
            da.Fill(dt);
           
            return dt;
        }
        public DataSet GetSOXScope(int AppID)
        {
            SqlDataAdapter daApp = new SqlDataAdapter("SP_GetSOXApp1", strconnectionString);
            daApp.SelectCommand.CommandType = CommandType.StoredProcedure;
            daApp.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = AppID;
                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daApp.SelectCommand.Parameters.Add(p);
                }
                daApp.Fill(ds);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public DataSet GetAppScope(int AppID,string strApproverAdid)
        {
            SqlDataAdapter daApp = new SqlDataAdapter("SP_GetAppScope", strconnectionString);
            daApp.SelectCommand.CommandType = CommandType.StoredProcedure;
            daApp.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = AppID;

                para[1] = new SqlParameter("@ApproverADID", SqlDbType.VarChar,50);
                para[1].Value = strApproverAdid;


                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daApp.SelectCommand.Parameters.Add(p);
                }
                daApp.Fill(ds);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public DataSet GetControlOwnersEmailIDs()
        {
            string strText = "";
            SqlDataAdapter daText = new SqlDataAdapter("SP_GetControlOwnersEmailIDS", strconnectionString);
            daText.SelectCommand.CommandType = CommandType.StoredProcedure;
            daText.SelectCommand.CommandTimeout = 36000;
            DataSet ds_Text = new DataSet();

            daText.Fill(ds_Text);

//            strText = ds_Text.Tables[0].Rows[0][0].ToString();
            return ds_Text;
        
        }

        public DataSet GetAppControlOwnerEmailID(int intAppID)
        {
            string strEmail = "";
            SqlDataAdapter da = new SqlDataAdapter("SP_GetAppControlOwnerEmailID", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 36000;
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = intAppID;
                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(ds);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        
        }


        public DataSet GetAppControlOwnerInfo(int intAppID)
        {
            
            SqlDataAdapter da = new SqlDataAdapter("SP_GetAppControlOwnerInfo", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 36000;
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = intAppID;
                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(ds);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
    

    }
}
