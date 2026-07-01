using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNStructureMember 
	{
		/// <summary>
		/// BNType* type
		/// </summary>
		internal IntPtr type;
		
		/// <summary>
		/// const char* name
		/// </summary>
		internal IntPtr name;
		
		/// <summary>
		/// uint64_t offset
		/// </summary>
		public ulong offset;
		
		/// <summary>
		/// uint8_t typeConfidence
		/// </summary>
		internal byte typeConfidence;
		
		/// <summary>
		/// BNMemberAccess access
		/// </summary>
		internal MemberAccess access;
		
		/// <summary>
		/// BNMemberScope scope
		/// </summary>
		internal MemberScope scope;

		/// <summary>
		/// uint8_t bitPosition
		/// </summary>
		internal byte bitPosition;

		/// <summary>
		/// uint8_t bitWidth
		/// </summary>
		internal byte bitWidth;
	}

    public sealed class StructureMember : INativeWrapperEx<BNStructureMember>
    {
		public BinaryNinja.Type Type { get; set; }
		
		public string Name { get; set; } = string.Empty;
		
		public ulong Offset { get; set; } = 0;
		
		public byte TypeConfidence { get; set; } = 0;
		
		public MemberAccess Access { get; set; } = MemberAccess.NoAccess;
		
		public MemberScope Scope { get; set; } = MemberScope.NoScope;

		public byte BitPosition { get; set; } = 0;

		public byte BitWidth { get; set; } = 0;

		public StructureMember(
			BinaryNinja.Type kind,
			string name,
			ulong offset = 0,
			byte typeConfidence = 0,
			MemberAccess access = MemberAccess.PublicAccess,
			MemberScope scope = MemberScope.NoScope,
			byte bitPosition = 0,
			byte bitWidth = 0
		)
		{
		    this.Type = kind;
		    this.Name = name;
		    this.Offset = offset;
		    this.TypeConfidence = typeConfidence;
		    this.Access = access;
		    this.Scope = scope;
		    this.BitPosition = bitPosition;
		    this.BitWidth = bitWidth;
		}

		internal static StructureMember MustFromNativePointer(IntPtr pointer)
		{
			if (IntPtr.Zero == pointer)
			{
				throw new ArgumentNullException(nameof(pointer));
			}
			
			return StructureMember.FromNative( Marshal.PtrToStructure<BNStructureMember>(pointer) );
		}

		internal static StructureMember? FromNativePointer(IntPtr pointer)
		{
			if (IntPtr.Zero == pointer)
			{
				return null;
			}
			
			return StructureMember.FromNative( Marshal.PtrToStructure<BNStructureMember>(pointer) ); 
		}
		
		internal static StructureMember FromNative(BNStructureMember native)
		{
			return new StructureMember(
				new BinaryNinja.Type(NativeMethods.BNNewTypeReference(native.type) , true) ,
				UnsafeUtils.ReadAnsiString(native.name) ,
				native.offset ,
				native.typeConfidence ,
				native.access ,
				native.scope ,
				native.bitPosition ,
				native.bitWidth
			);
		}

		internal static StructureMember? TakeNativePointer(IntPtr pointer)
		{
			if (IntPtr.Zero == pointer)
			{
				return null;
			}
			
			StructureMember member = StructureMember.FromNative( 
				Marshal.PtrToStructure<BNStructureMember>(pointer) 
			);

			NativeMethods.BNFreeStructureMember(pointer);
			
			return member;
		}

		public BNStructureMember ToNativeEx(ScopedAllocator allocator)
		{
			return new BNStructureMember()
			{
				type = this.Type.DangerousGetHandle() ,
				name = allocator.AllocAnsiString(this.Name) ,
				offset = this.Offset ,
				typeConfidence = this.TypeConfidence ,
				access = this.Access ,
				scope = this.Scope ,
				bitPosition = this.BitPosition ,
				bitWidth = this.BitWidth ,
			};
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			
			builder.AppendLine($"// offset: {this.Offset}");
			
			builder.AppendLine($"// confidence: {this.TypeConfidence}");
			
			builder.Append($"{this.Type.GetString()}  {this.Name};");
			
			return builder.ToString();
		}
    }
}