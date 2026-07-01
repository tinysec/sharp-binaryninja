using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class CallingConvention : AbstractSafeHandle<CallingConvention>
	{
	    internal CallingConvention(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {
	       
	    }

	    internal static BinaryNinja.CallingConvention? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryNinja.CallingConvention(
			    NativeMethods.BNNewCallingConventionReference(handle) ,
			    true
		    );
	    }
	   
	    internal static BinaryNinja.CallingConvention MustNewFromHandle(IntPtr handle)
	    {
		    return new BinaryNinja.CallingConvention(
			    NativeMethods.BNNewCallingConventionReference(handle) ,
			    true
		    );
	    }
	    
	    
	    internal static BinaryNinja.CallingConvention? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryNinja.CallingConvention(handle, true);
	    }
	    
	    internal static BinaryNinja.CallingConvention MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryNinja.CallingConvention(handle, true);
	    }
	    
	    internal static BinaryNinja.CallingConvention? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryNinja.CallingConvention(handle, false);
	    }
	    
	    internal static BinaryNinja.CallingConvention MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryNinja.CallingConvention(handle, false);
	    }
	
        /// <summary>
        /// Releases the native BNCallingConvention handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native calling convention handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeCallingConvention(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the registered name of this calling convention (e.g., "cdecl", "stdcall", "fastcall").
        /// The name is used to identify the calling convention within an architecture.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the calling convention name.
                IntPtr raw = NativeMethods.BNGetCallingConventionName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the architecture that this calling convention belongs to.
        /// Architecture handles are borrowed (not reference-counted).
        /// </summary>
        public Architecture? Architecture
        {
            get
            {
                // Retrieve a borrowed reference to the owning architecture; use FromHandle.
                return BinaryNinja.Architecture.FromHandle(
                    NativeMethods.BNGetCallingConventionArchitecture(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the register used to return integer values from functions using this calling convention.
        /// </summary>
        public uint IntegerReturnValueRegister
        {
            get
            {
                return NativeMethods.BNGetIntegerReturnValueRegister(this.handle);
            }
        }

        /// <summary>
        /// Gets the register used to return floating-point values from functions using this calling convention.
        /// </summary>
        public uint FloatReturnValueRegister
        {
            get
            {
                return NativeMethods.BNGetFloatReturnValueRegister(this.handle);
            }
        }

        /// <summary>
        /// Gets the register used for the high half of a 64-bit integer return value
        /// on architectures that split return values across two registers.
        /// </summary>
        public uint HighIntegerReturnValueRegister
        {
            get
            {
                return NativeMethods.BNGetHighIntegerReturnValueRegister(this.handle);
            }
        }

        /// <summary>
        /// Gets the registers used as global pointers (e.g., GP on MIPS) for this calling convention.
        /// </summary>
        public unsafe uint[] GlobalPointerRegisters
        {
            get
            {
                IntPtr ptr = NativeMethods.BNGetGlobalPointerRegisters(this.handle, out ulong count);

                return UnsafeUtils.TakeNumberArray<uint>(ptr, count, NativeMethods.BNFreeRegisterList);
            }
        }

        /// <summary>
        /// Gets whether integer and float argument registers share a single index counter,
        /// meaning a float argument consumes an integer register slot and vice versa.
        /// </summary>
        public bool AreArgumentRegistersSharedIndex
        {
            get
            {
                return NativeMethods.BNAreArgumentRegistersSharedIndex(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the argument registers defined by this calling convention
        /// are also used for variadic (varargs) arguments.
        /// </summary>
        public bool AreArgumentRegistersUsedForVarArgs
        {
            get
            {
                return NativeMethods.BNAreArgumentRegistersUsedForVarArgs(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the stack pointer is adjusted by the callee on return
        /// (e.g., stdcall pops arguments off the stack before returning).
        /// </summary>
        public bool IsStackAdjustedOnReturn
        {
            get
            {
                return NativeMethods.BNIsStackAdjustedOnReturn(this.handle);
            }
        }

        /// <summary>
        /// Gets whether stack space is reserved for argument registers even when
        /// arguments are passed in registers (e.g., the Windows x64 shadow space).
        /// </summary>
        public bool IsStackReservedForArgumentRegisters
        {
            get
            {
                return NativeMethods.BNIsStackReservedForArgumentRegisters(this.handle);
            }
        }

        /// <summary>
        /// Returns the set of registers that a callee must preserve (save and restore)
        /// according to this calling convention.
        /// </summary>
        public unsafe uint[] GetCalleeSavedRegisters()
        {
            // 1. Call the native API to retrieve the callee-saved register list.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetCalleeSavedRegisters(this.handle, (IntPtr)(&count));

            // 2. Return empty if no registers or null pointer.
            if (0 == count || IntPtr.Zero == ptr)
            {
                return Array.Empty<uint>();
            }

            // 3. Marshal the native array into a managed uint[] and free the native buffer.
            return UnsafeUtils.TakeNumberArray<uint>(ptr, count, NativeMethods.BNFreeRegisterList);
        }

        /// <summary>
        /// Returns the set of registers that the caller must assume are clobbered (not preserved)
        /// across a function call using this calling convention.
        /// </summary>
        public unsafe uint[] GetCallerSavedRegisters()
        {
            // 1. Call the native API to retrieve the caller-saved register list.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetCallerSavedRegisters(this.handle, (IntPtr)(&count));

            // 2. Return empty if no registers or null pointer.
            if (0 == count || IntPtr.Zero == ptr)
            {
                return Array.Empty<uint>();
            }

            // 3. Marshal the native array into a managed uint[] and free the native buffer.
            return UnsafeUtils.TakeNumberArray<uint>(ptr, count, NativeMethods.BNFreeRegisterList);
        }

        /// <summary>
        /// Returns the ordered list of registers used to pass integer arguments
        /// to functions using this calling convention.
        /// </summary>
        public unsafe uint[] GetIntegerArgumentRegisters()
        {
            // 1. Call the native API to retrieve the integer argument register list.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetIntegerArgumentRegisters(this.handle, (IntPtr)(&count));

            // 2. Return empty if no registers or null pointer.
            if (0 == count || IntPtr.Zero == ptr)
            {
                return Array.Empty<uint>();
            }

            // 3. Marshal the native array into a managed uint[] and free the native buffer.
            return UnsafeUtils.TakeNumberArray<uint>(ptr, count, NativeMethods.BNFreeRegisterList);
        }

        /// <summary>
        /// Returns the ordered list of registers used to pass floating-point arguments
        /// to functions using this calling convention.
        /// </summary>
        public unsafe uint[] GetFloatArgumentRegisters()
        {
            // 1. Call the native API to retrieve the float argument register list.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetFloatArgumentRegisters(this.handle, (IntPtr)(&count));

            // 2. Return empty if no registers or null pointer.
            if (0 == count || IntPtr.Zero == ptr)
            {
                return Array.Empty<uint>();
            }

            // 3. Marshal the native array into a managed uint[] and free the native buffer.
            return UnsafeUtils.TakeNumberArray<uint>(ptr, count, NativeMethods.BNFreeRegisterList);
        }

        /// <summary>
        /// Returns the set of registers that are implicitly defined (written) by any function call
        /// using this calling convention, beyond the explicit return value registers.
        /// </summary>
        public unsafe uint[] GetImplicitlyDefinedRegisters()
        {
            // 1. Call the native API to retrieve the implicitly defined register list.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetImplicitlyDefinedRegisters(this.handle, (IntPtr)(&count));

            // 2. Return empty if no registers or null pointer.
            if (0 == count || IntPtr.Zero == ptr)
            {
                return Array.Empty<uint>();
            }

            // 3. Marshal the native array into a managed uint[] and free the native buffer.
            return UnsafeUtils.TakeNumberArray<uint>(ptr, count, NativeMethods.BNFreeRegisterList);
        }

        /// <summary>
        /// Maps a parameter variable (how the caller passes an argument) to the corresponding
        /// incoming variable (how the callee receives it) using the default convention rules,
        /// without considering any function-specific overrides.
        /// </summary>
        /// <param name="var">The parameter variable to map.</param>
        /// <returns>The corresponding incoming variable as seen by the callee.</returns>
        public unsafe BNVariable GetDefaultIncomingVariableForParameterVariable(BNVariable var)
        {
            // 1. Pin the variable on the stack and pass its address to the native API.
            BNVariable native = var;

            return NativeMethods.BNGetDefaultIncomingVariableForParameterVariable(
                this.handle, (IntPtr)(&native)
            );
        }

        /// <summary>
        /// Maps an incoming variable (how the callee receives an argument) to the corresponding
        /// parameter variable (how the caller passes it) using the default convention rules,
        /// without considering any function-specific overrides.
        /// </summary>
        /// <param name="var">The incoming variable to map.</param>
        /// <returns>The corresponding parameter variable as seen by the caller.</returns>
        public unsafe BNVariable GetDefaultParameterVariableForIncomingVariable(BNVariable var)
        {
            // 1. Pin the variable on the stack and pass its address to the native API.
            BNVariable native = var;

            return NativeMethods.BNGetDefaultParameterVariableForIncomingVariable(
                this.handle, (IntPtr)(&native)
            );
        }

        /// <summary>
        /// Maps a parameter variable to the corresponding incoming variable, taking into
        /// account any function-specific calling convention overrides. Falls back to the
        /// default mapping when no function is provided.
        /// </summary>
        /// <param name="var">The parameter variable to map.</param>
        /// <param name="func">Optional function providing context for overrides; null uses defaults.</param>
        /// <returns>The corresponding incoming variable as seen by the callee.</returns>
        public unsafe BNVariable GetIncomingVariableForParameterVariable(BNVariable var, Function? func = null)
        {
            // 1. Pin the variable on the stack.
            BNVariable native = var;

            // 2. Resolve the function handle, using IntPtr.Zero when no function is provided.
            IntPtr funcHandle = null == func ? IntPtr.Zero : func.DangerousGetHandle();

            return NativeMethods.BNGetIncomingVariableForParameterVariable(
                this.handle, (IntPtr)(&native), funcHandle
            );
        }

        /// <summary>
        /// Maps an incoming variable to the corresponding parameter variable, taking into
        /// account any function-specific calling convention overrides. Falls back to the
        /// default mapping when no function is provided.
        /// </summary>
        /// <param name="var">The incoming variable to map.</param>
        /// <param name="func">Optional function providing context for overrides; null uses defaults.</param>
        /// <returns>The corresponding parameter variable as seen by the caller.</returns>
        public unsafe BNVariable GetParameterVariableForIncomingVariable(BNVariable var, Function? func = null)
        {
            // 1. Pin the variable on the stack.
            BNVariable native = var;

            // 2. Resolve the function handle, using IntPtr.Zero when no function is provided.
            IntPtr funcHandle = null == func ? IntPtr.Zero : func.DangerousGetHandle();

            return NativeMethods.BNGetParameterVariableForIncomingVariable(
                this.handle, (IntPtr)(&native), funcHandle
            );
        }

        /// <summary>
        /// Checks whether this calling convention is eligible for heuristic analysis.
        /// </summary>
        /// <returns>True if the calling convention can be used for heuristics.</returns>
        public bool IsEligibleForHeuristics
        {
            get
            {
                return NativeMethods.BNIsEligibleForHeuristics(this.handle);
            }
        }

        /// <summary>
        /// Gets the incoming value of a flag register at function entry according to this calling convention.
        /// </summary>
        /// <param name="reg">The flag register index to query.</param>
        /// <param name="func">The function providing context for the query.</param>
        /// <returns>The register value representing the incoming state of the flag.</returns>
        public RegisterValue GetIncomingFlagValue(uint reg , Function func)
        {
            return RegisterValue.FromNative(
                NativeMethods.BNGetIncomingFlagValue(
                    this.handle ,
                    reg ,
                    func.DangerousGetHandle()
                )
            );
        }

        /// <summary>
        /// Gets the incoming value of a register at function entry according to this calling convention.
        /// </summary>
        /// <param name="reg">The register index to query.</param>
        /// <param name="func">The function providing context for the query.</param>
        /// <returns>The register value representing the incoming state of the register.</returns>
        public RegisterValue GetIncomingRegisterValue(uint reg , Function func)
        {
            return RegisterValue.FromNative(
                NativeMethods.BNGetIncomingRegisterValue(
                    this.handle ,
                    reg ,
                    func.DangerousGetHandle()
                )
            );
        }

        /// <summary>
        /// Gets the parameter ordering for an array of variables and their types.
        /// Returns the ordered variable array according to this calling convention's rules.
        /// </summary>
        /// <param name="paramVars">The parameter variables to order.</param>
        /// <param name="paramTypes">The types corresponding to each parameter variable.</param>
        /// <returns>An array of BNVariable structs in the correct parameter order.</returns>
        public unsafe CoreVariable[] GetParameterOrderingForVariables(
            BinaryView view ,
            CoreVariable[] paramVars ,
            BinaryNinja.Type[] paramTypes
        )
        {
            // 1. Validate inputs.
            if (null == paramVars || null == paramTypes || paramVars.Length != paramTypes.Length)
            {
                return Array.Empty<CoreVariable>();
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Marshal the variable array.
                BNVariable[] nativeVars = new BNVariable[paramVars.Length];
                for (int i = 0; i < paramVars.Length; i++)
                {
                    nativeVars[i] = paramVars[i].ToNative();
                }
                IntPtr varsPtr = allocator.AllocStructArray<BNVariable>(nativeVars);

                // 3. Marshal the type handle array (BNType**).
                IntPtr[] typeHandles = new IntPtr[paramTypes.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    typeHandles[i] = paramTypes[i].DangerousGetHandle();
                }
                IntPtr typesPtr = allocator.AllocStructArray<IntPtr>(typeHandles);

                // 4. Stack-allocate the output count.
                ulong count = 0;

                // 5. Call the native API.
                IntPtr resultPtr = NativeMethods.BNGetParameterOrderingForVariables(
                    this.handle ,
                    view.DangerousGetHandle() ,
                    varsPtr ,
                    typesPtr ,
                    (UIntPtr)paramVars.Length ,
                    (IntPtr)(&count)
                );

                // 6. Marshal the result array.
                if (IntPtr.Zero == resultPtr || 0 == count)
                {
                    return Array.Empty<CoreVariable>();
                }

                CoreVariable[] result = new CoreVariable[(int)count];
                BNVariable* rawResult = (BNVariable*)resultPtr;

                for (ulong i = 0; i < count; i++)
                {
                    result[i] = new CoreVariable(rawResult[i]);
                }

                // 7. Free the native result array.
                NativeMethods.BNFreeVariableList(resultPtr);

                return result;
            }
        }

    }
}