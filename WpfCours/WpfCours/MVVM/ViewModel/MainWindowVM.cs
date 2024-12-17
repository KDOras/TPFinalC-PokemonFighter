using CommunityToolkit.Mvvm.ComponentModel;
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
    public class MainWindowVM : BaseVM
    {

        static public  Action<BaseVM> OnRequestVMChange;

        #region Commands
        #endregion
        #region Variables

        private BaseVM _currentVM;
        public BaseVM CurrentVM
        {
            get => _currentVM;
            set
            {
                SetProperty(ref _currentVM, value);
                OnPropertyChanged(nameof(CurrentVM));
            }
        }


     
        #endregion

        public MainWindowVM()
        {
            MainWindowVM.OnRequestVMChange += HandleRequestViewChange;
            MainWindowVM.OnRequestVMChange?.Invoke(new MainBackgroundVM());

        }

        public void HandleRequestViewChange(BaseVM a_VMToChange)
        {
            CurrentVM?.OnHideVM();
            CurrentVM = a_VMToChange;
            CurrentVM?.OnShowVM();
        }
      
    }
}
