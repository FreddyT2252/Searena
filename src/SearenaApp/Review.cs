using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class Review
    {
        private int reviewId;
        private int userId;
        private int destinationId;
        private string comment;
        public int ReviewId
        {
            get { return reviewId; }
            set { reviewId = value; }
        }
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public int DestinationId
        {
            get { return destinationId; }
            set { destinationId = value; }
        }
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        public void AddReview()
        {
            throw new NotImplementedException();
        }
        public void EditReview()
        {
            throw new NotImplementedException();
        }
    }
}