using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace imageboard
{
    public class Post
    {
        public long no;
        public long tim;
        public string ext;
    }
    public class Thread
    {
        public List<Post> Posts;
    }
    public class ImageboardController
    {
        public static Thread GetThread(string board, int postNumber)
        {
            string threadURL = "https://a.4cdn.org/" + board + "/thread/" + postNumber + ".json";
            var webclient = new WebClient();
            string response = webclient.DownloadString(threadURL);
            Thread threadObject = JsonConvert.DeserializeObject<Thread>(response);
            return threadObject;
        }
    }
}
