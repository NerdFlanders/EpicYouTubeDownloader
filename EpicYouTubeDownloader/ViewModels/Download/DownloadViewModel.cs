using Caliburn.Micro;

namespace EpicYouTubeDownloader.ViewModels.Download
{
    internal class DownloadViewModel : Screen
    {
        public DownloadControlViewModel DownloadControlViewModel { get; set; }

        public DownloadViewModel(DownloadControlViewModel downloadControlViewModel)
        {
            //DownloadControlViewModel = downloadControlViewModel;
        }
    }
}