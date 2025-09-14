using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class Admin : Person
    {
        private int adminId;
        private string username;
        public int AdminId
        {
            get { return adminId; }
            set { adminId = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public void AddDestination(string name, string location, string description, string image)
        {
            throw new NotImplementedException();
        }
        public void UpdateDestination(int destinationId)
        {
            throw new NotImplementedException();
        }
        public void DeleteDestination(int destinationId)
        {
            throw new NotImplementedException();
        }
        public void DeleteReview(int reviewId)
        {
            throw new NotImplementedException();
        }
        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }
        public void UpdateWeather(string location, float temperature, string condition)
        {
            throw new NotImplementedException();
        }
    }
}
