using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infraestructure.Models;
using System.Threading.Tasks;

namespace Comercio.Pages
{
    public class CrearClienteModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        [BindProperty]
        public Cliente Cliente { get; set; }

        public CrearClienteModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Cliente = new Cliente();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clientes.Add(Cliente);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListarClientes");
        }
    }
}
