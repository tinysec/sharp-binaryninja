using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNUpdateVersion 
	{
		/// <summary>
		/// const char* version
		/// </summary>
		public IntPtr version;
		
		/// <summary>
		/// const char* notes
		/// </summary>
		public IntPtr notes;
		
		/// <summary>
		/// uint64_t time
		/// </summary>
		public ulong time;
	}

    public sealed class UpdateVersion 
    {
		private UpdateChannel? channel;

		public string Version { get; set; } = string.Empty;
		
		public string Notes { get; set; } = string.Empty;
		
		public ulong Time { get; set; } = 0;
		
		public UpdateVersion() 
		{
		    
		}

		internal static UpdateVersion FromNative(BNUpdateVersion native)
		{
			return new UpdateVersion
			{
				Version = UnsafeUtils.ReadUtf8String(native.version),
				Notes = UnsafeUtils.ReadUtf8String(native.notes),
				Time = native.time
			};
		}

		/// <summary>Gets the update channel containing this version.</summary>
		public UpdateChannel? Channel
		{
			get { return this.channel; }
		}

		/// <summary>Gets all versions in the named update channel.</summary>
		public static UpdateVersion[] GetChannelVersions(string channel)
		{
			if (null == channel)
			{
				throw new ArgumentNullException(nameof(channel));
			}

			ulong count;
			IntPtr error;
			IntPtr raw = NativeMethods.BNGetUpdateChannelVersions(channel, out count, out error);
			UpdateInterop.ThrowIfError(error);

			try
			{
				UpdateVersion[] versions = UnsafeUtils.ReadStructArray<BNUpdateVersion, UpdateVersion>(
					raw,
					count,
					UpdateVersion.FromNative
				);
				UpdateChannel owner = new UpdateChannel { Name = channel };
				foreach (UpdateVersion version in versions)
				{
					version.channel = owner;
				}

				return versions;
			}
			finally
			{
				NativeMethods.BNFreeUpdateChannelVersionList(raw, count);
			}
		}

		/// <summary>Downloads this update version.</summary>
		public UpdateResult Update(ProgressDelegate? progress = null)
		{
			if (null == this.channel)
			{
				throw new InvalidOperationException("The update version is not associated with a channel.");
			}

			return this.channel.UpdateToVersion(this.Version, progress);
		}
    }
}
