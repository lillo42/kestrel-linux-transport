using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    [TypeConverter(typeof(CpuSetTypeConverter))]
    internal struct CpuSet
    {
        private readonly int[] _cpus;

        public int[] Cpus => _cpus ?? Array.Empty<int>();

        public bool IsEmpty => _cpus == null || _cpus.Length == 0;

        private CpuSet(int[] cpus)
        {
            _cpus = cpus;
        }

        private static bool ParseFailed(in bool tryParse, in string error)
        {
            if (tryParse)
            {
                return false;
            }

            throw new FormatException(error);
        }

        public static bool Parse(in string set, out CpuSet cpus, in bool tryParse)
        {
            cpus = default;

            if (set == null)
            {
                if (tryParse)
                {
                    return false;
                }

                throw new ArgumentNullException(nameof(set));
            }

            if (set.Length == 0)
            {
                cpus = new CpuSet(Array.Empty<int>());
                return true;
            }

            int index = 0;

            var cpuList = new List<int>();

            do
            {
                if (!TryParseNumber(set, ref index, out int start))
                {
                    return ParseFailed(tryParse, $"Can not parse number at {index}");
                }

                if (index == set.Length)
                {
                    cpuList.Add(start);
                    break;
                }
                else if (set[index] == ',')
                {
                    cpuList.Add(start);
                    index++;
                }
                else if (set[index] == '-')
                {
                    index++;
                    if (!TryParseNumber(set, ref index, out int end))
                    {
                        return ParseFailed(tryParse, $"Can not parse number at {index}");
                    }

                    if (start > end)
                    {
                        return ParseFailed(tryParse, "End of range is larger than start");
                    }

                    for (int i = start; i <= end; i++)
                    {
                        cpuList.Add(i);
                    }

                    if (index == set.Length)
                    {
                        break;
                    }
                    else if (set[index] == ',')
                    {
                        index++;
                    }
                    else
                    {
                        return ParseFailed(tryParse, $"Invalid character at {index}: '{set[index]}'");
                    }
                }
                else
                {
                    return ParseFailed(tryParse, $"Invalid character at {index}: '{set[index]}'");
                }
            } while (index != set.Length);

            var cpuArray = cpuList.ToArray();
            Array.Sort(cpuArray);
            cpus = new CpuSet(cpuArray);
            return true;
        }

        public static bool TryParse(in string set, out CpuSet cpus)
        {
            return Parse(set, out cpus, tryParse: true);
        }

        public static CpuSet Parse(in string set)
        {
            Parse(set, out CpuSet cpus, tryParse: false);
            return cpus;
        }

        private static bool TryParseNumber(in string s, ref int index, out int value)
        {
            if (index == s.Length)
            {
                value = 0;
                return false;
            }

            int startIndex = index;
            
            while (index < s.Length && char.IsDigit(s[index]))
            {
                index++;
            }

            return int.TryParse(s.Substring(startIndex, index - startIndex), out value);
        }

        public override string ToString() 
            => _cpus == null ? string.Empty : string.Join(",", _cpus);
    }
}