using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNCallLayout
	{
		/// <summary>
		/// BNValueLocation* parameters
		/// </summary>
		internal IntPtr parameters;

		/// <summary>
		/// size_t parameterCount
		/// </summary>
		internal ulong parameterCount;

		/// <summary>
		/// bool returnValueValid
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool returnValueValid;

		/// <summary>
		/// BNValueLocation returnValue
		/// </summary>
		internal BNValueLocation returnValue;

		/// <summary>
		/// int64_t stackAdjustment
		/// </summary>
		internal long stackAdjustment;

		/// <summary>
		/// uint32_t* registerStackAdjustmentRegisters
		/// </summary>
		internal IntPtr registerStackAdjustmentRegisters;

		/// <summary>
		/// int32_t* registerStackAdjustmentAmounts
		/// </summary>
		internal IntPtr registerStackAdjustmentAmounts;

		/// <summary>
		/// size_t registerStackAdjustmentCount
		/// </summary>
		internal ulong registerStackAdjustmentCount;
	}
}
