using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class StructureBuilder : AbstractSafeHandle<StructureBuilder> 
	{
		public StructureBuilder(
			StructureVariant kind = StructureVariant.StructStructureType
			, bool packed = false)
			: this(NativeMethods.BNCreateStructureBuilderWithOptions(kind , packed) , true)
		{

		}

		internal StructureBuilder(IntPtr handle, bool owner)
			: base( handle , owner)
		{

		}

        /// <summary>
        /// Creates a new empty StructureBuilder with default settings.
        /// </summary>
        /// <returns>A new owned StructureBuilder instance.</returns>
        public static StructureBuilder Create()
        {
            // Create an empty structure builder; the returned handle is owned.
            return StructureBuilder.MustTakeHandle(
                NativeMethods.BNCreateStructureBuilder()
            );
        }

        /// <summary>
        /// Creates a StructureBuilder from an existing Structure instance,
        /// copying all members, alignment, and other properties.
        /// </summary>
        /// <param name="structure">The structure to copy into the builder.</param>
        /// <returns>A new owned StructureBuilder pre-populated from the given structure.</returns>
        public static StructureBuilder FromStructure(Structure structure)
        {
            // 1. Validate the required structure parameter.
            if (null == structure)
            {
                throw new ArgumentNullException(nameof(structure));
            }

            // 2. Create a builder from the existing structure; the returned handle is owned.
            return StructureBuilder.MustTakeHandle(
                NativeMethods.BNCreateStructureBuilderFromStructure(structure.DangerousGetHandle())
            );
        }

		internal static StructureBuilder? TakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new StructureBuilder(handle, true);
		}
	    
		internal static StructureBuilder MustTakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new StructureBuilder(handle, true);
		}
	    
		internal static StructureBuilder? BorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new StructureBuilder(handle, false);
		}
	    
		internal static StructureBuilder MustBorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new StructureBuilder(handle, false);
		}
		
		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				NativeMethods.BNFreeTypeBuilder(this.handle);
				this.SetHandleAsInvalid();
			}

			return true;
		}
		
		public StructureMember[] Members
		{
			get
			{
				ulong arrayLength = 0;
				
				IntPtr arrayPointer = NativeMethods.BNGetStructureBuilderMembers(this.handle , out arrayLength);
				
				return UnsafeUtils.TakeStructArrayEx<BNStructureMember , StructureMember>(
					arrayPointer,
					arrayLength,
					StructureMember.FromNative,
					NativeMethods.BNFreeStructureMemberList
				);
			}

			set
			{
				if (this.Members.Length > 0)
				{
					for (int index = this.Members.Length - 1; index >= 0; index--)
					{
						this.RemoveMember(index);
					}
				}
				
				foreach (StructureMember member in value)
				{
					NativeMethods.BNAddStructureBuilderMemberAtOffset(
						this.handle ,
						new TypeWithConfidence(member.Type , member.TypeConfidence).ToNative() ,
						member.Name ,
						member.Offset ,
						true , // overwriteExisting
						member.Access ,
						member.Scope ,
						0 , // bitPosition (non-bitfield)
						0   // bitWidth (non-bitfield)
					);
				}
			}
		}

		public void AddMember(
			TypeWithConfidence kind ,
			string name ,
			MemberAccess access = MemberAccess.PublicAccess,
			MemberScope scope = MemberScope.NoScope
		)
		{
			NativeMethods.BNAddStructureBuilderMember(
				this.handle ,
				kind.ToNative() ,
				name ,
				access ,
				scope
			);
		}
		
		public void AddMemberAtOffset(
			TypeWithConfidence kind ,
			string name ,
			ulong offset ,
			bool overwriteExisting = true,
			MemberAccess access = MemberAccess.PublicAccess,
			MemberScope scope = MemberScope.NoScope,
			byte bitPosition = 0,
			byte bitWidth = 0
		)
		{
			NativeMethods.BNAddStructureBuilderMemberAtOffset(
				this.handle ,
				kind.ToNative() ,
				name ,
				offset ,
				overwriteExisting ,
				access ,
				scope ,
				bitPosition ,
				bitWidth
			);
		}
		
		public BaseStructure[] BaseStructures
		{
			get
			{
				ulong arrayLength = 0;

				IntPtr arrayPointer = NativeMethods.BNGetBaseStructuresForStructureBuilder(
					this.handle ,
					out arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNBaseStructure , BaseStructure>(
					arrayPointer, 
					arrayLength,
					BaseStructure.FromNative,
					NativeMethods.BNFreeBaseStructureList
				);
			}

			set
			{
				NativeMethods.BNSetBaseStructuresForStructureBuilder(
					this.handle ,
					UnsafeUtils.ConvertToNativeArray<BNBaseStructure , BaseStructure>(
						value
					),
					(ulong)value.Length
				);
			}
		}

		public bool Packed
		{
			get
			{
				return NativeMethods.BNIsStructureBuilderPacked(this.handle);
			}

			set
			{
				NativeMethods.BNSetStructureBuilderPacked(this.handle, value);
			}
		}

		public ulong Alignment
		{
			get
			{
				return NativeMethods.BNGetStructureBuilderAlignment(this.handle);
			}

			set
			{
				NativeMethods.BNSetStructureBuilderAlignment(this.handle, value);
			}
		}
		
		public ulong Width
		{
			get
			{
				return NativeMethods.BNGetStructureBuilderWidth(this.handle);
			}

			set
			{
				NativeMethods.BNSetStructureBuilderWidth(this.handle, value);
			}
		}
		
		public long PointerOffset
		{
			get
			{
				return NativeMethods.BNGetStructureBuilderPointerOffset(this.handle);
			}

			set
			{
				NativeMethods.BNSetStructureBuilderPointerOffset(this.handle, value);
			}
		}
		
		public bool IsUnion
		{
			get
			{
				return NativeMethods.BNIsStructureBuilderUnion(this.handle);
			}
		}
		
		public bool PropagatesDataVariableReferences
		{
			get
			{
				return NativeMethods.BNStructureBuilderPropagatesDataVariableReferences(this.handle);
			}

			set
			{
				NativeMethods.BNSetStructureBuilderPropagatesDataVariableReferences(this.handle, value);
			}
		}
		
		public StructureVariant Type
		{
			get
			{
				return NativeMethods.BNGetStructureBuilderType(this.handle);
			}

			set
			{
				NativeMethods.BNSetStructureBuilderType(this.handle, value);
			}
		}

		public void RemoveMember(int index)
		{
			NativeMethods.BNRemoveStructureBuilderMember(this.handle , (ulong)index);
		}
		
		public void ReplaceMember(
			ulong index , 
			TypeWithConfidence kind , 
			string name , 
			bool overwriteExisting)
		{
			NativeMethods.BNReplaceStructureBuilderMember(
				this.handle ,
				index,
				kind.ToNative() ,
				name ,
				overwriteExisting 
			);
		}

		public StructureMember? GetMemberByName(string name )
		{
			IntPtr rawPtr = NativeMethods.BNGetStructureBuilderMemberByName(this.handle , name);

			if (rawPtr == IntPtr.Zero)
			{
				return null;
			}
	
			return StructureMember.FromNativePointer(rawPtr);
		}
		
		public StructureMember? GetMemberByOffset(ulong offset )
		{
			foreach (StructureMember member in Members)
			{
				if (member.Offset == offset)
				{
					return member;
				}
			}

			return null;
		}
		
		public int? GetIndexByName(string name )
		{
			for (int i = 0; i < this.Members.Length; i++)
			{
				if (this.Members[i].Name == name)
				{
					return i;
				}
			}

			return null;
		}
		
		public int? GetIndexByOffset(ulong offset )
		{
			for (int i = 0; i < this.Members.Length; i++)
			{
				if (this.Members[i].Offset == offset)
				{
					return i;
				}
			}

			return null;
		}

		public Structure Build()
		{
			return new Structure(
				NativeMethods.BNFinalizeStructureBuilder(this.handle),
				true
			);
		}

		/// <summary>
		/// Gets the structure member at the specified byte offset, along with its index.
		/// </summary>
		/// <param name="offset">The byte offset within the structure.</param>
		/// <param name="index">When this method returns, contains the member index if found.</param>
		/// <returns>The StructureMember at the offset, or null if no member exists there.</returns>
		public unsafe StructureMember? GetMemberAtOffset(long offset , out ulong index)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr idxPtr = allocator.AllocStruct<ulong>(0);

				IntPtr raw = NativeMethods.BNGetStructureBuilderMemberAtOffset(
					this.handle ,
					offset ,
					idxPtr
				);

				index = (ulong)Marshal.ReadInt64(idxPtr);

				return StructureMember.TakeNativePointer(raw);
			}
		}
	}
}