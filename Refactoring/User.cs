using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    [Serializable]
    public class User
    {
        [JsonProperty("Username")]
        public string Name;
        [JsonProperty("Password")]
        public string Pwd;
        [JsonProperty("Balance")]
        public double Bal;

        public bool IsUser(string name, string pwd)
        {
            return this.Name == name && this.Pwd == pwd;
        }
    }

    public class Users : List<User>
    {
        private Users(){} // Reserved constructor

        public Users(List<User> users) 
        {
            this.Clear();
            this.AddRange(users);
        }

        public User FindUserByName(string name)
        {

            for (int i = 0; i < this.Count; i++)
            {
                User usr = this[i];

                // Check that name and password match
                if (usr.Name == name)
                {
                    return usr;

                }
            }
            return null;
        }

        public User FindUser(string name, string pwd)
        {

            for (int i = 0; i < this.Count; i++)
            {
                User usr = this[i];

                // Check that name and password match
                if (usr.IsUser(name, pwd))
                {
                    return usr;

                }
            }
            return null;
        }
    }
}
