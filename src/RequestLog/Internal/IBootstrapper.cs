using System.Threading;
using System.Threading.Tasks;

namespace RequestLog.Internal
{
    /// <summary>
    /// Represents bootstrapping logic. For example, adding initial state to the storage or querying certain entities.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        Task BootstrapAsync(CancellationToken stoppingToken);
    }
}
