using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Контроллер транзакций расходов.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExpenseTransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Конструктор контроллера транзакций расходов.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public ExpenseTransactionsController(AppDbContext context) => _context = context;

    /// <summary>
    /// Получение списка всех транзакций.
    /// </summary>
    /// <returns>Список транзакций расходов.</returns>
    [HttpGet]
    public async Task<IEnumerable<ExpenseTransaction>> GetAll() =>
        await _context.Transactions.Include(t => t.ExpenseItem).ToListAsync();

    /// <summary>
    /// Получение списка транзакций за день.
    /// </summary>
    /// <param name="date">Дата выборки.</param>
    /// <returns>Список транзакций за указанный день.</returns>
    [HttpGet("byday/{date}")]
    public async Task<IEnumerable<ExpenseTransaction>> GetByDay(DateTime date) =>
        await _context.Transactions
            .Where(t => t.Date.Date == date.Date)
            .Include(t => t.ExpenseItem)
            .ToListAsync();

    /// <summary>
    /// Получение списка транзакций за месяц.
    /// </summary>
    /// <param name="year">Год выборки.</param>
    /// <param name="month">Месяц выборки.</param>
    /// <returns>Список транзакций за указанный месяц.</returns>
    [HttpGet("bymonth/{year}/{month}")]
    public async Task<IEnumerable<ExpenseTransaction>> GetByMonth(int year, int month) =>
        await _context.Transactions
            .Where(t => t.Date.Year == year && t.Date.Month == month)
            .Include(t => t.ExpenseItem)
            .ToListAsync();

    /// <summary>
    /// Визуализация расходов за день.
    /// </summary>
    /// <param name="date">Дата анализа.</param>
    /// <returns>Текстовая оценка расходов за день.</returns>
    [HttpGet("sticker/{date}")]
    public async Task<string> GetDaySticker(string date)
    {
        // Преобразование вручную
        if (!DateTime.TryParse(date, out var parsedDate))
            return "Ошибка: неверный формат даты! Ожидается yyyy-MM-dd";

        var sum = await _context.Transactions
            .Where(t => t.Date.Date == parsedDate.Date)
            .SumAsync(t => t.Amount);

        if (sum < 500)
            return "🟩 День прошёл экономно!";
        else if (sum <= 2000)
            return "🟨 Траты в пределах обычного.";
        else
            return "🟥 День был затратным!";
    }

    /// <summary>
    /// Создание новой транзакции.
    /// </summary>
    /// <param name="transaction">Данные новой транзакции.</param>
    /// <returns>Результат создания транзакции.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(ExpenseTransaction transaction)
    {
        var sumForDay = await _context.Transactions
            .Where(t => t.Date == transaction.Date)
            .SumAsync(t => t.Amount);

        if (sumForDay + transaction.Amount > 1_000_000m)
            return BadRequest("Суммарная сумма транзакций за день превышает 1 000 000 рублей!");

        var item = await _context.Items.FindAsync(transaction.ExpenseItemId);
        if (item == null || !item.IsActive)
            return BadRequest("Статья расхода неактивна или не найдена!");

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return Ok(transaction);
    }

    /// <summary>
    /// Обновление транзакции.
    /// </summary>
    /// <param name="id">Идентификатор транзакции.</param>
    /// <param name="transaction">Новые данные транзакции.</param>
    /// <returns>Результат обновления транзакции.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ExpenseTransaction transaction)
    {
        if (id != transaction.Id) return BadRequest();

        var existing = await _context.Transactions
            .Include(t => t.ExpenseItem)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (existing == null) return NotFound();

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

    /// <summary>
    /// Удаление транзакции.
    /// </summary>
    /// <param name="id">Идентификатор транзакции.</param>
    /// <returns>Результат удаления транзакции.</returns>
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