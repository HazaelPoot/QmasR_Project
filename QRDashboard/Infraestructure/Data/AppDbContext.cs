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
        public virtual DbSet<FotosProyecto> FotosProyectos { get; set; } = null!;
        public virtual DbSet<ProyectoQr> ProyectoQrs { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         optionsBuilder.UseSqlServer("Data source=HAZAEL\\SQLEXPRESS; Initial Catalog=QR_DATABASE; Trusted_Connection=True;");
        //     }
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminType>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__AdminTyp__2A49584C253B6B58");

                entity.ToTable("AdminType");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FotosProyecto>(entity =>
            {
                entity.HasKey(e => e.IdImg)
                    .HasName("PK__FotosPro__0C1AF99B5F1DB885");

                entity.ToTable("FotosProyecto");

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.UrlImage).IsUnicode(false);

                entity.HasOne(d => d.IdProjNavigation)
                    .WithMany(p => p.FotosProyectos)
                    .HasForeignKey(d => d.IdProj)
                    .HasConstraintName("FK__FotosProy__IdPro__3D5E1FD2");
            });

            modelBuilder.Entity<ProyectoQr>(entity =>
            {
                entity.HasKey(e => e.IdProj)
                    .HasName("PK__Proyecto__E40D9717D5B9C62B");

                entity.ToTable("ProyectoQR");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.FinaliDatre).HasColumnType("date");

                entity.Property(e => e.Presupuesto).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ubicacion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UrlImagen).IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__Usuarios__B7C92638C1334D9B");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UrlImagen).IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AdminTypeNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.AdminType)
                    .HasConstraintName("FK__Usuarios__AdminT__38996AB5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}