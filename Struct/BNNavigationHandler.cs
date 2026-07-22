using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNNavigationHandler
	{
		/// <summary>
		/// void* context
		/// </summary>
		public IntPtr context;

		/// <summary>
		/// char* (*getCurrentView)(void* ctxt)
		/// </summary>
		public IntPtr getCurrentView;

		/// <summary>
		/// uint64_t (*getCurrentOffset)(void* ctxt)
		/// </summary>
		public IntPtr getCurrentOffset;

		/// <summary>
		/// bool (*navigate)(void* ctxt, const char* view, uint64_t offset)
		/// </summary>
		public IntPtr navigate;
	}

	/// <summary>
	/// Supplies the current view and offset for a file and handles navigation requests.
	/// </summary>
	public abstract class NavigationHandler
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntPtr GetCurrentViewCallback(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate ulong GetCurrentOffsetCallback(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool NavigateCallback(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string view,
			ulong offset);

		private readonly GetCurrentViewCallback getCurrentViewCallback;
		private readonly GetCurrentOffsetCallback getCurrentOffsetCallback;
		private readonly NavigateCallback navigateCallback;
		private readonly IntPtr callbacks;

		protected NavigationHandler()
		{
			this.getCurrentViewCallback = this.GetCurrentViewAdapter;
			this.getCurrentOffsetCallback = this.GetCurrentOffsetAdapter;
			this.navigateCallback = this.NavigateAdapter;

			BNNavigationHandler nativeCallbacks = new BNNavigationHandler
			{
				context = IntPtr.Zero,
				getCurrentView = Marshal.GetFunctionPointerForDelegate<GetCurrentViewCallback>(
					this.getCurrentViewCallback),
				getCurrentOffset = Marshal.GetFunctionPointerForDelegate<GetCurrentOffsetCallback>(
					this.getCurrentOffsetCallback),
				navigate = Marshal.GetFunctionPointerForDelegate<NavigateCallback>(this.navigateCallback)
			};

			this.callbacks = Marshal.AllocHGlobal(Marshal.SizeOf<BNNavigationHandler>());
			Marshal.StructureToPtr(nativeCallbacks, this.callbacks, false);
		}

		~NavigationHandler()
		{
			Marshal.FreeHGlobal(this.callbacks);
		}

		internal IntPtr Callbacks
		{
			get
			{
				return this.callbacks;
			}
		}

		/// <summary>
		/// Gets the name of the currently displayed binary view.
		/// </summary>
		public abstract string GetCurrentView();

		/// <summary>
		/// Gets the currently displayed address.
		/// </summary>
		public abstract ulong GetCurrentOffset();

		/// <summary>
		/// Navigates to an address in a binary view.
		/// </summary>
		public abstract bool Navigate(string view, ulong offset);

		private IntPtr GetCurrentViewAdapter(IntPtr context)
		{
			try
			{
				return NativeMethods.BNAllocString(this.GetCurrentView() ?? string.Empty);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in NavigationHandler.GetCurrentView: {0}", exception);
				return NativeMethods.BNAllocString(string.Empty);
			}
		}

		private ulong GetCurrentOffsetAdapter(IntPtr context)
		{
			try
			{
				return this.GetCurrentOffset();
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in NavigationHandler.GetCurrentOffset: {0}", exception);
				return 0;
			}
		}

		private bool NavigateAdapter(IntPtr context, string view, ulong offset)
		{
			try
			{
				return this.Navigate(view, offset);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in NavigationHandler.Navigate: {0}", exception);
				return false;
			}
		}
	}
}
