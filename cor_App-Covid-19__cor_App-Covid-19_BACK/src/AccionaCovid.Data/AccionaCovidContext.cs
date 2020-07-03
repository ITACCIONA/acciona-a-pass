using AccionaCovid.Crosscutting;
using AccionaCovid.Data.Core;
using AccionaCovid.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccionaCovid.Data
{
    public partial class AccionaCovidContext : GenericDbContext
    {
        /// <summary>
        /// Instancia del usuario de acceso
        /// </summary>
        private readonly IUserInfoAccesor userInfoAccesor;

        /// <summary>
        /// Instancia del usuario de acceso
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }

        #region Constructor

        public AccionaCovidContext() : base()
        {
        }

        public AccionaCovidContext(DbContextOptions<AccionaCovidContext> options, IUserInfoAccesor userInfoAccesor, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.userInfoAccesor = userInfoAccesor ?? throw new ArgumentNullException(nameof(userInfoAccesor));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        #endregion

        #region Entidades

        public virtual DbSet<AlertaEmpleado> AlertaEmpleado { get; set; }
        public virtual DbSet<AlertaServiciosMedicos> AlertaServiciosMedicos { get; set; }
        public virtual DbSet<Audits> Audits { get; set; }
        public virtual DbSet<ColorEstado> ColorEstado { get; set; }
        public virtual DbSet<Departamento> Departamento { get; set; }
        public virtual DbSet<Subcontrata> Subcontrata { get; set; }
        public virtual DbSet<Obra> Obra { get; set; }
        public virtual DbSet<EstadoObra> EstadoObra { get; set; }
        public virtual DbSet<AdjudicacionTrabajoObra> AdjudicacionTrabajoObra { get; set; }
        public virtual DbSet<AsignacionAdjudicacion> AsignacionAdjudicacion { get; set; }
        public virtual DbSet<IntegracionExternos> IntegracionExternos { get; set; }
        public virtual DbSet<Division> Division { get; set; }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<EmpleadoRole> EmpleadoRole { get; set; }
        public virtual DbSet<EntregaEquipoProteccion> EntregaEquipoProteccion { get; set; }
        public virtual DbSet<EquipoProteccion> EquipoProteccion { get; set; }
        public virtual DbSet<EstadoPasaporte> EstadoPasaporte { get; set; }
        public virtual DbSet<EstadoPasaporteIdioma> EstadoPasaporteIdioma { get; set; }
        public virtual DbSet<FactorRiesgo> FactorRiesgo { get; set; }
        public virtual DbSet<FichaLaboral> FichaLaboral { get; set; }
        public virtual DbSet<FichaMedica> FichaMedica { get; set; }
        public virtual DbSet<Localizacion> Localizacion { get; set; }
        public virtual DbSet<LocalizacionEmpleados> LocalizacionEmpleados { get; set; }
        public virtual DbSet<ParametroMedico> ParametroMedico { get; set; }
        public virtual DbSet<Pasaporte> Pasaporte { get; set; }
        public virtual DbSet<ResultadoEncuestaSintomas> ResultadoEncuestaSintomas { get; set; }
        public virtual DbSet<ResultadoTestMedico> ResultadoTestMedico { get; set; }
        public virtual DbSet<ResultadoTestPcr> ResultadoTestPcr { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SeguimientoMedico> SeguimientoMedico { get; set; }
        public virtual DbSet<TipoEstado> TipoEstado { get; set; }
        public virtual DbSet<TipoParametroMedico> TipoParametroMedico { get; set; }
        public virtual DbSet<TipoSintomas> TipoSintomas { get; set; }
        public virtual DbSet<UsuarioWorkDay> UsuarioWorkDay { get; set; }
        public virtual DbSet<ValoracionFactorRiesgo> ValoracionFactorRiesgo { get; set; }
        public virtual DbSet<ValoracionParametroMedico> ValoracionParametroMedico { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AmbitoAccesoEmpleadoPais> AmbitoAccesoEmpleadoPais { get; set; }
        public virtual DbSet<AmbitoAccesoEmpleadoRegion> AmbitoAccesoEmpleadoRegion { get; set; }
        public virtual DbSet<AmbitoAccesoEmpleadoArea> AmbitoAccesoEmpleadoArea { get; set; }
        public virtual DbSet<Tecnologia> Tecnologia { get; set; }
        public virtual DbSet<ResultadosIntegracion> ResultadosIntegracion { get; set; }
        #endregion

        #region Configuracion

        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLoggerFactory(GetLoggerFactory()).EnableSensitiveDataLogging();
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlertaEmpleado>(entity =>
            {
                entity.HasIndex(e => e.IdEmpleado);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.AlertaEmpleado)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertaEmpleado_Empleado");
            });

            modelBuilder.Entity<AlertaServiciosMedicos>(entity =>
            {
                entity.HasIndex(e => e.IdEmpleado);

                entity.Property(e => e.Comentario)
                    .IsRequired()
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.FechaNotificacion).HasDefaultValueSql("('0001-01-01T00:00:00.0000000+00:00')");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Leido)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.AlertaServiciosMedicos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertaServiciosMedicos_Empleado");
            });

            modelBuilder.Entity<Audits>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ColorEstado>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Prioridad)
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.Property(e => e.IdWorkday).HasDefaultValueSql("(CONVERT([bigint],(0)))");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre).IsRequired();
            });

            modelBuilder.Entity<Subcontrata>(entity =>
            {
                entity.Property(e => e.LastAction)
                     .IsRequired()
                     .HasMaxLength(10)
                     .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Cif)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Cif)
                    .IsUnique();
            });

            modelBuilder.Entity<Obra>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CodigoObra)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.EstadoObra)
                    .WithMany(p => p.Obra)
                    .HasForeignKey(d => d.IdEstadoObra)
                    .HasConstraintName("FK_Obra_EstadoObra");
            });

            modelBuilder.Entity<EstadoObra>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(40);
            });


            modelBuilder.Entity<AdjudicacionTrabajoObra>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Obra)
                    .WithMany(p => p.AdjudicacionTrabajoObra)
                    .HasForeignKey(d => d.IdObra)
                    .HasConstraintName("FK_AdjudicacionTrabajoObra_Obra");

                entity.HasOne(d => d.Subcontrata)
                    .WithMany(p => p.AdjudicacionTrabajoObra)
                    .HasForeignKey(d => d.IdSubcontrata)
                    .HasConstraintName("FK_AdjudicacionTrabajoObra_Subcontrata");

                entity.HasOne(d => d.EmpleadoResponsable)
                    .WithMany(p => p.AdjudicacionTrabajoObra)
                    .HasForeignKey(d => d.IdEmpleadoResponsable)
                    .HasConstraintName("FK_AdjudicacionTrabajoObra_Empleado");

                entity.Property(d => d.IdEmpleadoResponsable)
                .IsRequired(false);
            });

            modelBuilder.Entity<AsignacionAdjudicacion>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AdjudicacionTrabajoObra)
                    .WithMany(p => p.AsignacionAdjudicacion)
                    .HasForeignKey(d => d.IdAdjudicacionTrabajoObra)
                    .HasConstraintName("FK_AsignacionAdjudicacion_AdjudicacionTrabajoObra");

                entity.HasOne(d => d.FichaLaboral)
                    .WithMany(p => p.AsignacionAdjudicacion)
                    .HasForeignKey(d => d.IdFichaLaboral)
                    .HasConstraintName("FK_AsignacionAdjudicacion_FichaLaboral");
            });

            modelBuilder.Entity<IntegracionExternos>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasIndex(e=>e.Nif).IsUnique();

            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.Property(e => e.IdWorkday).HasDefaultValueSql("(CONVERT([bigint],(0)))");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre).IsRequired();
            });

            modelBuilder.Entity<Empleado>(entity =>
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

                entity.HasOne(d => d.IdFichaLaboralNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdFichaLaboral)
                    .HasConstraintName("FK_Empleado_FichaLaboral");

                entity.HasOne(d => d.IdFichaMedicaNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdFichaMedica)
                    .HasConstraintName("FK_Empleado_FichaMedica");

                entity.HasOne(d => d.IdUsuarioWorkDayNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdUsuarioWorkDay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Empleado_UsuarioWorkDay");

                entity.HasOne(d => d.IntegracionExternos)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.IdIntegracionExternos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Empleado_IntegracionExternos");
            });

            modelBuilder.Entity<EmpleadoRole>(entity =>
            {
                entity.HasIndex(e => e.IdEmpleado);

                entity.HasIndex(e => e.IdRole);

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

            modelBuilder.Entity<EntregaEquipoProteccion>(entity =>
            {
                entity.HasIndex(e => e.IdEmpleado);

                entity.HasIndex(e => e.IdEquipoProteccion);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.EntregaEquipoProteccion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntregaEquipoProteccion_Empleado");

                entity.HasOne(d => d.IdEquipoProteccionNavigation)
                    .WithMany(p => p.EntregaEquipoProteccion)
                    .HasForeignKey(d => d.IdEquipoProteccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntregaEquipoProteccion_EquipoProteccion");
            });

            modelBuilder.Entity<EquipoProteccion>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<EstadoPasaporte>(entity =>
            {
                entity.Property(e => e.DiasValidez).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Pcrant).HasColumnName("PCRAnt");

                entity.Property(e => e.Pcrult).HasColumnName("PCRUlt");

                entity.HasOne(d => d.IdColorEstadoNavigation)
                    .WithMany(p => p.EstadoPasaporte)
                    .HasForeignKey(d => d.IdColorEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstadoPasaporte_ColorEstado");

                entity.HasOne(d => d.IdTipoEstadoNavigation)
                    .WithMany(p => p.EstadoPasaporte)
                    .HasForeignKey(d => d.IdTipoEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstadoPasaporte_TipoEstado");
            });

            modelBuilder.Entity<EstadoPasaporteIdioma>(entity =>
            {
                entity.Property(e => e.Idioma)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdEstadoPasaporteNavigation)
                    .WithMany(p => p.EstadoPasaporteIdioma)
                    .HasForeignKey(d => d.IdEstadoPasaporte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstadoPasaporteIdioma_EstadoPasaporte");
            });

            modelBuilder.Entity<FactorRiesgo>(entity =>
            {
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

            modelBuilder.Entity<FichaLaboral>(entity =>
            {
                entity.HasIndex(e => e.IdDepartamento);

                entity.HasIndex(e => e.IdDivision);

                entity.HasIndex(e => e.IdLocalizacion);

                entity.HasIndex(e => e.IdResponsableDirecto);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MailProf).HasMaxLength(100);

                entity.Property(e => e.Obra).HasMaxLength(100);

                entity.Property(e => e.TelefonoCorp).HasMaxLength(50);

                entity.HasOne(d => d.IdDepartamentoNavigation)
                    .WithMany(p => p.FichaLaboral)
                    .HasForeignKey(d => d.IdDepartamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FichaLaboral_Departamento");

                entity.HasOne(d => d.IdDivisionNavigation)
                    .WithMany(p => p.FichaLaboral)
                    .HasForeignKey(d => d.IdDivision)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FichaLaboral_Division");

                entity.HasOne(d => d.IdLocalizacionNavigation)
                    .WithMany(p => p.FichaLaboral)
                    .HasForeignKey(d => d.IdLocalizacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FichaLaboral_Localizacion3");

                entity.HasOne(d => d.IdResponsableDirectoNavigation)
                    .WithMany(p => p.FichaLaboral)
                    .HasForeignKey(d => d.IdResponsableDirecto)
                    .HasConstraintName("FK_FichaLaboral_Empleado");

                entity.HasOne(d => d.Tecnologia)
                    .WithMany(p => p.FichaLaboral)
                    .HasForeignKey(d => d.IdTecnologia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FichaLaboral_Tecnologia");
            });

            modelBuilder.Entity<FichaMedica>(entity =>
            {
                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('0001-01-01T00:00:00.000')");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Localizacion>(entity =>
            {
                entity.Property(e => e.Ciudad).HasMaxLength(100);

                entity.Property(e => e.CodigoPostal).HasMaxLength(20);

                entity.Property(e => e.Direccion1).HasMaxLength(300);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre).IsRequired();

                entity.Property(e => e.Pais)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Localizacion)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Localizacion_Area");
            });

            modelBuilder.Entity<LocalizacionEmpleados>(entity =>
            {
                entity.HasIndex(e => e.IdEmpleado);

                entity.HasIndex(e => e.IdLocalizacion);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.LocalizacionEmpleados)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocalizacionEmpleados_Empleado");

                entity.HasOne(d => d.IdLocalizacionNavigation)
                    .WithMany(p => p.LocalizacionEmpleados)
                    .HasForeignKey(d => d.IdLocalizacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocalizacionEmpleados_Localizacion");
            });

            modelBuilder.Entity<ParametroMedico>(entity =>
            {
                entity.HasIndex(e => e.IdTipoParametro);

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

                entity.HasOne(d => d.IdTipoParametroNavigation)
                    .WithMany(p => p.ParametroMedico)
                    .HasForeignKey(d => d.IdTipoParametro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParametroMedico_TipoParametroMedico");
            });

            modelBuilder.Entity<Pasaporte>(entity =>
            {
                entity.HasIndex(e => e.IdAccion);

                entity.HasIndex(e => e.IdEmpleado);

                entity.HasIndex(e => e.IdEstadoPasaporte);

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsManual)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.Pasaporte)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pasaporte_Empleado");

                entity.HasOne(d => d.IdEstadoPasaporteNavigation)
                    .WithMany(p => p.Pasaporte)
                    .HasForeignKey(d => d.IdEstadoPasaporte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pasaporte_EstadoPasaporte");
            });

            modelBuilder.Entity<ResultadoEncuestaSintomas>(entity =>
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

                entity.HasOne(d => d.IdTipoSintomaNavigation)
                    .WithMany(p => p.ResultadoEncuestaSintomas)
                    .HasForeignKey(d => d.IdTipoSintoma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultadoEncuestaSintomas_TipoSintomas");
            });

            modelBuilder.Entity<ResultadoTestMedico>(entity =>
            {
                entity.HasIndex(e => e.IdFichaMedica);

                entity.Property(e => e.FechaTest).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Igg).HasColumnName("IGG");

                entity.Property(e => e.Igm).HasColumnName("IGM");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdFichaMedicaNavigation)
                    .WithMany(p => p.ResultadoTestMedico)
                    .HasForeignKey(d => d.IdFichaMedica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultadoTestMedico_FichaMedica");
            });

            modelBuilder.Entity<ResultadoTestPcr>(entity =>
            {
                entity.ToTable("ResultadoTestPCR");

                entity.HasIndex(e => e.IdFichaMedica);

                entity.Property(e => e.FechaTest).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdFichaMedicaNavigation)
                    .WithMany(p => p.ResultadoTestPcr)
                    .HasForeignKey(d => d.IdFichaMedica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultadoTestPCR_FichaMedica");
            });

            modelBuilder.Entity<Role>(entity =>
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

            modelBuilder.Entity<SeguimientoMedico>(entity =>
            {
                entity.HasIndex(e => e.IdFichaMedica);

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.FechaSeguimiento).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdFichaMedicaNavigation)
                    .WithMany(p => p.SeguimientoMedico)
                    .HasForeignKey(d => d.IdFichaMedica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoMedico_FichaMedica");
            });

            modelBuilder.Entity<TipoEstado>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TipoParametroMedico>(entity =>
            {
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

            modelBuilder.Entity<TipoSintomas>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre).IsRequired();
            });

            modelBuilder.Entity<UsuarioWorkDay>(entity =>
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

            modelBuilder.Entity<ValoracionFactorRiesgo>(entity =>
            {
                entity.HasIndex(e => e.IdFactorRiesgo);

                entity.HasIndex(e => e.IdFichaMedica)
                    .HasName("IX_ValoracionFactorRiesgo_IdEvaluacionFactoresRiesgo");

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdFactorRiesgoNavigation)
                    .WithMany(p => p.ValoracionFactorRiesgo)
                    .HasForeignKey(d => d.IdFactorRiesgo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValoracionFactorRiesgo_FactorRiesgo");

                entity.HasOne(d => d.IdFichaMedicaNavigation)
                    .WithMany(p => p.ValoracionFactorRiesgo)
                    .HasForeignKey(d => d.IdFichaMedica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValoracionFactorRiesgo_FichaMedica");
            });

            modelBuilder.Entity<ValoracionParametroMedico>(entity =>
            {
                entity.HasIndex(e => e.IdParametroMedico);

                entity.HasIndex(e => e.IdSegumientoMedico);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdParametroMedicoNavigation)
                    .WithMany(p => p.ValoracionParametroMedico)
                    .HasForeignKey(d => d.IdParametroMedico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValoracionParametroMedico_ParametroMedico");

                entity.HasOne(d => d.IdSegumientoMedicoNavigation)
                    .WithMany(p => p.ValoracionParametroMedico)
                    .HasForeignKey(d => d.IdSegumientoMedico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValoracionParametroMedico_SeguimientoMedico");
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasOne(d => d.Empleado)
                     .WithMany(p => p.AspNetUsers)
                     .HasForeignKey(d => d.IdEmpleado)
                     .HasConstraintName("FK_AspNetUsers_Empleado");
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Pais)
                    .WithMany(p => p.Region)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Region_Pais");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Area)
                    .HasForeignKey(d => d.IdRegion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Area_Region");
            });

            modelBuilder.Entity<AmbitoAccesoEmpleadoPais>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.AmbitoAccesoEmpleadoPais)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AmbitoAccesoEmpleadoPais_Empleado");

                entity.HasOne(d => d.Pais)
                    .WithMany(p => p.AmbitoAccesoEmpleadoPais)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AmbitoAccesoEmpleadoPais_Pais");
            });

            modelBuilder.Entity<AmbitoAccesoEmpleadoRegion>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.AmbitoAccesoEmpleadoRegion)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AmbitoAccesoEmpleadoRegion_Empleado");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.AmbitoAccesoEmpleadoRegion)
                    .HasForeignKey(d => d.IdRegion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AmbitoAccesoEmpleadoRegion_Region");
            });

            modelBuilder.Entity<AmbitoAccesoEmpleadoArea>(entity =>
            {
                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'CREATE')");

                entity.Property(e => e.LastActionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.AmbitoAccesoEmpleadoArea)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AmbitoAccesoEmpleadoArea_Empleado");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.AmbitoAccesoEmpleadoArea)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AmbitoAccesoEmpleadoPais_Area");
            });

            modelBuilder.Entity<Tecnologia>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        /// <summary>
        /// OnModelCreatingPartial
        /// </summary>
        /// <param name="modelBuilder"></param>
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        #endregion

        #region Auditoria

        /// <summary>
        /// OnBeforeSaveChanges
        /// </summary>
        /// <returns></returns>
        override internal List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audits || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                //auditEntry.TableName = entry.Metadata.GetTableName();
                //auditEntry.IdUser = userInfoAccesor.IdUser;
                //auditEntry.UserName = userInfoAccesor.UserFullName;
                //auditEntry.RequestId = httpContextAccessor.HttpContext.TraceIdentifier;

                //auditEntries.Add(auditEntry);

                bool isDeleteLogic = false;

                PropertyEntry deleteLogic = entry.Properties.FirstOrDefault(c => c.Metadata.Name == nameof(auditEntry.Deleted));
                string lastAction = GetLastAction(entry);
                //PropertyEntry lastAction = entry.Properties.FirstOrDefault(c => c.Metadata.Name == nameof(auditEntry.LastAction));

                //if (deleteLogic != null) isDeleteLogic = auditEntry.Deleted = (bool)deleteLogic.CurrentValue;
                //if (lastAction != null) auditEntry.LastAction = lastAction;
                //auditEntry.LastActionDate = DateTime.UtcNow;

                // se actualiza las propiedades de usuario y username para entidad a tratar
                PropertyEntry idUser = entry.Properties.FirstOrDefault(c => c.Metadata.Name == nameof(auditEntry.IdUser));
                PropertyEntry userName = entry.Properties.FirstOrDefault(c => c.Metadata.Name == nameof(auditEntry.UserName));
                PropertyEntry lastActionEntry = entry.Properties.FirstOrDefault(c => c.Metadata.Name == nameof(auditEntry.LastAction));
                PropertyEntry lastActionDateEntry = entry.Properties.FirstOrDefault(c => c.Metadata.Name == nameof(auditEntry.LastActionDate));
                idUser.CurrentValue = userInfoAccesor.IdUser;
                userName.CurrentValue = userInfoAccesor.UserFullName;
                lastActionEntry.CurrentValue = lastAction;
                lastActionDateEntry.CurrentValue = DateTime.UtcNow;
                // *************************************************************************************************************

                //foreach (var property in entry.Properties)
                //{
                //    if (property.IsTemporary)
                //    {
                //        // value will be generated by the database, get the value after saving
                //        auditEntry.TemporaryProperties.Add(property);
                //        continue;
                //    }

                //    string propertyName = property.Metadata.Name;
                //    if (property.Metadata.IsPrimaryKey())
                //    {
                //        auditEntry.KeyValue = (int)property.CurrentValue;
                //        continue;
                //    }

                //    switch (entry.State)
                //    {
                //        case EntityState.Added:
                //            auditEntry.NewValues[propertyName] = property.CurrentValue;

                //            break;

                //        case EntityState.Deleted:
                //            auditEntry.OldValues[propertyName] = property.OriginalValue;
                //            break;

                //        case EntityState.Modified:
                //            if (property.IsModified)
                //            {
                //                auditEntry.OldValues[propertyName] = property.OriginalValue;
                //                if(!isDeleteLogic) auditEntry.NewValues[propertyName] = property.CurrentValue;
                //            }
                //            break;
                //    }
                //}
            }

            // Save audit entities that have all the modifications
            //foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            //{
            //    Audits.Add(auditEntry.ToAudit());
            //}

            // keep a list of entries where the value of some properties are unknown at this step
            //return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
            return auditEntries;
        }

        private string GetLastAction(EntityEntry entry)
        {
            string lastAction = string.Empty;

            switch (entry.State)
            {
                case EntityState.Added:
                    lastAction = "CREATE";

                    break;

                case EntityState.Deleted:
                    lastAction = "DELETE";
                    break;

                case EntityState.Modified:
                    lastAction = "UPDATE";
                    break;
            }

            return lastAction;
        }

        /// <summary>
        /// OnAfterSaveChanges
        /// </summary>
        /// <param name="auditEntries"></param>
        /// <returns></returns>
        override internal Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            //if (auditEntries == null || auditEntries.Count == 0)
            //    return Task.CompletedTask;

            //foreach (var auditEntry in auditEntries)
            //{
            //    // Get the final value of the temporary properties
            //    foreach (var prop in auditEntry.TemporaryProperties)
            //    {
            //        if (prop.Metadata.IsPrimaryKey())
            //        {
            //            auditEntry.KeyValue = (int)prop.CurrentValue;
            //        }
            //        else
            //        {
            //            auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
            //        }
            //    }

            //    // Save the Audit entry
            //    Audits.Add(auditEntry.ToAudit());
            //}

            //return SaveChangesAsync();

            return Task.FromResult<object>(null);
        }

        #endregion
    }
}
