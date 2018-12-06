// Copyright 2017 Tom Deseyn <tom.deseyn@gmail.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.IO;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal partial struct PosixResult
    {
        private int _value;

        public int Value => _value;

        public PosixResult(int value)
        {
            _value = value;
        }

        public bool IsSuccess => _value >= 0;

        internal string ErrorDescription()
        {
            if (_value >= 0)
            {
                return string.Empty;
            }

            lock (s_descriptions)
            {
                if (s_descriptions.TryGetValue(_value, out string description))
                {
                    return description;
                }
                
                description = ErrorInterop.StrError(-_value);
                s_descriptions.Add(_value, description);
                return description;
            }
        }

        internal string ErrorName()
        {
            if (_value < 0)
            {
                if (s_names.TryGetValue(_value, out string name))
                {
                    return name;
                }
                
                return $"E{-_value}";
            }

            return string.Empty;
        }

        public Exception AsException()
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException($"{nameof(PosixResult)} is not an error.");
            }
            
            return new IOException(ErrorName(), _value);
        }

        public void ThrowOnError()
        {
            if (!IsSuccess)
            {
                ThrowException();
            }
        }

        private void ThrowException()
        {
            throw AsException();
        }

        public static implicit operator bool(PosixResult result)
        {
            return result.IsSuccess;
        }

        public override bool Equals (object obj)
        {
            var other = obj as PosixResult?;
            if (other == null)
            {
                return false;
            }
            return _value == other.Value._value;
        }

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString()
        {
            if (IsSuccess)
            {
                return _value.ToString();
            }

            return ErrorName();
        }

        public static bool operator==(PosixResult lhs, int nativeValue)
        {
            return lhs._value == nativeValue;
        }

        public static bool operator!=(PosixResult lhs, int nativeValue)
        {
            return lhs._value != nativeValue;
        }

        public static bool operator==(PosixResult lhs, PosixResult rhs)
        {
            return lhs._value == rhs._value;
        }

        public static bool operator!=(PosixResult lhs, PosixResult rhs)
        {
            return lhs._value != rhs._value;
        }
    }
}