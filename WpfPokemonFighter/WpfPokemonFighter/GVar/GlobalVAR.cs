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
            var service = new ExerciceMonsterContext();
            return !service.Monsters.Any();
        }

        public void CleanData()
        {
            using (SqlConnection connection = new SqlConnection("Server=MSI\\SQLEXPRESS;Database=ExerciceMonster;Trusted_Connection=True; TrustServerCertificate=True;"))
            {
                connection.Open();

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
            //Family Nidoran♀
            service.AddPokemon("Nidoran♀", 55, "https://img.pokemondb.net/sprites/black-white/normal/nidoran-f.png", "Scratch", "Poison Sting");
            service.AddPokemon("Nidorina", 70, "https://img.pokemondb.net/sprites/black-white/normal/nidorina.png", "Earth Power", "Poison Sting");
            service.AddPokemon("Nidoqueen", 90, "https://img.pokemondb.net/sprites/black-white/normal/nidoqueen.png", "Earth Power", "Sludge Wave");
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

        public int TestEvolution(int Random)
        {
            MainPageVM Main = new MainPageVM();
            // Not Enought Good For Evolution
            //Family Bulbasaur
            if (Random == 2 && Main.VictoryCount < 4) {
                Random = 1;
                return Random;
            }
            if (Random == 3 && Main.VictoryCount < 4)
            {
                Random = 1;
                return Random;
            }
            if (Random == 3 && Main.VictoryCount > 4 && Main.VictoryCount < 9)
            {
                Random = 2;
                return Random;
            }
            if (Random == 1 && Main.VictoryCount > 4 && Main.VictoryCount < 9)
            {
                Random = 2;
                return Random;
            }
            if (Random == 1 && Main.VictoryCount > 9)
            {
                Random = 3;
                return Random;
            }
            if (Random == 2 && Main.VictoryCount > 9)
            {
                Random = 3;
                return Random;
            }
            //Family Charmander
            if (Random == 5 &&  Main.VictoryCount < 4 )
            {
                Random = 4;
                return Random;
            }
            if (Random == 6 && Main.VictoryCount < 4)
            {
                Random = 4;
                return Random;
            }
            if (Random == 6 && Main.VictoryCount < 4 && Main.VictoryCount < 9)
            {
                Random = 5;
                return Random;
            }
            if (Random == 4 && Main.VictoryCount > 4 && Main.VictoryCount < 9)
            {
                Random = 5;
                return Random;
            }
            if (Random == 4 && Main.VictoryCount > 9)
            {
                Random = 6;
                return Random;
            }
            if (Random == 5 && Main.VictoryCount > 9)
            {
                Random = 6;
                return Random;
            }
            //Family Squirtle
            if (Random == 8 && Main.VictoryCount < 4)
            {
                Random = 7;
                return Random;
            }
            if (Random == 9 && Main.VictoryCount < 4)
            {
                Random = 7;
                return Random;
            }
            if (Random == 9 && Main.VictoryCount < 4 && Main.VictoryCount < 9)
            {
                Random = 8;
                return Random;
            }
            if (Random == 7 && Main.VictoryCount > 4 && Main.VictoryCount < 9)
            {
                Random = 8;
                return Random;
            }
            if (Random == 7 && Main.VictoryCount > 9)
            {
                Random = 9;
                return Random;
            }
            if (Random == 8 && Main.VictoryCount > 9)
            {
                Random = 9;
                return Random;
            }
            //Family Pikachu
            if (Random == 11 && Main.VictoryCount < 4)
            {
                Random = 10;
                return Random;
            }
            if (Random == 10 && Main.VictoryCount > 4)
            {
                Random = 11;
                return Random;
            }
            //Family Pidgey
            if (Random == 13 && Main.VictoryCount < 4)
            {
                Random = 12;
                return Random;
            }
            if (Random == 14 && Main.VictoryCount < 4)
            {
                Random = 12;
                return Random;
            }
            if (Random == 14 && Main.VictoryCount < 4 && Main.VictoryCount < 9)
            {
                Random = 13;
                return Random;
            }
            if (Random == 12 && Main.VictoryCount > 4 && Main.VictoryCount < 9)
            {
                Random = 13;
                return Random;
            }
            if (Random == 12 && Main.VictoryCount > 9)
            {
                Random = 14;
                return Random;
            }
            if (Random == 13 && Main.VictoryCount > 9)
            {
                Random = 14;
                return Random;
            }
            //Family Nidoran♂
            if (Random == 16 && Main.VictoryCount < 4)
            {
                Random = 15;
                return Random;
            }
            if (Random == 17 && Main.VictoryCount < 4)
            {
                Random = 15;
                return Random;
            }
            if (Random == 17 && Main.VictoryCount < 4 && Main.VictoryCount < 9)
            {
                Random = 16;
                return Random;
            }
            if (Random == 15 && Main.VictoryCount > 4 && Main.VictoryCount < 9)
            {
                Random = 16;
                return Random;
            }
            if (Random == 15 && Main.VictoryCount > 9)
            {
                Random = 17;
                return Random;
            }
            if (Random == 16 && Main.VictoryCount > 9)
            {
                Random = 17;
                return Random;
            }
            //Family Nidoran♀
            if (Random == 19 && Main.VictoryCount < 4)
            {
                Random = 18;
                return Random;
            }
            if (Random == 20 && Main.VictoryCount < 4)
            {
                Random = 18;
                return Random;
            }
            if (Random == 20 && Main.VictoryCount < 4 && Main.VictoryCount < 9)
            {
                Random = 19;
                return Random;
            }
            if (Random == 18 && Main.VictoryCount > 4 && Main.VictoryCount < 9)
            {
                Random = 19;
                return Random;
            }
            if (Random == 18 && Main.VictoryCount > 9)
            {
                Random = 20;
                return Random;
            }
            if (Random == 19 && Main.VictoryCount > 9)
            {
                Random = 20;
                return Random;
            }
            //Family Vulpix
            if (Random == 22 && Main.VictoryCount < 4)
            {
                Random = 21;
                return Random;
            }
            if (Random == 21 && Main.VictoryCount > 4)
            {
                Random = 22;
                return Random;
            }
            //Family Magikarp
            if (Random == 24 && Main.VictoryCount < 4)
            {
                Random = 23;
                return Random;
            }
            if (Random == 23 && Main.VictoryCount > 4)
            {
                Random = 24;
                return Random;
            }

            // Only if all test are not good
            return Random;
        }
    }
}
