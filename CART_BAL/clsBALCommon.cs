using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CART_EAL;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using CART_DAL;
using System.Data;
using System.Net.Mail;
using System.Configuration;

namespace CART_BAL
{
     public class clsBALCommon
    {

         clsDALCommon objclsDALCommon;
         public static string strDomains = "";
         public static clsBALCommon objclsBALCommon = new clsBALCommon();

         //code added by Suman
         public DataTable GetCommonFields(string Field)
         {
             objclsDALCommon = new clsDALCommon();
             return (objclsDALCommon.GetCommonFields(Field));
         }
         public DataTable GetRoleWiseUser(int roleID)
         {
             objclsDALCommon = new clsDALCommon();
             return (objclsDALCommon.GetRoleWiseUser(roleID));
         }
         public void ModifyAdminRight(string strUserSID, int intAppID, string scope, string strLoggedInUserSID, string strSelectedQuarter)
         {
             objclsDALCommon = new clsDALCommon();
             objclsDALCommon.ModifyAdminRights(strUserSID,intAppID,scope,strLoggedInUserSID,strSelectedQuarter);
         }
         public void ModifyReportAdminRight(string strUserSID, string strGroupSID,int AppID, int RepID, string scope, string strLoggedInUserSID, string strSelectedQuarter)
         {
             objclsDALCommon = new clsDALCommon();
             objclsDALCommon.ModifyReportAdminRights(strUserSID, strGroupSID,AppID, RepID, scope, strLoggedInUserSID, strSelectedQuarter);
         }

         public void ModifyAdminRight_SQL(string strUserSID, int intAppID, string scope, string strLoggedInUserSID, string strSelectedQuarter, string strDBType)
         {
             objclsDALCommon = new clsDALCommon();
             objclsDALCommon.ModifyAdminRights_SQL(strUserSID, intAppID, scope, strLoggedInUserSID, strSelectedQuarter, strDBType);
         }

         public DataSet FetchApproval(string strLoggedInSID)
         {
             objclsDALCommon = new clsDALCommon();
             return (objclsDALCommon.FetchApproval(strLoggedInSID));
         }

         public DataSet FetchApproval_DB(string strLoggedInSID,string strDBType)
         {
             objclsDALCommon = new clsDALCommon();
             return (objclsDALCommon.FetchApproval_DB(strLoggedInSID, strDBType));
         }
         public DataSet FetchApproval_PSI(string strLoggedInSID)
         {
             objclsDALCommon = new clsDALCommon();
             return (objclsDALCommon.FetchApproval_PSI(strLoggedInSID));
         }

         //code end here

         public void sendMailBMC(string strTo, string strCc, string strSubject, string strBody)
         {
             MailMessage objMailMsg = new MailMessage();
             objMailMsg.IsBodyHtml = false;
             objMailMsg.From = new MailAddress(System.Configuration.ConfigurationSettings.AppSettings["MailFromField"]);
             
             //if more than one email addresses present in To/Cc field
             if (strTo != null && (strTo != string.Empty))
             {
                 ArrayList aryTo = new ArrayList(strTo.Split(new char[] { ';' }));
                 foreach (string s in aryTo)
                 {
                     objMailMsg.To.Add(s);
                 }
             }
             if (strCc != null && (strCc != string.Empty))
             {
                 ArrayList aryCc = new ArrayList(strCc.Split(new char[] { ';' }));
                 foreach (string s in aryCc)
                 {
                     objMailMsg.CC.Add(s);
                 }
             }

             

             // objMailMsg.To.Add(strTo);
             //objMailMsg.Bcc.Add("");

             // Set the subject of the mail messagel
             objMailMsg.Subject = strSubject;

             // Set the body of the mail message
             objMailMsg.Body = strBody;

             // Set the format of the mail message body as HTML
             objMailMsg.IsBodyHtml = false;

             // Set the priority of the mail message to normal
             objMailMsg.Priority = MailPriority.Normal;

             // Instantiate a new instance of SmtpClient
             SmtpClient objSmtpClient = new SmtpClient();
             // Send the mail message
             objSmtpClient.Send(objMailMsg);
         }

         public void sendMail(string strTo, string strCc, string strSubject, string strBody)
         {
             MailMessage objMailMsg = new MailMessage();
             objMailMsg.IsBodyHtml = true;
             objMailMsg.From = new MailAddress(System.Configuration.ConfigurationSettings.AppSettings["MailFromField"]);

             //if more than one email addresses present in To/Cc field
             if (strTo != null && (strTo != string.Empty))
             {
                 ArrayList aryTo = new ArrayList(strTo.Split(new char[] { ';' }));
                 foreach (string s in aryTo)
                 {
                     objMailMsg.To.Add(s);
                 }
             }
             if (strCc != null && (strCc != string.Empty))
             {
                 ArrayList aryCc = new ArrayList(strCc.Split(new char[] { ';' }));
                 foreach (string s in aryCc)
                 {
                     objMailMsg.CC.Add(s);
                 }
             }



             // objMailMsg.To.Add(strTo);
             //objMailMsg.Bcc.Add("");

             // Set the subject of the mail messagel
             objMailMsg.Subject = strSubject;

             // Set the body of the mail message
             objMailMsg.Body = strBody;

             // Set the format of the mail message body as HTML
             objMailMsg.IsBodyHtml = true;

             // Set the priority of the mail message to normal
             objMailMsg.Priority = MailPriority.Normal;

             // Instantiate a new instance of SmtpClient
             SmtpClient objSmtpClient = new SmtpClient();
             // Send the mail message
             objSmtpClient.Send(objMailMsg);
         }

        public string GetHomepageText()
        {
            objclsDALCommon = new clsDALCommon();
            string strText = objclsDALCommon.GetHomepageText();
            return strText;
            
        }
        public DataSet GetSOXScope(int AppId)
        {
            objclsDALCommon = new clsDALCommon();
            DataSet ds = objclsDALCommon.GetSOXScope(AppId);
            return ds;
        }
        public DataSet GetAppScope(int AppId, string strApproverADID)
        {
            objclsDALCommon = new clsDALCommon();
            DataSet ds = objclsDALCommon.GetAppScope(AppId, strApproverADID);
            return ds;
        }
        public string GetCurrentQuarter()
        {
            objclsDALCommon = new clsDALCommon();
            string strCurrQuarter = objclsDALCommon.GetCurrentQuarter();
            return strCurrQuarter;

        }
        public string GetLatestQuarter()
        {
            objclsDALCommon = new clsDALCommon();
            string strCurrQuarter = objclsDALCommon.GetLatestQuarter();
            return strCurrQuarter;

        }

        #region to check if user exists for current quarter

        public DataSet CheckIfUserExistsForCurrentQuarter_DB(string strUserSID, string scope, int Appid, string strquarter,string strDBType)
        {
            objclsDALCommon = new clsDALCommon();
            DataSet ds = objclsDALCommon.CheckIfUserExistsForCurrentQuarter_DB(strUserSID, scope, Appid, strquarter, strDBType);
            return ds;
        }

        #endregion

        public string GetLatestQuarter_DB(string strDBType)
        {
            objclsDALCommon = new clsDALCommon();
            string strCurrQuarter = objclsDALCommon.GetLatestQuarter_DB(strDBType);
            return strCurrQuarter;

        }

        public string GetNextQuarter(string strcurrQuarter)
        {
            objclsDALCommon = new clsDALCommon();
            string strNextQuarter = objclsDALCommon.GetNextQuarter(strcurrQuarter);
            return strNextQuarter;

        }
        public void CheckIFUserExistsInCurrentQuarter(string strUserSID, string strCurrQuarter, string PreviousQuartertoSelected, int intAppId, string role)
         {
             objclsDALCommon = new clsDALCommon();
             objclsDALCommon.CheckIFUserExistsInCurrentQuarter(strUserSID, strCurrQuarter,PreviousQuartertoSelected,intAppId,role);
             
         }
        public bool CheckIfNextQuarterReportExists(string strCurrQuarter)
        {
            objclsDALCommon = new clsDALCommon();
            bool blnNextQuarterReportExists = objclsDALCommon.CheckIfNextQuarterReportExists(strCurrQuarter);
            return blnNextQuarterReportExists;
 
        }
        public void SaveSelectiveApproval(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverName, string strApproverEmail, string strUserSID, string strgroupSID, string strUserName, string strGroupNm, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope)
        {
            objclsDALCommon = new clsDALCommon();
            objclsDALCommon.SaveSelectiveApproval(strLoggedInUserName, strloggedInUserSID, strApproverSID,strApproverName,strApproverEmail, strUserSID, strgroupSID, strUserName, strGroupNm, strCurrQuarter, intAppId, IsGlobal, strScope);
            //return blnSave;
        }

        public void DeleteSelectiveApproval(string strloggedInUserSID, string strApproverSID,
            string strCurrQuarter, int intAppId, string strScope, string sReportType)
        {
            objclsDALCommon = new clsDALCommon();
            objclsDALCommon.DeleteSelectiveApproval(strloggedInUserSID, strApproverSID, strCurrQuarter, intAppId, strScope, sReportType);
        }

        public void SaveSelectiveApproval_DB(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverName, string strApproverEmail, string strUserSID, string strServerName, string strUserName, string strDataBaseName, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope,string strDBType,string strSignOffStatus, string strRole)
        {
            objclsDALCommon = new clsDALCommon();
            objclsDALCommon.SaveSelectiveApproval_DB(strLoggedInUserName, strloggedInUserSID, strApproverSID, strApproverName, strApproverEmail, strUserSID, strServerName, strUserName, strDataBaseName, strCurrQuarter, intAppId, IsGlobal, strScope, strDBType, strSignOffStatus, strRole);
            //return blnSave;
        }
        public void SaveSelectiveApproval_linux(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverName, string strApproverEmail, string strUserSID, string strServerName, string strUserName, string strDataBaseName, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope, string strDBType, string strSignOffStatus, string strRole)
        {
            objclsDALCommon = new clsDALCommon();
            objclsDALCommon.SaveSelectiveApproval_linux(strLoggedInUserName, strloggedInUserSID, strApproverSID, strApproverName, strApproverEmail, strUserSID, strServerName, strUserName, strDataBaseName, strCurrQuarter, intAppId, IsGlobal, strScope, strDBType, strSignOffStatus, strRole);
            //return blnSave;
        }
        public void UpdateSelectiveQuarter(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverName, string strApproverEmail, string strUserSID, string strgroupSID, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope, string strusernm, string strGroupNm)
        {
            objclsDALCommon = new clsDALCommon();
            objclsDALCommon.UpdateSelectiveQuarter(strLoggedInUserName, strloggedInUserSID, strApproverSID, strApproverName, strApproverEmail, strUserSID, strgroupSID, strCurrQuarter, intAppId, IsGlobal, strScope, strusernm, strGroupNm);
            //return blnSave;
        }

        public void UpdateSelectiveQuarter_DB(string strLoggedInUserName, string strloggedInUserSID, string strApproverSID, string strApproverName, string strApproverEmail,string strFetchedQuarter, string strCurrQuarter, int intAppId, bool IsGlobal, string strScope, string strDBType)
        {
            objclsDALCommon = new clsDALCommon();
            objclsDALCommon.UpdateSelectiveQuarter_DB(strLoggedInUserName, strloggedInUserSID, strApproverSID, strApproverName, strApproverEmail, strFetchedQuarter, strCurrQuarter, intAppId, IsGlobal, strScope, strDBType);
            //return blnSave;
        }

        public DataTable GetSignOffStatus()
        {
            objclsDALCommon = new clsDALCommon();
            DataTable dtstatus = objclsDALCommon.GetSignOffStatus();
            return dtstatus;
        }

        public DataTable GetAccountStatus()
        {
            objclsDALCommon = new clsDALCommon();
            DataTable dtstatus = objclsDALCommon.GetAccountStatus();
            return dtstatus;
        }

        public DataTable GetAccountStatusPSI()
        {
            objclsDALCommon = new clsDALCommon();
            DataTable dtstatus = objclsDALCommon.GetAccountStatusPSI();
            return dtstatus;
        }

        public DataTable GetCurrentManagers(string quarter)
        {
            objclsDALCommon = new clsDALCommon();
            DataTable dtstatus = objclsDALCommon.GetCurrentManagers(quarter);
            return dtstatus;
        }

        //Added by Nag
        public DataTable GetAllApprovers(string ReportType)
        {
            objclsDALCommon = new clsDALCommon();
            DataTable dtstatus = objclsDALCommon.GetAllApprovers(ReportType);
            return dtstatus;
        }

        public DataTable  GetAvailableQuarters()
        {
            objclsDALCommon = new clsDALCommon();
            DataTable dtQuarter = objclsDALCommon.GetAvailableQuarters();
            return dtQuarter;
        }
        public DataSet GetControlOwnersEmailIDS()
        {
            objclsDALCommon = new clsDALCommon();
           DataSet ds = objclsDALCommon.GetControlOwnersEmailIDs();
           return ds;
        }
        public DataSet GetAppControlOwnerEmailID(int intAppID)
        {
            objclsDALCommon = new clsDALCommon();
            DataSet ds = objclsDALCommon.GetAppControlOwnerEmailID(intAppID);
            return ds;
        }

        public DataSet GetAppControlOwnerInfo(int intAppID)
        {
            objclsDALCommon = new clsDALCommon();
            DataSet ds = objclsDALCommon.GetAppControlOwnerInfo(intAppID);
            return ds;
        }

         

        public bool SetHomepageText(string strText)
        {
            objclsDALCommon = new clsDALCommon();
            objclsDALCommon.SetHomepageText(strText);
            return true;
        }
        
        #region FetchUserDetailsFromAD
        public clsEALUser FetchUserDetailsFromAD(string name)
        {
            try
            {

               
                ResultPropertyCollection propcoll = null;
                string str_ADUserName = System.Configuration.ConfigurationSettings.AppSettings["ad_username"].ToString();
                string str_ADPassword = System.Configuration.ConfigurationSettings.AppSettings["ad_password"].ToString();
                clsEALUser objclsEALUser = new clsEALUser();

                DirectoryEntry entry = new DirectoryEntry("LDAP://" + name.Substring(0, name.IndexOf("\\")).Trim() + ".ad.viacom.com", str_ADUserName, str_ADPassword, AuthenticationTypes.None);
                string strFilter = "(samaccountname=" + (name.Substring(name.IndexOf("\\") + 1)) + ")";
                

                string allPropNames = "";
                SearchResult result = null;
                DirectorySearcher mySearcher = new DirectorySearcher(entry, strFilter);
                mySearcher.PageSize = 1000;
                result = mySearcher.FindOne();    //Finds all users present in AD.





                if (result != null)
                {
                   
                        propcoll = result.Properties;

                        foreach (string key in propcoll.PropertyNames)
                        {
                            allPropNames = allPropNames + key + "|";
                        }


                        if (allPropNames.Contains("|sn|"))
                        {
                            objclsEALUser.StrLname = propcoll["sn"][0].ToString();
                        }
                        if (allPropNames.Contains("|givenname|"))
                        {
                            objclsEALUser.StrLname = propcoll["givenname"][0].ToString();
                        }


                        if (allPropNames.Contains("|mail|"))
                        {
                            objclsEALUser.StrUserEmailID = propcoll["mail"][0].ToString();
                        }
                        if (allPropNames.Contains("|samaccountname|"))
                        {
                            objclsEALUser.StrUserADID = name;
                        }

                        if (allPropNames.Contains("|displayname|"))
                        {
                            objclsEALUser.StrUserName = propcoll["displayname"][0].ToString();
                        }
                        if (allPropNames.Contains("|objectsid|"))
                        {
                            byte[] dBytes = (byte[])propcoll["objectsid"][0];
                            objclsEALUser.StrUserSID = ConvertByteToStringSid(dBytes);

                        }

                   


                    return objclsEALUser;
                }
                else
                {
                    return null;
                }
                
               
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion



        #region FetchUserDetailsFromAD
        public clsEALUser FetchUserDetailsFromAD1(string name)
        {
            try
            {

                name = name.Replace("\\",@"\");
                ResultPropertyCollection propcoll = null;
                string str_ADUserName = System.Configuration.ConfigurationSettings.AppSettings["ad_username"].ToString();
                string str_ADPassword = System.Configuration.ConfigurationSettings.AppSettings["ad_password"].ToString();
                clsEALUser objclsEALUser = new clsEALUser();

                DirectoryEntry entry = new DirectoryEntry("LDAP://" + name.Substring(0, name.IndexOf("\\")) + ".ad.viacom.com", str_ADUserName, str_ADPassword, AuthenticationTypes.None);
                string strFilter = "(samaccountname=" + (name.Substring(name.IndexOf("\\") + 1)) + ")";


                string allPropNames = "";
                SearchResult result = null;
                DirectorySearcher mySearcher = new DirectorySearcher(entry, strFilter);
                mySearcher.PageSize = 1000;
                result = mySearcher.FindOne();    //Finds all users present in AD.





                if (result != null)
                {

                    propcoll = result.Properties;

                    foreach (string key in propcoll.PropertyNames)
                    {
                        allPropNames = allPropNames + key + "|";
                    }


                    if (allPropNames.Contains("|sn|"))
                    {
                        objclsEALUser.StrLname = propcoll["sn"][0].ToString();
                    }
                    if (allPropNames.Contains("|givenname|"))
                    {
                        objclsEALUser.StrLname = propcoll["givenname"][0].ToString();
                    }


                    if (allPropNames.Contains("|mail|"))
                    {
                        objclsEALUser.StrUserEmailID = propcoll["mail"][0].ToString();
                    }
                    if (allPropNames.Contains("|samaccountname|"))
                    {
                        objclsEALUser.StrUserADID = name;
                    }

                    if (allPropNames.Contains("|displayname|"))
                    {
                        objclsEALUser.StrUserName = propcoll["displayname"][0].ToString();
                    }
                    if (allPropNames.Contains("|objectsid|"))
                    {
                        byte[] dBytes = (byte[])propcoll["objectsid"][0];
                        objclsEALUser.StrUserSID = ConvertByteToStringSid(dBytes);

                    }




                    return objclsEALUser;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion

        #region FetchUserDetailsFromSID
        public string FetchUserDomainFromSID(string SID)
        {
            try
            {
                strDomains = ConfigurationManager.AppSettings["Domains"].ToString();
                ResultPropertyCollection propcoll = null;
                string str_ADUserName = System.Configuration.ConfigurationSettings.AppSettings["ad_username"].ToString();
                string str_ADPassword = System.Configuration.ConfigurationSettings.AppSettings["ad_password"].ToString();
                clsEALUser objclsEALUser = new clsEALUser();
                SearchResult result = null;
                string allPropNames = "";
                string[] strDomainArr = strDomains.Split(',');
                string strUserDomain = "";
                for (int i = 0; i < (strDomainArr.Length) ; i++)
                {
                    DirectoryEntry entry = new DirectoryEntry("LDAP://" + strDomainArr[i].ToString(), str_ADUserName, str_ADPassword, AuthenticationTypes.None);
                    string strFilter = "(objectsid=" + SID + ")";
                    DirectorySearcher mySearcher = new DirectorySearcher(entry, strFilter);
                    mySearcher.PageSize = 1000;
                    result = mySearcher.FindOne();    //Finds all users present in AD.
                    if (result != null)
                    {
                        strUserDomain = strDomainArr[i].ToString();
                        break;
                    }

                }


                if (result != null)
                {

                    propcoll = result.Properties;

                    foreach (string key in propcoll.PropertyNames)
                    {
                        allPropNames = allPropNames + key + "|";
                    }


                    if (allPropNames.Contains("|sn|"))
                    {
                        objclsEALUser.StrLname = propcoll["sn"][0].ToString();
                    }
                    if (allPropNames.Contains("|givenname|"))
                    {
                        objclsEALUser.StrLname = propcoll["givenname"][0].ToString();
                    }


                    if (allPropNames.Contains("|mail|"))
                    {
                        objclsEALUser.StrUserEmailID = propcoll["mail"][0].ToString();
                    }
                    if (allPropNames.Contains("|samaccountname|"))
                    {
                        objclsEALUser.StrUserADID = propcoll["samaccountname"][0].ToString();
                    }

                    if (allPropNames.Contains("|displayname|"))
                    {
                        objclsEALUser.StrUserName = propcoll["displayname"][0].ToString();
                    }
                    if (allPropNames.Contains("|objectsid|"))
                    {
                        //byte[] dBytes = (byte[])propcoll["objectsid"][0];
                        objclsEALUser.StrUserSID = SID;// ConvertByteToStringSid(dBytes);

                    }




                    return strUserDomain;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion

        #region FetchUserDetailsFromSID 
        public clsEALUser FetchUserDomainFromSIDAll(string SID)
        {
            try
            {
                strDomains = ConfigurationManager.AppSettings["Domains"].ToString();
                ResultPropertyCollection propcoll = null;
                string str_ADUserName = System.Configuration.ConfigurationSettings.AppSettings["ad_username"].ToString();
                string str_ADPassword = System.Configuration.ConfigurationSettings.AppSettings["ad_password"].ToString();
                clsEALUser objclsEALUser = new clsEALUser();
                SearchResult result = null;
                string allPropNames = "";
                string[] strDomainArr = strDomains.Split(',');
                string strUserDomain = "";
                for (int i = 0; i < (strDomainArr.Length); i++)
                {
                    DirectoryEntry entry = new DirectoryEntry("LDAP://" + strDomainArr[i].ToString(), str_ADUserName, str_ADPassword, AuthenticationTypes.None);
                    string strFilter = "(objectsid=" + SID + ")";
                    DirectorySearcher mySearcher = new DirectorySearcher(entry, strFilter);
                    mySearcher.PageSize = 1000;
                    result = mySearcher.FindOne();    //Finds all users present in AD.
                    if (result != null)
                    {
                        strUserDomain = strDomainArr[i].ToString();
                        break;
                    }

                }


                if (result != null)
                {

                    propcoll = result.Properties;

                    foreach (string key in propcoll.PropertyNames)
                    {
                        allPropNames = allPropNames + key + "|";
                    }


                    if (allPropNames.Contains("|sn|"))
                    {
                        objclsEALUser.StrLname = propcoll["sn"][0].ToString();
                    }
                    if (allPropNames.Contains("|givenname|"))
                    {
                        objclsEALUser.StrLname = propcoll["givenname"][0].ToString();
                    }


                    if (allPropNames.Contains("|mail|"))
                    {
                        objclsEALUser.StrUserEmailID = propcoll["mail"][0].ToString();
                    }
                    if (allPropNames.Contains("|samaccountname|"))
                    {
                        objclsEALUser.StrUserADID = propcoll["samaccountname"][0].ToString();
                    }

                    if (allPropNames.Contains("|displayname|"))
                    {
                        objclsEALUser.StrUserName = propcoll["displayname"][0].ToString();
                    }
                    if (allPropNames.Contains("|objectsid|"))
                    {
                        //byte[] dBytes = (byte[])propcoll["objectsid"][0];
                        objclsEALUser.StrUserSID = SID;// ConvertByteToStringSid(dBytes);

                    }

                    return objclsEALUser;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion
        


        #region SID to String Conversion
        private string ConvertByteToStringSid(Byte[] sidBytes)
        {
            short sSubAuthorityCount = 0;
            StringBuilder strSid = new StringBuilder();
            strSid.Append("S-");
            try
            {
                // Add SID revision.
                strSid.Append(sidBytes[0].ToString());

                sSubAuthorityCount = Convert.ToInt16(sidBytes[1]);

                // Next six bytes are SID authority value.
                if (sidBytes[2] != 0 || sidBytes[3] != 0)
                {
                    string strAuth = String.Format("0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}",
                                (Int16)sidBytes[2],
                                (Int16)sidBytes[3],
                                (Int16)sidBytes[4],
                                (Int16)sidBytes[5],
                                (Int16)sidBytes[6],
                                (Int16)sidBytes[7]);
                    strSid.Append("-");
                    strSid.Append(strAuth);
                }
                else
                {
                    Int64 iVal = (Int32)(sidBytes[7]) +
                            (Int32)(sidBytes[6] << 8) +
                            (Int32)(sidBytes[5] << 16) +
                            (Int32)(sidBytes[4] << 24);
                    strSid.Append("-");
                    strSid.Append(iVal.ToString());
                }

                // Get sub authority count...
                int idxAuth = 0;
                for (int i = 0; i < sSubAuthorityCount; i++)
                {
                    idxAuth = 8 + i * 4;
                    UInt32 iSubAuth = BitConverter.ToUInt32(sidBytes, idxAuth);
                    strSid.Append("-");
                    strSid.Append(iSubAuth.ToString());
                }
            }
            catch (Exception ex)
            {
               
                return "";
            }
            return strSid.ToString();
        }		

        #endregion

        #region Selective Approval message
        public string ControlOwnerShareServerMessage(string strUserName, string applicationName, string urllink)
        {
            string strMailBody;
            strMailBody = "This Selective Approval request is being forwarded to you by Control Owner <b>" + strUserName + 
                                "</b> for the " + applicationName + " Application for recertification of user who is" + 
                                " a member of Security Group that has rights to servers/shares for this Application." +
                                "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                "<br><br><font style=font-weight:bold>If a User Account in a given Group is" +
                                " “Approved or Removed”, this User Account will be “Approved or Removed” across ALL" + 
                                " Servers and Shares you are responsible for. Additionally, by approving a User with" + 
                                " Administrative Rights you are implicitly approving these Rights across ALL " + 
                                "<font style=text-decoration:underline>" + "your</font> reports.</font>";

            return strMailBody;
        }

        public string GlobalApproverShareServerMessage(string strUserName, string urllink)
        {
            string strMailBody;
            strMailBody = "This Selective Approval request is being forwarded to you by Global Approver <b>" + 
                          strUserName + "</b> for recertification of user who is not associated with a specific " + 
                          "application but is a member of a Security Group that has rights to servers/shares across" + 
                          " many applications.<br><a href='" + urllink + "'>Click here</a>.</font><br><br>" + 
                          "<font style=font-weight:bold>If a User Account in a given Group is “Approved or Removed”, " + 
                          "this User Account will be “Approved or Removed” across ALL Servers and Shares. Additionally, " + 
                          "by approving a User with Administrative Rights you are implicitly approving these Rights " + 
                          "across all reports.</font>";

            return strMailBody;
        }
        #endregion

    }
}
