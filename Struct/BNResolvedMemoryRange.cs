using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNResolvedMemoryRange
	{
		internal ulong start;
		internal ulong length;
		internal IntPtr regions;
		internal UIntPtr regionCount;
	}

	/// <summary>Immutable non-overlapping interval in the resolved memory map.</summary>
	public sealed class ResolvedMemoryRange
	{
		public ulong Start { get; }
		public ulong Length { get; }
		public ulong End { get { return checked(this.Start + this.Length); } }
		public MemoryRegionInfo[] Regions { get; }

		public MemoryRegionInfo? ActiveRegion
		{
			get { return 0 == this.Regions.Length ? null : this.Regions[0]; }
		}

		public string? Name
		{
			get
			{
				MemoryRegionInfo? active = this.ActiveRegion;
				return null == active ? null : active.Name;
			}
		}

		public SegmentFlag Flags
		{
			get
			{
				MemoryRegionInfo? active = this.ActiveRegion;
				return null == active ? (SegmentFlag)0 : active.Flags;
			}
		}

		internal ResolvedMemoryRange(
			ulong start,
			ulong length,
			MemoryRegionInfo[] regions
		)
		{
			this.Start = start;
			this.Length = length;
			this.Regions = regions;
		}

		internal static ResolvedMemoryRange FromNative(BNResolvedMemoryRange native)
		{
			ulong count = native.regionCount.ToUInt64();
			MemoryRegionInfo[] regions = new MemoryRegionInfo[checked((int)count)];
			int nativeSize = Marshal.SizeOf<BNMemoryRegionInfo>();

			for (ulong i = 0; i < count; i++)
			{
				int offset = checked((int)(i * (ulong)nativeSize));
				BNMemoryRegionInfo item = Marshal.PtrToStructure<BNMemoryRegionInfo>(
					IntPtr.Add(native.regions, offset)
				);
				regions[checked((int)i)] = MemoryRegionInfo.FromNative(item);
			}

			return new ResolvedMemoryRange(native.start, native.length, regions);
		}
	}
}
