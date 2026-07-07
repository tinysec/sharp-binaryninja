using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNCustomBinaryViewType 
	{
		// BNBinaryView* (*create)(void* ctxt, BNBinaryView* data);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate IntPtr CreateDelegate(
		    IntPtr ctxt,
		    IntPtr data
	    );
	    
	    // BNBinaryView* (*parse)(void* ctxt, BNBinaryView* data);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate IntPtr ParseDelegate(
		    IntPtr ctxt,
		    IntPtr data
	    );
	    
	    // bool (*isValidForData)(void* ctxt, BNBinaryView* data);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsValidForDataDelegate(
		    IntPtr ctxt,
		    IntPtr data
	    );
	    
	    // bool (*isDeprecated)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsDeprecatedDelegate(
		    IntPtr ctxt
	    );
	    
	    // bool (*isForceLoadable)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsForceLoadableDelegate(
		    IntPtr ctxt
	    );
	    
	    // BNSettings* (*getLoadSettingsForData)(void* ctxt, BNBinaryView* data);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate IntPtr GetLoadSettingsForDataDelegate(
		    IntPtr ctxt,
		    IntPtr data
	    );

		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void* create
		/// </summary>
		internal IntPtr create;
		
		/// <summary>
		/// void* parse
		/// </summary>
		internal IntPtr parse;
		
		/// <summary>
		/// void* isValidForData
		/// </summary>
		internal IntPtr isValidForData;
		
		/// <summary>
		/// void* isDeprecated
		/// </summary>
		internal IntPtr isDeprecated;
		
		/// <summary>
		/// void* isForceLoadable
		/// </summary>
		internal IntPtr isForceLoadable;
		
		/// <summary>
		/// void* getLoadSettingsForData
		/// </summary>
		internal IntPtr getLoadSettingsForData;

		/// <summary>
		/// bool (*hasNoInitialContent)(void* ctxt)
		/// </summary>
		internal IntPtr hasNoInitialContent;
	}
	
    public abstract class CustomBinaryViewType : INativeWrapper<BNCustomBinaryViewType>
    {
		// Cached thunk delegates for the ToNative() direction. A function pointer returned by
		// GetFunctionPointerForDelegate stays valid only while its source delegate is alive; the
		// inline method-group delegates (this.CreateThunk) would otherwise be collectible the moment
		// ToNative() returns, and the next native callback into this registered type would
		// dereference freed memory (AccessViolation).
		private BNCustomBinaryViewType.CreateDelegate? m_createThunk = null;

		private BNCustomBinaryViewType.ParseDelegate? m_parseThunk = null;

		private BNCustomBinaryViewType.IsValidForDataDelegate? m_isValidForDataThunk = null;

		private BNCustomBinaryViewType.IsDeprecatedDelegate? m_isDeprecatedThunk = null;

		private BNCustomBinaryViewType.IsForceLoadableDelegate? m_isForceLoadableThunk = null;

		private BNCustomBinaryViewType.GetLoadSettingsForDataDelegate? m_getLoadSettingsForDataThunk = null;

		public CustomBinaryViewType() 
		{
		    
		}
		
		public BNCustomBinaryViewType ToNative()
		{
			// Build the thunk delegates once and store them in fields so they stay rooted for the
			// lifetime of this type. The core keeps the function pointers after Register(), so
			// the delegate objects must outlive every native callback.
			BNCustomBinaryViewType.CreateDelegate createThunk =
				new BNCustomBinaryViewType.CreateDelegate(this.CreateThunk);

			BNCustomBinaryViewType.ParseDelegate parseThunk =
				new BNCustomBinaryViewType.ParseDelegate(this.ParseThunk);

			BNCustomBinaryViewType.IsValidForDataDelegate isValidForDataThunk =
				new BNCustomBinaryViewType.IsValidForDataDelegate(this.IsValidForDataThunk);

			BNCustomBinaryViewType.IsDeprecatedDelegate isDeprecatedThunk =
				new BNCustomBinaryViewType.IsDeprecatedDelegate(this.IsDeprecatedThunk);

			BNCustomBinaryViewType.IsForceLoadableDelegate isForceLoadableThunk =
				new BNCustomBinaryViewType.IsForceLoadableDelegate(this.IsForceLoadableThunk);

			BNCustomBinaryViewType.GetLoadSettingsForDataDelegate getLoadSettingsForDataThunk =
				new BNCustomBinaryViewType.GetLoadSettingsForDataDelegate(this.GetLoadSettingsForDataThunk);

			this.m_createThunk = createThunk;
			this.m_parseThunk = parseThunk;
			this.m_isValidForDataThunk = isValidForDataThunk;
			this.m_isDeprecatedThunk = isDeprecatedThunk;
			this.m_isForceLoadableThunk = isForceLoadableThunk;
			this.m_getLoadSettingsForDataThunk = getLoadSettingsForDataThunk;

			return new BNCustomBinaryViewType
			{
				context = IntPtr.Zero,
				create = Marshal.GetFunctionPointerForDelegate(createThunk),
				parse = Marshal.GetFunctionPointerForDelegate(parseThunk),
				isValidForData = Marshal.GetFunctionPointerForDelegate(isValidForDataThunk),
				isDeprecated = Marshal.GetFunctionPointerForDelegate(isDeprecatedThunk),
				isForceLoadable = Marshal.GetFunctionPointerForDelegate(isForceLoadableThunk),
				getLoadSettingsForData = Marshal.GetFunctionPointerForDelegate(getLoadSettingsForDataThunk),
				hasNoInitialContent = IntPtr.Zero,
			};
		}
		
		public BinaryViewType? RegisterBinaryViewType(string name , string longName)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return BinaryViewType.FromHandle(
					NativeMethods.BNRegisterBinaryViewType(
						name , 
						longName ,
						allocator.AllocStruct(this.ToNative())
					)
				);
			}
		}
		
		
		#region thunk

		// BNBinaryView* (*create)(void* ctxt, BNBinaryView* data);
		private unsafe IntPtr CreateThunk(
			IntPtr ctxt,
			IntPtr data
		)
		{
			BinaryView? view = this.Create(
				new BinaryView(data , false)
			);

			if (view == null)
			{
				return IntPtr.Zero;
			}

			return view.DangerousGetHandle();
		}
	    
		// BNBinaryView* (*parse)(void* ctxt, BNBinaryView* data);
		private unsafe IntPtr ParseThunk(
			IntPtr ctxt ,
			IntPtr data
		)
		{
			BinaryView? view = this.Parse(new BinaryView(data , false));

			if (view == null)
			{
				return IntPtr.Zero;
			}

			return view.DangerousGetHandle();
		}
	    
		// bool (*isValidForData)(void* ctxt, BNBinaryView* data);
		private unsafe bool IsValidForDataThunk(
			IntPtr ctxt ,
			IntPtr data
		)
		{
			return this.IsValidForData(
				new BinaryView(data, false)
			);
		}
	    
		// bool (*isDeprecated)(void* ctxt);
		private unsafe bool IsDeprecatedThunk(
			IntPtr ctxt
		)
		{
			return this.Deprecated;
		}
	    
		// bool (*isForceLoadable)(void* ctxt);
		private unsafe bool IsForceLoadableThunk(IntPtr ctxt)
		{
			return this.ForceLoadable;
		}
		
	    
		// BNSettings* (*getLoadSettingsForData)(void* ctxt, BNBinaryView* data);
		private unsafe IntPtr GetLoadSettingsForDataThunk(
			IntPtr ctxt ,
			IntPtr data
		)
		{
			Settings? settings = this.GetLoadSettingsForData(
				new BinaryView(data , false)
			);

			if (null == settings)
			{
				return IntPtr.Zero;
			}

			return settings.DangerousGetHandle();
		}

		#endregion
		
		#region  methods
		
	
		public virtual BinaryView? Create(BinaryView data)
		{
			return null;
		}
		
		public virtual BinaryView? Parse(BinaryView data)
		{
			return null;
		}
		
		public virtual bool IsValidForData(BinaryView data)
		{
			return false;
		}
		
		public virtual bool Deprecated
		{
			get
			{
				return false;
			}
		}
		
		public virtual bool ForceLoadable
		{
			get
			{
				return false;
			}
		}
		
		public virtual Settings? GetLoadSettingsForData(BinaryView data)
		{
			return null;
		}
		
		#endregion methods
    }
}