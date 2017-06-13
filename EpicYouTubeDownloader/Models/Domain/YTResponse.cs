namespace EpicYouTubeDownloader.Models.Domain
{
    public class YTResponse
    {
        public string nextPageToken { get; set; }
        public YTResponseItem[] items { get; set; }
    }
}