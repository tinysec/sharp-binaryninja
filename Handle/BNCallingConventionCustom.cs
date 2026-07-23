using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public partial class CallingConvention
    {
        private static readonly object customConventionLock = new object();

        private static readonly List<CallingConvention> customConventions =
            new List<CallingConvention>();

        private NativeDelegates.BNCallingConventionEvent? freeObjectCallback;

        private NativeDelegates.BNCallingConventionGetRegisterList?
            callerSavedCallback;

        private NativeDelegates.BNCallingConventionGetRegisterList?
            calleeSavedCallback;

        private NativeDelegates.BNCallingConventionGetRegisterList?
            integerArgumentsCallback;

        private NativeDelegates.BNCallingConventionGetRegisterList?
            floatArgumentsCallback;

        private NativeDelegates.BNCallingConventionGetRegisterList?
            requiredArgumentsCallback;

        private NativeDelegates.BNCallingConventionGetRegisterList?
            requiredClobberedCallback;

        private NativeDelegates.BNCallingConventionFreeRegisterList?
            freeRegisterListCallback;

        private NativeDelegates.BNCallingConventionGetBoolean? sharedIndexCallback;

        private NativeDelegates.BNCallingConventionGetBoolean?
            stackReservedCallback;

        private NativeDelegates.BNCallingConventionGetBoolean?
            stackAdjustedCallback;

        private NativeDelegates.BNCallingConventionGetBoolean? heuristicsCallback;

        private NativeDelegates.BNCallingConventionGetRegister?
            integerReturnCallback;

        private NativeDelegates.BNCallingConventionGetRegister?
            highIntegerReturnCallback;

        private NativeDelegates.BNCallingConventionGetRegister? floatReturnCallback;

        private NativeDelegates.BNCallingConventionGetRegister? globalPointerCallback;

        private NativeDelegates.BNCallingConventionGetRegisterList?
            implicitlyDefinedCallback;

        private NativeDelegates.BNCallingConventionGetIncomingValue?
            incomingRegisterCallback;

        private NativeDelegates.BNCallingConventionGetIncomingValue?
            incomingFlagCallback;

        private NativeDelegates.BNCallingConventionMapVariable?
            incomingVariableCallback;

        private NativeDelegates.BNCallingConventionMapVariable?
            parameterVariableCallback;

        private NativeDelegates.BNCallingConventionGetBoolean? varArgsCallback;

        /// <summary>Creates a custom calling convention for an architecture.</summary>
        protected CallingConvention(Architecture architecture, string name)
            : base(false)
        {
            if (null == architecture)
            {
                throw new ArgumentNullException(nameof(architecture));
            }

            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.custom = true;
            this.InitializeCustomCallbacks();
            BNCustomCallingConvention callbacks = this.CreateCustomCallbacks();
            IntPtr convention = NativeMethods.BNCreateCallingConvention(
                architecture.DangerousGetHandle(),
                name,
                in callbacks
            );
            if (IntPtr.Zero == convention)
            {
                throw new InvalidOperationException(
                    "The core rejected the calling convention."
                );
            }

            this.SetHandle(convention);
            lock (CallingConvention.customConventionLock)
            {
                CallingConvention.customConventions.Add(this);
            }
        }

        private void InitializeCustomCallbacks()
        {
            this.freeObjectCallback = new NativeDelegates.BNCallingConventionEvent(
                this.InvokeFreeObject
            );
            this.callerSavedCallback = this.CreateRegisterListCallback(
                this.InvokeCallerSaved
            );
            this.calleeSavedCallback = this.CreateRegisterListCallback(
                this.InvokeCalleeSaved
            );
            this.integerArgumentsCallback = this.CreateRegisterListCallback(
                this.InvokeIntegerArguments
            );
            this.floatArgumentsCallback = this.CreateRegisterListCallback(
                this.InvokeFloatArguments
            );
            this.requiredArgumentsCallback = this.CreateRegisterListCallback(
                this.InvokeRequiredArguments
            );
            this.requiredClobberedCallback = this.CreateRegisterListCallback(
                this.InvokeRequiredClobbered
            );
            this.freeRegisterListCallback =
                new NativeDelegates.BNCallingConventionFreeRegisterList(
                    this.InvokeFreeRegisterList
                );
            this.sharedIndexCallback = this.CreateBooleanCallback(
                this.InvokeSharedIndex
            );
            this.stackReservedCallback = this.CreateBooleanCallback(
                this.InvokeStackReserved
            );
            this.stackAdjustedCallback = this.CreateBooleanCallback(
                this.InvokeStackAdjusted
            );
            this.heuristicsCallback = this.CreateBooleanCallback(
                this.InvokeHeuristics
            );
            this.integerReturnCallback = this.CreateRegisterCallback(
                this.InvokeIntegerReturn
            );
            this.highIntegerReturnCallback = this.CreateRegisterCallback(
                this.InvokeHighIntegerReturn
            );
            this.floatReturnCallback = this.CreateRegisterCallback(
                this.InvokeFloatReturn
            );
            this.globalPointerCallback = this.CreateRegisterCallback(
                this.InvokeGlobalPointer
            );
            this.implicitlyDefinedCallback = this.CreateRegisterListCallback(
                this.InvokeImplicitlyDefined
            );
            this.incomingRegisterCallback =
                new NativeDelegates.BNCallingConventionGetIncomingValue(
                    this.InvokeIncomingRegister
                );
            this.incomingFlagCallback =
                new NativeDelegates.BNCallingConventionGetIncomingValue(
                    this.InvokeIncomingFlag
                );
            this.incomingVariableCallback =
                new NativeDelegates.BNCallingConventionMapVariable(
                    this.InvokeIncomingVariable
                );
            this.parameterVariableCallback =
                new NativeDelegates.BNCallingConventionMapVariable(
                    this.InvokeParameterVariable
                );
            this.varArgsCallback = this.CreateBooleanCallback(this.InvokeVarArgs);
        }

        private BNCustomCallingConvention CreateCustomCallbacks()
        {
            BNCustomCallingConvention callbacks = new BNCustomCallingConvention();
            callbacks.context = IntPtr.Zero;
            callbacks.freeObject = this.GetCallbackPointer(this.freeObjectCallback!);
            callbacks.getCallerSavedRegisters = this.GetCallbackPointer(
                this.callerSavedCallback!
            );
            callbacks.getCalleeSavedRegisters = this.GetCallbackPointer(
                this.calleeSavedCallback!
            );
            callbacks.getIntegerArgumentRegisters = this.GetCallbackPointer(
                this.integerArgumentsCallback!
            );
            callbacks.getFloatArgumentRegisters = this.GetCallbackPointer(
                this.floatArgumentsCallback!
            );
            callbacks.getRequiredArgumentRegisters = this.GetCallbackPointer(
                this.requiredArgumentsCallback!
            );
            callbacks.getRequiredClobberedRegisters = this.GetCallbackPointer(
                this.requiredClobberedCallback!
            );
            callbacks.freeRegisterList = this.GetCallbackPointer(
                this.freeRegisterListCallback!
            );
            callbacks.areArgumentRegistersSharedIndex = this.GetCallbackPointer(
                this.sharedIndexCallback!
            );
            callbacks.isStackReservedForArgumentRegisters = this.GetCallbackPointer(
                this.stackReservedCallback!
            );
            callbacks.isStackAdjustedOnReturn = this.GetCallbackPointer(
                this.stackAdjustedCallback!
            );
            callbacks.isEligibleForHeuristics = this.GetCallbackPointer(
                this.heuristicsCallback!
            );
            callbacks.getIntegerReturnValueRegister = this.GetCallbackPointer(
                this.integerReturnCallback!
            );
            callbacks.getHighIntegerReturnValueRegister = this.GetCallbackPointer(
                this.highIntegerReturnCallback!
            );
            callbacks.getFloatReturnValueRegister = this.GetCallbackPointer(
                this.floatReturnCallback!
            );
            callbacks.getGlobalPointerRegister = this.GetCallbackPointer(
                this.globalPointerCallback!
            );
            callbacks.getImplicitlyDefinedRegisters = this.GetCallbackPointer(
                this.implicitlyDefinedCallback!
            );
            callbacks.getIncomingRegisterValue = this.GetCallbackPointer(
                this.incomingRegisterCallback!
            );
            callbacks.getIncomingFlagValue = this.GetCallbackPointer(
                this.incomingFlagCallback!
            );
            callbacks.getIncomingVariableForParameterVariable =
                this.GetCallbackPointer(this.incomingVariableCallback!);
            callbacks.getParameterVariableForIncomingVariable =
                this.GetCallbackPointer(this.parameterVariableCallback!);
            callbacks.areArgumentRegistersUsedForVarArgs = this.GetCallbackPointer(
                this.varArgsCallback!
            );

            return callbacks;
        }

        private unsafe IntPtr AllocateRegisterList(uint[]? registers, IntPtr count)
        {
            uint[] safeRegisters = registers ?? Array.Empty<uint>();
            Marshal.WriteIntPtr(count, new IntPtr(safeRegisters.Length));
            if (0 == safeRegisters.Length)
            {
                return IntPtr.Zero;
            }

            IntPtr block = Marshal.AllocHGlobal(
                checked(safeRegisters.Length * sizeof(uint))
            );
            uint* output = (uint*)block;
            for (int i = 0; i < safeRegisters.Length; i++)
            {
                output[i] = safeRegisters[i];
            }

            return block;
        }

        private void InvokeFreeRegisterList(
            IntPtr context,
            IntPtr registers,
            UIntPtr count
        )
        {
            if (IntPtr.Zero != registers)
            {
                Marshal.FreeHGlobal(registers);
            }
        }

        private void InvokeFreeObject(IntPtr context)
        {
            lock (CallingConvention.customConventionLock)
            {
                CallingConvention.customConventions.Remove(this);
            }

            this.SetHandleAsInvalid();
        }

        private void WriteIncomingValue(
            Func<uint, Function?, RegisterValue> callback,
            uint register,
            IntPtr function,
            IntPtr result
        )
        {
            using Function? managedFunction = Function.NewFromHandle(function);
            RegisterValue value = callback(register, managedFunction);
            Marshal.StructureToPtr(value.ToNative(), result, false);
        }

        private void WriteMappedVariable(
            Func<BNVariable, Function?, BNVariable> callback,
            IntPtr variable,
            IntPtr function,
            IntPtr result
        )
        {
            using Function? managedFunction = Function.NewFromHandle(function);
            BNVariable input = Marshal.PtrToStructure<BNVariable>(variable);
            BNVariable output = callback(input, managedFunction);
            Marshal.StructureToPtr(output, result, false);
        }

        private NativeDelegates.BNCallingConventionGetRegisterList
            CreateRegisterListCallback(
                NativeDelegates.BNCallingConventionGetRegisterList callback
            )
        {
            return callback;
        }

        private NativeDelegates.BNCallingConventionGetBoolean
            CreateBooleanCallback(
                NativeDelegates.BNCallingConventionGetBoolean callback
            )
        {
            return callback;
        }

        private NativeDelegates.BNCallingConventionGetRegister
            CreateRegisterCallback(
                NativeDelegates.BNCallingConventionGetRegister callback
            )
        {
            return callback;
        }

        private IntPtr GetCallbackPointer<TDelegate>(TDelegate callback)
            where TDelegate : Delegate
        {
            return Marshal.GetFunctionPointerForDelegate<TDelegate>(callback);
        }
    }
}
