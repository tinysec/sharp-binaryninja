using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNMemoryRegionInfo
	{
		internal IntPtr name;
		internal IntPtr displayName;
		internal ulong start;
		internal ulong length;
		internal uint flags;
		[MarshalAs(UnmanagedType.I1)] internal bool enabled;
		[MarshalAs(UnmanagedType.I1)] internal bool rebaseable;
		internal byte fill;
		[MarshalAs(UnmanagedType.I1)] internal bool hasTarget;
		[MarshalAs(UnmanagedType.I1)] internal bool absoluteAddressMode;
		[MarshalAs(UnmanagedType.I1)] internal bool local;
	}

	/// <summary>Immutable snapshot of a configured memory region.</summary>
	public sealed class MemoryRegionInfo
	{
		public string Name { get; }
		public string DisplayName { get; }
		public ulong Start { get; }
		public ulong Length { get; }
		public ulong End { get { return checked(this.Start + this.Length); } }
		public SegmentFlag Flags { get; }
		public bool Enabled { get; }
		public bool Rebaseable { get; }
		public byte Fill { get; }
		public bool HasTarget { get; }
		public bool AbsoluteAddressMode { get; }
		public bool Local { get; }

		internal MemoryRegionInfo(
			string name,
			string displayName,
			ulong start,
			ulong length,
			SegmentFlag flags,
			bool enabled,
			bool rebaseable,
			byte fill,
			bool hasTarget,
			bool absoluteAddressMode,
			bool local
		)
		{
			this.Name = name;
			this.DisplayName = displayName;
			this.Start = start;
			this.Length = length;
			this.Flags = flags;
			this.Enabled = enabled;
			this.Rebaseable = rebaseable;
			this.Fill = fill;
			this.HasTarget = hasTarget;
			this.AbsoluteAddressMode = absoluteAddressMode;
			this.Local = local;
		}

		internal static MemoryRegionInfo FromNative(BNMemoryRegionInfo native)
		{
			return new MemoryRegionInfo(
				UnsafeUtils.ReadUtf8String(native.name),
				UnsafeUtils.ReadUtf8String(native.displayName),
				native.start,
				native.length,
				(SegmentFlag)native.flags,
				native.enabled,
				native.rebaseable,
				native.fill,
				native.hasTarget,
				native.absoluteAddressMode,
				native.local
			);
		}
	}
}
