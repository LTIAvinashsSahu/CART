using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CART_EAL;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;

namespace CART_DAL
{
  public  class clsDALMasterData:clsDBConnection 
    {
      SqlConnection con = null;
        public ArrayList FetchMasterData(string Key)
        {
            return new ArrayList();
        }
        public bool InsertNewInitiative(string InitiativeName,bool update,bool VisibleToAuditor)
        {
            return true;
        }
        public DataSet GetShareLists()
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_GetShareList", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
            
        }

        public DataSet GetServerLists()
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_GetServerList", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }
        public DataSet GetGroupList()
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_GetGroupList", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }

        public string GeTServerForShare(string strShareName)
        {
 
            SqlDataAdapter da = new SqlDataAdapter("select ServerName from tblSOXServerList as sl inner join tblSOXShareList as sh on sh.ServerID=sl.ServerID where ShareName like'%"+strShareName+"'",strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.CommandTimeout = 3600;
            DataSet ds = new DataSet();
            string strServerName = "";
            da.Fill(ds);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dtrow = ds.Tables[0].Rows[0];
                    if (dtrow != null)
                    {
                        strServerName = dtrow["ServerName"].ToString();
                    }
                }
            }
            return strServerName;
        }

        public DataSet GetDatabaseLists()
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_GetDBMappingList", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }

        public int GeTDB_ID(string DatabaseName)
        {
            string Query = "select DBID from tblsoxDatabasemapping where Datbasename ='"+ DatabaseName+"'";
            SqlDataAdapter da = new SqlDataAdapter(Query, strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.CommandTimeout = 3600;
            DataSet ds = new DataSet();
            int DB_ID = 0;
            da.Fill(ds);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dtrow = ds.Tables[0].Rows[0];
                    if (dtrow != null)
                    {
                        DB_ID = Convert.ToInt32(dtrow["DB_ID"]);
                    }
                }
            }
            return DB_ID;
        }

        public bool SaveDBMapping(string strDatabaseName, int ServerID)
        {
            try
            {
                //string User = ConfigurationManager.AppSettings["ADIDTemp"].ToString().Trim();
                SqlDataAdapter da = new SqlDataAdapter("SP_SaveDatabaseMapping", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                // strShareName = strShareName.Replace("\\","\\\\");
                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@DatabaseName", SqlDbType.VarChar, 500);
                para[0].Value = strDatabaseName;

                para[1] = new SqlParameter("@ServerID", SqlDbType.Int);
                para[1].Value = ServerID;

                para[2] = new SqlParameter("@User", SqlDbType.VarChar, 500);
                para[2].Value = "";

                //para[3] = new SqlParameter("@DB_ID", SqlDbType.Int);
                //para[3].Value = User;

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

        public bool SaveDBMapping(string strDBName, int ServerID, int AppID,string CreatedBy)
        {
            try
            {
                //string User = ConfigurationManager.AppSettings["ADIDTemp"].ToString().Trim();
                SqlDataAdapter da = new SqlDataAdapter("SP_SaveDatabaseMapping", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
                // strShareName = strShareName.Replace("\\","\\\\");
                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter("@DatabaseName", SqlDbType.VarChar, 500);
                para[0].Value = strDBName;

                para[1] = new SqlParameter("@ServerID", SqlDbType.Int);
                para[1].Value = ServerID;

                para[2] = new SqlParameter("@AppID", SqlDbType.Int);
                para[2].Value = AppID;

                para[3] = new SqlParameter("@CreatedBy", SqlDbType.VarChar);
                para[3].Value=CreatedBy;

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


        public int GetDatabaseID(string strDBNm)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_DatabaseMappingID", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[1];

                pDel[0] = new SqlParameter("@DatabaseNm", SqlDbType.VarChar, 100);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = strDBNm;



                da_del.SelectCommand.Parameters.Add(pDel[0]);

                da_del.Fill(ds_del);
                int ShareID = Convert.ToInt16(ds_del.Tables[0].Rows[0][0]);
                return ShareID;

            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Getting share id", ex);
                ExNew.Source = "CommonLogic.GetShareID";
                throw ExNew;
            }
        }

        public bool CheckIfServerDBMapping(string strDBNM, int ServerID)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_CheckDB", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter("@DBNM", SqlDbType.VarChar, 100);
            para[0].Value = strDBNM;

            para[1] = new SqlParameter("@ServerID", SqlDbType.Int);
            para[1].Value = ServerID;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            string strShareExists = ds.Tables[0].Rows[0][0].ToString();
            //string strServerMapExists = ds.Tables[0].Rows[0][1].ToString();
            bool flagShareExists;
            //bool flagServerMapExists;
            if (strShareExists == "False")
            {
                flagShareExists = false;
            }
            else
            {
                flagShareExists = true;
            }
            //    if (strServerMapExists == "False")
            //    {
            //        flag = true;
            //    }
            //}
            //else
            //{
            //    flagShareExists = true;
            //}
            conn.Close();
            return flagShareExists;
        }


        public DataSet CheckIfDatabaseExists(string strDatabaseNm,int strServerID,int strAppID)
        {
            SqlConnection conn = new SqlConnection(strconnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SP_CheckDatabaseNm", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;

            DataSet ds = new DataSet();

            conn.Open();

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@DatabaseName", SqlDbType.VarChar, 500);
            para[0].Value = strDatabaseNm;
            para[1] = new SqlParameter("@ServerID", SqlDbType.Int);
            para[1].Value = strServerID;
            para[2] = new SqlParameter("@AppID", SqlDbType.Int);
            para[2].Value = strAppID;
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

        public string GetServerForReport(int repID)
        {

            SqlDataAdapter da = new SqlDataAdapter("select servername from tblsoxsharedetails where Reportid='"+repID+"' union select servername from tblsoxserverdetails where reportid='"+repID+ "'", strconnectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.CommandTimeout = 3600;
            DataSet ds = new DataSet();
            string strServerName = "";
            da.Fill(ds);
            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {
                    DataRow dtrow = ds.Tables[0].Rows[0];
                    if (dtrow != null)
                    {
                        strServerName = dtrow["ServerName"].ToString();
                    }
                }
            }
            return strServerName;
        }

        //[SP_SaveShare]

        public bool SaveShare(string strShareName, int ServerID)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_SaveShare", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;
               // strShareName = strShareName.Replace("\\","\\\\");
                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@ShareName", SqlDbType.VarChar,500);
                para[0].Value = strShareName;

                para[1] = new SqlParameter("@ServerID", SqlDbType.Int);
                para[1].Value = ServerID;


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

        public void DeleteShare(int ShareID)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteShare", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[1];

                pDel[0] = new SqlParameter("@ShareID", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = ShareID;

               

                da_del.SelectCommand.Parameters.Add(pDel[0]);
                
                da_del.Fill(ds_del);

            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }

      public void DeleteLinuxServerMapping(int serverid)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_DeleteLinuxMapping", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[1];

                pDel[0] = new SqlParameter("@serverid", SqlDbType.Int);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = serverid;



                da_del.SelectCommand.Parameters.Add(pDel[0]);

                da_del.Fill(ds_del);

            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Deleting", ex);
                ExNew.Source = "CommonLogic.RowDeleting";
                throw ExNew;
            }
        }
      public void DeleteGroupMapping(int groupid)
      {
          try
          {
              //strAppNm = ddlAppName.SelectedItem.Value.ToString();
              DataSet ds_del = new DataSet();
              SqlDataAdapter da_del = new SqlDataAdapter("[SP_DeleteSecurityGroupMapping]", strconnectionString);
              da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
              da_del.SelectCommand.CommandTimeout = 36000;

              SqlParameter[] pDel = new SqlParameter[1];

              pDel[0] = new SqlParameter("@groupid", SqlDbType.Int);
              pDel[0].Direction = ParameterDirection.Input;
              pDel[0].Value = groupid;



              da_del.SelectCommand.Parameters.Add(pDel[0]);

              da_del.Fill(ds_del);

          }
          catch (Exception ex)
          {

              Exception ExNew = new Exception("Error While Deleting", ex);
              ExNew.Source = "CommonLogic.RowDeleting";
              throw ExNew;
          }
      }

        public int GetShareID(string strShareNm)
        {
            try
            {
                //strAppNm = ddlAppName.SelectedItem.Value.ToString();
                DataSet ds_del = new DataSet();
                SqlDataAdapter da_del = new SqlDataAdapter("SP_ShareID", strconnectionString);
                da_del.SelectCommand.CommandType = CommandType.StoredProcedure;
                da_del.SelectCommand.CommandTimeout = 36000;

                SqlParameter[] pDel = new SqlParameter[1];

                pDel[0] = new SqlParameter("@ShareNm", SqlDbType.VarChar);
                pDel[0].Direction = ParameterDirection.Input;
                pDel[0].Value = strShareNm;



                da_del.SelectCommand.Parameters.Add(pDel[0]);

                da_del.Fill(ds_del);
                int ShareID = Convert.ToInt16(ds_del.Tables[0].Rows[0][0]);
                return ShareID;

            }
            catch (Exception ex)
            {

                Exception ExNew = new Exception("Error While Getting share id", ex);
                ExNew.Source = "CommonLogic.GetShareID";
                throw ExNew;
            }
        }
        public bool CheckIfShareExists(string strShareNm)
        {
            SqlConnection conn = new SqlConnection(strconnectionString);
            
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckShareNm", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();
                
                conn.Open();

                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@ShareName", SqlDbType.VarChar);
                para[0].Value = strShareNm;

                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);

                }
                da.Fill(ds);

                string strShareExists = ds.Tables[0].Rows[0][0].ToString();
                //string strServerMapExists = ds.Tables[0].Rows[0][1].ToString();
                bool flagShareExists;
                //bool flagServerMapExists;
                if (strShareExists == "False")
                {
                    flagShareExists = false;
                }
                else
                {
                    flagShareExists = true;
                }
                //    if (strServerMapExists == "False")
                //    {
                //        flag = true;
                //    }
                //}
                //else
                //{
                //    flagShareExists = true;
                //}
                conn.Close();
                return flagShareExists;
           
               
            
        }

        public bool CheckIfServerExists(string strServerName)
        {
            SqlConnection conn = new SqlConnection(strconnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SP_CheckServerExists", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;

            DataSet ds = new DataSet();

            conn.Open();

            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter("@ServerName", SqlDbType.VarChar);
            para[0].Value = strServerName;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            //string strShareExists = ds.Tables[0].Rows[0][0].ToString();
            ////string strServerMapExists = ds.Tables[0].Rows[0][1].ToString();
            bool flagServerExists;
            ////bool flagServerMapExists;
            //if (strShareExists == "False")
            //{
            //    flagShareExists = false;
            //}
            //else
            //{
            //    flagShareExists = true;
            //}
            if (ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0))
            {
                flagServerExists = true;
            }
            else
                flagServerExists = false;

            conn.Close();
            return flagServerExists;
        }

      public bool CheckIfLinuxMappingExists(string strShareName,int intAppID)
      {
          SqlConnection conn = new SqlConnection(strconnectionString);
            
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckLinuxMappingExists", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();
                
                conn.Open();

                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@ServerName", SqlDbType.VarChar);
                para[0].Value = strShareName;
                para[1] = new SqlParameter("@AppID", SqlDbType.VarChar);
                para[1].Value = intAppID;

                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);

                }
                da.Fill(ds);

                bool flagServerExists;
                ////bool flagServerMapExists;
                //if (strShareExists == "False")
                //{
                //    flagShareExists = false;
                //}
                //else
                //{
                //    flagShareExists = true;
                //}
                if (ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0))
                {
                    flagServerExists = true;
                }
                else
                    flagServerExists = false;
                //    if (strServerMapExists == "False")
                //    {
                //        flag = true;
                //    }
                //}
                //else
                //{
                //    flagShareExists = true;
                //}
                conn.Close();
                return flagServerExists;
      }

      public bool CheckIfGroupMappingExists(string strGroupName, string strdomain, int appid)
      {
          SqlConnection conn = new SqlConnection(strconnectionString);

          SqlDataAdapter da = new SqlDataAdapter("SP_CheckGroupExists", strconnectionString);
          da.SelectCommand.CommandType = CommandType.StoredProcedure;
          da.SelectCommand.CommandTimeout = 3600;

          DataSet ds = new DataSet();

          conn.Open();

          SqlParameter[] para = new SqlParameter[3];
          para[0] = new SqlParameter("@GroupName", SqlDbType.VarChar);
          para[0].Value = strGroupName;
          para[1] = new SqlParameter("@Domain", SqlDbType.VarChar);
          para[1].Value = strdomain;
          para[2] = new SqlParameter("@Appid", SqlDbType.Int);
          para[2].Value = appid;

          foreach (SqlParameter p in para)
          {
              if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
              {
                  p.Value = DBNull.Value;
              }
              da.SelectCommand.Parameters.Add(p);

          }
          da.Fill(ds);

          //string strShareExists = ds.Tables[0].Rows[0][0].ToString();
          ////string strServerMapExists = ds.Tables[0].Rows[0][1].ToString();
          bool flagServerExists;
          ////bool flagServerMapExists;
          //if (strShareExists == "False")
          //{
          //    flagShareExists = false;
          //}
          //else
          //{
          //    flagShareExists = true;
          //}
          if (ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0))
          {
              flagServerExists = true;
          }
          else
              flagServerExists = false;

          conn.Close();
          return flagServerExists;
      }

      public bool CheckIfSecGrpMappingExists(string strGroupName, string strdomain, int appid)
      {
          SqlConnection conn = new SqlConnection(strconnectionString);

          SqlDataAdapter da = new SqlDataAdapter("SP_CheckSecGroupExists", strconnectionString);
          da.SelectCommand.CommandType = CommandType.StoredProcedure;
          da.SelectCommand.CommandTimeout = 3600;

          DataSet ds = new DataSet();

          conn.Open();

          SqlParameter[] para = new SqlParameter[3];
          para[0] = new SqlParameter("@GroupName", SqlDbType.VarChar);
          para[0].Value = strGroupName;
          para[1] = new SqlParameter("@Domain", SqlDbType.VarChar);
          para[1].Value = strdomain;
          para[2] = new SqlParameter("@Appid", SqlDbType.Int);
          para[2].Value = appid;

          foreach (SqlParameter p in para)
          {
              if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
              {
                  p.Value = DBNull.Value;
              }
              da.SelectCommand.Parameters.Add(p);

          }
          da.Fill(ds);

          //string strShareExists = ds.Tables[0].Rows[0][0].ToString();
          ////string strServerMapExists = ds.Tables[0].Rows[0][1].ToString();
          bool flagServerExists;
          ////bool flagServerMapExists;
          //if (strShareExists == "False")
          //{
          //    flagShareExists = false;
          //}
          //else
          //{
          //    flagShareExists = true;
          //}
          if (ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0))
          {
              flagServerExists = true;
          }
          else
              flagServerExists = false;

          conn.Close();
          return flagServerExists;
      }

      public bool CheckIfGroupIDExists(int groupid)
      {
          SqlConnection conn = new SqlConnection(strconnectionString);

          SqlDataAdapter da = new SqlDataAdapter("SP_CheckGroupIDExists", strconnectionString);
          da.SelectCommand.CommandType = CommandType.StoredProcedure;
          da.SelectCommand.CommandTimeout = 3600;

          DataSet ds = new DataSet();

          conn.Open();

          SqlParameter[] para = new SqlParameter[1];
        
          para[0] = new SqlParameter("@GroupID", SqlDbType.Int);
          para[0].Value = groupid;

          foreach (SqlParameter p in para)
          {
              if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
              {
                  p.Value = DBNull.Value;
              }
              da.SelectCommand.Parameters.Add(p);

          }
          da.Fill(ds);

          //string strShareExists = ds.Tables[0].Rows[0][0].ToString();
          ////string strServerMapExists = ds.Tables[0].Rows[0][1].ToString();
          bool flagServerExists;
          ////bool flagServerMapExists;
          //if (strShareExists == "False")
          //{
          //    flagShareExists = false;
          //}
          //else
          //{
          //    flagShareExists = true;
          //}
          if (ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0))
          {
              flagServerExists = true;
          }
          else
              flagServerExists = false;

          conn.Close();
          return flagServerExists;
 
      }

        public bool CheckIfServerShareExists(string strShareNm, int ServerID)
        {
            SqlConnection conn = new SqlConnection(strconnectionString);
           
                SqlDataAdapter da = new SqlDataAdapter("SP_CheckShare", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();

                conn.Open();

                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@ShareName", SqlDbType.VarChar);
                para[0].Value = strShareNm;

                para[1] = new SqlParameter("@ServerID", SqlDbType.Int);
                para[1].Value = ServerID;

                foreach (SqlParameter p in para)
                {
                    if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    da.SelectCommand.Parameters.Add(p);

                }
                da.Fill(ds);

                string strShareExists = ds.Tables[0].Rows[0][0].ToString();
                //string strServerMapExists = ds.Tables[0].Rows[0][1].ToString();
                bool flagShareExists;
                //bool flagServerMapExists;
                if (strShareExists == "False")
                {
                    flagShareExists = false;
                }
                else
                {
                    flagShareExists = true;
                }
                //    if (strServerMapExists == "False")
                //    {
                //        flag = true;
                //    }
                //}
                //else
                //{
                //    flagShareExists = true;
                //}
                conn.Close();
                return flagShareExists;
           
        }

        public bool CheckIfServerShareMapping(string strShareNm, int ServerID)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_CheckShare1", strconnectionString);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 3600;

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(strconnectionString);
            conn.Open();

            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter("@ShareName", SqlDbType.VarChar);
            para[0].Value = strShareNm;

            para[1] = new SqlParameter("@ServerID", SqlDbType.Int);
            para[1].Value = ServerID;

            foreach (SqlParameter p in para)
            {
                if ((p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                da.SelectCommand.Parameters.Add(p);

            }
            da.Fill(ds);

            string strShareExists = ds.Tables[0].Rows[0][0].ToString();
            //string strServerMapExists = ds.Tables[0].Rows[0][1].ToString();
            bool flagShareExists;
            //bool flagServerMapExists;
            if (strShareExists == "False")
            {
                flagShareExists = false;
            }
            else
            {
                flagShareExists = true;
            }
            //    if (strServerMapExists == "False")
            //    {
            //        flag = true;
            //    }
            //}
            //else
            //{
            //    flagShareExists = true;
            //}
            conn.Close();
            return flagShareExists;
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
        public bool UpdateServerName(string serverID,string servername)
        {
            return true;
        }
        public bool InsertShareName(string shareName,string serverID)
        {
            return true;
        }
        public bool DeleteShareName(string shareID)
        {
            return true;
        }
        public bool UpdateshareName(int shareID,string shareName,int serverID)
        {
            
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateShare", strconnectionString);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 3600;

                DataSet ds = new DataSet();


                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@ShareID", SqlDbType.Int);
                para[0].Value = shareID;

                para[1] = new SqlParameter("@ShareName", SqlDbType.VarChar, 500);
                para[1].Value = shareName;

                para[2] = new SqlParameter("@ServerID", SqlDbType.Int);
                para[2].Value = serverID;

                


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
      /// <summary>
      /// Method to update DBMapping
      /// Written by PM
      /// </summary>
      /// <param name="Dbname"></param>
      /// <param name="ShareId"></param>
      /// <param name="AppId"></param>
      /// <param name="DbId"></param>
      /// <returns>bool value whether true or false </returns>
        public bool UpdateDBMapping(string Dbname, int AppId, int ServerId, int DbId, string ModifiedBy)
         {
             try
             {
                 SqlDataAdapter da = new SqlDataAdapter("SP_UpdateDatabseMapping", strconnectionString);
                 da.SelectCommand.CommandType = CommandType.StoredProcedure;
                 da.SelectCommand.CommandTimeout = 3600;
                 DataSet ds = new DataSet();
                 SqlParameter[] para = new SqlParameter[5];
                 para[0] = new SqlParameter("@DatabaseName",SqlDbType.VarChar);
                 para[0].Value =Dbname;
                 para[1] = new SqlParameter("@ApplicationID", SqlDbType.Int);
                 para[1].Value = AppId;
                 para[2] = new SqlParameter("@ServerID", SqlDbType.Int);
                 para[2].Value =ServerId;
                 para[3]=new SqlParameter("@DBID",SqlDbType.Int);
                 para[3].Value = DbId;
                 para[4] = new SqlParameter("ModifiedBy", SqlDbType.VarChar);
                 para[4].Value = ModifiedBy;

                 foreach (SqlParameter param in para)
                 {
                     if((param.Direction==ParameterDirection.Input) && (param.Value==null))
                     {
                         param.Value=DBNull.Value;
                     }
                        da.SelectCommand.Parameters.Add(param);
                 }
                 da.Fill(ds);
               }
             catch (Exception ex)
             {
                 throw ex;
             }
           return true;
      
         }// "UpdateDBMapping"Methods Ends
        /// <summary>
        /// Method to Delete the Mapping Between Database,Application & Server 
        /// </summary>
        /// <param name="DBID"></param>
        /// <returns></returns>
       public bool DeleteDBMapping(int DBID,string ModifiedBy)
       {
           try
           {
               SqlDataAdapter da = new SqlDataAdapter("Sp_DeleteDBMapping", strconnectionString);
               da.SelectCommand.CommandType = CommandType.StoredProcedure;
               da.SelectCommand.CommandTimeout = 3600;
               DataSet ds = new DataSet();
               SqlParameter[] para = new SqlParameter[2];
               para[0] = new SqlParameter("@DBID", SqlDbType.Int);
               para[0].Value = DBID;
               para[1] = new SqlParameter("@ModifiedBy", SqlDbType.VarChar);
               para[1].Value = ModifiedBy;
               foreach (SqlParameter param in para)
               {
                   if ((param.Direction == ParameterDirection.Input) && (param.Value == null))
                   {
                       param.Value = DBNull.Value;
                   }
                   da.SelectCommand.Parameters.Add(param);
               }
               da.Fill(ds);
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return true;
       } // "DeleteDBMapping" Method Ends
  }//Class Ends
}// Namespace Ends
