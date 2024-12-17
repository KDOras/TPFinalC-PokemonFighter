using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfCours.MVVM.ViewModel
{
    public class GameVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }  
        public GameVM() 
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
        }
        
        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainBackgroundVM());
        }

        public override void OnShowVM()
        {
            MessageBox.Show("Gameview show");
        }
    }
}
