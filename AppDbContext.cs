using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<ExpenseCategory> Categories { get; set; }
    public DbSet<ExpenseItem> Items { get; set; }
    public DbSet<ExpenseTransaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ExpenseDb;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка ExpenseCategory
        modelBuilder.Entity<ExpenseCategory>(entity =>
        {
            entity.ToTable("ExpenseCategories");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.MonthlyBudget).HasColumnType("decimal(18,2)");
            entity.Property(c => c.IsActive).HasDefaultValue(true);
        });

        // Настройка ExpenseItem
        modelBuilder.Entity<ExpenseItem>(entity =>
        {
            entity.ToTable("ExpenseItems");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.IsActive).HasDefaultValue(true);

            entity.HasOne(i => i.Category)
                  .WithMany(c => c.ExpenseItems)
                  .HasForeignKey(i => i.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Настройка ExpenseTransaction
        modelBuilder.Entity<ExpenseTransaction>(entity =>
        {
            entity.ToTable("ExpenseTransactions");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Date).IsRequired();
            entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
            entity.Property(t => t.Comment).HasMaxLength(200);

            entity.HasOne(t => t.ExpenseItem)
                  .WithMany(i => i.Transactions)
                  .HasForeignKey(t => t.ExpenseItemId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
