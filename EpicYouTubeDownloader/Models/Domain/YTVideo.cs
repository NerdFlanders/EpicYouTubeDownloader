using System.Windows.Media.Imaging;

namespace EpicYouTubeDownloader.Models.Domain
{
    public class YTVideo
    {
        public string VideoName { get; set; }
        public string Length { get; set; }
        public BitmapImage Thumbnail { get; set; }
    }
}
