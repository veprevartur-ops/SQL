using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ExpenseItemsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ExpenseItemsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<ExpenseItem>> Get() => await _context.Items.Include(i => i.Category).ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseItem>> GetById(int id)
    {
        var item = await _context.Items.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);
        if (item == null) return NotFound();
        return item;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ExpenseItem item)
    {
        if (!_context.Categories.Any(c => c.Id == item.CategoryId && c.IsActive))
            return BadRequest("Категория неактивна или не найдена.");
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ExpenseItem item)
    {
        if (id != item.Id) return BadRequest();
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null) return NotFound();
        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}