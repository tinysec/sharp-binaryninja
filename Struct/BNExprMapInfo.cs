using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNExprMapInfo 
	{
		/// <summary>
		/// uint64_t lowerIndex
		/// </summary>
		public ulong lowerIndex;
		
		/// <summary>
		/// uint64_t higherIndex
		/// </summary>
		public ulong higherIndex;
		
		/// <summary>
		/// bool mapLowerToHigher
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool mapLowerToHigher;
		
		/// <summary>
		/// bool mapHigherToLower
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool mapHigherToLower;
		
		/// <summary>
		/// bool lowerToHigherDirect
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool lowerToHigherDirect;
		
		/// <summary>
		/// bool higherToLowerDirect
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool higherToLowerDirect;
	}

    public class ExprMapInfo 
    {
		public ulong LowerIndex { get; set; } = 0;
		
		public ulong HigherIndex { get; set; } = 0;
		
		public bool MapLowerToHigher { get; set; } = false;
		
		public bool MapHigherToLower { get; set; } = false;
		
		public bool LowerToHigherDirect { get; set; } = false;
		
		public bool HigherToLowerDirect { get; set; } = false;
		
		public ExprMapInfo() 
		{
		    
		}
    }
}