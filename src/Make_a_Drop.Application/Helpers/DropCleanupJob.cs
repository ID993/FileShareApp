using Make_a_Drop.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Make_a_Drop.Application.Helpers
{
    public class DropCleanupJob : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public DropCleanupJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoCleanupTask, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void DoCleanupTask(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var dropService = scope.ServiceProvider.GetRequiredService<IDropService>();
            dropService.DeleteExpiredAsync().Wait();
        }

        public void Dispose()
        {
            _timer?.Dispose();
            //GC.SuppressFinalize(this);
        }
    }
}
