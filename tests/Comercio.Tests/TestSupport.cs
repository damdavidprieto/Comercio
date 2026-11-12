using Infraestructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Comercio.Tests;

internal static class TestSupport
{
    /// <summary>
    /// Creates an isolated in-memory database per test. The in-memory provider has no
    /// transactions, so that warning is ignored (the code under test still calls BeginTransaction).
    /// </summary>
    public static ComercioElectronicoContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ComercioElectronicoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new ComercioElectronicoContext(options);
    }

    /// <summary>Gives a PageModel the minimal context it needs (ModelState, HttpContext).</summary>
    public static T WithPageContext<T>(this T page) where T : PageModel
    {
        page.PageContext = new PageContext(
            new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()));
        return page;
    }
}
