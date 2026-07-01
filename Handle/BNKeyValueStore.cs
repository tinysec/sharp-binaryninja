using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class KeyValueStore : AbstractSafeHandle<KeyValueStore>
	{
		public KeyValueStore() 
			: this( NativeMethods.BNCreateKeyValueStore() , true )
		{
			
		}
		
		public KeyValueStore(byte[] data) 
			: this( new DataBuffer(data) )
		{
			
		}
		
		public KeyValueStore(DataBuffer buffer) 
			: this( NativeMethods.BNCreateKeyValueStoreFromDataBuffer(buffer.DangerousGetHandle()) , true )
		{
			
		}
		
	    internal KeyValueStore(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }
	    
	    internal static KeyValueStore? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new KeyValueStore(
			    NativeMethods.BNNewKeyValueStoreReference(handle) ,
			    true
		    );
	    }
	    
	    internal static KeyValueStore MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new KeyValueStore(
			    NativeMethods.BNNewKeyValueStoreReference(handle) ,
			    true
		    );
	    }
	    
	    internal static KeyValueStore? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new KeyValueStore(handle, true);
	    }
	    
	    internal static KeyValueStore MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new KeyValueStore(handle, true);
	    }
	    
	    internal static KeyValueStore? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new KeyValueStore(handle, false);
	    }
	    
	    internal static KeyValueStore MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new KeyValueStore(handle, false);
	    }

	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeKeyValueStore(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public string[] Keys
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetKeyValueStoreKeys(
				    this.handle ,
				    out ulong arrayLength 
				);
			    
			    return UnsafeUtils.TakeAnsiStringArray(
				    arrayPointer,
				    arrayLength,
				    NativeMethods.BNFreeStringList
				);
		    }
	    }

	    public bool ContainsKey(string key)
	    {
		    return NativeMethods.BNKeyValueStoreHasValue(this.handle, key);
	    }
	    
	    public string GetValue(string key)
	    {
		    return UnsafeUtils.TakeUtf8String(
			    NativeMethods.BNGetKeyValueStoreValue(this.handle , key)
		    );
	    }
	    
	    public bool SetValue(string key , string value)
	    {
		    return NativeMethods.BNSetKeyValueStoreValue(this.handle , key , value);
	    }

	    public DataBuffer? GetValueBuffer(string key)
	    {
		    return DataBuffer.TakeHandle(
			    NativeMethods.BNGetKeyValueStoreBuffer(this.handle , key)
		    );
	    }

	    public bool SetValueBuffer(string key , DataBuffer buffer)
	    {
		    return NativeMethods.BNSetKeyValueStoreBuffer(this.handle , key , buffer.DangerousGetHandle());
	    }

	    public void BeginNamespace(string ns)
	    {
		    NativeMethods.BNBeginKeyValueStoreNamespace(this.handle, ns);
	    }
	    
	    public void EndNamespace()
	    {
		    NativeMethods.BNEndKeyValueStoreNamespace(this.handle);
	    }
	    
	    public bool IsEmpty()
	    {
		    return NativeMethods.BNIsKeyValueStoreEmpty(this.handle);
	    }

	    public ulong ValueSize
	    {
		    get
		    {
			    return NativeMethods.BNGetKeyValueStoreValueSize(this.handle);
		    }
	    }
	    
	    public ulong DataSize
	    {
		    get
		    {
			    return NativeMethods.BNGetKeyValueStoreDataSize(this.handle);
		    }
	    }
	    
	    public ulong StorageSize
	    {
		    get
		    {
			    return NativeMethods.BNGetKeyValueStoreValueStorageSize(this.handle);
		    }
	    }
	    
	    public ulong NamespaceSize
	    {
		    get
		    {
			    return NativeMethods.BNGetKeyValueStoreNamespaceSize(this.handle);
		    }
	    }

	    /// <summary>
	    /// Gets the entire contents of this key-value store as a serialized DataBuffer.
	    /// </summary>
	    /// <returns>A DataBuffer containing the serialized store data, or null on failure.</returns>
	    public DataBuffer? GetSerializedData()
	    {
		    return DataBuffer.TakeHandle(
			    NativeMethods.BNGetKeyValueStoreSerializedData(this.handle)
		    );
	    }

	    /// <summary>
	    /// Gets the hash of the value associated with the specified key.
	    /// </summary>
	    /// <param name="key">The key whose value hash to retrieve.</param>
	    /// <returns>A DataBuffer containing the hash, or null if the key does not exist.</returns>
	    public DataBuffer? GetValueHash(string key)
	    {
		    return DataBuffer.TakeHandle(
			    NativeMethods.BNGetKeyValueStoreValueHash(this.handle , key)
		    );
	    }
	}
}