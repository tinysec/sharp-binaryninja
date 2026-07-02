using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNDataVariable 
	{
		/// <summary>
		/// uint64_t address
		/// </summary>
		internal ulong address;
		
		/// <summary>
		/// BNType* type
		/// </summary>
		internal IntPtr type;
		
		/// <summary>
		/// bool autoDiscovered
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool autoDiscovered;
		
		/// <summary>
		/// uint8_t typeConfidence
		/// </summary>
		internal byte typeConfidence;
	}

    public sealed class DataVariable
    {
		public ulong Address { get;  } = 0;

		public BinaryNinja.Type Type { get;  }

		public bool AutoDiscovered { get;  } = false;

		public byte TypeConfidence { get;  } = 0;

		/// <summary>
		/// The view this data variable belongs to, if it was obtained from one.
		/// Enables the navigation members below (Python's <c>DataVariable.view</c>).
		/// </summary>
		public BinaryView? View { get; internal set; } = null;

		public DataVariable(BNDataVariable native)
		{
			this.Address = native.address;
			this.Type = BinaryNinja.Type.MustNewFromHandle(native.type);
			this.AutoDiscovered = native.autoDiscovered;
			this.TypeConfidence = native.typeConfidence;
		}

		internal static DataVariable FromNative(BNDataVariable native)
		{
			return new DataVariable(native);
		}

		internal static DataVariable FromNative(BNDataVariable native , BinaryView? view)
		{
			DataVariable target = new DataVariable(native);

			target.View = view;

			return target;
		}

		internal static DataVariable TakeNative(BNDataVariable native)
		{
			DataVariable target = DataVariable.FromNative(native);

			NativeMethods.BNFreeDataVariable(native);

			return target;
		}

		internal static DataVariable TakeNative(BNDataVariable native , BinaryView? view)
		{
			DataVariable target = DataVariable.FromNative(native , view);

			NativeMethods.BNFreeDataVariable(native);

			return target;
		}

		private BinaryView RequireView()
		{
			if (null == this.View)
			{
				throw new InvalidOperationException(
					"this DataVariable is not associated with a BinaryView"
				);
			}

			return this.View;
		}

		/// <summary>
		/// The symbol defined at this data variable's address, or <c>null</c>.
		/// Mirrors Python <c>DataVariable.symbol</c>.
		/// </summary>
		public Symbol? Symbol
		{
			get
			{
				return this.RequireView().GetSymbolByAddress(this.Address);
			}
		}

		/// <summary>
		/// The (raw) name of this data variable, or <c>null</c> if it has no symbol.
		/// Mirrors Python <c>DataVariable.name</c>.
		/// </summary>
		public string? Name
		{
			get
			{
				return this.Symbol?.RawName;
			}
		}

		/// <summary>
		/// Code references to this data variable. Mirrors Python
		/// <c>DataVariable.code_refs</c>.
		/// </summary>
		public ReferenceSource[] CodeRefs
		{
			get
			{
				return this.RequireView().GetCodeReferences(this.Address);
			}
		}

		/// <summary>
		/// Addresses that reference this data variable. Mirrors Python
		/// <c>DataVariable.data_refs</c>.
		/// </summary>
		public ulong[] DataRefs
		{
			get
			{
				return this.RequireView().GetDataReferences(this.Address);
			}
		}

		/// <summary>
		/// Addresses referenced from this data variable. Mirrors Python
		/// <c>DataVariable.data_refs_from</c>.
		/// </summary>
		public ulong[] DataRefsFrom
		{
			get
			{
				return this.RequireView().GetDataReferencesFrom(this.Address);
			}
		}

		/// <summary>
		/// The components that contain this data variable. Mirrors Python
		/// <c>DataVariable.components</c>.
		/// </summary>
		public Component[] Components
		{
			get
			{
				return this.RequireView().GetDataVariableParentComponents(this.Address);
			}
		}

		internal BNDataVariable ToNative()
		{
			return new BNDataVariable()
			{
				address = this.Address ,
				type = this.Type.DangerousGetHandle() ,
				autoDiscovered = this.AutoDiscovered ,
				typeConfidence = this.TypeConfidence
			};
		}
    }
}