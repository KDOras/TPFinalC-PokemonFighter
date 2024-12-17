using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfCours.Model;

namespace WpfCours.MVVM.ViewModel
{
    public class MainPageVM : BaseVM
    {
        public int IdAllies = 1;
        public int IdEnnemy = 1;
        private string AlliesVarURL;
        private string EnnemyVarURL;
        private int PlayerHealthVar;
        private int EnemyHealthVar;
        private int PlayerHealthMax;
        private int EnemyHealthMax;
        private string Attack1Var;
        private string Attack2Var;
        private int VictoryCount1 = 0;
        private string SpellID1;
        private string SpellID2;
        public double BoostPokemonAttack = 1;
        public double BoostPokemonPV = 1;
        public double BoostPokemonAttackEnnemy = 1;
        public double BoostPokemonPVEnnemy = 1;
        public string Attack1Ennemy;
        public string Attack2Ennemy;
        private string BackgroundPath;
        public List<string> ennemytype;
        public List<string> alliestype;

        public string Background{ get { return BackgroundPath; } set { if (SetProperty(ref BackgroundPath, value)) { OnPropertyChanged(nameof(Background)); } } }

        // Propriété pour le Pokémon ennemi en combat
        private Monster EnemyPokemon2;
        public Monster EnemyPokemon1
        {
            get => EnemyPokemon2;
            set
            {
                EnemyPokemon2 = value;
                OnPropertyChanged(nameof(EnemyPokemon1));
            }
        }

        public string AlliesURL { get { return AlliesVarURL; } set { if (SetProperty(ref AlliesVarURL, value)) { OnPropertyChanged(nameof(AlliesURL)); } } }
        public string EnnemyURL { get { return EnnemyVarURL; } set { if (SetProperty(ref EnnemyVarURL, value)) { OnPropertyChanged(nameof(EnnemyURL)); } } }
        public int PlayerHealth { get { return PlayerHealthVar; } set { if (SetProperty(ref PlayerHealthVar, value)) { OnPropertyChanged(nameof(PlayerHealth)); } } }

        public int EnemyHealth { get { return EnemyHealthVar; } set { if (SetProperty(ref EnemyHealthVar, value)) { OnPropertyChanged(nameof(EnemyHealth)); } } }
        public int PlayerHealth2 { get { return PlayerHealthMax; } set { if (SetProperty(ref PlayerHealthMax, value)) { OnPropertyChanged(nameof(PlayerHealth2)); } } }

        public int EnemyHealth2 { get { return EnemyHealthMax; } set { if (SetProperty(ref EnemyHealthMax, value)) { OnPropertyChanged(nameof(EnemyHealth2)); } } }
        public string Attack1 { get { return Attack1Var; } set { if (SetProperty(ref Attack1Var, value)) { OnPropertyChanged(nameof(Attack1)); } } }
        public string Attack2 { get { return Attack2Var; } set { if (SetProperty(ref Attack2Var, value)) { OnPropertyChanged(nameof(Attack2)); } } }
        public int VictoryCount { get { return VictoryCount1; } set { if (SetProperty(ref VictoryCount1, value)) { OnPropertyChanged(nameof(VictoryCount)); } } }
        public string SpellID { get { return SpellID1; } set { if (SetProperty(ref SpellID1, value)) { OnPropertyChanged(nameof(SpellID)); } } }
        public string SpellIDC{ get { return SpellID2; } set { if (SetProperty(ref SpellID2, value)) { OnPropertyChanged(nameof(SpellIDC)); } } }


        public ICommand AttackCommand { get; set; }

        public ICommand RequestChangeViewCommand { get; set; }
        public MainPageVM()
        {
            AttackCommand = new RelayCommand<string>(param =>
            {
                Attack(param);
            });
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
        }

        private Monster _currentPlayerPokemon;
        public Monster CurrentPlayerPokemon
        {
            get => _currentPlayerPokemon;
            set
            {
                _currentPlayerPokemon = value;
                OnPropertyChanged(nameof(CurrentPlayerPokemon));
            }
        }

        public void LoadPokemon(int pokemonId)
        {
            using (var context = new ExerciceMonsterContext())
                CurrentPlayerPokemon = context.Monsters.Include(m => m.Spells).FirstOrDefault(m => m.Id == pokemonId);
        }

        public int Attack(string attackName)
        {
            PokemonService service = new PokemonService();
            // Récupérer l'ID de l'attaque via son nom
            int spellId = GetSpellIdByName(attackName);
            double adjustedDamage;

            if (spellId != -1)
            {
                // Utilisez l'ID pour effectuer des actions
                var selectedSpell = service.GetSpellIdBySpellId(spellId, CurrentPlayerPokemon);

                double typeMultiplier = service.CalculateDamageMultiplier(alliestype, ennemytype);
                // Appliquez les dégâts

                adjustedDamage = GetSpellDamageByID(selectedSpell) * BoostPokemonAttack * typeMultiplier;


                // Log des dégâts calculés

                // Appliquez les dégâts à l'ennemi
                EnemyHealth -= (int)Math.Floor(adjustedDamage);

                // Vérifie si l'ennemi est vaincu
                if (EnemyHealth <= 0)
                {
                    HandleVictory();
                }
                else
                {

                    EnnemyAttack(); // Appel à l'attaque ennemie

                    // Si la santé du joueur est inférieure ou égale à 0, gestion de la défaite
                    if (PlayerHealth <= 0)
                    {
                        HandleDefeat();
                        return 0;
                    }
                }
            }
            return -1;
            }

        private void EnnemyAttack()
        {
            PokemonService service = new PokemonService();
            int spellId = 0;
            double adjustedDamage;

            Random random = new Random();
            int randomAttack = random.Next(1, 3);

            // Vérifier quel sort est choisi
            if (randomAttack == 1)
            {
                spellId = GetSpellIdByName(Attack1Ennemy);
            }
            else if (randomAttack == 2)
            {
                spellId = GetSpellIdByName(Attack2Ennemy);
            }

            if (spellId != -1)
            {
                // Sélectionner le sort en fonction de l'ID
                var selectedSpell = service.GetSpellIdBySpellId(spellId, EnemyPokemon1);

                if (selectedSpell == null)
                {
                    return;
                }

                // Vérifier l'ID du Pokémon ennemi

                // Calculer le multiplicateur de type
                double typeMultiplier = service.CalculateDamageMultiplier(ennemytype, alliestype);

                // Récupérer les dégâts du sort
                int spellDamage = GetSpellDamageByID(spellId);

                // Calculer les dégâts ajustés
                adjustedDamage = spellDamage * BoostPokemonAttackEnnemy * typeMultiplier;

                // Appliquer les dégâts à l'ennemi
                PlayerHealth -= (int)Math.Floor(adjustedDamage);
            }
            else
            {
                MessageBox.Show("Aucun sort valide n'a été trouvé pour l'ennemi.");
            }
        }

        private int HandleVictory()
        {
            VictoryCount++;
            MessageBox.Show("Victoire ! Voulez-vous recommencer ?", "Victoire", MessageBoxButton.YesNo);

            if (MessageBoxResult.Yes == MessageBox.Show("Voulez-vous recommencer ?", "Victoire", MessageBoxButton.YesNo))
            {
                RandomizeBoosts();
                LoadPokemons(); // Réinitialisation des Pokémon
                return 1;
            }
            else
            {
                HandleRequestChangeViewCommand();
                return 0;
            }
        }
        private int HandleDefeat()
        {
            VictoryCount = 0;
            ResetBoosts();

            MessageBox.Show("Défaite ! Voulez-vous recommencer ?", "Défaite", MessageBoxButton.YesNo);

            if (MessageBoxResult.Yes == MessageBox.Show("Voulez-vous recommencer ?", "Défaite", MessageBoxButton.YesNo))
            {
                LoadPokemons();
                return -1;
            }
            else
            {
                HandleRequestChangeViewCommand();
                return 0;
            }
        }

        private void ResetBoosts()
        {
            BoostPokemonAttack = 1;
            BoostPokemonAttackEnnemy = 1;
            BoostPokemonPV = 1;
            BoostPokemonPVEnnemy = 1;
        }

        private void RandomizeBoosts()
        {
            Random random = new Random();
            int randomBoost = random.Next(1, 3);

            if (randomBoost == 1)
            {
                BoostPokemonAttack += 0.10;
                BoostPokemonPVEnnemy += 0.15;
            }
            else
            {
                BoostPokemonPV += 0.10;
                BoostPokemonAttackEnnemy += 0.15;
            }
        }

        public int GetSpellDamageByID(int spellId)
        {
            using (var context = new ExerciceMonsterContext())
            {
                try
                {
                    // Vérifie que l'ID du sort est valide
                    if (spellId <= 0)
                    {
                        MessageBox.Show("ID du sort invalide.");
                        return 0;  // Retourne 0 pour indiquer que le sort n'est pas valide
                    }

                    // Recherche du sort dans la base de données
                    var spell = context.Spells.FirstOrDefault(m => m.Id == spellId);

                    // Vérifie si le sort a été trouvé
                    if (spell != null)
                    {
                        // Afficher les détails du sort pour déboguer

                        return spell.Damage;  // Retourne les dégâts du sort
                    }
                    else
                    {
                        // Si le sort n'est pas trouvé, afficher un message d'erreur
                        MessageBox.Show($"Le sort avec l'ID {spellId} n'a pas été trouvé dans la base de données.");
                        return 0;  // Retourner 0 ou une valeur appropriée si le sort n'existe pas
                    }
                }
                catch (Exception ex)
                {
                    // En cas d'exception, afficher un message d'erreur
                    MessageBox.Show($"Une erreur s'est produite lors de la récupération du sort : {ex.Message}");
                    return 0;  // Retourner 0 pour gérer l'exception
                }
            }
        }
        // Cette méthode retourne l'ID de l'attaque en fonction de son nom
        public int GetSpellIdByName(string attackName)
        {
            // Vérifiez si le Pokémon actuel est null
            if (CurrentPlayerPokemon == null)
            {
                MessageBox.Show("Aucun Pokémon n'est sélectionné.");
                return -1;
            }

            // Vérifiez si le Pokémon a des sorts (Spells)
            if (CurrentPlayerPokemon.Spells == null || !CurrentPlayerPokemon.Spells.Any())
            {
                MessageBox.Show("Ce Pokémon n'a pas de sorts disponibles.");
                return -1;
            }

            // Recherche du sort par nom (insensible à la casse)
            var spell = CurrentPlayerPokemon.Spells.FirstOrDefault(s => s.Name.Equals(attackName, StringComparison.OrdinalIgnoreCase));

            // Si le sort est trouvé, retournez son ID, sinon retournez -1
            if (spell != null)
            {
                return spell.Id;
            }
            else
            {
                MessageBox.Show($"L'attaque '{attackName}' n'a pas été trouvée.");
                return -1;
            }
        }

        public int GetMonsterIdByName(string monsterName)
        {
            // Vérifiez si le nom du monstre est nul ou vide
            if (string.IsNullOrEmpty(monsterName))
            {
                MessageBox.Show("Le nom du Pokémon ne peut pas être vide.");
                return -1;
            }

            using (var context = new ExerciceMonsterContext())
            {
                // Récupère tous les Pokémon en mémoire
                var monster = context.Monsters
                                     .AsEnumerable()  // Cela force l'évaluation côté client
                                     .FirstOrDefault(m => m.Name.Equals(monsterName, StringComparison.OrdinalIgnoreCase));

                // Si le Pokémon est trouvé, retournez son ID, sinon retournez -1
                if (monster != null)
                {
                    IdAllies = monster.Id;
                    return monster.Id;
                }
                else
                {
                    MessageBox.Show($"Le Pokémon '{monsterName}' n'a pas été trouvé.");
                    return -1;
                }
            }
        }
        public void LoadSelectedPokemon()
        {
            // Récupérer le Pokémon sélectionné depuis le SharedService
            string Test = SharedService.Instance.SelectedPokemon;

            if (Test != null)
            {
                // Utiliser les données du Pokémon comme tu veux
                IdAllies = GetMonsterIdByName(Test);
            }
        }

        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainBackgroundVM());
        }
        public override void OnShowVM()
        {
            ChangeBackground();
            LoadPokemons();
        }

        public void ChangeBackground()
        {
            Random random = new Random();
            int i = random.Next(1, 4);
            switch (i)
            {
                case 1:
                    Background = "https://pokemonrevolution.net/forum/uploads/monthly_2021_03/DVMT-6OXcAE2rZY.jpg.afab972f972bd7fbd4253bc7aa1cf27f.jpg";
                    break;
                case 2:
                    Background = "https://fiverr-res.cloudinary.com/images/q_auto,f_auto/gigs/204364595/original/86db6005cd51b4f60e71cca277f603a82cf5646a/draw-a-pixel-pokemon-battle-background.png";
                    break;
                case 3:
                    Background = "https://pbs.twimg.com/media/DXYtcu7WsAA4my4.png";
                    break;
            }
        }

        public void LoadPokemons()
        {
            LoadSelectedPokemon();
            double adjustedPV;
            double adjustedPVMax;
            double adjustedPVEnnemy;
            double adjustedPVMaxEnnemy;
            PokemonService pokemonService = new PokemonService();
            pokemonService.CleanData();
            // Création d'un contexte de base de données
            using (var context = new ExerciceMonsterContext()) // Récupère le DbContext configuré
            {
                if (context == null)
                {
                    throw new InvalidOperationException("Le contexte de base de données est null.");
                }

                // Génère un ID Pokémon aléatoire pour l'ennemi
                Random random = new Random();
                int randomId = random.Next(1, context.Monsters.Count() + 1);
                randomId = pokemonService.TestEvolution(randomId, VictoryCount); // Applique l'évolution
                IdAllies = pokemonService.TestEvolution(IdAllies, VictoryCount); // Evolution du Pokémon du joueur
                IdEnnemy = randomId;

                // Récupère les Pokémon du joueur et de l'ennemi en une seule requête pour optimiser
                var playerPokemon = context.Monsters.FirstOrDefault(p => p.Id == IdAllies);
                var enemyPokemon = context.Monsters.FirstOrDefault(p => p.Id == randomId);

                // Vérifie si les Pokémon existent avant de procéder
                if (playerPokemon == null || enemyPokemon == null)
                {
                    throw new InvalidOperationException("Le Pokémon du joueur ou l'ennemi est introuvable.");
                }

                // Attribue les valeurs des Pokémon du joueur
                EnemyPokemon1 = enemyPokemon;
                LoadPokemon(IdAllies);
                AlliesURL = playerPokemon.ImageUrl;

                // Calcul des points de vie du joueur
                adjustedPV = playerPokemon.Health * BoostPokemonPV;
                adjustedPVMax = playerPokemon.Health * BoostPokemonPV;
                PlayerHealth = (int)Math.Floor(adjustedPV);
                PlayerHealth2 = (int)Math.Floor(adjustedPVMax);

                // Attribue les valeurs des Pokémon ennemis
                EnnemyURL = enemyPokemon.ImageUrl;
                adjustedPVEnnemy = enemyPokemon.Health * BoostPokemonPVEnnemy;
                adjustedPVMaxEnnemy = enemyPokemon.Health * BoostPokemonPVEnnemy;
                EnemyHealth = (int)Math.Floor(adjustedPVEnnemy);
                EnemyHealth2 = (int)Math.Floor(adjustedPVMaxEnnemy);

                // Récupère les types des Pokémon
                ennemytype = pokemonService.GetPokemonTypes(IdEnnemy);
                alliestype = pokemonService.GetPokemonTypes(IdAllies);

                // Charge les attaques du Pokémon du joueur
                var playerMonster = context.Monsters
                                           .Include(m => m.Spells)
                                           .FirstOrDefault(m => m.Id == playerPokemon.Id);

                if (playerMonster != null && playerMonster.Spells != null && playerMonster.Spells.Count > 0)
                {
                    // Assigner les attaques du joueur
                    Attack1 = playerMonster.Spells.ElementAtOrDefault(0)?.Name ?? "Aucune attaque";
                    Attack2 = playerMonster.Spells.ElementAtOrDefault(1)?.Name ?? "Aucune attaque";
                    SpellID = Attack1;
                    SpellIDC = Attack2;
                }

                // Charge les attaques du Pokémon ennemi
                var enemyMonster = context.Monsters
                                          .Include(m => m.Spells)
                                          .FirstOrDefault(m => m.Id == enemyPokemon.Id);

                if (enemyMonster != null && enemyMonster.Spells != null && enemyMonster.Spells.Count > 0)
                {
                    // Assigner les attaques de l'ennemi
                    Attack1Ennemy = enemyMonster.Spells.ElementAtOrDefault(0)?.Name ?? "Aucune attaque";
                    Attack2Ennemy = enemyMonster.Spells.ElementAtOrDefault(1)?.Name ?? "Aucune attaque";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}