using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

namespace Projet.Net.Models;


public partial class Resultat
{
    public int Id { get; set; }

    public int? TournoiId { get; set; }

    public int? EquipeGagnanteId { get; set; }

    public int? EquipePerdanteId { get; set; }

    public int? ScoreGagnant { get; set; }

    public int? ScorePerdant { get; set; }

    public DateTime? DateMatch { get; set; }

    public virtual Equipe? EquipeGagnante { get; set; }

    public virtual Equipe? EquipePerdante { get; set; }

    public virtual Tournoi? Tournoi { get; set; }
}
