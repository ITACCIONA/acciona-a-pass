using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Abstrac class for scheduling task in background
    /// Source: https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06 (Muy Tuneado)
    /// </summary>
    public class CronJob<T> : IHostedService, IDisposable where T : ICronWorker
    {
        private System.Timers.Timer _timer;
        private readonly ICronHandlerProvider _cronHandler;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly T _worker;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="worker"></param>
        public CronJob(IScheduleConfig<T> options, Func<Type, ICronWorker> cronFactory)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _cronHandler = options.CronHandler;
            _cronHandler.Expression = options.CronExpression;
            _timeZoneInfo = options.TimeZoneInfo;
            _worker = (T)cronFactory(typeof(T));
        }

        /// <summary>
        /// Start cron task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        /// <summary>
        /// Core do work task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _cronHandler.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Stop();  // reset timer
                    await DoWork(cancellationToken);
                    await ScheduleJob(cancellationToken);    // reschedule next
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Do Work base - override
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await _worker.DoWork();  // do the work
        }

        /// <summary>
        /// Stop method base
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool dispose)
        {
            if (dispose) _timer?.Dispose();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
