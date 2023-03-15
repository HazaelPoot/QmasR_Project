using Microsoft.EntityFrameworkCore;
using QRDashboard.Domain.Entities;

namespace QRDashboard.Infraestructure.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<AdminType> AdminTypes { get; set; } = null!;
        public virtual DbSet<Categorium> Categoria { get; set; } = null!;
        public virtual DbSet<FotosProyecto> FotosProyectos { get; set; } = null!;
        public virtual DbSet<ProyectoQr> ProyectoQrs { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         optionsBuilder.UseSqlServer("Data source=HAZAEL\\SQLEXPRESS; Initial Catalog=QR_DATABASE; Trusted_Connection=True;");
        //         optionsBuilder.UseSqlServer("workstation id=qmasrdatabase.mssql.somee.com;packet size=4096;user id=Hazael_Poot_SQLLogin_1;pwd=epua3vqx4v;data source=qmasrdatabase.mssql.somee.com;persist security info=False;initial catalog=qmasrdatabase");
        //     }
        // }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminType>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__AdminTyp__2A49584C304D00EB");

                entity.ToTable("AdminType");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK__Categori__A3C02A10F77F585C");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FotosProyecto>(entity =>
            {
                entity.HasKey(e => e.IdImg)
                    .HasName("PK__FotosPro__0C1AF99B951E184D");

                entity.ToTable("FotosProyecto");

                entity.Property(e => e.NombreFoto)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UrlImage)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.IdProjNavigation)
                    .WithMany(p => p.FotosProyectos)
                    .HasForeignKey(d => d.IdProj)
                    .HasConstraintName("FK__FotosProy__IdPro__3587F3E0");
            });

            modelBuilder.Entity<ProyectoQr>(entity =>
            {
                entity.HasKey(e => e.IdProj)
                    .HasName("PK__Proyecto__E40D9717B8544DD7");

                entity.ToTable("ProyectoQR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.NombreFoto)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Presupuesto).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ubicacion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UrlImagen)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.ProyectoQrs)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK__ProyectoQ__IdCat__32AB8735");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__Usuarios__B7C92638B372DD68");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreFoto)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Passw)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UrlImagen)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AdminTypeNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.AdminType)
                    .HasConstraintName("FK__Usuarios__AdminT__2FCF1A8A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}