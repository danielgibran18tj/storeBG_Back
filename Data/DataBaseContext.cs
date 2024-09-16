using Microsoft.EntityFrameworkCore;
using proyectop.Data.Models;

namespace proyectop.Data;

public class DataBaseContext: DbContext
{
    public DbSet<Producto> Producto {get;set;}
    public DbSet<Usuario> Usuario {get;set;}
    public DbSet<Category> Categories {get;set;}
    public DbSet<Role> Role { get; set; }
    
    public DataBaseContext(DbContextOptions<DataBaseContext> options) :base(options) {  }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            
        modelBuilder.Entity<Producto>(producto=>
        {
            producto.ToTable("Producto");
            producto.HasKey(p=> p.id);
            producto.Property(p=> p.title).IsRequired().HasMaxLength(100);
            producto.Property(p=> p.description).IsRequired().HasMaxLength(1000);
            producto.Property(p => p.price).IsRequired();
            producto.Property(p => p.stock).IsRequired();
            producto.Property(p => p.status).IsRequired();
            producto.HasOne(u => u.category).WithMany(
                u => u.Productos).HasForeignKey(u => u.categoryId);
            producto.HasMany(i => i.images).WithOne(
                i => i.Producto).HasForeignKey(i => i.ProductoId);
        });
        
        modelBuilder.Entity<Imagen>(image =>
        {
            image.ToTable("Imagen");
            image.HasKey(i => i.ImagenId);
            image.Property(i => i.ProductoId);
            image.Property(i => i.imagenString);
        });
        
        modelBuilder.Entity<Category>(category =>
        {
            category.ToTable("Category");
            category.HasKey(c => c.id);
            category.Property(c => c.name).IsRequired().HasMaxLength(20);
            category.Property(c => c.images).IsRequired();
            category.Property(c => c.creationAt).IsRequired();
            category.Property(c => c.updatedAt).IsRequired(false);
        });

        modelBuilder.Entity<Role>(role =>
        {
            role.ToTable("Role");
            role.HasKey(r => r.RoleId);
            role.Property(r => r.Nombre).IsRequired().HasMaxLength(20);
        });
        
        modelBuilder.Entity<Usuario>(user =>
        {
            user.ToTable("Usuario"); 
            user.HasKey(u => u.UsuarioId);
            user.HasOne(u => u.Role).WithMany(u => u.Usuarios).HasForeignKey(u => u.RoleId);
            user.Property(u => u.Username).IsRequired().HasMaxLength(50);
            user.Property(u => u.Email).IsRequired().HasMaxLength(100);
            user.Property(u => u.status).IsRequired();
            user.Property(u => u.Password).IsRequired().HasMaxLength(80);
            user.Property(u=> u.PasswordByte).IsRequired(false);
        });
        
        base.OnModelCreating(modelBuilder);
    }
}