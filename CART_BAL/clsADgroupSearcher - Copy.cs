using System;
using System.Collections;
using System.Text;

using System.DirectoryServices; 
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
namespace CART_BAL
{
    public class oldclsADGroupMembers
    {
        /// <summary>
        /// searchedGroups will contain all groups already searched, in order to
        /// prevent endless loops when there are circular structured in the groups.
        /// </summary>
        static Hashtable searchedGroups = null;

        /// <summary>
        /// x will return all users in the group passed in as a parameter
        /// the names returned are the SAM Account Name of the users.
        /// The function will recursively search all nested groups.
        /// Remark: if there are multiple groups with the same name, this function will just
        /// use the first one it finds.
        /// </summary>
        /// <param name="strGroupName">Name of the group, which the users should be retrieved from</param>
        /// <returns>ArrayList containing the SAM Account Names of all users in this group and any nested groups</returns>
        public ArrayList x(string strGroupName)
        {
            ArrayList groupMembers = new ArrayList();
            searchedGroups = new Hashtable();

            // find group
            //DirectorySearcher search = new DirectorySearcher("LDAP://DC=company,DC=com");
            DirectorySearcher search = new DirectorySearcher("LDAP://MTVN.ad.viacom.com");
            search.Filter = String.Format("(&(objectCategory=group)(cn={0}))", strGroupName);
            search.PropertiesToLoad.Add("distinguishedName");
            SearchResult sru = null;
            DirectoryEntry group;

            try
            {
                sru = search.FindOne();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            group = sru.GetDirectoryEntry();

            groupMembers = getUsersInGroup(group.Properties["distinguishedName"].Value.ToString());

            return groupMembers;
        }

        /// <summary>
        /// getUsersInGroup will return all users in the group passed in as a parameter
        /// the names returned are the SAM Account Name of the users.
        /// The function will recursively search all nested groups.
        /// </summary>
        /// <param name="strGroupDN">DN of the group, which the users should be retrieved from</param>
        /// <returns>ArrayList containing the SAM Account Names of all users in this group and any nested groups</returns>
        public ArrayList getUsersInGroup(string strGroupDN)
        {
            ArrayList groupMembers = new ArrayList();
            //string strDomains = "playasur,mtvn,mtvne,viacom_corp,mtvnasia,paramount,ad,corp";
            //string[] strDomainArr = strDomains.Split(',');
            const string Domain = "viacom_corp.ad.viacom.com";

            ////for (int i = 0; i < (strDomainArr.Length); i++){
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Domain);

                GroupPrincipal qbeGroup = new GroupPrincipal(ctx, "LTGroup");
                PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

            //    // find all matches
            foreach (var found in srch.FindAll())
            {
                GroupPrincipal foundGroup = found as GroupPrincipal;

                if (foundGroup != null)
                {
                    //if (foundGroup.ToString().ToLower().Contains("group"))
                    //Group Name
                    groupMembers.Add(foundGroup.ToString());
                    foreach (Principal p in foundGroup.GetMembers(true))
                    {
                        //Member
                        groupMembers.Add(p.Name);
                    }

                    // do whatever you need to do, e.g. put name into a list of strings or something
                }
            }
            ////} 

            //SearchResult result;
            //DirectorySearcher search = new DirectorySearcher("LDAP://mtvn.ad.viacom.com");
            //search.Filter = String.Format("(cn={0})", "IT Group");
            //search.PropertiesToLoad.Add("member");
            //result = search.FindOne();

            //searchedGroups = new Hashtable();
            //searchedGroups.Add(strGroupDN, strGroupDN);
            //strGroupDN = "IT Group";
            //// find all users in this group
            //DirectorySearcher ds = new DirectorySearcher("LDAP://mtvn.ad.viacom.com");
            //ds.Filter = String.Format("(&(memberOf={0})(objectClass=person))", "IT Group");

            //ds.PropertiesToLoad.Add("distinguishedName");
            //ds.PropertiesToLoad.Add("givenname");
            //ds.PropertiesToLoad.Add("samaccountname");
            //ds.PropertiesToLoad.Add("sn");

            //foreach (SearchResult sr in ds.FindAll())
            //{
            //    groupMembers.Add(sr.Properties["samaccountname"][0].ToString());
            //}

            //// get nested groups
            //ArrayList al = getNestedGroups(strGroupDN);
            //foreach (object g in al)
            //{
            //    if (!searchedGroups.ContainsKey(g)) // only if we haven't searched this group before - avoid endless loops
            //    {
            //        // get members in nested group
            //        ArrayList ml = getUsersInGroup(g as string);
            //        // add them to result list
            //        foreach (object s in ml)
            //        {
            //            groupMembers.Add(s as string);
            //        }
            //    }
            //}


            ArrayList al = getNestedGroups(strGroupDN);
            return groupMembers;
        }

        /// <summary>
        /// getNestedGroups will return an array with the DNs of all groups contained
        /// in the group that was passed in as a parameter
        /// </summary>
        /// <param name="strGroupDN">DN of the group, which the nested groups should be retrieved from</param>
        /// <returns>ArrayList containing the DNs of each group contained in the group apssed in asa parameter</returns>
        public ArrayList getNestedGroups(string strGroupDN)
        {
            ArrayList groupMembers = new ArrayList();
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
            {
                Principal prototype = new GroupPrincipal(ctx);
                PrincipalSearcher searcher = new PrincipalSearcher(prototype);
                
                PropertyValueCollection email;

                foreach (var gp in searcher.FindAll()) using (gp)
                    {
                        GroupPrincipal group = gp as GroupPrincipal;

                        using (DirectoryEntry groupEntry = ((DirectoryEntry)group.GetUnderlyingObject()))
                        {
                            email = groupEntry.Properties["mail"];
                            //if (email.Value != null)
                            //{
                                groupMembers.Add(group.Name);
                            //}
                        }
                    }
            }

            //// find all nested groups in this group
            ////string strDomains = "playasur,mtvn,mtvne,viacom_corp,mtvnasia,paramount,ad,corp";
            //ds.Filter = String.Format("(&(memberOf={0})(objectClass=group))", "CITRIX-CF-OFFSHORE REMOTE DESKTOP");

            //ds.PropertiesToLoad.Add("distinguishedName");

            //foreach (SearchResult sr in ds.FindAll())
            //{
            //    groupMembers.Add(sr.Properties["distinguishedName"][0].ToString());
            //}

            return groupMembers;
        }

    }
}
