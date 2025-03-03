using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comercio.Pages
{
    public class ListarClientesModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public List<Cliente> Clientes { get; set; } = new List<Cliente>(); // Inicializar lista vacía

        public ListarClientesModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Clientes = await _context.Clientes.ToListAsync(); // Cargar clientes
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Pedidos) // Verifica si tiene pedidos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage();
            }

            if (cliente.Pedidos.Any()) // Si tiene pedidos, no se puede eliminar
            {
                TempData["ErrorMessage"] = "No se puede eliminar el cliente porque tiene pedidos asociados.";
                return RedirectToPage();
            }

            try
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cliente eliminado con éxito.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error al eliminar el cliente.";
            }

            return RedirectToPage();
        }
    }
}
