using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class Person
    {
        private int personId;
        private string name;
        private string email;
        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public virtual Destination[] ViewDestinations()
        {
            throw new NotImplementedException();
        }
    }
}
