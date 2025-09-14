using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class Rating
    {
        private int ratingId;
        private int userId;
        private int destinationId;
        private int score;
        public int RatingId
        {
            get { return ratingId; }
            set { ratingId = value; }
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
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public void GiveRating()
        {
            throw new NotImplementedException();
        }
    }
}
