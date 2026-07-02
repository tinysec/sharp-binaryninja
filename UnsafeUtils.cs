using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class UnsafeUtils
	{
		public static unsafe TTo ForceConvert<TFrom, TTo>(TFrom from)
			where TFrom : unmanaged 
			where TTo : unmanaged
		{
			return *(TTo*)&from;
		}

		internal static bool ReadBool(IntPtr address)
		{
			return 0 != Marshal.ReadByte(address);
		}
		
		internal static unsafe T ReadNumber<T>(IntPtr address) where T : unmanaged
		{
			// Enum index types (for example LowLevelILInstructionIndex : ulong) are not matched
			// by the primitive dispatch below, and Marshal-based reads throw on them. Read those
			// by their raw width. Unsafe.ReadUnaligned is AOT-safe and needs no reflection.
			if (typeof(T).IsEnum)
			{
				return Unsafe.ReadUnaligned<T>((void*)address);
			}

			if (typeof(T).UnderlyingSystemType == typeof(sbyte))
			{
				return UnsafeUtils.ForceConvert<byte, T>(Marshal.ReadByte(address) );
			}
			else if (typeof(T).UnderlyingSystemType == typeof(byte))
			{
				return UnsafeUtils.ForceConvert<byte, T>(Marshal.ReadByte(address) );
			}
			else if (typeof(T).UnderlyingSystemType == typeof(short))
			{
				return UnsafeUtils.ForceConvert<short, T>(Marshal.ReadInt16(address));
			}
			else if (typeof(T).UnderlyingSystemType == typeof(ushort))
			{
				return UnsafeUtils.ForceConvert<short, T>(Marshal.ReadInt16(address)); 
			}
			else if (typeof(T).UnderlyingSystemType == typeof(int))
			{
				return UnsafeUtils.ForceConvert<int , T>(Marshal.ReadInt32(address));
			}
			else if (typeof(T).UnderlyingSystemType == typeof(uint))
			{
				return UnsafeUtils.ForceConvert<int , T>(Marshal.ReadInt32(address));
			}
			else if (typeof(T).UnderlyingSystemType == typeof(long))
			{
				return UnsafeUtils.ForceConvert<long , T>(Marshal.ReadInt64(address));
			}
			else if (typeof(T).UnderlyingSystemType == typeof(ulong))
			{
				return UnsafeUtils.ForceConvert<long ,T>(Marshal.ReadInt64(address));
			}
			else if (typeof(T).UnderlyingSystemType == typeof(float))
			{
				byte[] buf = new byte[4];
				
				Marshal.Copy(address, buf, 0, 4);
				
				float value = BitConverter.ToSingle(buf, 0);

				return UnsafeUtils.ForceConvert<float , T>(value);
			}
			else if (typeof(T).UnderlyingSystemType == typeof(double))
			{
				byte[] buf = new byte[8];
				
				Marshal.Copy(address, buf, 0, 8);
				
				double value = BitConverter.ToDouble(buf, 0);

				return UnsafeUtils.ForceConvert<double , T>(value);
			}

			throw new NotSupportedException($"not supported type: {typeof(T).UnderlyingSystemType}");
		}
		
		internal static bool[] ReadBoolArray(IntPtr arrayPointer , ulong arrayLength) 
		{
			List<bool> targets = new List<bool>();

			// Binary Ninja stores bool arrays as 1-byte elements (C++ bool), and ReadBool reads
			// a single byte. The default managed marshaling of bool is a 4-byte Win32 BOOL, so
			// Marshal.SizeOf<bool>() returns 4; striding by that would read past the native
			// buffer into uninitialized memory. The native element stride is one byte.
			int integerSize = 1;

			if (( IntPtr.Zero != arrayPointer ) && ( 0 != arrayLength ))
			{
				for (ulong i = 0; i < arrayLength; i++)
				{
					int offset = checked((int)(i * (ulong)integerSize));
					
					IntPtr addressOfElement = IntPtr.Add(arrayPointer, offset);

					targets.Add(UnsafeUtils.ReadBool(addressOfElement));
				}
			}
			
			return targets.ToArray();
		}
		
		internal static T[] ReadNumberArray<T>(IntPtr arrayPointer , ulong arrayLength) 
			where T : unmanaged
		{
			List<T> targets = new List<T>();

			// Unsafe.SizeOf matches Marshal.SizeOf for the numeric primitives but also works for
			// enum index types, whereas Marshal.SizeOf throws on them.
			int integerSize = Unsafe.SizeOf<T>();

			if (( IntPtr.Zero != arrayPointer ) && ( 0 != arrayLength ))
			{
				for (ulong i = 0; i < arrayLength; i++)
				{
					int offset = checked((int)(i * (ulong)integerSize));

					IntPtr addressOfElement = IntPtr.Add(arrayPointer, offset);

					targets.Add(UnsafeUtils.ReadNumber<T>(addressOfElement));
				}
			}
			
			return targets.ToArray();
		}
		
		internal static string ReadAnsiString(IntPtr address)
		{
			string text = string.Empty;
			
			if (IntPtr.Zero != address)
			{
				string? optional = Marshal.PtrToStringAnsi(address);

				if (null != optional)
				{
					text = optional;
				}
			}

			return text;
		}
		
		internal static string ReadUtf8String(IntPtr address)
		{
			string text = string.Empty;
			
			if (IntPtr.Zero != address)
			{
				string? optional = Marshal.PtrToStringUTF8(address);

				if (null != optional)
				{
					text = optional;
				}
			}

			return text;
		}
		
		internal static string ReadUtf16String(IntPtr address)
		{
			string text = string.Empty;
			
			if (IntPtr.Zero != address)
			{
				string? optional = Marshal.PtrToStringUni(address);

				if (null != optional)
				{
					text = optional;
				}
			}

			return text;
		}
		
		internal static string ReadUtf32String(IntPtr address)
		{
			string text = string.Empty;
		
    
			if (IntPtr.Zero != address)
			{
				int length = 0;
				while (Marshal.ReadInt32(address, length * 4) != 0)
				{
					length++;
				}
        
				if (length > 0)
				{
					byte[] bytes = new byte[length * 4];
					
					Marshal.Copy(address, bytes, 0, bytes.Length);
					
					text = System.Text.Encoding.UTF32.GetString(bytes);
				}
			}

			return text;
		}

		internal static string[] ReadAnsiStringArray(
			IntPtr arrayPointer ,
			ulong arrayLength 
		)
		{
			return ReadStringArray(
				arrayPointer, 
				arrayLength, 
				UnsafeUtils.ReadAnsiString
			);
		}
		
		internal static string[] ReadUtf8StringArray(
			IntPtr arrayPointer ,
			ulong arrayLength 
		)
		{
			return ReadStringArray(
				arrayPointer, 
				arrayLength, 
				UnsafeUtils.ReadUtf8String
			);
		}
		
		internal static string[] ReadStringArray(
			IntPtr arrayPointer , 
			ulong arrayLength,
			Func<IntPtr,string>? fnRead = null
		)
		{
			if (null == fnRead)
			{
				fnRead = UnsafeUtils.ReadAnsiString;
			}
			
			List<string> targets = new List<string>();

			if (( IntPtr.Zero != arrayPointer ) && ( 0 != arrayLength ))
			{
				for (ulong i = 0; i < arrayLength; i++)
				{
					int offset = checked((int)( i * (ulong)IntPtr.Size ));
					
					IntPtr addressOfElement = IntPtr.Add(arrayPointer, offset);
				
					IntPtr element = Marshal.ReadIntPtr(addressOfElement);
					
					if (IntPtr.Zero != element)
					{
						targets.Add( fnRead(element));
					}
				}
			}

			return targets.ToArray();
		}
		
		internal static TManaged[] ReadHandleArray<TManaged>(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Func<IntPtr, TManaged> createManaged
		)
		{
			List<TManaged> targets = new List<TManaged>();

			if (( IntPtr.Zero != arrayPointer ) && ( 0 != arrayLength ))
			{
				for (ulong i = 0; i < arrayLength; i++)
				{
					int offset = checked((int)(i * (ulong)IntPtr.Size));
					
					IntPtr addressOfElement = IntPtr.Add(arrayPointer, offset);
					
					IntPtr element = Marshal.ReadIntPtr(addressOfElement);

					if (element != IntPtr.Zero)
					{
						targets.Add( createManaged(element));
					}
				}
			}
		
			return targets.ToArray();
		}
		
		internal static TManaged[] ReadStructArray<
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] 
			TNative , TManaged>(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Func<TNative, TManaged> createManaged
		) where TNative : struct
		{
			List<TManaged> targets = new List<TManaged>();

			if (( IntPtr.Zero != arrayPointer ) && ( 0 != arrayLength ))
			{
				for (ulong i = 0; i < arrayLength; i++)
				{
					int offset = checked((int)(i * (ulong)Marshal.SizeOf<TNative>()));
					
					IntPtr addressOfElement = IntPtr.Add(arrayPointer, offset);
					
					TNative? raw = Marshal.PtrToStructure<TNative>(addressOfElement);

					if (raw.HasValue)
					{
						targets.Add(createManaged(raw.Value));
					}
				}
			}
		
			return targets.ToArray();
		}
		
		internal static string TakeAnsiString(
			IntPtr address ,
			Action<IntPtr>? freeText = null
		)
		{
			if (null == freeText)
			{
				freeText = NativeMethods.BNFreeString;
			}
			
			string text = UnsafeUtils.ReadAnsiString(address);
			
			if (IntPtr.Zero != address)
			{
				if (null != freeText)
				{
					freeText(address);
				}
			}

			return text;
		}
		
		internal static string TakeUtf8String(IntPtr address , Action<IntPtr>? freeText = null)
		{
			if (null == freeText)
			{
				freeText = NativeMethods.BNFreeString;
			}
			
			string text = UnsafeUtils.ReadUtf8String(address);
			
			if (IntPtr.Zero != address)
			{
				if (null != freeText)
				{
					freeText(address);
				}
			}

			return text;
		}
		
		internal static string TakeUtf16String(IntPtr address)
		{
			string text = UnsafeUtils.ReadUtf16String(address);
			
			if (IntPtr.Zero != address)
			{
				NativeMethods.BNFreeString(address);
			}

			return text;
		}
		
		internal static string TakeUtf32String(IntPtr address)
		{
			string text = UnsafeUtils.ReadUtf32String(address);
			
			if (IntPtr.Zero != address)
			{
				NativeMethods.BNFreeString(address);
			}

			return text;
		}
		
		internal static string[] TakeAnsiStringArray(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Action<IntPtr,ulong>? freeArray
		)
		{
			if (null == freeArray)
			{
				freeArray = NativeMethods.BNFreeStringList;
			}
			
			return TakeStringArrayEx(
				arrayPointer , 
				arrayLength,
				UnsafeUtils.ReadAnsiString,
				freeArray
			);
		}
		
		internal static string[] TakeUtf8StringArray(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Action<IntPtr,ulong>? freeArray = null
		)
		{
			if (null == freeArray)
			{
				freeArray = NativeMethods.BNFreeStringList;
			}
			
			return TakeStringArrayEx(
				arrayPointer , 
				arrayLength,
				UnsafeUtils.ReadUtf8String,
				freeArray
			);
		}
		
		internal static string[] TakeStringArrayEx(
			IntPtr arrayPointer , 
			ulong arrayLength,
			Func<IntPtr,string>? fnRead  = null,
			Action<IntPtr,ulong>? freeArray = null
		)
		{
			if (null == fnRead)
			{
				fnRead = UnsafeUtils.ReadAnsiString;
			}

			if (null == freeArray)
			{
				freeArray = NativeMethods.BNFreeStringList;
			}
			
			string[] targets = UnsafeUtils.ReadStringArray(
				arrayPointer, 
				arrayLength,
				fnRead
			);
			
			if (IntPtr.Zero != arrayPointer)
			{
				if (freeArray != null)
				{
					freeArray(arrayPointer , arrayLength);
				}
			}

			return targets;
		}
		
		
		
		internal static T[] TakeNumberArray<T>(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Action<IntPtr>? freeArray
		)  where T : unmanaged
		{
			T[] targets = UnsafeUtils.ReadNumberArray<T>(arrayPointer , arrayLength);

			if (arrayPointer != IntPtr.Zero && freeArray != null)
			{
				freeArray(arrayPointer);
			}
			
			return targets;
		}
		
		internal static T[] TakeNumberArrayEx<T>(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Action<IntPtr,ulong>? freeArray = null
		)  where T : unmanaged
		{
			T[] targets = UnsafeUtils.ReadNumberArray<T>(arrayPointer , arrayLength);

			if (arrayPointer != IntPtr.Zero && freeArray != null)
			{
				freeArray(arrayPointer , arrayLength);
			}
			
			return targets;
		}
		
		
		
		internal static TManaged[] TakeHandleArray<TManaged>(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Func<IntPtr, TManaged> createManaged,
			Action<IntPtr>? freeArray = null
		)
		{
			TManaged[] targets = ReadHandleArray( arrayPointer , arrayLength , createManaged );

			if (arrayPointer != IntPtr.Zero && freeArray != null)
			{
				freeArray(arrayPointer);
			}
			
			return targets;
		}
		
		internal static TManaged[] TakeHandleArrayEx<TManaged>(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Func<IntPtr, TManaged> createManaged,
			Action<IntPtr,ulong>? freeArray = null
		) 
		{
			TManaged[] targets = ReadHandleArray( arrayPointer , arrayLength , createManaged );

			if (arrayPointer != IntPtr.Zero && freeArray != null)
			{
				freeArray(arrayPointer ,arrayLength);
			}
			
			return targets;
		}
		
		
		internal static TManaged[] TakeStructArray<
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] 
			TNative , TManaged>(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Func<TNative, TManaged> createManaged,
			Action<IntPtr>? freeArray = null
		) where TNative : struct
		{
			TManaged[] targets = ReadStructArray<TNative,TManaged>(arrayPointer , arrayLength , createManaged );

			if (arrayPointer != IntPtr.Zero && freeArray != null)
			{
				freeArray(arrayPointer);
			}
			
			return targets;
		}
		
		internal static TManaged[] TakeStructArrayEx<
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] 
			TNative , TManaged>(
			IntPtr arrayPointer ,
			ulong arrayLength ,
			Func<TNative, TManaged> createManaged,
			Action<IntPtr,ulong>? freeArray = null
		) where TNative : struct
		{
			TManaged[] targets = ReadStructArray<TNative,TManaged>(arrayPointer , arrayLength , createManaged );

			if (arrayPointer != IntPtr.Zero && freeArray != null)
			{
				freeArray(arrayPointer , arrayLength);
			}
			
			return targets;
		}
		
		internal static TNative[] ConvertToNativeArray<TNative,TManaged>(TManaged[] sources)
			where TManaged : INativeWrapper<TNative>
		{
			if (sources == null || sources.Length == 0)
			{
				return Array.Empty<TNative>();
			}
				
			List<TNative> targets = new List<TNative>();

			for (int i = 0; i < sources.Length; i++)
			{
				targets.Add( sources[i].ToNative() );
			}
				
			return targets.ToArray();
		}

	
		internal static IntPtr AllocAnsiString(string text)
		{
			return Marshal.StringToHGlobalAnsi(text);
		}
		
		internal static IntPtr AllocUtf8String(string text)
		{
			return Marshal.StringToCoTaskMemUTF8(text);
		}

		internal static void FreeUtf8String(IntPtr text)
		{
			Marshal.FreeCoTaskMem(text);
		}

		internal static IntPtr AllocStruct<T>(T structure) where T : struct
		{
			int size = Marshal.SizeOf<T>();
			
			IntPtr pointer = Marshal.AllocHGlobal(size);
	
			Marshal.StructureToPtr(structure , pointer , false);

			return pointer;
		}

		internal static IntPtr AllocStructArray<T>(T[] structures) where T : struct
		{
			if (structures == null || structures.Length == 0)
			{
				return IntPtr.Zero;
			}

			int structSize = Marshal.SizeOf<T>();
			
			int totalSize = structSize * structures.Length;
			
			IntPtr arrayPointer = Marshal.AllocHGlobal(totalSize);

			for (int i = 0; i < structures.Length; i++)
			{
				IntPtr addressOfElement = IntPtr.Add(arrayPointer , i * structSize);
				
				Marshal.StructureToPtr(structures[i] , addressOfElement , false);
			}

			return arrayPointer;
		}
	}
}
