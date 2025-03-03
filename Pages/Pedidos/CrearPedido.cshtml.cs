using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infraestructure.Models;

namespace Comercio.Pages.Pedidos
{
    public class CrearPedidoModel : PageModel
    {
        private readonly ComercioElectronicoContext _context;

        public CrearPedidoModel(ComercioElectronicoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pedido Pedido { get; set; } = new Pedido();

        [BindProperty]
        public List<DetallesPedido> Detalles { get; set; } = new List<DetallesPedido>();

        public List<SelectListItem> Clientes { get; set; }
        public List<SelectListItem> Productos { get; set; }

        public async Task OnGetAsync()
        {
            Clientes = await _context.Clientes
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
                .ToListAsync();

            Productos = await _context.Productos
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Pedido.FechaPedido ??= DateTime.Now;

                // ✅ Calcular el total sumando los subtotales de los detalles
                double totalPedido = 0;

                foreach (var detalle in Detalles)
                {
                    if (detalle.ProductoId.HasValue && detalle.Cantidad > 0 && detalle.PrecioUnitario > 0)
                    {
                        detalle.PedidoId = Pedido.Id;
                        totalPedido += detalle.Cantidad.Value * detalle.PrecioUnitario.Value;
                        _context.DetallesPedidos.Add(detalle);
                    }
                }

                // ✅ Asignar el total calculado antes de guardar el pedido
                Pedido.Total = totalPedido;

                _context.Pedidos.Add(Pedido);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToPage("/Pedidos/ListarPedidos");
            }
            catch
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Error al guardar el pedido. Inténtelo nuevamente.");
                await OnGetAsync();
                return Page();
            }
        }

    }
}
