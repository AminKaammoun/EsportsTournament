using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsportsTour.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Net.Models;

namespace Projet.Net.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TournoisController : Controller
    {
        private readonly EsportsDbContext _context;

        public TournoisController(EsportsDbContext context)
        {
            _context = context;
        }

        // GET: Tournois
        public async Task<IActionResult> Index()
        {
            


            var iitgamingContext = _context.Tournois.Include(j => j.Jeux);
            return View(await iitgamingContext.ToListAsync());
        }

        // GET: Tournois/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tournois == null)
            {
                return NotFound();
            }

            var tournoi = await _context.Tournois
        .Include(t => t.Jeux) // Include the Jeu navigation property
        .FirstOrDefaultAsync(m => m.Id == id);
            if (tournoi == null)
            {
                return NotFound();
            }

            return View(tournoi);
        }

        // GET: Tournois/Create
        public IActionResult Create()
        {
            ViewData["JeuId"] = new SelectList(_context.Jeux, "Id", "NomJeu"); // Add this line

            return View();
        }

        // POST: Tournois/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Descr,JeuId,DateDebut,DateFin")] Tournoi tournoi)
        {
            if (ModelState.IsValid)
            {
                if (_context.Tournois.Any(e => e.Nom == tournoi.Nom))
                {
                    ModelState.AddModelError("Nom", "A tournament with this name already exists.");
                    ViewData["JeuId"] = new SelectList(_context.Tournois, "Id", "NomJeu", tournoi.JeuId);
                    return View(tournoi);
                }

                // Check if DateDebut is greater than or equal to DateFin
                if (tournoi.DateDebut >= tournoi.DateFin)
                {
                    ModelState.AddModelError("DateDebut", "Start date must be before the end date.");
                    ViewData["JeuId"] = new SelectList(_context.Jeux, "Id", "NomJeu", tournoi.JeuId);
                    return View(tournoi);
                }

                _context.Add(tournoi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["JeuId"] = new SelectList(_context.Jeux, "Id", "NomJeu", tournoi.JeuId);
            return View(tournoi);
        }

        // GET: Tournois/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tournois == null)
            {
                return NotFound();
            }

            var tournoi = await _context.Tournois.Include(t => t.Jeux) // Include the Jeu navigation property
            .FirstOrDefaultAsync(m => m.Id == id);
            if (tournoi == null)
            {
                return NotFound();
            }
            ViewData["JeuId"] = new SelectList(_context.Jeux, "Id", "NomJeu", tournoi.JeuId); // Add this line

            return View(tournoi);
        }

        // POST: Tournois/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Nom,Descr,JeuId,DateDebut,DateFin")] Tournoi tournoi)
        {
            if (id == null || tournoi.Id != id)
            {
                return NotFound();
            }

            var existingTournois = await _context.Tournois.FindAsync(id);

            if (existingTournois == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the existing Tournois with the properties of the incoming tournoi
                    _context.Entry(existingTournois).CurrentValues.SetValues(tournoi);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournoiExists(tournoi.Id))
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

            // If ModelState is not valid, return to the view
            ViewData["JeuId"] = new SelectList(_context.Jeux, "Id", "NomJeu", tournoi.JeuId); // Add this line
            return View(tournoi);
        }

        // GET: Tournois/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tournois == null)
            {
                return NotFound();
            }

            var tournoi = await _context.Tournois
         .Include(t => t.Jeux) // Include the Jeu navigation property
         .FirstOrDefaultAsync(m => m.Id == id);
            if (tournoi == null)
            {
                return NotFound();
            }

            return View(tournoi);
        }

        // POST: Tournois/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tournois == null)
            {
                return Problem("Entity set 'IitgamingContext.Tournois' is null.");
            }

            var tournoi = await _context.Tournois.FindAsync(id);

            if (tournoi == null)
            {
                return NotFound();
            }

            // Check for associated Resultats
            bool hasResultats = _context.Resultats.Any(r => r.TournoiId == id);

            if (hasResultats)
            {
                ModelState.AddModelError(string.Empty, "Impossible de supprimer ce Tournois car il y a des Resultats associés.");
                return View(tournoi);
            }

            _context.Tournois.Remove(tournoi);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TournoiExists(int id)
        {
          return (_context.Tournois?.Any(e => e.Id == id)).GetValueOrDefault();
        }
       
    }
}
