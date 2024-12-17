using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCours.MVVM.ViewModel
{
    public class ProfilPageVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }  
        public ProfilPageVM() 
        {
        }
    }
}
