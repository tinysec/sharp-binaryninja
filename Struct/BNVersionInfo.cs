using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNVersionInfo
	{
		/// <summary>
		/// uint32_t major
		/// </summary>
		public uint major;
		
		/// <summary>
		/// uint32_t minor
		/// </summary>
		public uint minor;
		
		/// <summary>
		/// uint32_t build
		/// </summary>
		public uint build;
		
		/// <summary>
		/// const char* channel
		/// </summary>
		public IntPtr channel;
	}

    public sealed class VersionInfo 
    {
		public uint Major { get; set; } = 0;
		
		public uint Minor { get; set; } = 0;
		
		public uint Build { get; set; } = 0;
		
		public string Channel { get; set; } = string.Empty;
		
		public VersionInfo()
		{
		}

		public VersionInfo(uint major, uint minor = 0, uint build = 0, string channel = "")
		{
			this.Major = major;
			this.Minor = minor;
			this.Build = build;
			this.Channel = channel ?? string.Empty;
		}

		public VersionInfo(string version)
		{
			VersionInfo parsed = Core.ParseVersionString(version);
			this.Major = parsed.Major;
			this.Minor = parsed.Minor;
			this.Build = parsed.Build;
			this.Channel = parsed.Channel;
		}
		
		internal static VersionInfo TakeNative(BNVersionInfo raw)
		{
			try
			{
				return new VersionInfo(
					raw.major,
					raw.minor,
					raw.build,
					UnsafeUtils.ReadUtf8String(raw.channel));
			}
			finally
			{
				if (IntPtr.Zero != raw.channel)
				{
					NativeMethods.BNFreeString(raw.channel);
				}
			}
		}

		internal BNVersionInfo ToNativeEx(ScopedAllocator allocator)
		{
			return new BNVersionInfo
			{
				major = this.Major,
				minor = this.Minor,
				build = this.Build,
				channel = allocator.AllocUtf8String(this.Channel)
			};
		}

		public override string ToString()
		{
			string version = $"{this.Major}.{this.Minor}.{this.Build}";
			if (string.Empty == this.Channel)
			{
				return version;
			}

			return $"{version}-{this.Channel}";
		}

		public static bool operator <(VersionInfo smaller, VersionInfo larger)
		{
			return Core.VersionLessThan(smaller, larger);
		}

		public static bool operator >(VersionInfo larger, VersionInfo smaller)
		{
			return Core.VersionLessThan(smaller, larger);
		}

		public static bool operator <=(VersionInfo smaller, VersionInfo larger)
		{
			return !Core.VersionLessThan(larger, smaller);
		}

		public static bool operator >=(VersionInfo larger, VersionInfo smaller)
		{
			return !Core.VersionLessThan(larger, smaller);
		}
		
		public static VersionInfo GetVersionInfo()
		{
			return VersionInfo.TakeNative(
				NativeMethods.BNGetVersionInfo()
			);
		}

		public static uint GetBuildId()
		{
			return NativeMethods.BNGetBuildId();
		}
		
		public static uint GetCurrentCoreABIVersion()
		{
			return NativeMethods.BNGetCurrentCoreABIVersion();
		}
		
		public static uint GetMinimumCoreABIVersion()
		{
			return NativeMethods.BNGetMinimumCoreABIVersion();
		}
    }
}
