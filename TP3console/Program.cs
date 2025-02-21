using Microsoft.EntityFrameworkCore;
using TP3console.Models.EntityFramework;

namespace TP3console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new FilmsDbContext())
            {
                //Chargement de la catégorie Action
                Categorie categorieAction = ctx.Categories.First(c => c.Nom == "Action");
                Console.WriteLine("Categorie : " + categorieAction.Nom);
                Console.WriteLine("Films : ");

                //Chargement des films de la catégorie Action.
                foreach (var film in categorieAction.Films) // lazy loading initiated
                {
                    Console.WriteLine(film.Nom);
                }
            }
        }
    }
}
