using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>Live proxy for a binary view's configured and resolved memory map.</summary>
	public sealed class MemoryMap
	{
		private readonly BinaryView view;

		internal MemoryMap(BinaryView view)
		{
			this.view = view;
		}

		public bool IsActivated
		{
			get { return NativeMethods.BNIsMemoryMapActivated(this.view.DangerousGetHandle()); }
		}

		public MemoryRegionInfo[] Regions
		{
			get { return this.GetMemoryRegions(); }
		}

		public ResolvedMemoryRange[] Ranges
		{
			get { return this.GetResolvedRanges(); }
		}

		public ResolvedMemoryRange[] ResolvedRanges
		{
			get { return this.GetResolvedRanges(); }
		}

		public string Description
		{
			get { return this.view.MemoryMapDescription; }
		}

		public string BaseDescription
		{
			get { return this.view.BaseMemoryMapDescription; }
		}

		public void SetLogicalMemoryMapEnabled(bool enabled)
		{
			NativeMethods.BNSetLogicalMemoryMapEnabled(this.view.DangerousGetHandle(), enabled);
		}

		public bool AddBinaryMemoryRegion(
			string name,
			ulong start,
			BinaryView source,
			uint flags = 0
		)
		{
			return this.view.AddMemoryRegion(name, start, source, flags);
		}

		public bool AddMemoryRegion(
			string name,
			ulong start,
			BinaryView source,
			uint flags = 0
		)
		{
			return this.AddBinaryMemoryRegion(name, start, source, flags);
		}

		public bool AddDataMemoryRegion(
			string name,
			ulong start,
			DataBuffer source,
			uint flags = 0
		)
		{
			return this.view.AddMemoryRegion(name, start, source, flags);
		}

		public bool AddMemoryRegion(
			string name,
			ulong start,
			DataBuffer source,
			uint flags = 0
		)
		{
			return this.AddDataMemoryRegion(name, start, source, flags);
		}

		public bool AddRemoteMemoryRegion(
			string name,
			ulong start,
			FileAccessor source,
			uint flags = 0
		)
		{
			return this.view.AddMemoryRegion(name, start, source, flags);
		}

		public bool AddMemoryRegion(
			string name,
			ulong start,
			FileAccessor source,
			uint flags = 0
		)
		{
			return this.AddRemoteMemoryRegion(name, start, source, flags);
		}

		public bool AddUnbackedMemoryRegion(
			string name,
			ulong start,
			ulong length,
			uint flags = 0,
			byte fill = 0
		)
		{
			return this.view.AddMemoryRegion(name, start, length, flags, fill);
		}

		public bool AddMemoryRegion(
			string name,
			ulong start,
			ulong length,
			uint flags = 0,
			byte fill = 0
		)
		{
			return this.AddUnbackedMemoryRegion(name, start, length, flags, fill);
		}

		public bool RemoveMemoryRegion(string name)
		{
			return this.view.RemoveMemoryRegion(name);
		}

		public string GetActiveMemoryRegionAt(ulong address)
		{
			return this.view.GetActiveMemoryRegionAt(address);
		}

		public uint GetMemoryRegionFlags(string name)
		{
			return this.view.GetMemoryRegionFlags(name);
		}

		public bool SetMemoryRegionFlags(string name, uint flags)
		{
			return this.view.SetMemoryRegionFlags(name, flags);
		}

		public bool IsMemoryRegionEnabled(string name)
		{
			return this.view.IsMemoryRegionEnabled(name);
		}

		public bool SetMemoryRegionEnabled(string name, bool enabled)
		{
			return this.view.SetMemoryRegionEnabled(name, enabled);
		}

		public bool IsMemoryRegionRebaseable(string name)
		{
			return this.view.IsMemoryRegionRebaseable(name);
		}

		public bool SetMemoryRegionRebaseable(string name, bool rebaseable)
		{
			return this.view.SetMemoryRegionRebaseable(name, rebaseable);
		}

		public byte GetMemoryRegionFill(string name)
		{
			return this.view.GetMemoryRegionFill(name);
		}

		public bool SetMemoryRegionFill(string name, byte fill)
		{
			return this.view.SetMemoryRegionFill(name, fill);
		}

		public bool IsMemoryRegionLocal(string name)
		{
			return this.view.IsMemoryRegionLocal(name);
		}

		public string GetMemoryRegionDisplayName(string name)
		{
			return UnsafeUtils.TakeUtf8String(
				NativeMethods.BNGetMemoryRegionDisplayName(this.view.DangerousGetHandle(), name)
			);
		}

		public bool SetMemoryRegionDisplayName(string name, string displayName)
		{
			return NativeMethods.BNSetMemoryRegionDisplayName(
				this.view.DangerousGetHandle(),
				name,
				displayName
			);
		}

		public MemoryRegionInfo? GetMemoryRegionInfo(string name)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr result = allocator.AllocHGlobal(Marshal.SizeOf<BNMemoryRegionInfo>());
				if (!NativeMethods.BNGetMemoryRegionInfo(
					this.view.DangerousGetHandle(),
					name,
					result
				))
				{
					return null;
				}

				try
				{
					return MemoryRegionInfo.FromNative(
						Marshal.PtrToStructure<BNMemoryRegionInfo>(result)
					);
				}
				finally
				{
					NativeMethods.BNFreeMemoryRegionInfo(result);
				}
			}
		}

		public MemoryRegionInfo? GetRegion(string name)
		{
			return this.GetMemoryRegionInfo(name);
		}

		public MemoryRegionInfo? GetActiveMemoryRegionInfoAt(ulong address)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr result = allocator.AllocHGlobal(Marshal.SizeOf<BNMemoryRegionInfo>());
				if (!NativeMethods.BNGetActiveMemoryRegionInfoAt(
					this.view.DangerousGetHandle(),
					address,
					result
				))
				{
					return null;
				}

				try
				{
					return MemoryRegionInfo.FromNative(
						Marshal.PtrToStructure<BNMemoryRegionInfo>(result)
					);
				}
				finally
				{
					NativeMethods.BNFreeMemoryRegionInfo(result);
				}
			}
		}

		public MemoryRegionInfo? GetActiveRegionAt(ulong address)
		{
			return this.GetActiveMemoryRegionInfoAt(address);
		}

		public ResolvedMemoryRange? GetResolvedMemoryRangeAt(ulong address)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr result = allocator.AllocHGlobal(Marshal.SizeOf<BNResolvedMemoryRange>());
				if (!NativeMethods.BNGetResolvedMemoryRangeAt(
					this.view.DangerousGetHandle(),
					address,
					result
				))
				{
					return null;
				}

				try
				{
					return ResolvedMemoryRange.FromNative(
						Marshal.PtrToStructure<BNResolvedMemoryRange>(result)
					);
				}
				finally
				{
					NativeMethods.BNFreeResolvedMemoryRange(result);
				}
			}
		}

		public ResolvedMemoryRange? GetResolvedRangeAt(ulong address)
		{
			return this.GetResolvedMemoryRangeAt(address);
		}

		public MemoryRegionInfo[] GetMemoryRegions()
		{
			IntPtr regions = NativeMethods.BNGetMemoryRegions(
				this.view.DangerousGetHandle(),
				out UIntPtr countValue
			);
			ulong count = countValue.ToUInt64();
			if (IntPtr.Zero == regions)
			{
				return Array.Empty<MemoryRegionInfo>();
			}

			try
			{
				MemoryRegionInfo[] result = new MemoryRegionInfo[checked((int)count)];
				int nativeSize = Marshal.SizeOf<BNMemoryRegionInfo>();
				for (ulong i = 0; i < count; i++)
				{
					int offset = checked((int)(i * (ulong)nativeSize));
					BNMemoryRegionInfo item = Marshal.PtrToStructure<BNMemoryRegionInfo>(
						IntPtr.Add(regions, offset)
					);
					result[checked((int)i)] = MemoryRegionInfo.FromNative(item);
				}

				return result;
			}
			finally
			{
				NativeMethods.BNFreeMemoryRegions(regions, countValue);
			}
		}

		public ResolvedMemoryRange[] GetResolvedRanges()
		{
			IntPtr ranges = NativeMethods.BNGetResolvedMemoryRanges(
				this.view.DangerousGetHandle(),
				out UIntPtr countValue
			);
			ulong count = countValue.ToUInt64();
			if (IntPtr.Zero == ranges)
			{
				return Array.Empty<ResolvedMemoryRange>();
			}

			try
			{
				ResolvedMemoryRange[] result = new ResolvedMemoryRange[checked((int)count)];
				int nativeSize = Marshal.SizeOf<BNResolvedMemoryRange>();
				for (ulong i = 0; i < count; i++)
				{
					int offset = checked((int)(i * (ulong)nativeSize));
					BNResolvedMemoryRange item = Marshal.PtrToStructure<BNResolvedMemoryRange>(
						IntPtr.Add(ranges, offset)
					);
					result[checked((int)i)] = ResolvedMemoryRange.FromNative(item);
				}

				return result;
			}
			finally
			{
				NativeMethods.BNFreeResolvedMemoryRanges(ranges, countValue);
			}
		}

		public void Reset()
		{
			this.view.ResetMemoryMap();
		}
	}
}
