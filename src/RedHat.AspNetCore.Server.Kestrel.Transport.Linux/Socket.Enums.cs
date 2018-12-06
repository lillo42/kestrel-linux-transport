namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal enum AddressFamily
    {
        Unspecified = 0,
        Unix = 1,
        InterNetwork = 2,
        InterNetworkV6 = 23
    }

    internal enum SocketType
    {
        Stream = 1,
        Dgram = 2,
        Raw = 3,
        Rdm = 4,
        Seqpacket = 5
    }

    internal enum ProtocolType
    {
        Unspecified = 0,
        Icmp = 1,
        Tcp = 6,
        Udp = 17,
        Icmpv6 = 58
    }

    internal enum SocketShutdown
    {
        // Shutdown sockets for receive.
        Receive = 0x00,
        // Shutdown socket for send.
        Send = 0x01,
        // Shutdown socket for both send and receive.
        Both = 0x02,
    }

    internal enum SocketOptionLevel
    {
        // Indicates socket options apply to the socket itself.
        Socket = 0xffff,

        // Indicates socket options apply to IP sockets.
        IP = 0,

        // Indicates socket options apply to IPv6 sockets.
        IPv6 = 41,

        // Indicates socket options apply to Tcp sockets.
        Tcp = ProtocolType.Tcp,

        // Indicates socket options apply to Udp sockets.
        Udp = ProtocolType.Udp,
    }

    internal enum SocketOptionName
    {
        #region SocketOptionLevel.Socket
        // Record debugging information.
        Debug = 0x0001,

        // Socket is listening.
        AcceptConnection = 0x0002,

        // Allows the socket to be bound to an address that is already in use.
        ReuseAddress = 0x0004,

        // Send keep-alives.
        KeepAlive = 0x0008,

        // Do not route, send directly to interface addresses.
        DontRoute = 0x0010,

        // Permit sending broadcast messages on the socket.
        Broadcast = 0x0020,

        // Bypass hardware when possible.
        UseLoopback = 0x0040,

        // Linger on close if unsent data is present.
        //Linger = 0x0080,

        // Receives out-of-band data in the normal data stream.
        OutOfBandInline = 0x0100,

        // Close socket gracefully without lingering.
        //DontLinger = ~Linger,

        // Enables a socket to be bound for exclusive access.
        //ExclusiveAddressUse = ~ReuseAddress,

        // Specifies the total per-socket buffer space reserved for sends. This is
        // unrelated to the maximum message size or the size of a TCP window.
        SendBuffer = 0x1001,

        // Send low water mark.
        ReceiveBuffer = 0x1002,

        // Specifies the total per-socket buffer space reserved for receives. This is unrelated to the maximum message size or the size of a TCP window.
        //SendLowWater = 0x1003,

        // Receive low water mark.
        //ReceiveLowWater = 0x1004,

        // Send timeout.
        //SendTimeout = 0x1005,

        // Receive timeout.
        //ReceiveTimeout = 0x1006,

        // Get error status and clear.
        Error = 0x1007,

        // Get socket type.
        Type = 0x1008,

        // Allow ephemeral port reuse for outbound connections.
        //ReuseUnicastPort = 0x3007,

        // Maximum queue length that can be specified by <see cref='System.Net.Sockets.Socket.Listen'/>.
        //MaxConnections = 0x7fffffff,
        ReusePort = 0x2001,

        IncomingCpu = 0x2002,

        ZeroCopy = 0x2003,
        #endregion

        // The following values are taken from ws2tcpip.h,
        // note that these are understood only by ws2_32.dll and are not backwards compatible
        // with the values found in winsock.h which are understood by wsock32.dll.

        #region SocketOptionLevel.IP
        // IP options.
        /*IPOptions = 1,

        // Header is included with data.
        HeaderIncluded = 2,

        // IP type of service and preced.
        TypeOfService = 3,

        // IP time to live.
        IpTimeToLive = 4,

        // IP multicast interface.
        MulticastInterface = 9,

        // IP multicast time to live.
        MulticastTimeToLive = 10,

        // IP Multicast loopback.
        MulticastLoopback = 11,

        // Add an IP group membership.
        AddMembership = 12,

        // Drop an IP group membership.
        DropMembership = 13,

        // Don't fragment IP datagrams.
        DontFragment = 14,

        // Join IP group/source.
        AddSourceMembership = 15,

        // Leave IP group/source.
        DropSourceMembership = 16,

        // Block IP group/source.
        BlockSource = 17,

        // Unblock IP group/source.
        UnblockSource = 18,

        // Receive packet information for ipv4.
        PacketInformation = 19,
        #endregion

        #region SocketOptionLevel.IPv6
        HopLimit = 21,

        IPProtectionLevel = 23,*/

        IPv6Only = 27,
        #endregion

        #region SocketOptionLevel.Tcp
        // Disables the Nagle algorithm for send coalescing.
        NoDelay = 1,
        //BsdUrgent = 2,
        //Expedited = 2,
        DeferAccept = 3,
        #endregion

        #region SocketOptionlevel.Udp
        /*NoChecksum = 1,

        // Udp-Lite checksum coverage.
        ChecksumCoverage = 20,

        UpdateAcceptContext = 0x700B,

        UpdateConnectContext = 0x7010,*/
        #endregion
    }

}