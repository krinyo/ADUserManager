using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ADUserManager
{
    internal class ADComputerManagerUnit
    {
        Dictionary<string, string> _computers;
        private string _prefix = "OU=Workstations,";
        private string _directoryEntryPath;
        private string[] _propertiesToLoad = { "memberof" };
        private List<string> _sortedComputers;
        public void SetDirectoryEntryPath()
        {
            var de = new DirectoryEntry("LDAP://RootDSE");
            _directoryEntryPath = "LDAP://" + _prefix + de.Properties["defaultNamingContext"][0].ToString();
        }
        public ADComputerManagerUnit() 
        {
            _computers = new Dictionary<string, string>();
            _sortedComputers = new List<string>();
            SetDirectoryEntryPath();
            SearchComputers();
        }
        public Dictionary<string, string> GetComputers() 
        {
            return _computers;
        }
        public void SearchComputers()
        {
            /***/
            var de = new DirectoryEntry(_directoryEntryPath);
            var ds = new DirectorySearcher(de);
            ds.Filter = "(CN=*)";
            //ds.PropertiesToLoad.AddRange(_propertiesToLoad);
            SearchResultCollection results;
            try
            {
                results = ds.FindAll();
                foreach (SearchResult result in results)
                {

                    foreach (var prop in _propertiesToLoad)
                    {
                        string allMembers = "";
                        foreach (var el in result.Properties[prop])
                            allMembers += el.ToString() + "\n";
                        if (result.Properties.Contains(prop))
                        {
                            _computers.Add(result.Properties["name"][0].ToString(), allMembers);
                            _sortedComputers.Add(result.Properties["name"][0].ToString());
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка. Вероятнее всего нужно исправить префикс в config.xml");
            }

            _sortedComputers.Sort();
            /***/

        }
    }
}
