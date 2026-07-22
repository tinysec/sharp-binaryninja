using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>Formats disassembly text lines for high-level representations.</summary>
	public abstract class LineFormatter : AbstractSafeHandle<LineFormatter>
	{
		private static readonly object registrationLock = new object();

		private static readonly List<LineFormatter> registeredFormatters =
			new List<LineFormatter>();

		private readonly object outputLock = new object();

		private readonly Dictionary<IntPtr, ScopedAllocator> pendingOutputs =
			new Dictionary<IntPtr, ScopedAllocator>();

		private readonly string? registrationName;

		private bool isRegistered;

		private NativeDelegates.BNLineFormatterFormatLines? formatLinesCallback;

		private NativeDelegates.BNLineFormatterFreeLines? freeLinesCallback;

		/// <summary>Creates an unregistered custom line formatter.</summary>
		protected LineFormatter(string name)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private LineFormatter(IntPtr handle)
			: base(handle, false)
		{
		}

		/// <summary>Gets the registered formatter name.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetLineFormatterName(this.handle)
				);
			}
		}

		/// <summary>Registers this formatter and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException(
					"The line formatter is already registered."
				);
			}

			this.formatLinesCallback =
				new NativeDelegates.BNLineFormatterFormatLines(
					this.InvokeFormatLines
				);
			this.freeLinesCallback = new NativeDelegates.BNLineFormatterFreeLines(
				this.InvokeFreeLines
			);
			BNCustomLineFormatter callbacks = new BNCustomLineFormatter();
			callbacks.context = IntPtr.Zero;
			callbacks.formatLines = Marshal.GetFunctionPointerForDelegate(
				this.formatLinesCallback
			);
			callbacks.freeLines = Marshal.GetFunctionPointerForDelegate(
				this.freeLinesCallback
			);

			IntPtr handle = NativeMethods.BNRegisterLineFormatter(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException(
					"The core rejected the line formatter."
				);
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (LineFormatter.registrationLock)
			{
				LineFormatter.registeredFormatters.Add(this);
			}
		}

		/// <summary>Looks up a registered formatter by name.</summary>
		public static LineFormatter? GetByName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return LineFormatter.FromHandle(
				NativeMethods.BNGetLineFormatterByName(name)
			);
		}

		/// <summary>Gets the default formatter.</summary>
		public static LineFormatter? GetDefault()
		{
			return LineFormatter.FromHandle(NativeMethods.BNGetDefaultLineFormatter());
		}

		/// <summary>Gets every formatter registered with the core.</summary>
		public static unsafe LineFormatter[] GetList()
		{
			ulong count = 0;
			IntPtr formatters = NativeMethods.BNGetLineFormatterList((IntPtr)(&count));
			return UnsafeUtils.TakeHandleArray<LineFormatter>(
				formatters,
				count,
				LineFormatter.MustFromHandle,
				NativeMethods.BNFreeLineFormatterList
			);
		}

		/// <summary>Gets the default settings for a disassembly context.</summary>
		public static LineFormatterSettings GetDefaultSettings(
			HighLevelILFunction function,
			DisassemblySettings? settings = null
		)
		{
			if (null == function)
			{
				throw new ArgumentNullException(nameof(function));
			}

			IntPtr result = NativeMethods.BNGetDefaultLineFormatterSettings(
				null == settings ? IntPtr.Zero : settings.DangerousGetHandle(),
				function.DangerousGetHandle()
			);
			if (IntPtr.Zero == result)
			{
				return new LineFormatterSettings();
			}

			try
			{
				return LineFormatter.ReadSettings(
					Marshal.PtrToStructure<BNLineFormatterSettings>(result)
				);
			}
			finally
			{
				NativeMethods.BNFreeLineFormatterSettings(result);
			}
		}

		/// <summary>Formats input lines and returns a replacement sequence.</summary>
		public abstract DisassemblyTextLine[] FormatLines(
			DisassemblyTextLine[] lines,
			LineFormatterSettings settings
		);

		private static LineFormatter? FromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreLineFormatter(handle);
		}

		internal static LineFormatter? BorrowHandle(IntPtr handle)
		{
			return LineFormatter.FromHandle(handle);
		}

		private static LineFormatter MustFromHandle(IntPtr handle)
		{
			LineFormatter? formatter = LineFormatter.FromHandle(handle);
			if (null == formatter)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return formatter;
		}

		private IntPtr InvokeFormatLines(
			IntPtr context,
			IntPtr inputLines,
			ulong inputCount,
			IntPtr settings,
			IntPtr outputCount
		)
		{
			Marshal.WriteInt64(outputCount, 0);
			DisassemblyTextLine[] input =
				UnsafeUtils.ReadStructArray<BNDisassemblyTextLine, DisassemblyTextLine>(
					inputLines,
					inputCount,
					DisassemblyTextLine.FromNative
				);
			LineFormatterSettings? managedSettings = null;
			try
			{
				managedSettings = LineFormatter.ReadSettings(
					Marshal.PtrToStructure<BNLineFormatterSettings>(settings)
				);
				DisassemblyTextLine[] output = this.FormatLines(
					input,
					managedSettings
				);
				if (null == output || 0 == output.Length)
				{
					return IntPtr.Zero;
				}

				ScopedAllocator allocator = new ScopedAllocator();
				try
				{
					BNDisassemblyTextLine[] nativeOutput =
						new BNDisassemblyTextLine[output.Length];
					for (int i = 0; i < output.Length; i++)
					{
						nativeOutput[i] = output[i].ToNativeEx(allocator);
					}

					IntPtr result = allocator.AllocStructArray(nativeOutput);
					lock (this.outputLock)
					{
						this.pendingOutputs.Add(result, allocator);
					}

					Marshal.WriteInt64(outputCount, output.Length);
					return result;
				}
				catch
				{
					allocator.Dispose();
					throw;
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in LineFormatter.FormatLines: {0}",
					exception
				);
				return IntPtr.Zero;
			}
			finally
			{
				LineFormatter.DisposeLines(input);
				if (null != managedSettings && null != managedSettings.HighLevelIL)
				{
					managedSettings.HighLevelIL.Dispose();
				}
			}
		}

		private void InvokeFreeLines(IntPtr context, IntPtr lines, ulong count)
		{
			if (IntPtr.Zero == lines)
			{
				return;
			}

			ScopedAllocator? allocator = null;
			lock (this.outputLock)
			{
				if (this.pendingOutputs.TryGetValue(lines, out allocator))
				{
					this.pendingOutputs.Remove(lines);
				}
			}

			if (null == allocator)
			{
				Core.LogError("LineFormatter received an unknown output allocation.");
				return;
			}

			allocator.Dispose();
		}

		private static LineFormatterSettings ReadSettings(BNLineFormatterSettings native)
		{
			LineFormatterSettings settings = new LineFormatterSettings();
			settings.HighLevelIL = HighLevelILFunction.NewFromHandle(native.highLevelIL);
			settings.DesiredLineLength = native.desiredLineLength;
			settings.MinimumContentLength = native.minimumContentLength;
			settings.TabWidth = native.tabWidth;
			settings.MaximumAnnotationLength = native.maximumAnnotationLength;
			settings.StringWrappingWidth = native.stringWrappingWidth;
			settings.LanguageName = UnsafeUtils.ReadUtf8String(native.languageName);
			settings.CommentStartString = UnsafeUtils.ReadUtf8String(
				native.commentStartString
			);
			settings.CommentEndString = UnsafeUtils.ReadUtf8String(
				native.commentEndString
			);
			settings.AnnotationStartString = UnsafeUtils.ReadUtf8String(
				native.annotationStartString
			);
			settings.AnnotationEndString = UnsafeUtils.ReadUtf8String(
				native.annotationEndString
			);
			return settings;
		}

		private static void DisposeLines(DisassemblyTextLine[] lines)
		{
			foreach (DisassemblyTextLine line in lines)
			{
				foreach (Tag tag in line.Tags)
				{
					tag.Dispose();
				}
			}
		}

		private sealed class CoreLineFormatter : LineFormatter
		{
			internal CoreLineFormatter(IntPtr handle)
				: base(handle)
			{
			}

			public override unsafe DisassemblyTextLine[] FormatLines(
				DisassemblyTextLine[] lines,
				LineFormatterSettings settings
			)
			{
				if (null == lines)
				{
					throw new ArgumentNullException(nameof(lines));
				}

				if (null == settings)
				{
					throw new ArgumentNullException(nameof(settings));
				}

				if (null == settings.HighLevelIL)
				{
					throw new ArgumentException(
						"Line formatter settings require a high-level IL function.",
						nameof(settings)
					);
				}

				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					BNDisassemblyTextLine[] nativeLines =
						new BNDisassemblyTextLine[lines.Length];
					for (int i = 0; i < lines.Length; i++)
					{
						nativeLines[i] = lines[i].ToNativeEx(allocator);
					}

					BNLineFormatterSettings nativeSettings =
						LineFormatter.WriteSettings(settings, allocator);
					ulong outputCount = 0;
					IntPtr output = NativeMethods.BNFormatLines(
						this.handle,
						allocator.AllocStructArray(nativeLines),
						(ulong)nativeLines.Length,
						allocator.AllocStruct(nativeSettings),
						(IntPtr)(&outputCount)
					);
					return UnsafeUtils.TakeStructArrayEx<
						BNDisassemblyTextLine,
						DisassemblyTextLine
					>(
						output,
						outputCount,
						DisassemblyTextLine.FromNative,
						NativeMethods.BNFreeDisassemblyTextLines
					);
				}
			}
		}

		private static BNLineFormatterSettings WriteSettings(
			LineFormatterSettings settings,
			ScopedAllocator allocator
		)
		{
			BNLineFormatterSettings native = new BNLineFormatterSettings();
			native.highLevelIL = null == settings.HighLevelIL
				? IntPtr.Zero
				: settings.HighLevelIL.DangerousGetHandle();
			native.desiredLineLength = settings.DesiredLineLength;
			native.minimumContentLength = settings.MinimumContentLength;
			native.tabWidth = settings.TabWidth;
			native.maximumAnnotationLength = settings.MaximumAnnotationLength;
			native.stringWrappingWidth = settings.StringWrappingWidth;
			native.languageName = allocator.AllocUtf8String(settings.LanguageName);
			native.commentStartString = allocator.AllocUtf8String(
				settings.CommentStartString
			);
			native.commentEndString = allocator.AllocUtf8String(
				settings.CommentEndString
			);
			native.annotationStartString = allocator.AllocUtf8String(
				settings.AnnotationStartString
			);
			native.annotationEndString = allocator.AllocUtf8String(
				settings.AnnotationEndString
			);
			return native;
		}
	}
}
