using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfCours.Model;

namespace WpfCours.MVVM.ViewModel
{
    public class LoginVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }
        public ICommand ExecuteLoginE { get; set; }
        public ICommand ExecuteRegisterE { get; set; }

        private string _username;
        private string _password;
        public string HashPassword;

        // Propriété Username
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username)); // Notifie la vue que la propriété a changé
                }
            }
        }

        // Propriété Password
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password)); // Notifie la vue que la propriété a changé
                }
            }
        }

        public LoginVM() 
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            ExecuteLoginE = new RelayCommand(ExecuteLogin);
            ExecuteRegisterE = new RelayCommand(ExecuteRegister);
        }

        private void ExecuteLogin()
        {
            HashPassword = HashedPassword(Password);
            PokedexVM Pokedex = new PokedexVM();
            if (VerifyLogin(Username, HashPassword))
            {
                // If login is successful, pass the username to PokedexVM
                Pokedex.SetUsername(Username);
                HandleRequestChangeViewCommand();
                // Transition to PokedexVM (either navigate or set DataContext)
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void ExecuteRegister()
        {
            PokemonService service = new PokemonService();
            PokedexVM Pokedex = new PokedexVM();
            if (!VerifyUserExists(Username) && Username != null)
            {
                // If login is successful, pass the username to PokedexVM
                Pokedex.SetUsername(Username);
                HashPassword = HashedPassword(Password);
                MessageBox.Show("Utilisateur Bien Crée");
                service.AddLogin(Username, HashPassword);
                HandleRequestChangeViewCommand();
                // Transition to PokedexVM (either navigate or set DataContext)
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private string HashedPassword(string password)
        {
            // Convertir le mot de passe en bytes
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            // Utiliser SHA256 pour hasher
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(bytes);

                // Convertir le hash en chaîne hexadécimale
                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }

        public bool VerifyLogin(string username, string enteredPassword)
        {
            using (var context = new ExerciceMonsterContext())
            {
                // Recherche dans la base de données le Login correspondant au pseudo
                var user = context.Logins.FirstOrDefault(l => l.Username == username);

                if (user == null)
                {
                    // Aucun utilisateur trouvé avec ce pseudo
                    MessageBox.Show("Erreur : Utilisateur introuvable !");
                    return false;
                }

                // Vérifie le mot de passe en comparant celui haché dans la base et celui saisi
                if (user.PasswordHash == HashedPassword(enteredPassword))
                {
                    MessageBox.Show("Connexion réussie !");
                    return true;
                }
                else
                {
                    MessageBox.Show("Erreur : Mot de passe incorrect !");
                    return false;
                }
            }
        }

        public bool VerifyUserExists(string username)
        {
            using (var context = new ExerciceMonsterContext())
            {
                // Vérifie si un utilisateur avec le même Username existe
                return context.Logins.Any(l => l.Username == username);
            }
        }

        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new PokedexVM());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
