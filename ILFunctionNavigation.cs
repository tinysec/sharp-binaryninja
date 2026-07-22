using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    /// <summary>
    /// Shared native-list handling for IL function form navigation.
    /// </summary>
    internal static class ILFunctionNavigation
    {
        /// <summary>
        /// Takes ownership of a native basic-block list, reads its graph type, and frees the list.
        /// </summary>
        internal static FunctionGraphType TakeFunctionGraphType(IntPtr blocks, ulong count)
        {
            try
            {
                if (IntPtr.Zero == blocks || 0 == count)
                {
                    return FunctionGraphType.InvalidILViewType;
                }

                IntPtr firstBlock = Marshal.ReadIntPtr(blocks);
                return NativeMethods.BNGetBasicBlockFunctionGraphType(firstBlock);
            }
            finally
            {
                if (IntPtr.Zero != blocks)
                {
                    NativeMethods.BNFreeBasicBlockList(blocks, count);
                }
            }
        }
    }
}
