using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNCustomStringTypeInfo
	{
		internal IntPtr name;

		internal IntPtr stringPrefix;

		internal IntPtr stringPostfix;
	}

	/// <summary>
	/// Describes the name and rendering delimiters for strings produced by a custom recognizer.
	/// </summary>
	public sealed class CustomStringType : AbstractSafeHandle<CustomStringType>
	{
		private CustomStringType(IntPtr handle)
			: base(handle, false)
		{
		}

		internal static CustomStringType MustFromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new CustomStringType(handle);
		}

		internal static CustomStringType? FromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CustomStringType(handle);
		}

		/// <summary>Registers a custom string type with the core.</summary>
		public static CustomStringType Register(
			string name,
			string stringPrefix = "",
			string stringPostfix = ""
		)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				BNCustomStringTypeInfo info = new BNCustomStringTypeInfo();
				info.name = allocator.AllocUtf8String(name);
				info.stringPrefix = allocator.AllocUtf8String(stringPrefix ?? string.Empty);
				info.stringPostfix = allocator.AllocUtf8String(stringPostfix ?? string.Empty);
				return CustomStringType.MustFromHandle(
					NativeMethods.BNRegisterCustomStringType(in info)
				);
			}
		}

		/// <summary>Finds a registered custom string type by name.</summary>
		public static CustomStringType? FromName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return CustomStringType.FromHandle(
				NativeMethods.BNGetCustomStringTypeByName(name)
			);
		}

		/// <summary>Gets every custom string type registered with the core.</summary>
		public static CustomStringType[] GetTypes()
		{
			IntPtr types = NativeMethods.BNGetCustomStringTypeList(out ulong count);
			return UnsafeUtils.TakeHandleArray<CustomStringType>(
				types,
				count,
				CustomStringType.MustFromHandle,
				NativeMethods.BNFreeCustomStringTypeList
			);
		}

		/// <summary>Gets the unique registered name of this string type.</summary>
		public string Name
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetCustomStringTypeName(this.handle)
				);
			}
		}

		/// <summary>Gets the text rendered before the opening quote.</summary>
		public string StringPrefix
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetCustomStringTypePrefix(this.handle)
				);
			}
		}

		/// <summary>Gets the text rendered after the closing quote.</summary>
		public string StringPostfix
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetCustomStringTypePostfix(this.handle)
				);
			}
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
