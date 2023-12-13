using Projet.Net.Models;
using System.ComponentModel.DataAnnotations;

namespace EsportsTour
{
    public class EquipeViewModel

    {
        [Key]
        public int Id { get; set; }

        public string NomEquipe { get; set; } = null!;

        public IFormFile imageFile { get; set; }

        public virtual ICollection<Joueur> Joueurs { get; set; } = new List<Joueur>();

        public virtual ICollection<Resultat> ResultatEquipeGagnantes { get; set; } = new List<Resultat>();

        public virtual ICollection<Resultat> ResultatEquipePerdantes { get; set; } = new List<Resultat>();
    }
}
