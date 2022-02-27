// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
using System;

namespace Internal.Runtime.CompilerHelpers
{
    internal static class SynchronizedMethodHelpers
    {
        private static void MonitorEnterStatic(IntPtr pEEType, ref bool lockTaken) { lockTaken = true; }

        private static void MonitorExitStatic(IntPtr pEEType, ref bool lockTaken) { lockTaken = false; }
    }
}
