using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNDataVariableAndNameAndDebugParser 
	{
		/// <summary>
		/// uint64_t address
		/// </summary>
		public ulong address;
		
		/// <summary>
		/// BNType* type
		/// </summary>
		public IntPtr type;
		
		/// <summary>
		/// const char* name
		/// </summary>
		public IntPtr name;
		
		/// <summary>
		/// const char* parser
		/// </summary>
		public IntPtr parser;
		
		/// <summary>
		/// bool autoDiscovered
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool autoDiscovered;
		
		/// <summary>
		/// uint8_t typeConfidence
		/// </summary>
		public byte typeConfidence;
		
	}

    public class DataVariableAndNameAndDebugParser 
    {
		public ulong Address { get; set; } = 0;
		
		public BinaryNinja.Type? Type { get; set; } = null;
	
		public string Name { get; set; } = string.Empty;

		public string Parser { get; set; } = string.Empty;

		public bool AutoDiscovered { get; set; } = false;
		
		public byte TypeConfidence { get; set; } = 0;
		
		public DataVariableAndNameAndDebugParser() 
		{
		    
		}
    }
}