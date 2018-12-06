// Copyright 2017 Tom Deseyn <tom.deseyn@gmail.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Runtime.InteropServices;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal static class IOInterop
    {
        [DllImport(Interop.Library, EntryPoint = "RHXKL_Close")]
        public static extern PosixResult Close(int handle);

        [DllImport(Interop.Library, EntryPoint = "RHXKL_Write")]
        public static extern unsafe PosixResult Write(SafeHandle handle, byte* buf, int count);

        [DllImport(Interop.Library, EntryPoint = "RHXKL_Read")]
        public static extern unsafe PosixResult Read(SafeHandle handle, byte* buf, int count);
    }
}