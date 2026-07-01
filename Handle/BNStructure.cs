using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Structure : AbstractSafeHandle<Structure>
	{
		public Structure(StructureType type) 
			: base( NativeMethods.BNGetTypeStructure(type.DangerousGetHandle()) , true)
		{
	        
		}
		
	    internal Structure(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static Structure? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Structure(
			    NativeMethods.BNNewStructureReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Structure MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Structure(
			    NativeMethods.BNNewStructureReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Structure? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Structure(handle, true);
	    }
	    
	    internal static Structure MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Structure(handle, true);
	    }
	    
	    internal static Structure? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Structure(handle, false);
	    }
	    
	    internal static Structure MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Structure(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeStructure(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public void TryGetMemberByOffset(
		    long offset , 
		    out StructureMember? member ,
		    out ulong index
	    )
	    {
		    member = StructureMember.TakeNativePointer(
			    NativeMethods.BNGetStructureMemberAtOffset(this.handle , offset , out index )
		    );
	    }

	    public StructureMember[] Members
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetStructureMembers(this.handle, out ulong arrayLength);

			    return UnsafeUtils.TakeStructArrayEx<BNStructureMember , StructureMember>(
				    arrayPointer ,
				    arrayLength ,
				    StructureMember.FromNative ,
				    NativeMethods.BNFreeStructureMemberList
			    );
		    }
	    }
	    
	    public BaseStructure[] BaseStructures
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBaseStructuresForStructure(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArrayEx<BNBaseStructure , BaseStructure>(
				    arrayPointer, 
				    arrayLength,
				    BaseStructure.FromNative,
				    NativeMethods.BNFreeBaseStructureList
			    );
		    }
	    }
	    
	    public bool Packed
	    {
		    get
		    {
			    return NativeMethods.BNIsStructurePacked(this.handle);
		    }
	    }

	    public ulong Alignment
	    {
		    get
		    {
			    return NativeMethods.BNGetStructureAlignment(this.handle);
		    }
	    }
		
	    public ulong Width
	    {
		    get
		    {
			    return NativeMethods.BNGetStructureWidth(this.handle);
		    }
	    }
		
	    public long PointerOffset
	    {
		    get
		    {
			    return NativeMethods.BNGetStructurePointerOffset(this.handle);
		    }
	    }
		
	    public bool IsUnion
	    {
		    get
		    {
			    return NativeMethods.BNIsStructureUnion(this.handle);
		    }
	    }
		
	    public bool PropagatesDataVariableReferences
	    {
		    get
		    {
			    return NativeMethods.BNStructurePropagatesDataVariableReferences(this.handle);
		    }
	    }
		
	    public StructureVariant Type
	    {
		    get
		    {
			    return NativeMethods.BNGetStructureType(this.handle);
		    }
	    }

	    public StructureType CreateType()
	    {
		    return StructureType.MustNewFromHandle(
			    NativeMethods.BNCreateStructureType(this.handle)
		    );
	    }
	    
	    public void TryGetMemberByName(
		    string name, 
		    out StructureMember? member)
	    {
		    member = StructureMember.TakeNativePointer(
			    NativeMethods.BNGetStructureMemberByName(this.handle , name)
		    );
	    }
		
	    public StructureMember? GetMemberByName(string name)
	    {
		    this.TryGetMemberByName(name , out StructureMember? member);

		    return member;
	    }
		
	    public StructureMember this[string name]
	    {
		    get
		    {
			    this.TryGetMemberByName(name , out StructureMember? member);

			    if (null == member)
			    {
				    throw new KeyNotFoundException();
			    }
				
			    return member;
		    }
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
		
	    public ulong? GetOffsetByName(string name )
	    {
		    foreach (StructureMember member in this.Members)
		    {
			    if (member.Name == name)
			    {
				    return member.Offset;
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
	    
	    
	    /// <summary>
	    /// Create a copy of this structure with one enumeration replaced by another.
	    /// </summary>
	    public Structure WithReplacedEnumeration(Enumeration from , Enumeration to)
	    {
		    return Structure.MustTakeHandle(
			    NativeMethods.BNStructureWithReplacedEnumeration(
				    this.handle ,
				    from.DangerousGetHandle() ,
				    to.DangerousGetHandle()
			    )
		    );
	    }

	    /// <summary>
	    /// Create a copy of this structure with one named type reference replaced by another.
	    /// </summary>
	    public Structure WithReplacedNamedTypeReference(NamedTypeReference from , NamedTypeReference to)
	    {
		    return Structure.MustTakeHandle(
			    NativeMethods.BNStructureWithReplacedNamedTypeReference(
				    this.handle ,
				    from.DangerousGetHandle() ,
				    to.DangerousGetHandle()
			    )
		    );
	    }

	    /// <summary>
	    /// Create a copy of this structure with one nested structure replaced by another.
	    /// </summary>
	    public Structure WithReplacedStructure(Structure from , Structure to)
	    {
		    return Structure.MustTakeHandle(
			    NativeMethods.BNStructureWithReplacedStructure(
				    this.handle ,
				    from.DangerousGetHandle() ,
				    to.DangerousGetHandle()
			    )
		    );
	    }

	    /// <summary>
	    /// Gets all structure members including those inherited from base structures.
	    /// This resolves the full member list through the base structure chain using
	    /// the type information available in the given type container.
	    /// </summary>
	    /// <param name="types">The type container providing type resolution context.</param>
	    /// <returns>An array of InheritedStructureMember including base structure members.</returns>
	    public unsafe InheritedStructureMember[] GetMembersIncludingInherited(TypeContainer types)
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    // 2. Call the native API to retrieve the inherited member array.
		    IntPtr ptr = NativeMethods.BNGetStructureMembersIncludingInherited(
			    this.handle ,
			    types.DangerousGetHandle() ,
			    (IntPtr)(&count)
		    );

		    // 3. Return empty if no results.
		    if (IntPtr.Zero == ptr || 0 == count)
		    {
			    if (IntPtr.Zero != ptr)
			    {
				    NativeMethods.BNFreeInheritedStructureMemberList(ptr , count);
			    }

			    return Array.Empty<InheritedStructureMember>();
		    }

		    // 4. Marshal each native BNInheritedStructureMember into a managed object.
		    InheritedStructureMember[] results = new InheritedStructureMember[checked((int)count)];

		    int structSize = Marshal.SizeOf<BNInheritedStructureMember>();

		    for (ulong i = 0; i < count; i++)
		    {
			    IntPtr elementPtr = IntPtr.Add(ptr , checked((int)(i * (ulong)structSize)));

			    BNInheritedStructureMember native = Marshal.PtrToStructure<BNInheritedStructureMember>(elementPtr);

			    results[i] = new InheritedStructureMember(
				    NamedTypeReference.MustNewFromHandle(native._base) ,
				    native.baseOffset ,
				    StructureMember.FromNative(native.member) ,
				    native.memberIndex
			    );
		    }

		    // 5. Free the native array.
		    NativeMethods.BNFreeInheritedStructureMemberList(ptr , count);

		    return results;
	    }

	    public override string ToString()
	    {
		    StringBuilder builder = new StringBuilder();

		    if (this.Packed)
		    {
			    builder.AppendLine("// Packed ");
		    }
		   
		    if (this.Alignment > 0)
		    {
			    builder.AppendLine($"// Alignment: ({Alignment})");
		    }
		    
		    if (this.Width > 0)
		    {
			    builder.AppendLine($"// Width: ({Width})");
		    }
		    
		    if (this.PointerOffset > 0)
		    {
			    builder.AppendLine($"// PointerOffset: ({PointerOffset}) ");
		    }

		    if (this.IsUnion)
		    {
			    builder.AppendLine("union {");
		    }
		    else
		    {
			    builder.AppendLine("struct {");
		    }
		    
		    foreach (StructureMember member in Members)
		    {
			    builder.AppendLine( member.ToString());
			    builder.AppendLine();
		    }
		    
		    builder.AppendLine("}");

		    return builder.ToString();
	    }

	    /// <summary>
	    /// Gets the inherited structure member at the given byte offset, including
	    /// members from base structures. Returns null if no member is found at the offset.
	    /// </summary>
	    /// <param name="view">The binary view providing type context.</param>
	    /// <param name="offset">The byte offset to query.</param>
	    /// <returns>An InheritedStructureMember, or null if no member exists at the offset.</returns>
	    public unsafe InheritedStructureMember? GetMemberIncludingInheritedAtOffset(BinaryView view , long offset)
	    {
		    // 1. Call the native API.
		    IntPtr resultPtr = NativeMethods.BNGetMemberIncludingInheritedAtOffset(
			    this.handle ,
			    view.DangerousGetHandle() ,
			    offset
		    );

		    // 2. Return null if no member found.
		    if (IntPtr.Zero == resultPtr)
		    {
			    return null;
		    }

		    // 3. Marshal the native struct.
		    BNInheritedStructureMember native = Marshal.PtrToStructure<BNInheritedStructureMember>(resultPtr);

		    // 4. Convert to managed type.
		    InheritedStructureMember result = new InheritedStructureMember(
			    NamedTypeReference.MustNewFromHandle(native._base) ,
			    native.baseOffset ,
			    StructureMember.FromNative(native.member) ,
			    native.memberIndex
		    );

		    // 5. Free the native struct.
		    NativeMethods.BNFreeInheritedStructureMember(resultPtr);

		    return result;
	    }

	}

	
}