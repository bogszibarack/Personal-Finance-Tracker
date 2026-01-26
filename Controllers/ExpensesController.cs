using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Text;

namespace WebApplication1.Controllers;

public class ExpensesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExpensesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // CSAK EZ AZ EGY INDEX MARADJON!
    public async Task<IActionResult> Index(int? month, int? year)
    {
        var currentMonth = month ?? DateTime.Now.Month;
        var currentYear = year ?? DateTime.Now.Year;

        var expenses = await _context.Expenses
            .Where(e => e.CreatedAt.Month == currentMonth && e.CreatedAt.Year == currentYear)
            .ToListAsync();

        ViewBag.SelectedMonth = currentMonth;
        ViewBag.SelectedYear = currentYear;

        return View(expenses);
    }

    // CSV EXPORTÁLÁS FUNKCIÓ
    public async Task<IActionResult> ExportToCsv(int month, int year)
    {
        var data = await _context.Expenses
            .Where(e => e.CreatedAt.Month == month && e.CreatedAt.Year == year)
            .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Megnevezes,Osszeg,Kategoria,Datum");

        foreach (var item in data)
        {
            csv.AppendLine($"{item.Description},{item.Amount},{item.Category},{item.CreatedAt:yyyy-MM-dd}");
        }

        byte[] buffer = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
        return File(buffer, "text/csv", $"kiadasok_{year}_{month}.csv");
    }

    // --- A többi metódus (Create, Edit, Delete) változatlan marad ---
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Expense expense)
    {
        if (ModelState.IsValid)
        {
            _context.Add(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(expense);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var expense = await _context.Expenses.FindAsync(id);
        return expense == null ? NotFound() : View(expense);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Expense expense)
    {
        if (id != expense.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(expense);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Biztonság miatt javasolt ide is!
    public async Task<IActionResult> Delete(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}