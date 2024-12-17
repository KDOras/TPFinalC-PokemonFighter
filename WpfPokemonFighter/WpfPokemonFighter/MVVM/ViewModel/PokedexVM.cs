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
        private int selectedPokemon;
        private string _pokemon1Sprite;
        private string _pokemon2Sprite;
        private string _pokemon3Sprite;
        private string _pokemon4Sprite;
        private string _pokemon5Sprite;
        private string _pokemon6Sprite;
        private string _pokemon7Sprite;
        private string _pokemon8Sprite;
        public string Pokemon1Sprite { get { return _pokemon1Sprite; } set { _pokemon1Sprite = value; OnPropertyChanged(nameof(Pokemon1Sprite)); } }
        public string Pokemon2Sprite { get { return _pokemon2Sprite; } set { _pokemon2Sprite = value; OnPropertyChanged(nameof(Pokemon2Sprite)); } }
        public string Pokemon3Sprite { get { return _pokemon3Sprite; } set { _pokemon3Sprite = value; OnPropertyChanged(nameof(Pokemon3Sprite)); } }
        public string Pokemon4Sprite { get { return _pokemon4Sprite; } set { _pokemon4Sprite = value; OnPropertyChanged(nameof(Pokemon4Sprite)); } }
        public string Pokemon5Sprite { get { return _pokemon5Sprite; } set { _pokemon5Sprite = value; OnPropertyChanged(nameof(Pokemon5Sprite)); } }
        public string Pokemon6Sprite { get { return _pokemon6Sprite; } set { _pokemon6Sprite = value; OnPropertyChanged(nameof(Pokemon6Sprite)); } }
        public string Pokemon7Sprite { get { return _pokemon7Sprite; } set { _pokemon7Sprite = value; OnPropertyChanged(nameof(Pokemon7Sprite)); } }
        public string Pokemon8Sprite { get { return _pokemon8Sprite; } set { _pokemon8Sprite = value; OnPropertyChanged(nameof(Pokemon8Sprite)); } }

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

        public int SelectedPokemon
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

        public ICommand RequestChangeViewCommand { get; set; }

        public PokedexVM()
        {
            // Récupère les Pokémon depuis la base de données
            LoadPokemons();
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            SelectPokemonCommand = new RelayCommand<string>(param =>
            {
                SelectPokemon(param);
            });
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
            MainPageVM main = new MainPageVM();
            main.LoadPokemons();
            using (var context = new ExerciceMonsterContext())
            {
                var pokemons = context.Monsters.Take(24).ToList(); // Prend les 8 premiers Pokémon

                Pokemon1Sprite = pokemons[0]?.ImageUrl;
                Pokemon2Sprite = pokemons[3]?.ImageUrl;
                Pokemon3Sprite = pokemons[6]?.ImageUrl;
                Pokemon4Sprite = pokemons[9]?.ImageUrl;
                Pokemon5Sprite = pokemons[17]?.ImageUrl;
                Pokemon6Sprite = pokemons[11]?.ImageUrl;
                Pokemon7Sprite = pokemons[14]?.ImageUrl;
                Pokemon8Sprite = pokemons[19]?.ImageUrl;
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
        private void SelectPokemon(string pokemon)
        {
            MainPageVM Main = new MainPageVM();
            SharedService.Instance.SelectedPokemon = pokemon;
            SelectedPokemon = Main.GetMonsterIdByName(pokemon);
            HandleRequestChangeViewCommand();
        }
        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainPageVM());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}