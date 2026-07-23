using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public partial class CallingConvention
    {
        private IntPtr InvokeCallerSaved(IntPtr context, IntPtr count)
        {
            return this.InvokeRegisterList(
                this.GetCallerSavedRegisters,
                count,
                "GetCallerSavedRegisters"
            );
        }

        private IntPtr InvokeCalleeSaved(IntPtr context, IntPtr count)
        {
            return this.InvokeRegisterList(
                this.GetCalleeSavedRegisters,
                count,
                "GetCalleeSavedRegisters"
            );
        }

        private IntPtr InvokeIntegerArguments(IntPtr context, IntPtr count)
        {
            return this.InvokeRegisterList(
                this.GetIntegerArgumentRegisters,
                count,
                "GetIntegerArgumentRegisters"
            );
        }

        private IntPtr InvokeFloatArguments(IntPtr context, IntPtr count)
        {
            return this.InvokeRegisterList(
                this.GetFloatArgumentRegisters,
                count,
                "GetFloatArgumentRegisters"
            );
        }

        private IntPtr InvokeRequiredArguments(IntPtr context, IntPtr count)
        {
            return this.InvokeRegisterList(
                this.GetRequiredArgumentRegisters,
                count,
                "GetRequiredArgumentRegisters"
            );
        }

        private IntPtr InvokeRequiredClobbered(IntPtr context, IntPtr count)
        {
            return this.InvokeRegisterList(
                this.GetRequiredClobberedRegisters,
                count,
                "GetRequiredClobberedRegisters"
            );
        }

        private IntPtr InvokeImplicitlyDefined(IntPtr context, IntPtr count)
        {
            return this.InvokeRegisterList(
                this.GetImplicitlyDefinedRegisters,
                count,
                "GetImplicitlyDefinedRegisters"
            );
        }

        private bool InvokeSharedIndex(IntPtr context)
        {
            try
            {
                return this.AreArgumentRegistersSharedIndex;
            }
            catch (Exception exception)
            {
                this.LogCustomCallbackException("AreArgumentRegistersSharedIndex", exception);

                return false;
            }
        }

        private bool InvokeStackReserved(IntPtr context)
        {
            try
            {
                return this.IsStackReservedForArgumentRegisters;
            }
            catch (Exception exception)
            {
                this.LogCustomCallbackException(
                    "IsStackReservedForArgumentRegisters",
                    exception
                );

                return false;
            }
        }

        private bool InvokeStackAdjusted(IntPtr context)
        {
            try
            {
                return this.IsStackAdjustedOnReturn;
            }
            catch (Exception exception)
            {
                this.LogCustomCallbackException("IsStackAdjustedOnReturn", exception);

                return false;
            }
        }

        private bool InvokeHeuristics(IntPtr context)
        {
            try
            {
                return this.IsEligibleForHeuristics;
            }
            catch (Exception exception)
            {
                this.LogCustomCallbackException("IsEligibleForHeuristics", exception);

                return true;
            }
        }

        private bool InvokeVarArgs(IntPtr context)
        {
            try
            {
                return this.AreArgumentRegistersUsedForVarArgs;
            }
            catch (Exception exception)
            {
                this.LogCustomCallbackException(
                    "AreArgumentRegistersUsedForVarArgs",
                    exception
                );

                return true;
            }
        }

        private uint InvokeIntegerReturn(IntPtr context)
        {
            return this.InvokeRegister(this.ReadIntegerReturn, "IntegerReturnValueRegister");
        }

        private uint InvokeHighIntegerReturn(IntPtr context)
        {
            return this.InvokeRegister(
                this.ReadHighIntegerReturn,
                "HighIntegerReturnValueRegister"
            );
        }

        private uint InvokeFloatReturn(IntPtr context)
        {
            return this.InvokeRegister(this.ReadFloatReturn, "FloatReturnValueRegister");
        }

        private uint InvokeGlobalPointer(IntPtr context)
        {
            return this.InvokeRegister(this.ReadGlobalPointer, "GlobalPointerRegister");
        }

        private void InvokeIncomingRegister(
            IntPtr context,
            uint register,
            IntPtr function,
            IntPtr result
        )
        {
            try
            {
                this.WriteIncomingValue(
                    this.GetIncomingRegisterValue,
                    register,
                    function,
                    result
                );
            }
            catch (Exception exception)
            {
                this.WriteUndeterminedValue(result);
                this.LogCustomCallbackException("GetIncomingRegisterValue", exception);
            }
        }

        private void InvokeIncomingFlag(
            IntPtr context,
            uint flag,
            IntPtr function,
            IntPtr result
        )
        {
            try
            {
                this.WriteIncomingValue(
                    this.GetIncomingFlagValue,
                    flag,
                    function,
                    result
                );
            }
            catch (Exception exception)
            {
                this.WriteUndeterminedValue(result);
                this.LogCustomCallbackException("GetIncomingFlagValue", exception);
            }
        }

        private void InvokeIncomingVariable(
            IntPtr context,
            IntPtr variable,
            IntPtr function,
            IntPtr result
        )
        {
            this.InvokeMappedVariable(
                this.GetIncomingVariableForParameterVariable,
                variable,
                function,
                result,
                "GetIncomingVariableForParameterVariable"
            );
        }

        private void InvokeParameterVariable(
            IntPtr context,
            IntPtr variable,
            IntPtr function,
            IntPtr result
        )
        {
            this.InvokeMappedVariable(
                this.GetParameterVariableForIncomingVariable,
                variable,
                function,
                result,
                "GetParameterVariableForIncomingVariable"
            );
        }

        private IntPtr InvokeRegisterList(
            Func<uint[]> callback,
            IntPtr count,
            string name
        )
        {
            try
            {
                return this.AllocateRegisterList(callback(), count);
            }
            catch (Exception exception)
            {
                Marshal.WriteIntPtr(count, IntPtr.Zero);
                this.LogCustomCallbackException(name, exception);

                return IntPtr.Zero;
            }
        }

        private uint InvokeRegister(Func<uint> callback, string name)
        {
            try
            {
                return callback();
            }
            catch (Exception exception)
            {
                this.LogCustomCallbackException(name, exception);

                return uint.MaxValue;
            }
        }

        private void InvokeMappedVariable(
            Func<BNVariable, Function?, BNVariable> callback,
            IntPtr variable,
            IntPtr function,
            IntPtr result,
            string name
        )
        {
            BNVariable input = Marshal.PtrToStructure<BNVariable>(variable);
            try
            {
                this.WriteMappedVariable(callback, variable, function, result);
            }
            catch (Exception exception)
            {
                Marshal.StructureToPtr(input, result, false);
                this.LogCustomCallbackException(name, exception);
            }
        }

        private void WriteUndeterminedValue(IntPtr result)
        {
            RegisterValue value = new RegisterValue();
            Marshal.StructureToPtr(value.ToNative(), result, false);
        }

        private uint ReadIntegerReturn()
        {
            return this.IntegerReturnValueRegister;
        }

        private uint ReadHighIntegerReturn()
        {
            return this.HighIntegerReturnValueRegister;
        }

        private uint ReadFloatReturn()
        {
            return this.FloatReturnValueRegister;
        }

        private uint ReadGlobalPointer()
        {
            return this.GlobalPointerRegister;
        }

        private void LogCustomCallbackException(string name, Exception exception)
        {
            Core.LogError(
                "Unhandled exception in CallingConvention.{0}: {1}",
                name,
                exception
            );
        }
    }
}
