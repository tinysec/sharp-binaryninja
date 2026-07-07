using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNCustomBinaryView 
	{
		// bool (*init)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool InitDelegate(
		    IntPtr ctxt
	    );
	    
	    // void (*freeObject)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void FreeObjectDelegate(
		    IntPtr ctxt
	    );
	    
	    // void (*externalRefTaken)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void ExternalRefTakenDelegate(
		    IntPtr ctxt
	    );
	    
	    // void (*externalRefReleased)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void ExternalRefReleasedDelegate(
		    IntPtr ctxt
	    );
	    
	    // size_t (*read)(void* ctxt, void* dest, uint64_t offset, size_t len);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong ReadDelegate(
		    IntPtr ctxt,
		    IntPtr dest,
		    ulong offset,
		    ulong length
	    );
	    
	    // size_t (*write)(void* ctxt, uint64_t offset, const void* src, size_t len);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong WriteDelegate(
		    IntPtr ctxt,
		    ulong offset,
		    IntPtr src,
		    ulong length
	    );
	    
	    // size_t (*insert)(void* ctxt, uint64_t offset, const void* src, size_t len);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong InsertDelegate(
		    IntPtr ctxt,
		    ulong offset,
		    IntPtr src,
		    ulong length
	    );
	    
	    // size_t (*remove)(void* ctxt, uint64_t offset, uint64_t len);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong RemoveDelegate(
		    IntPtr ctxt,
		    ulong offset,
		    ulong length
	    );
	    
	    // BNModificationStatus (*getModification)(void* ctxt, uint64_t offset);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ModificationStatus GetModificationDelegate(
		    IntPtr ctxt,
		    ulong offset
	    );
	    
	    // bool (*isValidOffset)(void* ctxt, uint64_t offset);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsValidOffsetDelegate(
		    IntPtr ctxt ,
		    ulong offset
	    );
	    
	    // bool (*isOffsetReadable)(void* ctxt, uint64_t offset);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsOffsetReadableDelegate(
		    IntPtr ctxt ,
		    ulong offset
	    );
	    
	    // bool (*isOffsetWritable)(void* ctxt, uint64_t offset);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsOffsetWritableDelegate(
		    IntPtr ctxt,
		    ulong offset
	    );
	    
	    // bool (*isOffsetExecutable)(void* ctxt, uint64_t offset);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsOffsetExecutableDelegate(
		    IntPtr ctxt,
		    ulong offset
	    );
	    
	    // bool (*isOffsetBackedByFile)(void* ctxt, uint64_t offset);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsOffsetBackedByFileDelegate(
		    IntPtr ctxt,
		    ulong offset
	    );
	    
	    // uint64_t (*getNextValidOffset)(void* ctxt, uint64_t offset);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong GetNextValidOffsetDelegate(
		    IntPtr ctxt,
		    ulong offset
	    );
	    
	    // uint64_t (*getStart)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong GetStartDelegate(
		    IntPtr ctxt
	    );
	    
	    
	    // uint64_t (*getLength)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong GetLengthDelegate(
		    IntPtr ctxt
	    );
	    
	    // uint64_t (*getEntryPoint)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong GetEntryPointDelegate(
		    IntPtr ctxt
	    );
	    
	    // bool (*isExecutable)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsExecutableDelegate(
		    IntPtr ctxt
	    );
	    
	    // BNEndianness (*getDefaultEndianness)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate Endianness GetDefaultEndiannessDelegate(
		    IntPtr ctxt
	    );
	    
	    // bool (*isRelocatable)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool IsRelocatableDelegate(
		    IntPtr ctxt
	    );
	    
	    // size_t (*getAddressSize)(void* ctxt);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate ulong GetAddressSizeDelegate(
		    IntPtr ctxt
	    );
	    
	    // bool (*save)(void* ctxt, BNFileAccessor* accessor);
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool SaveDelegate(
		    IntPtr ctxt,
		    IntPtr accessor
	    );
	    
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void* init
		/// </summary>
		internal IntPtr init;
		
		/// <summary>
		/// void* freeObject
		/// </summary>
		internal IntPtr freeObject;
		
		/// <summary>
		/// void* externalRefTaken
		/// </summary>
		internal IntPtr externalRefTaken;
		
		/// <summary>
		/// void* externalRefReleased
		/// </summary>
		internal IntPtr externalRefReleased;
		
		/// <summary>
		/// void* read
		/// </summary>
		internal IntPtr read;
		
		/// <summary>
		/// void* write
		/// </summary>
		internal IntPtr write;
		
		/// <summary>
		/// void* insert
		/// </summary>
		internal IntPtr insert;
		
		/// <summary>
		/// void* remove
		/// </summary>
		internal IntPtr remove;
		
		/// <summary>
		/// void* getModification
		/// </summary>
		internal IntPtr getModification;
		
		/// <summary>
		/// void* isValidOffset
		/// </summary>
		internal IntPtr isValidOffset;
		
		/// <summary>
		/// void* isOffsetReadable
		/// </summary>
		internal IntPtr isOffsetReadable;
		
		/// <summary>
		/// void* isOffsetWritable
		/// </summary>
		internal IntPtr isOffsetWritable;
		
		/// <summary>
		/// void* isOffsetExecutable
		/// </summary>
		internal IntPtr isOffsetExecutable;
		
		/// <summary>
		/// void* isOffsetBackedByFile
		/// </summary>
		internal IntPtr isOffsetBackedByFile;
		
		/// <summary>
		/// void* getNextValidOffset
		/// </summary>
		internal IntPtr getNextValidOffset;
		
		/// <summary>
		/// void* getStart
		/// </summary>
		internal IntPtr getStart;
		
		/// <summary>
		/// void* getLength
		/// </summary>
		internal IntPtr getLength;
		
		/// <summary>
		/// void* getEntryPoint
		/// </summary>
		internal IntPtr getEntryPoint;
		
		/// <summary>
		/// void* isExecutable
		/// </summary>
		internal IntPtr isExecutable;
		
		/// <summary>
		/// void* getDefaultEndianness
		/// </summary>
		internal IntPtr getDefaultEndianness;
		
		/// <summary>
		/// void* isRelocatable
		/// </summary>
		internal IntPtr isRelocatable;
		
		/// <summary>
		/// void* getAddressSize
		/// </summary>
		internal IntPtr getAddressSize;
		
		/// <summary>
		/// void* save
		/// </summary>
		internal IntPtr save;
	}

    public abstract class CustomBinaryView
    {
		// Cached thunk delegates for the ToNative() direction. A function pointer returned by
		// GetFunctionPointerForDelegate stays valid only while its source delegate is alive; the
		// inline method-group delegates (this.InitThunk, etc.) would otherwise be collectible the
		// moment ToNative() returns, and the next native callback into this registered view would
		// dereference freed memory (AccessViolation).
		private BNCustomBinaryView.InitDelegate? m_initThunk = null;

		private BNCustomBinaryView.FreeObjectDelegate? m_freeObjectThunk = null;

		private BNCustomBinaryView.ExternalRefTakenDelegate? m_externalRefTakenThunk = null;

		private BNCustomBinaryView.ExternalRefReleasedDelegate? m_externalRefReleasedThunk = null;

		private BNCustomBinaryView.ReadDelegate? m_readThunk = null;

		private BNCustomBinaryView.WriteDelegate? m_writeThunk = null;

		private BNCustomBinaryView.InsertDelegate? m_insertThunk = null;

		private BNCustomBinaryView.RemoveDelegate? m_removeThunk = null;

		private BNCustomBinaryView.GetModificationDelegate? m_getModificationThunk = null;

		private BNCustomBinaryView.IsValidOffsetDelegate? m_isValidOffsetThunk = null;

		private BNCustomBinaryView.IsOffsetReadableDelegate? m_isOffsetReadableThunk = null;

		private BNCustomBinaryView.IsOffsetWritableDelegate? m_isOffsetWritableThunk = null;

		private BNCustomBinaryView.IsOffsetExecutableDelegate? m_isOffsetExecutableThunk = null;

		private BNCustomBinaryView.IsOffsetBackedByFileDelegate? m_isOffsetBackedByFileThunk = null;

		private BNCustomBinaryView.GetNextValidOffsetDelegate? m_getNextValidOffsetThunk = null;

		private BNCustomBinaryView.GetStartDelegate? m_getStartThunk = null;

		private BNCustomBinaryView.GetLengthDelegate? m_getLengthThunk = null;

		private BNCustomBinaryView.GetEntryPointDelegate? m_getEntryPointThunk = null;

		private BNCustomBinaryView.IsExecutableDelegate? m_isExecutableThunk = null;

		private BNCustomBinaryView.GetDefaultEndiannessDelegate? m_getDefaultEndiannessThunk = null;

		private BNCustomBinaryView.IsRelocatableDelegate? m_isRelocatableThunk = null;

		private BNCustomBinaryView.GetAddressSizeDelegate? m_getAddressSizeThunk = null;

		private BNCustomBinaryView.SaveDelegate? m_saveThunk = null;

		public CustomBinaryView()
		{

		}

		public BNCustomBinaryView ToNative()
		{
			// Build the thunk delegates once and store them in fields so they stay rooted for the
			// lifetime of this view. The core keeps the function pointers after CreateView(), so
			// the delegate objects must outlive every native callback.
			BNCustomBinaryView.InitDelegate initThunk = new BNCustomBinaryView.InitDelegate(this.InitThunk);

			BNCustomBinaryView.FreeObjectDelegate freeObjectThunk = new BNCustomBinaryView.FreeObjectDelegate(this.FreeObjectThunk);

			BNCustomBinaryView.ExternalRefTakenDelegate externalRefTakenThunk = new BNCustomBinaryView.ExternalRefTakenDelegate(this.ExternalRefTakenThunk);

			BNCustomBinaryView.ExternalRefReleasedDelegate externalRefReleasedThunk = new BNCustomBinaryView.ExternalRefReleasedDelegate(this.ExternalRefReleasedThunk);

			BNCustomBinaryView.ReadDelegate readThunk = new BNCustomBinaryView.ReadDelegate(this.ReadThunk);

			BNCustomBinaryView.WriteDelegate writeThunk = new BNCustomBinaryView.WriteDelegate(this.WriteThunk);

			BNCustomBinaryView.InsertDelegate insertThunk = new BNCustomBinaryView.InsertDelegate(this.InsertThunk);

			BNCustomBinaryView.RemoveDelegate removeThunk = new BNCustomBinaryView.RemoveDelegate(this.RemoveThunk);

			BNCustomBinaryView.GetModificationDelegate getModificationThunk = new BNCustomBinaryView.GetModificationDelegate(this.GetModificationThunk);

			BNCustomBinaryView.IsValidOffsetDelegate isValidOffsetThunk = new BNCustomBinaryView.IsValidOffsetDelegate(this.IsValidOffsetThunk);

			BNCustomBinaryView.IsOffsetReadableDelegate isOffsetReadableThunk = new BNCustomBinaryView.IsOffsetReadableDelegate(this.IsOffsetReadableThunk);

			BNCustomBinaryView.IsOffsetWritableDelegate isOffsetWritableThunk = new BNCustomBinaryView.IsOffsetWritableDelegate(this.IsOffsetWritableThunk);

			BNCustomBinaryView.IsOffsetExecutableDelegate isOffsetExecutableThunk = new BNCustomBinaryView.IsOffsetExecutableDelegate(this.IsOffsetExecutableThunk);

			BNCustomBinaryView.IsOffsetBackedByFileDelegate isOffsetBackedByFileThunk = new BNCustomBinaryView.IsOffsetBackedByFileDelegate(this.IsOffsetBackedByFileThunk);

			BNCustomBinaryView.GetNextValidOffsetDelegate getNextValidOffsetThunk = new BNCustomBinaryView.GetNextValidOffsetDelegate(this.GetNextValidOffsetThunk);

			BNCustomBinaryView.GetStartDelegate getStartThunk = new BNCustomBinaryView.GetStartDelegate(this.GetStartThunk);

			BNCustomBinaryView.GetLengthDelegate getLengthThunk = new BNCustomBinaryView.GetLengthDelegate(this.GetLengthThunk);

			BNCustomBinaryView.GetEntryPointDelegate getEntryPointThunk = new BNCustomBinaryView.GetEntryPointDelegate(this.GetEntryPointThunk);

			BNCustomBinaryView.IsExecutableDelegate isExecutableThunk = new BNCustomBinaryView.IsExecutableDelegate(this.IsExecutableThunk);

			BNCustomBinaryView.GetDefaultEndiannessDelegate getDefaultEndiannessThunk = new BNCustomBinaryView.GetDefaultEndiannessDelegate(this.GetDefaultEndiannessThunk);

			BNCustomBinaryView.IsRelocatableDelegate isRelocatableThunk = new BNCustomBinaryView.IsRelocatableDelegate(this.IsRelocatableThunk);

			BNCustomBinaryView.GetAddressSizeDelegate getAddressSizeThunk = new BNCustomBinaryView.GetAddressSizeDelegate(this.GetAddressSizeThunk);

			BNCustomBinaryView.SaveDelegate saveThunk = new BNCustomBinaryView.SaveDelegate(this.SaveThunk);

			this.m_initThunk = initThunk;
			this.m_freeObjectThunk = freeObjectThunk;
			this.m_externalRefTakenThunk = externalRefTakenThunk;
			this.m_externalRefReleasedThunk = externalRefReleasedThunk;
			this.m_readThunk = readThunk;
			this.m_writeThunk = writeThunk;
			this.m_insertThunk = insertThunk;
			this.m_removeThunk = removeThunk;
			this.m_getModificationThunk = getModificationThunk;
			this.m_isValidOffsetThunk = isValidOffsetThunk;
			this.m_isOffsetReadableThunk = isOffsetReadableThunk;
			this.m_isOffsetWritableThunk = isOffsetWritableThunk;
			this.m_isOffsetExecutableThunk = isOffsetExecutableThunk;
			this.m_isOffsetBackedByFileThunk = isOffsetBackedByFileThunk;
			this.m_getNextValidOffsetThunk = getNextValidOffsetThunk;
			this.m_getStartThunk = getStartThunk;
			this.m_getLengthThunk = getLengthThunk;
			this.m_getEntryPointThunk = getEntryPointThunk;
			this.m_isExecutableThunk = isExecutableThunk;
			this.m_getDefaultEndiannessThunk = getDefaultEndiannessThunk;
			this.m_isRelocatableThunk = isRelocatableThunk;
			this.m_getAddressSizeThunk = getAddressSizeThunk;
			this.m_saveThunk = saveThunk;

			return new BNCustomBinaryView
			{
				context = IntPtr.Zero,
				init = Marshal.GetFunctionPointerForDelegate(initThunk),
				freeObject = Marshal.GetFunctionPointerForDelegate(freeObjectThunk),
				externalRefTaken = Marshal.GetFunctionPointerForDelegate(externalRefTakenThunk),
				externalRefReleased = Marshal.GetFunctionPointerForDelegate(externalRefReleasedThunk),
				read = Marshal.GetFunctionPointerForDelegate(readThunk),
				write = Marshal.GetFunctionPointerForDelegate(writeThunk),
				insert = Marshal.GetFunctionPointerForDelegate(insertThunk),
				remove = Marshal.GetFunctionPointerForDelegate(removeThunk),
				getModification = Marshal.GetFunctionPointerForDelegate(getModificationThunk),
				isValidOffset = Marshal.GetFunctionPointerForDelegate(isValidOffsetThunk),
				isOffsetReadable = Marshal.GetFunctionPointerForDelegate(isOffsetReadableThunk),
				isOffsetWritable = Marshal.GetFunctionPointerForDelegate(isOffsetWritableThunk),
				isOffsetExecutable = Marshal.GetFunctionPointerForDelegate(isOffsetExecutableThunk),
				isOffsetBackedByFile = Marshal.GetFunctionPointerForDelegate(isOffsetBackedByFileThunk),
				getNextValidOffset = Marshal.GetFunctionPointerForDelegate(getNextValidOffsetThunk),
				getStart = Marshal.GetFunctionPointerForDelegate(getStartThunk),
				getLength = Marshal.GetFunctionPointerForDelegate(getLengthThunk),
				getEntryPoint = Marshal.GetFunctionPointerForDelegate(getEntryPointThunk),
				isExecutable = Marshal.GetFunctionPointerForDelegate(isExecutableThunk),
				getDefaultEndianness = Marshal.GetFunctionPointerForDelegate(getDefaultEndiannessThunk),
				isRelocatable = Marshal.GetFunctionPointerForDelegate(isRelocatableThunk),
				getAddressSize = Marshal.GetFunctionPointerForDelegate(getAddressSizeThunk),
				save = Marshal.GetFunctionPointerForDelegate(saveThunk),
			};
		}
		
		public BinaryView? CreateView(
			string  name,
			BinaryView parent ,
			FileMetadata? file = null
		)
		{
			if (null == file)
			{
				file = parent.File;
			}

			return BinaryView.TakeHandle(
				NativeMethods.BNCreateCustomBinaryView(
					name ,
					file.DangerousGetHandle() ,
					parent.DangerousGetHandle() ,
					this.ToNative()
				)
			);
		}
		
		#region Thunk
	
		// bool (*init)(void* ctxt);
	    private bool InitThunk(IntPtr ctxt)
	    {
		    return this.Init();
	    }
	    
	    // void (*freeObject)(void* ctxt);
	    private void FreeObjectThunk(IntPtr ctxt)
	    {
		    this.FreeObject();
	    }
	    
	    
	    // void (*externalRefTaken)(void* ctxt);
	    private void ExternalRefTakenThunk(IntPtr ctxt)
	    {
		    this.ExternalRefTaken();
	    }
	    
	    // void (*externalRefReleased)(void* ctxt);
	    private void ExternalRefReleasedThunk(IntPtr ctxt)
	    {
		   this.ExternalRefReleased();
	    }
	    
	    // size_t (*read)(void* ctxt, void* dest, uint64_t offset, size_t len);
	    private ulong ReadThunk(
		    IntPtr ctxt ,
		    IntPtr dest ,
		    ulong offset ,
		    ulong length
	    )
	    {
		    if (0 == length)
		    {
			    return 0;
		    }
		    
		    byte[] data = this.Read(offset, length);

		    if (0 != data.Length)
		    {
			    Marshal.Copy(data, 0, dest, data.Length);
		    }
		    
		    return (ulong)data.Length;
	    }
	    
	    // size_t (*write)(void* ctxt, uint64_t offset, const void* src, size_t len);
	    private ulong WriteThunk(
		    IntPtr ctxt ,
		    ulong offset ,
		    IntPtr src ,
		    ulong length
	    )
	    {
		    if (0 == length)
		    {
			    return 0;
		    }
		    
		    byte[] data = new byte[length];
		    
		    Marshal.Copy(src , data ,0, data.Length);
		    
		    return (ulong)this.Write(offset, data);
	    }
	    
	    // size_t (*insert)(void* ctxt, uint64_t offset, const void* src, size_t len);
	    private ulong InsertThunk(
		    IntPtr ctxt ,
		    ulong offset ,
		    IntPtr src ,
		    ulong length
	    )
	    {
		    if (0 == length)
		    {
			    return 0;
		    }
		    
		    byte[] data = new byte[length];
		    
		    Marshal.Copy(src , data ,0, data.Length);
		    
		    return (ulong)this.Insert(offset, data);
	    }
	    
	    // size_t (*remove)(void* ctxt, uint64_t offset, uint64_t len);
	    private ulong RemoveThunk(
		    IntPtr ctxt ,
		    ulong offset ,
		    ulong length
	    )
	    {
		    return this.Remove(offset, length);
	    }
	    
	    // BNModificationStatus (*getModification)(void* ctxt, uint64_t offset);
	    private ModificationStatus GetModificationThunk(
		    IntPtr ctxt ,
		    ulong offset
	    )
	    {
		    return this.GetModification(offset);
	    }
	    
	    // bool (*isValidOffset)(void* ctxt, uint64_t offset);
	    private bool IsValidOffsetThunk(
		    IntPtr ctxt ,
		    ulong offset
	    )
	    {
		    return this.IsValidOffset(offset);
	    }
	    
	    // bool (*isOffsetReadable)(void* ctxt, uint64_t offset);
	    private bool IsOffsetReadableThunk(
		    IntPtr ctxt ,
		    ulong offset
	    )
	    {
		    return this.IsOffsetReadable(offset);
	    }
	    
	    // bool (*isOffsetWritable)(void* ctxt, uint64_t offset);
	    private bool IsOffsetWritableThunk(
		    IntPtr ctxt ,
		    ulong offset
	    )
	    {
		    return this.IsOffsetWritable(offset);
	    }
	    
	    // bool (*isOffsetExecutable)(void* ctxt, uint64_t offset);
	    private bool IsOffsetExecutableThunk(
		    IntPtr ctxt ,
		    ulong offset
	    )
	    {
		    return this.IsOffsetExecutable(offset);
	    }
	    
	    // bool (*isOffsetBackedByFile)(void* ctxt, uint64_t offset);
	    private bool IsOffsetBackedByFileThunk(
		    IntPtr ctxt ,
		    ulong offset
	    )
	    {
		    return this.IsOffsetBackedByFile(offset);
	    }
	    
	    // uint64_t (*getNextValidOffset)(void* ctxt, uint64_t offset);
	    private ulong GetNextValidOffsetThunk(
		    IntPtr ctxt ,
		    ulong offset
	    )
	    {
		    return this.GetNextValidOffset(offset);
	    }
	    
	    // uint64_t (*getStart)(void* ctxt);
	    private ulong GetStartThunk(IntPtr ctxt)
	    {
		    return this.Start;
	    }
	    
	    
	    // uint64_t (*getLength)(void* ctxt);
	    private ulong GetLengthThunk(IntPtr ctxt)
	    {
		    return this.Length;
	    }
	    
	    // uint64_t (*getEntryPoint)(void* ctxt);
	    private ulong GetEntryPointThunk(IntPtr ctxt)
	    {
		    return this.EntryPoint;
	    }
	    
	    // bool (*isExecutable)(void* ctxt);
	    private bool IsExecutableThunk(IntPtr ctxt)
	    {
		    return this.Executable;
	    }
	    
	    // BNEndianness (*getDefaultEndianness)(void* ctxt);
	    private Endianness GetDefaultEndiannessThunk(IntPtr ctxt)
	    {
		    return this.DefaultEndianness;
	    }
	    
	    // bool (*isRelocatable)(void* ctxt);
	    private bool IsRelocatableThunk(IntPtr ctxt)
	    {
		    return this.Relocatable;
	    }
	    
	    // size_t (*getAddressSize)(void* ctxt);
	    private ulong GetAddressSizeThunk(IntPtr ctxt)
	    {
		    return this.AddressSize;
	    }
	    
	    // bool (*save)(void* ctxt, BNFileAccessor* accessor);
	    private bool SaveThunk(IntPtr ctxt , IntPtr accessor)
	    {
		    return this.Save(
			    FileAccessor.MustFromNativePointer(accessor)
			);
	    }
	    
	    #endregion Thunk

	    #region methods

	    public virtual bool Init()
	    {
		    return true;
	    }

	    public virtual void FreeObject()
	    {
		    
	    }
	    
	    public virtual void ExternalRefTaken()
	    {
		    
	    }
	    
	    public virtual void ExternalRefReleased()
	    {
		   
	    }
	    

	    public virtual byte[] Read(ulong offset , ulong length)
	    {
		    return Array.Empty<byte>();
	    }
	    
	    public virtual int Write(ulong offset , byte[] data)
	    {
		    return 0;
	    }
	    
	    public virtual int Insert(ulong offset , byte[] data)
	    {
		    return 0;
	    }
	    
	    public virtual ulong Remove(ulong offset , ulong length)
	    {
		    return 0;
	    }
	    
	    public virtual ModificationStatus GetModification(ulong offset)
	    {
		    return ModificationStatus.Original;
	    }
	    
	    public virtual bool IsValidOffset(ulong offset)
	    {
		    return false;
	    }
	    
	    public virtual bool IsOffsetReadable(ulong offset)
	    {
		    return false;
	    }
	    
	    public virtual bool IsOffsetWritable(ulong offset)
	    {
		    return false;
	    }
	    
	    public virtual bool IsOffsetExecutable(ulong offset)
	    {
		    return false;
	    }
	    
	    public virtual bool IsOffsetBackedByFile(ulong offset)
	    {
		    return false;
	    }
	    
	    public virtual ulong GetNextValidOffset(ulong offset)
	    {
		    return 0;
	    }
	    
	    public virtual ulong Start
	    {
		    get
		    {
			    return 0;
		    }
	    }
	    
	    public virtual ulong Length
	    {
		    get
		    {
			    return 0;
		    }
	    }
	    
	    public virtual ulong EntryPoint
	    {
		    get
		    {
			    return 0;
		    }
	    }
	    
	    public virtual bool Executable
	    {
		    get
		    {
			    return false;
		    }
	    }
	    
	    public virtual Endianness DefaultEndianness
	    {
		    get
		    {
			    return Endianness.LittleEndian;
		    }
	    }
	    
	    public virtual bool Relocatable
	    {
		    get
		    {
			    return false;
		    }
	    }
	    
	    public virtual ulong AddressSize
	    {
		    get
		    {
			    return 0;
		    }
	    }
	    
	    public virtual bool Save(FileAccessor accessor)
	    {
		    return false;
	    }

	    #endregion methods
    }
}