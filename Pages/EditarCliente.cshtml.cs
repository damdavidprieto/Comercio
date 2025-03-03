using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infraestructure.Models;
using System.Threading.Tasks;

namespace Comercio.Pages
{
    public class EditarClienteModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public EditarClienteModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Cliente = await _context.Clientes.FindAsync(id);

            if (Cliente == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clientes.Update(Cliente);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListarClientes");
        }
    }
}
