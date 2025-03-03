using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infraestructure.Models;
using System.Threading.Tasks;

namespace Comercio.Pages
{
    public class CrearCategoriaModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        [BindProperty]
        public Categoria Categoria { get; set; }

        public CrearCategoriaModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Categoria = new Categoria();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categorias.Add(Categoria);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListarCategorias");
        }
    }
}
