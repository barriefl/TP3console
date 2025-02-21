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

            Exo3Q1();
            Exo3Q2();
            Exo3Q3();
            Exo3Q4();
            Exo3Q5();
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

        public static void Exo3Q1()
        {
            var ctx = new FilmsDbContext();
            // Ajoutez - vous en tant qu’utilisateur.
            Utilisateur utilisateur = new Utilisateur
            {
                Login = "barriefl",
                Email = "Florian.Barrier@etu.univ-smb.fr",
                Pwd = "mdp"
            };
            ctx.Add(utilisateur);
            ctx.SaveChanges();
            Console.WriteLine("Nouvel utilisateur ajouté.");
        }

        public static void Exo3Q2()
        {
            var ctx = new FilmsDbContext();
            // Modifier un film.
            var film = ctx.Films.FirstOrDefault(f => f.Nom.ToLower() == "l'armee des douze singes".ToLower());
            if (film != null)
            {
                film.Description = "Film où il y a 12 singes qui font une armée.";

                var categorieDrame = ctx.Categories.FirstOrDefault(c => c.Nom.ToLower() == "drame".ToLower());
                if (categorieDrame != null)
                {
                    film.Idcategorie = categorieDrame.Idcategorie;
                    ctx.SaveChanges();
                    Console.WriteLine("Mise à jour du film réussie.");
                }
                else
                {
                    Console.WriteLine("La catégorie drame n'existe pas.");
                }            
            }
            else
            {
                Console.WriteLine("Le film l'armée des douze singes n'a pas été trouvée.");
            }  
        }

        public static void Exo3Q3()
        {
            var ctx = new FilmsDbContext();
            // Supprimer un film.
            var film = ctx.Films.FirstOrDefault(f => f.Nom.ToLower() == "l'armee des douze singes".ToLower());
            if (film != null) 
            {
                var avisAssocies = ctx.Avis.Where(a => a.Idfilm == film.Idfilm).ToList();
                ctx.Avis.RemoveRange(avisAssocies);
                ctx.Films.Remove(film);
                ctx.SaveChanges();
                Console.WriteLine("Le film et ses avis ont été supprimés.");
            }
            else
            {
                Console.WriteLine("Le film l'armée des douze singes n'a pas été trouvée.");
            }
        }

        public static void Exo3Q4()
        {
            var ctx = new FilmsDbContext();
            // Ajouter un avis.
            var film = ctx.Films.FirstOrDefault(f => f.Nom.ToLower() == "titanic".ToLower());
            var monAvis = new Avi()
            {
                Idfilm = film.Idfilm,
                Idutilisateur = 1,
                Commentaire = "Nicolas c'est une petite coquine.",
                Note = 0
            };
            ctx.Avis.Add(monAvis);
            ctx.SaveChanges();
            Console.WriteLine("Nouvel avis ajouté.");
        }

        public static void Exo3Q5() 
        {
            var ctx = new FilmsDbContext();
            // Ajouter 2 films dans la catégorie « Drame ».
            var categorieDrame = ctx.Categories.FirstOrDefault(c => c.Nom.ToLower() == "drame".ToLower());
            var nouveauxFilms = new List<Film>
            {
                new Film 
                { 
                    Idfilm = 50,
                    Nom = "Le Chemin du Destin", 
                    Description = "Un drame poignant sur le destin.", 
                    Idcategorie = categorieDrame.Idcategorie 
                },
                new Film 
                {
                    Idfilm = 51,
                    Nom = "Les Larmes de l'Horizon", 
                    Description = "Un film émouvant sur l'espoir et la perte.", 
                    Idcategorie = categorieDrame.Idcategorie 
                }
            };
            ctx.Films.AddRange(nouveauxFilms);
            ctx.SaveChanges();
            Console.WriteLine("Les nouveaux films ont été ajoutés à la catégorie 'Drame'.");
        }
    }
}
