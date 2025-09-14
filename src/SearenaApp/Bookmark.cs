using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class Bookmark
    {
        private int bookmarkId;
        private int userId;
        private int destinationId;
        public int BookmarkId
        {
            get { return bookmarkId; }
            set { bookmarkId = value; }
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
        public void AddBookmark()
        {
            throw new NotImplementedException();
        }
        public void RemoveBookmark()
        {
            throw new NotImplementedException();
        }
    }
}

