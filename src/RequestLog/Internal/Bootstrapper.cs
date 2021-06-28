using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RequestLog.Persistence;

namespace RequestLog.Internal
{
    /// <summary>
    /// Default implement of
    /// </summary>
    internal class Bootstrapper : BackgroundService, IBootstrapper
    {
        public Bootstrapper(IStorageInitializer storage)
        {
            Storage = storage;
        }

        private IStorageInitializer Storage { get; }

        public async Task BootstrapAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Storage.InitializeAsync(stoppingToken);
                await Storage.CheckAndUpdate(stoppingToken);
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await BootstrapAsync(stoppingToken);
        }
    }
}
