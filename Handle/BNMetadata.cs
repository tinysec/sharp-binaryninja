using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Metadata : AbstractSafeHandle<Metadata>
	{
		public Metadata() 
			: this(Metadata.rawCreateMetadataDict( new Dictionary<string , Metadata>()) , true)
		{
	        
		}
		
	    public Metadata(bool value) 
		    : this(NativeMethods.BNCreateMetadataBooleanData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(string value) 
		    : this(NativeMethods.BNCreateMetadataStringData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(sbyte value) 
		    : this(NativeMethods.BNCreateMetadataSignedIntegerData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(byte value) 
		    : this(NativeMethods.BNCreateMetadataUnsignedIntegerData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(short value) 
		    : this(NativeMethods.BNCreateMetadataSignedIntegerData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(ushort value) 
		    : this(NativeMethods.BNCreateMetadataUnsignedIntegerData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(int value) 
		    : this(NativeMethods.BNCreateMetadataSignedIntegerData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(uint value) 
		    : this(NativeMethods.BNCreateMetadataUnsignedIntegerData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(long value) 
		    : this(NativeMethods.BNCreateMetadataSignedIntegerData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(ulong value) 
		    : this(NativeMethods.BNCreateMetadataUnsignedIntegerData(value) , true)
	    {
	        
	    }
	    
	    
	    public Metadata(float value) 
		    : this(NativeMethods.BNCreateMetadataDoubleData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(double value) 
		    : this(NativeMethods.BNCreateMetadataDoubleData(value) , true)
	    {
	        
	    }
	    
	    public Metadata(MetadataType value) 
		    : this(NativeMethods.BNCreateMetadataOfType(value) , true)
	    {
	        
	    }

	    public Metadata(byte[] value) 
		    : this(NativeMethods.BNCreateMetadataRawData(value , (ulong)value.Length) , true)
	    {
	        
	    }

	    public Metadata(bool[] value) 
		    : this(NativeMethods.BNCreateMetadataBooleanListData(value , (ulong)value.Length) , true)
	    {
	        
	    }
	    
	    public Metadata(long[] value) 
		    : this(NativeMethods.BNCreateMetadataSignedIntegerListData(value , (ulong)value.Length) , true)
	    {
	        
	    }
	    
	    public Metadata(ulong[] value) 
		    : this(NativeMethods.BNCreateMetadataUnsignedIntegerListData(value , (ulong)value.Length) , true)
	    {
	        
	    }
	    
	    public Metadata(double[] value) 
		    : this(NativeMethods.BNCreateMetadataDoubleListData(value , (ulong)value.Length) , true)
	    {
	        
	    }

	    public Metadata(string[] value) 
		    : this(NativeMethods.BNCreateMetadataStringListData(value , (ulong)value.Length) , true)
	    {
	        
	    }
	    
	    public Metadata(Metadata[] value) 
		    : this(rawCreateMetadataArray(value) , true)
	    {
	        
	    }
	    
	    public Metadata(IDictionary<string,Metadata> value) 
		    : this(Metadata.rawCreateMetadataDict(value) , true)
	    {
	        
	    }

	    public Metadata(IntPtr handle , bool owner) : base(owner)
	    {
		    this.SetHandle(handle);
	    }

	    public static Metadata FromBool(bool value)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataBooleanData(value)
		    );
	    }
	    
	    public static Metadata FromString(string value)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataStringData(value)
		    );
	    }
	    
	    public static Metadata FromUnsignedInteger(ulong value)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataUnsignedIntegerData(value)
		    );
	    }
	    
	    public static Metadata FromSignedInteger(long value)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataSignedIntegerData(value)
		    );
	    }
	    
	    public static Metadata FromDouble(double value)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataDoubleData(value)
		    );
	    }
	    
	    public static Metadata FromBytes(byte[] data)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataRawData(data , (ulong)data.Length)
		    );
	    }
	    
	    public static Metadata FromArray(Metadata[] values)
	    {
		    return Metadata.MustTakeHandle(
			    Metadata.rawCreateMetadataArray(values)
		    );
	    }
	    
	    private static IntPtr rawCreateMetadataArray(Metadata[] values)
	    {
		    List<IntPtr> handles = new List<IntPtr>();

		    foreach (Metadata item in values)
		    {
			    handles.Add(item.DangerousGetHandle());
		    }
		    
		    return NativeMethods.BNCreateMetadataArray(
				    handles.ToArray() , 
				    (ulong)handles.Count
			    );
	    }
	    
	    public static Metadata FromDict(IDictionary<string,Metadata> dict)
	    {
		    return Metadata.MustTakeHandle(
			    Metadata.rawCreateMetadataDict(dict)
		    );
	    }
	    
	    private static IntPtr rawCreateMetadataDict(IDictionary<string,Metadata> dict)
	    {
		    List<IntPtr> handles = new List<IntPtr>();

		    foreach (Metadata item in  dict.Values)
		    {
			    handles.Add(item.DangerousGetHandle());
		    }
		    
		    return NativeMethods.BNCreateMetadataValueStore(
			    dict.Keys.ToArray() ,
			    handles.ToArray(),
				(ulong)handles.Count
			 );
	    }
	    
	    public static Metadata FromBoolArray(bool[] values)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataBooleanListData(values , (ulong)values.Length)
		    );
	    }
	    
	    public static Metadata FromUnsignedIntegeArray(ulong[] values)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataUnsignedIntegerListData(values , (ulong)values.Length)
		    );
	    }
	    
	    public static Metadata FromSignedIntegerArray(long[] values)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataSignedIntegerListData(values , (ulong)values.Length)
		    );
	    }
	    
	    public static Metadata FromDoubleArray(double[] values)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataDoubleListData(values , (ulong)values.Length)
		    );
	    }
	    
	    public static Metadata FromStringArray(string[] values)
	    {
		    return Metadata.MustTakeHandle(
			    NativeMethods.BNCreateMetadataStringListData(values , (ulong)values.Length)
		    );
	    }
	    
	    internal static Metadata? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Metadata(
			    NativeMethods.BNNewMetadataReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Metadata MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Metadata(
			    NativeMethods.BNNewMetadataReference(handle) ,
			    true
		    );
	    }
	
	    internal static Metadata? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Metadata(handle, true);
	    }
	    
	    internal static Metadata MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Metadata(handle, true);
	    }

	    
	    internal static Metadata? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }

		    return new Metadata(handle , false);
	    }

	    internal static Metadata MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }

		    return new Metadata(handle , false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeMetadata(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public MetadataType Type
	    {
		    get
		    {
			    return NativeMethods.BNMetadataGetType(this.handle);
		    }
	    }
	    
	    public bool IsEquals(Metadata other)
	    {
		    return NativeMethods.BNMetadataIsEqual(
			    this.DangerousGetHandle(), 
			    other.DangerousGetHandle()
			);
	    }
	    
	    public bool ToBoolean()
	    {
		    return NativeMethods.BNMetadataGetBoolean(this.handle);
	    }

	    public string AsString()
	    {
		    return UnsafeUtils.TakeUtf8String(
			    NativeMethods.BNMetadataGetString(this.handle)
		    );
	    }
	    
	    public string ToJsonString()
	    {
		    return UnsafeUtils.TakeUtf8String(
			    NativeMethods.BNMetadataGetJsonString(this.handle)
		    );
	    }

	    public override string ToString()
	    {
		    return this.ToJsonString();
	    }
	    
	    public long ToSignedInteger()
	    {
		    return NativeMethods.BNMetadataGetSignedInteger(this.handle);
	    }
	    
	    public ulong ToUnsignedIntege()
	    {
		    return NativeMethods.BNMetadataGetUnsignedInteger(this.handle);
	    }
	    
	    public double ToDouble()
	    {
		    return NativeMethods.BNMetadataGetDouble(this.handle);
	    }
	    
	    public bool[] ToBooleanList()
	    {
		    IntPtr arrayPointer =  NativeMethods.BNMetadataGetBooleanList(
			    this.handle ,
			    out ulong arrayLength
			);
		    
		    bool[] values = UnsafeUtils.ReadBoolArray(arrayPointer , arrayLength);

		    if (arrayPointer != IntPtr.Zero )
		    {
			    NativeMethods.BNFreeMetadataBooleanList(arrayPointer , arrayLength);
		    }

		    return values;
	    }
	    
	    public string[] ToStringList()
	    {
		    IntPtr arrayPointer =  NativeMethods.BNMetadataGetStringList(
			    this.handle ,
			    out ulong arrayLength
		    );
		    
		    string[] values = UnsafeUtils.ReadAnsiStringArray(arrayPointer , arrayLength);

		    if (arrayPointer != IntPtr.Zero )
		    {
			    NativeMethods.BNFreeMetadataStringList(arrayPointer , arrayLength);
		    }

		    return values;
	    }
	    
	    
	    public long[] ToSignedIntegerList()
	    {
		    IntPtr arrayPointer =  NativeMethods.BNMetadataGetSignedIntegerList(
			    this.handle ,
			    out ulong arrayLength
		    );
		    
		    long[] values = UnsafeUtils.ReadNumberArray<long>(arrayPointer , arrayLength);

		    if (arrayPointer != IntPtr.Zero )
		    {
			    NativeMethods.BNFreeMetadataSignedIntegerList(arrayPointer , arrayLength);
		    }

		    return values;
	    }
	    
	    public ulong[] ToUnsignedIntegerList()
	    {
		    IntPtr arrayPointer =  NativeMethods.BNMetadataGetUnsignedIntegerList(
			    this.handle ,
			    out ulong arrayLength
		    );
		    
		    ulong[] values = UnsafeUtils.ReadNumberArray<ulong>(arrayPointer , arrayLength);

		    if (arrayPointer != IntPtr.Zero )
		    {
			    NativeMethods.BNFreeMetadataUnsignedIntegerList(arrayPointer , arrayLength);
		    }

		    return values;
	    }
	    
	    public double[] ToDoubleList()
	    {
		    IntPtr arrayPointer =  NativeMethods.BNMetadataGetDoubleList(
			    this.handle ,
			    out ulong arrayLength
		    );
		    
		    double[] values = UnsafeUtils.ReadNumberArray<double>(arrayPointer , arrayLength);

		    if (arrayPointer != IntPtr.Zero )
		    {
			    NativeMethods.BNFreeMetadataDoubleList(arrayPointer , arrayLength);
		    }

		    return values;
	    }
	    
	    public Metadata[] ToArray()
	    {
		    IntPtr arrayPointer =  NativeMethods.BNMetadataGetArray(
			    this.handle ,
			    out ulong arrayLength
		    );
		    
		    return UnsafeUtils.TakeHandleArray<Metadata>(
			    arrayPointer,
			    arrayLength,
			    Metadata.MustNewFromHandle,
			    NativeMethods.BNFreeMetadataArray
			);
	    }

	    public byte[] ToBytes()
	    {
		    IntPtr arrayPointer =  NativeMethods.BNMetadataGetRaw(
			    this.handle ,
			    out ulong arrayLength
		    );
		    
		    byte[] values = UnsafeUtils.ReadNumberArray<byte>(arrayPointer , arrayLength);

		    if (arrayPointer != IntPtr.Zero )
		    {
			    NativeMethods.BNFreeMetadataRaw(arrayPointer);
		    }

		    return values;
	    }
	    
	    public IDictionary<string,Metadata> ToDictionary()
	    {
		    Dictionary<string,Metadata> target = new Dictionary<string,Metadata>();
		    
		    IntPtr pointer =  NativeMethods.BNMetadataGetValueStore(
			    this.handle 
		    );

		    if (IntPtr.Zero == pointer)
		    {
			    return target;
		    }

		    target = MetadataValueStore.MustFromNativePointer(pointer).ToDictionary();

		    NativeMethods.BNFreeMetadataValueStore(pointer);

		    return target;
	    }
	    
	    public bool IsBoolean()
	    {
		    return NativeMethods.BNMetadataIsBoolean(this.handle);
	    }
	    
	    public bool IsString()
	    {
		    return NativeMethods.BNMetadataIsString(this.handle);
	    }
	    
	    public bool IsUnsignedInteger()
	    {
		    return NativeMethods.BNMetadataIsUnsignedInteger(this.handle);
	    }
	    
	    public bool IsSignedInteger()
	    {
		    return NativeMethods.BNMetadataIsSignedInteger(this.handle);
	    }
	    
	    public bool IsDouble()
	    {
		    return NativeMethods.BNMetadataIsDouble(this.handle);
	    }
	    
	    public bool IsBooleanList()
	    {
		    return NativeMethods.BNMetadataIsBooleanList(this.handle);
	    }
	    
	    public bool IsStringList()
	    {
		    return NativeMethods.BNMetadataIsStringList(this.handle);
	    }
	    
	    public bool IsUnsignedIntegerList()
	    {
		    return NativeMethods.BNMetadataIsUnsignedIntegerList(this.handle);
	    }
	    
	    public bool IsSignedIntegerList()
	    {
		    return NativeMethods.BNMetadataIsSignedIntegerList(this.handle);
	    }
	    
	    public bool IsDoubleList()
	    {
		    return NativeMethods.BNMetadataIsDoubleList(this.handle);
	    }
	    
	    public bool IsRaw()
	    {
		    return NativeMethods.BNMetadataIsRaw(this.handle);
	    }
	    
	    public bool IsArray()
	    {
		    return NativeMethods.BNMetadataIsArray(this.handle);
	    }
	    
	    public bool IsKeyValueStore()
	    {
		    return NativeMethods.BNMetadataIsKeyValueStore(this.handle);
	    }

	    public ulong GetLength()
	    {
		    return NativeMethods.BNMetadataSize(this.handle);
	    }
	    
	    
	    public bool SetValue(string key , Metadata data)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    data.DangerousGetHandle()
			);
	    }

	    public bool SetValue(string key , bool value)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataBooleanData(value)
		    );
	    }
	    
	    public bool SetValue(string key , string value)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataStringData(value)
		    );
	    }
	  
	    public bool SetValue(string key , long value)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataSignedIntegerData(value)
		    );
	    }
	    
	    public bool SetValue(string key , ulong value)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataUnsignedIntegerData(value)
		    );
	    }
	   
	    public bool SetValue(string key , double value)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataDoubleData(value)
		    );
	    }
	    
	    public bool SetValue(string key , byte[] data)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataRawData(data , (ulong)data.Length)
		    );
	    }
	    
	    public bool SetValue(string key , MetadataType value)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataOfType(value)
		    );
	    }
	    
	    public bool SetValue(string key , bool[] values)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataBooleanListData(values , (ulong)values.Length)
		    );
	    }
	    
	    public bool SetValue(string key , long[] values)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataSignedIntegerListData(values , (ulong)values.Length)
		    );
	    }
	    
	    public bool SetValue(string key , ulong[] values)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataUnsignedIntegerListData(values , (ulong)values.Length)
		    );
	    }
	    
	    public bool SetValue(string key , double[] values)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataDoubleListData(values , (ulong)values.Length)
		    );
	    }
	    
	    public bool SetValue(string key , string[] values)
	    {
		    return NativeMethods.BNMetadataSetValueForKey(
			    this.handle, 
			    key, 
			    NativeMethods.BNCreateMetadataStringListData(values , (ulong)values.Length)
		    );
	    }

	    public Metadata? GetValue(string key)
	    {
		    return Metadata.TakeHandle(
			    NativeMethods.BNMetadataGetForKey(this.handle , key)
		    );
	    }
	    
	    public Metadata? GetValue(ulong index)
	    {
		    return Metadata.TakeHandle(
			    NativeMethods.BNMetadataGetForIndex(this.handle , index)
		    );
	    }
	    
	    public bool Append(Metadata element)
	    {
		    return NativeMethods.BNMetadataArrayAppend(
				    this.handle , 
				    element.DangerousGetHandle()
			);
	    }
	    
	    public void Remove(string key)
	    {
		    NativeMethods.BNMetadataRemoveKey(this.handle , key);
	    }

	    public void Remove(ulong index)
	    {
		    NativeMethods.BNMetadataRemoveIndex(this.handle , index);
	    }
	}
	
	
}