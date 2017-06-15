using System;
using System.Collections.Generic;
using System.Net;

namespace EpicYouTubeDownloader
{
    public class VerifyLinkService
    {
        #region Private Properties

        private string _link;

        #endregion

        public bool verifyLink(string link)
        {
            _link = link;
            bool isVerified = false;

            if (link.StartsWith("https://www.youtube.com/watch?v="))
            {
                isVerified = true;
            }

            if (isVerified)
            {
                HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(link);
                request.Method = "HEAD";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                    {
                        isVerified = true;
                    }
                }
                catch (Exception exception)
                {
                    isVerified = false;
                }
            }
            return isVerified;
        }
    }
}