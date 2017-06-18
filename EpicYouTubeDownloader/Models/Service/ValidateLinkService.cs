using System;
using System.Collections.Generic;
using System.Net;

namespace EpicYouTubeDownloader
{
    public class ValidateLinkService
    {
        #region Private Properties

        private string _link;

        #endregion

        #region Public Properties

        public bool Validated;

        #endregion
        
        #region Public Methods

        public void validateLink(string link)
        {
            _link = link;
            bool isValidate = false;

            if (link.StartsWith("https://www.youtube.com/watch?v="))
            {
                isValidate = true;
            }

            if (isValidate)
            {
                HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(link);
                request.Method = "HEAD";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                    {
                        isValidate = true;
                    }
                }
                catch (Exception exception)
                {
                    isValidate = false;
                }
            }
            Validated = isValidate;
        }

        #endregion
    }
}