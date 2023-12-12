using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EsportsTour.Data;
using EsportsTour.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Projet.Net.Models;

namespace EsportsTour.Controllers
{
    [Authorize]
    public class JeuxController : Controller
    {
        private readonly EsportsDbContext _context;

        public JeuxController(EsportsDbContext context)
        {
            _context = context;
        }

        // GET: Jeux
        public async Task<IActionResult> Index()
        {
              return _context.Jeux != null ? 
                          View(await _context.Jeux.ToListAsync()) :
                          Problem("Entity set 'EsportsDbContext.Jeux'  is null.");
        }

        // GET: Jeux/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jeux == null)
            {
                return NotFound();
            }

            var jeux = await _context.Jeux
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jeux == null)
            {
                return NotFound();
            }

            return View(jeux);
        }

        // GET: Jeux/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jeux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,NomJeu,Categorie,ImgJeu")] Jeux jeux)
        {
            if (ModelState.IsValid)
            {
                if (_context.Jeux.Any(e => e.NomJeu == jeux.NomJeu))
                {
                    ModelState.AddModelError("NomJeu", "A game with this name already exists.");
                    return View(jeux);
                }
                _context.Add(jeux);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jeux);
        }

        // GET: Jeux/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jeux == null)
            {
                return NotFound();
            }

            var jeux = await _context.Jeux.FindAsync(id);
            if (jeux == null)
            {
                return NotFound();
            }
            return View(jeux);
        }

        // POST: Jeux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomJeu,Categorie,ImgJeu")] Jeux jeux)
        {
            if (id != jeux.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jeux);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JeuxExists(jeux.Id))
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
            return View(jeux);
        }

        // GET: Jeux/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jeux == null)
            {
                return NotFound();
            }

            var jeux = await _context.Jeux
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jeux == null)
            {
                return NotFound();
            }

            return View(jeux);
        }

        // POST: Jeux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jeux == null)
            {
                return Problem("Entity set 'EsportsDbContext.Jeux'  is null.");
            }
            var jeux = await _context.Jeux.FindAsync(id);
            bool hasResultats = _context.Tournois.Any(r => r.JeuId == id);

            if (hasResultats)
            {
                ModelState.AddModelError(string.Empty, "Impossible de supprimer cette Jeux car il y a des Tournoi associés.");
                return View(jeux);
            }
            if (jeux != null)
            {
                _context.Jeux.Remove(jeux);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JeuxExists(int id)
        {
            return (_context.Jeux?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
