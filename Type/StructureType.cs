using System;

namespace BinaryNinja
{
	public sealed class StructureType : BinaryNinja.Type
	{
		internal StructureType(IntPtr handle , bool owner) : base(handle , owner)
		{
			
		}
		
		public StructureType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
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
				using (Structure structure = this.Structure)
				{
					return structure.Members;
				}
			}
		}

		/// <summary>
		/// The base structures. Forwards <c>Structure.BaseStructures</c>.
		/// </summary>
		public BaseStructure[] BaseStructures
		{
			get
			{
				using (Structure structure = this.Structure)
				{
					return structure.BaseStructures;
				}
			}
		}

		/// <summary>
		/// The child member types in declaration order.
		/// </summary>
		public BinaryNinja.Type[] Children
		{
			get
			{
				StructureMember[] members = this.Members;
				BinaryNinja.Type[] result = new BinaryNinja.Type[members.Length];

				for (int index = 0; index < members.Length; index++)
				{
					result[index] = members[index].Type;
				}

				return result;
			}
		}

		public long PointerOffset
		{
			get
			{
				using (Structure structure = this.Structure)
				{
					return structure.PointerOffset;
				}
			}
		}

		public bool Packed
		{
			get
			{
				using (Structure structure = this.Structure)
				{
					return structure.Packed;
				}
			}
		}

		public bool IsUnion
		{
			get
			{
				using (Structure structure = this.Structure)
				{
					return structure.IsUnion;
				}
			}
		}

		public bool PropagatesDataVariableReferences
		{
			get
			{
				using (Structure structure = this.Structure)
				{
					return structure.PropagatesDataVariableReferences;
				}
			}
		}

		public StructureVariant Variant
		{
			get
			{
				using (Structure structure = this.Structure)
				{
					return structure.Type;
				}
			}
		}

		public StructureMember this[string name]
		{
			get
			{
				using (Structure structure = this.Structure)
				{
					return structure[name];
				}
			}
		}

		/// <summary>
		/// The member at the given byte offset, or <c>null</c>. Forwards
		/// <c>Structure.GetMemberByOffset</c> (Python <c>StructureType.member_at_offset</c>).
		/// </summary>
		public StructureMember? MemberAtOffset(ulong offset)
		{
			using (Structure structure = this.Structure)
			{
				return structure.GetMemberByOffset(offset);
			}
		}

		/// <summary>
		/// Creates a writable builder initialized from this structure.
		/// </summary>
		public StructureBuilder MutableCopy()
		{
			using (Structure structure = this.Structure)
			{
				return StructureBuilder.FromStructure(structure);
			}
		}

		public InheritedStructureMember[] GetMembersIncludingInherited(TypeContainer types)
		{
			if (null == types)
			{
				throw new ArgumentNullException(nameof(types));
			}

			using (Structure structure = this.Structure)
			{
				return structure.GetMembersIncludingInherited(types);
			}
		}

		public InheritedStructureMember[] GetMembersIncludingInherited(BinaryView view)
		{
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			using (TypeContainer types = view.TypeContainer)
			{
				return this.GetMembersIncludingInherited(types);
			}
		}

		public InheritedStructureMember? GetMemberIncludingInheritedAtOffset(
			BinaryView view,
			long offset)
		{
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			using (Structure structure = this.Structure)
			{
				return structure.GetMemberIncludingInheritedAtOffset(view, offset);
			}
		}

		public bool ResolveMemberOrBaseMember(
			BinaryView? view,
			ulong offset,
			ulong size,
			StructureMemberResolutionCallback resolveCallback,
			ulong? memberIndexHint = null)
		{
			using (Structure structure = this.Structure)
			{
				return structure.ResolveMemberOrBaseMember(
					view,
					offset,
					size,
					resolveCallback,
					memberIndexHint);
			}
		}

		public NamedTypeReferenceType GenerateNamedTypeReference(
			string id,
			QualifiedName name)
		{
			NamedTypeReferenceClass referenceClass;

			switch (this.Variant)
			{
				case StructureVariant.StructStructureType:
					referenceClass = NamedTypeReferenceClass.StructNamedTypeClass;
					break;

				case StructureVariant.UnionStructureType:
					referenceClass = NamedTypeReferenceClass.UnionNamedTypeClass;
					break;

				default:
					referenceClass = NamedTypeReferenceClass.ClassNamedTypeClass;
					break;
			}

			using (NamedTypeReference reference = new NamedTypeReference(
				referenceClass,
				id,
				name))
			{
				return new NamedTypeReferenceType(
					reference,
					this.Width,
					this.Alignment,
					new BoolWithConfidence(false),
					new BoolWithConfidence(false));
			}
		}

		public new StructureType WithReplacedStructure(Structure from, Structure to)
		{
			using (Structure structure = this.Structure)
			using (Structure replaced = structure.WithReplacedStructure(from, to))
			{
				return replaced.CreateType();
			}
		}

		public new StructureType WithReplacedEnumeration(Enumeration from, Enumeration to)
		{
			using (Structure structure = this.Structure)
			using (Structure replaced = structure.WithReplacedEnumeration(from, to))
			{
				return replaced.CreateType();
			}
		}

		public new StructureType WithReplacedNamedTypeReference(
			NamedTypeReference from,
			NamedTypeReference to)
		{
			using (Structure structure = this.Structure)
			using (Structure replaced = structure.WithReplacedNamedTypeReference(from, to))
			{
				return replaced.CreateType();
			}
		}
	}
}
