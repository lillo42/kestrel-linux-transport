using System.Threading.Tasks;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal interface ITransportActionHandler
    {
        Task BindAsync();
        Task UnbindAsync();
        Task StopAsync();
    }
}