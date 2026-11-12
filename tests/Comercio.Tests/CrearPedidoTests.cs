using Comercio.Pages.Pedidos;
using Infraestructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Tests;

public class CrearPedidoTests
{
    private static CrearPedidoModel NewPage(ComercioElectronicoContext context, params DetallesPedido[] detalles)
    {
        var page = new CrearPedidoModel(context).WithPageContext();
        page.Pedido = new Pedido { Estado = "Pendiente" };
        page.Detalles = detalles.ToList();
        return page;
    }

    [Fact]
    public async Task OnGet_loads_customers_and_products_for_the_form()
    {
        using var context = TestSupport.CreateContext();
        context.Clientes.Add(new Cliente { Nombre = "Ana" });
        context.Productos.AddRange(new Producto { Nombre = "Teclado" }, new Producto { Nombre = "Raton" });
        await context.SaveChangesAsync();

        var page = new CrearPedidoModel(context);
        await page.OnGetAsync();

        Assert.Single(page.Clientes);
        Assert.Equal(2, page.Productos.Count);
    }

    [Fact]
    public async Task OnPost_total_is_the_sum_of_quantity_times_unit_price()
    {
        using var context = TestSupport.CreateContext();
        var page = NewPage(context,
            new DetallesPedido { ProductoId = 1, Cantidad = 2, PrecioUnitario = 10.5 },
            new DetallesPedido { ProductoId = 2, Cantidad = 1, PrecioUnitario = 4.0 });

        var result = await page.OnPostAsync();

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Pedidos/ListarPedidos", redirect.PageName);

        var pedido = await context.Pedidos.SingleAsync();
        Assert.Equal(25.0, pedido.Total);
        Assert.NotNull(pedido.FechaPedido);
    }

    [Fact]
    public async Task OnPost_ignores_incomplete_lines()
    {
        using var context = TestSupport.CreateContext();
        var page = NewPage(context,
            new DetallesPedido { ProductoId = 1, Cantidad = 3, PrecioUnitario = 5.0 },
            new DetallesPedido { ProductoId = 2, Cantidad = 0, PrecioUnitario = 9.0 },
            new DetallesPedido { ProductoId = 3, Cantidad = 2, PrecioUnitario = 0 },
            new DetallesPedido { ProductoId = null, Cantidad = 2, PrecioUnitario = 7.0 });

        await page.OnPostAsync();

        var pedido = await context.Pedidos.SingleAsync();
        Assert.Equal(15.0, pedido.Total);
        Assert.Equal(1, await context.DetallesPedidos.CountAsync());
    }

    [Fact]
    public async Task OnPost_links_every_saved_line_to_the_saved_order()
    {
        using var context = TestSupport.CreateContext();
        var page = NewPage(context,
            new DetallesPedido { ProductoId = 1, Cantidad = 1, PrecioUnitario = 2.0 },
            new DetallesPedido { ProductoId = 2, Cantidad = 4, PrecioUnitario = 3.0 });

        await page.OnPostAsync();

        var pedido = await context.Pedidos.SingleAsync();
        var detalles = await context.DetallesPedidos.ToListAsync();

        Assert.Equal(2, detalles.Count);
        Assert.All(detalles, d => Assert.Equal(pedido.Id, d.PedidoId));
    }

    [Fact]
    public async Task OnPost_with_invalid_model_saves_nothing_and_redisplays_the_page()
    {
        using var context = TestSupport.CreateContext();
        var page = NewPage(context,
            new DetallesPedido { ProductoId = 1, Cantidad = 1, PrecioUnitario = 2.0 });
        page.ModelState.AddModelError("Pedido.ClienteId", "required");

        var result = await page.OnPostAsync();

        Assert.IsType<PageResult>(result);
        Assert.Empty(context.Pedidos);
        Assert.Empty(context.DetallesPedidos);
    }
}
