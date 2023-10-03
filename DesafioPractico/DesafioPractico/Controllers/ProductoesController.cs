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
    public class ProductoesController : Controller
    {
        private readonly CrudBddContext _context;

        public ProductoesController(CrudBddContext context)
        {
            _context = context;
        }

        // GET: Productoes
        public async Task<IActionResult> Index()
        {
            var crudBddContext = _context.Productos.Include(p => p.IdFamiliaNavigation).Include(p => p.IdMarcaNavigation);
            return View(await crudBddContext.ToListAsync());
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdFamiliaNavigation)
                .Include(p => p.IdMarcaNavigation)
                .FirstOrDefaultAsync(m => m.CodigoProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewData["IdFamilia"] = new SelectList(_context.Familia, "Id", "Id");
            ViewData["IdMarca"] = new SelectList(_context.Marcas, "Id", "Id");
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodigoProducto,DescripcionProducto,PrecioCosto,PrecioVenta,IdMarca,IdFamilia,FechaModificacion,Baja,FechaBaja")] Producto producto)
        {

            _context.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["IdFamilia"] = new SelectList(_context.Familia, "Id", "Id", producto.IdFamilia);
            ViewData["IdMarca"] = new SelectList(_context.Marcas, "Id", "Id", producto.IdMarca);
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdFamilia"] = new SelectList(_context.Familia, "Id", "Id", producto.IdFamilia);
            ViewData["IdMarca"] = new SelectList(_context.Marcas, "Id", "Id", producto.IdMarca);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CodigoProducto,DescripcionProducto,PrecioCosto,PrecioVenta,IdMarca,IdFamilia,FechaModificacion,Baja,FechaBaja")] Producto producto)
        {
            if (id != producto.CodigoProducto)
            {
                return NotFound();
            }

            try
            {
               
                var originalProducto = await _context.Productos.FindAsync(id);

                if (originalProducto == null)
                {
                    return NotFound();
                }

              
                producto.Baja = originalProducto.Baja;
                producto.FechaBaja = originalProducto.FechaBaja;

                producto.FechaModificacion = DateTime.Now;
                _context.Entry(originalProducto).CurrentValues.SetValues(producto);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.CodigoProducto))
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


        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdFamiliaNavigation)
                .Include(p => p.IdMarcaNavigation)
                .FirstOrDefaultAsync(m => m.CodigoProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'CrudBddContext.Productos' is null.");
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            producto.Baja = true;
            producto.FechaBaja = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool ProductoExists(string id)
        {
            return (_context.Productos?.Any(e => e.CodigoProducto == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Listado(string codigoProducto, int? idMarca, int? idFamilia)
        {
            var productos = _context.Productos
                .Where(p =>
                    (string.IsNullOrEmpty(codigoProducto) || p.CodigoProducto.Contains(codigoProducto)) &&
                    (!idMarca.HasValue || p.IdMarca == idMarca) &&
                    (!idFamilia.HasValue || p.IdFamilia == idFamilia) &&
                    p.Baja == null
                )
                .OrderBy(p => p.FechaModificacion)
                .ToList();
           
            return View("Index", productos.AsEnumerable());
        }

    }
}
