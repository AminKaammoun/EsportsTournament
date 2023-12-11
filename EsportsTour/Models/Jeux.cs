using Projet.Net.Models;

namespace EsportsTour.Models
{
    public class Jeux
    {
        public int Id { get; set; }

        public string NomJeu { get; set; }

        public string Categorie { get; set; }
        public virtual ICollection<Tournoi> Tournois { get; set; } = new List<Tournoi>();


        public string ImgJeu { get; set; }
    }
}
