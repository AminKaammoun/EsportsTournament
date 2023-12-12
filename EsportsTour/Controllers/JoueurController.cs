using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Net.Models;
using EsportsTour.Data;
using Microsoft.AspNetCore.Authorization;

namespace Projet.Net.Controllers
{
    [Authorize]
    public class JoueurController : Controller
    {
      
        private readonly EsportsDbContext _context;

        public JoueurController(EsportsDbContext context)
        {
            _context = context;
        }

        // GET: Joueur
        public async Task<IActionResult> Index()
        {
            var iitgamingContext = _context.Joueurs.Include(j => j.Equipe);
            return View(await iitgamingContext.ToListAsync());
        }

        // GET: Joueur/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Joueurs == null)
            {
                return NotFound();
            }

            var joueur = await _context.Joueurs
                .Include(j => j.Equipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joueur == null)
            {
                return NotFound();
            }

            return View(joueur);
        }

        // GET: Joueur/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Populate EquipeId with the Equipe names
            ViewData["EquipeId"] = new SelectList(_context.Equipes, "Id", "NomEquipe");
       
            return View();
        }

        // POST: Joueur/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Pseudonyme,DateNaissance,EquipeId")] Joueur joueur)
        {
            if (ModelState.IsValid)
            {
                // Check if a player with the same pseudonyme already exists
                if (_context.Joueurs.Any(e => e.Pseudonyme == joueur.Pseudonyme))
                {
                    ModelState.AddModelError("Pseudonyme", "A player with this pseudonyme already exists.");
                    ViewData["EquipeId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", joueur.EquipeId);
                  
                    return View(joueur);
                }

                // Remove the manual assignment of EquipeId
                _context.Add(joueur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Use EquipeName instead of EquipeId for SelectList
            ViewData["EquipeId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", joueur.EquipeId);
        
            return View(joueur);
        }


        // GET: Joueur/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Joueurs == null)
            {
                return NotFound();
            }

            var joueur = await _context.Joueurs.FindAsync(id);
            if (joueur == null)
            {
                return NotFound();
            }

            // Use EquipeName instead of EquipeId for SelectList
            ViewData["EquipeId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", joueur.EquipeId);
            return View(joueur);
        }

        // POST: Joueur/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Pseudonyme,DateNaissance,EquipeId")] Joueur joueur)
        {
            if (id != joueur.Id)
            {
                return NotFound();
            }

            // Retrieve the existing player including its Equipe
            var existingJoueur = await _context.Joueurs.Include(j => j.Equipe).FirstOrDefaultAsync(m => m.Id == id);
            if (existingJoueur == null)
            {
                return NotFound();
            }

            // Check if another player with the same pseudonyme exists
            var playerWithSamePseudonyme = await _context.Joueurs
                .Where(j => j.Id != id && j.Pseudonyme == joueur.Pseudonyme)
                .FirstOrDefaultAsync();

            if (playerWithSamePseudonyme != null)
            {
                ModelState.AddModelError(nameof(Joueur.Pseudonyme), "A player with the same pseudonyme already exists.");
            }
            else
            {
                // Update the properties of the existing player
                existingJoueur.Pseudonyme = joueur.Pseudonyme;
                existingJoueur.DateNaissance = joueur.DateNaissance;
                existingJoueur.EquipeId = joueur.EquipeId;

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(existingJoueur);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!JoueurExists(existingJoueur.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            // Use EquipeName instead of EquipeId for SelectList
            ViewData["EquipeId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", existingJoueur.EquipeId);
            return View(existingJoueur);
        }

        // GET: Joueur/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Joueurs == null)
            {
                return NotFound();
            }

            var joueur = await _context.Joueurs
                .Include(j => j.Equipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joueur == null)
            {
                return NotFound();
            }

            return View(joueur);
        }

        // POST: Joueur/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Joueurs == null)
            {
                return Problem("Entity set 'IitgamingContext.Joueurs'  is null.");
            }

            var joueur = await _context.Joueurs.FindAsync(id);
            if (joueur != null)
            {
                _context.Joueurs.Remove(joueur);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JoueurExists(int id)
        {
            return (_context.Joueurs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
