using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void InitCallback(IntPtr context, IntPtr architecture);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate Endianness GetEndiannessCallback(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate ulong GetSizeCallback(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntPtr GetAssociatedArchitectureCallback(
			IntPtr context,
			ref ulong address);

		private readonly object registrationLock = new object();
		private Architecture? registeredArchitecture;

		/// <summary>
		/// Registers this architecture with the core for the lifetime of the process.
		/// </summary>
		public Architecture Register(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("An architecture name is required.", nameof(name));
			}

			lock (this.registrationLock)
			{
				if (null != this.registeredArchitecture)
				{
					throw new InvalidOperationException("This architecture is already registered.");
				}

				BNCustomArchitecture callbacks = new BNCustomArchitecture
				{
					context = IntPtr.Zero,
					init = UnsafeUtils.PinCallback<InitCallback>(this.InitAdapter),
					getEndianness = UnsafeUtils.PinCallback<GetEndiannessCallback>(
						this.GetEndiannessAdapter),
					getAddressSize = UnsafeUtils.PinCallback<GetSizeCallback>(
						this.GetAddressSizeAdapter),
					getDefaultIntegerSize = UnsafeUtils.PinCallback<GetSizeCallback>(
						this.GetDefaultIntegerSizeAdapter),
					getInstructionAlignment = UnsafeUtils.PinCallback<GetSizeCallback>(
						this.GetInstructionAlignmentAdapter),
					getMaxInstructionLength = UnsafeUtils.PinCallback<GetSizeCallback>(
						this.GetMaxInstructionLengthAdapter),
					getOpcodeDisplayLength = UnsafeUtils.PinCallback<GetSizeCallback>(
						this.GetOpcodeDisplayLengthAdapter),
					getAssociatedArchitectureByAddress =
						UnsafeUtils.PinCallback<GetAssociatedArchitectureCallback>(
							this.GetAssociatedArchitectureAdapter)
				};
				this.AddRegisterCallbacks(ref callbacks);
				this.AddFlagCallbacks(ref callbacks);
				this.AddInstructionCallbacks(ref callbacks);
				this.AddAssemblyAndPatchCallbacks(ref callbacks);

				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					IntPtr handle = NativeMethods.BNRegisterArchitecture(
						name,
						allocator.AllocStruct(callbacks));
					this.registeredArchitecture = Architecture.MustFromHandle(handle);
					return Architecture.MustFromHandle(handle);
				}
			}
		}

		private void InitAdapter(IntPtr context, IntPtr architectureHandle)
		{
			try
			{
				Architecture architecture = Architecture.MustFromHandle(architectureHandle);
				this.registeredArchitecture = architecture;
				this.Init(architecture);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in CustomArchitecture.Init: {0}", exception);
			}
		}

		private Endianness GetEndiannessAdapter(IntPtr context)
		{
			try
			{
				return this.GetEndianness();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetEndianness: {0}",
					exception);
				return Endianness.LittleEndian;
			}
		}

		private ulong GetAddressSizeAdapter(IntPtr context)
		{
			return this.InvokeSizeCallback(this.GetAddressSize, "GetAddressSize");
		}

		private ulong GetDefaultIntegerSizeAdapter(IntPtr context)
		{
			return this.InvokeSizeCallback(
				this.GetDefaultIntegerSize,
				"GetDefaultIntegerSize");
		}

		private ulong GetInstructionAlignmentAdapter(IntPtr context)
		{
			return this.InvokeSizeCallback(
				this.GetInstructionAlignment,
				"GetInstructionAlignment");
		}

		private ulong GetMaxInstructionLengthAdapter(IntPtr context)
		{
			return this.InvokeSizeCallback(
				this.GetMaxInstructionLength,
				"GetMaxInstructionLength");
		}

		private ulong GetOpcodeDisplayLengthAdapter(IntPtr context)
		{
			return this.InvokeSizeCallback(
				this.GetOpcodeDisplayLength,
				"GetOpcodeDisplayLength");
		}

		private ulong InvokeSizeCallback(Func<ulong> callback, string callbackName)
		{
			try
			{
				return callback();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.{0}: {1}",
					callbackName,
					exception);
				return 0;
			}
		}

		private IntPtr GetAssociatedArchitectureAdapter(
			IntPtr context,
			ref ulong address)
		{
			try
			{
				Architecture? architecture = this.GetAssociatedArchitectureByAddress(ref address);
				if (null == architecture)
				{
					architecture = this.registeredArchitecture;
				}

				return null == architecture
					? IntPtr.Zero
					: architecture.DangerousGetHandle();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetAssociatedArchitectureByAddress: {0}",
					exception);
				return null == this.registeredArchitecture
					? IntPtr.Zero
					: this.registeredArchitecture.DangerousGetHandle();
			}
		}
	}
}
