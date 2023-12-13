using Projet.Net.Models;

namespace EsportsTour
{
    public class JeuViewModel
    {
        public int Id { get; set; }

        public string NomJeu { get; set; }

        public string Categorie { get; set; }
        public virtual ICollection<Tournoi> Tournois { get; set; } = new List<Tournoi>();


        public IFormFile fileImage { get; set; }
    }
}
