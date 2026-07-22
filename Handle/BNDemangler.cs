using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Translates compiler-generated names into qualified names and optional types.
	/// </summary>
	public abstract class Demangler : AbstractSafeHandle<Demangler>
	{
		private static readonly object registrationLock = new object();

		private static readonly List<Demangler> registeredDemanglers =
			new List<Demangler>();

		private readonly string? registrationName;

		private bool isRegistered;

		private NativeDelegates.BNDemanglerIsMangledString? isMangledStringCallback;

		private NativeDelegates.BNDemanglerDemangle? demangleCallback;

		private NativeDelegates.BNDemanglerFreeVarName? freeVarNameCallback;

		/// <summary>Creates an unregistered custom demangler.</summary>
		protected Demangler(string name)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private Demangler(IntPtr handle)
			: base(handle, false)
		{
		}

		/// <summary>Gets the unique name registered with the core.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetDemanglerName(this.handle)
				);
			}
		}

		/// <summary>Registers this demangler and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException("The demangler is already registered.");
			}

			this.isMangledStringCallback =
				new NativeDelegates.BNDemanglerIsMangledString(
					this.InvokeIsMangledString
				);
			this.demangleCallback = new NativeDelegates.BNDemanglerDemangle(
				this.InvokeDemangle
			);
			this.freeVarNameCallback = new NativeDelegates.BNDemanglerFreeVarName(
				this.InvokeFreeVarName
			);

			BNDemanglerCallbacks callbacks = new BNDemanglerCallbacks();
			callbacks.context = IntPtr.Zero;
			callbacks.isMangledString = Marshal.GetFunctionPointerForDelegate(
				this.isMangledStringCallback
			);
			callbacks.demangle = Marshal.GetFunctionPointerForDelegate(
				this.demangleCallback
			);
			callbacks.freeVarName = Marshal.GetFunctionPointerForDelegate(
				this.freeVarNameCallback
			);

			IntPtr handle = NativeMethods.BNRegisterDemangler(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException("The core rejected the demangler.");
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (Demangler.registrationLock)
			{
				Demangler.registeredDemanglers.Add(this);
			}
		}

		/// <summary>Looks up a registered demangler by name.</summary>
		public static Demangler? GetByName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return Demangler.FromHandle(NativeMethods.BNGetDemanglerByName(name));
		}

		/// <summary>Gets every demangler registered with the core.</summary>
		public static unsafe Demangler[] GetList()
		{
			ulong count = 0;
			IntPtr demanglers = NativeMethods.BNGetDemanglerList((IntPtr)(&count));
			return UnsafeUtils.TakeHandleArray<Demangler>(
				demanglers,
				count,
				Demangler.MustFromHandle,
				NativeMethods.BNFreeDemanglerList
			);
		}

		/// <summary>Promotes a demangler to the highest-priority registry position.</summary>
		public static void Promote(Demangler demangler)
		{
			if (null == demangler)
			{
				throw new ArgumentNullException(nameof(demangler));
			}

			NativeMethods.BNPromoteDemangler(demangler.DangerousGetHandle());
		}

		/// <summary>Determines whether this demangler may handle a name.</summary>
		public abstract bool IsMangledString(string name);

		/// <summary>
		/// Attempts to produce a qualified name and optional type for a mangled name.
		/// </summary>
		public abstract bool Demangle(
			Architecture arch,
			string name,
			BinaryView? view,
			out BinaryNinja.Type? outType,
			out QualifiedName outVarName
		);

		private static Demangler? FromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreDemangler(handle);
		}

		private static Demangler MustFromHandle(IntPtr handle)
		{
			Demangler? demangler = Demangler.FromHandle(handle);
			if (null == demangler)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return demangler;
		}

		private bool InvokeIsMangledString(IntPtr context, string name)
		{
			try
			{
				return this.IsMangledString(name);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in Demangler.IsMangledString: {0}", exception);
				return false;
			}
		}

		private bool InvokeDemangle(
			IntPtr context,
			IntPtr architecture,
			string name,
			IntPtr outType,
			IntPtr outVarName,
			IntPtr view
		)
		{
			IntPtr typeReference = IntPtr.Zero;
			BNQualifiedName nativeName = new BNQualifiedName();
			bool nameAllocated = false;
			BinaryView? managedView = null;

			Marshal.WriteIntPtr(outType, IntPtr.Zero);
			Marshal.StructureToPtr(new BNQualifiedName(), outVarName, false);

			try
			{
				Architecture managedArchitecture = Architecture.MustFromHandle(architecture);
				if (IntPtr.Zero != view)
				{
					managedView = BinaryView.MustTakeHandle(
						NativeMethods.BNNewViewReference(view)
					);
				}

				BinaryNinja.Type? managedType;
				QualifiedName managedName;
				bool success = this.Demangle(
					managedArchitecture,
					name,
					managedView,
					out managedType,
					out managedName
				);
				if (!success || null == managedName)
				{
					return false;
				}

				nativeName = Demangler.AllocateQualifiedName(managedName);
				nameAllocated = true;
				Marshal.StructureToPtr(nativeName, outVarName, false);

				if (null != managedType)
				{
					typeReference = NativeMethods.BNNewTypeReference(
						managedType.DangerousGetHandle()
					);
					Marshal.WriteIntPtr(outType, typeReference);
				}

				return true;
			}
			catch (Exception exception)
			{
				if (IntPtr.Zero != typeReference)
				{
					NativeMethods.BNFreeType(typeReference);
					Marshal.WriteIntPtr(outType, IntPtr.Zero);
				}

				if (nameAllocated)
				{
					Demangler.FreeQualifiedName(nativeName);
					Marshal.StructureToPtr(new BNQualifiedName(), outVarName, false);
				}

				Core.LogError("Unhandled exception in Demangler.Demangle: {0}", exception);
				return false;
			}
			finally
			{
				if (null != managedView)
				{
					managedView.Dispose();
				}
			}
		}

		private void InvokeFreeVarName(IntPtr context, IntPtr name)
		{
			try
			{
				BNQualifiedName nativeName = Marshal.PtrToStructure<BNQualifiedName>(name);
				Demangler.FreeQualifiedName(nativeName);
				Marshal.StructureToPtr(new BNQualifiedName(), name, false);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in Demangler.FreeVarName: {0}", exception);
			}
		}

		private static BNQualifiedName AllocateQualifiedName(QualifiedName name)
		{
			string[] components = name.Name ?? Array.Empty<string>();
			BNQualifiedName result = new BNQualifiedName();
			try
			{
				result.nameCount = (ulong)components.Length;
				if (0 != components.Length)
				{
					result.name = NativeMethods.BNAllocStringList(
						components,
						(ulong)components.Length
					);
				}

				result.join = NativeMethods.BNAllocString(name.Join ?? string.Empty);
				return result;
			}
			catch
			{
				Demangler.FreeQualifiedName(result);
				throw;
			}
		}

		private static void FreeQualifiedName(BNQualifiedName name)
		{
			if (IntPtr.Zero != name.name)
			{
				NativeMethods.BNFreeStringList(name.name, name.nameCount);
			}

			if (IntPtr.Zero != name.join)
			{
				NativeMethods.BNFreeString(name.join);
			}
		}

		private sealed class CoreDemangler : Demangler
		{
			internal CoreDemangler(IntPtr handle)
				: base(handle)
			{
			}

			public override bool IsMangledString(string name)
			{
				return NativeMethods.BNIsDemanglerMangledName(
					this.handle,
					name ?? string.Empty
				);
			}

			public override unsafe bool Demangle(
				Architecture arch,
				string name,
				BinaryView? view,
				out BinaryNinja.Type? outType,
				out QualifiedName outVarName
			)
			{
				if (null == arch)
				{
					throw new ArgumentNullException(nameof(arch));
				}

				IntPtr type = IntPtr.Zero;
				BNQualifiedName nativeName = new BNQualifiedName();
				bool success = NativeMethods.BNDemanglerDemangle(
					this.handle,
					arch.DangerousGetHandle(),
					name ?? string.Empty,
					(IntPtr)(&type),
					(IntPtr)(&nativeName),
					null == view ? IntPtr.Zero : view.DangerousGetHandle()
				);
				if (!success)
				{
					outType = null;
					outVarName = new QualifiedName(Array.Empty<string>());
					return false;
				}

				outType = BinaryNinja.Type.TakeHandle(type);
				outVarName = QualifiedName.TakeNative(nativeName);
				return true;
			}
		}
	}
}
