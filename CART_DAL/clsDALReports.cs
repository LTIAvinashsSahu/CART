using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CART_EAL;
using System.Data.SqlClient;

namespace CART_DAL
{
    public class clsDALReports : clsDBConnection
    {
        #region DataMembers
        SqlCommand cmd = null;
        //SqlConnection con = null;
        DataSet objds = null;
        #endregion

        #region RecievedReport
        public DataSet GetAllRecievedReports(string Quarter)
        {

            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP-1", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[1];
                
                FetchReportParam[0] = new SqlParameter("@Quarter", SqlDbType.VarChar,50);
                FetchReportParam[0].Value = Quarter;

                
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
        #endregion
        public DataSet GetApplicationLinuxUsers(int AppID, string strQuarter)
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

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCRemoval_Linux", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Added by Nag
        public DataSet GetPendingApprovalUsers(string approverName, string reportType)
        {
            string strConnStr = "";
            string spname = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
                {
                    new SqlParameter("@signoffbyaprovername", approverName),
                };

                switch (reportType)
                {
                    case "Linux":
                        spname = "sp_OutstandingRecertification_Linux";
                        break;
                    case "Oracle":
                        spname = "sp_OutstandingRecertification_Oracle";
                        break;
                    case "SQL":
                        spname = "sp_OutstandingRecertification_SQL";
                        break;
                    case "Server/Share":
                        spname = "sp_OutstandingRecertification_ServerShare";
                        break;
                    case "Security Group":
                        spname = "sp_OutstandingRecertification_SecGrp";
                        break;
                    case "Online Databases":
                        spname = "sp_OutstandingRecertification_Online";
                        break;
                }

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, spname, parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //Added by Nag
        public DataSet GetLoadExceptionReports(string reportType)
        {
            string strConnStr = "";
            string spname = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
                {
                    new SqlParameter("@ReportType", reportType),
                };

                spname = "sp_MappingReport_ServerShare";

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, spname, parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        //Added by Nag
        public DataSet GetTicketReports(string strQuarter, string reporttype)
        {
            string strConnStr = "";
            string spname = "";
            try
            {
                strConnStr = strconnectionString;

                switch (reporttype)
                {
                    case "Server/Share":
                        spname = "sp_SNTicketReport_ServerShare";
                        break;
                    case "SQL":
                        spname = "sp_SNTicketReport_SQL";
                        break;
                    case "Linux":
                        spname = "sp_SNTicketReport_Linux";
                        break;
                    case "Oracle":
                        spname = "sp_SNTicketReport_Oracle";
                        break;
                    case "Security Group":
                        spname = "sp_SNTicketReport_SecGrp";
                        break;
                    case "Online Databases":
                        spname = "sp_SNTicketReport_Online";
                        break;
                }
              
                SqlParameter[] parm = new SqlParameter[] 
                { 
                    new SqlParameter("@Quarter", strQuarter)
                                    
                };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, spname, parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //Added by Nag
        public DataSet GetUnmappedSecGrpReport()
        {
            string strConnStr = "";
            string spname = "sp_UnmappedSecGroup";
            try
            {
                strConnStr = strconnectionString;

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, spname);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        public DataSet GetApplicationSQLUsers(int AppID, string strQuarter)
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

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCRemoval_Sql", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetApplicationSecGrpUsers(int AppID, string strQuarter)
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

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCRemoval_SecGrp", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetApplicationPSILUsers(int AppID, string strQuarter)
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

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCRemoval_PSI", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetApplicationOracleUsers(int AppID, string strQuarter)
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

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCRemoval_Oracle", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetDistinctSQLReportsForUser(int AppID, string strUserNm, string strRole, string strQuarter)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;
                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@AppID", AppID),
                new SqlParameter("@UserNm", strUserNm),
                new SqlParameter("@URole", strRole),
                new SqlParameter("@Quarter", strQuarter)
                                    
              };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCUserReports_SQL", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetDistinctLinuxReportsForUser(int AppID, string strUserID, string strQuarter)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;
                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@AppID", AppID),
                new SqlParameter("@UserID", strUserID),
                new SqlParameter("@Quarter", strQuarter)
                                    
              };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCUserReports_Linux", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetDistinctSecGrpReportsForUser(int AppID, string strUserID, string strQuarter)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;
                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@AppID", AppID),
                new SqlParameter("@UserName", strUserID),
                new SqlParameter("@Quarter", strQuarter)
                                    
              };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "[SP_BMCUserReports_SecGrp]", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetDistinctOracleReportsForUser(int AppID, string strUserNm, string strRole, string strQuarter)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;
                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@AppID", AppID),
                new SqlParameter("@UserNm", strUserNm),
                new SqlParameter("@URole", strRole),
                new SqlParameter("@Quarter", strQuarter)
                                    
              };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_BMCUserReports_Oracle", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #region GetAllReports
        public DataSet GetAllReport(int ApplicationID, string[] role, string Quarter)
        {
            string strRole = "";
            clsEALSession objclsEALSession = new clsEALSession();
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole + ";" + role[i];

            }
            strRole = strRole.Substring(1);
            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetAllReports1_DB2", strconnectionString);
            //SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_GetAllReports168", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;

            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@AppId", SqlDbType.Int);
                FetchReportParam[0].Value = ApplicationID;

                FetchReportParam[1] = new SqlParameter("@Role", SqlDbType.VarChar);
                FetchReportParam[1].Value = strRole;

                FetchReportParam[2] = new SqlParameter("@Quarter", SqlDbType.VarChar);
                FetchReportParam[2].Value = Quarter;

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
        #endregion

        #region Search
        public DataSet SearchAllReport(string Type, string Quarter, int AppID, string SecurityGroup, string LastApproved, string AccountName, string signoffStatus, string Accountstatus, string LoggedInUser, string PreviousQuartertoSelected)
        {
            clsEALSession objclsEALSession = new clsEALSession();

            SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_Search", strconnectionString);
            daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchAppReports.SelectCommand.CommandTimeout = 36000;

            //string strName = objclsEALUser.StrUserName;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[10];

                FetchReportParam[0] = new SqlParameter("@Quarter", SqlDbType.VarChar);
                FetchReportParam[0].Value = Quarter;

                FetchReportParam[1] = new SqlParameter("@AppID", SqlDbType.Int);
                FetchReportParam[1].Value = AppID;

                FetchReportParam[2] = new SqlParameter("@SecurityGroup", SqlDbType.VarChar);
                FetchReportParam[2].Value = SecurityGroup;

                FetchReportParam[3] = new SqlParameter("@LastApproved", SqlDbType.VarChar);
                FetchReportParam[3].Value = LastApproved;

                FetchReportParam[4] = new SqlParameter("@AccountName", SqlDbType.VarChar);
                FetchReportParam[4].Value = AccountName;

                FetchReportParam[5] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar);
                FetchReportParam[5].Value = signoffStatus;

                FetchReportParam[6] = new SqlParameter("@AccountStatus", SqlDbType.VarChar);
                FetchReportParam[6].Value = Accountstatus;

                FetchReportParam[7] = new SqlParameter("@Type", SqlDbType.VarChar);
                FetchReportParam[7].Value = Type;

                FetchReportParam[8] = new SqlParameter("@LoggedInUserNm", SqlDbType.VarChar);
                FetchReportParam[8].Value = LoggedInUser;

                FetchReportParam[9] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar);
                FetchReportParam[9].Value = PreviousQuartertoSelected;

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
        #endregion

        #region FetchReportDetails
        public DataSet GetReportDetails(int ReportID, string strReportType, string SelectedQuarter)
        {

            SqlDataAdapter daFetchReportDetails = new SqlDataAdapter("SP_GetReportDetails_DB", strconnectionString);
            daFetchReportDetails.SelectCommand.CommandType = CommandType.StoredProcedure;
            daFetchReportDetails.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            
                SqlParameter[] FetchReportParam = new SqlParameter[3];
                FetchReportParam[0] = new SqlParameter("@RepId", SqlDbType.Int);
                FetchReportParam[0].Value = ReportID;

                FetchReportParam[1] = new SqlParameter("@RepType", SqlDbType.VarChar, 20);
                FetchReportParam[1].Value = strReportType;

                FetchReportParam[2] = new SqlParameter("@SelectedQuarter", SqlDbType.VarChar, 20);
                FetchReportParam[2].Value = SelectedQuarter;


                foreach (SqlParameter p in FetchReportParam)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daFetchReportDetails.SelectCommand.Parameters.Add(p);
                }
                daFetchReportDetails.Fill(objds);
                return objds;
           


        }
        #endregion


        #region Get Users of a Report
        public DataSet GetReportUsers(int ReportID, string strRepType, string strQuarter, string PreviousQuarter, int ApplicationID, bool IsGlobalApprover, string LoggeInUser, string[] role)
        {

            if (strRepType == "SQLReport" || strRepType == "OracleReport")
            {
                SqlDataAdapter daFetchReportUsers = new SqlDataAdapter("SP_GetReportUsers_DB", strconnectionString);
                daFetchReportUsers.SelectCommand.CommandType = CommandType.StoredProcedure;
                daFetchReportUsers.SelectCommand.CommandTimeout = 36000;
                objds = new DataSet();
                try
                {
                    SqlParameter[] FetchReportuserParam = new SqlParameter[6];
                    FetchReportuserParam[0] = new SqlParameter("@RepId", SqlDbType.Int);
                    FetchReportuserParam[0].Value = ReportID;

                    FetchReportuserParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[1].Value = PreviousQuarter;

                    FetchReportuserParam[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                    FetchReportuserParam[2].Value = ApplicationID;

                    FetchReportuserParam[3] = new SqlParameter("@IsGloabalApprover", SqlDbType.Bit);
                    FetchReportuserParam[3].Value = IsGlobalApprover;

                    FetchReportuserParam[4] = new SqlParameter("@ReportType", SqlDbType.VarChar, 50);
                    FetchReportuserParam[4].Value = strRepType;

                    FetchReportuserParam[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[5].Value = strQuarter;


                    foreach (SqlParameter p in FetchReportuserParam)
                    {
                        if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        daFetchReportUsers.SelectCommand.Parameters.Add(p);
                    }
                    daFetchReportUsers.Fill(objds);
                    return objds;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }

            else if (strRepType == "LinuxReport")
            {
                
                SqlDataAdapter daFetchReportUsers = new SqlDataAdapter("SP_GetReportUsers_Linux", strconnectionString);
                daFetchReportUsers.SelectCommand.CommandType = CommandType.StoredProcedure;
                daFetchReportUsers.SelectCommand.CommandTimeout = 36000;
                objds = new DataSet();
                try
                {
                    SqlParameter[] FetchReportuserParam = new SqlParameter[4];
                    FetchReportuserParam[0] = new SqlParameter("@RepId", SqlDbType.Int);
                    FetchReportuserParam[0].Value = ReportID;

                    FetchReportuserParam[1] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                    FetchReportuserParam[1].Value = ApplicationID;

                    FetchReportuserParam[2] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[2].Value = strQuarter;

                    FetchReportuserParam[3] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[3].Value = PreviousQuarter;


                    foreach (SqlParameter p in FetchReportuserParam)
                    {
                        if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        daFetchReportUsers.SelectCommand.Parameters.Add(p);
                    }
                    daFetchReportUsers.Fill(objds);
                    return objds;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
 
            }
            else if (strRepType == "SecurityGroupReport")
            {

                SqlDataAdapter daFetchReportUsers = new SqlDataAdapter("SP_GetReportUsers_SecGrp", strconnectionString);
                daFetchReportUsers.SelectCommand.CommandType = CommandType.StoredProcedure;
                daFetchReportUsers.SelectCommand.CommandTimeout = 36000;
                objds = new DataSet();
                try
                {
                    SqlParameter[] FetchReportuserParam = new SqlParameter[4];
                    FetchReportuserParam[0] = new SqlParameter("@RepId", SqlDbType.Int);
                    FetchReportuserParam[0].Value = ReportID;

                    FetchReportuserParam[1] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                    FetchReportuserParam[1].Value = ApplicationID;

                    FetchReportuserParam[2] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[2].Value = strQuarter;

                    FetchReportuserParam[3] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[3].Value = PreviousQuarter;


                    foreach (SqlParameter p in FetchReportuserParam)
                    {
                        if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        daFetchReportUsers.SelectCommand.Parameters.Add(p);
                    }
                    daFetchReportUsers.Fill(objds);
                    return objds;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }

            }
            else
            {
                string strRole = "";
                clsEALSession objclsEALSession = new clsEALSession();
                for (int i = 0; i < role.Length; i++)
                {
                    strRole = strRole + ";" + role[i];

                }
                strRole = strRole.Substring(1);
                //SqlDataAdapter daFetchReportUsers = new SqlDataAdapter(" ", strconnectionString);
                SqlDataAdapter daFetchReportUsers = new SqlDataAdapter("SP_GetReportUsers", strconnectionString);
                daFetchReportUsers.SelectCommand.CommandType = CommandType.StoredProcedure;
                daFetchReportUsers.SelectCommand.CommandTimeout = 36000;
                objds = new DataSet();
                try
                {
                    SqlParameter[] FetchReportuserParam = new SqlParameter[6];
                    FetchReportuserParam[0] = new SqlParameter("@RepId", SqlDbType.Int);
                    FetchReportuserParam[0].Value = ReportID;

                    FetchReportuserParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[1].Value = PreviousQuarter;

                    FetchReportuserParam[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                    FetchReportuserParam[2].Value = ApplicationID;

                    FetchReportuserParam[3] = new SqlParameter("@IsGloabalApprover", SqlDbType.Bit);
                    FetchReportuserParam[3].Value = IsGlobalApprover;

                    FetchReportuserParam[4] = new SqlParameter("@LoggedInUserNm", SqlDbType.VarChar);
                    FetchReportuserParam[4].Value = LoggeInUser;

                    FetchReportuserParam[5] = new SqlParameter("@Role", SqlDbType.VarChar);
                    FetchReportuserParam[5].Value = strRole;


                    foreach (SqlParameter p in FetchReportuserParam)
                    {
                        if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        daFetchReportUsers.SelectCommand.Parameters.Add(p);
                    }
                    daFetchReportUsers.Fill(objds);

                    return objds;
                }

                catch (Exception Ex)
                {
                    throw Ex;
                }
            }
            
            
            
        }
        #endregion

        #region Get Search Details
        public DataSet GetSearchDetails(int ReportID, string strRepType, string strQuarter, string PreviousQuarter, int ApplicationID, bool IsGlobalApprover, string LoggeInUser, string[] role, Int32 UserID)
        {

            if (strRepType == "SQLReport" || strRepType == "OracleReport")
            {
                SqlDataAdapter daFetchReportUsers = new SqlDataAdapter("SP_GetSearchDetails_DB", strconnectionString);
                daFetchReportUsers.SelectCommand.CommandType = CommandType.StoredProcedure;
                daFetchReportUsers.SelectCommand.CommandTimeout = 36000;
                objds = new DataSet();
                try
                {
                    SqlParameter[] FetchReportuserParam = new SqlParameter[7];
                    FetchReportuserParam[0] = new SqlParameter("@RepId", SqlDbType.Int);
                    FetchReportuserParam[0].Value = ReportID;

                    FetchReportuserParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[1].Value = PreviousQuarter;

                    FetchReportuserParam[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                    FetchReportuserParam[2].Value = ApplicationID;

                    FetchReportuserParam[3] = new SqlParameter("@IsGloabalApprover", SqlDbType.Bit);
                    FetchReportuserParam[3].Value = IsGlobalApprover;

                    FetchReportuserParam[4] = new SqlParameter("@ReportType", SqlDbType.VarChar, 50);
                    FetchReportuserParam[4].Value = strRepType;

                    FetchReportuserParam[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[5].Value = strQuarter;

                    FetchReportuserParam[6] = new SqlParameter("@Para_UserID", SqlDbType.Int, 50);
                    FetchReportuserParam[6].Value = UserID;



                    foreach (SqlParameter p in FetchReportuserParam)
                    {
                        if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        daFetchReportUsers.SelectCommand.Parameters.Add(p);
                    }
                    daFetchReportUsers.Fill(objds);
                    return objds;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }
            else
            {
                string strRole = "";
                clsEALSession objclsEALSession = new clsEALSession();
                for (int i = 0; i < role.Length; i++)
                {
                    strRole = strRole + ";" + role[i];

                }
                strRole = strRole.Substring(1);
                SqlDataAdapter daFetchReportUsers = new SqlDataAdapter("SP_GetReportUsers", strconnectionString);
                daFetchReportUsers.SelectCommand.CommandType = CommandType.StoredProcedure;
                daFetchReportUsers.SelectCommand.CommandTimeout = 36000;
                objds = new DataSet();
                try
                {
                    SqlParameter[] FetchReportuserParam = new SqlParameter[6];
                    FetchReportuserParam[0] = new SqlParameter("@RepId", SqlDbType.Int);
                    FetchReportuserParam[0].Value = ReportID;

                    FetchReportuserParam[1] = new SqlParameter("@PreviousQuarter", SqlDbType.VarChar, 50);
                    FetchReportuserParam[1].Value = PreviousQuarter;

                    FetchReportuserParam[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                    FetchReportuserParam[2].Value = ApplicationID;

                    FetchReportuserParam[3] = new SqlParameter("@IsGloabalApprover", SqlDbType.Bit);
                    FetchReportuserParam[3].Value = IsGlobalApprover;

                    FetchReportuserParam[4] = new SqlParameter("@LoggedInUserNm", SqlDbType.VarChar);
                    FetchReportuserParam[4].Value = LoggeInUser;

                    FetchReportuserParam[5] = new SqlParameter("@Role", SqlDbType.VarChar);
                    FetchReportuserParam[5].Value = strRole;


                    foreach (SqlParameter p in FetchReportuserParam)
                    {
                        if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        daFetchReportUsers.SelectCommand.Parameters.Add(p);
                    }
                    daFetchReportUsers.Fill(objds);
                    return objds;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }



        }
        #endregion

        #region If report exists

        public bool CheckIfReportSubmitted(int repID)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckIfReportSubmitted", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                // string strControlOwnerSID = objControlOwner.StrUserSID;

                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(strconnectionString);
                conn.Open();

                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@RepID", SqlDbType.Int);
                para[0].Value = repID;

                para[1] = new SqlParameter("@flag", SqlDbType.Bit);
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
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public bool CheckIfDBReportSubmitted(int repID, string repType, string strQuarter)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckIfDBReportSubmitted", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                // string strControlOwnerSID = objControlOwner.StrUserSID;

                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(strconnectionString);
                conn.Open();

                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter("@RepID", SqlDbType.Int);
                para[0].Value = repID;

                para[1] = new SqlParameter("@RepType", SqlDbType.VarChar, 50);
                para[1].Value = repType;

                para[2] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                para[2].Value = strQuarter;

                para[3] = new SqlParameter("@flag", SqlDbType.Bit);
                para[3].Direction = ParameterDirection.Output;

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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region to check if user exists in current quarter

        public DataSet CheckIfUserExistsForCurrentQuarter(string strUserSID, string strGroupSID, string scope, int Appid, string strquarter)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckUserForNextQuarter", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                // string strControlOwnerSID = objControlOwner.StrUserSID;

                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection(strconnectionString);
                conn.Open();

                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter("@Scope", SqlDbType.VarChar,50);
                para[0].Value = scope;

                para[1] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                para[1].Value = strUserSID;

                para[2] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                para[2].Value = strGroupSID;

                para[3] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                para[3].Value = Appid;

                para[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                para[4].Value = strquarter;


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
        #region SignoffUsersByGlobal
        public int SignOffUsersByGlobalReport(string usersid,string strgroupsid, string signoffstatus, clsEALUser objclsEALApprover, string Quarter,bool IsAdminUpdate,bool IsAdmin, int ReportID,string scope)
        {
            int i = 0;
            try
            {

                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter[] pSignoffGlobal = new SqlParameter[11];
                pSignoffGlobal[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = usersid;


                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserSID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@SignOffDateTime", SqlDbType.DateTime);
                pSignoffGlobal[5].Value = DateTime.Now;

                pSignoffGlobal[6] = new SqlParameter("@IsAdminupdate", SqlDbType.Bit);
                pSignoffGlobal[6].Value = IsAdminUpdate;


                pSignoffGlobal[7] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                pSignoffGlobal[7].Value = IsAdmin;

                pSignoffGlobal[8] = new SqlParameter("@reportID", SqlDbType.Int);
                pSignoffGlobal[8].Value = ReportID;

                pSignoffGlobal[9] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffGlobal[9].Value = scope;

                pSignoffGlobal[10] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[10].Value = strgroupsid;






                cmd.CommandText = "SP_SignOffUserGlobal_ph2";
                foreach (SqlParameter p in pSignoffGlobal)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }

                i = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return i;
        }

        public int SignOffUsersByGlobalReport_SQL(string userNm, string strRole, string strSA, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, int ReportID, string scope)
        {
            int i = 0;
            try
            {

                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter[] pSignoffGlobal = new SqlParameter[10];
                pSignoffGlobal[0] = new SqlParameter("@UserNm", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = userNm;


                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserADID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@SignOffDateTime", SqlDbType.DateTime);
                pSignoffGlobal[5].Value = DateTime.Now;

                pSignoffGlobal[6] = new SqlParameter("@ISSA", SqlDbType.VarChar, 10);
                pSignoffGlobal[6].Value = strSA;

                pSignoffGlobal[7] = new SqlParameter("@reportID", SqlDbType.Int);
                pSignoffGlobal[7].Value = ReportID;

                pSignoffGlobal[8] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffGlobal[8].Value = scope;

                pSignoffGlobal[9] = new SqlParameter("@Role", SqlDbType.VarChar, 100);
                pSignoffGlobal[9].Value = strRole;


                cmd.CommandText = "SP_SignOffUserGlobal_SQL";
                foreach (SqlParameter p in pSignoffGlobal)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }

                i = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }

        public int SignOffUsersByGlobalReport_Linux(string userNm, string strRole,string signoffstatus, clsEALUser objclsEALApprover, string Quarter, int ReportID, string scope)
        {
            int i = 0;
            try
            {

                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter[] pSignoffGlobal = new SqlParameter[9];
                pSignoffGlobal[0] = new SqlParameter("@UserNm", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = userNm;


                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserADID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@SignOffDateTime", SqlDbType.DateTime);
                pSignoffGlobal[5].Value = DateTime.Now;

                //pSignoffGlobal[6] = new SqlParameter("@ISSA", SqlDbType.VarChar, 10);
                //pSignoffGlobal[6].Value = strSA;

                pSignoffGlobal[6] = new SqlParameter("@reportID", SqlDbType.Int);
                pSignoffGlobal[6].Value = ReportID;

                pSignoffGlobal[7] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffGlobal[7].Value = scope;

                pSignoffGlobal[8] = new SqlParameter("@Role", SqlDbType.VarChar, 100);
                pSignoffGlobal[8].Value = strRole;


                cmd.CommandText = "SP_SignOffUserGlobal_Linux";
                foreach (SqlParameter p in pSignoffGlobal)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }

                i = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }
        public int SignOffUsersByGlobalReport_SecGrp(string userNm, string strRole, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, int ReportID, string scope)
        {
            int i = 0;
            try
            {

                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter[] pSignoffGlobal = new SqlParameter[9];
                pSignoffGlobal[0] = new SqlParameter("@UserNm", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = userNm;


                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserADID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@SignOffDateTime", SqlDbType.DateTime);
                pSignoffGlobal[5].Value = DateTime.Now;

                //pSignoffGlobal[6] = new SqlParameter("@ISSA", SqlDbType.VarChar, 10);
                //pSignoffGlobal[6].Value = strSA;

                pSignoffGlobal[6] = new SqlParameter("@reportID", SqlDbType.Int);
                pSignoffGlobal[6].Value = ReportID;

                pSignoffGlobal[7] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffGlobal[7].Value = scope;

                pSignoffGlobal[8] = new SqlParameter("@Role", SqlDbType.VarChar, 100);
                pSignoffGlobal[8].Value = strRole;


                cmd.CommandText = "SP_SignOffUserGlobal_SecGrp";
                foreach (SqlParameter p in pSignoffGlobal)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }

                i = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }
        public int SignOffUsersByGlobalReport_Ora(string userNm, string strRole, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, int ReportID, string scope)
        {
            int i = 0;
            try
            {

                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter[] pSignoffGlobal = new SqlParameter[9];
                pSignoffGlobal[0] = new SqlParameter("@UserNm", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = userNm;


                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserADID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@SignOffDateTime", SqlDbType.DateTime);
                pSignoffGlobal[5].Value = DateTime.Now;

                
                pSignoffGlobal[6] = new SqlParameter("@reportID", SqlDbType.Int);
                pSignoffGlobal[6].Value = ReportID;

                pSignoffGlobal[7] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pSignoffGlobal[7].Value = scope;

                pSignoffGlobal[8] = new SqlParameter("@Role", SqlDbType.VarChar, 100);
                pSignoffGlobal[8].Value = strRole;


                cmd.CommandText = "SP_SignOffUserGlobal_Ora";
                foreach (SqlParameter p in pSignoffGlobal)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }

                i = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }

        public bool checkuserforCurrentQuarter(string strUserSID, string strGroupSID, string scope, int Appid, string strQuarter)
        {

            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Scope",scope),
                new SqlParameter("@UserSID", strUserSID),
                new SqlParameter("@GroupSID", strGroupSID),
                new SqlParameter("@ApplicationID",Appid),
                new SqlParameter("@Quarter", strQuarter)
               
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_CheckUserForNextQuarter", parm);

                if (ds.Tables[0].Rows[0][0] == "0")

                    return false;
                else return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void SignOffUsersByGlobal(string usersid, string strGroupSID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, bool IsAdminUpdate, bool IsAdmin)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter[] pSignoffGlobal = new SqlParameter[9];
                pSignoffGlobal[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[0].Value = usersid;

                pSignoffGlobal[1] = new SqlParameter("@SignOffStatus", SqlDbType.VarChar, 50);
                pSignoffGlobal[1].Value = signoffstatus;

                pSignoffGlobal[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffGlobal[2].Value = objclsEALApprover.StrUserName;

                pSignoffGlobal[3] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[3].Value = objclsEALApprover.StrUserSID;

                pSignoffGlobal[4] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffGlobal[4].Value = Quarter;

                pSignoffGlobal[5] = new SqlParameter("@SignOffDateTime", SqlDbType.DateTime);
                pSignoffGlobal[5].Value = DateTime.Now;

                pSignoffGlobal[6] = new SqlParameter("@IsAdminupdate", SqlDbType.Bit);
                pSignoffGlobal[6].Value = IsAdminUpdate;


                pSignoffGlobal[7] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                pSignoffGlobal[7].Value = IsAdmin;
                if (strGroupSID == "")
                {
                    strGroupSID = null;
                }
                pSignoffGlobal[8] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                pSignoffGlobal[8].Value = strGroupSID;



                cmd.CommandText = "SP_SignOffUserGlobal";
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
        #endregion
        #region UpdateIsAdminStatus
        public void UpdateISAdmin(string usersid,Int32 ReportID,bool IsAdmin)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter[] pUpdateIsAdmin = new SqlParameter[3];
                pUpdateIsAdmin[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                pUpdateIsAdmin[0].Value = usersid;

                pUpdateIsAdmin[1] = new SqlParameter("@ReportID", SqlDbType.Int);
                pUpdateIsAdmin[1].Value = ReportID;

                pUpdateIsAdmin[2] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                pUpdateIsAdmin[2].Value = IsAdmin;





                cmd.CommandText = "SP_UpdateIsAdmin";
                foreach (SqlParameter p in pUpdateIsAdmin)
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
        #endregion

        #region SignoffUsers by Other
        public int SignOffUsersByOthers(string scope, Int32 ReportID, string usersid,string strGroupSID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin)
        {
            int i  =0;
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

                pSignoffOther[1] = new SqlParameter("@ReportID", SqlDbType.VarChar, 50);
                pSignoffOther[1].Value = ReportID;


                pSignoffOther[2] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                pSignoffOther[2].Value = usersid;

                pSignoffOther[3] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[3].Value = ApplicationID;

                pSignoffOther[4] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[4].Value = signoffstatus;

                pSignoffOther[5] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = objclsEALApprover.StrUserName;

                pSignoffOther[6] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[6].Value = objclsEALApprover.StrUserSID;

                pSignoffOther[7] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[7].Value = Quarter;

                pSignoffOther[8] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[8].Value = DateTime.Now;

                pSignoffOther[9] = new SqlParameter("@IsAdminupdate", SqlDbType.Bit);
                pSignoffOther[9].Value = IsAdminUpdate;


                pSignoffOther[10] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                pSignoffOther[10].Value = IsAdmin;

                pSignoffOther[11] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                pSignoffOther[11].Value = strGroupSID;



                cmd.CommandText = "SP_SignoffUserOthers";
                foreach (SqlParameter p in pSignoffOther)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }

                i = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return i;
           

        }


        //public int SignOffDBUsersByOthers(string scope, Int32 ReportID, string userNm, string strDatabase, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string IsSA, string strRepType)
        //{
        //    int i = 0;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(strconnectionString);
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand();

        //        cmd.Connection = con;
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        SqlParameter[] pSignoffOther = new SqlParameter[12];
        //        pSignoffOther[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
        //        pSignoffOther[0].Value = scope;

        //        pSignoffOther[1] = new SqlParameter("@ReportID", SqlDbType.VarChar, 50);
        //        pSignoffOther[1].Value = ReportID;


        //        pSignoffOther[2] = new SqlParameter("@UserNm", SqlDbType.VarChar, 100);
        //        pSignoffOther[2].Value = userNm;

        //        pSignoffOther[3] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
        //        pSignoffOther[3].Value = ApplicationID;

        //        pSignoffOther[4] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
        //        pSignoffOther[4].Value = signoffstatus;

        //        pSignoffOther[5] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
        //        pSignoffOther[5].Value = objclsEALApprover.StrUserName;

        //        pSignoffOther[6] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
        //        pSignoffOther[6].Value = objclsEALApprover.StrUserADID;

        //        pSignoffOther[7] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
        //        pSignoffOther[7].Value = Quarter;

        //        pSignoffOther[8] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
        //        pSignoffOther[8].Value = DateTime.Now;

        //        pSignoffOther[9] = new SqlParameter("@IsSA", SqlDbType.VarChar, 100);
        //        pSignoffOther[9].Value = IsSA;

        //        pSignoffOther[10] = new SqlParameter("@Database", SqlDbType.VarChar, 100);
        //        pSignoffOther[10].Value = strDatabase;

        //        pSignoffOther[11] = new SqlParameter("@RepType", SqlDbType.VarChar, 100);
        //        pSignoffOther[11].Value = strRepType;

        //        cmd.CommandText = "SP_SignoffDBUserOthers";
        //        foreach (SqlParameter p in pSignoffOther)
        //        {
        //            if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
        //            {
        //                p.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(p);
        //        }

        //        i = cmd.ExecuteNonQuery();

        //        cmd.Parameters.Clear();
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return i;


        //}

        public int SignOffDBUsersByOthers(string scope, Int32 ReportID, string userNm, string strDatabase, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string IsSA, string strRepType,string strDBUser_ID)
        {
            int i = 0;
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

                pSignoffOther[1] = new SqlParameter("@ReportID", SqlDbType.VarChar, 50);
                pSignoffOther[1].Value = ReportID;


                pSignoffOther[2] = new SqlParameter("@UserNm", SqlDbType.VarChar, 100);
                pSignoffOther[2].Value = userNm;

                pSignoffOther[3] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[3].Value = ApplicationID;

                pSignoffOther[4] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[4].Value = signoffstatus;

                pSignoffOther[5] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = objclsEALApprover.StrUserName;

                pSignoffOther[6] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
                pSignoffOther[6].Value = objclsEALApprover.StrUserADID;

                pSignoffOther[7] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[7].Value = Quarter;

                pSignoffOther[8] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[8].Value = DateTime.Now;

                pSignoffOther[9] = new SqlParameter("@IsSA", SqlDbType.VarChar, 100);
                pSignoffOther[9].Value = IsSA;

                pSignoffOther[10] = new SqlParameter("@Database", SqlDbType.VarChar, 100);
                pSignoffOther[10].Value = strDatabase;

                pSignoffOther[11] = new SqlParameter("@RepType", SqlDbType.VarChar, 100);
                pSignoffOther[11].Value = strRepType;

                pSignoffOther[12] = new SqlParameter("@DBUser_ID", SqlDbType.VarChar, 100);
                pSignoffOther[12].Value = strDBUser_ID;

                cmd.CommandText = "SP_SignoffDBUserOthers";
                foreach (SqlParameter p in pSignoffOther)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }

                i = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;


        }

        public DataSet GetPrevDBSignoffByAproverName_Reports(string PreviousQuarter, string userNm, string strSA, int RepId, string RepType)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                    new SqlParameter("@PreviousQuarter", PreviousQuarter),
                    new SqlParameter("@UserNm", userNm),
                    new SqlParameter("@strSA", strSA),
                    new SqlParameter("@RepId", RepId),
                    new SqlParameter("@RepType", RepType)
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_GetPrevDBSignoffByAproverName_Reports", parm);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string LastStatus_DB(string strUSerNm, string strCurrQuarter, string strRepType)
        {

            SqlDataAdapter dalastapprovers = new SqlDataAdapter("SP_lastStatus_DB", strconnectionString);
            dalastapprovers.SelectCommand.CommandType = CommandType.StoredProcedure;
            dalastapprovers.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
            string strLastStatus = "";
            try
            {
                SqlParameter[] FetchReportParam = new SqlParameter[3];

                FetchReportParam[0] = new SqlParameter("@UserNm", SqlDbType.VarChar);
                FetchReportParam[0].Value = strUSerNm;

                FetchReportParam[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar);
                FetchReportParam[1].Value = strCurrQuarter;

                FetchReportParam[2] = new SqlParameter("@RepType", SqlDbType.VarChar);
                FetchReportParam[2].Value = strRepType;

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

        //public DataSet UpdateDBResetToPending(string scope, Int32 ReportID, string userNm, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strSA, string strRepType)
        //{
        //    string strConnStr = "";
        //    try
        //    {
        //        strConnStr = strconnectionString;

        //        SqlParameter[] parm = new SqlParameter[] 
        //      {
                  
        //        new SqlParameter("@Scope", scope),
        //        new SqlParameter("@Quarter", Quarter),
        //        new SqlParameter("@ApproverADID",objclsEALApprover.StrUserADID ),
        //        new SqlParameter("@ApplicationID", ApplicationID),
        //        new SqlParameter("@ReportID", ReportID),
        //        new SqlParameter("@userNm", userNm),
        //        new SqlParameter("@IsSA", strSA),
        //        new SqlParameter("@RepType", strRepType)
                                    
        //       };

        //        DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateDBResetToPending", parm);

        //        return ds;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public void UpdateDBResetToPending(string scope, Int32 ReportID, string userNm, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, string strSA, string strRepType, string strDBUser_ID, string ResetBy)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@Scope", scope),
                new SqlParameter("@Quarter", Quarter),
                new SqlParameter("@ApproverADID",objclsEALApprover.StrUserADID ),
                new SqlParameter("@ApplicationID", ApplicationID),
                new SqlParameter("@ReportID", ReportID),
                new SqlParameter("@userNm", userNm),
                new SqlParameter("@IsSA", strSA),
                new SqlParameter("@RepType", strRepType),
                new SqlParameter("@DBUser_ID", strDBUser_ID),
                new SqlParameter("@ResetBy", ResetBy)           
               };

                //DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_UpdateDBResetToPending", parm);
                clsDAL_SqlHelper.ExecuteNonQuery(strConnStr, CommandType.StoredProcedure, "SP_UpdateDBResetToPending", parm);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void SignOffUsersByOthersAllAcc(string scope, Int32 ReportID, string usersid, string strGroupSID, string signoffstatus, clsEALUser objclsEALApprover, string Quarter, Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin)
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

                pSignoffOther[1] = new SqlParameter("@ReportID", SqlDbType.VarChar, 50);
                pSignoffOther[1].Value = ReportID;


                pSignoffOther[2] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                pSignoffOther[2].Value = usersid;

                pSignoffOther[3] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[3].Value = ApplicationID;

                pSignoffOther[4] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[4].Value = signoffstatus;

                pSignoffOther[5] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = objclsEALApprover.StrUserName;

                pSignoffOther[6] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[6].Value = objclsEALApprover.StrUserSID;

                pSignoffOther[7] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[7].Value = Quarter;

                pSignoffOther[8] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[8].Value = DateTime.Now;

                pSignoffOther[9] = new SqlParameter("@IsAdminupdate", SqlDbType.Bit);
                pSignoffOther[9].Value = IsAdminUpdate;


                pSignoffOther[10] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                pSignoffOther[10].Value = IsAdmin;

                if (strGroupSID == "")
                {
                    strGroupSID = null;
                }
                pSignoffOther[11] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                pSignoffOther[11].Value = strGroupSID;



                cmd.CommandText = "SP_SignoffUserOthersAllAcc";
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

        public void SignOffUsersByOthersAllAppScope(string scope, Int32 ReportID, string usersid, string strgroupsid, string signoffstatus, clsEALUser objclsEALApprover,string strCOSID, string Quarter, Int32 ApplicationID, bool IsAdminUpdate, bool IsAdmin)
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

                pSignoffOther[1] = new SqlParameter("@ReportID", SqlDbType.VarChar, 50);
                pSignoffOther[1].Value = ReportID;


                pSignoffOther[2] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                pSignoffOther[2].Value = usersid;

                pSignoffOther[3] = new SqlParameter("@ApplicationID", SqlDbType.VarChar, 100);
                pSignoffOther[3].Value = ApplicationID;

                pSignoffOther[4] = new SqlParameter("@SignoffStatus", SqlDbType.VarChar, 50);
                pSignoffOther[4].Value = signoffstatus;

                pSignoffOther[5] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                pSignoffOther[5].Value = objclsEALApprover.StrUserName;

                pSignoffOther[6] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                pSignoffOther[6].Value = objclsEALApprover.StrUserSID;

                pSignoffOther[7] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pSignoffOther[7].Value = Quarter;

                pSignoffOther[8] = new SqlParameter("@signoffDate", SqlDbType.VarChar, 50);
                pSignoffOther[8].Value = DateTime.Now;

                pSignoffOther[9] = new SqlParameter("@IsAdminupdate", SqlDbType.Bit);
                pSignoffOther[9].Value = IsAdminUpdate;


                pSignoffOther[10] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                pSignoffOther[10].Value = IsAdmin;

                pSignoffOther[11] = new SqlParameter("@COSID", SqlDbType.VarChar, 100);
                pSignoffOther[11].Value = strCOSID;

                if (strgroupsid == "")
                {
                    strgroupsid = null;
                }




                pSignoffOther[12] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                pSignoffOther[12].Value = strgroupsid;



                cmd.CommandText = "SP_SignoffUserAllAppScope";
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


        #endregion


        #region SubmitReport
        public void SubmitReport(Int32 ReportID, clsEALUser objApprover)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
               
                    SqlParameter[] psubmitReport = new SqlParameter[3];
                    psubmitReport[0] = new SqlParameter("@ReportID", SqlDbType.Int);
                    psubmitReport[0].Value = ReportID;

                    psubmitReport[1] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
                    psubmitReport[1].Value = objApprover.StrUserSID;

                    psubmitReport[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                    psubmitReport[2].Value = objApprover.StrUserName;

                    cmd.CommandText = "SP_SubmitReport";
                    foreach (SqlParameter p in psubmitReport)
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
        public void SubmitDBReport(Int32 ReportID, clsEALUser objApprover, string quarter,string repType)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] psubmitReport = new SqlParameter[5];
                psubmitReport[0] = new SqlParameter("@ReportID", SqlDbType.Int);
                psubmitReport[0].Value = ReportID;

                psubmitReport[1] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 100);
                psubmitReport[1].Value = objApprover.StrUserADID;

                psubmitReport[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                psubmitReport[2].Value = objApprover.StrUserName;

                psubmitReport[3] = new SqlParameter("@quarter", SqlDbType.VarChar, 50);
                psubmitReport[3].Value = quarter;

                psubmitReport[4] = new SqlParameter("@repType", SqlDbType.VarChar, 50);
                psubmitReport[4].Value = repType;

                cmd.CommandText = "SP_SubmitDBReport";
                foreach (SqlParameter p in psubmitReport)
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
        #endregion
        public DataSet GetApplication(Int32 AppID)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_GetApplicationName", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = AppID;

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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet CheckAllDBReports(int AppID, string strCurrQuarter, string strRepType)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                new SqlParameter("@ApplicationID", AppID),
                new SqlParameter("@Quarter", strCurrQuarter),
                new SqlParameter("@RepType", strRepType)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_CheckAllDBReports", parm);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region GetComment
        public string GetComment(Int32 ReportID, string UserSID, string GroupSID)
        {
            SqlDataAdapter daGetComments = new SqlDataAdapter("SP_GetUserComment", strconnectionString);
            daGetComments.SelectCommand.CommandType = CommandType.StoredProcedure;
            daGetComments.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            string strCommentDate = string.Empty;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchCommentParam = new SqlParameter[3];
                FetchCommentParam[0] = new SqlParameter("@ReportID", SqlDbType.Int);
                FetchCommentParam[0].Value = ReportID;

                FetchCommentParam[1] = new SqlParameter("@SID", SqlDbType.VarChar,100);
                FetchCommentParam[1].Value = UserSID;

                FetchCommentParam[2] = new SqlParameter("@GroupSID", SqlDbType.VarChar, 100);
                FetchCommentParam[2].Value = GroupSID;

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

        public string GetDBComment(Int32 ReportID, string UserNm, string strRepType)
        {
            SqlDataAdapter daGetComments = new SqlDataAdapter("SP_GetDBUserComment", strconnectionString);
            int UID = Convert.ToInt32(UserNm);
            daGetComments.SelectCommand.CommandType = CommandType.StoredProcedure;
            daGetComments.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            string strCommentDate = string.Empty;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchCommentParam = new SqlParameter[3];
                FetchCommentParam[0] = new SqlParameter("@ReportID", SqlDbType.Int);
                FetchCommentParam[0].Value = ReportID;

                FetchCommentParam[1] = new SqlParameter("@UserNm", SqlDbType.Int);
                FetchCommentParam[1].Value = UID;

                FetchCommentParam[2] = new SqlParameter("@RepType", SqlDbType.VarChar, 100);
                FetchCommentParam[2].Value = strRepType;

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

        public string GetLinuxComment(Int32 RowID)
        {
            SqlDataAdapter daGetComments = new SqlDataAdapter("SP_GetLinuxUserComment", strconnectionString);
            daGetComments.SelectCommand.CommandType = CommandType.StoredProcedure;
            daGetComments.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            string strCommentDate = string.Empty;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchCommentParam = new SqlParameter[1];
                FetchCommentParam[0] = new SqlParameter("@RowID", SqlDbType.Int);
                FetchCommentParam[0].Value = RowID;

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
        public string GetSecGrpComment(Int32 RowID)
        {
            SqlDataAdapter daGetComments = new SqlDataAdapter("[SP_GetSecGrpUserComment]", strconnectionString);
            daGetComments.SelectCommand.CommandType = CommandType.StoredProcedure;
            daGetComments.SelectCommand.CommandTimeout = 36000;
            string Comment = string.Empty;
            string strCommentDate = string.Empty;
            objds = new DataSet();
            try
            {
                SqlParameter[] FetchCommentParam = new SqlParameter[1];
                FetchCommentParam[0] = new SqlParameter("@RowID", SqlDbType.Int);
                FetchCommentParam[0].Value = RowID;

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


        #endregion

        #region UpdateComment
        public void UpdateComment(string comment, string scope, string strQuarter, int AppID, string ApproverSID, Int32 ReportID, string SID, string GroupSID)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter[] pUpdateComment = new SqlParameter[8];

                pUpdateComment[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pUpdateComment[0].Value = scope;

                pUpdateComment[1] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pUpdateComment[1].Value = strQuarter;

                pUpdateComment[2] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 50);
                pUpdateComment[2].Value = ApproverSID;

                pUpdateComment[3] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                pUpdateComment[3].Value = AppID;

                pUpdateComment[4] = new SqlParameter("@ReportID", SqlDbType.VarChar, 50);
                pUpdateComment[4].Value = ReportID;

                pUpdateComment[5] = new SqlParameter("@sid", SqlDbType.VarChar, 100);
                pUpdateComment[5].Value = SID;


                pUpdateComment[6] = new SqlParameter("@Comment", SqlDbType.Text);
                pUpdateComment[6].Value = comment;

                pUpdateComment[7] = new SqlParameter("@groupSid", SqlDbType.VarChar, 100);
                pUpdateComment[7].Value = GroupSID;


                cmd.CommandText = "SP_UpdateComment1";
                foreach (SqlParameter p in pUpdateComment)
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


        //public void UpdateDBComment(string comment, string scope, string strQuarter, int AppID, string ApproverADID, Int32 ReportID, string UserNm, string reportType, string strDBnm)
        //{
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(strconnectionString);
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand();

        //        cmd.Connection = con;
        //        cmd.CommandType = CommandType.StoredProcedure;
                
        //        SqlParameter[] pUpdateComment = new SqlParameter[9];

        //        pUpdateComment[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
        //        pUpdateComment[0].Value = scope;

        //        pUpdateComment[1] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
        //        pUpdateComment[1].Value = strQuarter;

        //        pUpdateComment[2] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 50);
        //        pUpdateComment[2].Value = ApproverADID;

        //        pUpdateComment[3] = new SqlParameter("@ApplicationID", SqlDbType.Int);
        //        pUpdateComment[3].Value = AppID;

        //        pUpdateComment[4] = new SqlParameter("@ReportID", SqlDbType.VarChar, 50);
        //        pUpdateComment[4].Value = ReportID;

        //        pUpdateComment[5] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
        //        pUpdateComment[5].Value = UserNm;


        //        pUpdateComment[6] = new SqlParameter("@Comment", SqlDbType.Text);
        //        pUpdateComment[6].Value = comment;

        //        pUpdateComment[7] = new SqlParameter("@repType", SqlDbType.VarChar, 100);
        //        pUpdateComment[7].Value = reportType;


        //        pUpdateComment[8] = new SqlParameter("@DBNm", SqlDbType.VarChar, 100);
        //        pUpdateComment[8].Value = strDBnm;


        //        cmd.CommandText = "SP_UpdateDBComment1";
        //        foreach (SqlParameter p in pUpdateComment)
        //        {
        //            if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
        //            {
        //                p.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(p);
        //        }

        //        cmd.ExecuteNonQuery();

        //        cmd.Parameters.Clear();
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public void UpdateDBComment(string comment, string scope, string strQuarter, int AppID, string ApproverADID, Int32 ReportID, string UserNm, string reportType, string strDBnm,string strDBUser_ID)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pUpdateComment = new SqlParameter[10];

                pUpdateComment[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pUpdateComment[0].Value = scope;

                pUpdateComment[1] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pUpdateComment[1].Value = strQuarter;

                pUpdateComment[2] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 50);
                pUpdateComment[2].Value = ApproverADID;

                pUpdateComment[3] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                pUpdateComment[3].Value = AppID;

                pUpdateComment[4] = new SqlParameter("@ReportID", SqlDbType.VarChar, 50);
                pUpdateComment[4].Value = ReportID;

                pUpdateComment[5] = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
                pUpdateComment[5].Value = UserNm;

                pUpdateComment[6] = new SqlParameter("@Comment", SqlDbType.Text);
                pUpdateComment[6].Value = comment;

                pUpdateComment[7] = new SqlParameter("@repType", SqlDbType.VarChar, 100);
                pUpdateComment[7].Value = reportType;

                pUpdateComment[8] = new SqlParameter("@DBNm", SqlDbType.VarChar, 100);
                pUpdateComment[8].Value = strDBnm;

                pUpdateComment[9] = new SqlParameter("@DBUser_ID", SqlDbType.VarChar, 100);
                pUpdateComment[9].Value = strDBUser_ID;

                cmd.CommandText = "SP_UpdateDBComment1";
                foreach (SqlParameter p in pUpdateComment)
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

        public void UpdateLinuxComment(string comment, string scope, string strQuarter, int AppID, string ApproverADID, Int32 ServerID, string strUser_ID)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pUpdateComment = new SqlParameter[7];

                pUpdateComment[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pUpdateComment[0].Value = scope;

                pUpdateComment[1] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pUpdateComment[1].Value = strQuarter;

                pUpdateComment[2] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 50);
                pUpdateComment[2].Value = ApproverADID;

                pUpdateComment[3] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                pUpdateComment[3].Value = AppID;

                pUpdateComment[4] = new SqlParameter("@ServerID", SqlDbType.VarChar, 50);
                pUpdateComment[4].Value = ServerID;

                pUpdateComment[5] = new SqlParameter("@Comment", SqlDbType.Text);
                pUpdateComment[5].Value = comment;

                pUpdateComment[6] = new SqlParameter("@UserID", SqlDbType.VarChar,100);
                pUpdateComment[6].Value = strUser_ID;

                cmd.CommandText = "SP_UpdateLinuxComment";
                foreach (SqlParameter p in pUpdateComment)
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
        public void UpdateSecGrpComment(string comment, string scope, string strQuarter, int AppID, string ApproverADID, Int32 ServerID, string strUser_ID)
        {
            try
            {
                SqlConnection con = new SqlConnection(strconnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] pUpdateComment = new SqlParameter[7];

                pUpdateComment[0] = new SqlParameter("@Scope", SqlDbType.VarChar, 50);
                pUpdateComment[0].Value = scope;

                pUpdateComment[1] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                pUpdateComment[1].Value = strQuarter;

                pUpdateComment[2] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 50);
                pUpdateComment[2].Value = ApproverADID;

                pUpdateComment[3] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                pUpdateComment[3].Value = AppID;

                pUpdateComment[4] = new SqlParameter("@ServerID", SqlDbType.VarChar, 50);
                pUpdateComment[4].Value = ServerID;

                pUpdateComment[5] = new SqlParameter("@Comment", SqlDbType.Text);
                pUpdateComment[5].Value = comment;

                pUpdateComment[6] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                pUpdateComment[6].Value = strUser_ID;

                cmd.CommandText = "[SP_UpdateSecGrpComment]";
                foreach (SqlParameter p in pUpdateComment)
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


        #endregion

        public DataSet GetAllAccounts(clsEALRoles objRole, string ApplicationID, string Quarter)
        {
            return new DataSet();
        }
    }
}
