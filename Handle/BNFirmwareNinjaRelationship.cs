using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class FirmwareNinjaRelationship : AbstractSafeHandle<FirmwareNinjaRelationship>
	{
	    internal FirmwareNinjaRelationship(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {

	    }

        /// <summary>
        /// Creates a new FirmwareNinjaRelationship for the given binary view.
        /// The caller is responsible for disposing the returned instance.
        /// </summary>
        /// <param name="view">The binary view to create the relationship for.</param>
        /// <returns>A new owned FirmwareNinjaRelationship instance, or null on failure.</returns>
        public static FirmwareNinjaRelationship? Create(BinaryView view)
        {
            // 1. Validate the required binary view parameter.
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            // 2. Create a new relationship for the given view; the returned handle is owned.
            return FirmwareNinjaRelationship.TakeHandle(
                NativeMethods.BNCreateFirmwareNinjaRelationship(view.DangerousGetHandle())
            );
        }
	    
	    internal static FirmwareNinjaRelationship? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FirmwareNinjaRelationship(
			    NativeMethods.BNNewFirmwareNinjaRelationshipReference(handle) ,
			    true
		    );
	    }
	    
	    internal static FirmwareNinjaRelationship MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FirmwareNinjaRelationship(
			    NativeMethods.BNNewFirmwareNinjaRelationshipReference(handle) ,
			    true
		    );
	    }
	    
	    internal static FirmwareNinjaRelationship? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FirmwareNinjaRelationship(handle, true);
	    }
	    
	    internal static FirmwareNinjaRelationship MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FirmwareNinjaRelationship(handle, true);
	    }
	    
	    internal static FirmwareNinjaRelationship? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FirmwareNinjaRelationship(handle, false);
	    }
	    
	    internal static FirmwareNinjaRelationship MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FirmwareNinjaRelationship(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNFirmwareNinjaRelationship handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native relationship handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeFirmwareNinjaRelationship(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the unique GUID string that identifies this relationship.
        /// </summary>
        public string Guid
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the relationship GUID.
                IntPtr raw = NativeMethods.BNFirmwareNinjaRelationshipGetGuid(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the human-readable description of this relationship.
        /// </summary>
        public string Description
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the relationship description.
                IntPtr raw = NativeMethods.BNFirmwareNinjaRelationshipGetDescription(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }

            set
            {
                // Forward the description string to the native setter.
                NativeMethods.BNFirmwareNinjaRelationshipSetDescription(
                    this.handle,
                    value ?? string.Empty
                );
            }
        }

        /// <summary>
        /// Gets or sets the provenance string indicating the source or origin of this relationship.
        /// </summary>
        public string Provenance
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the relationship provenance.
                IntPtr raw = NativeMethods.BNFirmwareNinjaRelationshipGetProvenance(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }

            set
            {
                // Forward the provenance string to the native setter.
                NativeMethods.BNFirmwareNinjaRelationshipSetProvenance(
                    this.handle,
                    value ?? string.Empty
                );
            }
        }

        /// <summary>
        /// Gets whether the primary side of this relationship is a function.
        /// </summary>
        public bool PrimaryIsFunction
        {
            get
            {
                return NativeMethods.BNFirmwareNinjaRelationshipPrimaryIsFunction(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the primary side of this relationship is a raw address.
        /// </summary>
        public bool PrimaryIsAddress
        {
            get
            {
                return NativeMethods.BNFirmwareNinjaRelationshipPrimaryIsAddress(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the primary side of this relationship is a data variable.
        /// </summary>
        public bool PrimaryIsDataVariable
        {
            get
            {
                return NativeMethods.BNFirmwareNinjaRelationshipPrimaryIsDataVariable(this.handle);
            }
        }

        /// <summary>
        /// Gets the primary function of this relationship.
        /// Check PrimaryIsFunction before accessing this property.
        /// </summary>
        public Function? PrimaryFunction
        {
            get
            {
                // Retrieve a new owned reference to the primary function.
                return BinaryNinja.Function.TakeHandle(
                    NativeMethods.BNFirmwareNinjaRelationshipGetPrimaryFunction(this.handle)
                );
            }

            set
            {
                // Set the primary function; null clears the association.
                NativeMethods.BNFirmwareNinjaRelationshipSetPrimaryFunction(
                    this.handle,
                    (value != null) ? value.DangerousGetHandle() : IntPtr.Zero
                );
            }
        }

        /// <summary>
        /// Gets the primary address of this relationship.
        /// Returns null if the primary side is not an address.
        /// Check PrimaryIsAddress before calling.
        /// </summary>
        public unsafe ulong? PrimaryAddress
        {
            get
            {
                // 1. Stack-allocate the address output slot.
                ulong addr = 0;

                // 2. Call the native getter; returns true if the primary is an address.
                bool ok = NativeMethods.BNFirmwareNinjaRelationshipGetPrimaryAddress(
                    this.handle,
                    (IntPtr)(&addr)
                );

                // 3. Return the address on success, null otherwise.
                if (!ok)
                {
                    return null;
                }

                return addr;
            }
        }

        /// <summary>
        /// Gets the primary data variable of this relationship.
        /// Returns null if the primary side is not a data variable.
        /// Check PrimaryIsDataVariable before calling.
        /// </summary>
        public unsafe DataVariable? PrimaryDataVariable
        {
            get
            {
                // 1. Stack-allocate a BNDataVariable struct for the native output.
                BNDataVariable nativeVar = new BNDataVariable();

                // 2. Call the native getter; returns true when successful.
                bool ok = NativeMethods.BNFirmwareNinjaRelationshipGetPrimaryDataVariable(
                    this.handle,
                    (IntPtr)(&nativeVar)
                );

                if (!ok)
                {
                    return null;
                }

                // 3. Convert the native struct to the managed DataVariable object.
                return DataVariable.FromNative(nativeVar);
            }
        }

        /// <summary>
        /// Gets whether the secondary side of this relationship is a function.
        /// </summary>
        public bool SecondaryIsFunction
        {
            get
            {
                return NativeMethods.BNFirmwareNinjaRelationshipSecondaryIsFunction(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the secondary side of this relationship is a raw address.
        /// </summary>
        public bool SecondaryIsAddress
        {
            get
            {
                return NativeMethods.BNFirmwareNinjaRelationshipSecondaryIsAddress(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the secondary side of this relationship is a data variable.
        /// </summary>
        public bool SecondaryIsDataVariable
        {
            get
            {
                return NativeMethods.BNFirmwareNinjaRelationshipSecondaryIsDataVariable(this.handle);
            }
        }

        /// <summary>
        /// Gets the secondary function of this relationship.
        /// Check SecondaryIsFunction before accessing this property.
        /// </summary>
        public Function? SecondaryFunction
        {
            get
            {
                // Retrieve a new owned reference to the secondary function.
                return BinaryNinja.Function.TakeHandle(
                    NativeMethods.BNFirmwareNinjaRelationshipGetSecondaryFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the secondary address of this relationship.
        /// Returns null if the secondary side is not an address.
        /// Check SecondaryIsAddress before calling.
        /// </summary>
        public unsafe ulong? SecondaryAddress
        {
            get
            {
                // 1. Stack-allocate the address output slot.
                ulong addr = 0;

                // 2. Call the native getter; returns true if the secondary is an address.
                bool ok = NativeMethods.BNFirmwareNinjaRelationshipGetSecondaryAddress(
                    this.handle,
                    (IntPtr)(&addr)
                );

                // 3. Return the address on success, null otherwise.
                if (!ok)
                {
                    return null;
                }

                return addr;
            }
        }

        /// <summary>
        /// Gets the secondary data variable of this relationship.
        /// Returns null if the secondary side is not a data variable.
        /// Check SecondaryIsDataVariable before calling.
        /// </summary>
        public unsafe DataVariable? SecondaryDataVariable
        {
            get
            {
                // 1. Stack-allocate a BNDataVariable struct for the native output.
                BNDataVariable nativeVar = new BNDataVariable();

                // 2. Call the native getter; returns true when successful.
                bool ok = NativeMethods.BNFirmwareNinjaRelationshipGetSecondaryDataVariable(
                    this.handle,
                    (IntPtr)(&nativeVar)
                );

                if (!ok)
                {
                    return null;
                }

                // 3. Convert the native struct to the managed DataVariable object.
                return DataVariable.FromNative(nativeVar);
            }
        }

        /// <summary>
        /// Sets the primary address of this relationship to the given raw address.
        /// </summary>
        /// <param name="address">The raw address to assign to the primary side.</param>
        public void SetPrimaryAddress(ulong address)
        {
            // Forward the address to the native setter on this relationship handle.
            NativeMethods.BNFirmwareNinjaRelationshipSetPrimaryAddress(
                this.handle,
                address
            );
        }

        /// <summary>
        /// Sets the primary data variable of this relationship by its address.
        /// </summary>
        /// <param name="dataVariableAddress">The address of the data variable to assign to the primary side.</param>
        public void SetPrimaryDataVariable(ulong dataVariableAddress)
        {
            // Forward the data variable address to the native setter on this relationship handle.
            NativeMethods.BNFirmwareNinjaRelationshipSetPrimaryDataVariable(
                this.handle,
                dataVariableAddress
            );
        }

        /// <summary>
        /// Sets the secondary address of this relationship to the given raw address.
        /// </summary>
        /// <param name="address">The raw address to assign to the secondary side.</param>
        public void SetSecondaryAddress(ulong address)
        {
            // Forward the address to the native setter on this relationship handle.
            NativeMethods.BNFirmwareNinjaRelationshipSetSecondaryAddress(
                this.handle,
                address
            );
        }

        /// <summary>
        /// Sets the secondary data variable of this relationship by its address.
        /// </summary>
        /// <param name="dataVariableAddress">The address of the data variable to assign to the secondary side.</param>
        public void SetSecondaryDataVariable(ulong dataVariableAddress)
        {
            // Forward the data variable address to the native setter on this relationship handle.
            NativeMethods.BNFirmwareNinjaRelationshipSetSecondaryDataVariable(
                this.handle,
                dataVariableAddress
            );
        }

        /// <summary>
        /// Sets the secondary function of this relationship.
        /// </summary>
        /// <param name="function">The function to assign to the secondary side.</param>
        public void SetSecondaryFunction(Function function)
        {
            // Forward the function handle to the native setter on this relationship handle.
            NativeMethods.BNFirmwareNinjaRelationshipSetSecondaryFunction(
                this.handle,
                function.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Sets the secondary side of this relationship to an external address in another project file.
        /// </summary>
        /// <param name="projectFile">The project file that contains the external address.</param>
        /// <param name="address">The external address within the project file.</param>
        public void SetSecondaryExternalAddress(ProjectFile projectFile, ulong address)
        {
            // Forward the project file handle and address to the native setter.
            NativeMethods.BNFirmwareNinjaRelationshipSetSecondaryExternalAddress(
                this.handle,
                projectFile.DangerousGetHandle(),
                address
            );
        }

        /// <summary>
        /// Sets the secondary side of this relationship to an external symbol in another project file.
        /// </summary>
        /// <param name="projectFile">The project file that contains the external symbol.</param>
        /// <param name="symbol">The symbol name within the project file.</param>
        public void SetSecondaryExternalSymbol(ProjectFile projectFile, string symbol)
        {
            // Forward the project file handle and symbol string to the native setter.
            NativeMethods.BNFirmwareNinjaRelationshipSetSecondaryExternalSymbol(
                this.handle,
                projectFile.DangerousGetHandle(),
                symbol ?? string.Empty
            );
        }

        /// <summary>
        /// Gets the project file associated with the secondary external reference.
        /// Returns null if the secondary side is not an external reference.
        /// </summary>
        public ProjectFile? SecondaryExternalProjectFile
        {
            get
            {
                // Retrieve the native project file pointer and take ownership.
                IntPtr raw = NativeMethods.BNFirmwareNinjaRelationshipGetSecondaryExternalProjectFile(
                    this.handle
                );

                // Wrap the returned pointer; null on not-found (zero pointer).
                return ProjectFile.TakeHandle(raw);
            }
        }

        /// <summary>
        /// Gets the external symbol name on the secondary side of this relationship.
        /// Returns an empty string if the secondary side is not an external symbol.
        /// </summary>
        public string SecondaryExternalSymbol
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the external symbol.
                IntPtr raw = NativeMethods.BNFirmwareNinjaRelationshipGetSecondaryExternalSymbol(
                    this.handle
                );

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets whether the secondary side of this relationship is an external address
        /// referencing another project file.
        /// </summary>
        public bool SecondaryIsExternalAddress
        {
            get
            {
                return NativeMethods.BNFirmwareNinjaRelationshipSecondaryIsExternalAddress(this.handle);
            }
        }

        /// <summary>
        /// Gets whether the secondary side of this relationship is an external symbol
        /// referencing another project file.
        /// </summary>
        public bool SecondaryIsExternalSymbol
        {
            get
            {
                return NativeMethods.BNFirmwareNinjaRelationshipSecondaryIsExternalSymbol(this.handle);
            }
        }
    }
}