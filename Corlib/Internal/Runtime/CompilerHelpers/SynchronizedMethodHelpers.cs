using System;

namespace Internal.Runtime.CompilerHelpers
{
    internal static class SynchronizedMethodHelpers
    {
        private static void MonitorEnter(object obj, ref bool lockTaken) { lockTaken = true; }

        private static void MonitorExit(object obj, ref bool lockTaken) { lockTaken = false; }

        private static void MonitorEnterStatic(IntPtr pEEType, ref bool lockTaken) { lockTaken = true; }

        private static void MonitorExitStatic(IntPtr pEEType, ref bool lockTaken) { lockTaken = false; }
    }
}