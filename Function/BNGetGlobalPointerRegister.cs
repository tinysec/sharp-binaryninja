using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
        [DllImport(
            "binaryninjacore",
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetGlobalPointerRegister"
        )]
        internal static extern uint BNGetGlobalPointerRegister(IntPtr convention);
    }
}
