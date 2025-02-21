using TP3console.Models.EntityFramework;

namespace TP3console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new FilmsDbContext())
            {
                // Requête SELECT.
                Film titanic = ctx.Films.First(f => f.Nom.Contains("Titanic"));

                // Modification de l'entité (dans le contexte seulement).
                titanic.Description = "Un bateau échoué. Date : " + DateTime.Now;

                // Sauvegarde du contexte => Application de la modification dans la BD.
                int nbchanges = ctx.SaveChanges();

                Console.WriteLine("Nombre d'enregistrements modifiés ou ajoutés : " + nbchanges);
            }
        }
    }
}
