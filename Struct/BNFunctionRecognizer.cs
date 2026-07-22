using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNRecognizeLowLevelIL(
			IntPtr context,
			IntPtr view,
			IntPtr function,
			IntPtr il
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNRecognizeMediumLevelIL(
			IntPtr context,
			IntPtr view,
			IntPtr function,
			IntPtr il
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNFunctionRecognizer
	{
		internal IntPtr context;

		internal IntPtr recognizeLowLevelIL;

		internal IntPtr recognizeMediumLevelIL;
	}

	/// <summary>Recognizes known functions from their low- and medium-level IL.</summary>
	public class FunctionRecognizer
	{
		private static readonly object registrationLock = new object();

		private static readonly List<FunctionRecognizer> registeredRecognizers =
			new List<FunctionRecognizer>();

		private readonly NativeDelegates.BNRecognizeLowLevelIL lowLevelCallback;

		private readonly NativeDelegates.BNRecognizeMediumLevelIL mediumLevelCallback;

		private bool isGloballyRegistered;

		/// <summary>Creates an unregistered function recognizer.</summary>
		public FunctionRecognizer()
		{
			this.lowLevelCallback = new NativeDelegates.BNRecognizeLowLevelIL(
				this.InvokeRecognizeLowLevelIL
			);
			this.mediumLevelCallback = new NativeDelegates.BNRecognizeMediumLevelIL(
				this.InvokeRecognizeMediumLevelIL
			);
		}

		/// <summary>Registers this recognizer for every architecture.</summary>
		public void RegisterGlobal()
		{
			if (this.isGloballyRegistered)
			{
				throw new InvalidOperationException(
					"The function recognizer is already registered globally."
				);
			}

			BNFunctionRecognizer callbacks = this.CreateCallbacks();
			NativeMethods.BNRegisterGlobalFunctionRecognizer(in callbacks);
			this.isGloballyRegistered = true;
			this.RootForRegistration();
		}

		/// <summary>Registers this recognizer for a specific architecture.</summary>
		public void RegisterArchitecture(Architecture architecture)
		{
			if (null == architecture)
			{
				throw new ArgumentNullException(nameof(architecture));
			}

			BNFunctionRecognizer callbacks = this.CreateCallbacks();
			NativeMethods.BNRegisterArchitectureFunctionRecognizer(
				architecture.DangerousGetHandle(),
				in callbacks
			);
			this.RootForRegistration();
		}

		/// <summary>Attempts recognition from low-level IL.</summary>
		public virtual bool RecognizeLowLevelIL(
			BinaryView view,
			Function function,
			LowLevelILFunction il
		)
		{
			return false;
		}

		/// <summary>Attempts recognition from medium-level IL.</summary>
		public virtual bool RecognizeMediumLevelIL(
			BinaryView view,
			Function function,
			MediumLevelILFunction il
		)
		{
			return false;
		}

		private BNFunctionRecognizer CreateCallbacks()
		{
			BNFunctionRecognizer callbacks = new BNFunctionRecognizer();
			callbacks.context = IntPtr.Zero;
			callbacks.recognizeLowLevelIL = Marshal.GetFunctionPointerForDelegate(
				this.lowLevelCallback
			);
			callbacks.recognizeMediumLevelIL = Marshal.GetFunctionPointerForDelegate(
				this.mediumLevelCallback
			);
			return callbacks;
		}

		private void RootForRegistration()
		{
			lock (FunctionRecognizer.registrationLock)
			{
				if (!FunctionRecognizer.registeredRecognizers.Contains(this))
				{
					FunctionRecognizer.registeredRecognizers.Add(this);
				}
			}
		}

		private bool InvokeRecognizeLowLevelIL(
			IntPtr context,
			IntPtr view,
			IntPtr function,
			IntPtr il
		)
		{
			try
			{
				using (BinaryView managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)
				))
				using (Function managedFunction = Function.MustNewFromHandle(function))
				using (LowLevelILFunction managedIL =
					LowLevelILFunction.MustNewFromHandle(il))
				{
					return this.RecognizeLowLevelIL(
						managedView,
						managedFunction,
						managedIL
					);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in FunctionRecognizer.RecognizeLowLevelIL: {0}",
					exception
				);
				return false;
			}
		}

		private bool InvokeRecognizeMediumLevelIL(
			IntPtr context,
			IntPtr view,
			IntPtr function,
			IntPtr il
		)
		{
			try
			{
				using (BinaryView managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)
				))
				using (Function managedFunction = Function.MustNewFromHandle(function))
				using (MediumLevelILFunction managedIL =
					MediumLevelILFunction.MustNewFromHandle(il))
				{
					return this.RecognizeMediumLevelIL(
						managedView,
						managedFunction,
						managedIL
					);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in FunctionRecognizer.RecognizeMediumLevelIL: {0}",
					exception
				);
				return false;
			}
		}
	}
}
