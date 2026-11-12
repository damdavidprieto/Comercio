using Comercio.Pages;
using Infraestructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Tests;

public class InventarioAndCatalogTests
{
    [Fact]
    public async Task AgregarInventario_saves_the_stock_and_redirects_to_the_list()
    {
        using var context = TestSupport.CreateContext();
        var page = new AgregarInventarioModel(context).WithPageContext();
        page.Inventario = new Inventario { ProductoId = 1, CantidadStock = 12, UbicacionAlmacen = "A-01" };

        var result = await page.OnPostAsync();

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/ListarInventario", redirect.PageName);

        var saved = await context.Inventarios.SingleAsync();
        Assert.Equal(12, saved.CantidadStock);
        Assert.Equal("A-01", saved.UbicacionAlmacen);
    }

    [Fact]
    public async Task AgregarInventario_with_invalid_model_saves_nothing()
    {
        using var context = TestSupport.CreateContext();
        var page = new AgregarInventarioModel(context).WithPageContext();
        page.Inventario = new Inventario { ProductoId = 1, CantidadStock = 1 };
        page.ModelState.AddModelError("Inventario.UbicacionAlmacen", "required");

        var result = await page.OnPostAsync();

        Assert.IsType<PageResult>(result);
        Assert.Empty(context.Inventarios);
    }

    [Fact]
    public async Task Product_keeps_its_category_and_inventory_relations()
    {
        using (var seed = TestSupport.CreateContext())
        {
            var categoria = new Categoria { Nombre = "Perifericos" };
            var producto = new Producto { Nombre = "Teclado", Precio = 39.9, Categoria = categoria };
            producto.Inventarios.Add(new Inventario { CantidadStock = 7, UbicacionAlmacen = "B-02" });
            seed.Productos.Add(producto);
            await seed.SaveChangesAsync();

            var loaded = await seed.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Inventarios)
                .SingleAsync();

            Assert.Equal("Perifericos", loaded.Categoria.Nombre);
            Assert.Single(loaded.Inventarios);
            Assert.Equal(7, loaded.Inventarios.First().CantidadStock);
            Assert.Equal(loaded.Id, loaded.Inventarios.First().ProductoId);
        }
    }
}
