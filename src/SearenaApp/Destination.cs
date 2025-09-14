using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class Destination
    {
        private int destinationId;
        private string name;
        private string location;
        private string description;
        private string image;
        public int DestinationId
        {
            get { return destinationId; }
            set { destinationId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Image
        {
            get { return image; }
            set { image = value; }
        }
        public void AddDestination()
        {
            throw new NotImplementedException();
        }
        public void UpdateDestination()
        {
            throw new NotImplementedException();
        }
        public void DeleteDestination()
        {
            throw new NotImplementedException();
        }
        public Destination ViewDestination()
        {
            throw new NotImplementedException();
        }
    }
}
