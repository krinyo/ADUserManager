using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserManager
{
    internal class ADUserManager
    {
        private List<ADUser> users;

        public ADUserManager() 
        { 
            users = new List<ADUser>();
        }
        public void SearchUsers(string pattern) 
        {
            ADUser user = new ADUser();
            users.Add(user);
        }
    }
}
