using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VerificationsService.Abstraction;
using VerificationsService.Service.CronJobService.CronJobMethods;

namespace VerificationsService.Service.CronJobService.CronJobMethods
{
    public class ClearVerifications : CronJobService, IDisposable
    {
        private readonly IServiceScope _scope;
        public ClearVerifications(IScheduleConfig<ClearVerifications> config, IServiceProvider scope) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _scope = scope.CreateScope();
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override Task DoWork(CancellationToken cancellationToken)
        {
            IVerificationsRepository _verificationRepository = _scope.ServiceProvider.GetRequiredService<IVerificationsRepository>();
            try
            {
                _verificationRepository.RemoveALLFailedVerifications();
            }
            catch (Exception) { }
            return Task.CompletedTask;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _scope?.Dispose();
        }
    }
}