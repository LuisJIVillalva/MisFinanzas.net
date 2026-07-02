namespace CursoApis.Models;

public class Summary
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}