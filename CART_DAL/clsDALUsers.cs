using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CART_EAL;
using System.Data;
using System.Data.SqlClient;

namespace CART_DAL
{
    public class clsDALUsers:clsDBConnection
    {
        #region DataMembers
        SqlCommand cmd=null;
        SqlConnection con=null;
        DataSet objds=null;
        #endregion


        #region GetCurrentUser Role
        public string[] GetCurrentUserRole(clsEALUser objclsEALUser)
        {
            string Roles;
            string[] Role;
            
            SqlDataAdapter daFetchCurrentUserRole = new SqlDataAdapter("SP_GetCurrentUserRole", strconnectionString);
            daFetchCurrentUserRole.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchCurrentUserRole.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            
                SqlParameter[] pGetCUSerRoleParam = new SqlParameter[1];
                pGetCUSerRoleParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
                pGetCUSerRoleParam[0].Value = objclsEALUser.StrUserSID;


                foreach (SqlParameter p in pGetCUSerRoleParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daFetchCurrentUserRole.SelectCommand.Parameters.Add(p);
                }
                daFetchCurrentUserRole.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables.Count != 0)
                    {
                        if (objds.Tables[0].Rows[0]["UserRole"] != null)
                        {
                            Roles = objds.Tables[0].Rows[0]["UserRole"].ToString();
                            Role = Roles.Split(";".ToCharArray());

                            return Role;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
         
            
        }


        public void UpdateTicketStatus(int intAppID, string RITMNo, string strUSID, string strGroupSID, string strUName, string strQuarterSelected)
        {
    
            SqlConnection con =new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatus", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppID", intAppID);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);
            cmd.Parameters.AddWithValue("@strUSID", strUSID);
            cmd.Parameters.AddWithValue("@strGroupSID", strGroupSID);
            cmd.Parameters.AddWithValue("@strUName", strUName);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strQuarterSelected);
                     
            cmd.ExecuteNonQuery();
            
            con.Close();
                       
        }

        public void UpdateTicketStatusSQL(int intAppID, string RITMNo,string strUName, string strURole,string strQuarterSelected)
        {
    
            SqlConnection con =new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusSQL", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppID", intAppID);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);
            cmd.Parameters.AddWithValue("@strUName", strUName);
            cmd.Parameters.AddWithValue("@strURole", strURole);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strQuarterSelected);
                     
            cmd.ExecuteNonQuery();
            
            con.Close();
                       
        }


        public void UpdateTicketStatusOracle(int intAppID, string RITMNo, string strUName, string strURole, string strQuarterSelected)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusOracle", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppID", intAppID);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);
            cmd.Parameters.AddWithValue("@strUName", strUName);
            cmd.Parameters.AddWithValue("@strURole", strURole);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strQuarterSelected);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public void UpdateTicketStatusLinux(int intAppID, string RITMNo, string strUName, string strQuarterSelected)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusLinux", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppID", intAppID);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);
            cmd.Parameters.AddWithValue("@strUName", strUName);
            //cmd.Parameters.AddWithValue("@strURole", strURole);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strQuarterSelected);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public void UpdateTicketStatusSecGrp(int intAppID, string RITMNo, string strUName, string strQuarterSelected)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusSecGrp", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppID", intAppID);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);
            cmd.Parameters.AddWithValue("@strUName", strUName);
            //cmd.Parameters.AddWithValue("@strURole", strURole);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strQuarterSelected);

            cmd.ExecuteNonQuery();

            con.Close();

        }


        public void UpdateTicketStatusPSI(string strUName, string RITMNo, string strQuarterSelected)
        {
            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusPSI", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@strUName", strUName);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);
            //cmd.Parameters.AddWithValue("@strURole", strURole);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strQuarterSelected);

            cmd.ExecuteNonQuery();

            con.Close();
        }


        public void UpdateTicketStatusSecGrpIndividualRep(string lblServer, string RITMNo, string lblUserName, string strSelectedQuarter)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusSecGrpIndividualRep", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Grouppname", lblServer);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);
            cmd.Parameters.AddWithValue("@strUName", lblUserName);
            //cmd.Parameters.AddWithValue("@strURole", strURole);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strSelectedQuarter);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public void UpdateTicketStatusIndividualRep(int ReportID, string lblAccountName, string lblUserSID, string strSelectedQuarter, string RITMNo)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusIndividualRep", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RepID", ReportID);
            cmd.Parameters.AddWithValue("@UserName", lblAccountName);
            cmd.Parameters.AddWithValue("@UserSID", lblUserSID);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strSelectedQuarter);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public void UpdateTicketStatusSQLIndividualRep(int ReportID, string lblAccountName,  string lblServer, string strSelectedQuarter, string RITMNo)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusSQLIndividualRep", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RepID", ReportID);
            cmd.Parameters.AddWithValue("@SQLLoginName", lblAccountName);
            
            cmd.Parameters.AddWithValue("@ServerName", lblServer);
            //cmd.Parameters.AddWithValue("@UserName", lblUserName);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strSelectedQuarter);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public void UpdateTicketStatusOracleIndividualRep(int ReportID, string lblServerNm, string lblUserName, string strSelectedQuarter, string RITMNo)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusOracleIndividualRep", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RepID", ReportID);
            cmd.Parameters.AddWithValue("@ServerName", lblServerNm);
            cmd.Parameters.AddWithValue("@UserName", lblUserName);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strSelectedQuarter);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public void UpdateTicketStatusLinuxIndividualRep(int ReportID, string lblUserName, string lblServer, string strSelectedQuarter, string RITMNo)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateTicketStatusLinuxIndividualRep", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ServerID", ReportID);
            cmd.Parameters.AddWithValue("@UserID", lblUserName);
            cmd.Parameters.AddWithValue("@ServerName", lblServer);
            cmd.Parameters.AddWithValue("@strQuarterSelected", strSelectedQuarter);
            cmd.Parameters.AddWithValue("@RITMNo", RITMNo);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public string[] ApproverOrCO(clsEALUser objclsEALUser)
        {
            string Roles;
            string[] Role;

            SqlDataAdapter daFetchCurrentUserRole = new SqlDataAdapter("SP_ApproverOrCO", strconnectionString);
            daFetchCurrentUserRole.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchCurrentUserRole.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();

            SqlParameter[] pGetCUSerRoleParam = new SqlParameter[1];
            pGetCUSerRoleParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
            pGetCUSerRoleParam[0].Value = objclsEALUser.StrUserSID;


            foreach (SqlParameter p in pGetCUSerRoleParam)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                daFetchCurrentUserRole.SelectCommand.Parameters.Add(p);
            }
            daFetchCurrentUserRole.Fill(objds);
            if (objds != null)
            {
                if (objds.Tables.Count != 0)
                {
                    if (objds.Tables[0].Rows[0]["UserRole"] != null)
                    {
                        Roles = objds.Tables[0].Rows[0]["UserRole"].ToString();
                        Role = Roles.Split(";".ToCharArray());

                        return Role;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }


        }
        #endregion
        //GetRoles
        #region GetRoles
        public DataSet GetRoles()
        {
            SqlDataAdapter daRoles = new SqlDataAdapter("select distinct UserRole  from tblSOXUserRoles", strconnectionString);
            daRoles.SelectCommand.CommandType = CommandType.Text;
            daRoles.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            daRoles.Fill(objds);
            return objds;
            
        }
        public DataSet GetUserDetails()
        {
            SqlDataAdapter daRoles = new SqlDataAdapter("select distinct *  from tblSOXUserRoles where(Status=1 or Status is null)", strconnectionString);
            daRoles.SelectCommand.CommandType = CommandType.Text;
            daRoles.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            daRoles.Fill(objds);
            return objds;

        }
        //[SP_SaveUserRole]
        public bool SaveUserRole(clsEALUser objclsUser, string userRole, string StrUserADID)
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("SP_SaveUserRole", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                string strSID = objclsUser.StrUserSID;
                string strName = objclsUser.StrUserName;
                string strADID = objclsUser.StrUserADID;
                string strEmail = objclsUser.StrUserEmailID;
                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[6];
                para[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
                para[0].Value = strName;

                para[1] = new SqlParameter("@UserEmailID", SqlDbType.VarChar, 100);
                para[1].Value = strEmail;

                para[2] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
                para[2].Value = strSID;


                para[3] = new SqlParameter("@UserRole", SqlDbType.VarChar, 50);
                para[3].Value = userRole;

                para[4] = new SqlParameter("@UserADID", SqlDbType.VarChar, 50);
                para[4].Value = strADID;

                para[5] = new SqlParameter("@StrUserADID", SqlDbType.VarChar, 50);
                para[5].Value = StrUserADID;
               
                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }

                da.Fill(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;

        }
        #endregion

        #region Admin Mail IDs
        public string GetAdminMailIDs()
        {
            SqlDataAdapter daRoles = new SqlDataAdapter("SP_AdminMailIDs", strconnectionString);
            daRoles.SelectCommand.CommandType = CommandType.StoredProcedure;
            daRoles.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            daRoles.Fill(objds);
            string mailID = "";
            if (objds != null)
            {
                if (objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                {
                    DataRow dtrow = objds.Tables[0].Rows[0];
                    if (dtrow != null)
                    {
                        mailID = dtrow["AdminID"].ToString();
                    }
                }
            }
            return mailID;
        }
        #endregion

        #region GetUserRoleBYApplication
        public string GetUserRoleByApplication(clsEALUser objclsEALUser,Int32 ApplicationID)
        {
            string Role=string.Empty;


            SqlDataAdapter daFetchCurrentUserRole = new SqlDataAdapter("SP_GetRoleBYApplication", strconnectionString);
            daFetchCurrentUserRole.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchCurrentUserRole.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] pGetCUSerRoleParam = new SqlParameter[2];
                pGetCUSerRoleParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
                pGetCUSerRoleParam[0].Value = objclsEALUser.StrUserSID;

                pGetCUSerRoleParam[1] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 50);
                pGetCUSerRoleParam[1].Value = ApplicationID;

                foreach (SqlParameter p in pGetCUSerRoleParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daFetchCurrentUserRole.SelectCommand.Parameters.Add(p);
                }
                daFetchCurrentUserRole.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables.Count != 0)
                    {
                        if (objds.Tables[0].Rows[0]["UserRole"] != null)
                        {
                            Role = objds.Tables[0].Rows[0]["UserRole"].ToString();
                          
                         
                        }
                       
                    }
                    
                   
                }
                return Role;
               
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        //code added by suman

        public DataSet GetLockOut()
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetLockOut");

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet UpdateLockout(string sid, string status)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@sid", sid),
                new SqlParameter("@status", status)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_Update_SOX_Lock_Out", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet UpdateEmailBody(string GroupID, string Body)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@GroupID", GroupID),
                new SqlParameter("@Body", Body)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateEmailBody", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetApplicationUsers(int AppID, string strQuarter)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@ApplicationID", AppID),
                new SqlParameter("@Quarter", strQuarter)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCRemoval", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetDistinctReportsForUser(int AppID, string strUserSID, string strGrouSID, string strQuarter)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;
               if (strGrouSID=="")
               {
                   strGrouSID = null;
               }
               SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@AppID", AppID),
                new SqlParameter("@UserSID", strUserSID),
                new SqlParameter("@GroupSID", strGrouSID),
                new SqlParameter("@Quarter", strQuarter)
                                    
              };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCUserReports", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <Allreports>

        public DataSet UpdateIsReportSubmitted(int ReportId)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@ReportId", ReportId)
                 //new SqlParameter("@Quarter", Quarter)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_UpdatePending", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateStatus(string strUserSID, int intRepID, string strScope, string strQuarter, int ApplicationID, clsEALUser objclsEALLoggedInUser, string strSignoffStatus)
        {

            SqlDataAdapter da = new SqlDataAdapter("SP_UpdateStatus", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
           
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[7];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                FetchReportParam[0].Value = strUserSID;

                //FetchReportParam[1] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                //FetchReportParam[1].Value = strGroupSID;

                FetchReportParam[1] = new SqlParameter("@RepID", SqlDbType.Int);
                FetchReportParam[1].Value = intRepID;

                FetchReportParam[2] = new SqlParameter("@Scope", SqlDbType.VarChar,20);
                FetchReportParam[2].Value = strScope;

                FetchReportParam[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 20);
                FetchReportParam[3].Value = strQuarter;

                FetchReportParam[4] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                FetchReportParam[4].Value = ApplicationID;

                FetchReportParam[5] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                FetchReportParam[5].Value = objclsEALLoggedInUser.StrUserSID;


                FetchReportParam[6] = new SqlParameter("@Status", SqlDbType.VarChar, 100);
                FetchReportParam[6].Value = strSignoffStatus;


                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(ds);
               
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void UpdateSQLStatus(string strUserSID, int intRepID, string strScope, string strQuarter, int ApplicationID, clsEALUser objclsEALLoggedInUser, string strSignoffStatus,string strDBType,string strUserName,string strDatabase)
        {

            //SqlDataAdapter da = new SqlDataAdapter("SP_UpdateSQLStatus", strconnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SP_UpdateSQLStatus3", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;

            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[8];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                FetchReportParam[0].Value = strUserSID;

                //FetchReportParam[1] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                //FetchReportParam[1].Value = strGroupSID;

                FetchReportParam[1] = new SqlParameter("@RepID", SqlDbType.Int);
                FetchReportParam[1].Value = intRepID;

                FetchReportParam[2] = new SqlParameter("@Scope", SqlDbType.VarChar, 20);
                FetchReportParam[2].Value = strScope;

                FetchReportParam[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 20);
                FetchReportParam[3].Value = strQuarter;

                FetchReportParam[4] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                FetchReportParam[4].Value = ApplicationID;

                //FetchReportParam[5] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
                //FetchReportParam[5].Value = objclsEALLoggedInUser.StrUserSID;
                FetchReportParam[5] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
                FetchReportParam[5].Value = objclsEALLoggedInUser.StrUserADID;

                FetchReportParam[6] = new SqlParameter("@Status", SqlDbType.VarChar, 100);
                FetchReportParam[6].Value = strSignoffStatus;

                FetchReportParam[7] = new SqlParameter("@DBType", SqlDbType.VarChar, 50);
                FetchReportParam[7].Value = strDBType;

                FetchReportParam[8] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
                FetchReportParam[8].Value = strUserName;

                FetchReportParam[9] = new SqlParameter("@DBNm", SqlDbType.VarChar, 50);
                FetchReportParam[9].Value = strDatabase;


                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(ds);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void UpdateStatus_Reports(string strUserSID, int intRepID, string strScope, string strQuarter, int ApplicationID, clsEALUser objclsEALLoggedInUser, string strSignoffStatus, string strGroupSID)
        {

            SqlDataAdapter da = new SqlDataAdapter("SP_UpdateStatus_Reports", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;

            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[8];

                FetchReportParam[0] = new SqlParameter("@sid", SqlDbType.VarChar, 100);
                FetchReportParam[0].Value = strUserSID;



                FetchReportParam[1] = new SqlParameter("@ReportID", SqlDbType.Int);
                FetchReportParam[1].Value = intRepID;

                FetchReportParam[2] = new SqlParameter("@Scope", SqlDbType.VarChar, 20);
                FetchReportParam[2].Value = strScope;

                FetchReportParam[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 20);
                FetchReportParam[3].Value = strQuarter;

                FetchReportParam[4] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                FetchReportParam[4].Value = ApplicationID;

                FetchReportParam[5] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                FetchReportParam[5].Value = objclsEALLoggedInUser.StrUserSID;


                FetchReportParam[6] = new SqlParameter("@Status", SqlDbType.VarChar, 100);
                FetchReportParam[6].Value = strSignoffStatus;

                FetchReportParam[7] = new SqlParameter("@groupSid", SqlDbType.VarChar, 100);
                FetchReportParam[7].Value = strGroupSID;


                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(ds);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }



        public DataSet GetReportStatusPending(int ReportId, string strRepType, string strquarter)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@ReportId", ReportId),
                new SqlParameter("@RepType", strRepType),
                new SqlParameter("@Quarter", strquarter)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_GetReportStatusPending_DB", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetAccountStatusNoAdmin(int ReportId)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@ReportId", ReportId)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_AccountStatusNoAdmin", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        

        /// </Allreports>
       
        /// <Report>
        /// <Allacounts>
        /// 

        public DataSet GetMultipleApprovals(int ApplicationID, clsEALUser objclsEALApprover)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@ApplicationID", ApplicationID),
                    new SqlParameter("@ControlOwnerSID", objclsEALApprover.StrUserSID)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_GetMultipleApprovals", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet UpdateResetToPendingAllAccounts(string scope, Int32 ReportID, string usersid, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strRight, string GroupSID, string ResetBy)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Scope", scope),
                new SqlParameter("@Quarter", Quarter),
                new SqlParameter("@ApproverSID",objclsEALApprover.StrUserSID ),
                new SqlParameter("@ApplicationID", ApplicationID),
                new SqlParameter("@ReportID", ReportID),
                new SqlParameter("@UserSID", usersid),
                new SqlParameter("@strRight", strRight),
                new SqlParameter("@GroupSID",GroupSID),
                new SqlParameter("@ResetBy",ResetBy),
                                                   
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateResetToPendingAllAcounts", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet UpdateResetToPendingAllSQLAccounts(string scopereset_SQL, clsEALUser objclsEALApprover, string strQuarter, int intAppId, string strSQLUserName, string strDBType, string strDatabase, string strRole, string strServer)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Scope", scopereset_SQL),
                new SqlParameter("@Quarter", strQuarter),
                //new SqlParameter("@ApproverADID",objclsEALApprover.StrUserSID ),
                new SqlParameter("@ApproverADID",objclsEALApprover.StrUserADID ),
                new SqlParameter("@ApproverName",objclsEALApprover.StrUserName ),
                new SqlParameter("@ApplicationID", intAppId),
                new SqlParameter("@strDBType",strDBType),                
                new SqlParameter("@UserName",strSQLUserName),
                new SqlParameter("@Database",strDatabase),
                new SqlParameter("@Server",strServer), 
                new SqlParameter("@Role", strRole)
                                                   
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateResetToPendingAllDBAcounts3", parm);
                //DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateResetToPendingAllDBAcounts3_new", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet UpdateResetToPendingAllLinuxAccounts(string scopereset_Linux, clsEALUser objclsEALApprover, string strQuarter, int intAppId, string strUserid, string strServer, string strloginstatus, string strGroup)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Scope", scopereset_Linux),
                new SqlParameter("@Quarter", strQuarter),
                new SqlParameter("@ApproverADID",objclsEALApprover.StrUserADID ),
                new SqlParameter("@ApproverName",objclsEALApprover.StrUserName ),
                new SqlParameter("@ApplicationID", intAppId),
                new SqlParameter("@UserID",strUserid),
                new SqlParameter("@loginstatus",strloginstatus), 
                new SqlParameter("@Group",strGroup)
                                                   
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateResetToPendingAllLinuxAcounts", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet UpdateResetToPendingAllSecGrpAccounts(string scopereset_Linux, clsEALUser objclsEALApprover, string strQuarter, int intAppId, string strUserid, string strServer)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Scope", scopereset_Linux),
                new SqlParameter("@Quarter", strQuarter),
                new SqlParameter("@ApproverADID",objclsEALApprover.StrUserADID ),
                new SqlParameter("@ApproverName",objclsEALApprover.StrUserName ),
                new SqlParameter("@ApplicationID", intAppId),
                new SqlParameter("@UserName",strUserid),
                //new SqlParameter("@Server",strServer), 
                                                   
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateResetToPendingAllSecGrpAccounts", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet UpdateResetToPendingPSIAccounts(string strQuarter, clsEALUser objclsEALApprover, int intAppId, string strPSIUserName, string strUserID)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Quarter", strQuarter),
                //new SqlParameter("@ApproverADID",objclsEALApprover.StrUserSID ),
                new SqlParameter("@ApproverName",objclsEALApprover.StrUserName ),
                new SqlParameter("@ApplicationID", intAppId),
                new SqlParameter("@UserName",strPSIUserName),
                new SqlParameter("@UserID", strUserID)
                                                   
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateResetToPendingPSIAccounts", parm);
                //DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateResetToPendingAllDBAcounts3_new", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataSet UpdateResetToPending(string scope, Int32 ReportID, string usersid, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string groupSid, string strRight, string ResetBy)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Scope", scope),
                new SqlParameter("@Quarter", Quarter),
                new SqlParameter("@ApproverSID",objclsEALApprover.StrUserSID ),
                new SqlParameter("@ApplicationID", ApplicationID),
                new SqlParameter("@ReportID", ReportID),
                new SqlParameter("@sid", usersid),
                new SqlParameter("@groupSid", groupSid),
                new SqlParameter("@strRight", strRight),
                new SqlParameter("@ResetBy", ResetBy)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateResetToPending", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet UpdateReset_AdminRights(string scope, Int32 ReportID, string usersid, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string groupSid, string strRight)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Scope", scope),
                new SqlParameter("@Quarter", Quarter),
                new SqlParameter("@ApproverSID",objclsEALApprover.StrUserSID ),
                new SqlParameter("@ApplicationID", ApplicationID),
                new SqlParameter("@ReportID", ReportID),
                new SqlParameter("@sid", usersid),
                new SqlParameter("@groupSid", groupSid),
                new SqlParameter("@strRight", strRight)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateReset_AdminRights", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
       /// </Allacounts>
        /// </Report>
        /// <returns></returns>


        public DataTable GroupSchedule()
        {
            try
            {
                return (clsDAL_SqlHelper.ExecuteDataset
                        (
                            strconnectionString,
                            CommandType.StoredProcedure,
                            "SP_GroupSchedule"
                         ).Tables[0]);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet DeleteScheduleMail(string groupID, string Track)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@GroupID", groupID),
                    new SqlParameter("@Track", Track)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_InsertDeleteScheduleMail", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet UpdateScheduleMail( string groupID,DateTime Date,string Subject,string Body,  string Track)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@GroupID", groupID),
                    new SqlParameter("@Date", Date),
                    new SqlParameter("@Subject", Subject),    
                    new SqlParameter("@Body",Body ), 
                    new SqlParameter("@Track", Track)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_InsertDeleteScheduleMail", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet InsertDelete_RoleWiseUser(int roleID, string UserName, string UserADID, string UserEmail, string Track)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@roleID", roleID),
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@UserADID", UserADID),
                    new SqlParameter("@UserEmail", UserEmail),
                    new SqlParameter("@Track", Track),
                  
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_InsertDeleteRoleWiseUser", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet UpdateUserScheduleMail(string UserName, string Track, string groupID)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@GroupID", groupID),
                    new SqlParameter("@UserName", UserName),
                   // new SqlParameter("@UserEmail", UserEmail),      
                    new SqlParameter("@Track", Track)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_InsertDeleteScheduleMail", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet InsertDeleteScheduleMail(int RoleID,string groupID ,string UserName, string UserADID, string UserEmail, string Track, DateTime Date, string Subject, string Body)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@RoleID", RoleID),
                    new SqlParameter("@GroupID", groupID),
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@UserADID", UserADID),
                    new SqlParameter("@UserEmail", UserEmail),
                    new SqlParameter("@Date", Date),
                    new SqlParameter("@Subject", Subject),
                    new SqlParameter("@Body", Body),
                    new SqlParameter("@Track", Track),
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_InsertDeleteScheduleMail", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet Delete_RoleWiseUser(int RoleMappingId, string Track)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
               {
                  
                    new SqlParameter("@RoleMappingId", RoleMappingId),
                    new SqlParameter("@Track", Track),
                  
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "sp_InsertDeleteRoleWiseUser", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataSet GetSignoffByAproverName(string PreviousQuarter, string UserSID, string GroupSID)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@PreviousQuarter", PreviousQuarter),
                    new SqlParameter("@UserSID", UserSID),
                    new SqlParameter("@GroupSID", GroupSID)             
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetSignoffByAproverName", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetSignoffByAproverNameLinux(string PreviousQuarter, string UserID, string loginstatus)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@PreviousQuarter", PreviousQuarter),
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("@loginstatus", loginstatus)
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetSignoffByAproverName_linux", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSignoffByAproverNameSecGrp(string PreviousQuarter, string UserID)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@PreviousQuarter", PreviousQuarter),
                    new SqlParameter("@UserID", UserID),
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetSignoffByAproverName_secGrp", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSignoffByAproverNameDB(string PreviousQuarter, string UserSID, string strReportType)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@PreviousQuarter", PreviousQuarter),
                    new SqlParameter("@UserSID", UserSID),
                    new SqlParameter("@DBType", strReportType)             
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetSignoffByAproverName_DB3", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSignoffByAproverNamePSI(string PreviousQuarter, string UserName, string strCurrQuarter)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                    new SqlParameter("@PreviousQuarter", PreviousQuarter),
                    new SqlParameter("@UserName", UserName),           
                    new SqlParameter("@CurrQuarter", strCurrQuarter),
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetSignoffByAproverName_PSI", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetSignoffByAproverName_Reports(string PreviousQuarter,string userSID, string groupsid, int RepId)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                    new SqlParameter("@PreviousQuarter", PreviousQuarter),
                    new SqlParameter("@userSID", userSID),
                    new SqlParameter("@groupsid", groupsid),
                    new SqlParameter("@RepId", RepId)             
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetSignoffByAproverName_Reports", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdatelinuxAccountComment(string comment, string quarter, string loggedinAdid, string strUserID, string server)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                    new SqlParameter("@Comment", comment),
                    new SqlParameter("@Quarter", quarter),
                    new SqlParameter("@signoffadid", loggedinAdid),
                    new SqlParameter("@userid", strUserID) ,
                    new SqlParameter("@Server", server),
               };

                clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdatelinuxComment_GA", parm);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateSecGrpAccountComment(string comment, string quarter, string loggedinAdid, string strUserID, string server)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                    new SqlParameter("@Comment", comment),
                    new SqlParameter("@Quarter", quarter),
                    new SqlParameter("@signoffadid", loggedinAdid),
                    new SqlParameter("@userid", strUserID) ,
                    new SqlParameter("@Server", server),
               };

                clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateSecGrpComment_GA", parm);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateAccountComment(string comment, string quarter, string loggedinAdid, string strUserName, string strRole, string reptype, string strDB, string server)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                    new SqlParameter("@Comment", comment),
                    new SqlParameter("@Quarter", quarter),
                    new SqlParameter("@signoffadid", loggedinAdid),
                    new SqlParameter("@usernm", strUserName) ,
                    new SqlParameter("@role", strRole),
                    new SqlParameter("@DB", strDB),
                    new SqlParameter("@Server", server),
                    new SqlParameter("@reptype", reptype)
               };

                clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateDBComment_GA", parm);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdatePSIAccountComment(string comment, string quarter,  string strUserName, string strUserID)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                    new SqlParameter("@Comment", comment),
                    new SqlParameter("@Quarter", quarter),
                    new SqlParameter("@usernm", strUserName) ,
                    new SqlParameter("@UserID", strUserID)
               };

                clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdatePSIComment_GA", parm);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //code end here


        public DataSet FetchAllUser(string[] role, int intAppID, string strQuarter, string strPreviousQuarter, string strLoggedInUserName)
        {
            string strRole = "";
            clsEALSession objclsEALSession = new clsEALSession();
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole + ";" + role[i];

            }
            strRole = strRole.Substring(1);
            SqlDataAdapter daFetchAppReports = null;
            if (strRole == "Global Approver")
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetAllAccounts_GlobalApprover", strconnectionString);
            }
            else
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetAllAccounts", strconnectionString);
            }

            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[5];

                FetchReportParam[0] = new SqlParameter("@AppId", SqlDbType.Int);
                FetchReportParam[0].Value = intAppID;

                FetchReportParam[1] = new SqlParameter("@Role", SqlDbType.VarChar);
                FetchReportParam[1].Value = strRole;

                FetchReportParam[2] = new SqlParameter("@SelectedQuarter", SqlDbType.VarChar);
                FetchReportParam[2].Value = strQuarter;

                FetchReportParam[3] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar);
                FetchReportParam[3].Value = strPreviousQuarter;

                FetchReportParam[4] = new SqlParameter("@LoggedInUserNm", SqlDbType.VarChar);
                FetchReportParam[4].Value = strLoggedInUserName;


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
        public DataSet FetchPSIUser(string[] role, int intAppID, string strQuarter, string strPreviousQuarter)
        {
            string strRole = "";
            clsEALSession objclsEALSession = new clsEALSession();
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole + ";" + role[i];

            }
            strRole = strRole.Substring(1);
            SqlDataAdapter daFetchAppReports = null;
            //if (strRole == "Global Approver")
            //{
                daFetchAppReports = new SqlDataAdapter("SP_GetPSIAccounts", strconnectionString);
            //}
            //else
            //{
            //    daFetchAppReports = new SqlDataAdapter("SP_GetAllAccounts21", strconnectionString);
            //}

            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[4];

                FetchReportParam[0] = new SqlParameter("@AppId", SqlDbType.Int);
                FetchReportParam[0].Value = intAppID;

                FetchReportParam[1] = new SqlParameter("@Role", SqlDbType.VarChar);
                FetchReportParam[1].Value = strRole;

                FetchReportParam[2] = new SqlParameter("@SelectedQuarter", SqlDbType.VarChar);
                FetchReportParam[2].Value = strQuarter;

                FetchReportParam[3] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar);
                FetchReportParam[3].Value = strPreviousQuarter;


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

        public DataSet FetchAllSQLAccountUser(string strQuarter, string strPreviousQuarter, int intAppID, string[] role)
        {
            string strRole = "";
            clsEALSession objclsEALSession = new clsEALSession();
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole + ";" + role[i];

            }
            strRole = strRole.Substring(1);
            SqlDataAdapter daFetchAppReports = null;
            if (strRole == "Global Approver")
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetSQLDBAccounts3", strconnectionString);
            }
            else
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetSQLDBAccounts3_CO", strconnectionString);
            }
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@Quarter", SqlDbType.VarChar);
                FetchReportParam[0].Value = strQuarter;

                FetchReportParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strPreviousQuarter;

                FetchReportParam[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                FetchReportParam[2].Value = intAppID;

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

        public DataSet FetchAllLinuxAccountUser(string strQuarter, string strPreviousQuarter, int intAppID, string[] role)
        {
            string strRole = "";
            clsEALSession objclsEALSession = new clsEALSession();
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole + ";" + role[i];

            }
            strRole = strRole.Substring(1);
            SqlDataAdapter daFetchAppReports = null;
            if (strRole == "Global Approver")
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetLinuxAccounts", strconnectionString);
            }
            else
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetlinuxAccounts_CO", strconnectionString);
            }
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@Quarter", SqlDbType.VarChar);
                FetchReportParam[0].Value = strQuarter;

                FetchReportParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strPreviousQuarter;

                FetchReportParam[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                FetchReportParam[2].Value = intAppID;

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
        public DataSet FetchAllSecGrpAccountUser(string strQuarter, string strPreviousQuarter, int intAppID, string[] role)
        {
            string strRole = "";
            clsEALSession objclsEALSession = new clsEALSession();
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole + ";" + role[i];

            }
            strRole = strRole.Substring(1);
            SqlDataAdapter daFetchAppReports = null;
            if (strRole == "Global Approver")
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetSecGrpAccounts", strconnectionString);
            }
            else
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetSecGrpAccounts_CO", strconnectionString);
            }
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@Quarter", SqlDbType.VarChar);
                FetchReportParam[0].Value = strQuarter;

                FetchReportParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strPreviousQuarter;

                FetchReportParam[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                FetchReportParam[2].Value = intAppID;

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

        public string GetAccountComment(string UserNm, string strRole, string strRepType, string strQuarter, string strDB, string strServer)
        {
            SqlDataAdapter daGetComments = new SqlDataAdapter("SP_GetAccountComment", strconnectionString);
            daGetComments.SelectCommand.CommandType = CommandType.StoredProcedure;
            daGetComments.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            string strCommentDate = string.Empty;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchCommentParam = new SqlParameter[6];
                FetchCommentParam[0] = new SqlParameter("@UserNm", SqlDbType.VarChar,100);
                FetchCommentParam[0].Value = UserNm;

                FetchCommentParam[1] = new SqlParameter("@Role", SqlDbType.VarChar, 100);
                FetchCommentParam[1].Value = strRole;

                FetchCommentParam[2] = new SqlParameter("@RepType", SqlDbType.VarChar, 100);
                FetchCommentParam[2].Value = strRepType;

                FetchCommentParam[3] = new SqlParameter("@quarter", SqlDbType.VarChar, 100);
                FetchCommentParam[3].Value = strQuarter;

                FetchCommentParam[4] = new SqlParameter("@DB", SqlDbType.VarChar, 100);
                FetchCommentParam[4].Value = strDB;

                FetchCommentParam[5] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                FetchCommentParam[5].Value = strServer;


                foreach (SqlParameter p in FetchCommentParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daGetComments.SelectCommand.Parameters.Add(p);
                }
                daGetComments.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables.Count != 0)
                    {
                        if (objds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dtrow = objds.Tables[0].Rows[0];
                            if (dtrow != null)
                            {
                                Comment = dtrow["SignOffComment"].ToString();
                            }
                        }

                    }
                }
                return Comment;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string GetAccountCommentLinux(string UserID, string strRepType, string strQuarter,  string strServer)
        {
            SqlDataAdapter daGetComments = new SqlDataAdapter("SP_GetAccountCommentLinux", strconnectionString);
            daGetComments.SelectCommand.CommandType = CommandType.StoredProcedure;
            daGetComments.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            string strCommentDate = string.Empty;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchCommentParam = new SqlParameter[4];
                FetchCommentParam[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                FetchCommentParam[0].Value = UserID;

                FetchCommentParam[1] = new SqlParameter("@RepType", SqlDbType.VarChar, 100);
                FetchCommentParam[1].Value = strRepType;

                FetchCommentParam[2] = new SqlParameter("@quarter", SqlDbType.VarChar, 100);
                FetchCommentParam[2].Value = strQuarter;

                FetchCommentParam[3] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                FetchCommentParam[3].Value = strServer;


                foreach (SqlParameter p in FetchCommentParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daGetComments.SelectCommand.Parameters.Add(p);
                }
                daGetComments.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables.Count != 0)
                    {
                        if (objds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dtrow = objds.Tables[0].Rows[0];
                            if (dtrow != null)
                            {
                                Comment = dtrow["SignOffComment"].ToString();
                            }
                        }

                    }
                }
                return Comment;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string GetPSIComment(string UserNm, string strQuarter )
        {
            SqlDataAdapter daGetComments = new SqlDataAdapter("SP_GetPSIComment", strconnectionString);
            daGetComments.SelectCommand.CommandType = CommandType.StoredProcedure;
            daGetComments.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            string strCommentDate = string.Empty;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchCommentParam = new SqlParameter[2];
                FetchCommentParam[0] = new SqlParameter("@UserNm", SqlDbType.VarChar, 100);
                FetchCommentParam[0].Value = UserNm;

                FetchCommentParam[1] = new SqlParameter("@quarter", SqlDbType.VarChar, 100);
                FetchCommentParam[1].Value = strQuarter;

                foreach (SqlParameter p in FetchCommentParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daGetComments.SelectCommand.Parameters.Add(p);
                }
                daGetComments.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables.Count != 0)
                    {
                        if (objds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dtrow = objds.Tables[0].Rows[0];
                            if (dtrow != null)
                            {
                                Comment = dtrow["SignOffComment"].ToString();
                            }
                        }

                    }
                }
                return Comment;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataSet FetchAllORACLEAccountUser(string strQuarter, string strPreviousQuarter, int intAppID, string[] role)
        {
            string strRole = "";
            clsEALSession objclsEALSession = new clsEALSession();
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole + ";" + role[i];

            }
            strRole = strRole.Substring(1);
            SqlDataAdapter daFetchAppReports = null;
            if (strRole == "Global Approver")
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetORACLEDBAccounts3", strconnectionString);
            }
            else
            {
                daFetchAppReports = new SqlDataAdapter("SP_GetORACLEDBAccounts3_CO", strconnectionString);
            }

            //if (strRole == "Global Approver")
            //{
            //    daFetchAppReports = new SqlDataAdapter("SP_GetORACLEDBAccounts4", strconnectionString);
            //}
            //else
            //{
            //    daFetchAppReports = new SqlDataAdapter("SP_GetORACLEDBAccounts4_CO", strconnectionString);
            //}

            //daFetchAppReports = new SqlDataAdapter("SP_GetORACLEDBAccounts2", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@Quarter", SqlDbType.VarChar);
                FetchReportParam[0].Value = strQuarter;

                FetchReportParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strPreviousQuarter;

                FetchReportParam[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                FetchReportParam[2].Value = intAppID;

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

        public void SignOffUsersByGlobal_SQL(string uName, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, bool IsAdminUpdate, bool IsAdmin, string strDBType, string strRole, string strDB, string strServer)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffGlobal = new SqlParameter[11];
                pSignoffGlobal[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = uName;

                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserADID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@IsAdminupdate", SqlDbType.Bit);
                pSignoffGlobal[5].Value = IsAdminUpdate;


                pSignoffGlobal[6] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                pSignoffGlobal[6].Value = IsAdmin;

                pSignoffGlobal[7] = new SqlParameter("@DBType", SqlDbType.VarChar, 50);
                pSignoffGlobal[7].Value = strDBType;

                pSignoffGlobal[8] = new SqlParameter("@Role", SqlDbType.VarChar, 100);
                pSignoffGlobal[8].Value = strRole;

                pSignoffGlobal[9] = new SqlParameter("@DB", SqlDbType.VarChar, 50);
                pSignoffGlobal[9].Value = strDB;

                pSignoffGlobal[10] = new SqlParameter("@Server", SqlDbType.VarChar, 50);
                pSignoffGlobal[10].Value = strServer;
                
                //cmd.CommandText = "SP_SignOffUserGlobalSQLDB_1";
                //cmd.CommandText = "SP_SignOffUserGlobalSQLDB_2";
                cmd.CommandText = "SP_SignOffUserGlobalSQLDB_3";
                foreach (SqlParameter p in pSignoffGlobal)
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
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffGlobal = new SqlParameter[8];
                pSignoffGlobal[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = uID;

                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserADID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@Server", SqlDbType.VarChar, 50);
                pSignoffGlobal[5].Value = strServer;

                pSignoffGlobal[6] = new SqlParameter("@loginstatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[6].Value = loginstatus ;

                pSignoffGlobal[7] = new SqlParameter("@Group", SqlDbType.VarChar, 50);
                pSignoffGlobal[7].Value = strgroup;

                cmd.CommandText = "SP_SignOffUserGloballinux";
                foreach (SqlParameter p in pSignoffGlobal)
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
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffGlobal = new SqlParameter[6];
                pSignoffGlobal[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = uID;

                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserADID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@Server", SqlDbType.VarChar, 50);
                pSignoffGlobal[5].Value = strServer;

                cmd.CommandText = "[SP_SignOffUserGlobalSecGrp]";
                foreach (SqlParameter p in pSignoffGlobal)
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

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void SignOffPSIUsers(string uName, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, string strUserID)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffPSI = new SqlParameter[5];
                pSignoffPSI[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
                pSignoffPSI[0].Value = uName;

                pSignoffPSI[1] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffPSI[1].Value = signoffstatus;

                pSignoffPSI[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffPSI[2].Value = objclsEALApprover.StrUserName;

                pSignoffPSI[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffPSI[3].Value = Quarter;

                pSignoffPSI[4] = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
                pSignoffPSI[4].Value = strUserID;

                cmd.CommandText = "SP_SignOffPSIUsers";
                foreach (SqlParameter p in pSignoffPSI)
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

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void SignOffUsersByOthersAllAcc_SQL(string scope, Int32 ReportID,string strUserName, string signoffstatus, clsEALUser objclsEALApprover, string Quarter,Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin, string strDBType, string strRole, string strDB, string strServer)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffOther = new SqlParameter[12];
                pSignoffOther[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffOther[0].Value = scope;

                pSignoffOther[1] = new SqlParameter("@ApplicationID", SqlDbType.VarChar,100);
                pSignoffOther[1].Value = ApplicationID;

                pSignoffOther[2] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar,50);
                pSignoffOther[2].Value = signoffstatus;

                pSignoffOther[3] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[3].Value = objclsEALApprover.StrUserName;

                //pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                //pSignoffOther[4].Value = objclsEALApprover.StrUserSID;
                pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[4].Value = objclsEALApprover.StrUserADID;

                pSignoffOther[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = Quarter;

                pSignoffOther[6] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[6].Value = DateTime.Now;

                pSignoffOther[7] = new SqlParameter("@DBType", SqlDbType.VarChar, 50);
                pSignoffOther[7].Value = strDBType;

                pSignoffOther[8] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
                pSignoffOther[8].Value = strUserName;

                pSignoffOther[9] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
                pSignoffOther[9].Value = strRole;

                pSignoffOther[10] = new SqlParameter("@DB", SqlDbType.VarChar, 100);
                pSignoffOther[10].Value = strDB;

                pSignoffOther[11] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                pSignoffOther[11].Value = strServer;

                //cmd.CommandText = "SP_SignoffUserOthersAllAcc_SQL";
                //cmd.CommandText = "SP_SignoffUserOthersAllAcc_DB_Reports";

                cmd.CommandText = "SP_SignoffUserOthersAllAcc_DB_Reports_3_new";
                foreach (SqlParameter p in pSignoffOther)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void SignOffUsersByOthersAllAcc_SQL_AllApp(string scope, Int32 ReportID, string usersid, string strUserName, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin, string strDBType, string strCOSID, string strRole, string strDB, string strServer)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffOther = new SqlParameter[13];
                pSignoffOther[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffOther[0].Value = scope;

                pSignoffOther[1] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[1].Value = ApplicationID;

                pSignoffOther[2] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[2].Value = signoffstatus;

                pSignoffOther[3] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[3].Value = objclsEALApprover.StrUserName;

                //pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                //pSignoffOther[4].Value = objclsEALApprover.StrUserSID;
                pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[4].Value = objclsEALApprover.StrUserADID;

                pSignoffOther[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = Quarter;

                pSignoffOther[6] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[6].Value = DateTime.Now;

                pSignoffOther[7] = new SqlParameter("@DBType", SqlDbType.VarChar, 50);
                pSignoffOther[7].Value = strDBType;

                pSignoffOther[8] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
                pSignoffOther[8].Value = strUserName;

                pSignoffOther[9] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
                pSignoffOther[9].Value = strCOSID;

                pSignoffOther[10] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                pSignoffOther[10].Value = strServer;

                pSignoffOther[11] = new SqlParameter("@Role", SqlDbType.VarChar, 100);
                pSignoffOther[11].Value = strRole;

                pSignoffOther[12] = new SqlParameter("@DB", SqlDbType.VarChar, 100);
                pSignoffOther[12].Value = strDB;

                //cmd.CommandText = "SP_SignoffUserOthersAllAcc_SQL";
                //cmd.CommandText = "SP_SignoffUserOthersAllAcc_DB_Reports";
                cmd.CommandText = "SP_SignoffUserOthersAllAcc_DB_Reports_3_AllApp_new";
                foreach (SqlParameter p in pSignoffOther)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByOthersAllAcc_Linux(string scope, Int32 ReportID, string strUserID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID,    string strServer)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffOther = new SqlParameter[9];
                pSignoffOther[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffOther[0].Value = scope;

                pSignoffOther[1] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[1].Value = ApplicationID;

                pSignoffOther[2] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[2].Value = signoffstatus;

                pSignoffOther[3] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[3].Value = objclsEALApprover.StrUserName;

                pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[4].Value = objclsEALApprover.StrUserADID;

                pSignoffOther[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = Quarter;

                pSignoffOther[6] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[6].Value = DateTime.Now;

                pSignoffOther[7] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                pSignoffOther[7].Value = strUserID;

                pSignoffOther[8] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                pSignoffOther[8].Value = strServer;

                cmd.CommandText = "SP_SignoffUserOthersAllAcc_linux_Reports";
                foreach (SqlParameter p in pSignoffOther)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByOthersAllAcc_Linux_New(string scope, Int32 ReportID, string strUserID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strServer, string loginstatus)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffOther = new SqlParameter[10];
                pSignoffOther[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffOther[0].Value = scope;

                pSignoffOther[1] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[1].Value = ApplicationID;

                pSignoffOther[2] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[2].Value = signoffstatus;

                pSignoffOther[3] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[3].Value = objclsEALApprover.StrUserName;

                pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[4].Value = objclsEALApprover.StrUserADID;

                pSignoffOther[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = Quarter;

                pSignoffOther[6] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[6].Value = DateTime.Now;

                pSignoffOther[7] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                pSignoffOther[7].Value = strUserID;

                pSignoffOther[8] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                pSignoffOther[8].Value = strServer;

                pSignoffOther[9] = new SqlParameter("@loginstatus", SqlDbType.VarChar, 100);
                pSignoffOther[9].Value = loginstatus;

                cmd.CommandText = "SP_SignoffUserOthersAllAcc_linux_Reports_New";
                foreach (SqlParameter p in pSignoffOther)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SignOffUsersByOthersAllAcc_SecGrp(string scope, Int32 ReportID, string strUserID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strServer)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffOther = new SqlParameter[9];
                pSignoffOther[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffOther[0].Value = scope;

                pSignoffOther[1] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[1].Value = ApplicationID;

                pSignoffOther[2] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[2].Value = signoffstatus;

                pSignoffOther[3] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[3].Value = objclsEALApprover.StrUserName;

                pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[4].Value = objclsEALApprover.StrUserADID;

                pSignoffOther[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = Quarter;

                pSignoffOther[6] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[6].Value = DateTime.Now;

                pSignoffOther[7] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                pSignoffOther[7].Value = strUserID;

                pSignoffOther[8] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                pSignoffOther[8].Value = strServer;

                cmd.CommandText = "[SP_SignoffUserOthersAllAcc_SecGrp_Reports]";
                foreach (SqlParameter p in pSignoffOther)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SignOffUsersByOthersAllAcc_Linux_AllApp(string scope, string usersid,  string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID,  string strCOSID,  string strServer)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffOther = new SqlParameter[10];
                pSignoffOther[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffOther[0].Value = scope;

                pSignoffOther[1] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[1].Value = ApplicationID;

                pSignoffOther[2] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[2].Value = signoffstatus;

                pSignoffOther[3] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[3].Value = objclsEALApprover.StrUserName;

                pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[4].Value = objclsEALApprover.StrUserADID;

                pSignoffOther[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = Quarter;

                pSignoffOther[6] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[6].Value = DateTime.Now;

                pSignoffOther[7] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                pSignoffOther[7].Value = usersid;

                pSignoffOther[8] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
                pSignoffOther[8].Value = strCOSID;

                pSignoffOther[9] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                pSignoffOther[9].Value = strServer;


                cmd.CommandText = "SP_SignoffUserOthersAllAcc_linux_Reports_AllApp";
                foreach (SqlParameter p in pSignoffOther)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SignOffUsersByOthersAllAcc_SecGrp_AllApp(string scope, string usersid, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strCOSID, string strServer)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pSignoffOther = new SqlParameter[10];
                pSignoffOther[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffOther[0].Value = scope;

                pSignoffOther[1] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[1].Value = ApplicationID;

                pSignoffOther[2] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[2].Value = signoffstatus;

                pSignoffOther[3] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[3].Value = objclsEALApprover.StrUserName;

                pSignoffOther[4] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[4].Value = objclsEALApprover.StrUserADID;

                pSignoffOther[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = Quarter;

                pSignoffOther[6] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[6].Value = DateTime.Now;

                pSignoffOther[7] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                pSignoffOther[7].Value = usersid;

                pSignoffOther[8] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
                pSignoffOther[8].Value = strCOSID;

                pSignoffOther[9] = new SqlParameter("@Server", SqlDbType.VarChar, 100);
                pSignoffOther[9].Value = strServer;


                cmd.CommandText = "SP_SignoffUserOthersAllAcc_SecGrp_Reports_AllApp";
                foreach (SqlParameter p in pSignoffOther)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FetchAssignedUser(string strApproverAdid, int intAppID, string strQuarter, string strprevquarter,string strCOSID,string strScope,string strreportType)
        {
            
            clsEALSession objclsEALSession = new clsEALSession();

            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetAssignedUsers", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[7];

                FetchReportParam[0] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 50);
                FetchReportParam[0].Value = strApproverAdid;

                FetchReportParam[1] = new SqlParameter("@AppId", SqlDbType.Int);
                FetchReportParam[1].Value = intAppID;

                FetchReportParam[2] = new SqlParameter("@SelectedQuarter", SqlDbType.VarChar, 50);
                FetchReportParam[2].Value = strQuarter;

                FetchReportParam[3] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                FetchReportParam[3].Value = strprevquarter;

                FetchReportParam[4] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
                FetchReportParam[4].Value = strCOSID;

                FetchReportParam[5] = new SqlParameter("@Scope", SqlDbType.VarChar, 100);
                FetchReportParam[5].Value = strScope;

                FetchReportParam[6] = new SqlParameter("@DBType", SqlDbType.VarChar, 100);
                FetchReportParam[6].Value = strreportType;

                
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

        public DataSet FetchAssignedUser_DB(string strApproverAdid, int intAppID, string strQuarter, string strprevquarter, string strCOSID, string strScope, string strReportType)
        {

            clsEALSession objclsEALSession = new clsEALSession();

            //SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetAssignedUsers_DB1", strconnectionString);
            //SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetAssignedUsers_DB1_2", strconnectionString);
            //SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetAssignedUsers_DB1_211", strconnectionString);
            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetAssignedUsers_DB1_2111_new", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[7];

                FetchReportParam[0] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 50);
                FetchReportParam[0].Value = strApproverAdid;

                FetchReportParam[1] = new SqlParameter("@AppId", SqlDbType.Int);
                FetchReportParam[1].Value = intAppID;

                FetchReportParam[2] = new SqlParameter("@SelectedQuarter", SqlDbType.VarChar, 50);
                FetchReportParam[2].Value = strQuarter;

                FetchReportParam[3] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                FetchReportParam[3].Value = strprevquarter;

                FetchReportParam[4] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
                FetchReportParam[4].Value = strCOSID;

                FetchReportParam[5] = new SqlParameter("@Scope", SqlDbType.VarChar, 100);
                FetchReportParam[5].Value = strScope;

                FetchReportParam[6] = new SqlParameter("@DBType", SqlDbType.VarChar, 100);
                FetchReportParam[6].Value = strReportType;


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
        public DataSet FetchAssignedUser_PSI_DB(string strApproverAdid, int intAppID, string strQuarter, string strprevquarter, string strCOSID, string strScope, string strReportType)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                    new SqlParameter("@ApproverADID", strApproverAdid),
                    new SqlParameter("@AppId", intAppID),
                    new SqlParameter("@SelectedQuarter", strQuarter),
                    new SqlParameter("@PreviousQuarter", strprevquarter) ,
                    new SqlParameter("@COSID", strCOSID),
                    new SqlParameter("@Scope", strScope) ,
                    new SqlParameter("@DBType", strReportType)
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetAssignedUsers_PSI_DB", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet FetchAssignedUser_Linux(string strApproverAdid, int intAppID, string strQuarter, string strprevquarter, string strCOSID, string strScope, string strReportType)
        {

            clsEALSession objclsEALSession = new clsEALSession();

            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetAssignedUsers_linux", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[7];

                FetchReportParam[0] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 50);
                FetchReportParam[0].Value = strApproverAdid;

                FetchReportParam[1] = new SqlParameter("@AppId", SqlDbType.Int);
                FetchReportParam[1].Value = intAppID;

                FetchReportParam[2] = new SqlParameter("@SelectedQuarter", SqlDbType.VarChar, 50);
                FetchReportParam[2].Value = strQuarter;

                FetchReportParam[3] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                FetchReportParam[3].Value = strprevquarter;

                FetchReportParam[4] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
                FetchReportParam[4].Value = strCOSID;

                FetchReportParam[5] = new SqlParameter("@Scope", SqlDbType.VarChar, 100);
                FetchReportParam[5].Value = strScope;

                FetchReportParam[6] = new SqlParameter("@DBType", SqlDbType.VarChar, 100);
                FetchReportParam[6].Value = strReportType;


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
        public DataSet GetNewUsers(string strCurrQuarter, string strPreviousQuarter)
        {
            
            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_Newusers1", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[1];

                FetchReportParam[0] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[0].Value = strCurrQuarter;

                //FetchReportParam[1] = new SqlParameter("@PrevQuarter", SqlDbType.VarChar);
                //FetchReportParam[1].Value = strPreviousQuarter;


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
        }

        public string LastApprovedBy(string strUSerSID,string strCurrQuarter)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                         strLastApprover = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastApprover;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataSet LastApprovedByDetails(string strUSerSID, string strCurrQuarter)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedbyDetails", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);

                return objds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string LastSQLApprovedBy(string strUSerSID, string strCurrQuarter, string strDBType)
        {

            //SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_SQLDB", strconnectionString);
            //SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_SQL", strconnectionString);
            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_SQL3", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                FetchReportParam[2] = new SqlParameter("@DBType", SqlDbType.VarChar);
                FetchReportParam[2].Value = strDBType;

                FetchReportParam[2] = new SqlParameter("@DBType", SqlDbType.VarChar);
                FetchReportParam[2].Value = strDBType;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastApprover = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastApprover;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataSet LastSQLApprovedByDetails(string strUSerSID, string strCurrQuarter, string strDBType)
        {
            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_details", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                FetchReportParam[2] = new SqlParameter("@DBType", SqlDbType.VarChar);
                FetchReportParam[2].Value = strDBType;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);

                return objds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string LastLinuxApprovedBy(string strUSerSID, string strCurrQuarter, string strDBType, string loginstatus)
        {
            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_Linux", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[4];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                FetchReportParam[2] = new SqlParameter("@DBType", SqlDbType.VarChar);
                FetchReportParam[2].Value = strDBType;

                FetchReportParam[2] = new SqlParameter("@DBType", SqlDbType.VarChar);
                FetchReportParam[2].Value = strDBType;

                FetchReportParam[3] = new SqlParameter("@loginstatus", SqlDbType.VarChar);
                FetchReportParam[3].Value = loginstatus;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastApprover = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastApprover;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string LastPSIApprovedBy(string strUSerName, string strCurrQuarter)
        {

            //SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_SQLDB", strconnectionString);
            //SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_SQL", strconnectionString);
            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_PSI", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@UserNm", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerName;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastApprover = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastApprover;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string LastApprovedBy_ADSec(string strUSerSID, string strCurrQuarter)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_ADSec", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@SamAccountName", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastApprover = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastApprover;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataSet LastApprovedBy_ADSecDetails(string strUSerSID, string strCurrQuarter)
        {
            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_ADSecDetails", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@SamAccountName", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);

                return objds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string LastSQLApprovedBy_SQL(string strUSerSID, string strCurrQuarter)
        {

            //SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_SQL", strconnectionString);
            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastApprovedby_SQL3", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastApprover = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastApprover = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastApprover;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string LastStatus(string strUSerSID, string strCurrQuarter)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastStatus", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastStatus = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastStatus = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastStatus;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string LastStatus_SQL(string strUSerSID, string strRole, string strDB, string strServer, string strCurrQuarter)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastStatus_SQL_new", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastStatus = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[5];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerSID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                FetchReportParam[2] = new SqlParameter("@Server", SqlDbType.VarChar);
                FetchReportParam[2].Value = strServer;

                FetchReportParam[3] = new SqlParameter("@Role", SqlDbType.VarChar);
                FetchReportParam[3].Value = strRole;

                FetchReportParam[4] = new SqlParameter("@DB", SqlDbType.VarChar);
                FetchReportParam[4].Value = strDB;


                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastStatus = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastStatus;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string LastStatus_Linux(string strUSerID,  string strServer, string strCurrQuarter)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastStatus_Linux", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastStatus = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@UserID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                FetchReportParam[2] = new SqlParameter("@Server", SqlDbType.VarChar);
                FetchReportParam[2].Value = strServer;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastStatus = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastStatus;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public string LastStatus_SecGrp(string strUSerID, string strServer, string strCurrQuarter)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastStatus_SecGrp", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastStatus = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@UserID", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerID;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                FetchReportParam[2] = new SqlParameter("@Server", SqlDbType.VarChar);
                FetchReportParam[2].Value = strServer;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastStatus = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastStatus;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public string LastStatus_PSI(string strLoggedInUserNm, string strCurrQuarter)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastStatus_PSI", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastStatus = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@UserNm", SqlDbType.VarChar);
                FetchReportParam[0].Value = strLoggedInUserNm;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;


                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    dalastapprovers.SelectCommand.Parameters.Add(p);
                }
                dalastapprovers.Fill(objds);
                if (objds != null)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        strLastStatus = objds.Tables[0].Rows[0][0].ToString();
                    }
                }

                return strLastStatus;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool InsertNewUser(clsEALUser user, clsEALRoles role, string modifiedby)
        {
            return true;
        }
        public bool UpdateUser(clsEALUser user, string strUserRole, string modifiedby)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_UpdateUserRole", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            string strUserName = user.StrUserName;
            string stremail = user.StrUserEmailID;

            string strUserSID = user.StrUserSID;
            string strRole = strUserRole;
            string strUserADID = user.StrUserADID;
            DataSet dsDelete = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[6];

                FetchReportParam[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
                FetchReportParam[0].Value = strUserName;

                FetchReportParam[1] = new SqlParameter("@UserEmailID", SqlDbType.VarChar, 100);
                FetchReportParam[1].Value = stremail;

                FetchReportParam[2] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
                FetchReportParam[2].Value = strUserSID;

                FetchReportParam[3] = new SqlParameter("@UserRole", SqlDbType.VarChar, 50);
                FetchReportParam[3].Value = strRole;

                FetchReportParam[4] = new SqlParameter("@UserADID", SqlDbType.VarChar, 50);
                FetchReportParam[4].Value = strUserADID;

                FetchReportParam[5] = new SqlParameter("@modifiedby", SqlDbType.VarChar, 100);
                FetchReportParam[5].Value = modifiedby;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(dsDelete);
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public bool DeleteUser(clsEALUser user, string modifiedby)
        {
            //[SP_DeleteUserRole]
            SqlDataAdapter da = new SqlDataAdapter("SP_DeleteUserRole", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            string strUserSID = user.StrUserSID;
            DataSet dsDelete = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[2];

                FetchReportParam[0] = new SqlParameter("@userSID", SqlDbType.VarChar,50);
                FetchReportParam[0].Value = strUserSID;


                FetchReportParam[1] = new SqlParameter("@modifiedby", SqlDbType.VarChar, 100);
                FetchReportParam[1].Value = modifiedby;
                

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(dsDelete);
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public bool DeleteUserFromDb(string userADID)
        {
            //[SP_DeleteUserRole]
            SqlDataAdapter da = new SqlDataAdapter("delete from tblsoxUserRoles where UserAdid='" + userADID+"'", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.CommandTimeout = 3600;
           
            DataSet dsDelete = new DataSet();
           
                da.Fill(dsDelete);
                return true;
            
        }
        public bool DeleteApproverFromDb(string userADID)
        {
            //[SP_DeleteUserRole]
            SqlDataAdapter da = new SqlDataAdapter("delete from tblsoxApproverMapping where ApproverADID='" + userADID + "'", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.CommandTimeout = 3600;

            DataSet dsDelete = new DataSet();

            da.Fill(dsDelete);
            return true;

        }
        public bool DeleteCOFromDb(string userADID)
        {
            //[SP_DeleteUserRole]
            SqlDataAdapter da = new SqlDataAdapter("delete from tblsoxApplicationDetails where ControlOwnerADID='" + userADID + "'", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.CommandTimeout = 3600;

            DataSet dsDelete = new DataSet();

            da.Fill(dsDelete);
            return true;

        }
        public bool CheckRole(string strRole)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckIfRoleExists", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                SqlConnection conn = new SqlConnection(strconnectionString);

                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(strconnectionString);
                conn.Open();

                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
                para[0].Value = strRole;

                para[1] = new SqlParameter("@flagexists", SqlDbType.Bit);
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
                con.Close();
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public bool CheckIFCO(string loggedInUserSID)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckIfCO", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();
                SqlConnection con= new SqlConnection(strconnectionString);
                con.Open();

                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                para[0].Value = loggedInUserSID;

                para[1] = new SqlParameter("@IsCO", SqlDbType.Bit);
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
                con.Close();
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void InsertBMCTable(string scope, string strQuarter, Int32 ReportID, clsEALUser objclsEALoggedInUser, Int32 ApplicationID, string usersid, string strUserNm, string groupSid, bool  IsRemoved)
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_BMCRemovalMail", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
           
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[11];

                FetchReportParam[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                FetchReportParam[0].Value = usersid;

                //FetchReportParam[1] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                //FetchReportParam[1].Value = strGroupSID;

                FetchReportParam[1] = new SqlParameter("@RepID", SqlDbType.Int);
                FetchReportParam[1].Value = ReportID;

                FetchReportParam[2] = new SqlParameter("@Scope", SqlDbType.VarChar,20);
                FetchReportParam[2].Value = scope;

                FetchReportParam[3] = new SqlParameter("@COName", SqlDbType.VarChar, 200);
                FetchReportParam[3].Value = objclsEALoggedInUser.StrUserName
;

                FetchReportParam[4] = new SqlParameter("@AppID", SqlDbType.Int);
                FetchReportParam[4].Value = ApplicationID;

                FetchReportParam[5] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
                FetchReportParam[5].Value = objclsEALoggedInUser.StrUserSID;


                FetchReportParam[6] = new SqlParameter("@date", SqlDbType.VarChar, 100);
                FetchReportParam[6].Value = DateTime.Now;

                FetchReportParam[7] = new SqlParameter("@IsRemoved", SqlDbType.Bit);
                FetchReportParam[7].Value = IsRemoved;

                FetchReportParam[8] = new SqlParameter("@UserName", SqlDbType.VarChar, 200);
                FetchReportParam[8].Value = strUserNm;

                FetchReportParam[9] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                FetchReportParam[9].Value = groupSid;

                FetchReportParam[10] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                FetchReportParam[10].Value = strQuarter;


                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(ds);
               
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        public DataSet FetchReportData(string Type, string strPreviousQuarter, string strQuarter)
        {
            SqlDataAdapter daFetchReports = null;
            daFetchReports = new SqlDataAdapter("SP_RemovedReport_Freq", strconnectionString);

            daFetchReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@Type", SqlDbType.Int);
                FetchReportParam[0].Value = Type;

                FetchReportParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strPreviousQuarter;

                FetchReportParam[2] = new SqlParameter("@CurrentQuarter", SqlDbType.VarChar);
                FetchReportParam[2].Value = strQuarter;

                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daFetchReports.SelectCommand.Parameters.Add(p);
                }
                daFetchReports.Fill(objds);
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

        public void ADUsersAdd(string mygroup, string childitems)
        {

            SqlConnection con = new SqlConnection(strconnectionString);
            SqlCommand cmd = new SqlCommand("testADUsersAdd", con);

            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mygroup", mygroup);
            cmd.Parameters.AddWithValue("@childitems", childitems); 

            cmd.ExecuteNonQuery();

            con.Close();

        }


    }
}
