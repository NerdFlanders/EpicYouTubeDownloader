using System.Collections.Generic;

namespace EpicYouTubeDownloader.Models.Domain
{
    public class ReturnError
    {
        public List<string> errorLinks { get; set; }

        public int errorNumber { get; set; }

    }
}