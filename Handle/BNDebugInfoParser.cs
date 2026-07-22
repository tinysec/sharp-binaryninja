using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNDebugInfoParserIsValid(
			IntPtr context,
			IntPtr view
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNDebugInfoParserParse(
			IntPtr context,
			IntPtr debugInfo,
			IntPtr view,
			IntPtr debugFile,
			IntPtr progress,
			IntPtr progressContext
		);
	}

	/// <summary>Parses ground-truth type, function, and variable information.</summary>
	public abstract class DebugInfoParser : AbstractSafeHandle<DebugInfoParser>
	{
		private static readonly object registrationLock = new object();

		private static readonly List<DebugInfoParser> registeredParsers =
			new List<DebugInfoParser>();

		private readonly string? registrationName;

		private bool isRegistered;

		private NativeDelegates.BNDebugInfoParserIsValid? isValidCallback;

		private NativeDelegates.BNDebugInfoParserParse? parseCallback;

		/// <summary>Creates an unregistered custom debug info parser.</summary>
		protected DebugInfoParser(string name)
			: base(true)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private DebugInfoParser(IntPtr handle)
			: base(handle, true)
		{
		}

		internal static DebugInfoParser? NewFromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreDebugInfoParser(
				NativeMethods.BNNewDebugInfoParserReference(handle)
			);
		}

		internal static DebugInfoParser MustNewFromHandle(IntPtr handle)
		{
			DebugInfoParser? parser = DebugInfoParser.NewFromHandle(handle);
			if (null == parser)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return parser;
		}

		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				NativeMethods.BNFreeDebugInfoParserReference(this.handle);
				this.SetHandleAsInvalid();
			}

			return true;
		}

		/// <summary>Registers this parser and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException(
					"The debug info parser is already registered."
				);
			}

			this.isValidCallback = new NativeDelegates.BNDebugInfoParserIsValid(
				this.InvokeIsValid
			);
			this.parseCallback = new NativeDelegates.BNDebugInfoParserParse(
				this.InvokeParse
			);
			IntPtr parser = NativeMethods.BNRegisterDebugInfoParser(
				this.registrationName ?? string.Empty,
				Marshal.GetFunctionPointerForDelegate(this.isValidCallback),
				Marshal.GetFunctionPointerForDelegate(this.parseCallback),
				IntPtr.Zero
			);
			if (IntPtr.Zero == parser)
			{
				throw new InvalidOperationException(
					"The core rejected the debug info parser."
				);
			}

			IntPtr parserReference = NativeMethods.BNNewDebugInfoParserReference(parser);
			if (IntPtr.Zero == parserReference)
			{
				throw new InvalidOperationException(
					"The core could not retain the debug info parser."
				);
			}

			this.SetHandle(parserReference);
			this.isRegistered = true;
			lock (DebugInfoParser.registrationLock)
			{
				DebugInfoParser.registeredParsers.Add(this);
			}
		}

		/// <summary>Looks up a registered debug info parser by name.</summary>
		public static DebugInfoParser? GetByName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return DebugInfoParser.NewFromHandle(
				NativeMethods.BNGetDebugInfoParserByName(name)
			);
		}

		/// <summary>Gets every parser registered with the core.</summary>
		public static unsafe DebugInfoParser[] GetParsers()
		{
			ulong count = 0;
			IntPtr parsers = NativeMethods.BNGetDebugInfoParsers((IntPtr)(&count));
			return UnsafeUtils.TakeHandleArrayEx<DebugInfoParser>(
				parsers,
				count,
				DebugInfoParser.MustNewFromHandle,
				NativeMethods.BNFreeDebugInfoParserList
			);
		}

		/// <summary>Gets the registered parser name.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetDebugInfoParserName(this.handle)
				);
			}
		}

		/// <summary>Determines whether this parser accepts a binary view.</summary>
		public abstract bool IsValidForView(BinaryView view);

		/// <summary>Populates a debug info object from the supplied views.</summary>
		public abstract bool ParseInfo(
			DebugInfo debugInfo,
			BinaryView view,
			BinaryView? debugFile,
			ProgressDelegate progress
		);

		/// <summary>Runs this parser through the core.</summary>
		public DebugInfo? Parse(
			BinaryView view,
			BinaryView? debugFile = null,
			DebugInfo? existing = null,
			ProgressDelegate? progress = null
		)
		{
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			ProgressCallbackContext callbackContext =
				new ProgressCallbackContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = callbackContext.Invoke;
			IntPtr result;
			try
			{
				result = NativeMethods.BNParseDebugInfo(
					this.handle,
					view.DangerousGetHandle(),
					null == debugFile
						? IntPtr.Zero
						: debugFile.DangerousGetHandle(),
					null == existing ? IntPtr.Zero : existing.DangerousGetHandle(),
					Marshal.GetFunctionPointerForDelegate(nativeProgress),
					IntPtr.Zero
				);
			}
			finally
			{
				GC.KeepAlive(nativeProgress);
			}

			callbackContext.ThrowIfFailed();
			return null == existing
				? DebugInfo.TakeHandle(result)
				: DebugInfo.NewFromHandle(result);
		}

		private bool InvokeIsValid(IntPtr context, IntPtr view)
		{
			try
			{
				using (BinaryView managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)
				))
				{
					return this.IsValidForView(managedView);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in DebugInfoParser.IsValidForView: {0}",
					exception
				);
				return false;
			}
		}

		private bool InvokeParse(
			IntPtr context,
			IntPtr debugInfo,
			IntPtr view,
			IntPtr debugFile,
			IntPtr progress,
			IntPtr progressContext
		)
		{
			BinaryView? managedDebugFile = null;
			try
			{
				using (DebugInfo managedDebugInfo =
					DebugInfo.MustNewFromHandle(debugInfo))
				using (BinaryView managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)
				))
				{
					if (IntPtr.Zero != debugFile)
					{
						managedDebugFile = BinaryView.MustTakeHandle(
							NativeMethods.BNNewViewReference(debugFile)
						);
					}

					ProgressForwarder forwarder = new ProgressForwarder(
						progress,
						progressContext
					);
					return this.ParseInfo(
						managedDebugInfo,
						managedView,
						managedDebugFile,
						forwarder.Invoke
					);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in DebugInfoParser.ParseInfo: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedDebugFile)
				{
					managedDebugFile.Dispose();
				}
			}
		}

		private sealed class CoreDebugInfoParser : DebugInfoParser
		{
			internal CoreDebugInfoParser(IntPtr handle)
				: base(handle)
			{
			}

			public override bool IsValidForView(BinaryView view)
			{
				if (null == view)
				{
					throw new ArgumentNullException(nameof(view));
				}

				return NativeMethods.BNIsDebugInfoParserValidForView(
					this.handle,
					view.DangerousGetHandle()
				);
			}

			public override bool ParseInfo(
				DebugInfo debugInfo,
				BinaryView view,
				BinaryView? debugFile,
				ProgressDelegate progress
			)
			{
				throw new NotSupportedException(
					"Core parsers are invoked with Parse, not ParseInfo."
				);
			}
		}

		private sealed class ProgressForwarder
		{
			private readonly NativeDelegates.BNProgressFunction? progress;

			private readonly IntPtr context;

			internal ProgressForwarder(IntPtr progress, IntPtr context)
			{
				this.context = context;
				if (IntPtr.Zero != progress)
				{
					this.progress = Marshal.GetDelegateForFunctionPointer<
						NativeDelegates.BNProgressFunction
					>(progress);
				}
			}

			internal bool Invoke(ulong current, ulong total)
			{
				return null == this.progress
					|| this.progress(this.context, current, total);
			}
		}

		private sealed class ProgressCallbackContext
		{
			private readonly ProgressDelegate? progress;

			private ExceptionDispatchInfo? exception;

			internal ProgressCallbackContext(ProgressDelegate? progress)
			{
				this.progress = progress;
			}

			internal bool Invoke(IntPtr context, ulong current, ulong total)
			{
				if (null == this.progress)
				{
					return true;
				}

				try
				{
					return this.progress(current, total);
				}
				catch (Exception caught)
				{
					this.exception = ExceptionDispatchInfo.Capture(caught);
					return false;
				}
			}

			internal void ThrowIfFailed()
			{
				if (null != this.exception)
				{
					this.exception.Throw();
				}
			}
		}
	}
}
