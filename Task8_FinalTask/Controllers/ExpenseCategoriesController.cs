using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ExpenseCategoriesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ExpenseCategoriesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<ExpenseCategory>> Get() => await _context.Categories.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseCategory>> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return category;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ExpenseCategory category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ExpenseCategory category)
    {
        if (id != category.Id) return BadRequest();
        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
