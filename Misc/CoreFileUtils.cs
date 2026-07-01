using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		/// <summary>
		/// Join a path and a part together using the platform path separator.
		/// </summary>
		public static string AppendPath(string path , string part)
		{
			// ABI 176 removed BNAppendPath from core; use the managed equivalent.
			return System.IO.Path.Combine(path , part);
		}

		/// <summary>
		/// Get the parent directory of a path.
		/// </summary>
		public static string GetParentPath(string path)
		{
			// ABI 176 removed BNGetParentPath from core; use the managed equivalent.
			return System.IO.Path.GetDirectoryName(path) ?? string.Empty;
		}

		/// <summary>
		/// Get the file extension of a path.
		/// </summary>
		public static string GetFileExtension(string path)
		{
			// ABI 176 removed BNGetFileExtension from core; use the managed equivalent.
			return System.IO.Path.GetExtension(path);
		}

		/// <summary>
		/// Get a path relative to the bundled plugin directory.
		/// </summary>
		public static string GetPathRelativeToBundledPluginDirectory(string path)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetPathRelativeToBundledPluginDirectory(path)
			);
		}

		/// <summary>
		/// Get a path relative to the user directory.
		/// </summary>
		public static string GetPathRelativeToUserDirectory(string path)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetPathRelativeToUserDirectory(path)
			);
		}

		/// <summary>
		/// Get a path relative to the user plugin directory.
		/// </summary>
		public static string GetPathRelativeToUserPluginDirectory(string path)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetPathRelativeToUserPluginDirectory(path)
			);
		}

		/// <summary>
		/// Get the system cache directory path.
		/// </summary>
		public static string GetSystemCacheDirectory()
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetSystemCacheDirectory()
			);
		}

		/// <summary>
		/// Copy a file from source to destination.
		/// </summary>
		public static bool CopyFile(string source , string dest)
		{
			return NativeMethods.BNCopyFile(source , dest);
		}

		/// <summary>
		/// Delete a file at the specified path.
		/// </summary>
		public static bool DeleteFile(string path)
		{
			return NativeMethods.BNDeleteFile(path);
		}

		/// <summary>
		/// Delete a directory at the specified path.
		/// </summary>
		public static bool DeleteDirectory(string path)
		{
			return NativeMethods.BNDeleteDirectory(path);
		}

		/// <summary>
		/// Create a directory at the specified path.
		/// </summary>
		public static bool CreateDirectory(string path , bool createSubdirectories = false)
		{
			return NativeMethods.BNCreateDirectory(path , createSubdirectories);
		}

		/// <summary>
		/// Rename or move a file from source to destination.
		/// </summary>
		public static bool RenameFile(string source , string dest)
		{
			return NativeMethods.BNRenameFile(source , dest);
		}

		/// <summary>
		/// Check whether a path exists on the filesystem.
		/// </summary>
		public static bool PathExists(string path)
		{
			return NativeMethods.BNPathExists(path);
		}

		/// <summary>
		/// Check whether a path is a directory.
		/// </summary>
		public static bool IsPathDirectory(string path)
		{
			return NativeMethods.BNIsPathDirectory(path);
		}

		/// <summary>
		/// Check whether a path is a regular file.
		/// </summary>
		public static bool IsPathRegularFile(string path)
		{
			return NativeMethods.BNIsPathRegularFile(path);
		}

		/// <summary>
		/// Get the size of a file in bytes. Returns null if the file does not exist or the size cannot be determined.
		/// </summary>
		public static unsafe ulong? FileSize(string path)
		{
			ulong size = 0;

			bool result = NativeMethods.BNFileSize(path , (IntPtr)(&size));

			if (!result)
			{
				return null;
			}

			return size;
		}

		/// <summary>
		/// Get all file paths in a directory.
		/// </summary>
		public static unsafe string[] GetFilePathsInDirectory(string path)
		{
			ulong count = 0;

			IntPtr arrayPointer = NativeMethods.BNGetFilePathsInDirectory(path , (IntPtr)(&count));

			return UnsafeUtils.TakeAnsiStringArray(
				arrayPointer ,
				count ,
				NativeMethods.BNFreeStringList
			);
		}

		/// <summary>
		/// Decode a base64-encoded string into a data buffer.
		/// </summary>
		public static DataBuffer DecodeBase64(string str)
		{
			return DataBuffer.MustTakeHandle(
				NativeMethods.BNDecodeBase64(str)
			);
		}

		/// <summary>
		/// Decode an escaped string into a data buffer.
		/// </summary>
		public static DataBuffer DecodeEscapedString(string str)
		{
			return DataBuffer.MustTakeHandle(
				NativeMethods.BNDecodeEscapedString(str)
			);
		}
	}
}
