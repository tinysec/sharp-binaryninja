using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class FirmwareNinjaReferenceNode : AbstractSafeHandle<FirmwareNinjaReferenceNode>
	{
	    internal FirmwareNinjaReferenceNode(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }
	    
	    internal static FirmwareNinjaReferenceNode? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FirmwareNinjaReferenceNode(
			    NativeMethods.BNNewFirmwareNinjaReferenceNodeReference(handle) ,
			    true
		    );
	    }
	    
	    internal static FirmwareNinjaReferenceNode MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FirmwareNinjaReferenceNode(
			    NativeMethods.BNNewFirmwareNinjaReferenceNodeReference(handle) ,
			    true
		    );
	    }
	    
	    internal static FirmwareNinjaReferenceNode? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FirmwareNinjaReferenceNode(handle, true);
	    }
	    
	    internal static FirmwareNinjaReferenceNode MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FirmwareNinjaReferenceNode(handle, true);
	    }
	    
	    internal static FirmwareNinjaReferenceNode? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FirmwareNinjaReferenceNode(handle, false);
	    }
	    
	    internal static FirmwareNinjaReferenceNode MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FirmwareNinjaReferenceNode(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNFirmwareNinjaReferenceNode handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native reference node handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeFirmwareNinjaReferenceNode(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets whether this reference node has any child nodes in the reference tree.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                // Query the native layer for the has-children flag.
                return NativeMethods.BNFirmwareNinjaReferenceNodeHasChildren(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this reference node represents a function.
        /// When true, use the Function property to retrieve the function handle.
        /// </summary>
        public bool IsFunction
        {
            get
            {
                // Query the native layer for the is-function flag.
                return NativeMethods.BNFirmwareNinjaReferenceNodeIsFunction(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this reference node represents a data variable.
        /// When true, use the GetDataVariable() method to retrieve the variable.
        /// </summary>
        public bool IsDataVariable
        {
            get
            {
                // Query the native layer for the is-data-variable flag.
                return NativeMethods.BNFirmwareNinjaReferenceNodeIsDataVariable(this.handle);
            }
        }

        /// <summary>
        /// Gets the function associated with this reference node.
        /// Returns null if this node does not represent a function.
        /// Check IsFunction before calling this property.
        /// </summary>
        public Function? Function
        {
            get
            {
                // Retrieve a new owned reference to the function from the native node.
                return BinaryNinja.Function.TakeHandle(
                    NativeMethods.BNFirmwareNinjaReferenceNodeGetFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Retrieves the data variable associated with this reference node.
        /// Returns null if this node does not represent a data variable or retrieval fails.
        /// Check IsDataVariable before calling this method.
        /// </summary>
        /// <returns>The DataVariable for this node, or null if not applicable.</returns>
        public unsafe DataVariable? GetDataVariable()
        {
            // 1. Stack-allocate a BNDataVariable struct for the native output.
            BNDataVariable nativeVar = new BNDataVariable();

            // 2. Call the native API, which fills in the struct and returns true on success.
            bool ok = NativeMethods.BNFirmwareNinjaReferenceNodeGetDataVariable(
                this.handle,
                (IntPtr)(&nativeVar)
            );

            if (!ok)
            {
                // Not a data variable node or retrieval failed; return null.
                return null;
            }

            // 3. Convert the native struct to the managed DataVariable object.
            return DataVariable.FromNative(nativeVar);
        }

        /// <summary>
        /// Gets all child reference nodes of this node in the reference tree.
        /// Each returned node is a new owned reference.
        /// </summary>
        public unsafe FirmwareNinjaReferenceNode[] Children
        {
            get
            {
                // 1. Stack-allocate the count variable and retrieve the native child array.
                ulong count = 0;

                IntPtr ptr = NativeMethods.BNFirmwareNinjaReferenceNodeGetChildren(
                    this.handle,
                    (IntPtr)(&count)
                );

                // 2. Convert the pointer array to managed objects, freeing the native array.
                return UnsafeUtils.TakeHandleArrayEx<FirmwareNinjaReferenceNode>(
                    ptr,
                    count,
                    FirmwareNinjaReferenceNode.MustNewFromHandle,
                    NativeMethods.BNFreeFirmwareNinjaReferenceNodes
                );
            }
        }
    }
}