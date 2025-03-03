using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infraestructure.Models;

namespace Comercio.Pages
{
    public class ListarInventarioModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public ListarInventarioModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public List<Infraestructure.Models.Inventario> InventarioLista { get; set; }

        public async Task OnGetAsync()
        {
            InventarioLista = await _context.Inventarios
                .Include(i => i.Producto)
                .ToListAsync();
        }
    }
}
