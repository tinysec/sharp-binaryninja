using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryNinja
{
	public class ScopedAllocator : IDisposable
	{
		private readonly List<IntPtr> m_pointers = new List<IntPtr>();

		private bool m_disposed = false;
		
		public void Dispose()
		{
			if (!m_disposed)
			{
				foreach (IntPtr pointer in m_pointers)
				{
					if (pointer != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(pointer);
					}
				}

				m_pointers.Clear();
				
				m_disposed = true;
			}
		}
		
		internal IntPtr AllocHGlobal(int size)
		{
			IntPtr pointer = Marshal.AllocHGlobal(size);
			
			m_pointers.Add(pointer);
		
			return pointer;
		}
		
		// BN core expects UTF-8 for every string input. Encode into an HGlobal buffer
		// tracked by this allocator, so disposal keeps using Marshal.FreeHGlobal.
		internal IntPtr AllocUtf8String(string text)
		{
			byte[] utf8 = Encoding.UTF8.GetBytes(text ?? string.Empty);

			IntPtr pointer = this.AllocHGlobal(utf8.Length + 1);

			Marshal.Copy(utf8 , 0 , pointer , utf8.Length);
			Marshal.WriteByte(pointer , utf8.Length , 0);

			return pointer;
		}

		internal IntPtr AllocUtf8StringArray(string[] texts)
		{
			if (texts == null || texts.Length == 0)
			{
				return IntPtr.Zero;
			}

			IntPtr arrayPointer = this.AllocHGlobal(IntPtr.Size * texts.Length);

			for (int i = 0; i < texts.Length; i++)
			{
				IntPtr textPointer = this.AllocUtf8String(texts[i]);

				Marshal.WriteIntPtr(arrayPointer , i * IntPtr.Size , textPointer);
			}

			return arrayPointer;
		}

		// The "Ansi"-named builders are misnomers retained for the generated call
		// sites; they delegate to the UTF-8 builders so every write path emits UTF-8.
		internal IntPtr AllocAnsiString(string text)
		{
			return this.AllocUtf8String(text);
		}

		internal IntPtr AllocAnsiStringArray(string[] texts)
		{
			return this.AllocUtf8StringArray(texts);
		}

		internal IntPtr AllocInteger<T>(T value) where T : IConvertible
		{
			int elementSize = Marshal.SizeOf<T>();
			
			IntPtr pointer = this.AllocHGlobal(elementSize);
			
			if (1 == elementSize)
			{
				Marshal.WriteByte(pointer , Convert.ToByte((IConvertible)value));
			}
			else if (2 == elementSize)
			{
				Marshal.WriteInt16(pointer , Convert.ToInt16((IConvertible)value));
			}
			else if (4 == elementSize)
			{
				Marshal.WriteInt32(pointer , Convert.ToInt32((IConvertible)value));
			}
			else if (8 == elementSize)
			{
				Marshal.WriteInt64(pointer , Convert.ToInt64((IConvertible)value));
			}
			else
			{
				throw new NotSupportedException("Unsupported element type");
			}

			return pointer;
		}
		
		internal IntPtr AllocIntegerArray<T>(T[] items) where T : IConvertible
		{
			if (items == null || items.Length == 0)
			{
				return IntPtr.Zero;
			}
			
			int elementSize = Marshal.SizeOf<T>();

			// Stride by the actual element size, not IntPtr.Size: AllocIntegerArray<uint>
			// (BNRegisterSetWithConfidence) writes 4 bytes per element, so the previous
			// IntPtr.Size stride over-allocated 2x (8x for byte arrays). Matches the
			// element-sized allocation already used by AllocStructArray.
			int totalSize = elementSize * items.Length;

			IntPtr arrayPointer = this.AllocHGlobal(totalSize);

			for (int i = 0; i < items.Length; i++)
			{
				IntPtr addressOfElement = IntPtr.Add(arrayPointer , i * elementSize);

				if (1 == elementSize)
				{
					Marshal.WriteByte(addressOfElement , Convert.ToByte((IConvertible)items[i]));
				}
				else if (2 == elementSize)
				{
					Marshal.WriteInt16(addressOfElement , Convert.ToInt16((IConvertible)items[i]));
				}
				else if (4 == elementSize)
				{
					Marshal.WriteInt32(addressOfElement , Convert.ToInt32((IConvertible)items[i]));
				}
				else if (8 == elementSize)
				{
					Marshal.WriteInt64(addressOfElement , Convert.ToInt64((IConvertible)items[i]));
				}
				else
				{
					throw new NotSupportedException("Unsupported element type");
				}
			}

			return arrayPointer;
		}
		
		internal IntPtr AllocStruct<T>(T structure) where T : struct
		{
			int size = Marshal.SizeOf<T>();
			
			IntPtr pointer = this.AllocHGlobal(size);
	
			Marshal.StructureToPtr(structure , pointer , false);

			return pointer;
		}

		internal IntPtr AllocStructArray<T>(T[] structures) where T : struct
		{
			if (structures == null || structures.Length == 0)
			{
				return IntPtr.Zero;
			}

			int structSize = Marshal.SizeOf<T>();
			
			int totalSize = structSize * structures.Length;
			
			IntPtr arrayPointer = this.AllocHGlobal(totalSize);

			for (int i = 0; i < structures.Length; i++)
			{
				IntPtr addressOfElement = IntPtr.Add(arrayPointer , i * structSize);
				
				Marshal.StructureToPtr(structures[i] , addressOfElement , false);
			}

			return arrayPointer;
		}
		
		internal IntPtr AllocHandleArray<T>(T[] handles) where T : SafeHandle
		{
			if (handles == null || handles.Length == 0)
			{
				return IntPtr.Zero;
			}
			
			int totalSize = IntPtr.Size * handles.Length;
			
			IntPtr arrayPointer = this.AllocHGlobal(totalSize);
	
			for (int i = 0; i < handles.Length; i++)
			{
				IntPtr addressOfElement = IntPtr.Add(arrayPointer , i * IntPtr.Size);
				
				Marshal.WriteIntPtr(addressOfElement , handles[i].DangerousGetHandle() );
			}

			return arrayPointer;
		}
		
		internal TNative[] ConvertToNativeArrayEx<TNative,TManaged>(TManaged[] sources)
			where TManaged : INativeWrapperEx<TNative>
		{
			if (sources == null || sources.Length == 0)
			{
				return Array.Empty<TNative>();
			}
				
			List<TNative> targets = new List<TNative>();

			for (int i = 0; i < sources.Length; i++)
			{
				targets.Add( sources[i].ToNativeEx(this));
			}
				
			return targets.ToArray();
		} 
	}
}
