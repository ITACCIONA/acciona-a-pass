using Microsoft.Extensions.DependencyInjection;
using System;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Interfaz de configuración del servicio
    /// Source: https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScheduleConfig<T>
    {
        /// <summary>
        /// Manejador de expressiones cron
        /// </summary>
        ICronHandlerProvider CronHandler { get; set; }

        /// <summary>
        /// Expresión en fomrato cron
        /// </summary>
        string CronExpression { get; set; }

        /// <summary>
        /// Zona horaria para el cálculo de las itereaciones
        /// </summary>
        TimeZoneInfo TimeZoneInfo { get; set; }
    }

    /// <summary>
    /// Interfaz de configuración del servicio
    /// Source: https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        /// <summary>
        /// Manejador de expressiones cron
        /// </summary>
        public ICronHandlerProvider CronHandler { get; set; }

        /// <summary>
        /// Expresión en fomrato cron
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// Zona horaria para el cálculo de las itereaciones
        /// </summary>
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }

    /// <summary>
    /// Extensiones de agragación de los servcios de cron en segundo plano
    /// Source: https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06
    /// </summary>
    public static class ScheduledServiceExtensions
    {
        /// <summary>
        /// Agrega un servicio de ejecución en segundo plano con la configuración temporal facilitada mediante una expresión en formato cron
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="hostedServiceResolver"></param>
        /// <returns></returns>
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : ICronWorker
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }
            var config = new ScheduleConfig<T>();
            options.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            if (config.CronHandler == null)
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronHandler), @"Cron handler is mandatory.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<CronJob<T>>();

            return services;
        }

        /// <summary>
        /// Agrega un servicio de ejecución en segundo plano con la configuración temporal facilitada mediante una expresión en formato cron
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="hostedServiceResolver"></param>
        /// <returns></returns>
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options, Func<IServiceProvider, CronJob<T>> hostedServiceResolver) where T : ICronWorker
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }
            var config = new ScheduleConfig<T>();
            options.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            if (config.CronHandler == null)
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronHandler), @"Cron handler is mandatory.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService(hostedServiceResolver);

            return services;
        }
    }
}
