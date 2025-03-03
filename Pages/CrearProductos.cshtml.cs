using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infraestructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comercio.Pages
{
    public class CrearProductoModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        [BindProperty]
        public Producto Producto { get; set; }

        public List<Categoria> Categorias { get; set; }

        public CrearProductoModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Categorias = _context.Categorias.ToList();
            Producto = new Producto();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categorias = _context.Categorias.ToList();
                return Page();
            }

            _context.Productos.Add(Producto);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListarProductos");
        }
    }
}
