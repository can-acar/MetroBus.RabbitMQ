using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace test.services.Service
{
    public class BusService : IHostedService
    {
        private readonly IBusControl BusControl;


        public BusService(IBusControl busControl)
        {
            BusControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return BusControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return BusControl.StopAsync(cancellationToken);
        }
    }
}