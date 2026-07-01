using System;

namespace BinaryNinja
{
	public sealed class StructureType : BinaryNinja.Type
	{
		internal StructureType(IntPtr handle , bool owner) : base(handle , owner)
		{
			
		}
		
		public StructureType(BinaryNinja.Type type) 
			: base( type.DangerousGetHandle() , true)
		{
			
		}
		
		internal new static StructureType MustNewFromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new StructureType(
				NativeMethods.BNNewTypeReference(handle) ,
				true
			);
		}
		
		public QualifiedName StructureName
		{
			get
			{
				return QualifiedName.TakeNative(
					NativeMethods.BNTypeGetStructureName(this.handle)
				);
			}
		}

		public Structure Structure
		{
			get
			{
				return Structure.MustTakeHandle(
					NativeMethods.BNGetTypeStructure(this.handle)
				);
			}
		}

		/// <summary>
		/// The members of the underlying structure. Convenience forwarder for
		/// <c>Structure.Members</c> (Python <c>StructureType.members</c>).
		/// </summary>
		public StructureMember[] Members
		{
			get
			{
				return this.Structure.Members;
			}
		}

		/// <summary>
		/// The base structures. Forwards <c>Structure.BaseStructures</c>.
		/// </summary>
		public BaseStructure[] BaseStructures
		{
			get
			{
				return this.Structure.BaseStructures;
			}
		}

		/// <summary>
		/// The member at the given byte offset, or <c>null</c>. Forwards
		/// <c>Structure.GetMemberByOffset</c> (Python <c>StructureType.member_at_offset</c>).
		/// </summary>
		public StructureMember? MemberAtOffset(ulong offset)
		{
			return this.Structure.GetMemberByOffset(offset);
		}
	}
}
