namespace CursoApis.Models;

public class Expense
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public DateTime Date { get; set; }
    public int SummaryId { get; set; }
    public decimal Amount { get; set; }
    public int NumberOfInstallments { get; set; } = 1;

    public int CardId { get; set; }
    
    public Card? Card { get; set; }
    public Summary? Summary { get; set; }
    
}