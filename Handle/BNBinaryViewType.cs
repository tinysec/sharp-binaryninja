using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class BinaryViewType : AbstractSafeHandle<BinaryViewType>
	{
		internal BinaryViewType(IntPtr handle)
			:base(handle, false)
	    {
	       
	    }
	
	    internal static BinaryViewType? FromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryViewType(handle);
	    }
	    
	    internal static BinaryViewType MustFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryViewType(handle);
	    }

	    public static BinaryViewType? FromName(string name)
	    {
		    return BinaryViewType.FromHandle(
			    NativeMethods.BNGetBinaryViewTypeByName(name)
		    );
	    }

	    public static BinaryViewType[] GetTypes()
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetBinaryViewTypes(
			    out ulong arrayLength
		    );
	
		    return UnsafeUtils.TakeHandleArray<BinaryViewType>(
			    arrayPointer, 
			    arrayLength,
			    BinaryViewType.MustFromHandle,
			    BinaryNinja.NativeMethods.BNFreeBinaryViewTypeList
		    );
	    }
	    
	    public static BinaryViewType? Register(
		    string name , 
		    string longName,
			CustomBinaryViewType type
		)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return BinaryViewType.FromHandle(
				    NativeMethods.BNRegisterBinaryViewType(
					    name , 
					    longName ,
					    allocator.AllocStruct(type.ToNative())
				    )
			    );
		    }
	    }
	    
	  
	    public string Name
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetBinaryViewTypeName(this.handle);
			  
			    return UnsafeUtils.TakeAnsiString(raw);
		    }
	    }
	    
	    public string LongName
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetBinaryViewTypeLongName(this.handle);
		
			    return UnsafeUtils.TakeAnsiString(raw);
		    }
	    }

	    public bool Deprecated
	    {
		    get
		    {
			    return NativeMethods.BNIsBinaryViewTypeDeprecated(this.handle);
		    }
	    }

	    public BinaryView? CreateBinaryView(BinaryView data)
	    {
		    return BinaryView.TakeHandle(
			    NativeMethods.BNCreateBinaryViewOfType(
				    this.handle ,
				    data.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public BinaryView? ParseBinaryView(BinaryView data)
	    {
		    return BinaryView.TakeHandle(
			    NativeMethods.BNParseBinaryViewOfType(
				    this.handle ,
				    data.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public bool IsValidForData(BinaryView data)
	    {
		    return NativeMethods.BNIsBinaryViewTypeValidForData(
			    this.handle ,
			    data.DangerousGetHandle()
		    );
	    }

	    public bool ForceLoadable
	    {
		    get
		    {
			    return NativeMethods.BNIsBinaryViewTypeForceLoadable(this.handle);
		    }
	    }

	    public Settings? GetDefaultLoadSettingsForData(BinaryView data)
	    {
		    return Settings.TakeHandle(
			    NativeMethods.BNGetBinaryViewDefaultLoadSettingsForData(
				    this.handle ,
				    data.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public Settings? GetLoadSettingsForData(BinaryView data)
	    {
		    return Settings.TakeHandle(
			    NativeMethods.BNGetBinaryViewLoadSettingsForData(
				    this.handle ,
				    data.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public void RegisterArchitecture(
		    uint id ,
		    Endianness endianness,
		    Architecture arch 
		)
	    {
		    NativeMethods.BNRegisterArchitectureForViewType(
			    this.handle, 
			    id ,
			    endianness,
			    arch.DangerousGetHandle() 
		    );
	    }
	    
	    public Architecture? GetArchitecture(uint id , Endianness endianness)
	    {
		    return Architecture.FromHandle(
			    NativeMethods.BNGetArchitectureForViewType(
				    this.handle ,
				    id ,
				    endianness
			    )
		    );
	    }

	 
	    public void RegisterPlatform(
		    uint id ,
		    Architecture arch ,
		    Platform platform
	    )
	    {
		    NativeMethods.BNRegisterPlatformForViewType(
			    this.handle, 
			    id ,
			    arch.DangerousGetHandle() ,
			    platform.DangerousGetHandle()
		    );
	    }
	    
	    public Platform? GetPlatform(uint id ,  Architecture arch )
	    {
		    return Platform.TakeHandle(
			    NativeMethods.BNGetPlatformForViewType(
				    this.handle ,
				    id ,
				    arch.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public void RegisterDefaultPlatform(Architecture arch , Platform platform)
	    {
		    NativeMethods.BNRegisterDefaultPlatformForViewType(
			    this.handle, 
			    arch.DangerousGetHandle() ,
			    platform.DangerousGetHandle()
			);
	    }
	    
	    public void RegisterPlatformRecognizer(
		    uint id ,
		    Endianness endianness,
		    Func<BinaryView,Metadata,Platform?> recognize
		)
	    {
		    Func<IntPtr,IntPtr,IntPtr,IntPtr> adapter = (ctx , view , metadata) => {
			 
			    Platform? platform = recognize( 
				    BinaryView.MustBorrowHandle(view), 
				    Metadata.MustBorrowHandle(metadata)
			    );

			    if (null == platform)
			    {
				    return IntPtr.Zero;
			    }
				
			    return platform.DangerousGetHandle();
		    };
		    
		    NativeMethods.BNRegisterPlatformRecognizerForViewType(
			    this.handle, 
			    id ,
			    endianness,
			    Marshal.GetFunctionPointerForDelegate<Func<IntPtr,IntPtr,IntPtr,IntPtr>>(
				    adapter
				),
			    IntPtr.Zero
		    );
	    }
	    
	    /// <summary>
	    /// this function should be called from the BNCustomBinaryView::init implementation
	    /// </summary>
	    /// <param name="id"></param>
	    /// <param name="endianness"></param>
	    /// <param name="view"></param>
	    /// <param name="metadata"></param>
	    public void RegisterPlatform(
		    uint id ,
		    BinaryNinja.Endianness endianness,
		    BinaryView view,
		    Metadata metadata
		)
	    {
		    NativeMethods.BNRecognizePlatformForViewType(
			    this.handle, 
			    id ,
			    endianness,
			    view.DangerousGetHandle(),
			    metadata.DangerousGetHandle()
		    );
	    }
	}
}