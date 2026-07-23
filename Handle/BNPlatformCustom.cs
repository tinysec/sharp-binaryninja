using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public partial class Platform
	{
		private static readonly object registrationLock = new object();

		private static readonly List<Platform> registeredPlatforms =
			new List<Platform>();

		private readonly bool custom;
		private bool registered;

		private NativeDelegates.BNPlatformInit? initCallback;
		private NativeDelegates.BNPlatformViewInit? viewInitCallback;
		private NativeDelegates.BNPlatformGetRegisterList? getGlobalRegistersCallback;
		private NativeDelegates.BNPlatformFreeRegisterList? freeRegisterListCallback;
		private NativeDelegates.BNPlatformGetGlobalRegisterType?
			getGlobalRegisterTypeCallback;
		private NativeDelegates.BNPlatformGetAddressSize? getAddressSizeCallback;
		private NativeDelegates.BNPlatformAdjustTypeParserInput?
			adjustTypeParserInputCallback;
		private NativeDelegates.BNPlatformFreeTypeParserInput?
			freeTypeParserInputCallback;
		private NativeDelegates.BNPlatformGetFallbackEnabled? getFallbackEnabledCallback;

		/// <summary>Runs platform-specific initialization after a view selects this platform.</summary>
		public virtual void BinaryViewInit(BinaryView view)
		{
		}

		/// <summary>Adjusts type-parser arguments and virtual source files in place.</summary>
		public virtual void AdjustTypeParserInput(
			TypeParser parser,
			List<string> arguments,
			List<PlatformTypeParserSourceFile> sourceFiles
		)
		{
		}

		/// <summary>Gets whether the platform may use its fallback type library.</summary>
		public virtual bool GetFallbackEnabled()
		{
			return true;
		}

		private static void RootForRegistration(Platform platform)
		{
			if (!platform.custom)
			{
				return;
			}

			lock (Platform.registrationLock)
			{
				if (!platform.registered)
				{
					platform.registered = true;
					Platform.registeredPlatforms.Add(platform);
				}
			}
		}

		private void InitializeCustomPlatform(
			Architecture architecture,
			string name,
			string? typeFile,
			string[] includeDirs
		)
		{
			if (null == architecture)
			{
				throw new ArgumentNullException(nameof(architecture));
			}

			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			BNCustomPlatform callbacks = this.CreateCustomCallbacks();
			IntPtr platformHandle;
			if (null == typeFile)
			{
				platformHandle = NativeMethods.BNCreateCustomPlatform(
					architecture.DangerousGetHandle(), name, in callbacks
				);
			}
			else
			{
				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					IntPtr nativeIncludeDirs = allocator.AllocUtf8StringArray(includeDirs);
					platformHandle = NativeMethods.BNCreateCustomPlatformWithTypes(
						architecture.DangerousGetHandle(),
						name,
						in callbacks,
						typeFile,
						nativeIncludeDirs,
						(ulong)includeDirs.Length
					);
				}
			}

			if (IntPtr.Zero == platformHandle)
			{
				throw new InvalidOperationException("The core rejected the custom platform.");
			}

			this.SetHandle(platformHandle);
		}

		private BNCustomPlatform CreateCustomCallbacks()
		{
			this.initCallback = new NativeDelegates.BNPlatformInit(this.InvokeInit);
			this.viewInitCallback = new NativeDelegates.BNPlatformViewInit(
				this.InvokeViewInit
			);
			this.getGlobalRegistersCallback =
				new NativeDelegates.BNPlatformGetRegisterList(
					this.InvokeGetGlobalRegisters
				);
			this.freeRegisterListCallback =
				new NativeDelegates.BNPlatformFreeRegisterList(
					this.InvokeFreeRegisterList
				);
			this.getGlobalRegisterTypeCallback =
				new NativeDelegates.BNPlatformGetGlobalRegisterType(
					this.InvokeGetGlobalRegisterType
				);
			this.getAddressSizeCallback =
				new NativeDelegates.BNPlatformGetAddressSize(this.InvokeGetAddressSize);
			this.adjustTypeParserInputCallback =
				new NativeDelegates.BNPlatformAdjustTypeParserInput(
					this.InvokeAdjustTypeParserInput
				);
			this.freeTypeParserInputCallback =
				new NativeDelegates.BNPlatformFreeTypeParserInput(
					this.InvokeFreeTypeParserInput
				);
			this.getFallbackEnabledCallback =
				new NativeDelegates.BNPlatformGetFallbackEnabled(
					this.InvokeGetFallbackEnabled
				);

			BNCustomPlatform callbacks = new BNCustomPlatform();
			callbacks.context = IntPtr.Zero;
			callbacks.init = Marshal.GetFunctionPointerForDelegate(this.initCallback);
			callbacks.viewInit = Marshal.GetFunctionPointerForDelegate(this.viewInitCallback);
			callbacks.getGlobalRegisters = Marshal.GetFunctionPointerForDelegate(
				this.getGlobalRegistersCallback
			);
			callbacks.freeRegisterList = Marshal.GetFunctionPointerForDelegate(
				this.freeRegisterListCallback
			);
			callbacks.getGlobalRegisterType = Marshal.GetFunctionPointerForDelegate(
				this.getGlobalRegisterTypeCallback
			);
			callbacks.getAddressSize = Marshal.GetFunctionPointerForDelegate(
				this.getAddressSizeCallback
			);
			callbacks.adjustTypeParserInput = Marshal.GetFunctionPointerForDelegate(
				this.adjustTypeParserInputCallback
			);
			callbacks.freeTypeParserInput = Marshal.GetFunctionPointerForDelegate(
				this.freeTypeParserInputCallback
			);
			callbacks.getFallbackEnabled = Marshal.GetFunctionPointerForDelegate(
				this.getFallbackEnabledCallback
			);

			return callbacks;
		}

		private void InvokeInit(IntPtr context, IntPtr platform)
		{
		}

		private void InvokeViewInit(IntPtr context, IntPtr view)
		{
			BinaryView? managedView = null;
			try
			{
				managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)
				);
				this.BinaryViewInit(managedView);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in Platform.BinaryViewInit: {0}", exception
				);
			}
			finally
			{
				if (null != managedView)
				{
					managedView.Dispose();
				}
			}
		}

		private IntPtr InvokeGetGlobalRegisters(IntPtr context, IntPtr count)
		{
			Marshal.WriteInt64(count, 0);
			try
			{
				uint[] registers = this.GlobalRegisters ?? Array.Empty<uint>();
				if (0 == registers.Length)
				{
					return IntPtr.Zero;
				}

				IntPtr result = Marshal.AllocHGlobal(registers.Length * sizeof(uint));
				for (int i = 0; i < registers.Length; i++)
				{
					Marshal.WriteInt32(result, i * sizeof(uint), unchecked((int)registers[i]));
				}

				Marshal.WriteInt64(count, registers.Length);
				return result;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in Platform.GlobalRegisters: {0}", exception
				);
				return IntPtr.Zero;
			}
		}

		private void InvokeFreeRegisterList(
			IntPtr context,
			IntPtr registers,
			ulong count
		)
		{
			if (IntPtr.Zero != registers)
			{
				Marshal.FreeHGlobal(registers);
			}
		}

		private IntPtr InvokeGetGlobalRegisterType(IntPtr context, uint register)
		{
			try
			{
				BinaryNinja.Type? type = this.GetGlobalRegisterType(register);
				if (null == type)
				{
					return IntPtr.Zero;
				}

				return NativeMethods.BNNewTypeReference(type.DangerousGetHandle());
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in Platform.GetGlobalRegisterType: {0}", exception
				);
				return IntPtr.Zero;
			}
		}

		private ulong InvokeGetAddressSize(IntPtr context)
		{
			try
			{
				return this.AddressSize;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in Platform.AddressSize: {0}", exception
				);
				return 0;
			}
		}

		private bool InvokeGetFallbackEnabled(IntPtr context)
		{
			try
			{
				return this.GetFallbackEnabled();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in Platform.GetFallbackEnabled: {0}", exception
				);
				return true;
			}
		}

		private uint[] GetDefaultGlobalRegisters()
		{
			Architecture architecture = this.Architecture;
			IntPtr result = NativeMethods.BNGetArchitectureGlobalRegisters(
				architecture.DangerousGetHandle(), out ulong count
			);

			return UnsafeUtils.TakeNumberArray<uint>(
				result, count, NativeMethods.BNFreeRegisterList
			);
		}
	}
}
