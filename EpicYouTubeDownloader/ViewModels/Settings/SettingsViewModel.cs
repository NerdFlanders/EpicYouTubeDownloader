using Caliburn.Micro;

namespace EpicYouTubeDownloader.ViewModels.Settings
{
    public class SettingsViewModel : Screen
    {
        public SettingsGeneralViewModel SettingsGeneralViewModel { get; set; }

        public SettingsViewModel(SettingsGeneralViewModel settingsGeneralViewModel)
        {
            SettingsGeneralViewModel = settingsGeneralViewModel;
        }
    }
}