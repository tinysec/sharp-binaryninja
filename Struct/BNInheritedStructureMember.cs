using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	using MemberIndex = ulong;
	
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNInheritedStructureMember 
	{
		/// <summary>
		/// BNNamedTypeReference* _base
		/// </summary>
		public IntPtr _base;
		
		/// <summary>
		/// uint64_t baseOffset
		/// </summary>
		public ulong baseOffset;
		
		/// <summary>
		/// BNStructureMember member
		/// </summary>
		public BNStructureMember member;
		
		/// <summary>
		/// uint64_t memberIndex
		/// </summary>
		public ulong memberIndex;
	}

    public sealed class InheritedStructureMember 
    {
		public NamedTypeReference? BaseType { get; set; }
		
		public ulong BaseOffset { get; set; } = 0;
		
		public StructureMember Member { get; set; }
		
		public MemberIndex MemberIndex { get; set; } = 0;
		
		public InheritedStructureMember(
			NamedTypeReference? baseType,
			ulong baseOffset, 
			StructureMember member,
			MemberIndex memberIndex
		) 
		{
		    this.BaseType = baseType;
		    
		    this.BaseOffset = baseOffset;
		    
		    this.Member = member;
		    
		    this.MemberIndex = memberIndex;
		}
    }
}
