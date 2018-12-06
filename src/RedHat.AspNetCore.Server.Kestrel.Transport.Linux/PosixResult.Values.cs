// Copyright 2017 Tom Deseyn <tom.deseyn@gmail.com>
// This software is made available under the MIT License
// See COPYING for details

using System.Collections.Generic;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal partial struct PosixResult
    {
        private static Dictionary<int, string> s_names;
        private static Dictionary<int, string> s_descriptions;

        static PosixResult()
        {
            const int count = 82;
            var nativeValues = new int[count];
            ErrorInterop.GetErrnoValues(nativeValues, count);
            var names = new Dictionary<int, string>(count);

            InitErrorInfo(ref s_E2BIG, nativeValues[0], names, "E2BIG");
            InitErrorInfo(ref s_EACCES, nativeValues[1], names, "EACCES");
            InitErrorInfo(ref s_EADDRINUSE, nativeValues[2], names, "EADDRINUSE");
            InitErrorInfo(ref s_EADDRNOTAVAIL, nativeValues[3], names, "EADDRNOTAVAIL");
            InitErrorInfo(ref s_EAFNOSUPPORT, nativeValues[4], names, "EAFNOSUPPORT");
            InitErrorInfo(ref s_EAGAIN, nativeValues[5], names, "EAGAIN");
            InitErrorInfo(ref s_EALREADY, nativeValues[6], names, "EALREADY");
            InitErrorInfo(ref s_EBADF, nativeValues[7], names, "EBADF");
            InitErrorInfo(ref s_EBADMSG, nativeValues[8], names, "EBADMSG");
            InitErrorInfo(ref s_EBUSY, nativeValues[9], names, "EBUSY");
            InitErrorInfo(ref s_ECANCELED, nativeValues[10], names, "ECANCELED");
            InitErrorInfo(ref s_ECHILD, nativeValues[11], names, "ECHILD");
            InitErrorInfo(ref s_ECONNABORTED, nativeValues[12], names, "ECONNABORTED");
            InitErrorInfo(ref s_ECONNREFUSED, nativeValues[13], names, "ECONNREFUSED");
            InitErrorInfo(ref s_ECONNRESET, nativeValues[14], names, "ECONNRESET");
            InitErrorInfo(ref s_EDEADLK, nativeValues[15], names, "EDEADLK");
            InitErrorInfo(ref s_EDESTADDRREQ, nativeValues[16], names, "EDESTADDRREQ");
            InitErrorInfo(ref s_EDOM, nativeValues[17], names, "EDOM");
            InitErrorInfo(ref s_EDQUOT, nativeValues[18], names, "EDQUOT");
            InitErrorInfo(ref s_EEXIST, nativeValues[19], names, "EEXIST");
            InitErrorInfo(ref s_EFAULT, nativeValues[20], names, "EFAULT");
            InitErrorInfo(ref s_EFBIG, nativeValues[21], names, "EFBIG");
            InitErrorInfo(ref s_EHOSTUNREACH, nativeValues[22], names, "EHOSTUNREACH");
            InitErrorInfo(ref s_EIDRM, nativeValues[23], names, "EIDRM");
            InitErrorInfo(ref s_EILSEQ, nativeValues[24], names, "EILSEQ");
            InitErrorInfo(ref s_EINPROGRESS, nativeValues[25], names, "EINPROGRESS");
            InitErrorInfo(ref s_EINTR, nativeValues[26], names, "EINTR");
            InitErrorInfo(ref s_EINVAL, nativeValues[27], names, "EINVAL");
            InitErrorInfo(ref s_EIO, nativeValues[28], names, "EIO");
            InitErrorInfo(ref s_EISCONN, nativeValues[29], names, "EISCONN");
            InitErrorInfo(ref s_EISDIR, nativeValues[30], names, "EISDIR");
            InitErrorInfo(ref s_ELOOP, nativeValues[31], names, "ELOOP");
            InitErrorInfo(ref s_EMFILE, nativeValues[32], names, "EMFILE");
            InitErrorInfo(ref s_EMLINK, nativeValues[33], names, "EMLINK");
            InitErrorInfo(ref s_EMSGSIZE, nativeValues[34], names, "EMSGSIZE");
            InitErrorInfo(ref s_EMULTIHOP, nativeValues[35], names, "EMULTIHOP");
            InitErrorInfo(ref s_ENAMETOOLONG, nativeValues[36], names, "ENAMETOOLONG");
            InitErrorInfo(ref s_ENETDOWN, nativeValues[37], names, "ENETDOWN");
            InitErrorInfo(ref s_ENETRESET, nativeValues[38], names, "ENETRESET");
            InitErrorInfo(ref s_ENETUNREACH, nativeValues[39], names, "ENETUNREACH");
            InitErrorInfo(ref s_ENFILE, nativeValues[40], names, "ENFILE");
            InitErrorInfo(ref s_ENOBUFS, nativeValues[41], names, "ENOBUFS");
            InitErrorInfo(ref s_ENODEV, nativeValues[42], names, "ENODEV");
            InitErrorInfo(ref s_ENOENT, nativeValues[43], names, "ENOENT");
            InitErrorInfo(ref s_ENOEXEC, nativeValues[44], names, "ENOEXEC");
            InitErrorInfo(ref s_ENOLCK, nativeValues[45], names, "ENOLCK");
            InitErrorInfo(ref s_ENOLINK, nativeValues[46], names, "ENOLINK");
            InitErrorInfo(ref s_ENOMEM, nativeValues[47], names, "ENOMEM");
            InitErrorInfo(ref s_ENOMSG, nativeValues[48], names, "ENOMSG");
            InitErrorInfo(ref s_ENOPROTOOPT, nativeValues[49], names, "ENOPROTOOPT");
            InitErrorInfo(ref s_ENOSPC, nativeValues[50], names, "ENOSPC");
            InitErrorInfo(ref s_ENOSYS, nativeValues[51], names, "ENOSYS");
            InitErrorInfo(ref s_ENOTCONN, nativeValues[52], names, "ENOTCONN");
            InitErrorInfo(ref s_ENOTDIR, nativeValues[53], names, "ENOTDIR");
            InitErrorInfo(ref s_ENOTEMPTY, nativeValues[54], names, "ENOTEMPTY");
            InitErrorInfo(ref s_ENOTRECOVERABLE, nativeValues[55], names, "ENOTRECOVERABLE");
            InitErrorInfo(ref s_ENOTSOCK, nativeValues[56], names, "ENOTSOCK");
            InitErrorInfo(ref s_ENOTSUP, nativeValues[57], names, "ENOTSUP");
            InitErrorInfo(ref s_ENOTTY, nativeValues[58], names, "ENOTTY");
            InitErrorInfo(ref s_ENXIO, nativeValues[59], names, "ENXIO");
            InitErrorInfo(ref s_EOVERFLOW, nativeValues[60], names, "EOVERFLOW");
            InitErrorInfo(ref s_EOWNERDEAD, nativeValues[61], names, "EOWNERDEAD");
            InitErrorInfo(ref s_EPERM, nativeValues[62], names, "EPERM");
            InitErrorInfo(ref s_EPIPE, nativeValues[63], names, "EPIPE");
            InitErrorInfo(ref s_EPROTO, nativeValues[64], names, "EPROTO");
            InitErrorInfo(ref s_EPROTONOSUPPORT, nativeValues[65], names, "EPROTONOSUPPORT");
            InitErrorInfo(ref s_EPROTOTYPE, nativeValues[66], names, "EPROTOTYPE");
            InitErrorInfo(ref s_ERANGE, nativeValues[67], names, "ERANGE");
            InitErrorInfo(ref s_EROFS, nativeValues[68], names, "EROFS");
            InitErrorInfo(ref s_ESPIPE, nativeValues[69], names, "ESPIPE");
            InitErrorInfo(ref s_ESRCH, nativeValues[70], names, "ESRCH");
            InitErrorInfo(ref s_ESTALE, nativeValues[71], names, "ESTALE");
            InitErrorInfo(ref s_ETIMEDOUT, nativeValues[72], names, "ETIMEDOUT");
            InitErrorInfo(ref s_ETXTBSY, nativeValues[73], names, "ETXTBSY");
            InitErrorInfo(ref s_EXDEV, nativeValues[74], names, "EXDEV");
            InitErrorInfo(ref s_ESOCKTNOSUPPORT, nativeValues[75], names, "ESOCKTNOSUPPORT");
            InitErrorInfo(ref s_EPFNOSUPPORT, nativeValues[76], names, "EPFNOSUPPORT");
            InitErrorInfo(ref s_ESHUTDOWN, nativeValues[77], names, "ESHUTDOWN");
            InitErrorInfo(ref s_EHOSTDOWN, nativeValues[78], names, "EHOSTDOWN");
            InitErrorInfo(ref s_ENODATA, nativeValues[79], names, "ENODATA");
            InitErrorInfo(ref s_EOPNOTSUPP, nativeValues[80], names, "EOPNOTSUPP");
            InitErrorInfo(ref s_EWOULDBLOCK, nativeValues[81], names, "EWOULDBLOCK");

            s_names = names;
            s_descriptions = new Dictionary<int, string>();
        }

        private static void InitErrorInfo(ref int s_error, in int errno, Dictionary<int, string> names,
            in string errName)
        {
            if (errno <= 0)
            {
                s_error = -1;
                return;
            }

            s_error = -errno;
            names[-errno] = errName;
        }

        public static int E2BIG => s_E2BIG;
        public static int EACCES => s_EACCES;
        public static int EADDRINUSE => s_EADDRINUSE;
        public static int EADDRNOTAVAIL => s_EADDRNOTAVAIL;
        public static int EAFNOSUPPORT => s_EAFNOSUPPORT;
        public static int EAGAIN => s_EAGAIN;
        public static int EALREADY => s_EALREADY;
        public static int EBADF => s_EBADF;
        public static int EBADMSG => s_EBADMSG;
        public static int EBUSY => s_EBUSY;
        public static int ECANCELED => s_ECANCELED;
        public static int ECHILD => s_ECHILD;
        public static int ECONNABORTED => s_ECONNABORTED;
        public static int ECONNREFUSED => s_ECONNREFUSED;
        public static int ECONNRESET => s_ECONNRESET;
        public static int EDEADLK => s_EDEADLK;
        public static int EDESTADDRREQ => s_EDESTADDRREQ;
        public static int EDOM => s_EDOM;
        public static int EDQUOT => s_EDQUOT;
        public static int EEXIST => s_EEXIST;
        public static int EFAULT => s_EFAULT;
        public static int EFBIG => s_EFBIG;
        public static int EHOSTUNREACH => s_EHOSTUNREACH;
        public static int EIDRM => s_EIDRM;
        public static int EILSEQ => s_EILSEQ;
        public static int EINPROGRESS => s_EINPROGRESS;
        public static int EINTR => s_EINTR;
        public static int EINVAL => s_EINVAL;
        public static int EIO => s_EIO;
        public static int EISCONN => s_EISCONN;
        public static int EISDIR => s_EISDIR;
        public static int ELOOP => s_ELOOP;
        public static int EMFILE => s_EMFILE;
        public static int EMLINK => s_EMLINK;
        public static int EMSGSIZE => s_EMSGSIZE;
        public static int EMULTIHOP => s_EMULTIHOP;
        public static int ENAMETOOLONG => s_ENAMETOOLONG;
        public static int ENETDOWN => s_ENETDOWN;
        public static int ENETRESET => s_ENETRESET;
        public static int ENETUNREACH => s_ENETUNREACH;
        public static int ENFILE => s_ENFILE;
        public static int ENOBUFS => s_ENOBUFS;
        public static int ENODEV => s_ENODEV;
        public static int ENOENT => s_ENOENT;
        public static int ENOEXEC => s_ENOEXEC;
        public static int ENOLCK => s_ENOLCK;
        public static int ENOLINK => s_ENOLINK;
        public static int ENOMEM => s_ENOMEM;
        public static int ENOMSG => s_ENOMSG;
        public static int ENOPROTOOPT => s_ENOPROTOOPT;
        public static int ENOSPC => s_ENOSPC;
        public static int ENOSYS => s_ENOSYS;
        public static int ENOTCONN => s_ENOTCONN;
        public static int ENOTDIR => s_ENOTDIR;
        public static int ENOTEMPTY => s_ENOTEMPTY;
        public static int ENOTRECOVERABLE => s_ENOTRECOVERABLE;
        public static int ENOTSOCK => s_ENOTSOCK;
        public static int ENOTSUP => s_ENOTSUP;
        public static int ENOTTY => s_ENOTTY;
        public static int ENXIO => s_ENXIO;
        public static int EOVERFLOW => s_EOVERFLOW;
        public static int EOWNERDEAD => s_EOWNERDEAD;
        public static int EPERM => s_EPERM;
        public static int EPIPE => s_EPIPE;
        public static int EPROTO => s_EPROTO;
        public static int EPROTONOSUPPORT => s_EPROTONOSUPPORT;
        public static int EPROTOTYPE => s_EPROTOTYPE;
        public static int ERANGE => s_ERANGE;
        public static int EROFS => s_EROFS;
        public static int ESPIPE => s_ESPIPE;
        public static int ESRCH => s_ESRCH;
        public static int ESTALE => s_ESTALE;
        public static int ETIMEDOUT => s_ETIMEDOUT;
        public static int ETXTBSY => s_ETXTBSY;
        public static int EXDEV => s_EXDEV;
        public static int ESOCKTNOSUPPORT => s_ESOCKTNOSUPPORT;
        public static int EPFNOSUPPORT => s_EPFNOSUPPORT;
        public static int ESHUTDOWN => s_ESHUTDOWN;
        public static int EHOSTDOWN => s_EHOSTDOWN;
        public static int ENODATA => s_ENODATA;
        public static int EOPNOTSUPP => s_EOPNOTSUPP;
        public static int EWOULDBLOCK => s_EWOULDBLOCK;

        private static int s_E2BIG;
        private static int s_EACCES;
        private static int s_EADDRINUSE;
        private static int s_EADDRNOTAVAIL;
        private static int s_EAFNOSUPPORT;
        private static int s_EAGAIN;
        private static int s_EALREADY;
        private static int s_EBADF;
        private static int s_EBADMSG;
        private static int s_EBUSY;
        private static int s_ECANCELED;
        private static int s_ECHILD;
        private static int s_ECONNABORTED;
        private static int s_ECONNREFUSED;
        private static int s_ECONNRESET;
        private static int s_EDEADLK;
        private static int s_EDESTADDRREQ;
        private static int s_EDOM;
        private static int s_EDQUOT;
        private static int s_EEXIST;
        private static int s_EFAULT;
        private static int s_EFBIG;
        private static int s_EHOSTUNREACH;
        private static int s_EIDRM;
        private static int s_EILSEQ;
        private static int s_EINPROGRESS;
        private static int s_EINTR;
        private static int s_EINVAL;
        private static int s_EIO;
        private static int s_EISCONN;
        private static int s_EISDIR;
        private static int s_ELOOP;
        private static int s_EMFILE;
        private static int s_EMLINK;
        private static int s_EMSGSIZE;
        private static int s_EMULTIHOP;
        private static int s_ENAMETOOLONG;
        private static int s_ENETDOWN;
        private static int s_ENETRESET;
        private static int s_ENETUNREACH;
        private static int s_ENFILE;
        private static int s_ENOBUFS;
        private static int s_ENODEV;
        private static int s_ENOENT;
        private static int s_ENOEXEC;
        private static int s_ENOLCK;
        private static int s_ENOLINK;
        private static int s_ENOMEM;
        private static int s_ENOMSG;
        private static int s_ENOPROTOOPT;
        private static int s_ENOSPC;
        private static int s_ENOSYS;
        private static int s_ENOTCONN;
        private static int s_ENOTDIR;
        private static int s_ENOTEMPTY;
        private static int s_ENOTRECOVERABLE;
        private static int s_ENOTSOCK;
        private static int s_ENOTSUP;
        private static int s_ENOTTY;
        private static int s_ENXIO;
        private static int s_EOVERFLOW;
        private static int s_EOWNERDEAD;
        private static int s_EPERM;
        private static int s_EPIPE;
        private static int s_EPROTO;
        private static int s_EPROTONOSUPPORT;
        private static int s_EPROTOTYPE;
        private static int s_ERANGE;
        private static int s_EROFS;
        private static int s_ESPIPE;
        private static int s_ESRCH;
        private static int s_ESTALE;
        private static int s_ETIMEDOUT;
        private static int s_ETXTBSY;
        private static int s_EXDEV;
        private static int s_ESOCKTNOSUPPORT;
        private static int s_EPFNOSUPPORT;
        private static int s_ESHUTDOWN;
        private static int s_EHOSTDOWN;
        private static int s_ENODATA;
        private static int s_EOPNOTSUPP;
        private static int s_EWOULDBLOCK;
    }
}