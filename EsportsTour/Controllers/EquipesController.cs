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
using EsportsTour;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Projet.Net.Controllers
{
    [Authorize]
    public class EquipesController : Controller
    {
        private readonly EsportsDbContext _context;
        IWebHostEnvironment hostEnvironment;
        public EquipesController(EsportsDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: Equipes
        public async Task<IActionResult> Index()
        {
            return _context.Equipes != null ?
                        View(await _context.Equipes.ToListAsync()) :
                        Problem("Entity set 'IitgamingContext.Equipes'  is null.");
        }

        // GET: Equipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipes == null)
            {
                return NotFound();
            }

            var equipe = await _context.Equipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipe == null)
            {
                return NotFound();
            }

            return View(equipe);
        }

        // GET: Equipes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("NomEquipe","imageFile")] EquipeViewModel equipe)
        {
            if (ModelState.IsValid)
            {
                // Check if a team with the same name already exists
                if (_context.Equipes.Any(e => e.NomEquipe == equipe.NomEquipe))
                {
                    ModelState.AddModelError("NomEquipe", "Team with this name already exists.");
                    return View(equipe);
                }
                string filename = "";
                if (equipe.imageFile != null)
                {
                    string uploadfolder = Path.Combine(hostEnvironment.WebRootPath, "img");
                    filename= Guid.NewGuid().ToString()+ "_" + equipe.imageFile.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    equipe.imageFile.CopyTo(new FileStream(filepath, FileMode.Create));

                }
                Equipe e = new Equipe();
                e.NomEquipe = equipe.NomEquipe;
                e.image = filename;
                _context.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipe);
        }

        // GET: Equipes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Equipes == null)
            {
                return NotFound();
            }

            var equipe = await _context.Equipes.FindAsync(id);
            if (equipe == null)
            {
                return NotFound();
            }
            return View(equipe);
        }

        // POST: Equipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomEquipe")] Equipe equipe)
        {
            if (id != equipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    _context.Update(equipe);
                   
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipeExists(equipe.Id))
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
            return View(equipe);
        }

        // GET: Equipes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Equipes == null)
            {
                return NotFound();
            }

            var equipe = await _context.Equipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipe == null)
            {
                return NotFound();
            }

            return View(equipe);
        }

        // POST: Equipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Equipes == null)
            {
                return Problem("Entity set 'IitgamingContext.Equipes'  is null.");
            }
            var equipe = await _context.Equipes.FindAsync(id);
            // Check for associated Resultats
            bool hasjoueur = _context.Joueurs.Any(r => r.EquipeId == id);
            bool hasresultg = _context.Resultats.Any(r => r.EquipeGagnanteId == id);
            bool hasresultp = _context.Resultats.Any(r => r.EquipePerdanteId == id);


            if (hasjoueur)
            {
                ModelState.AddModelError(string.Empty, "Impossible de supprimer cette équipe car il y a des joueurs associés.");
                return View(equipe);
            }
            if (hasresultg)
            {
                ModelState.AddModelError(string.Empty, "Impossible de supprimer cette équipe car il y a des résultats associés.");
                return View(equipe);
            }
            if (hasresultp)
            {
                ModelState.AddModelError(string.Empty, "Impossible de supprimer cette équipe car il y a des résultats associés.");
                return View(equipe);
            }


            if (equipe != null)
            {
                _context.Equipes.Remove(equipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipeExists(int id)
        {
            return (_context.Equipes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
