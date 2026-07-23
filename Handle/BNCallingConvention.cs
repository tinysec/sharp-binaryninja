using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public partial class CallingConvention : AbstractSafeHandle<CallingConvention>
	{
		private bool custom;

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
            if (this.custom)
            {
                NativeMethods.BNFreeCallingConvention(this.handle);

                return true;
            }

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
                IntPtr raw = NativeMethods.BNGetCallingConventionName(this.handle);

                return UnsafeUtils.TakeUtf8String(raw);
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
        public virtual uint IntegerReturnValueRegister
        {
            get
            {
                return this.custom
                    ? uint.MaxValue
                    : NativeMethods.BNGetIntegerReturnValueRegister(this.handle);
            }
        }

        /// <summary>
        /// Gets the register used to return floating-point values from functions using this calling convention.
        /// </summary>
        public virtual uint FloatReturnValueRegister
        {
            get
            {
                return this.custom
                    ? uint.MaxValue
                    : NativeMethods.BNGetFloatReturnValueRegister(this.handle);
            }
        }

        /// <summary>
        /// Gets the register used for the high half of a 64-bit integer return value
        /// on architectures that split return values across two registers.
        /// </summary>
        public virtual uint HighIntegerReturnValueRegister
        {
            get
            {
                return this.custom
                    ? uint.MaxValue
                    : NativeMethods.BNGetHighIntegerReturnValueRegister(this.handle);
            }
        }

        /// <summary>
        /// Gets the registers used as global pointers (e.g., GP on MIPS) for this calling convention.
        /// </summary>
        public uint[] GlobalPointerRegisters
        {
            get
            {
                uint register = this.GlobalPointerRegister;
                if (uint.MaxValue == register)
                {
                    return Array.Empty<uint>();
                }

                return new uint[] { register };
            }
        }

        /// <summary>Gets the global pointer register used by this convention.</summary>
        public virtual uint GlobalPointerRegister
        {
            get
            {
                return this.custom
                    ? uint.MaxValue
                    : NativeMethods.BNGetGlobalPointerRegister(this.handle);
            }
        }

        /// <summary>
        /// Gets whether integer and float argument registers share a single index counter,
        /// meaning a float argument consumes an integer register slot and vice versa.
        /// </summary>
        public virtual bool AreArgumentRegistersSharedIndex
        {
            get
            {
                return !this.custom &&
                    NativeMethods.BNAreArgumentRegistersSharedIndex(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the argument registers defined by this calling convention
        /// are also used for variadic (varargs) arguments.
        /// </summary>
        public virtual bool AreArgumentRegistersUsedForVarArgs
        {
            get
            {
                return this.custom ||
                    NativeMethods.BNAreArgumentRegistersUsedForVarArgs(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the stack pointer is adjusted by the callee on return
        /// (e.g., stdcall pops arguments off the stack before returning).
        /// </summary>
        public virtual bool IsStackAdjustedOnReturn
        {
            get
            {
                return !this.custom &&
                    NativeMethods.BNIsStackAdjustedOnReturn(this.handle);
            }
        }

        /// <summary>
        /// Gets whether stack space is reserved for argument registers even when
        /// arguments are passed in registers (e.g., the Windows x64 shadow space).
        /// </summary>
        public virtual bool IsStackReservedForArgumentRegisters
        {
            get
            {
                return !this.custom &&
                    NativeMethods.BNIsStackReservedForArgumentRegisters(this.handle);
            }
        }

        /// <summary>
        /// Returns the set of registers that a callee must preserve (save and restore)
        /// according to this calling convention.
        /// </summary>
        public virtual unsafe uint[] GetCalleeSavedRegisters()
        {
            if (this.custom)
            {
                return Array.Empty<uint>();
            }

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
        public virtual unsafe uint[] GetCallerSavedRegisters()
        {
            if (this.custom)
            {
                return Array.Empty<uint>();
            }

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
        /// Returns the registers that must be used as arguments for the heuristic calling
        /// convention detection to pick this convention. Mirrors Python required_arg_regs.
        /// </summary>
        public virtual unsafe uint[] GetRequiredArgumentRegisters()
        {
            if (this.custom)
            {
                return Array.Empty<uint>();
            }

            // 1. Call the native API to retrieve the required-argument register list.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetRequiredArgumentRegisters(this.handle, (IntPtr)(&count));

            // 2. Return empty if no registers or null pointer.
            if (0 == count || IntPtr.Zero == ptr)
            {
                return Array.Empty<uint>();
            }

            // 3. Marshal the native array into a managed uint[] and free the native buffer.
            return UnsafeUtils.TakeNumberArray<uint>(ptr, count, NativeMethods.BNFreeRegisterList);
        }

        /// <summary>
        /// Returns the registers that must be clobbered for the heuristic calling convention
        /// detection to pick this convention. Mirrors Python required_clobbered_regs.
        /// </summary>
        public virtual unsafe uint[] GetRequiredClobberedRegisters()
        {
            if (this.custom)
            {
                return Array.Empty<uint>();
            }

            // 1. Call the native API to retrieve the required-clobbered register list.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetRequiredClobberedRegisters(this.handle, (IntPtr)(&count));

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
        public virtual unsafe uint[] GetIntegerArgumentRegisters()
        {
            if (this.custom)
            {
                return Array.Empty<uint>();
            }

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
        public virtual unsafe uint[] GetFloatArgumentRegisters()
        {
            if (this.custom)
            {
                return Array.Empty<uint>();
            }

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
        public virtual unsafe uint[] GetImplicitlyDefinedRegisters()
        {
            if (this.custom)
            {
                return Array.Empty<uint>();
            }

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

    }
}
