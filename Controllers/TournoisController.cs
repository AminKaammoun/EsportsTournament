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
              return _context.Tournois != null ? 
                          View(await _context.Tournois.ToListAsync()) :
                          Problem("Entity set 'IitgamingContext.Tournois'  is null.");
        }

        // GET: Tournois/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tournois == null)
            {
                return NotFound();
            }

            var tournoi = await _context.Tournois
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
            return View();
        }

        // POST: Tournois/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Descr,Jeu,DateDebut,DateFin")] Tournoi tournoi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tournoi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tournoi);
        }

        // GET: Tournois/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tournois == null)
            {
                return NotFound();
            }

            var tournoi = await _context.Tournois.FindAsync(id);
            if (tournoi == null)
            {
                return NotFound();
            }
            return View(tournoi);
        }

        // POST: Tournois/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nom,Descr,Jeu,DateDebut,DateFin")] Tournoi tournoi)
        {
            if (id != tournoi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournoi);
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
                return Problem("Entity set 'IitgamingContext.Tournois'  is null.");
            }
            var tournoi = await _context.Tournois.FindAsync(id);
            if (tournoi != null)
            {
                _context.Tournois.Remove(tournoi);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TournoiExists(int id)
        {
          return (_context.Tournois?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
