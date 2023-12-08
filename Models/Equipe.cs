using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

namespace Projet.Net.Models;


public partial class Equipe
{
    public int Id { get; set; }

    public string NomEquipe { get; set; } = null!;

    public virtual ICollection<Joueur> Joueurs { get; set; } = new List<Joueur>();

    public virtual ICollection<Resultat> ResultatEquipeGagnantes { get; set; } = new List<Resultat>();

    public virtual ICollection<Resultat> ResultatEquipePerdantes { get; set; } = new List<Resultat>();
}
