using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfCours.MVVM.View;
using static System.Net.WebRequestMethods;

namespace WpfCours.MVVM.ViewModel
{
    public class MainBackgroundVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }
        private string BackgroundImage;
        public string BackgroundPath { get { return BackgroundImage; } set { if (SetProperty(ref BackgroundImage, value)) { OnPropertyChanged(nameof(BackgroundPath)); } } }
        public MainBackgroundVM()
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
        }

        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new LoginVM());
        }

        public void ChangeBackground()
        {
            Random random = new Random();
            int i = random.Next(1, 4);
            switch (i)
            {
                case 1:
                    BackgroundPath = "https://www.chromethemer.com/download/hd-wallpapers/pokemon-3840x2160.jpg";
                    break;
                case 2:
                    BackgroundPath = "https://www.chromethemer.com/wallpapers/chromebook-wallpapers/images/960/water-pokemon-chromebook-wallpaper.jpg";
                    break;
                case 3:
                    BackgroundPath = "https://cdn.wallpaper.tn/large/8K-Ultra-Hd-Pokemon-Wallpaper-171126.jpg";
                    break;
            }
        }

        public override void OnShowVM()
        {
            ChangeBackground();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
