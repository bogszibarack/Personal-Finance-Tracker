using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Expense
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A leírás kötelező")]
    [Display(Name = "Megnevezés")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Az összegnek nagyobbnak kell lennie nullánál")]
    [Display(Name = "Összeg")]
    public decimal Amount { get; set; }

    [Display(Name = "Dátum")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Display(Name = "Kategória")]
    public ExpenseCategory Category { get; set; } // Itt használjuk az Enum-ot
}