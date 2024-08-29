using Microsoft.EntityFrameworkCore;
using proyectop.Data.Models;

namespace proyectop.Data;

public class DataBaseContext: DbContext
{
    public DbSet<ProductoEntity> Producto {get;set;}
    public DbSet<Category> Categories {get;set;}
    
    public DataBaseContext(DbContextOptions<DataBaseContext> options) :base(options) {  }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            
        modelBuilder.Entity<ProductoEntity>(producto=>
        {
            producto.ToTable("Producto");
            producto.HasKey(p=> p.id);
            producto.Property(p=> p.title).IsRequired().HasMaxLength(100);
            producto.Property(p=> p.description).IsRequired().HasMaxLength(1000);
            producto.Property(p => p.price).IsRequired();
            producto.HasOne(u => u.category).WithMany(
                u => u.Productos).HasForeignKey(u => u.categoryId);
        });
        
        modelBuilder.Entity<ProductSelect>(image =>
        {
            image.ToTable("ProductSelect");
            image.HasKey(i => i.Id);
            image.Property(i => i.ProductoId);
        });
        
        modelBuilder.Entity<Category>(category =>
        {
            category.ToTable("Category");
            category.HasKey(c => c.id);
            category.Property(c => c.name).IsRequired().HasMaxLength(20);
            category.Property(c => c.creationAt).IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}