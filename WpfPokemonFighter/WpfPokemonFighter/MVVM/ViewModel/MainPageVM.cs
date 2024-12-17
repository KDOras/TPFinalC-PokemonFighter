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
                var selectedSpell = service.GetSpellIdBySpellId(spellId,  CurrentPlayerPokemon);
                // Appliquez les dégâts

                    adjustedDamage = GetSpellDamageByID(selectedSpell) * BoostPokemonAttack;
                    EnemyHealth -= (int)Math.Floor(adjustedDamage);
                if (EnemyHealth <= 0)
                {
                    VictoryCount++; // Incrémentation du compteur;
                    if (MessageBox.Show("Voulez-vous recommencer ?", "Victoire", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        LoadPokemons();
                        Random random = new Random();
                        int randomId1 = random.Next(1, 3);
                        if (randomId1 == 1)
                        {
                            BoostPokemonAttack += 0.10;
                            BoostPokemonPVEnnemy += 0.15;
                        }
                        else if (randomId1 == 2)
                        {
                            BoostPokemonPV += 0.10;
                            BoostPokemonAttackEnnemy += 0.15;
                        }
                        return 1;
                    } else
                    {
                        HandleRequestChangeViewCommand();
                        return 0;
                    }
                }
                EnnemyAttack();
                if (PlayerHealth <= 0)
                {
                    VictoryCount = 0;
                    BoostPokemonAttack = 1;
                    BoostPokemonAttackEnnemy = 1;
                    BoostPokemonPV = 1;
                    BoostPokemonPVEnnemy = 1;
                    if (MessageBox.Show("Voulez-vous recommencer ?", "Défaite", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        LoadPokemons();
                        return -1;
                    } else { 
                        HandleRequestChangeViewCommand(); 
                        return 0; 
                    }
                }
            }
            return -1;
        }

        private void EnnemyAttack()
        {
            PokemonService service = new PokemonService();
            int ID = 0;
            double adjustedDamageEnnemy;
            Random random = new Random();
            int randomId1 = random.Next(1, 3);
            if (randomId1 == 1)
            {
                ID = GetSpellIdByName(Attack1Ennemy);

            }
            else if (randomId1 == 2)
            {
                ID = GetSpellIdByName(Attack2Ennemy);
            }
            if (ID != -1)
            {
                // Utilisez l'ID pour effectuer des actions
                var selectedSpell = service.GetSpellIdBySpellId(ID, CurrentPlayerPokemon);
                // Appliquez les dégâts

                adjustedDamageEnnemy = GetSpellDamageByID(selectedSpell) * BoostPokemonAttackEnnemy;
                PlayerHealth -= (int)Math.Floor(adjustedDamageEnnemy);
            }
        }
        public int GetSpellDamageByID(int I)
        {
            using (var context = new ExerciceMonsterContext())
            {
                var spell = context.Spells.FirstOrDefault(m => m.Id == I);
                return spell.Damage;
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

        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainBackgroundVM());
        }
        public override void OnShowVM()
        {
            LoadPokemons();
        }

        private void LoadPokemons()
        {
            double adjustedPV;
            double adjustedPVMax;
            double adjustedPVEnnemy;
            double adjustedPVMaxEnnemy;
            PokemonService pokemonService = new PokemonService();
            pokemonService.CleanData();
            // Création d'un contexte de base de données
            using (var context = new ExerciceMonsterContext()) // Remplace par ton contexte de base de données
            {
                // Charger le Pokémon du joueur (ici, j'assume que c'est Pikachu)
                Random random = new Random();
                Random random2 = new Random();
                int randomId = random.Next(1, context.Monsters.Count() + 1);
                int randomId1 = random.Next(1, context.Monsters.Count() + 1);
                randomId = pokemonService.TestEvolution(randomId);
                randomId1 = pokemonService.TestEvolution(randomId1);
                var playerPokemon = context.Monsters.FirstOrDefault(p => p.Id == randomId1);
                var enemyPokemon = context.Monsters.FirstOrDefault(p => p.Id == randomId);
                LoadPokemon(randomId1);
                // Pokemon du Joueurs
                AlliesURL = playerPokemon.ImageUrl;
                adjustedPV = playerPokemon.Health * BoostPokemonPV;
                adjustedPVMax = playerPokemon.Health * BoostPokemonPV;
                PlayerHealth = (int)Math.Floor(adjustedPV);
                PlayerHealth2 = (int)Math.Floor(adjustedPVMax);
                //Pokemon Ennemy
                EnnemyURL = enemyPokemon.ImageUrl;
                adjustedPVEnnemy = enemyPokemon.Health * BoostPokemonPVEnnemy;
                adjustedPVMaxEnnemy = enemyPokemon.Health * BoostPokemonPVEnnemy;
                EnemyHealth = (int)Math.Floor(adjustedPVEnnemy);
                EnemyHealth2 = (int)Math.Floor(adjustedPVMaxEnnemy);
                //Spell
                var spellsList = playerPokemon.Spells.ToList(); // Convertir ICollection<Spell> en List<Spell>
                                                                // Charger le Monstre avec ses attaques
                var monster = context.Monsters
                                     .Include(m => m.Spells)
                                     .FirstOrDefault(m => m.Id == playerPokemon.Id);

                if (monster != null)
                {
                    // Vérifier si le Monstre a des attaques
                    if (monster.Spells != null && monster.Spells.Count > 0)
                    {
                        // Extraire la première attaque
                        var attack1 = monster.Spells.ElementAtOrDefault(0)?.Name ?? "Aucune attaque";

                        // Extraire la deuxième attaque
                        var attack2 = monster.Spells.ElementAtOrDefault(1)?.Name ?? "Aucune attaque";

                        // Assigner les valeurs aux variables de liaison
                        Attack1 = attack1;
                        Attack2 = attack2;
                        SpellID = attack1;
                        SpellIDC = attack2;
                        }
                    }
                var monster2 = context.Monsters
                                     .Include(m => m.Spells)
                                     .FirstOrDefault(m => m.Id == enemyPokemon.Id);

                if (monster2 != null) {
                    // Vérifier si le Monstre a des attaques
                    if (monster.Spells != null && monster.Spells.Count > 0)
                    {
                        // Extraire la première attaque
                        var attack1E = monster.Spells.ElementAtOrDefault(0)?.Name ?? "Aucune attaque";

                        // Extraire la deuxième attaque
                        var attack2E = monster.Spells.ElementAtOrDefault(1)?.Name ?? "Aucune attaque";

                        // Assigner les valeurs aux variables de liaison
                        Attack1Ennemy = attack1E;
                        Attack2Ennemy = attack2E;
                    }
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