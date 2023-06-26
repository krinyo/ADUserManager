using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserManager
{
    internal class ADUserManager
    {
        private string[] _propertiesToLoad;
        private string _prefix;
        
        private string _directoryEntryPath;
        
        private List<ADUser> _users;

        public ADUserManager() 
        { 
            _users = new List<ADUser>();
        }
        public string[] PropertiesToLoad 
        {
            get { return _propertiesToLoad; }
            set 
            {
                _propertiesToLoad = value;
            }
        }
        public void SetPrefix(string prefix) 
        {
            _prefix = prefix;
            SetDirectoryEntryPath();
        }
        public void SetDirectoryEntryPath() 
        {
            var de = new DirectoryEntry("LDAP://RootDSE");
            _directoryEntryPath = "LDAP://" + _prefix + de.Properties["defaultNamingContext"][0].ToString();
        }
        private string FormSearchFilter(string crit)
        {
            string result = "(| ";
            foreach (var prop in _propertiesToLoad)
            {
                if (prop != "distinguishedName")
                    result += string.Format("({0}={1}*)", prop, crit);
            }
            return result += ")";
        }
        public void Search_users(string query) 
        {   
            /***/
            var de = new DirectoryEntry(_directoryEntryPath);
            var ds = new DirectorySearcher(de);
            ds.Filter = FormSearchFilter(query);
            ds.PropertiesToLoad.AddRange(_propertiesToLoad);
            SearchResultCollection results = ds.FindAll();
            _users.Clear();
            foreach (SearchResult result in results) 
            {
                ADUser user = new ADUser();
                foreach (var prop in _propertiesToLoad)
                {
                    if (result.Properties.Contains(prop))
                        user.SetField(prop, result.Properties[prop][0].ToString());
                }
                _users.Add(user);
            }
            /***/

        }
    }
}
