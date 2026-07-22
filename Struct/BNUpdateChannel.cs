using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNUpdateChannel 
	{
		/// <summary>
		/// const char* name
		/// </summary>
		public IntPtr name;
		
		/// <summary>
		/// const char* description
		/// </summary>
		public IntPtr description;
		
		/// <summary>
		/// const char* latestVersion
		/// </summary>
		public IntPtr latestVersion;
	}

    public sealed class UpdateChannel 
    {
		public string Name { get; set; } = string.Empty;
		
		public string Description { get; set; } = string.Empty;
		
		public string LatestVersion { get; set; } = string.Empty;
		
		public UpdateChannel() 
		{
		    
		}

		internal static UpdateChannel FromNative(BNUpdateChannel native)
		{
			return new UpdateChannel
			{
				Name = UnsafeUtils.ReadUtf8String(native.name),
				Description = UnsafeUtils.ReadUtf8String(native.description),
				LatestVersion = UnsafeUtils.ReadUtf8String(native.latestVersion)
			};
		}

		/// <summary>Gets all available update channels.</summary>
		public static UpdateChannel[] GetList()
		{
			ulong count;
			IntPtr error;
			IntPtr raw = NativeMethods.BNGetUpdateChannels(out count, out error);
			UpdateInterop.ThrowIfError(error);

			try
			{
				return UnsafeUtils.ReadStructArray<BNUpdateChannel, UpdateChannel>(
					raw,
					count,
					UpdateChannel.FromNative
				);
			}
			finally
			{
				NativeMethods.BNFreeUpdateChannelList(raw, count);
			}
		}

		/// <summary>Gets an update channel by name.</summary>
		public static UpdateChannel GetByName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			UpdateChannel[] channels = UpdateChannel.GetList();
			foreach (UpdateChannel channel in channels)
			{
				if (string.Equals(channel.Name, name, StringComparison.Ordinal))
				{
					return channel;
				}
			}

			throw new KeyNotFoundException("Unknown update channel: " + name);
		}

		/// <summary>Gets the versions available in this channel.</summary>
		public UpdateVersion[] Versions
		{
			get { return UpdateVersion.GetChannelVersions(this.Name); }
		}

		/// <summary>Gets metadata for the channel's latest version, if available.</summary>
		public UpdateVersion? LatestVersionInfo
		{
			get
			{
				UpdateVersion[] versions = this.Versions;
				foreach (UpdateVersion version in versions)
				{
					if (string.Equals(version.Version, this.LatestVersion, StringComparison.Ordinal))
					{
						return version;
					}
				}

				return null;
			}
		}

		/// <summary>Checks whether this channel contains an update.</summary>
		public bool AreUpdatesAvailable(out ulong expireTime, out ulong serverTime)
		{
			IntPtr error;
			bool result = NativeMethods.BNAreUpdatesAvailable(
				this.Name,
				out expireTime,
				out serverTime,
				out error
			);
			UpdateInterop.ThrowIfError(error);
			return result;
		}

		/// <summary>Checks whether this channel contains an update.</summary>
		public bool UpdatesAvailable
		{
			get
			{
				ulong expireTime;
				ulong serverTime;
				return this.AreUpdatesAvailable(out expireTime, out serverTime);
			}
		}

		/// <summary>Downloads the requested channel version.</summary>
		public UpdateResult UpdateToVersion(string version, ProgressDelegate? progress = null)
		{
			if (null == version)
			{
				throw new ArgumentNullException(nameof(version));
			}

			UpdateProgressContext progressContext = new UpdateProgressContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			IntPtr error;
			UpdateResult result = NativeMethods.BNUpdateToVersion(
				this.Name,
				version,
				out error,
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress),
				IntPtr.Zero
			);
			GC.KeepAlive(nativeProgress);
			UpdateInterop.ThrowIfError(error);
			progressContext.ThrowIfFailed();
			return result;
		}

		/// <summary>Downloads the latest version in this channel.</summary>
		public UpdateResult UpdateToLatestVersion(ProgressDelegate? progress = null)
		{
			UpdateProgressContext progressContext = new UpdateProgressContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			IntPtr error;
			UpdateResult result = NativeMethods.BNUpdateToLatestVersion(
				this.Name,
				out error,
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress),
				IntPtr.Zero
			);
			GC.KeepAlive(nativeProgress);
			UpdateInterop.ThrowIfError(error);
			progressContext.ThrowIfFailed();
			return result;
		}
    }
}
