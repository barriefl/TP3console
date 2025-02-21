using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using System.Text;
using TP3console.Models.EntityFramework;

namespace TP3console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Exo2Q1();
            Exo2Q2();
            Exo2Q3();
            Exo2Q4();
            Exo2Q5();
            Exo2Q6();
            Exo2Q7();
            Exo2Q8();
            Exo2Q9();
            Console.ReadKey();
        }

        public static void Exo2Q1()
        {
            var ctx = new FilmsDbContext();
            foreach (var film in ctx.Films)
            {
                Console.WriteLine(film.ToString());
            }
            
            // Afficher les noms et id des films de la catégorie « Action ».
            foreach (var film in ctx.Films)
            {
                ctx.Films.Where(f => f.Idcategorie == 2);
                Console.WriteLine($"ID : {film.Idfilm}, Nom : {film.Nom}");
            }
        }

        //Autre possibilité :
        public static void Exo2Q1Bis()
        {
            var ctx = new FilmsDbContext();
            //Pour que cela marche, il faut que la requête envoie les mêmes noms de colonnes que les classes c#.
            var films = ctx.Films.FromSqlRaw("SELECT * FROM film");
            foreach (var film in films)
            {
                Console.WriteLine(film.ToString());
            }
        }

        public static void Exo2Q2()
        {
            var ctx = new FilmsDbContext();
            // Afficher les emails de tous les utilisateurs.
            foreach (var utilisateur in ctx.Utilisateurs)
            {
                Console.WriteLine($"Email : {utilisateur.Email.ToString()}");
            }
        }

        public static void Exo2Q3()
        {
            var ctx = new FilmsDbContext();
            var utilisateurs = ctx.Utilisateurs.OrderBy(u => u.Login);
            // Afficher tous les utilisateurs triés par login croissant.
            foreach (var utilisateur in utilisateurs)
            {
                Console.WriteLine($"Login : {utilisateur.Login.ToString()}");
            }
        }

        public static void Exo2Q4()
        {
            var ctx = new FilmsDbContext();
            var filmsAction = ctx.Films
                .Include(f => f.IdcategorieNavigation) // Charger la propriété sinon NullException.
                .Where(f => f.IdcategorieNavigation.Nom == "Action");
            // Afficher les noms et id des films de la catégorie « Action ».
            foreach (var film in filmsAction)
            {
                Console.WriteLine($"ID : {film.Idfilm}, Nom : {film.Nom}, Catégorie {film.IdcategorieNavigation.Nom}");
            }
        }

        public static void Exo2Q5()
        {
            var ctx = new FilmsDbContext();
            var nbCategories = ctx.Categories.Count();
            // Afficher le nombre de catégories.
            Console.WriteLine($"Nombre de catégories : {nbCategories}");
        }

        public static void Exo2Q6()
        {
            var ctx = new FilmsDbContext();
            var pireNote = ctx.Avis.Min(a => a.Note);
            // Afficher la note la plus basse dans la base.
            Console.WriteLine($"Note la plus basse : {pireNote}");
        }

        public static void Exo2Q7()
        {
            var ctx = new FilmsDbContext();
            var filmsCommencentParLe = ctx.Films.Where(f => f.Nom.StartsWith("Le"));
            // Rechercher tous les films qui commencent par « le ».
            foreach (var film in filmsCommencentParLe) 
            {
                Console.WriteLine($"Film commançant par « le » : {film}");
            }         
        }

        public static void Exo2Q8()
        {
            var ctx = new FilmsDbContext();
            var moyennePulpFiction = ctx.Films
                .Where(f => f.Nom.ToLower() == "pulp fiction".ToLower()) // (Note : le nom du film ne devra pas être sensible à la casse).
                .SelectMany(f => f.Avis)
                .Average(a => a.Note);
            // Rechercher tous les films qui commencent par « le ».
            Console.WriteLine($"Note moyenne de Pulp Fiction : {moyennePulpFiction}");
        }

        public static void Exo2Q9()
        {
            var ctx = new FilmsDbContext();
            var utilisateurMeilleureNote = ctx.Avis
                .Join(ctx.Utilisateurs, a => a.Idutilisateur, u => u.Idutilisateur, (a, u) => new { u, a.Note })
                .OrderByDescending(x => x.Note)
                .FirstOrDefault();
            // Afficher l’utilisateur qui a mis la meilleure note dans la base.
            Console.WriteLine($"Utilisateur ayant mis la meilleure note : {utilisateurMeilleureNote}");
        }
    }
}
