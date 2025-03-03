using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comercio.Pages
{
    public class ListarProductosModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public List<Producto> Productos { get; set; } = new List<Producto>();

        public ListarProductosModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Productos = await _context.Productos
                .Include(p => p.Categoria) // Cargar la categoría si existe
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                TempData["ErrorMessage"] = "Producto no encontrado.";
                return RedirectToPage();
            }

            try
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Producto eliminado con éxito.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error al eliminar el producto.";
            }

            return RedirectToPage();
        }
    }
}
