using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNPluginVersion
	{
		/// <summary>
		/// char* id
		/// </summary>
		internal IntPtr id;

		/// <summary>
		/// char* versionString
		/// </summary>
		internal IntPtr versionString;

		/// <summary>
		/// char* longDescription
		/// </summary>
		internal IntPtr longDescription;

		/// <summary>
		/// char* changelog
		/// </summary>
		internal IntPtr changelog;

		/// <summary>
		/// uint64_t minimumClientVersion
		/// </summary>
		internal ulong minimumClientVersion;

		/// <summary>
		/// BNPluginVersionPlatform* platforms
		/// </summary>
		internal IntPtr platforms;

		/// <summary>
		/// size_t platformCount
		/// </summary>
		internal ulong platformCount;

		/// <summary>
		/// char* created
		/// </summary>
		internal IntPtr created;
	}
}
