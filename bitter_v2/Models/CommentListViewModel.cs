using System;
using System.Collections.Generic;
using System.Text;

namespace bitter_v2.Models
{
    public class CommentListViewModel : CommentList
    {
        public CommentListViewModel(string tweetID) : base(tweetID)
        {

        }
    }
}
