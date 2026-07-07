using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static void RegisterBinaryViewEvent( BinaryViewEventType type ,Action<BinaryView> callback)
		{
			Action<IntPtr , IntPtr> adapter = (ctx , view) =>
			{
				callback( BinaryView.MustBorrowHandle(view));
			};
		    
			// The core stores this function pointer for the BinaryView event's lifetime, so the
			// adapter must be rooted for the process lifetime. PinCallback adds it to a static
			// root list and returns the function pointer; without it the next GC would reclaim the
			// adapter and the event callback would crash.
			NativeMethods.BNRegisterBinaryViewEvent(
				type,
				UnsafeUtils.PinCallback(adapter),
				IntPtr.Zero
			);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNRegisterBinaryViewEvent(BNBinaryViewEventType type, void** callback, void* ctx)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterBinaryViewEvent"
        )]
		internal static extern void BNRegisterBinaryViewEvent(
			
			// BNBinaryViewEventType type
		    BinaryViewEventType type  , 
			
			// void** callback
		    IntPtr callback  , 
			
			// void* ctx
		    IntPtr ctx  
		);
	}
}