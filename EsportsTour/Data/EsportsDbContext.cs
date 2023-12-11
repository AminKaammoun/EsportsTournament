using EsportsTour.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projet.Net.Models;

namespace EsportsTour.Data
{
    public partial class EsportsDbContext : IdentityDbContext
    {
        public EsportsDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Equipe> Equipes { get; set; }

        public virtual DbSet<Joueur> Joueurs { get; set; }

        public virtual DbSet<Resultat> Resultats { get; set; }

        public virtual DbSet<Tournoi> Tournois { get; set; }

        public virtual DbSet<Jeux> Jeux { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(iur => new { iur.UserId });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(iur => new { iur.UserId, iur.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(iut => new { iut.UserId, iut.LoginProvider, iut.Name });

            modelBuilder.Entity<Equipe>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Equipes__DC0A3743F019E1C7");

                entity.Property(e => e.NomEquipe).HasMaxLength(255);
            });

            modelBuilder.Entity<Joueur>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Joueurs__D6CEE2407FCE1182");

                entity.Property(e => e.DateNaissance).HasColumnType("date");
                entity.Property(e => e.Pseudonyme).HasMaxLength(255);

                entity.HasOne(d => d.Equipe).WithMany(p => p.Joueurs)
                    .HasForeignKey(d => d.EquipeId)
                    .HasConstraintName("FK__Joueurs__EquipeI__3B75D760");
            });

            modelBuilder.Entity<Resultat>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Resultat__20BF3E6BBAD92AB0");

                entity.Property(e => e.DateMatch).HasColumnType("date");

                entity.HasOne(d => d.EquipeGagnante).WithMany(p => p.ResultatEquipeGagnantes)
                    .HasForeignKey(d => d.EquipeGagnanteId)
                    .HasConstraintName("FK__Resultats__Equip__3F466844");

                entity.HasOne(d => d.EquipePerdante).WithMany(p => p.ResultatEquipePerdantes)
                    .HasForeignKey(d => d.EquipePerdanteId)
                    .HasConstraintName("FK__Resultats__Equip__403A8C7D");

                entity.HasOne(d => d.Tournoi).WithMany(p => p.Resultats)
                    .HasForeignKey(d => d.TournoiId)
                    .HasConstraintName("FK__Resultats__Tourn__3E52440B");
            });

            modelBuilder.Entity<Tournoi>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Tournois__6536E3D9E8BDE8DD");

                entity.Property(e => e.DateDebut).HasColumnType("date");
                entity.Property(e => e.DateFin).HasColumnType("date");
                entity.Property(e => e.Descr).HasMaxLength(255);
                entity.HasOne(d => d.Jeux)
                    .WithMany(p => p.Tournois)
                    .HasForeignKey(d => d.JeuId)
                    .HasConstraintName("FK__Tournois__JeuId__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
