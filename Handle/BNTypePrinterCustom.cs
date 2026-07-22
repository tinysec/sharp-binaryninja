using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class TypePrinter
	{
		private static readonly object registrationLock = new object();

		private static readonly List<TypePrinter> registeredPrinters =
			new List<TypePrinter>();

		private bool isRegistered;

		private NativeDelegates.BNTypePrinterGetTypeTokens? getTypeTokensCallback;
		private NativeDelegates.BNTypePrinterGetTypeTokensWithParent?
			getTypeTokensBeforeNameCallback;
		private NativeDelegates.BNTypePrinterGetTypeTokensWithParent?
			getTypeTokensAfterNameCallback;
		private NativeDelegates.BNTypePrinterGetTypeString? getTypeStringCallback;
		private NativeDelegates.BNTypePrinterGetTypeStringPart?
			getTypeStringBeforeNameCallback;
		private NativeDelegates.BNTypePrinterGetTypeStringPart?
			getTypeStringAfterNameCallback;
		private NativeDelegates.BNTypePrinterGetTypeLines? getTypeLinesCallback;
		private NativeDelegates.BNTypePrinterPrintAllTypes? printAllTypesCallback;
		private NativeDelegates.BNTypePrinterFreeList? freeTokensCallback;
		private NativeDelegates.BNTypePrinterFreeString? freeStringCallback;
		private NativeDelegates.BNTypePrinterFreeList? freeLinesCallback;

		/// <summary>Registers this printer and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException("The type printer is already registered.");
			}

			this.getTypeTokensCallback = new NativeDelegates.BNTypePrinterGetTypeTokens(
				this.InvokeGetTypeTokens
			);
			this.getTypeTokensBeforeNameCallback =
				new NativeDelegates.BNTypePrinterGetTypeTokensWithParent(
					this.InvokeGetTypeTokensBeforeName
				);
			this.getTypeTokensAfterNameCallback =
				new NativeDelegates.BNTypePrinterGetTypeTokensWithParent(
					this.InvokeGetTypeTokensAfterName
				);
			this.getTypeStringCallback = new NativeDelegates.BNTypePrinterGetTypeString(
				this.InvokeGetTypeString
			);
			this.getTypeStringBeforeNameCallback =
				new NativeDelegates.BNTypePrinterGetTypeStringPart(
					this.InvokeGetTypeStringBeforeName
				);
			this.getTypeStringAfterNameCallback =
				new NativeDelegates.BNTypePrinterGetTypeStringPart(
					this.InvokeGetTypeStringAfterName
				);
			this.getTypeLinesCallback = new NativeDelegates.BNTypePrinterGetTypeLines(
				this.InvokeGetTypeLines
			);
			this.printAllTypesCallback = new NativeDelegates.BNTypePrinterPrintAllTypes(
				this.InvokePrintAllTypes
			);
			this.freeTokensCallback = new NativeDelegates.BNTypePrinterFreeList(
				this.InvokeFreeTokens
			);
			this.freeStringCallback = new NativeDelegates.BNTypePrinterFreeString(
				this.InvokeFreeString
			);
			this.freeLinesCallback = new NativeDelegates.BNTypePrinterFreeList(
				this.InvokeFreeLines
			);

			BNTypePrinterCallbacks callbacks = new BNTypePrinterCallbacks();
			callbacks.context = IntPtr.Zero;
			callbacks.getTypeTokens = Marshal.GetFunctionPointerForDelegate(
				this.getTypeTokensCallback
			);
			callbacks.getTypeTokensBeforeName = Marshal.GetFunctionPointerForDelegate(
				this.getTypeTokensBeforeNameCallback
			);
			callbacks.getTypeTokensAfterName = Marshal.GetFunctionPointerForDelegate(
				this.getTypeTokensAfterNameCallback
			);
			callbacks.getTypeString = Marshal.GetFunctionPointerForDelegate(
				this.getTypeStringCallback
			);
			callbacks.getTypeStringBeforeName = Marshal.GetFunctionPointerForDelegate(
				this.getTypeStringBeforeNameCallback
			);
			callbacks.getTypeStringAfterName = Marshal.GetFunctionPointerForDelegate(
				this.getTypeStringAfterNameCallback
			);
			callbacks.getTypeLines = Marshal.GetFunctionPointerForDelegate(
				this.getTypeLinesCallback
			);
			callbacks.printAllTypes = Marshal.GetFunctionPointerForDelegate(
				this.printAllTypesCallback
			);
			callbacks.freeTokens = Marshal.GetFunctionPointerForDelegate(
				this.freeTokensCallback
			);
			callbacks.freeString = Marshal.GetFunctionPointerForDelegate(
				this.freeStringCallback
			);
			callbacks.freeLines = Marshal.GetFunctionPointerForDelegate(
				this.freeLinesCallback
			);

			IntPtr handle = NativeMethods.BNRegisterTypePrinter(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException("The core rejected the type printer.");
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (TypePrinter.registrationLock)
			{
				TypePrinter.registeredPrinters.Add(this);
			}
		}
	}
}
