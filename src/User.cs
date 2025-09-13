using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class User : Person
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

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
