using System;

namespace BinaryNinja
{
    public partial class CallingConvention
    {
        public unsafe BNVariable GetDefaultIncomingVariableForParameterVariable(
            BNVariable variable
        )
        {
            BNVariable native = variable;

            return NativeMethods.BNGetDefaultIncomingVariableForParameterVariable(
                this.handle,
                (IntPtr)(&native)
            );
        }

        public unsafe BNVariable GetDefaultParameterVariableForIncomingVariable(
            BNVariable variable
        )
        {
            BNVariable native = variable;

            return NativeMethods.BNGetDefaultParameterVariableForIncomingVariable(
                this.handle,
                (IntPtr)(&native)
            );
        }

        public virtual unsafe BNVariable GetIncomingVariableForParameterVariable(
            BNVariable variable,
            Function? function = null
        )
        {
            if (this.custom)
            {
                return this.GetDefaultIncomingVariableForParameterVariable(variable);
            }

            BNVariable native = variable;
            IntPtr functionHandle = null == function
                ? IntPtr.Zero
                : function.DangerousGetHandle();

            return NativeMethods.BNGetIncomingVariableForParameterVariable(
                this.handle,
                (IntPtr)(&native),
                functionHandle
            );
        }

        public virtual unsafe BNVariable GetParameterVariableForIncomingVariable(
            BNVariable variable,
            Function? function = null
        )
        {
            if (this.custom)
            {
                return this.GetDefaultParameterVariableForIncomingVariable(variable);
            }

            BNVariable native = variable;
            IntPtr functionHandle = null == function
                ? IntPtr.Zero
                : function.DangerousGetHandle();

            return NativeMethods.BNGetParameterVariableForIncomingVariable(
                this.handle,
                (IntPtr)(&native),
                functionHandle
            );
        }

        public virtual bool IsEligibleForHeuristics
        {
            get
            {
                return this.custom ||
                    NativeMethods.BNIsEligibleForHeuristics(this.handle);
            }
        }

        public virtual RegisterValue GetIncomingFlagValue(
            uint register,
            Function? function
        )
        {
            if (this.custom)
            {
                return new RegisterValue();
            }

            IntPtr functionHandle = null == function
                ? IntPtr.Zero
                : function.DangerousGetHandle();

            return RegisterValue.FromNative(
                NativeMethods.BNGetIncomingFlagValue(
                    this.handle,
                    register,
                    functionHandle
                )
            );
        }

        public virtual RegisterValue GetIncomingRegisterValue(
            uint register,
            Function? function
        )
        {
            if (this.custom)
            {
                return this.GetDefaultIncomingRegisterValue(register);
            }

            IntPtr functionHandle = null == function
                ? IntPtr.Zero
                : function.DangerousGetHandle();

            return RegisterValue.FromNative(
                NativeMethods.BNGetIncomingRegisterValue(
                    this.handle,
                    register,
                    functionHandle
                )
            );
        }

        public unsafe CoreVariable[] GetParameterOrderingForVariables(
            BinaryView view,
            CoreVariable[] parameterVariables,
            BinaryNinja.Type[] parameterTypes
        )
        {
            if (
                null == parameterVariables ||
                null == parameterTypes ||
                parameterVariables.Length != parameterTypes.Length
            )
            {
                return Array.Empty<CoreVariable>();
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                BNVariable[] nativeVariables = new BNVariable[
                    parameterVariables.Length
                ];
                IntPtr[] typeHandles = new IntPtr[parameterTypes.Length];
                for (int i = 0; i < parameterVariables.Length; i++)
                {
                    nativeVariables[i] = parameterVariables[i].ToNative();
                    typeHandles[i] = parameterTypes[i].DangerousGetHandle();
                }

                IntPtr variables = allocator.AllocStructArray<BNVariable>(
                    nativeVariables
                );
                IntPtr types = allocator.AllocStructArray<IntPtr>(typeHandles);
                ulong count = 0;
                IntPtr resultPointer = NativeMethods.BNGetParameterOrderingForVariables(
                    this.handle,
                    view.DangerousGetHandle(),
                    variables,
                    types,
                    (UIntPtr)parameterVariables.Length,
                    (IntPtr)(&count)
                );
                if (IntPtr.Zero == resultPointer || 0 == count)
                {
                    return Array.Empty<CoreVariable>();
                }

                CoreVariable[] result = new CoreVariable[checked((int)count)];
                BNVariable* nativeResult = (BNVariable*)resultPointer;
                for (ulong i = 0; i < count; i++)
                {
                    result[checked((int)i)] = new CoreVariable(nativeResult[i]);
                }

                NativeMethods.BNFreeVariableList(resultPointer);

                return result;
            }
        }

        private RegisterValue GetDefaultIncomingRegisterValue(uint register)
        {
            Architecture? architecture = this.Architecture;
            if (null != architecture)
            {
                uint registerStack = architecture.GetRegisterStackForRegister(register);
                if (uint.MaxValue != registerStack)
                {
                    RegisterStackInfo info = architecture.GetRegisterStackInfo(
                        registerStack
                    );
                    if (register == info.StackTopReg)
                    {
                        RegisterValue stackTop = new RegisterValue();
                        stackTop.State = RegisterValueType.ConstantValue;

                        return stackTop;
                    }
                }
            }

            return new RegisterValue();
        }
    }
}
