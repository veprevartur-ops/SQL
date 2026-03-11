
public class ExpenseCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal MonthlyBudget { get; set; }
    public bool IsActive { get; set; }
    public ICollection<ExpenseItem>? ExpenseItems { get; set; }
}

