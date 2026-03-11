using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ExpenseTransactionsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ExpenseTransactionsController(AppDbContext context) => _context = context;

    // Получить все транзакции
    [HttpGet]
    public async Task<IEnumerable<ExpenseTransaction>> GetAll() =>
        await _context.Transactions.Include(t => t.ExpenseItem).ToListAsync();

    // Получить транзакции за день
    [HttpGet("byday/{date}")]
    public async Task<IEnumerable<ExpenseTransaction>> GetByDay(DateTime date) =>
        await _context.Transactions
            .Where(t => t.Date.Date == date.Date)
            .Include(t => t.ExpenseItem)
            .ToListAsync();

    // Получить транзакции за месяц
    [HttpGet("bymonth/{year}/{month}")]
    public async Task<IEnumerable<ExpenseTransaction>> GetByMonth(int year, int month) =>
        await _context.Transactions
            .Where(t => t.Date.Year == year && t.Date.Month == month)
            .Include(t => t.ExpenseItem)
            .ToListAsync();

    // Визуализация по дню
    [HttpGet("sticker/{date}")]
    public async Task<string> GetDaySticker(DateTime date)
    {
        var sum = await _context.Transactions
            .Where(t => t.Date.Date == date.Date)
            .SumAsync(t => t.Amount);

        if (sum < 500)
            return "🟩 День прошёл экономно!";
        else if (sum <= 2000)
            return "🟨 Траты в пределах обычного.";
        else
            return "🟥 День был затратным!";
    }

    // Создать транзакцию
    [HttpPost]
    public async Task<IActionResult> Create(ExpenseTransaction transaction)
    {
        // Ограничение: не более 1 000 000 руб. за день
        var sumForDay = await _context.Transactions
            .Where(t => t.Date == transaction.Date)
            .SumAsync(t => t.Amount);

        if (sumForDay + transaction.Amount > 1_000_000m)
            return BadRequest("Суммарная сумма транзакций за день превышает 1 000 000 рублей!");

        // Проверка активности статьи
        var item = await _context.Items.FindAsync(transaction.ExpenseItemId);
        if (item == null || !item.IsActive)
            return BadRequest("Статья расхода неактивна или не найдена!");

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return Ok(transaction);
    }

    // Обновить транзакцию
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ExpenseTransaction transaction)
    {
        if (id != transaction.Id) return BadRequest();

        var existing = await _context.Transactions.Include(t => t.ExpenseItem).FirstOrDefaultAsync(t => t.Id == id);
        if (existing == null) return NotFound();

        // Если статья расхода неактивна, запретить изменение ExpenseItemId
        if (!existing.ExpenseItem.IsActive && transaction.ExpenseItemId != existing.ExpenseItemId)
            return BadRequest("Нельзя изменить статью расхода, если она неактивна.");

        existing.Date = transaction.Date;
        existing.Amount = transaction.Amount;
        existing.Comment = transaction.Comment;
        if (existing.ExpenseItem.IsActive)
            existing.ExpenseItemId = transaction.ExpenseItemId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Удалить транзакцию
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _context.Transactions.FindAsync(id);
        if (t == null) return NotFound();
        _context.Transactions.Remove(t);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}