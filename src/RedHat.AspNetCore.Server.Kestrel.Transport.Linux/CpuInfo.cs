using System.Collections.Generic;
using System.IO;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal class CpuInfo
    {
        private class LogicalCpuInfo
        {
            public int Id;
            public string SocketId;
            public string CoreId;
        }

        private static readonly LogicalCpuInfo[] _cpuInfos = GetCpuInfos();

        private static LogicalCpuInfo[] GetCpuInfos()
        {
            const string sysPath = "/sys/devices/system/cpu";
            var directories = Directory.GetDirectories(sysPath, "cpu*");
            var cpuInfos = new List<LogicalCpuInfo>();
            foreach (var directory in directories)
            {
                if (int.TryParse(directory.Substring(sysPath.Length + 4), out int id))
                {
                    var cpuInfo = new LogicalCpuInfo
                    {
                        Id = id,
                        SocketId = File.ReadAllText($"{sysPath}/cpu{id}/topology/physical_package_id").Trim(),
                        CoreId = File.ReadAllText($"{sysPath}/cpu{id}/topology/core_id").Trim()
                    };
                    
                    cpuInfos.Add(cpuInfo);
                }
            }

            return cpuInfos.ToArray();
        }

        public static IEnumerable<string> GetSockets()
        {
            for (int i = 0; i < _cpuInfos.Length; i++)
            {
                var socket = _cpuInfos[i].SocketId;
                bool duplicate = false;
                
                for (int j = 0; j < i; j++)
                {
                    if (socket == _cpuInfos[j].SocketId)
                    {
                        duplicate = true;
                        break;
                    }
                }
                
                if (!duplicate)
                {
                    yield return socket;
                }
            }
        }
        
        public static IEnumerable<string> GetCores(string socket)
        {
            for (int i = 0; i < _cpuInfos.Length; i++)
            {
                var cpuInfo = _cpuInfos[i];
                
                if (cpuInfo.SocketId != socket)
                {
                    continue;
                }
                
                var core = _cpuInfos[i].CoreId;
                bool duplicate = false;
                
                for (int j = 0; j < i; j++)
                {
                    if (_cpuInfos[j].SocketId != socket
                        && core != _cpuInfos[j].CoreId)
                    {
                        continue;
                    }

                    duplicate = true;
                    break;
                }
                
                if (!duplicate)
                
                {
                    yield return core;
                }
            }
        }
        
        public static IEnumerable<int> GetCpuIds(string socket, string core)
        {
            for (int i = 0; i < _cpuInfos.Length; i++)
            {
                var cpuInfo = _cpuInfos[i];
                if (cpuInfo.SocketId != socket || cpuInfo.CoreId != core)
                {
                    continue;
                }
                
                yield return _cpuInfos[i].Id;
            }
        }
        
        public static int GetAvailableCpus() 
            => _cpuInfos.Length;
    }
}