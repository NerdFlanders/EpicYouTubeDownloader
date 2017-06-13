using Caliburn.Micro;

namespace EpicYouTubeDownloader.ViewModels.Download
{
    internal class DownloadViewModel : Screen
    {
        public DownloadControlViewModel DownloadControlViewModel { get; set; }

        public DownloadListViewModel DownloadListViewModel { get; set; }


        public DownloadViewModel(DownloadControlViewModel downloadControlViewModel,
                                DownloadListViewModel downloadListViewModel)
        {
            DownloadControlViewModel = downloadControlViewModel;
            DownloadListViewModel = downloadListViewModel;
        }
    }
}