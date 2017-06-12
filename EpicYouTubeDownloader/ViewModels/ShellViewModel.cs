using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using EpicYouTubeDownloader.ViewModels.Download;

namespace EpicYouTubeDownloader.ViewModels
{
    internal class ShellViewModel : Conductor<object>
    {
        public DownloadViewModel DownloadViewModel { get; set; }

        //public OptionsViewModel CustomerViewModel { get; set; }

        private const int ActiveItemMargin = -2;
        private const int InactiveItemMargin = 0;

        public ShellViewModel(DownloadViewModel downloadViewModel)
        {
            DownloadViewModel = downloadViewModel;

            DisplayName = "EpicYouTubeDownloader";
        }

        public void OnTabSelectionChanged(TabControl sender)
        {
            foreach (TabItem item in sender.Items)
            {
                if (item.Equals(sender.SelectedItem))
                {
                    ChangeTabMargin(item, ActiveItemMargin);
                }
                else
                {
                    ChangeTabMargin(item, InactiveItemMargin);
                }
            }
        }

        private static void ChangeTabMargin(TabItem item, int value)
        {
            Thickness margin = item.Margin;
            margin.Bottom = value;
            item.Margin = margin;
        }
    }
}