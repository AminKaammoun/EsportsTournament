using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

namespace Projet.Net.Models;


public partial class Joueur
{
    public int Id { get; set; }

    public string Pseudonyme { get; set; } = null!;

    public DateTime? DateNaissance { get; set; }

    public int? EquipeId { get; set; }

    public virtual Equipe? Equipe { get; set; }
}
