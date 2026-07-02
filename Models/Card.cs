namespace CursoApis.Models;

public class Card
{
    public int Id { get; set; }
    public required string CardName { get; set; }
    
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    
}