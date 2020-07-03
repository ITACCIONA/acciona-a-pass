using AccionaCovid.Application.Core;
using AccionaCovid.Crosscutting;
using AccionaCovid.Crosscutting.Domain;
using AccionaCovid.Data;
using AccionaCovid.Data.Seed;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Services;
using AccionaCovid.WebApi.Core;
using AccionaCovid.WebApi.HostedServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace AccionaCovid.WebApi
{
    /// <summary>
    /// Clase que representa la configuracion de autofac
    /// </summary>
    public static class AutofacConfiguration
    {
        /// <summary>
        /// ConfigureAutofac
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        /// <param name="logicalRemove"></param>
        /// <returns></returns>
        public static IContainer ConfigureAutofac(IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration, bool logicalRemove)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);

            ConfigureContext(builder, configuration);
            ConfigureMediatR(builder);
            ConfigureRepositories(builder, environment, configuration, logicalRemove);
            ConfigureHostedServices(builder);

            builder.RegisterAssemblyTypes(Assembly.Load("AccionaCovid.Application"))
                        .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();

            builder.RegisterType<DataSeeder>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<CreatePassportService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<AzureSharedResFileStgService>().AsImplementedInterfaces()
                .WithParameter("connectionString", configuration.GetConnectionString("AccionaCovidFileStorage"))
                .InstancePerLifetimeScope();
            builder.Register(c =>
            {
                var integrationSettings = new IntegrationSettings();
                configuration.GetSection("InfrastructureSettings:IntegrationSettings").Bind(integrationSettings);
                return integrationSettings;
            }).SingleInstance();
            //builder.RegisterType<MockFileStorageService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ZipFilesRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<AppInsightsTelemetryService>().AsImplementedInterfaces().InstancePerLifetimeScope();


            return builder.Build();
        }

        /// <summary>
        /// Configuracion de mediatR
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        private static void ConfigureMediatR(ContainerBuilder builder)
        {
            /* Mediator */
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                IComponentContext c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            /* Pipelines */
            builder.RegisterGeneric(typeof(LogPipeline<,>))
                .As(typeof(IPipelineBehavior<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ValidatorPipeline<,>))
                .As(typeof(IPipelineBehavior<,>))
                .InstancePerLifetimeScope();

            /* Handlers */
            builder.RegisterAssemblyTypes(Assembly.Load("AccionaCovid.Application"))
                        .Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(Assembly.Load("AccionaCovid.Application"))
            //            .Where(t => t.IsClosedTypeOf(typeof(INotificationHandler<>)))
            //            .AsImplementedInterfaces()
            //            .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Configuracion de los repositorios
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        /// <param name="logicalRemove"></param>
        private static void ConfigureRepositories(ContainerBuilder builder, IWebHostEnvironment environment, IConfiguration configuration, bool logicalRemove)
        {
            /* Custom Repositories */
            builder.RegisterAssemblyTypes(typeof(GenericRepository<>).Assembly)
                        .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)))
                        .WithParameter(new TypedParameter(typeof(bool), logicalRemove))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();

            /* Generic Repositories */
            builder.RegisterGeneric(typeof(GenericRepository<>)).WithParameter(new TypedParameter(typeof(bool), logicalRemove)).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Configuracion del contexto de datos
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="configuration">IConfiguration</param>
        private static void ConfigureContext(ContainerBuilder builder, IConfiguration configuration)
        {
            string sqlConnection = configuration.GetConnectionString("AccionaCovidConnection");

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AccionaCovidContext>().UseSqlServer(sqlConnection);

            builder.RegisterType<AccionaCovidContext>()
                .WithParameter("options", dbContextOptionsBuilder.Options)
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Configuración de servicios en segundo plano
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        private static void ConfigureHostedServices(ContainerBuilder builder)
        {
            builder.Register<Func<Type, ICronWorker>>(ctx => (t) => {
                var lifetimeScope = ctx.Resolve<ILifetimeScope>();
                var scope = lifetimeScope.BeginLifetimeScope(builder =>
                                                builder.RegisterType<UserInfoAccesorHostedServices>()
                                                .AsImplementedInterfaces().InstancePerLifetimeScope());
                return (ICronWorker)scope.Resolve(t);
            });

            builder.RegisterType<IntegrationFileStorageCronWorker>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
