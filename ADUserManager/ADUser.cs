using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserManager
{
    internal class ADUser
    {
        private Dictionary<string, string> _userFields;

        public ADUser() 
        {
            _userFields = new Dictionary<string, string>();
        }
        public void SetField(string fieldName, string fieldValue)
        {
            _userFields[fieldName] = fieldValue;
        }
        public string GetField(string fieldName)
        {
            if (_userFields.ContainsKey(fieldName))
            {
                return _userFields[fieldName];
            }
            else
            {
                return null;
            }
        }
    }
}
