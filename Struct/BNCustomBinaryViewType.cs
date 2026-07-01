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
		public CustomBinaryViewType() 
		{
		    
		}
		
		public BNCustomBinaryViewType ToNative()
		{
			return new BNCustomBinaryViewType
			{
				context = IntPtr.Zero,
				create = Marshal.GetFunctionPointerForDelegate<BNCustomBinaryViewType.CreateDelegate>(this.CreateThunk),
				parse = Marshal.GetFunctionPointerForDelegate<BNCustomBinaryViewType.ParseDelegate>(this.ParseThunk),
				isValidForData = Marshal.GetFunctionPointerForDelegate<BNCustomBinaryViewType.IsValidForDataDelegate>(this.IsValidForDataThunk),
				isDeprecated = Marshal.GetFunctionPointerForDelegate<BNCustomBinaryViewType.IsDeprecatedDelegate>(this.IsDeprecatedThunk),
				isForceLoadable = Marshal.GetFunctionPointerForDelegate<BNCustomBinaryViewType.IsForceLoadableDelegate>(this.IsForceLoadableThunk),
				getLoadSettingsForData = Marshal.GetFunctionPointerForDelegate<BNCustomBinaryViewType.GetLoadSettingsForDataDelegate>(this.GetLoadSettingsForDataThunk),
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