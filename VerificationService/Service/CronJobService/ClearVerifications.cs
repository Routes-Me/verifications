using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using VerificationService.Abstraction;
using VerificationService.Service.CronJobService.CronJobMethods;

namespace VerificationService.Service.CronJobService
{
    public class ClearVerifications : CronJobServices, IDisposable
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
            IVerificationRepository _verificationRepository = _scope.ServiceProvider.GetRequiredService<IVerificationRepository>();
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