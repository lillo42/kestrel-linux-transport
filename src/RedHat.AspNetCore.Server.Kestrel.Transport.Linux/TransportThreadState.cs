namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal enum TransportThreadState
    {
        Initial,
        Starting,
        Started,
        ClosingAccept,
        AcceptClosed,
        Stopping,
        Stopped
    }
}