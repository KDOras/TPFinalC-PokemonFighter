using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfCours.Model;

namespace WpfCours.MVVM.ViewModel
{
    public class PokedexVM : BaseVM
    {
        private string username;
        private int loginID;
        private string selectedPokemon;

        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public int LoginID
        {
            get { return loginID; }
            set
            {
                if (loginID != value)
                {
                    loginID = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string SelectedPokemon
        {
            get { return selectedPokemon; }
            set
            {
                if (selectedPokemon != value)
                {
                    selectedPokemon = value;
                    OnPropertyChanged(nameof(SelectedPokemon));
                }
            }
        }

        // Command to handle Pokemon selection
        public ICommand SelectPokemonCommand { get; }

        public PokedexVM()
        {
            SelectPokemonCommand = new RelayCommand(SelectPokemon);
            LoadPokemons();
        }
        private ObservableCollection<Monster> _pokemons;

        public ObservableCollection<Monster> Pokemons
        {
            get { return _pokemons; }
            set
            {
                _pokemons = value;
                OnPropertyChanged(nameof(Pokemons));
            }
        }

        public int GetLoginIDByName(string username)
        {
            using (var context = new ExerciceMonsterContext()) // Assurez-vous d'utiliser votre contexte de données
            {
                // Recherche de l'utilisateur par son username
                var user = context.Logins.FirstOrDefault(l => l.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

                // Si l'utilisateur est trouvé, retourne son LoginId
                if (user != null)
                {
                    return user.Id;
                }
                else
                {
                    MessageBox.Show($"L'utilisateur '{username}' n'a pas été trouvé.");
                    return -1; // Retourne -1 si l'utilisateur n'a pas été trouvé
                }
            }
        }

        private void LoadPokemons()
        {
            using (var context = new ExerciceMonsterContext())
            {
                // Charge les Pokémon depuis la base de données
                var pokemonsFromDb = context.Monsters
                    .Select(m => new Monster
                    {
                        Name = m.Name,
                        ImageUrl = m.ImageUrl,
                    })
                    .ToList();

                Pokemons = new ObservableCollection<Monster>(pokemonsFromDb);
            }
        }

        // Set the username from LoginVM
        public void SetUsername(string username)
        {
            Username = username;
        }

        public void SetLoginID(string username)
        {
            LoginID = GetLoginIDByName(username);
        }

        // Select Pokemon action
        private void SelectPokemon()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}