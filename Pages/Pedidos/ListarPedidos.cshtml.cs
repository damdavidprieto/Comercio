using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infraestructure.Models;

namespace Comercio.Pages.Pedidos
{
    public class ListarPedidosModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public ListarPedidosModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public List<Pedido> Pedidos { get; set; }

        public async Task OnGetAsync()
        {
            Pedidos = await _context.Pedidos
                .Include(p => p.Cliente) // Carga los datos del cliente
                .ToListAsync();
        }
    }
}
