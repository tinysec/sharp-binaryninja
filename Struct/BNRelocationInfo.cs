using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNRelocationInfo 
	{
		/// <summary>
		/// BNRelocationType type
		/// </summary>
		public RelocationType type;
		
		/// <summary>
		/// bool pcRelative
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool pcRelative;
		
		/// <summary>
		/// bool baseRelative
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool baseRelative;
		
		/// <summary>
		/// uint64_t _base
		/// </summary>
		public ulong _base;
		
		/// <summary>
		/// uint64_t size
		/// </summary>
		public ulong size;
		
		/// <summary>
		/// uint64_t truncateSize
		/// </summary>
		public ulong truncateSize;
		
		/// <summary>
		/// uint64_t nativeType
		/// </summary>
		public ulong nativeType;
		
		/// <summary>
		/// uint64_t addend
		/// </summary>
		public ulong addend;
		
		/// <summary>
		/// bool hasSign
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool hasSign;
		
		/// <summary>
		/// bool implicitAddend
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool implicitAddend;
		
		/// <summary>
		/// bool external
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool external;
		
		/// <summary>
		/// uint64_t symbolIndex
		/// </summary>
		public ulong symbolIndex;
		
		/// <summary>
		/// uint64_t sectionIndex
		/// </summary>
		public ulong sectionIndex;
		
		/// <summary>
		/// uint64_t address
		/// </summary>
		public ulong address;
		
		/// <summary>
		/// uint64_t target
		/// </summary>
		public ulong target;
		
		/// <summary>
		/// bool dataRelocation
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool dataRelocation;
		
		/// <summary>
		/// uint8_t[8] relocationDataCache
		/// </summary>
		public IntPtr relocationDataCache;
		
		/// <summary>
		/// BNRelocationInfo* prev
		/// </summary>
		public IntPtr prev;
		
		/// <summary>
		/// BNRelocationInfo* next
		/// </summary>
		public IntPtr next;
	}

    public class RelocationInfo 
    {
		public RelocationType Type { get; set; } = RelocationType.ELFGlobalRelocationType;
		
		public bool PcRelative { get; set; } = false;
	
		public bool BaseRelative { get; set; } = false;
		
		public ulong Base { get; set; } = 0;
	
		public ulong Size { get; set; } = 0;
	
		public ulong TruncateSize { get; set; } = 0;
		
		public ulong NativeType { get; set; } = 0;
		
		public ulong Addend { get; set; } = 0;
		
		public bool HasSign { get; set; } = false;
		
		public bool ImplicitAddend { get; set; } = false;
	
		public bool External { get; set; } = false;
		
		public ulong SymbolIndex { get; set; } = 0;
		
		public ulong SectionIndex { get; set; } = 0;
		
		public ulong Address { get; set; } = 0;
		
		public ulong Target { get; set; } = 0;
		
		public bool DataRelocation { get; set; } = false;
		
		public byte[] RelocationDataCache { get; set; } = Array.Empty<byte>();
		
		public RelocationInfo? Prev { get; set; } = null;
	
		public RelocationInfo? Next { get; set; } = null;
		
		public RelocationInfo() 
		{
		    
		}
    }
}