using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using CART_EAL;
using System.Data.SqlClient;


namespace CART_DAL
{
   public class clsDALApplication:clsDBConnection
   {
       #region DataField
       SqlCommand cmd = null;
       SqlConnection con = null;
       DataSet objds;
       #endregion
       clsEALUser objclsEALLoggedInUser;
       public ArrayList GetAllAppHavingAccess(clsEALUser objclsEalUser,clsEALRoles objclsEALRol)
        {
            return new ArrayList();
        }
       public bool GetApplicationCOCompletion(int AppID,string strCurrQuarter)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_COCompletion", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 36000;
           DataSet ds = new DataSet();
           SqlParameter[] para = new SqlParameter[3];

           para[0] = new SqlParameter("@AppID", SqlDbType.Int);
           para[0].Value = AppID;

           para[1] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
           para[1].Value = strCurrQuarter;

           para[2] = new SqlParameter("@flag", SqlDbType.Bit);
           para[2].Direction = ParameterDirection.Output;

           foreach (SqlParameter p in para)
           {
               if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
               {
                   p.Value = DBNull.Value;
               }
               da.SelectCommand.Parameters.Add(p);

           }
           da.Fill(ds);
           string strSignOff = ds.Tables[0].Rows[0][0].ToString();
           bool blnCOComp;
           if (strSignOff == "True")
           {
               blnCOComp = true;
           }
           else
           {
               blnCOComp = false;
           }
           return blnCOComp;
       }

       public bool SaveDBApplicationMapping(int AppID, string Database, int serverid)
       {
           try
           {
               SqlDataAdapter da = new SqlDataAdapter("SP_SaveDBApplicationMapping", strconnectionString);
               da.SelectCommand.CommandType = CommandType.StoredProcedure;
               da.SelectCommand.CommandTimeout = 3600;

               DataSet ds = new DataSet();


               SqlParameter[] para = new SqlParameter[4];
               para[0] = new SqlParameter("@AppID", SqlDbType.Int);
               para[0].Value = AppID;

               para[1] = new SqlParameter("@Database", SqlDbType.VarChar, 100);
               para[1].Value = Database;

               para[2] = new SqlParameter("@ServerID", SqlDbType.Int);
               para[2].Value = serverid;


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
       public void DeleteAppDatabaseMapping(int Appid, int ShareID)
       {
           try
           {
               //strAppNm = ddlAppName.SelectedItem.Value.ToString();
               DataSet ds_del = new DataSet();
               SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteAppDatabase", strconnectionString);
               da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
               da_del.SelectCommand.CommandTimeout = 36000;

               SqlParameter[] pDel = new SqlParameter[2];

               pDel[0] = new SqlParameter("@AppID", SqlDbType.Int);
               pDel[0].Direction = ParameterDirection.Input;
               pDel[0].Value = Appid;

               pDel[1] = new SqlParameter("@DatabaseMappingID", SqlDbType.Int);
               pDel[1].Direction = ParameterDirection.Input;
               pDel[1].Value = ShareID;

               da_del.SelectCommand.Parameters.Add(pDel[0]);
               da_del.SelectCommand.Parameters.Add(pDel[1]);

               da_del.Fill(ds_del);
               //return true;
           }
           catch (Exception ex)
           {
               Exception ExNew = new Exception("Error While Deleting", ex);
               ExNew.Source = "CommonLogic.RowDeleting";
               throw ExNew;
           }
       }

       public bool CheckIfDBMappingExists(int AppID, int Database)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_CheckDBApplicationMapping", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
           DataSet ds = new DataSet();
           SqlConnection conn = new SqlConnection(strconnectionString);
           conn.Open();

           SqlParameter[] para = new SqlParameter[3];
           para[0] = new SqlParameter("@AppID", SqlDbType.Int);
           para[0].Value = AppID;

           para[1] = new SqlParameter("@DatabaseID", SqlDbType.Int);
           para[1].Value = Database;

           para[2] = new SqlParameter("@flag", SqlDbType.Bit);
           para[2].Direction = ParameterDirection.Output;

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


       public bool CheckIfDBServerExists(string strDbServerNm, int servertypeID)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_CheckDBServerExists", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
           DataSet ds = new DataSet();
           SqlConnection conn = new SqlConnection(strconnectionString);
           conn.Open();

           SqlParameter[] para = new SqlParameter[3];
           para[0] = new SqlParameter("@DBServerNm", SqlDbType.VarChar , 100);
           para[0].Value = strDbServerNm;

           para[1] = new SqlParameter("@servertypeID", SqlDbType.Int);
           para[1].Value = servertypeID;


           para[2] = new SqlParameter("@flag", SqlDbType.Bit);
           para[2].Direction = ParameterDirection.Output;

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
       public bool CheckIfDBServerExists1(string strDbServerNm, int servertypeID)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_CheckDBServerExists1", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
           DataSet ds = new DataSet();
           SqlConnection conn = new SqlConnection(strconnectionString);
           conn.Open();

           SqlParameter[] para = new SqlParameter[3];
           para[0] = new SqlParameter("@DBServerNm", SqlDbType.VarChar, 100);
           para[0].Value = strDbServerNm;

           para[1] = new SqlParameter("@servertypeID", SqlDbType.Int);
           para[1].Value = servertypeID;


           para[2] = new SqlParameter("@flag", SqlDbType.Bit);
           para[2].Direction = ParameterDirection.Output;

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


       public bool GetCOSignOff(int AppID,string strCOSID, string strCurrQuarter)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_COSignoff", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
           DataSet ds = new DataSet();
           SqlConnection conn = new SqlConnection(strconnectionString);
           conn.Open();

           SqlParameter[] para = new SqlParameter[4];
           para[0] = new SqlParameter("@AppID", SqlDbType.Int);
           para[0].Value = AppID;

           para[1] = new SqlParameter("@COSID", SqlDbType.VarChar, 50);
           para[1].Value = strCOSID;

           para[2] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
           para[2].Value = strCurrQuarter;

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
       //select isreportSubmitted from tblSOXReport where ReportID in(select ReportID from tblSOXServerdetails where Applicationid=30
//union select ReportID from tblSOXSharedetails where ShareName in(select ShareName from tblSOXshareList 
//where ShareID in(select ShareID from tblSoxApplicationmapping where ApplicationID=30)))

       public DataSet CheckAllReports(int AppID,string strCurrQuarter)
       {
           SqlDataAdapter da = new SqlDataAdapter("select isreportSubmitted from tblSOXReport where ReportID in(select ReportID from tblSOXServerdetails where Applicationid= "+AppID+" union select ReportID from tblSOXSharedetails where ShareName in(select ShareName from tblSOXshareList where ShareID in(select ShareID from tblSoxApplicationmapping where ApplicationID="+AppID+"))) and Quarter='"+strCurrQuarter+"'", strconnectionString);
           da.SelectCommand.CommandType = CommandType.Text;
           da.SelectCommand.CommandTimeout = 36000;
           DataSet ds = new DataSet();
           da.Fill(ds);

           return ds;
       }

       public void UpdateCOSignOff(int AppID, string strCurrQuarter)
       {
           SqlDataAdapter da = new SqlDataAdapter("update tblSOXReportStatusControlOwner set COSignOff=false where ApplicationID= " + AppID + " and Quarter= '" + strCurrQuarter + "'", strconnectionString);
           da.SelectCommand.CommandType = CommandType.Text;
           da.SelectCommand.CommandTimeout = 36000;
           DataSet ds = new DataSet();
           da.Fill(ds);
           
       }
       public DataTable GetInitiatives()
       {
           SqlDataAdapter da = new SqlDataAdapter("select InitiativeID, InitiativeName from tblSOXInitiativeDetails", strconnectionString);
           da.SelectCommand.CommandType = CommandType.Text;
           da.SelectCommand.CommandTimeout = 36000;
           DataTable dt= new DataTable();
           da.Fill(dt);
           return dt;
       }
       //select InitiativeName,UpdateInAllApplicationScope,VisibleToComplianceAuditor from tblSOXInitiativeDetails
       public DataTable GetInitiativesDetails()
       {
           SqlDataAdapter da = new SqlDataAdapter("select InitiativeID,InitiativeName,case when UpdateInAllApplicationScope = 0 then 'No' else 'Yes' END UpdateInAllApplicationScope , case when VisibleToComplianceAuditor = 0 then 'No' else 'Yes' END VisibleToComplianceAuditor from tblSOXInitiativeDetails", strconnectionString);
           da.SelectCommand.CommandType = CommandType.Text;
           da.SelectCommand.CommandTimeout = 36000;
           DataTable dt = new DataTable();
           da.Fill(dt);
           return dt;
       }
       

       public DataTable GetAppInitiatives()
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_AppInitiatives", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 36000;
           DataTable dt = new DataTable();
           da.Fill(dt);
           return dt;
       }
       public DataSet GetAdminCOSelectiveApproval(string COSID, string strReportType, string strROle)
       {
           clsEALSession objclsEALSession = new clsEALSession();

           SqlDataAdapter daFetchAppReports = new SqlDataAdapter("SP_AdminCOSelectiveApproval", strconnectionString);
           daFetchAppReports.SelectCommand.CommandType = CommandType.StoredProcedure;
           daFetchAppReports.SelectCommand.CommandTimeout = 36000;
           if (strROle == "2")
           {
               strROle = "Global Approver";
           }
           else
           {
               strROle = "Control Owner";
           }
           objds = new DataSet();
           try
           {
               SqlParameter[] FetchReportParam = new SqlParameter[3];

               FetchReportParam[0] = new SqlParameter("@loggedInSID", SqlDbType.VarChar, 200);
               FetchReportParam[0].Value = COSID;

               FetchReportParam[1] = new SqlParameter("@ReportType", SqlDbType.VarChar, 50);
               FetchReportParam[1].Value = strReportType;

               FetchReportParam[2] = new SqlParameter("@Role", SqlDbType.VarChar, 100);
               FetchReportParam[2].Value = strROle;

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

       #region GetCompletion Status
       public bool GetApplicationCompletionStatus(string Role,clsEALUser objclsEALUser,string Quarter,Int32 AppID )
        {

            SqlDataAdapter daAppCompletionStatus = new SqlDataAdapter("SP_GetApplicationCompletionStatus", strconnectionString);
            daAppCompletionStatus.SelectCommand.CommandType = CommandType.StoredProcedure;
            daAppCompletionStatus.SelectCommand.CommandTimeout = 36000;
            objds = new DataSet();
           
                SqlParameter[] GetAppCOmpletionStatus = new SqlParameter[5];

                GetAppCOmpletionStatus[0] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                GetAppCOmpletionStatus[0].Value = AppID;

                GetAppCOmpletionStatus[1] = new SqlParameter("@UserSID", SqlDbType.VarChar, 100);
                GetAppCOmpletionStatus[1].Value = objclsEALUser.StrUserSID;

                GetAppCOmpletionStatus[2] = new SqlParameter("@UserRole", SqlDbType.VarChar, 50);
                GetAppCOmpletionStatus[2].Value = Role;

                GetAppCOmpletionStatus[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                GetAppCOmpletionStatus[3].Value = Quarter;

                GetAppCOmpletionStatus[4] = new SqlParameter("@flag", SqlDbType.Bit);
                GetAppCOmpletionStatus[4].Direction = ParameterDirection.Output;


                foreach (SqlParameter p in GetAppCOmpletionStatus)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    daAppCompletionStatus.SelectCommand.Parameters.Add(p);
                }
                daAppCompletionStatus.Fill(objds);
                if (objds == null)
                {
                    return false;
                }
                else
                {
                    if (objds.Tables.Count != 0)
                    {
                        if (objds.Tables[0].Rows.Count!=0)
                        {
                            return Convert.ToBoolean(objds.Tables[0].Rows[0][0]);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
           

        }
       #endregion

       #region Update Completion Status
       public void updateCompletionStatus(string role, clsEALUser objclsEALApprover, Int32 ApplicationiD, string Quarter, bool status)
       {
          
               SqlConnection con = new SqlConnection(strconnectionString);
               con.Open();

               SqlCommand cmd = new SqlCommand();

               cmd.Connection = con;
               cmd.CommandType = CommandType.StoredProcedure;



               SqlParameter[] pUpdateCompletion = new SqlParameter[5];
               pUpdateCompletion[0] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
               pUpdateCompletion[0].Value = role;

               pUpdateCompletion[1] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
               pUpdateCompletion[1].Value = objclsEALApprover.StrUserSID;

               pUpdateCompletion[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
               pUpdateCompletion[2].Value = ApplicationiD;

               pUpdateCompletion[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
               pUpdateCompletion[3].Value = Quarter;

               pUpdateCompletion[4] = new SqlParameter("@Status", SqlDbType.Bit);
               pUpdateCompletion[4].Value = status;
               


               cmd.CommandText = "SP_UpdateCompletionStatus";
               foreach (SqlParameter p in pUpdateCompletion)
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
       public void updateCOCompletionStatus(string role, clsEALUser objclsEALApprover, Int32 ApplicationiD, string Quarter, bool status, bool COSignOff)
       {
               SqlConnection con = new SqlConnection(strconnectionString);
               con.Open();

               SqlCommand cmd = new SqlCommand();

               cmd.Connection = con;
               cmd.CommandType = CommandType.StoredProcedure;



               SqlParameter[] pUpdateCompletion = new SqlParameter[6];
               pUpdateCompletion[0] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
               pUpdateCompletion[0].Value = role;

               pUpdateCompletion[1] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 100);
               pUpdateCompletion[1].Value = objclsEALApprover.StrUserSID;

               pUpdateCompletion[2] = new SqlParameter("@ApplicationID", SqlDbType.Int);
               pUpdateCompletion[2].Value = ApplicationiD;

               pUpdateCompletion[3] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
               pUpdateCompletion[3].Value = Quarter;

               pUpdateCompletion[4] = new SqlParameter("@Status", SqlDbType.Bit);
               pUpdateCompletion[4].Value = status;
               //COSignOff

               pUpdateCompletion[5] = new SqlParameter("@COSignOff", SqlDbType.Bit);
               pUpdateCompletion[5].Value = COSignOff;



               cmd.CommandText = "SP_UpdateCOCompletionStatus";
               foreach (SqlParameter p in pUpdateCompletion)
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

       #endregion
       public DataSet GetAllApproversByApplication(string[] role, clsEALUser objclsEALUserApp)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_GetApproversCtrlowners", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            string strApproverSID = objclsEALUserApp.StrUserSID;
            DataSet ds = new DataSet();
            string strRole = "";
            for (int i = 0; i < role.Length; i++)
            {
                strRole =  strRole+ "," + role[i];

            }

            SqlParameter[] para = new SqlParameter[2];
           para[0] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
           para[0].Value = strRole;

            para[1] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
            para[1].Value = strApproverSID;
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

       public bool UnlockApprover(string ApproverTOUnlock, int AppID, string strCurrentQuarter, string loggedInUser)
       {
           
               SqlDataAdapter da = new SqlDataAdapter("SP_UnlockApprover_New", strconnectionString);
               da.SelectCommand.CommandType = CommandType.StoredProcedure;
               da.SelectCommand.CommandTimeout = 3600;
               string strApproverSID = ApproverTOUnlock;
               DataSet ds = new DataSet();

               SqlParameter[] para = new SqlParameter[4];
               para[0] = new SqlParameter("@ApproverSID", SqlDbType.VarChar, 50);
               para[0].Value = strApproverSID;

               para[1] = new SqlParameter("@AppID", SqlDbType.VarChar, 50);
               para[1].Value = AppID;

               para[2] =new SqlParameter("@CurrQuarter", SqlDbType.VarChar,50);
               para[2].Value = strCurrentQuarter;

               para[3] = new SqlParameter("@UnlockedBy", SqlDbType.VarChar, 100);
               para[3].Value = loggedInUser;

               foreach (SqlParameter p in para)
               {
                   if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                   {
                       p.Value = DBNull.Value;
                   }
                   da.SelectCommand.Parameters.Add(p);
               }
               da.Fill(ds);
               return true;
           
       }

       public bool UnlockControlOwner(string ControlOwnerSIDTOUnlock, int AppID, string strCurrentQuarter, bool blnCOSignOff, string strLoggedInUser)
       {
           
               SqlDataAdapter da = new SqlDataAdapter("SP_UnlockControlOwner_New", strconnectionString);
               da.SelectCommand.CommandType = CommandType.StoredProcedure;
               da.SelectCommand.CommandTimeout = 3600;
               //string strApproverSID = ControlOwnerSIDTOUnlock;
               DataSet ds = new DataSet();

               SqlParameter[] para = new SqlParameter[5];
               para[0] = new SqlParameter("@ControlOwnerSID", SqlDbType.VarChar, 50);
               para[0].Value = ControlOwnerSIDTOUnlock;


               para[1] = new SqlParameter("@AppID", SqlDbType.VarChar, 50);
               para[1].Value = AppID;


               para[2] = new SqlParameter("@CurrentQuarter", SqlDbType.VarChar, 50);
               para[2].Value = strCurrentQuarter;

               para[3] = new SqlParameter("@COSignOff", SqlDbType.Bit);
               para[3].Value = blnCOSignOff;

               para[4] = new SqlParameter("@UnlockedBy", SqlDbType.VarChar, 100);
               para[4].Value = strLoggedInUser;

               foreach (SqlParameter p in para)
               {
                   if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                   {
                       p.Value = DBNull.Value;
                   }
                   da.SelectCommand.Parameters.Add(p);
               }
               da.Fill(ds);
               return true;
           
       }

       public bool UnlockGlobalApprover(string Admin, string strCurrQuarter, string loggeInUser)
       {
           
               SqlDataAdapter da = new SqlDataAdapter("SP_UnlockGlobalApprover_New", strconnectionString);
               da.SelectCommand.CommandType = CommandType.StoredProcedure;
               da.SelectCommand.CommandTimeout = 3600;
               string strApproverSID = Admin;
               DataSet ds = new DataSet();

               SqlParameter[] para = new SqlParameter[3];
               para[0] = new SqlParameter("@AdminSID", SqlDbType.VarChar, 50);
               para[0].Value = Admin;
               para[1] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar, 50);
               para[1].Value = strCurrQuarter;
               para[2] = new SqlParameter("@UnlockedBy", SqlDbType.VarChar, 100);
               para[2].Value = loggeInUser;
              
               
               foreach (SqlParameter p in para)
               {
                   if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                   {
                       p.Value = DBNull.Value;
                   }
                   da.SelectCommand.Parameters.Add(p);
               }
               da.Fill(ds);
               return true;
           
       }


       public DataSet GetApproverByApplication(string[] role, clsEALUser objclsEALLoggedInUser, string strCurrentQuarter)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_GetApproverForApp", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
           string strLoggedInSID = objclsEALLoggedInUser.StrUserSID;
           DataSet ds = new DataSet();
           string strRole = "";
           for (int i = 0; i < role.Length; i++)
           {
               strRole = role[i];

           }

           SqlParameter[] para = new SqlParameter[3];
           para[0] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
           para[0].Value = strRole;

           para[1] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
           para[1].Value = strLoggedInSID;

           para[2] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar, 50);
           para[2].Value = strCurrentQuarter;

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

       public bool CheckIfMappingExists(int AppID, int  share)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_CheckApplicationMapping", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
           DataSet ds = new DataSet();
           SqlConnection conn = new SqlConnection(strconnectionString);
           conn.Open();

           SqlParameter[] para = new SqlParameter[3];
           para[0] = new SqlParameter("@AppID", SqlDbType.Int);
           para[0].Value = AppID;

           para[1] = new SqlParameter("@ShareID", SqlDbType.Int);
           para[1].Value = share;

           para[2] = new SqlParameter("@flag", SqlDbType.Bit);
           para[2].Direction = ParameterDirection.Output;

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
           bool flag ;
           if (strDs == "True")
           {
               flag =true;
           }
           else
           {
               flag = false;
           }
           conn.Close();
           return flag;
       }

       public DataSet GETGlobalApprovers(string strCurrQuarter)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_GetGlobalApprovers", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
           DataSet ds = new DataSet();
           SqlParameter[] para = new SqlParameter[1];
           para[0] = new SqlParameter("@CurrQuarter", SqlDbType.VarChar, 50);
           para[0].Value = strCurrQuarter;
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
       //[SP_CheckApplication]
       public bool CheckIfApplicationExists(string AppNm)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_CheckApplication1", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
          // string strControlOwnerSID = objControlOwner.StrUserSID;

           DataSet ds = new DataSet();
           SqlConnection conn = new SqlConnection(strconnectionString);
           conn.Open();

           SqlParameter[] para = new SqlParameter[2];
           para[0] = new SqlParameter("@AppNm", SqlDbType.VarChar, 50);
           para[0].Value = AppNm;

           //para[1] = new SqlParameter("@Initiatives", SqlDbType.VarChar, 50);
           //para[1].Value = Initiatives;

           //para[2] = new SqlParameter("@ControlOwnerSID", SqlDbType.VarChar, 50);
           //para[2].Value = strControlOwnerSID;

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
       public bool CheckIfApplicationExistsForUpdate(string AppNm, string Initiavtives, clsEALUser objControlOwner)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_CheckApplication", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
           string strControlOwnerSID = objControlOwner.StrUserSID;

           DataSet ds = new DataSet();
           SqlConnection conn = new SqlConnection(strconnectionString);
           conn.Open();

           SqlParameter[] para = new SqlParameter[4];
           para[0] = new SqlParameter("@AppNm", SqlDbType.VarChar, 50);
           para[0].Value = AppNm;

           para[1] = new SqlParameter("@Initiatives", SqlDbType.VarChar, 50);
           para[1].Value = Initiavtives;

           para[2] = new SqlParameter("@ControlOwnerSID", SqlDbType.VarChar, 50);
           para[2].Value = strControlOwnerSID;

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


       public bool CheckIfServerExists(string strServer)
       {
           SqlDataAdapter da = new SqlDataAdapter("SP_CheckServer", strconnectionString);
           da.SelectCommand.CommandType = CommandType.StoredProcedure;
           da.SelectCommand.CommandTimeout = 3600;
          
           DataSet ds = new DataSet();
           SqlConnection conn = new SqlConnection(strconnectionString);
           conn.Open();

           SqlParameter[] para = new SqlParameter[2];
           para[0] = new SqlParameter("@ServerNm", SqlDbType.VarChar, 100);
           para[0].Value = strServer;

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



        public bool GetAppCompletionStatus(string role, clsEALUser objClsEALUser,string Quarter)
        {
            return true;
        }

        public bool SaveApplicationDetails(string role, clsEALUser objclsAppOwner, string AppName, string strInitiatives, bool admin, bool MultipleApprovals, bool ExcludeGA, bool UnlockApp, string StrUserADID, string procCycle, string quarters)
        {
                    SqlDataAdapter da = new SqlDataAdapter("SP_SaveApplicationDetails", strconnectionString);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 3600;
                    string strCOSID = objclsAppOwner.StrUserSID;
                    string strCOName = objclsAppOwner.StrUserName;
                    string strADID = objclsAppOwner.StrUserADID;
                    string stremail = objclsAppOwner.StrUserEmailID;
                    DataSet ds = new DataSet();


                    SqlParameter[] para = new SqlParameter[14];
                    para[0] = new SqlParameter("@Appname", SqlDbType.VarChar,50);
                    para[0].Value = AppName;

                    para[1] = new SqlParameter("@ControlOwnerName", SqlDbType.VarChar, 50);
                    para[1].Value = strCOName;

                    para[2] = new SqlParameter("@ControlOwnerSID", SqlDbType.VarChar, 50);
                    para[2].Value = strCOSID;


                    para[3] = new SqlParameter("@ControlOwnerEmailID", SqlDbType.VarChar, 50);
                    para[3].Value = stremail;

                    para[4] = new SqlParameter("@ControlOwnerADID", SqlDbType.VarChar, 50);
                    para[4].Value = strADID;

                    para[5] = new SqlParameter("@Initiatives", SqlDbType.VarChar, 1000);
                    para[5].Value = strInitiatives;
                 
                    para[6] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
                    para[6].Value = role;

                    para[7] = new SqlParameter("@Admin", SqlDbType.Bit);
                    para[7].Value = admin;

                    para[8] = new SqlParameter("@MultipleApprovals", SqlDbType.Bit);
                    para[8].Value = MultipleApprovals;

                    para[9] = new SqlParameter("@ExcludeGA", SqlDbType.Bit);
                    para[9].Value = ExcludeGA;

                    para[10] = new SqlParameter("@UnlockApp", SqlDbType.Bit);
                    para[10].Value = UnlockApp;

                    para[11] = new SqlParameter("@StrUserADID", SqlDbType.VarChar, 500);
                    para[11].Value = StrUserADID;

                    para[12] = new SqlParameter("@Frequency", SqlDbType.VarChar, 20);
                    para[12].Value = procCycle;

                    para[13] = new SqlParameter("@Quarter", SqlDbType.VarChar, 20);
                    para[13].Value = quarters;

                    foreach (SqlParameter p in para)
                    {
                        if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        da.SelectCommand.Parameters.Add(p);
                    }

                    da.Fill(ds);
               
            return true;

        }
        public void DeleteGroupMapping(int GroupID, string strOwnerType, string strOwnerNm, string strNoRestrictions, string strAllControlOwners)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@GroupID", GroupID),
                new SqlParameter("@OwnerType", strOwnerType),
                new SqlParameter("@OwnerNm", strOwnerNm),
                new SqlParameter("@NoRestriction", strNoRestrictions),
                new SqlParameter("@ALLCO", strAllControlOwners)
              };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "SP_DeleteGroupMapping", parm);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool SaveGroupDetails(string GroupName, string OwnerType, string OwnerName, string UserADID, bool NoRestrictions, bool AllControlOwners, string CreatedBy)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_SavegroupDetails", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            DataSet ds = new DataSet();

            SqlParameter[] para = new SqlParameter[7];
            para[0] = new SqlParameter("@GroupName", SqlDbType.VarChar, 1000);
            para[0].Value = GroupName;

            para[1] = new SqlParameter("@OwnerType", SqlDbType.VarChar, 100);
            para[1].Value = OwnerType;

            para[2] = new SqlParameter("@OwnerName", SqlDbType.VarChar, 100);
            para[2].Value = OwnerName;


            para[3] = new SqlParameter("@UserADID", SqlDbType.VarChar, 200);
            para[3].Value = UserADID;

            para[4] = new SqlParameter("@NoRestrictions", SqlDbType.Bit);
            para[4].Value = NoRestrictions;

            para[5] = new SqlParameter("@AllControlOwners", SqlDbType.Bit);
            para[5].Value = AllControlOwners;

            para[6] = new SqlParameter("@CreatedBy", SqlDbType.VarChar, 500);
            para[6].Value = CreatedBy;

            

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);
            }

            da.Fill(ds);

            return true;

        }

        public bool CheckGroupMapping(string GroupName)
        {
             string strConnStr = "";
             try
             {
                 strConnStr = strconnectionString;

                 SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@GroupName", GroupName)                   
              };

                 DataSet  ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "CheckGroupMapping", parm);

                 return Convert.ToBoolean(ds.Tables[0].Rows[0][0]);

             }
             catch (Exception ex)
             {
                 throw ex;
             }

        }

        public bool CheckOwnerMapping(string GroupName, string OwnerName, string strOwnerType)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@GroupName", GroupName),
                new SqlParameter("@OwnerName", OwnerName),
                new SqlParameter("@OwnerType", strOwnerType)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "CheckOwnerMapping", parm);

                return Convert.ToBoolean(ds.Tables[0].Rows[0][0]);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool CheckAllCO(string GroupName)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@GroupName", GroupName)
                                    
               };

                DataSet ds = clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "CheckAllCO", parm);

                return Convert.ToBoolean(ds.Tables[0].Rows[0][0]);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool CheckNoRestrictions(string GroupName)
        {
            string strConnStr = "";
            try
            {
                strConnStr = strconnectionString;

                SqlParameter[] parm = new SqlParameter[] 
              {
                  
                new SqlParameter("@GroupName", GroupName)
                                    
               };

                DataSet ds= clsDAL_SqlHelper.ExecuteDataset(strConnStr, CommandType.StoredProcedure, "CheckNoRestrictions", parm);

                return Convert.ToBoolean(ds.Tables[0].Rows[0][0]);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool SaveApplicationStatus(clsEALUser objclsAppOwner, int intAppId, string strQuarter)
        {
           
                SqlDataAdapter da = new SqlDataAdapter("SP_SaveControlOwnerApplicationStatus", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                string strCOSID = objclsAppOwner.StrUserSID;

                //string strCOName = objclsAppOwner.StrUserName;
                //string strADID = objclsAppOwner.StrUserADID;
                //string stremail = objclsAppOwner.StrUserEmailID;
                DataSet ds = new DataSet();

                 

                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = intAppId;

                para[1] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
                para[1].Value = strCOSID;


                para[2] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                para[2].Value = strQuarter;

                //para[4] = new SqlParameter("@ControlOwnerADID", SqlDbType.VarChar, 50);
                //para[4].Value = strADID;

                //para[5] = new SqlParameter("@Initiatives", SqlDbType.VarChar, 50);
                //para[5].Value = strInitiatives;

                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }

                da.Fill(ds);

           
            return true;

        }

        public bool SaveInitiative(string InitiativeNm, bool blnUpdate, bool blnVisible)
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("SP_SaveInitiatives1", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                
                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter("@InitiativeName", SqlDbType.VarChar,50);
                para[0].Value = InitiativeNm;

                para[1] = new SqlParameter("@Update", SqlDbType.Bit);
                para[1].Value = blnUpdate;

                para[2] = new SqlParameter("@Visible", SqlDbType.Bit);
                para[2].Value = blnVisible;

                para[3] = new SqlParameter("@exists", SqlDbType.Bit);
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
                bool blnSave;
                if (strDs == "True")
                {
                    blnSave = true;
                }
                else
                {
                    blnSave = false;
                }
                return blnSave;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;

        }


        public bool SaveServer(string strServer)
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("SP_SaveServer", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@ServerName", SqlDbType.VarChar, 100);
                para[0].Value = strServer;

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

        public bool SaveDBServer(string strServer, int servertypeid)
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("SP_SaveDBServer", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@ServerName", SqlDbType.VarChar, 100);
                para[0].Value = strServer;

                para[1] = new SqlParameter("@ServerTypeID", SqlDbType.Int);
                para[1].Value = servertypeid;

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

        public bool SaveApplicationApproverMapping(ArrayList arrclsApprover, int AppID, string strstrCurrentQuarter)
        {
            try
            {
                for (int i = 0; i < arrclsApprover.Count; i++)
                {
                    clsEALUser clsApprover = new clsEALUser();
                    clsApprover = (clsEALUser)arrclsApprover[i];
                    SqlDataAdapter da = new SqlDataAdapter("SP_SaveApplicationApproverMapping", strconnectionString);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 3600;
                    string strApproverSID = clsApprover.StrUserSID;
                    string strApproverName = clsApprover.StrUserName;
                    string strADID = clsApprover.StrUserADID;
                    string stremail = clsApprover.StrUserEmailID;
                    DataSet ds = new DataSet();


                    SqlParameter[] para = new SqlParameter[6];
                    para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                    para[0].Value = AppID;

                    para[1] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
                    para[1].Value = strApproverSID;

                    para[2] = new SqlParameter("@ApproverName", SqlDbType.VarChar, 50);
                    para[2].Value = strApproverName;

                    para[3] = new SqlParameter("@ApproverEmailID", SqlDbType.VarChar, 50);
                    para[3].Value = stremail;


                    para[4] = new SqlParameter("@ApproverADID", SqlDbType.VarChar, 50);
                    para[4].Value = strADID;

                     para[5] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                     para[5].Value = strstrCurrentQuarter;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return true;

        }
        #region Application ShareMapping
        
        public DataSet GetAppShareMapping()
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_GetApplicationMappedShare", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetAllApplications()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from tblsoxApplicationDetails where (Status=1 or Status is null) order by ApplicationName", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetServerTypes()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from tblSOXDBReportServerConfig", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetApplicationsByAppName( string appName)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from tblsoxApplicationDetails where ApplicationName = '" + appName +"'"  , strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet GetServerLists()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from tblSOXServerList where DeleteStatus=0 or DeleteStatus is null  order by ServerName", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        //GetCOFromSelectiveApproval
        public DataSet GetCOFromSelectiveApproval(string strrblCO_GA)
        {
            SqlDataAdapter da=null;
            if (strrblCO_GA == "1")
            {
                da = new SqlDataAdapter("select distinct COName,COSID from tblsoxSelectiveApproval1 where ISGlobal=0 and applicationid in "
                                        + "(select applicationid from tblsoxapplicationdetails where status =1 or status is null)"
                                        + "union "
                                        + "select distinct COName,COSID from tblsoxSelectiveApproval1_DB where ISGlobal=0 and applicationid in "
                                        + "(select applicationid from tblsoxapplicationdetails where status =1 or status is null)", strconnectionString);
            }
            else if (strrblCO_GA == "2")
            {
                da = new SqlDataAdapter("select distinct COName,COSID from tblsoxSelectiveApproval1 where ISGlobal=1 and cosid in (select usersid from tblsoxuserroles where UserRole='Global Approver' and Status=1 or status is null)"
                                    + "union "
                                    + "select distinct COName,COSID from tblsoxSelectiveApproval1_DB where ISGlobal=1 and cosid in (select usersid from tblsoxuserroles where UserRole='Global Approver' and Status=1 or status is null)", strconnectionString);
            }
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet GetOwner(string strrblCO_GA) 
        {
            SqlDataAdapter da = null;
            if (strrblCO_GA == "1")
            {
                da = new SqlDataAdapter("select distinct UserName as OwnerName, UserADID as ADID  from tblsoxuserRoles where (status =1 or status is null) and UserRole='Global Approver'", strconnectionString);
            }
            else if (strrblCO_GA == "2")
            {
                da = new SqlDataAdapter("select distinct ControlOwnerName as OwnerName , ControlOwnerADID as ADID from tblsoxapplicationdetails ", strconnectionString);
            }
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetSecurityGroup() 
        {
            SqlDataAdapter da = null;
            //da = new SqlDataAdapter("select distinct GroupName from tblSOXGroups order by GroupName", strconnectionString);
            da = new SqlDataAdapter("select distinct groupname from tblsoxgroups union select Distinct groupsamaccountname from tblsoxgroups where groupsamaccountname is not null order by groupname", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetSecurityGroupMapping()
        {
            SqlDataAdapter da = null;
            da = new SqlDataAdapter("SP_GetSecurityGroupMappings", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetDBServerLists()
        {
            //SqlDataAdapter da = new SqlDataAdapter("select distinct r.servername,r.serverid, r.servertype_id,serverType from tblSOXDBReportServer as r inner join tblsoxDBReportserverconfig as c on c.servertypeid=r.servertype_id", strconnectionString);
            SqlDataAdapter da = new SqlDataAdapter("select distinct r.servername,r.serverid, r.servertype_id,serverType from tblSOXDBReportServer as r inner join tblsoxDBReportserverconfig as c on c.servertypeid=r.servertype_id AND r.DeleteStatus = 0", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetDBServerTypes()
        {
            SqlDataAdapter da = new SqlDataAdapter("select distinct servertypeid,serverType from tblSOXDBReportServerConfig ", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetAllShares()
        {
            SqlDataAdapter da = new SqlDataAdapter("select ShareID,ShareName from tblSOXShareList order by ShareName", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public bool SaveApplicationMapping(int AppID, int Share, string StrUserADID)
        {
            try
            {
                
                SqlDataAdapter da = new SqlDataAdapter("SP_SaveApplicationMapping", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                
                DataSet ds = new DataSet();


                    SqlParameter[] para = new SqlParameter[3];
                    para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                    para[0].Value = AppID;

                    para[1] = new SqlParameter("@Share", SqlDbType.Int);
                    para[1].Value = Share;

                    para[2] = new SqlParameter("@CreatedBy", SqlDbType.VarChar);
                    para[2].Value = StrUserADID;
                
                   
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

        public bool SaveLinuxMapping(int AppID, string strServerName, string StrUserADID)
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("SP_SaveLinuxMapping", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = AppID;

                para[1] = new SqlParameter("@ServerName", SqlDbType.VarChar);
                para[1].Value = strServerName;

                para[2] = new SqlParameter("@CreatedBy", SqlDbType.VarChar);
                para[2].Value = StrUserADID;


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
        public bool SaveGroupMapping(int AppID, string strServerName, string strdomain, string StrUserADID)
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("SP_SaveGroupMapping", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = AppID;

                para[1] = new SqlParameter("@GroupName", SqlDbType.VarChar);
                para[1].Value = strServerName;

                para[2] = new SqlParameter("@CreatedBy", SqlDbType.VarChar);
                para[2].Value = StrUserADID;

                para[3] = new SqlParameter("@Domain", SqlDbType.VarChar);
                para[3].Value = strdomain;


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
        public DataSet GetAllApplicationList(string strUserSid, string[] role,string strQuarter)
        {
            string strRole="";
            SqlDataAdapter da = new SqlDataAdapter("SP_GetApplicationList1_VS_3", strconnectionString);

            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver))
            //{
            //    strQuarter = "";
            //}
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole+";" + role[i];

            }
            strRole = strRole.Substring(1);
            da.SelectCommand.CommandTimeout = 36000;
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@UserSID", SqlDbType.VarChar, 50);
                para[0].Value = strUserSid;
                para[1] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
                para[1].Value = strRole;
                para[2] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                para[2].Value = strQuarter;


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
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }


        public string GetAllApplicationStatus(string[] role, string strQuarter, int intAppId)
        {
            string strRole = "";
            SqlDataAdapter da = new SqlDataAdapter("SP_GetReportsForApp", strconnectionString);

            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver))
            //{
            //    strQuarter = "";
            //}
            for (int i = 0; i < role.Length; i++)
            {
                strRole = strRole + ";" + role[i];

            }
            strRole = strRole.Substring(1);

            da.SelectCommand.CommandTimeout = 36000;
            string strStatus = "";
            DataSet ds = new DataSet(); 
            try
            {
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@AppId", SqlDbType.Int);
                para[0].Value = intAppId;
                para[1] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
                para[1].Value = strRole;
                para[2] = new SqlParameter("@Quarter", SqlDbType.VarChar, 50);
                para[2].Value = strQuarter;


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
            catch (Exception Ex)
            {
                throw Ex;
            }
             
            if(ds != null)
            {
                strStatus = ds.Tables[0].Rows[0][0].ToString();  

            }
            return strStatus;
        }





        public bool InsertAppShareMapping(string Appid,string ShareID)
        {
            return true;
        }
        public bool UpdateAppShareMapping(int Appid, int ShareID, int rowid)
        {

            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateAppShareServer", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = Appid;

                para[1] = new SqlParameter("@ShareID", SqlDbType.Int);
                para[1].Value = ShareID;

                para[2] = new SqlParameter("@RowID", SqlDbType.Int);
                para[2].Value = rowid;


                


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

        public bool UpdateServerShare(string strShare, int serverid, int ShareID)
        {

            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateServerShare", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter("@ShareNm", SqlDbType.VarChar, 100);
                para[0].Value = strShare;

                para[1] = new SqlParameter("@Serverid", SqlDbType.Int);
                para[1].Value = serverid;

                para[2] = new SqlParameter("@ShareID", SqlDbType.VarChar, 100);
                para[2].Value = ShareID;




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

        public void DeleteAppShareMapping(int Appid, int ShareID, string StrUserADID)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteAppShare", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[3];

                pDel[0] = new SqlParameter("@AppID", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = Appid;

                pDel[1] = new SqlParameter("@ShareID", SqlDbType.Int);
                pDel[1].Direction = ParameterDirection.Input;
                pDel[1].Value = ShareID;

                pDel[2] = new SqlParameter("@ModifiedBy", SqlDbType.VarChar);
                pDel[2].Direction = ParameterDirection.Input;
                pDel[2].Value = StrUserADID;

                da_del.SelectCommand.Parameters.Add(pDel[0]);
                da_del.SelectCommand.Parameters.Add(pDel[1]);
                da_del.SelectCommand.Parameters.Add(pDel[2]);

                da_del.Fill(ds_del);
                //return true;
            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }

        public void UpdateLinuxServerMapping(int serverid,int appid, string StrUserADID, string servername)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_UpdateLinuxMapping", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[3];

                pDel[0] = new SqlParameter("@Serverid", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = serverid;

                pDel[1] = new SqlParameter("@AppId", SqlDbType.Int);
                pDel[1].Direction = ParameterDirection.Input;
                pDel[1].Value = appid;

                pDel[2] = new SqlParameter("@servername", SqlDbType.VarChar);
                pDel[2].Direction = ParameterDirection.Input;
                pDel[2].Value = servername;

                da_del.SelectCommand.Parameters.Add(pDel[0]);
                da_del.SelectCommand.Parameters.Add(pDel[1]);
                da_del.SelectCommand.Parameters.Add(pDel[2]);

                da_del.Fill(ds_del);
                //return true;
            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }

        public void UpdateGroupMapping(int serverid, int appid, string StrUserADID, string servername, string domain)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_UpdateGroupMapping", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[4];

                pDel[0] = new SqlParameter("@Groupid", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = serverid;

                pDel[1] = new SqlParameter("@AppId", SqlDbType.Int);
                pDel[1].Direction = ParameterDirection.Input;
                pDel[1].Value = appid;

                pDel[2] = new SqlParameter("@groupname", SqlDbType.VarChar);
                pDel[2].Direction = ParameterDirection.Input;
                pDel[2].Value = servername;

                pDel[3] = new SqlParameter("@domain", SqlDbType.VarChar);
                pDel[3].Direction = ParameterDirection.Input;
                pDel[3].Value = domain;

                da_del.SelectCommand.Parameters.Add(pDel[0]);
                da_del.SelectCommand.Parameters.Add(pDel[1]);
                da_del.SelectCommand.Parameters.Add(pDel[2]);
                da_del.SelectCommand.Parameters.Add(pDel[3]);


                da_del.Fill(ds_del);
                //return true;
            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }

        public void DeleteApproverMapping(ArrayList arrApprover,int Appid)
        {
            try
            {
                for (int i = 0; i < arrApprover.Count; i++)
                {
                    clsEALUser clsApprover = new clsEALUser();
                    clsApprover = (clsEALUser)arrApprover[i];
                    DataSet ds_del = new DataSet();
                    SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteApproverMapping", strconnectionString);
                    da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da_del.SelectCommand.CommandTimeout = 36000;
                    string ApproverToDelete = clsApprover.StrUserADID;
                    SqlParameter[] pDel = new SqlParameter[3];

                    pDel[0] = new SqlParameter("@AppAdid", SqlDbType.VarChar, 50);
                    pDel[0].Direction = ParameterDirection.Input;
                    pDel[0].Value = ApproverToDelete;

                    pDel[1] = new SqlParameter("@AppID", SqlDbType.Int);
                    pDel[1].Direction = ParameterDirection.Input;
                    pDel[1].Value = Appid;

                    

                    da_del.SelectCommand.Parameters.Add(pDel[0]);
                    da_del.SelectCommand.Parameters.Add(pDel[1]);
                   

                    da_del.Fill(ds_del);
                    //return true;
                }
            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }

        public bool InsertApplicationDetails(int AppID,string AppName,string Initiative,clsEALUser objControlOwner)
        {
            return true;
        }
        public bool UpdateApplicationDetails(string strRole, int AppID, string AppName, string Initiative, clsEALUser objControlOwner, bool Admin, bool MultipleApprovals, bool flagExclude,bool flagUnlockApp,string Proccycle, string quarter)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateAppDetails", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                //objControlOwner = new clsEALUser();
                string strCOName = objControlOwner.StrUserName;
                string strCOSID = objControlOwner.StrUserSID;
                string strEmail = objControlOwner.StrUserEmailID;
                string strAdid = objControlOwner.StrUserADID;
                string strInitiatives = Initiative;
            

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[15];
                para[0] = new SqlParameter("@AppID", SqlDbType.Int);
                para[0].Value = AppID;

                para[1] = new SqlParameter("@AppNm", SqlDbType.VarChar, 50);
                para[1].Value = AppName;

                para[2] = new SqlParameter("@Initiative", SqlDbType.NVarChar, 1000);
                para[2].Value = Initiative;

                para[3] = new SqlParameter("@ControlOwnerName", SqlDbType.VarChar,50);
                para[3].Value = strCOName;

                para[4] = new SqlParameter("@ControlOwnerSID", SqlDbType.VarChar,50);
                para[4].Value = strCOSID;

                para[5] = new SqlParameter("@ControlOwnerEmailID", SqlDbType.VarChar,100);
                para[5].Value = strEmail;

                para[6] = new SqlParameter("@ControlOwnerADID", SqlDbType.VarChar,50);
                para[6].Value = strAdid;

                para[7] = new SqlParameter("@Role", SqlDbType.VarChar, 50);
                para[7].Value = strRole;

                para[8] = new SqlParameter("@Admin", SqlDbType.Bit);
                para[8].Value = Admin;

                para[9] = new SqlParameter("@MultipleApprovals", SqlDbType.Bit);
                para[9].Value = MultipleApprovals;

                para[10] = new SqlParameter("@ExcludeGA", SqlDbType.Bit);
                para[10].Value = flagExclude;

                para[11] = new SqlParameter("@flagUnlockApp", SqlDbType.Bit);
                para[11].Value = flagUnlockApp;

                para[12] = new SqlParameter("@flagUpdated", SqlDbType.Bit);
                para[12].Direction = ParameterDirection.Output;

                para[13] = new SqlParameter("@frequency", SqlDbType.VarChar, 20);
                para[13].Value = Proccycle;

                para[14] = new SqlParameter("@quarter", SqlDbType.VarChar, 20);
                para[14].Value = quarter;


                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }

                da.Fill(ds);

                string strUpdated = ds.Tables[0].Rows[0][0].ToString();
                bool updatedflag;
                if (strUpdated == "True")
                {
                    updatedflag = true;
                }
                else
                {
                    updatedflag = false;
                }
                return updatedflag;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }


        public bool UpdateInitiatives(int InitiaveID, bool blnUpdate, bool blnVisible)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateInitiative", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
               
                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@initiativeID", SqlDbType.Int);
                para[0].Value = InitiaveID;

                para[1] = new SqlParameter("@Update", SqlDbType.Bit);
                para[1].Value = blnUpdate;

                para[2] = new SqlParameter("@Visible", SqlDbType.Bit);
                para[2].Value = blnVisible;

                
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


        public bool UpdateServer(int ServerID,string serverName)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateServer", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@ServerID", SqlDbType.Int);
                para[0].Value = ServerID;

                para[1] = new SqlParameter("@ServerName", SqlDbType.VarChar,100);
                para[1].Value = serverName;

                


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


        public bool UpdateServerStatus(string serverName)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateServerStatus", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@ServerName", SqlDbType.VarChar, 100);
                para[0].Value = serverName;


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

        public bool UpdateDBServer(int ServerID, string serverName,int intServerType_ID)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateDBServer", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@ServerID", SqlDbType.Int);
                para[0].Value = ServerID;

                para[1] = new SqlParameter("@ServerName", SqlDbType.VarChar, 100);
                para[1].Value = serverName;

                para[2] = new SqlParameter("@ServerType_ID", SqlDbType.VarChar, 100);
                para[2].Value = intServerType_ID;

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

        public bool UpdateDBServerStatus(string serverName, int intServerType_ID)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateDBServerStatus", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter("@ServerName", SqlDbType.VarChar, 100);
                para[0].Value = serverName;

                para[1] = new SqlParameter("@ServerType_ID", SqlDbType.VarChar, 100);
                para[1].Value = intServerType_ID;



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


        public bool DeleteApplicationDetails(int AppID, string ModifiedBy)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteAppDetails", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[3];

                pDel[0] = new SqlParameter("@AppID", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = AppID;

                pDel[1] = new SqlParameter("@ModifiedBy", SqlDbType.VarChar);
                pDel[1].Direction = ParameterDirection.Input;
                pDel[1].Value = ModifiedBy;

                pDel[2] = new SqlParameter("@flagDelete", SqlDbType.Bit);
                pDel[2].Direction = ParameterDirection.Output;

                

                foreach (SqlParameter p in pDel)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da_del.SelectCommand.Parameters.Add(p);

                }
                da_del.Fill(ds_del);

                string strDs = ds_del.Tables[0].Rows[0][0].ToString();
                bool flagDelete;
                if (strDs == "True")
                {
                    flagDelete = true;
                }
                else
                {
                    flagDelete = false;
                }
                return flagDelete;
                //return true;
            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
            return true;
        }


        public bool DeleteInitiative(int Initiative)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteInitiatives1", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[2];

                pDel[0] = new SqlParameter("@InitiativeID", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = Initiative;

                pDel[1] = new SqlParameter("@flagexists", SqlDbType.Bit);
                pDel[1].Direction = ParameterDirection.Output;

                foreach (SqlParameter p in pDel)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da_del.SelectCommand.Parameters.Add(p);

                }
                da_del.Fill(ds_del);

                string strDs = ds_del.Tables[0].Rows[0][0].ToString();
                bool flagexists;
                if (strDs == "True")
                {
                    flagexists = true;
                }
                else
                {
                    flagexists = false;
                }
                return flagexists;

            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }

        public void DeleteDBServer(int ServerID)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                //SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteServer", strconnectionString);
                SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteDBServer1", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[1];

                pDel[0] = new SqlParameter("@ServerID", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = ServerID;

                foreach (SqlParameter p in pDel)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da_del.SelectCommand.Parameters.Add(p);

                }
                da_del.Fill(ds_del);
            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }


        public void DeleteServerDB(int ServerID)
        {

            try
            {
                clsDAL_SqlHelper.ExecuteDataset
                       (
                           strconnectionString,
                           CommandType.StoredProcedure,
                           "SP_DeleteServer_DB",
                           new SqlParameter("@ServerID", ServerID)
                        );


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool DeleteServer(int ServerID, string ModifiedBy)
        {


            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteServer", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[3];

                pDel[0] = new SqlParameter("@ServerID", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = ServerID;

                pDel[1] = new SqlParameter("@ModifiedBy", SqlDbType.VarChar,200);
                pDel[1].Direction = ParameterDirection.Input;
                pDel[1].Value = ModifiedBy;

                pDel[2] = new SqlParameter("@flagexists", SqlDbType.Bit);
                pDel[2].Direction = ParameterDirection.Output;

                foreach (SqlParameter p in pDel)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da_del.SelectCommand.Parameters.Add(p);

                }
                da_del.Fill(ds_del);

                string strDs = ds_del.Tables[0].Rows[0][0].ToString();
                bool flagexists;
                if (strDs == "True")
                {
                    flagexists = true;
                }
                else
                {
                    flagexists = false;
                }
                return flagexists;

            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }
        public DataTable GetUnlockApprover(string ApproverADID)
        {
            try
            {
                return (clsDAL_SqlHelper.ExecuteDataset
                        (
                            strconnectionString,
                            CommandType.StoredProcedure,
                            "sp_GetUnlockApprover",
                            new SqlParameter("@ApproverADID", ApproverADID)
                         ).Tables[0]);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUnlockCO(string ApproverADID) 
        {
            try
            {
                return (clsDAL_SqlHelper.ExecuteDataset
                        (
                            strconnectionString,
                            CommandType.StoredProcedure,
                            "sp_GetUnlockCO",
                            new SqlParameter("@ControlOwnerADID", ApproverADID)
                         ).Tables[0]);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CheckApprover(int AppID, string strName)
        {
            DataSet ds = new DataSet();
            try
            {
                
                SqlDataAdapter da = new SqlDataAdapter("usp_CheckApprovers", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 36000;
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@ApplicationId", SqlDbType.Int);
                para[0].Value = AppID;
                para[0].Direction = ParameterDirection.Input;
                para[1] = new SqlParameter("@ApproverName", SqlDbType.VarChar);
                para[1].Value = strName;
                para[1].Direction = ParameterDirection.Input;
                para[2]=new SqlParameter ("@flag",SqlDbType.Bit);
                para[2].Direction=ParameterDirection.Output;

                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);
                }
                da.Fill(ds);
                string flagvalue = ds.Tables[0].Rows[0][0].ToString();
                bool flag;
                if (flagvalue == "True")
                {
                    flag = true;
                }
                else
                {
                    flag= false;
                }
                return flag;
  
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
           // return ds;
            
        }

      
    }
}
