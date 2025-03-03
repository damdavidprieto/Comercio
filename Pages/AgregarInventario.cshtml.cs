using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infraestructure.Models;

namespace Comercio.Pages
{
    public class AgregarInventarioModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public AgregarInventarioModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inventario Inventario { get; set; }

        public List<SelectListItem> Productos { get; set; }

        public async Task OnGetAsync()
        {
            Productos = await _context.Productos
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Recargar productos si hay error
                return Page();
            }

            _context.Inventarios.Add(Inventario);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListarInventario");
        }
    }
}
