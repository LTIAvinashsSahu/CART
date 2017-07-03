using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.DirectoryServices;
using System.Collections;

namespace PPLPicker
{
    public partial class FetchData : System.Web.UI.Page
    {
        public DataRow[] sortrow;
        public DataSet dsEmployee = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            SearchADUpdated();
            if (dsEmployee != null)
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8);

                // Write the XML document header.
                xmlWriter.WriteStartDocument();

                // Write our first XML header.
                xmlWriter.WriteStartElement("rows");
                xmlWriter.WriteAttributeString("total_count", dsEmployee.Tables[0].Rows.Count.ToString());

                string validdir = "asc" + ",desc";
                string strDirection = Convert.ToString(Request.QueryString["direction"]);
                string strColumn = Convert.ToString(Request.QueryString["orderBy"]);

                if (strDirection == null)
                {
                    strDirection = "asc";
                }

                if (strColumn == null)
                {
                    strColumn = "0";
                }
               
                if ((validdir.ToLower().Contains(strDirection.ToLower())) && strColumn.Equals("0"))
                {
                    sortrow = dsEmployee.Tables[0].Select(null, "givenname " + strDirection);
                }
                else if ((validdir.ToLower().Contains(strDirection.ToLower())) && strColumn.Equals("2"))
                {
                    sortrow = dsEmployee.Tables[0].Select(null, "sn " + strDirection);
                }
                else
                {
                    sortrow = dsEmployee.Tables[0].Select(null, "domain asc");
                }
                foreach (DataRow row in sortrow)
                {
                    // Write an element representing a single web application object.
                    xmlWriter.WriteStartElement("row");
                    xmlWriter.WriteAttributeString("id", row["ADID"].ToString());

                    // Write child element data for our web application object.
                    xmlWriter.WriteElementString("cell", row["givenname"].ToString());
                    xmlWriter.WriteElementString("cell", row["Initials"].ToString());
                    xmlWriter.WriteElementString("cell", row["sn"].ToString());
                    xmlWriter.WriteElementString("cell", row["mail"].ToString());
                    xmlWriter.WriteElementString("cell", row["title"].ToString());
                    xmlWriter.WriteElementString("cell", row["department"].ToString());
                    xmlWriter.WriteElementString("cell", row["telephonenumber"].ToString());
                    xmlWriter.WriteElementString("cell", row["DisplayName"].ToString());
                    xmlWriter.WriteElementString("cell", row["ADID"].ToString());
                    xmlWriter.WriteElementString("cell", row["employeeNumber"].ToString());
                    // End the element WebApplication
                    xmlWriter.WriteEndElement();
                }
                // End the document WebApplications
                xmlWriter.WriteEndElement();

                // Finilize the XML document by writing any required closing tag.
                xmlWriter.WriteEndDocument();

                // To be safe, flush the document to the memory stream.
                xmlWriter.Flush();
                xmlWriter.Close();
                // conDb.Close();
            }
        }


        public void getADproperty(ref DataSet dsEmployee, DirectoryEntry objDirectoryEntry, string domain, ref int count)
        {
            string strPropertyName;
            DataRow drEmployee = dsEmployee.Tables[0].NewRow();
            foreach (DataColumn dcEmployee in dsEmployee.Tables[0].Columns)
            {
                strPropertyName = dcEmployee.ColumnName;
                if (objDirectoryEntry.Properties.Contains(strPropertyName))
                {
                    drEmployee[strPropertyName] = objDirectoryEntry.Properties[strPropertyName].Value.ToString();
                }
            }

            if (objDirectoryEntry.Properties["distinguishedname"].Value == null)
                domain = "";
            else
            {

                domain = objDirectoryEntry.Properties["distinguishedname"].Value.ToString();
                string[] domains = domain.Split(',');

                for (int i = 0; i < domains.Count(); i++)
                {
                    if (domains[i] != null && domains[i].Contains("DC="))
                    {
                        domain = (domains[i].Split('='))[1];
                        break;
                    }
                    else
                    {
                        domain = "";
                    }
                }
                drEmployee["domain"] = domain;
            }

            drEmployee["ADID"] = domain + "\\" + drEmployee["samaccountname"];

            dsEmployee.Tables["dtEmployee"].Rows.Add(drEmployee);
            count++;
        }


        public void SearchADUpdated()
        {
            int i;
            string var_domains = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["validdomains"]);
            ArrayList domains = new ArrayList(var_domains.Split(new char[] { ',' }));
            string domain;

            int count;
            count = 1;

            DataTable dtEmployee = new DataTable("dtEmployee");
            dtEmployee.Columns.Add("employeeNumber", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("displayName", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("GivenName", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("sn", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("initials", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("Mail", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("sAMAccountName", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("domain", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("employeeType", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("department", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("telephoneNumber", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("title", System.Type.GetType("System.String"));
            dtEmployee.Columns.Add("ADID", System.Type.GetType("System.String"));


            dsEmployee.Tables.Add(dtEmployee);

            string strFilter = "";
            string strFname = "";
            string strLname = "";

            strFname = Convert.ToString(Request.QueryString["ppl_Fname"]);
            strLname = Convert.ToString(Request.QueryString["ppl_Lname"]);

            if (strFname != null || strLname != null)
            {
                if (strFname.Length > 0 || strLname.Length > 0)
                {

                    if (strFname != null && strFname.Length > 0)
                    {
                        strFilter = "(&(givenname=" + strFname + "*)(ObjectCategory=User)(ObjectClass=Person)(samaccountname=*))";
                    }

                    else if (strLname != null && strLname.Length > 0)
                    {
                        strFilter = "(&(sn=" + strLname + "*)(ObjectCategory=User)(ObjectClass=Person)(samaccountname=*))";
                    }

                    if (strFname != null && strLname != null && strFname.Length > 0 && strLname.Length > 0)
                    {
                        strFilter = "(&(sn=" + strLname + "*)(givenname=" + strFname + "*)(ObjectCategory=User)(ObjectClass=Person)(samaccountname=*))";
                    }


                    //for (i = 0; i < domains.Count; i++)
                    //{
                        //domain = domains[i].ToString();
                        string str_ADUserName = System.Configuration.ConfigurationSettings.AppSettings["ad_username"].ToString();
                        string str_ADPassword = System.Configuration.ConfigurationSettings.AppSettings["ad_password"].ToString();

                        //DirectoryEntry enTry = new DirectoryEntry("LDAP://" + domain.Trim() + ".ad.viacom.com", str_ADUserName, str_ADPassword, AuthenticationTypes.None);
                        DirectoryEntry enTry = new DirectoryEntry("GC://" + var_domains.Trim(), str_ADUserName, str_ADPassword);  

                        DirectorySearcher mySearcher = new DirectorySearcher(enTry, strFilter);

                        mySearcher.PropertyNamesOnly = true;
                        mySearcher.PageSize = 8;
                        mySearcher.SizeLimit = 8000;

                        foreach (System.DirectoryServices.SearchResult resEnt in mySearcher.FindAll())
                            getADproperty(ref dsEmployee, resEnt.GetDirectoryEntry(), var_domains, ref count);

                        mySearcher = null;
                    //}
                }
            }
        }

    }
        
                    
        
    }