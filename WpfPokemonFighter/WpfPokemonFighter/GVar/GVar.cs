using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCours.Model;

namespace WpfCours.MVVM.ViewModel
{
    public class SharedService
    {
        // Singleton pour garantir un accès unique
        private static SharedService _instance;
        public static SharedService Instance => _instance ??= new SharedService();

        // Propriété pour stocker le Pokémon sélectionné
        public string SelectedPokemon { get; set; }
    }
}
