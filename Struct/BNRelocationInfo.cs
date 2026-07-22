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
		/// uint8_t relocationDataCache[8]
		/// </summary>
		public fixed byte relocationDataCache[8];
		
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

		internal static unsafe RelocationInfo FromNative(BNRelocationInfo native)
		{
			byte[] cache = new byte[8];
			for (int i = 0; i < cache.Length; i++)
			{
				cache[i] = native.relocationDataCache[i];
			}

			return new RelocationInfo()
			{
				Type = native.type,
				PcRelative = native.pcRelative,
				BaseRelative = native.baseRelative,
				Base = native._base,
				Size = native.size,
				TruncateSize = native.truncateSize,
				NativeType = native.nativeType,
				Addend = native.addend,
				HasSign = native.hasSign,
				ImplicitAddend = native.implicitAddend,
				External = native.external,
				SymbolIndex = native.symbolIndex,
				SectionIndex = native.sectionIndex,
				Address = native.address,
				Target = native.target,
				DataRelocation = native.dataRelocation,
				RelocationDataCache = cache
			};
		}

		internal unsafe BNRelocationInfo ToNative(BNRelocationInfo template)
		{
			template.type = this.Type;
			template.pcRelative = this.PcRelative;
			template.baseRelative = this.BaseRelative;
			template._base = this.Base;
			template.size = this.Size;
			template.truncateSize = this.TruncateSize;
			template.nativeType = this.NativeType;
			template.addend = this.Addend;
			template.hasSign = this.HasSign;
			template.implicitAddend = this.ImplicitAddend;
			template.external = this.External;
			template.symbolIndex = this.SymbolIndex;
			template.sectionIndex = this.SectionIndex;
			template.address = this.Address;
			template.target = this.Target;
			template.dataRelocation = this.DataRelocation;
			byte[] cache = this.RelocationDataCache ?? Array.Empty<byte>();
			for (int i = 0; i < 8; i++)
			{
				template.relocationDataCache[i] =
					i < cache.Length ? cache[i] : (byte)0;
			}

			return template;
		}
    }
}
