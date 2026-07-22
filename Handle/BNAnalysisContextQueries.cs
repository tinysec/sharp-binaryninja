using System;

namespace BinaryNinja
{
	public sealed partial class AnalysisContext
	{
		/// <summary>Gets a cached boolean analysis setting.</summary>
		public bool GetSettingBool(string key)
		{
			return NativeMethods.BNAnalysisContextGetSettingBool(
				this.handle,
				AnalysisContext.RequireKey(key)
			);
		}

		/// <summary>Gets a cached double analysis setting.</summary>
		public double GetSettingDouble(string key)
		{
			return NativeMethods.BNAnalysisContextGetSettingDouble(
				this.handle,
				AnalysisContext.RequireKey(key)
			);
		}

		/// <summary>Gets a cached signed integer analysis setting.</summary>
		public long GetSettingInt64(string key)
		{
			return NativeMethods.BNAnalysisContextGetSettingInt64(
				this.handle,
				AnalysisContext.RequireKey(key)
			);
		}

		/// <summary>Gets a cached unsigned integer analysis setting.</summary>
		public ulong GetSettingUInt64(string key)
		{
			return NativeMethods.BNAnalysisContextGetSettingUInt64(
				this.handle,
				AnalysisContext.RequireKey(key)
			);
		}

		/// <summary>Gets a cached string analysis setting.</summary>
		public string GetSettingString(string key)
		{
			return UnsafeUtils.TakeUtf8String(
				NativeMethods.BNAnalysisContextGetSettingString(
					this.handle,
					AnalysisContext.RequireKey(key)
				)
			);
		}

		/// <summary>Gets a cached string-list analysis setting.</summary>
		public string[] GetSettingStringList(string key)
		{
			IntPtr values = NativeMethods.BNAnalysisContextGetSettingStringList(
				this.handle,
				AnalysisContext.RequireKey(key),
				out ulong count
			);
			return UnsafeUtils.TakeUtf8StringArray(
				values,
				count,
				NativeMethods.BNFreeStringList
			);
		}

		/// <summary>Gets the start of the cached memory map.</summary>
		public ulong Start
		{
			get { return NativeMethods.BNAnalysisContextGetStart(this.handle); }
		}

		/// <summary>Gets the end of the cached memory map.</summary>
		public ulong End
		{
			get { return NativeMethods.BNAnalysisContextGetEnd(this.handle); }
		}

		/// <summary>Gets the length of the cached memory map.</summary>
		public ulong Length
		{
			get { return NativeMethods.BNAnalysisContextGetLength(this.handle); }
		}

		/// <summary>Checks whether an address is mapped.</summary>
		public bool IsValidOffset(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsValidOffset(this.handle, offset);
		}

		/// <summary>Checks whether an address is readable.</summary>
		public bool IsOffsetReadable(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsOffsetReadable(this.handle, offset);
		}

		/// <summary>Checks whether an address is writable.</summary>
		public bool IsOffsetWritable(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsOffsetWritable(this.handle, offset);
		}

		/// <summary>Checks whether an address is executable.</summary>
		public bool IsOffsetExecutable(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsOffsetExecutable(this.handle, offset);
		}

		/// <summary>Checks whether an address is backed by file data.</summary>
		public bool IsOffsetBackedByFile(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsOffsetBackedByFile(this.handle, offset);
		}

		/// <summary>Checks whether an address has code section semantics.</summary>
		public bool IsOffsetCodeSemantics(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsOffsetCodeSemantics(this.handle, offset);
		}

		/// <summary>Checks whether an address has external section semantics.</summary>
		public bool IsOffsetExternSemantics(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsOffsetExternSemantics(this.handle, offset);
		}

		/// <summary>Checks whether an address has writable section semantics.</summary>
		public bool IsOffsetWritableSemantics(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsOffsetWritableSemantics(this.handle, offset);
		}

		/// <summary>Checks whether an address has read-only section semantics.</summary>
		public bool IsOffsetReadOnlySemantics(ulong offset)
		{
			return NativeMethods.BNAnalysisContextIsOffsetReadOnlySemantics(this.handle, offset);
		}

		/// <summary>Gets the next valid offset after an address.</summary>
		public ulong GetNextValidOffset(ulong offset)
		{
			return NativeMethods.BNAnalysisContextGetNextValidOffset(this.handle, offset);
		}

		/// <summary>Gets the next mapped address accepted by the flags.</summary>
		public ulong GetNextMappedAddress(ulong address, uint flags = 0)
		{
			return NativeMethods.BNAnalysisContextGetNextMappedAddress(this.handle, address, flags);
		}

		/// <summary>Gets the next file-backed address accepted by the flags.</summary>
		public ulong GetNextBackedAddress(ulong address, uint flags = 0)
		{
			return NativeMethods.BNAnalysisContextGetNextBackedAddress(this.handle, address, flags);
		}

		/// <summary>Gets all mapped ranges in the cached memory map.</summary>
		public AddressRange[] GetMappedAddressRanges()
		{
			IntPtr ranges = NativeMethods.BNAnalysisContextGetMappedAddressRanges(
				this.handle,
				out ulong count
			);
			return UnsafeUtils.TakeStructArray<BNAddressRange, AddressRange>(
				ranges,
				count,
				AddressRange.FromNative,
				NativeMethods.BNFreeAddressRanges
			);
		}

		/// <summary>Gets all file-backed ranges in the cached memory map.</summary>
		public AddressRange[] GetBackedAddressRanges()
		{
			IntPtr ranges = NativeMethods.BNAnalysisContextGetBackedAddressRanges(
				this.handle,
				out ulong count
			);
			return UnsafeUtils.TakeStructArray<BNAddressRange, AddressRange>(
				ranges,
				count,
				AddressRange.FromNative,
				NativeMethods.BNFreeAddressRanges
			);
		}

		private static string RequireKey(string key)
		{
			if (null == key)
			{
				throw new ArgumentNullException(nameof(key));
			}

			return key;
		}
	}
}
