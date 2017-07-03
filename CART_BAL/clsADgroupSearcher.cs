using System;
using System.Collections;
using System.Text;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CART_BAL
{
    public class clsADGroupMembers
    {
        private static List<GroupObj> _groups = new List<GroupObj>();
        private clsBALUsers objclsBALUsers;

        public List<string> GetMembershipWithPath(string groupname= "slink_developers")
        {
            //try
            //{
            List<string> retVal = new List<string>();
            string domainName = "VIACOM_CORP";
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName + ".ad.viacom.com");
            GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, groupname);
            //GroupPrincipal qbeGroup = new GroupPrincipal(ctx, "NOCLIVESQL01-eLog-TridentStaging-Users");

            // create your principal searcher passing in the QBE principal    
            //PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

            // find all matches
            //foreach (var found in srch.FindAll())
            //{
            //    GroupPrincipal grp = found as GroupPrincipal; 

                if (grp != null)
                {
                    BuildHList(grp, 0, null);
                    try
                    {
                        foreach (UserPrincipal usr in grp.GetMembers(true))
                            retVal.Add(GetMbrPath(usr));
                    }
                    catch{}
                    
                }

                foreach(var item in retVal) {
                    string childitems = item;
                    objclsBALUsers = new clsBALUsers();
                    objclsBALUsers.ADUsersAdd(domainName + "\\" + groupname, childitems);

                }
            //}
            return retVal;
            //}
            //catch (Exception ex)
            //{

            //    throw new Exception(ex.ToString());
            //}           

        }

        private void BuildHList(GroupPrincipal node, int level, GroupPrincipal parent)
        {
            try
            {

                PrincipalSearchResult<Principal> rslts = node.GetMembers();
                _groups.Add(new GroupObj() { Group = node, Level = level, Parent = parent });
                try
                {
                    foreach (GroupPrincipal grp in rslts.Where(g => g is GroupPrincipal))
                    BuildHList(grp, level + 1, node);
                }
                catch { }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }

        private string GetMbrPath(UserPrincipal usr)
        {
            try
            {

                Stack<string> output = new Stack<string>();
                StringBuilder retVal = new StringBuilder();
                GroupObj fg = null, tg = null;
                output.Push(usr.Name);
                foreach (GroupObj go in _groups)
                {
                    if (usr.IsMemberOf(go.Group))
                    {
                        output.Push(go.Group.Name);
                        fg = go;
                        while (fg.Parent != null)
                        {
                            output.Push(fg.Parent.Name);
                            tg = (from g in _groups where g.Group == fg.Parent select g).FirstOrDefault();
                            fg = tg;
                        }
                        break;
                    }
                }
                while (output.Count > 1)
                    retVal.AppendFormat("{0} ->", output.Pop());

                //if (usr.Name == "Prasad, Abhishek")
                //{
                    retVal.Append(output.Pop());
                //}
                return retVal.ToString();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
           
        }
        /////// <summary>
        /////// searchedGroups will contain all groups already searched, in order to
        /////// prevent endless loops when there are circular structured in the groups.
        /////// </summary>
        ////static Hashtable searchedGroups = null;
        /////// <summary>
        /////// x will return all users in the group passed in as a parameter
        /////// the names returned are the SAM Account Name of the users.
        /////// The function will recursively search all nested groups.
        /////// Remark: if there are multiple groups with the same name, this function will just
        /////// use the first one it finds.
        /////// </summary>
        /////// <param name="strGroupName">Name of the group, which the users should be retrieved from</param>
        /////// <returns>ArrayList containing the SAM Account Names of all users in this group and any nested groups</returns>
        ////public ArrayList x(string strGroupName)
        ////{
        ////    string var_domains = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["validdomains"]);
        ////    string str_ADUserName = System.Configuration.ConfigurationSettings.AppSettings["ad_username"].ToString();
        ////    string str_ADPassword = System.Configuration.ConfigurationSettings.AppSettings["ad_password"].ToString();

        ////    //DirectoryEntry enTry = new DirectoryEntry("LDAP://" + domain.Trim() + ".ad.viacom.com", str_ADUserName, str_ADPassword, AuthenticationTypes.None);
        ////    DirectoryEntry enTry = new DirectoryEntry("GC://" + var_domains.Trim(), str_ADUserName, str_ADPassword);

        ////    DirectorySearcher search = new DirectorySearcher(enTry, String.Format("(&(objectCategory=group)(cn={0}))", strGroupName));

        ////    //xx
        ////    ArrayList groupMembers = new ArrayList();
        ////    searchedGroups = new Hashtable();

        ////    // find group
        ////    //DirectorySearcher search = new DirectorySearcher("LDAP://DC=company,DC=com");
        ////    //search.Filter = String.Format("(&(objectCategory=group)(cn={0}))", strGroupName);


        ////    search.PropertiesToLoad.Add("distinguishedName");
        ////    SearchResult sru = null;
        ////    DirectoryEntry group;

        ////    try
        ////    {
        ////        sru = search.FindOne();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    group = sru.GetDirectoryEntry();

        ////    groupMembers = getUsersInGroup(group.Properties["distinguishedName"].Value.ToString());

        ////    return groupMembers;
        ////}

        /////// <summary>
        /////// getUsersInGroup will return all users in the group passed in as a parameter
        /////// the names returned are the SAM Account Name of the users.
        /////// The function will recursively search all nested groups.
        /////// </summary>
        /////// <param name="strGroupDN">DN of the group, which the users should be retrieved from</param>
        /////// <returns>ArrayList containing the SAM Account Names of all users in this group and any nested groups</returns>
        ////private static ArrayList getUsersInGroup(string strGroupDN)
        ////{
        ////    ArrayList groupMembers = new ArrayList();
        ////    //string strDomains = "playasur,mtvn,mtvne,viacom_corp,mtvnasia,paramount,ad,corp";
        ////    //string[] strDomainArr = strDomains.Split(',');
        ////    const string Domain = "viacom_corp.ad.viacom.com";

        ////    ////for (int i = 0; i < (strDomainArr.Length); i++){
        ////    PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Domain);

        ////    GroupPrincipal qbeGroup = new GroupPrincipal(ctx, "LTGroup");
        ////    PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

        ////    //    // find all matches
        ////    foreach (var found in srch.FindAll())
        ////    {
        ////        GroupPrincipal foundGroup = found as GroupPrincipal;

        ////        if (foundGroup != null)
        ////        {
        ////            //if (foundGroup.ToString().ToLower().Contains("group"))
        ////            //Group Name
        ////            groupMembers.Add(foundGroup.ToString());
        ////            foreach (Principal p in foundGroup.GetMembers(recursive: true))
        ////            {
        ////                //Member
        ////                groupMembers.Add(p.Name);
        ////            }

        ////            //which group
        ////            //foreach (Principal userGroup in user.GetGroups(ctx))
        ////            //{
        ////            //    Console.WriteLine("   - Member of Group: {0}", userGroup.Name);
        ////            //}

        ////            // do whatever you need to do, e.g. put name into a list of strings or something
        ////        }
        ////    }



        ////    //const string DomainName = "viacom_corp.ad.viacom.com"; 
        ////    //PrincipalContext ctxNew = new PrincipalContext(ContextType.Domain, DomainName);
        ////    //GroupPrincipal findAllGroups = new GroupPrincipal(ctxNew, "LTGroup");
        ////    const string DomainName = "mtvn.ad.viacom.com";
        ////    PrincipalContext ctxNew = new PrincipalContext(ContextType.Domain, DomainName);
        ////    GroupPrincipal findAllGroups = new GroupPrincipal(ctxNew, "NOCLIVESQL01-eLog-TridentStaging-Users");
        ////    PrincipalSearcher ps = new PrincipalSearcher(findAllGroups);
        ////    foreach (var group in ps.FindAll())
        ////    {
        ////        groupMembers.Add(group.DistinguishedName);
        ////    }

        ////    //// get nested groups
        ////    //ArrayList al = getNestedGroups(strGroupDN);
        ////    //foreach (object g in al)
        ////    //{
        ////    //    if (!searchedGroups.ContainsKey(g)) // only if we haven't searched this group before - avoid endless loops
        ////    //    {
        ////    //        // get members in nested group
        ////    //        ArrayList ml = getUsersInGroup(g as string);
        ////    //        // add them to result list
        ////    //        foreach (object s in ml)
        ////    //        {
        ////    //            groupMembers.Add(s as string);
        ////    //        }
        ////    //    }
        ////    //}

        ////    return groupMembers;
        ////}

        /////// <summary>
        /////// getNestedGroups will return an array with the DNs of all groups contained
        /////// in the group that was passed in as a parameter
        /////// </summary>
        /////// <param name="strGroupDN">DN of the group, which the nested groups should be retrieved from</param>
        /////// <returns>ArrayList containing the DNs of each group contained in the group apssed in asa parameter</returns>
        ////private static ArrayList getNestedGroups(string strGroupDN)
        ////{
        ////    ArrayList groupMembers = new ArrayList();
        ////    try
        ////    {
        ////        // find all nested groups in this group
        ////        //DirectorySearcher ds = new DirectorySearcher("LDAP://DC=company,DC=com");
        ////        //ds.Filter = String.Format("(&(memberOf={0})(objectClass=group))", strGroupDN);

        ////        string var_domains = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["validdomains"]);
        ////        string str_ADUserName = System.Configuration.ConfigurationSettings.AppSettings["ad_username"].ToString();
        ////        string str_ADPassword = System.Configuration.ConfigurationSettings.AppSettings["ad_password"].ToString();


        ////        DirectoryEntry enTry = new DirectoryEntry("LDAP://DC=viacom_corp,DC=viacom,DC=com", str_ADUserName, str_ADPassword);
        ////        DirectorySearcher ds = new DirectorySearcher(enTry, String.Format("(&(memberOf={0})(objectClass=group))", strGroupDN));


        ////        ds.PageSize = 8;
        ////        ds.SizeLimit = 8000;



        ////        ds.PropertiesToLoad.Add("distinguishedName");

        ////        foreach (SearchResult sr in ds.FindAll())
        ////        {
        ////            groupMembers.Add(sr.Properties["distinguishedName"][0].ToString());
        ////        }

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        throw new Exception(ex.ToString());
        ////    }


        ////    return groupMembers;
        ////}
    }


    public class GroupObj
    {
        public GroupPrincipal Group { get; set; }
        public int Level { get; set; }
        public GroupPrincipal Parent { get; set; }
    }
}
