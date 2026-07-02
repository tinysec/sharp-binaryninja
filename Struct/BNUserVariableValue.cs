using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNUserVariableValue 
	{
		/// <summary>
		/// BNVariable variable
		/// </summary>
		internal BNVariable variable;
		
		/// <summary>
		/// BNArchitectureAndAddress defSite
		/// </summary>
		internal BNArchitectureAndAddress defSite;
		
		/// <summary>
		/// bool after
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool after;
		
		/// <summary>
		/// BNPossibleValueSet value
		/// </summary>
		internal BNPossibleValueSet value;
	}

    public sealed class UserVariableValue 
    {
		public CoreVariable Variable { get; set; }
		
		public ArchitectureAndAddress DefSite { get; set; }
		
		public bool After { get; set; }
		
		public PossibleValueSet Value { get; set; }
		
		internal UserVariableValue(BNUserVariableValue native)
		{
		    this.Variable = CoreVariable.FromNative(native.variable);
		    this.DefSite = ArchitectureAndAddress.FromNative(native.defSite);
		    this.After = native.after;
		    this.Value = PossibleValueSet.FromNative(native.value);
		}
    }
}