using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var currentYear = DateTime.Now.Year;

        // Adatok lekérése
        var yearlyData = await _context.Expenses
            .Where(e => e.CreatedAt.Year == currentYear)
            .GroupBy(e => (e.CreatedAt.Month - 1) / 3) // Negyedévek kiszámítása (0, 1, 2, 3)
            .Select(g => new { 
                Quarter = g.Key, 
                Total = g.Sum(e => e.Amount) 
            })
            .ToListAsync();

        // Feltöltünk egy 4 elemű tömböt (0-ra állítva az üreseket)
        var quarterlyTotals = new decimal[4];
        foreach (var item in yearlyData)
        {
            quarterlyTotals[item.Quarter] = item.Total;
        }

        ViewBag.QuarterlyData = quarterlyTotals;
        ViewBag.CurrentYear = currentYear;

        return View();
    }

}
