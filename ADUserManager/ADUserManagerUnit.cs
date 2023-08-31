using System.Collections.Generic;
using System.DirectoryServices;
using System.Windows;
using System.Xml;

namespace ADUserManager
{
    internal class ADUserManagerUnit
    {
        private string[] _propertiesToLoad;
        private string _prefix;
        
        private string _directoryEntryPath;
        
        private List<ADUser> _users;

        public ADUserManagerUnit() 
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
        public List<ADUser> Users() 
        {
            return _users;
        }
        public void SetPrefix(string prefix) 
        {
            _prefix = prefix;
            SetDirectoryEntryPath();
        }
        public string GetPrefix() 
        {
            return _prefix;
        }
        public void SetDirectoryEntryPath() 
        {
            var de = new DirectoryEntry("LDAP://RootDSE");
            _directoryEntryPath = "LDAP://" + _prefix + de.Properties["defaultNamingContext"][0].ToString();
        }
        public string GetDirectoryEntryPath() 
        {
            return _directoryEntryPath;
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
        public void LoadConfiguration(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode prefixNode = doc.SelectSingleNode("/Configuration/Prefix");
            if (prefixNode != null)
            {
                string prefix = prefixNode.InnerText;
                SetPrefix(prefix);
            }

            XmlNodeList propertiesNodes = doc.SelectNodes("/Configuration/PropertiesToLoad/Property");
            if (propertiesNodes != null)
            {
                List<string> propertiesToLoad = new List<string>();
                foreach (XmlNode node in propertiesNodes)
                {
                    string property = node.InnerText;
                    propertiesToLoad.Add(property);
                }
                PropertiesToLoad = propertiesToLoad.ToArray();
            }
        }
        public void SearchUsers(string query) 
        {
            /***/  
            var de = new DirectoryEntry(_directoryEntryPath);
            var ds = new DirectorySearcher(de);
            ds.Filter = FormSearchFilter(query);
            ds.PropertiesToLoad.AddRange(_propertiesToLoad);
            SearchResultCollection results;
            try
            {
                results = ds.FindAll();
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
            }
            catch 
            {
                MessageBox.Show("Ошибка. Вероятнее всего нужно исправить префикс в config.xml");
            }
            

            /***/

        }
    }
}
