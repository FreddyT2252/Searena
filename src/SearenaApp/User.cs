using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class User : Person
    {
        private int userId;
        private string username;
        private string password;
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }
        public bool Register(string username, string password, string email)
        {
            throw new NotImplementedException();
        }
        public void Logout()
        {
            throw new NotImplementedException();
        }
        public void AddReview(int destinationId, string comment)
        {
            throw new NotImplementedException();
        }
        public void EditReview(int reviewId, string newComment)
        {
            throw new NotImplementedException();
        }
        public void GiveRating(int destinationId, int score)
        {
            throw new NotImplementedException();
        }
        public void AddBookmark(int destinationId)
        {
            throw new NotImplementedException();
        }
        public void RemoveBookmark(int destinationId)
        {
            throw new NotImplementedException();
        }
        public Weather ViewWeather(string location)
        {
            throw new NotImplementedException();
        }
    }
}
