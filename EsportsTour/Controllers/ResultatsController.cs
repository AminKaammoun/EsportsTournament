using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsportsTour.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Net.Models;

namespace Projet.Net.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ResultatsController : Controller
    {

        private readonly EsportsDbContext _context;

        public ResultatsController(EsportsDbContext context)
        {
            _context = context;
        }

        // GET: Resultats
        public async Task<IActionResult> Index()
        {
            var iitgamingContext = _context.Resultats.Include(r => r.EquipeGagnante).Include(r => r.EquipePerdante).Include(r => r.Tournoi);
            return View(await iitgamingContext.ToListAsync());
        }

        // GET: Resultats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Resultats == null)
            {
                return NotFound();
            }

            var resultat = await _context.Resultats
                .Include(r => r.EquipeGagnante)
                .Include(r => r.EquipePerdante)
                .Include(r => r.Tournoi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat == null)
            {
                return NotFound();
            }

            return View(resultat);
        }

        // GET: Resultats/Create
        public IActionResult Create()
        {
            ViewData["EquipeGagnanteId"] = new SelectList(_context.Equipes, "Id", "NomEquipe");
            ViewData["EquipePerdanteId"] = new SelectList(_context.Equipes, "Id", "NomEquipe");
            ViewData["TournoiId"] = new SelectList(_context.Tournois, "Id", "Nom");
            return View();
        }

        // POST: Resultats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TournoiId,EquipeGagnanteId,EquipePerdanteId,ScoreGagnant,ScorePerdant,DateMatch")] Resultat resultat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resultat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipeGagnanteId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", resultat.EquipeGagnanteId);
            ViewData["EquipePerdanteId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", resultat.EquipePerdanteId);
            ViewData["TournoiId"] = new SelectList(_context.Tournois, "Id", "Nom", resultat.TournoiId);
            return View(resultat);
        }

        // GET: Resultats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Resultats == null)
            {
                return NotFound();
            }

            var resultat = await _context.Resultats.FindAsync(id);
            if (resultat == null)
            {
                return NotFound();
            }
            ViewData["EquipeGagnanteId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", resultat.EquipeGagnanteId);
            ViewData["EquipePerdanteId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", resultat.EquipePerdanteId);
            ViewData["TournoiId"] = new SelectList(_context.Tournois, "Id", "Nom", resultat.TournoiId);
            return View(resultat);
        }

        // POST: Resultats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,TournoiId,EquipeGagnanteId,EquipePerdanteId,ScoreGagnant,ScorePerdant,DateMatch")] Resultat resultat)
        {
            if (id == null || resultat.Id != id)
            {
                return NotFound();
            }

            var existingResultat = await _context.Resultats.FindAsync(id);

            if (existingResultat == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the existing Resultat with the properties of the incoming resultat
                    _context.Entry(existingResultat).CurrentValues.SetValues(resultat);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultatExists(resultat.Id))
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

            // If ModelState is not valid, set up ViewData for the dropdowns and return to the view
            ViewData["EquipeGagnanteId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", resultat.EquipeGagnanteId);
            ViewData["EquipePerdanteId"] = new SelectList(_context.Equipes, "Id", "NomEquipe", resultat.EquipePerdanteId);
            ViewData["TournoiId"] = new SelectList(_context.Tournois, "Id", "Nom", resultat.TournoiId);
            return View(resultat);
        }

        // GET: Resultats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Resultats == null)
            {
                return NotFound();
            }

            var resultat = await _context.Resultats
                .Include(r => r.EquipeGagnante)
                .Include(r => r.EquipePerdante)
                .Include(r => r.Tournoi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat == null)
            {
                return NotFound();
            }

            return View(resultat);
        }

        // POST: Resultats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Resultats == null)
            {
                return Problem("Entity set 'IitgamingContext.Resultats'  is null.");
            }
            var resultat = await _context.Resultats.FindAsync(id);
            if (resultat != null)
            {
                _context.Resultats.Remove(resultat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultatExists(int id)
        {
          return (_context.Resultats?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
