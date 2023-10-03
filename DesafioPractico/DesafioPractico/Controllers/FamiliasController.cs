using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DesafioPractico.Models;

namespace DesafioPractico.Controllers
{
    public class FamiliasController : Controller
    {
        private readonly CrudBddContext _context;

        public FamiliasController(CrudBddContext context)
        {
            _context = context;
        }

        // GET: Familias
        public async Task<IActionResult> Index()
        {
              return _context.Familia != null ? 
                          View(await _context.Familia.ToListAsync()) :
                          Problem("Entity set 'CrudBddContext.Familia'  is null.");
        }

        // GET: Familias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Familia == null)
            {
                return NotFound();
            }

            var familia = await _context.Familia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (familia == null)
            {
                return NotFound();
            }

            return View(familia);
        }

        // GET: Familias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Familias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,FechaModificacion,Baja,FechaBaja")] Familia familia)
        {
            if (ModelState.IsValid)
            {
                familia.FechaModificacion = DateTime.Now;
                _context.Add(familia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(familia);
        }

        // GET: Familias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Familia == null)
            {
                return NotFound();
            }

            var familia = await _context.Familia.FindAsync(id);
            if (familia == null)
            {
                return NotFound();
            }
            return View(familia);
        }

        // POST: Familias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,FechaModificacion,Baja,FechaBaja")] Familia familia)
        {
            if (id != familia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
               
                    var existingFamilia = await _context.Familia.FindAsync(id);

                    
                    if (existingFamilia.Descripcion != familia.Descripcion)
                    {
                        
                        existingFamilia.Descripcion = familia.Descripcion;
                        existingFamilia.FechaModificacion = DateTime.Now;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamiliaExists(familia.Id))
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
            return View(familia);
        }



        // GET: Familias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Familia == null)
            {
                return NotFound();
            }

            var familia = await _context.Familia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (familia == null)
            {
                return NotFound();
            }

            return View(familia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Familia == null)
            {
                return Problem("Entity set 'CrudBddContext.Familia' is null.");
            }

            var familia = await _context.Familia.FindAsync(id);
            if (familia == null)
            {
                return NotFound();
            }

           
            bool hasAssociatedProductsInBaja = _context.Productos.Any(p => p.IdFamilia == id && p.Baja == true);

            if (hasAssociatedProductsInBaja)
            {
            
                familia.Baja = true;

               
                familia.FechaBaja = DateTime.Now;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

 
            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult FamilyHasProducts()
        {
            return View();
        }



        private bool FamiliaExists(int id)
        {
          return (_context.Familia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
