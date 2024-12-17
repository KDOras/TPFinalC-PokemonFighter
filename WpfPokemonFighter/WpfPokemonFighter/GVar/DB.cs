using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCours.Model;

namespace WpfCours.MVVM.ViewModel
{
    public static class DbContextManager
    {
        private static ExerciceMonsterContext _context;

        public static ExerciceMonsterContext GetDbContext()
        {
            if (_context == null)
            {
                // Assurez-vous que la chaîne de connexion est correctement récupérée ici
                string connectionString = SharedService.Instance.DataBase;
                var options = new DbContextOptionsBuilder<ExerciceMonsterContext>()
                    .UseSqlServer(connectionString)
                    .Options;
                _context = new ExerciceMonsterContext(options);
            }
            return _context;
        }


        // Optionnel : Méthode pour réinitialiser le contexte si nécessaire
        public static void ResetDbContext()
        {
            _context = null;
        }

        public static void UpdateDbContext()
        {
            if (string.IsNullOrEmpty(SharedService.Instance.DataBase))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ExerciceMonsterContext>();
            optionsBuilder.UseSqlServer(SharedService.Instance.DataBase);

            // Recrée le contexte
            _context = new ExerciceMonsterContext(optionsBuilder.Options);
        }
    }
}
