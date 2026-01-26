using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Ez a tábla fogja tárolni a kiadásokat az adatbázisban
    public DbSet<Expense> Expenses { get; set; }
}