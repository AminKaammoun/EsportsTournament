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
using EsportsTour;

namespace EsportsTour.Controllers
{
    [Authorize]
    public class JeuxController : Controller
    {
        private readonly EsportsDbContext _context;
        IWebHostEnvironment hostEnvironment;

        public JeuxController(EsportsDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Create([Bind("Id,NomJeu,Categorie,imageFile")] JeuViewModel jeux)
        {
            if (ModelState.IsValid)
            {
                if (_context.Jeux.Any(e => e.NomJeu == jeux.NomJeu))
                {
                    ModelState.AddModelError("NomJeu", "A game with this name already exists.");
                    return View(jeux);
                }
                string filename = "";
                if (jeux.imageFile != null)
                {
                    string uploadfolder = Path.Combine(hostEnvironment.WebRootPath, "img");
                    filename = Guid.NewGuid().ToString() + "_" + jeux.imageFile.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    jeux.imageFile.CopyTo(new FileStream(filepath, FileMode.Create));

                }
                Jeux j = new Jeux();
                j.NomJeu = jeux.NomJeu;
                j.Categorie = jeux.Categorie;
                j.ImgJeu = filename;
                _context.Add(j);
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

            var jeu = await _context.Jeux.FindAsync(id);
            if (jeu == null)
            {
                return NotFound();
            }
            var jeuViewModel = new JeuViewModel
            {
                Id = jeu.Id,
                NomJeu = jeu.NomJeu,
                Categorie = jeu.Categorie,

            };
            return View(jeuViewModel);
        }

        // POST: Jeux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomJeu,Categorie,imageFile")] JeuViewModel jeux)
        {
            if (id != jeux.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingJeu = await _context.Jeux.FindAsync(id);

                    if (existingJeu == null)
                    {
                        return NotFound();
                    }

                    if (_context.Jeux.Any(e => e.NomJeu == jeux.NomJeu && e.Id != id))
                    {
                        ModelState.AddModelError("NomJeu", "Jeu with this name already exists.");
                        return View(jeux);
                    }

                    existingJeu.NomJeu = jeux.NomJeu;

                    if (jeux.imageFile != null)
                    {
                        string uploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img");
                        string filename = Guid.NewGuid().ToString() + "_" + jeux.imageFile.FileName;
                        string filePath = Path.Combine(uploadFolder, filename);
                       jeux.imageFile.CopyTo(new FileStream(filePath, FileMode.Create));

                        existingJeu.ImgJeu = filename;
                    }

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
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
