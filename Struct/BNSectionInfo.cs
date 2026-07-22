using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNSectionInfo
	{
		internal IntPtr name;
		internal ulong start;
		internal ulong length;
		internal SectionSemantics semantics;
		internal IntPtr type;
		internal ulong align;
		internal ulong entrySize;
		internal IntPtr linkedSection;
		internal IntPtr infoSection;
		internal ulong infoData;
	}

	/// <summary>Describes a section for a bulk add operation.</summary>
	public sealed class SectionInfo
	{
		public string Name { get; set; }
		public ulong Start { get; set; }
		public ulong Length { get; set; }
		public SectionSemantics Semantics { get; set; }
		public string Type { get; set; }
		public ulong Align { get; set; }
		public ulong EntrySize { get; set; }
		public string LinkedSection { get; set; }
		public string InfoSection { get; set; }
		public ulong InfoData { get; set; }

		public SectionInfo(
			string name,
			ulong start,
			ulong length,
			SectionSemantics semantics = SectionSemantics.DefaultSectionSemantics,
			string type = "",
			ulong align = 1,
			ulong entrySize = 1,
			string linkedSection = "",
			string infoSection = "",
			ulong infoData = 0
		)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.Start = start;
			this.Length = length;
			this.Semantics = semantics;
			this.Type = type ?? throw new ArgumentNullException(nameof(type));
			this.Align = align;
			this.EntrySize = entrySize;
			this.LinkedSection = linkedSection
				?? throw new ArgumentNullException(nameof(linkedSection));
			this.InfoSection = infoSection
				?? throw new ArgumentNullException(nameof(infoSection));
			this.InfoData = infoData;
		}

		internal BNSectionInfo ToNative(ScopedAllocator allocator)
		{
			return new BNSectionInfo()
			{
				name = allocator.AllocUtf8String(this.Name),
				start = this.Start,
				length = this.Length,
				semantics = this.Semantics,
				type = allocator.AllocUtf8String(this.Type),
				align = this.Align,
				entrySize = this.EntrySize,
				linkedSection = allocator.AllocUtf8String(this.LinkedSection),
				infoSection = allocator.AllocUtf8String(this.InfoSection),
				infoData = this.InfoData,
			};
		}
	}
}
