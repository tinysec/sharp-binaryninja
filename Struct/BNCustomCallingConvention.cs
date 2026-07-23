using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNCallingConventionEvent(IntPtr context);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate IntPtr BNCallingConventionGetRegisterList(
            IntPtr context,
            IntPtr count
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNCallingConventionFreeRegisterList(
            IntPtr context,
            IntPtr registers,
            UIntPtr count
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNCallingConventionGetBoolean(IntPtr context);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate uint BNCallingConventionGetRegister(IntPtr context);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNCallingConventionGetIncomingValue(
            IntPtr context,
            uint register,
            IntPtr function,
            IntPtr result
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNCallingConventionMapVariable(
            IntPtr context,
            IntPtr variable,
            IntPtr function,
            IntPtr result
        );
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNCustomCallingConvention
    {
        internal IntPtr context;

        internal IntPtr freeObject;

        internal IntPtr getCallerSavedRegisters;

        internal IntPtr getCalleeSavedRegisters;

        internal IntPtr getIntegerArgumentRegisters;

        internal IntPtr getFloatArgumentRegisters;

        internal IntPtr getRequiredArgumentRegisters;

        internal IntPtr getRequiredClobberedRegisters;

        internal IntPtr freeRegisterList;

        internal IntPtr areArgumentRegistersSharedIndex;

        internal IntPtr isStackReservedForArgumentRegisters;

        internal IntPtr isStackAdjustedOnReturn;

        internal IntPtr isEligibleForHeuristics;

        internal IntPtr getIntegerReturnValueRegister;

        internal IntPtr getHighIntegerReturnValueRegister;

        internal IntPtr getFloatReturnValueRegister;

        internal IntPtr getGlobalPointerRegister;

        internal IntPtr getImplicitlyDefinedRegisters;

        internal IntPtr getIncomingRegisterValue;

        internal IntPtr getIncomingFlagValue;

        internal IntPtr getIncomingVariableForParameterVariable;

        internal IntPtr getParameterVariableForIncomingVariable;

        internal IntPtr areArgumentRegistersUsedForVarArgs;
    }

    /// <summary>
    /// Retained for source compatibility. Custom conventions use CallingConvention directly.
    /// </summary>
    public class CustomCallingConvention
    {
    }
}
