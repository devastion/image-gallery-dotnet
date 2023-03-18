namespace image_gallery.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(
            "Server=mssql-db,1433;Database=image_gallery;User Id=sa;password=test12!@#$@asd;Trusted_Connection=False;Encrypt=false;MultipleActiveResultSets=true;");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Username })
            .IsUnique(true);
    }
}