using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public abstract class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual Destination[] ViewDestinations()
        {
            throw new NotImplementedException();
        }
    }
}
