using System.Text.Json.Serialization;

public class ExpenseItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public ExpenseCategory? Category { get; set; }
    public bool IsActive { get; set; }
    [JsonIgnore]
    public ICollection<ExpenseTransaction>? Transactions { get; set; }
}
