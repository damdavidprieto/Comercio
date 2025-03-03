using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infraestructure.Models;

namespace Comercio.Pages.Pedidos
{
    public class DetallesPedidoModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public DetallesPedidoModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public Pedido Pedido { get; set; }
        public List<DetallesPedido> Detalles { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Pedido == null)
            {
                return NotFound();
            }

            Detalles = await _context.DetallesPedidos
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == id)
                .ToListAsync();

            return Page();
        }
    }
}
