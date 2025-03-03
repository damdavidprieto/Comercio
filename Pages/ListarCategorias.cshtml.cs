using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Comercio.Pages
{
    public class ListarCategoriasModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public List<Categoria> Categorias { get; set; } = new();

        public ListarCategoriasModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Categorias = await _context.Categorias.ToListAsync();
        }
    }
}
