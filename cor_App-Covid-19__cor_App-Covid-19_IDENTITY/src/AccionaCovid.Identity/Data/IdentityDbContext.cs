using AccionaCovid.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccionaCovid.Identity.Data
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<UsuarioWorkDay> UsuarioWorkDay { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<EmpleadoRole> EmpleadoRole { get; set; }
        public virtual DbSet<FichaMedica> FichaMedica { get; set; }
        public virtual DbSet<ResultadoEncuestaSintomas> ResultadoEncuestaSintomas { get; set; }

        public virtual DbSet<FichaLaboral> FichaLaboral { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Empleado>(entity =>
            {
                entity.HasIndex(e => e.IdFichaLaboral);

                entity.HasIndex(e => e.IdFichaMedica);

                entity.HasIndex(e => e.IdUsuarioWorkDay)
                    .HasName("IX_Empleado_IdUsuario");

                entity.Property(e => e.Bloqueado)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.EsServicioMedico)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.InterAcciona)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.Nif)
                    .HasColumnName("NIF")
                    .HasMaxLength(50);

                entity.Property(e => e.Nombre).IsRequired();

                entity.Property(e => e.Telefono).HasMaxLength(50);

                entity.Property(e => e.UltimaModif).HasColumnType("datetime");

                entity.Property(e => e.Upn)
                    .HasColumnName("UPN")
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdUsuarioWorkDayNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdUsuarioWorkDay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Empleado_UsuarioWorkDay");

                entity.HasOne(d => d.IdFichaMedicaNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdFichaMedica)
                    .HasConstraintName("FK_Empleado_FichaMedica");

                entity.HasOne(d => d.IdFichaLaboralNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdFichaLaboral)
                    .HasConstraintName("FK_Empleado_FichaLaboral");
            });

            builder.Entity<EmpleadoRole>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EmpleadoRole)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmpleadoRole_Empleado");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.EmpleadoRole)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmpleadoRole_Role");
            });

            builder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Descripcion).HasMaxLength(100);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.Entity<UsuarioWorkDay>(entity =>
            {
                entity.Property(e => e.Departamento).HasDefaultValueSql("(CONVERT([bigint],(0)))");

                entity.Property(e => e.Division).HasDefaultValueSql("(CONVERT([bigint],(0)))");

                entity.Property(e => e.EsServicioMedico)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.InterAcciona)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.Nif)
                    .HasColumnName("NIF")
                    .HasMaxLength(50);

                entity.Property(e => e.Nombre).IsRequired();

                entity.Property(e => e.Telefono).HasMaxLength(50);

                entity.Property(e => e.UltimaModif).HasColumnType("datetime");

                entity.Property(e => e.Upn)
                    .HasColumnName("UPN")
                    .HasMaxLength(100);
            });

            builder.Entity<FichaMedica>(entity =>
            {
                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('0001-01-01T00:00:00.000')");
            });

            builder.Entity<ResultadoEncuestaSintomas>(entity =>
            {
                entity.HasIndex(e => e.IdFichaMedica);

                entity.HasIndex(e => e.IdTipoSintoma);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdFichaMedicaNavigation)
                    .WithMany(p => p.ResultadoEncuestaSintomas)
                    .HasForeignKey(d => d.IdFichaMedica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultadoEncuestaSintomas_FichaMedica");
            });

            builder.Entity<FichaLaboral>(entity =>
            {
            });


            base.OnModelCreating(builder);
        }
    }
}
