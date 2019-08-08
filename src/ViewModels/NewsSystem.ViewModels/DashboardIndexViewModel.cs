using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.ViewModels
{
    public class DashboardIndexViewModel
    {
        public int AllPosts { get; set; }

        public int PublishedPosts { get; set; }

        public string PublishedPostsAsPercentege
        {
            get
            {
                if (PublishedPosts==0)
                {
                    return "0";
                }
                return (PublishedPosts / AllPosts * 100).ToString("F1");
            }
        }

        public int NotPublishedPosts { get; set; }
        public string NotPublishedPostsAsPercentege
        {
            get
            {
                if (NotPublishedPosts==0)
                {
                    return "0";
                }

                return (NotPublishedPosts / AllPosts * 100).ToString("F1");
            }
        }


        public int DeletedPosts { get; set; }

        public string DeletedPostsAsPercentege
        {
            get
            {
                if (DeletedPosts==0)
                {
                    return "0";
                }
                return (DeletedPosts / AllPosts * 100).ToString("F1");
            }
        }
    }
}
