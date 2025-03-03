using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infraestructure.Models;

namespace Comercio.Pages.Pedidos
{
    public class EditarPedidoModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public EditarPedidoModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pedido Pedido { get; set; }

        public List<SelectListItem> Clientes { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Pedido = await _context.Pedidos.FindAsync(id);

            if (Pedido == null)
            {
                return NotFound();
            }

            Clientes = await _context.Clientes
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Clientes = await _context.Clientes
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
                    .ToListAsync();
                return Page();
            }

            _context.Attach(Pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(Pedido.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Pedidos/ListarPedidos");
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(p => p.Id == id);
        }
    }
}
