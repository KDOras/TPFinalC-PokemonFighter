using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfCours.Model;
using WpfCours.MVVM.View;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WpfCours.MVVM.ViewModel
{
    public class PokemonService
    {

        public void AddPlayer(string name, int LoginID, string PokemonUsed)
        {
            using (var context = new ExerciceMonsterContext())
            {
                var player = new Player
                {
                    Name = name,
                    LoginId = LoginID,
                    Monsters = [context.Monsters.FirstOrDefault(x => x.Name == PokemonUsed)]
                };

                context.Players.Add(player);
                context.SaveChanges();
            }
        }

        public void AddLogin(string username, string hashedpassword)
        {
            using (var context = new ExerciceMonsterContext())
            {
                if (IsUsernameAvailable(username))
                {
                    var Login = new Login
                    {
                        Username = username,
                        PasswordHash = hashedpassword
                    };

                    context.Logins.Add(Login);
                    context.SaveChanges();

                }
            }
        }

        public void AddPokemon(string name, int hp, string imagePath, string Attack1)
        {
            using (var context = new ExerciceMonsterContext())
            {
                var pokemon = new Monster
                {
                    Name = name,
                    Health = hp,
                    ImageUrl = imagePath,
                    Spells = [context.Spells.FirstOrDefault(x => x.Name == Attack1)],
                };

                context.Monsters.Add(pokemon);
                context.SaveChanges();
            }
        }
        public void AddPokemon(string name, int hp, string imagePath, string Attack1, string Attack2)
        {
            using (var context = new ExerciceMonsterContext())
            {
                var pokemon = new Monster
                {
                    Name = name,
                    Health = hp,
                    ImageUrl = imagePath,
                    Spells = [context.Spells.FirstOrDefault(x => x.Name == Attack1), context.Spells.FirstOrDefault(x => x.Name == Attack2)],
                };

                context.Monsters.Add(pokemon);
                context.SaveChanges();
            }
        }

        public void AddPokemon(string name, int hp, string imagePath, string Attack1, string Attack2, string Attack3)
        {
            using (var context = new ExerciceMonsterContext())
            {
                var pokemon = new Monster
                {
                    Name = name,
                    Health = hp,
                    ImageUrl = imagePath,
                    Spells = [context.Spells.FirstOrDefault(x => x.Name == Attack1), context.Spells.FirstOrDefault(x => x.Name == Attack2), context.Spells.FirstOrDefault(x => x.Name == Attack3)],
                };

                context.Monsters.Add(pokemon);
                context.SaveChanges();
            }
        }

        public void AddPokemon(string name, int hp, string imagePath, string Attack1, string Attack2, string Attack3, string Attack4)
        {
            using (var context = new ExerciceMonsterContext())
            {
                var pokemon = new Monster
                {
                    Name = name,
                    Health = hp,
                    ImageUrl = imagePath,
                    Spells = [context.Spells.FirstOrDefault(x => x.Name == Attack1), context.Spells.FirstOrDefault(x => x.Name == Attack2), context.Spells.FirstOrDefault(x => x.Name == Attack3), context.Spells.FirstOrDefault(x => x.Name == Attack4)],
                };

                context.Monsters.Add(pokemon);
                context.SaveChanges();
            }
        }

        public void AddSpell(string name, int damage, string desc)
        {
            using (var context = new ExerciceMonsterContext())
            {
                var spell = new Spell
                {
                    Name = name,
                    Damage = damage,
                    Description = desc,
                };

                context.Spells.Add(spell);
                context.SaveChanges();
            }
        }

    public bool DatabaseVerif()
        {
            var service = DbContextManager.GetDbContext();
            return !service.Monsters.Any();
        }

        public void CleanData()
        {
                using (SqlConnection connection = new SqlConnection("Server=MSI\\SQLEXPRESS;Database=ExerciceMonster;Trusted_Connection=True; TrustServerCertificate=True;"))
                {
                    connection.Open(); // Assurez-vous que cette ligne n'échoue pas

                    // Désactiver les contraintes
                    string disableConstraints = "ALTER TABLE Monster NOCHECK CONSTRAINT ALL;";
                    using (SqlCommand command = new SqlCommand(disableConstraints, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Supprimer les données
                    string deleteQuery = "DELETE FROM MonsterSpell;";
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    string deletedQuery = "DELETE FROM PlayerMonster;";
                    using (SqlCommand command = new SqlCommand(deletedQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    string deletQuery = "DELETE FROM Monster;";
                    using (SqlCommand command = new SqlCommand(deletQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    string deletedsQuery = "DELETE FROM Spell;";
                    using (SqlCommand command = new SqlCommand(deletedsQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Réactiver les contraintes
                    string enableConstraints = "ALTER TABLE Monster CHECK CONSTRAINT ALL;";
                    using (SqlCommand command = new SqlCommand(enableConstraints, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Reset Auto-increment
                    string AUTODELETEDsQuery = "DBCC CHECKIDENT ('Spell', RESEED,0);";
                    using (SqlCommand command = new SqlCommand(AUTODELETEDsQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    string AUTODELETEsQuery = "DBCC CHECKIDENT ('Monster', RESEED,0);";
                    using (SqlCommand command = new SqlCommand(AUTODELETEsQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    string AUTODELETEQuery = "DBCC CHECKIDENT ('Player', RESEED,0);";
                    using (SqlCommand command = new SqlCommand(AUTODELETEQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    string AUTODELETQuery = "DBCC CHECKIDENT ('Login', RESEED,0);";
                    using (SqlCommand command = new SqlCommand(AUTODELETQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    SpellInit();
                    PokemonInit();
                }
            }

        public void PokemonInit()
        {

            PokemonService service = new PokemonService();
            //Family Bulbasaur
            service.AddPokemon("Bulbasaur", 45, "https://img.pokemondb.net/sprites/black-white/normal/bulbasaur.png", "Tackle", "Vine Whip");
            service.AddPokemon("Ivysaur", 60, "https://img.pokemondb.net/sprites/black-white/normal/ivysaur.png", "Razor Leaf", "Vine Whip");
            service.AddPokemon("Venusaur", 80, "https://img.pokemondb.net/sprites/black-white/normal/venusaur-f.png", "Razor Leaf", "Vine Whip");
            //Family Charmander
            service.AddPokemon("Charmander", 39, "https://img.pokemondb.net/sprites/black-white/normal/charmander.png", "Scratch", "Ember");
            service.AddPokemon("Charmeleon", 58, "https://img.pokemondb.net/sprites/black-white/normal/charmeleon.png", "Ember", "Flamethrower");
            service.AddPokemon("Charizard", 78, "https://img.pokemondb.net/sprites/black-white/normal/charizard.png", "Dragon Breath", "Flamethrower");
            //Family Squirtle
            service.AddPokemon("Squirtle", 44, "https://img.pokemondb.net/sprites/black-white/normal/squirtle.png", "Tackle", "Water Gun");
            service.AddPokemon("Wartortle", 59, "https://img.pokemondb.net/sprites/black-white/normal/wartortle.png", "Water Pulse", "Water Gun");
            service.AddPokemon("Blastoise", 79, "https://img.pokemondb.net/sprites/black-white/normal/blastoise.png", "Water Pulse", "Bite");
            //Family Pikachu
            service.AddPokemon("Pikachu", 35, "https://img.pokemondb.net/sprites/black-white/normal/pikachu-f.png", "Tackle", "ThunderShock");
            service.AddPokemon("Raichu", 60, "https://img.pokemondb.net/sprites/black-white/normal/raichu-f.png", "Thunderbolt", "ThunderShock");
            //Family Pidgey
            service.AddPokemon("Pidgey", 40, "https://img.pokemondb.net/sprites/black-white/normal/pidgey.png", "Scratch", "Gust");
            service.AddPokemon("Pidgeotto", 63, "https://img.pokemondb.net/sprites/black-white/normal/pidgeotto.png", "Wing Attack", "Gust");
            service.AddPokemon("Pidgeot", 83, "https://img.pokemondb.net/sprites/black-white/normal/pidgeot.png", "Air Slash", "Wing Attack");
            //Family Nidoran♂
            service.AddPokemon("Nidoran♂", 46, "https://img.pokemondb.net/sprites/black-white/normal/nidoran-m.png", "Tackle", "Poison Sting");
            service.AddPokemon("Nidorino", 61, "https://img.pokemondb.net/sprites/black-white/normal/nidorino.png", "Earth Power", "Poison Sting");
            service.AddPokemon("Nidoking", 81, "https://img.pokemondb.net/sprites/black-white/normal/nidoking.png", "Earth Power", "Sludge Wave");
            //Family Vulpix
            service.AddPokemon("Vulpix", 38, "https://img.pokemondb.net/sprites/black-white/normal/vulpix.png", "Tackle", "Ember");
            service.AddPokemon("Ninetales", 73, "https://img.pokemondb.net/sprites/black-white/normal/ninetales.png", "Flamethrower", "Ember");
            //Family Magikarp
            service.AddPokemon("Magikarp", 20, "https://img.pokemondb.net/sprites/black-white/normal/magikarp-f.png", "Splash", "Scratch");
            service.AddPokemon("Gyarados", 95, "https://img.pokemondb.net/sprites/black-white/normal/gyarados-f.png", "Twister", "Ice Fang");
        }

        public void SpellInit()
        {

            PokemonService service = new PokemonService();
            service.AddSpell("Scratch", 7, "The Pokemon Scratch the Ennemy");
            service.AddSpell("Tackle", 7, "The Pokemon Tackle the Ennemy");
            service.AddSpell("Vine Whip", 15, "The Pokemon Launch Vine On The Ennemy");
            service.AddSpell("Ember", 19, "The Pokemon Launch an Ember on the Ennemy");
            service.AddSpell("Water Gun", 17, "The Pokemon Launch Water on The Ennemy");
            service.AddSpell("Gust", 18, "The Pokemon Launch Wind On the ennemy");
            service.AddSpell("ThunderShock", 10, "The Pokemon Scratch the Ennemy");
            service.AddSpell("Splash", 0, "Does Nothing But It Will Be EPIC");
            service.AddSpell("Poison Sting", 14, "The Pokemon Launch Sting Poisonus On The Ennemy");
            service.AddSpell("Twister", 20, "Whips up a tornado to attack.");
            service.AddSpell("Ice Fang", 30, "he user bites with cold-infused fangs");
            service.AddSpell("Razor Leaf", 30, "Cuts the enemy with leaves");
            service.AddSpell("Dragon Breath", 30, "Strikes the foe with an incredible blast of breath");
            service.AddSpell("Flamethrower", 30, "The foe is scorched with intense flames");
            service.AddSpell("Water Pulse", 25, "An attack with a pulsing blast of water");
            service.AddSpell("Bite", 30, "Bites with vicious fangs");
            service.AddSpell("Thunderbolt", 35, "A strong electric blast is loosed at the foe");
            service.AddSpell("Wing Attack", 25, "The foe is struck with large");
            service.AddSpell("Air Slash", 30, "The user attacks with a blade of air that slices even the sky");
            service.AddSpell("Earth Power", 30, "The user makes the ground under the foe erupt with power");
            service.AddSpell("Sludge Wave", 30, "Sludge Wave");

        }

        public int GetSpellIdBySpellId(int spellId,Monster CurrentPlayerPokemon)
        {
            // Vérifie si le Pokémon est chargé
            if (CurrentPlayerPokemon != null)
            {
                // Recherche le sort avec l'ID donné dans la liste des sorts du Pokémon
                var spell = CurrentPlayerPokemon.Spells.FirstOrDefault(s => s.Id == spellId);

                if (spell != null)
                {
                    return spell.Id;  // Retourne l'ID du sort trouvé
                }
                else
                {
                    return -1;  // Retourne -1 si l'attaque n'est pas trouvée
                }
            }
            else
            {
                MessageBox.Show("ERREUR - Pokémon non chargé.");
                return -1;  // Retourne -1 si le Pokémon n'est pas chargé
            }
        }
        public Monster GetPokemonById(int pokemonId)
        {
            // Exemple de récupération depuis une base de données, ou un dictionnaire ou liste
            using (var context = new ExerciceMonsterContext())
            {
                return context.Monsters.FirstOrDefault(m => m.Id == pokemonId);
            }
        }

        public int GetSpellId(int spellId, int pokemonId)
        {
            // Récupérer le Pokémon par son ID (par exemple, depuis la base de données ou un dictionnaire)
            var currentPlayerPokemon = GetPokemonById(pokemonId);

            // Vérifie si le Pokémon est chargé
            if (currentPlayerPokemon != null)
            {
                // Recherche le sort avec l'ID donné dans la liste des sorts du Pokémon
                var spell = currentPlayerPokemon.Spells.FirstOrDefault(s => s.Id == spellId);

                if (spell != null)
                {
                    return spell.Id;  // Retourne l'ID du sort trouvé
                }
                else
                {
                    MessageBox.Show("ERREUR - L'attaque n'existe pas.");
                    return -1;  // Retourne -1 si l'attaque n'est pas trouvée
                }
            }
            else
            {
                MessageBox.Show("ERREUR - Pokémon non chargé.");
                return -1;  // Retourne -1 si le Pokémon n'est pas chargé
            }
        }

        public bool IsUsernameAvailable(string username)
        {
            using (var context = new ExerciceMonsterContext())
            {
                // Vérifie si un utilisateur avec le même pseudo existe déjà
                bool userExists = context.Logins.Any(l => l.Username == username);

                return !userExists; // Retourne vrai si le pseudo est disponible
            }
        }

        public int TestEvolution(int random, int victories)
        {
            // Dictionnaire pour stocker les évolutions (ID actuel -> ID suivant)
            var evolutionFamilies = new Dictionary<int, (int nextLevel1, int nextLevel2)>
    {
        { 1, (2, 3) }, // Bulbasaur
        { 2, (2, 3) }, // Ivysaur
        { 3, (3, 3) }, // Venusaur

        { 4, (5, 6) }, // Charmander
        { 5, (5, 6) }, // Charmeleon
        { 6, (6, 6) }, // Charizard

        { 7, (8, 9) }, // Squirtle
        { 8, (8, 9) }, // Wartortle
        { 9, (9, 9) }, // Blastoise

        { 10, (11, 11) }, // Pikachu
        { 11, (11, 11) }, // Raichu

        { 12, (13, 14) }, // Pidgey
        { 13, (13, 14) }, // Pidgeotto
        { 14, (14, 14) }, // Pidgeot

        { 15, (16, 17) }, // Nidoran♂
        { 16, (16, 17) }, // Nidorino
        { 17, (17, 17) }, // Nidoking

        { 18, (19, 19) }, // Vulpix
        { 19, (19, 19) }, // Ninetales

        { 20, (21, 21) }, // Magikarp
        { 21, (21, 21) }  // Gyarados
    };

            // Vérifier que random est dans les clés du dictionnaire
            if (evolutionFamilies.ContainsKey(random))
            {
                var levels = evolutionFamilies[random];

                // Si VictoryCount >= 9 : évolution finale
                if (victories >= 9)
                {
                    return levels.nextLevel2; // Retourne l'évolution finale
                }
                // Si VictoryCount >= 4 : première évolution
                else if (victories >= 4)
                {
                    return levels.nextLevel1; // Retourne la première évolution
                }
                else
                {
                    // Retourne le niveau de base si VictoryCount trop bas
                    return random; // Pas d'évolution, reste au niveau de base
                }
            }
            // Si aucune évolution n'est possible, retourne l'ID original
            return random;
        }

        public static Dictionary<string, List<string>> TypeAdvantages = new Dictionary<string, List<string>>()
{
    { "Fire", new List<string> { "Grass" } },       // Fire est fort contre Grass
    { "Grass", new List<string> { "Water", "Ground" } }, // Grass est fort contre Water et Ground
    { "Water", new List<string> { "Fire", "Ground" } },  // Water est fort contre Fire et Ground
    { "Ground", new List<string> { "Fire", "Electric" } }, // Ground est fort contre Fire et Electric
    { "Electric", new List<string> { "Water", "Flying" } }, // Electric est fort contre Water et Flying
    { "Flying", new List<string> { "Grass" } },     // Flying est fort contre Grass
    { "Poison", new List<string> { "Grass" } },     // Poison est fort contre Grass
    { "Normal", new List<string>() },
    { "Dragon", new List<string> { "Dragon" }
        }
    };

        // Étape 2 : Association Pokémon -> Type (Id = numéro du Pokémon)
        public static Dictionary<int, List<string>> PokemonTypes = new Dictionary<int, List<string>>()
{
    { 1, new List<string> { "Grass", "Poison" } },
    { 2, new List<string> { "Grass", "Poison" } },
    { 3, new List<string> { "Grass", "Poison" } },
    { 4, new List<string> { "Fire" } },
    { 5, new List<string> { "Fire" } },
    { 6, new List<string> { "Fire" } },
    { 7, new List<string> { "Fire", "Flying" } },
    { 8, new List<string> { "Water" } },
    { 9, new List<string> { "Water" } },
    { 10, new List<string> { "Electric" } },
    { 11, new List<string> { "Electric" } },
    { 12, new List<string> { "Normal", "Flying" } },
    { 13, new List<string> { "Normal", "Flying" } },
    { 14, new List<string> { "Normal", "Flying" } },
    { 15, new List<string> { "Poison" } },
    { 16, new List<string> { "Poison" } },
    { 17, new List<string> { "Poison", "Ground" } },
    { 18, new List<string> { "Fire" } },
    { 19, new List<string> { "Fire" } },
    { 20, new List<string> { "Water" } },
    { 21, new List<string> { "Water", "Flying" } },
};

        // Étape 3 : Méthode pour calculer les dégâts en fonction des types
        public double CalculateDamageMultiplier(List<string> attackerTypes, List<string> defenderTypes)
        {
            double multiplier = 1.0;

            foreach (var attackerType in attackerTypes)
            {
                if (TypeAdvantages.ContainsKey(attackerType))
                {
                    var effectiveTypes = TypeAdvantages[attackerType];
                    foreach (var effectiveType in effectiveTypes)
                    {
                        if (defenderTypes.Contains(effectiveType))
                        {
                            multiplier *= 2.0; // Augmente les dégâts si le type est efficace
                        }
                    }
                }
            }

            return multiplier;
        }

        public List<string> GetPokemonTypes(int pokemonId)
        {
            if (PokemonTypes.ContainsKey(pokemonId))
            {
                return PokemonTypes[pokemonId]; // Retourne les types du Pokémon
            }
            else
            {
                // Si l'ID du Pokémon n'existe pas, renvoie une liste vide
                MessageBox.Show($"ERREUR - Aucun type trouvé pour le Pokémon ID {pokemonId}");
                return new List<string>();
            }
        }
    }
}
