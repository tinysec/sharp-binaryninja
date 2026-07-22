using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNTypeContext 
	{
		/// <summary>
		/// BNType* type
		/// </summary>
		public IntPtr type;
		
		/// <summary>
		/// uint64_t offset
		/// </summary>
		public ulong offset;
	}

    public sealed class TypeContext 
    {
		public BinaryNinja.Type? Type { get; set; } = null;
		
		public ulong Offset { get; set; } = 0;
		
		public TypeContext() 
		{
		    
		}

		public TypeContext(BinaryNinja.Type type, ulong offset)
		{
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			this.Type = type;
			this.Offset = offset;
		}
    }
}
