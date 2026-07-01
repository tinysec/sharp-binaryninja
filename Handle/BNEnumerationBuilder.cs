using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class EnumerationBuilder :  AbstractSafeHandle<EnumerationBuilder>
	{
		public EnumerationBuilder() 
			: this(NativeMethods.BNCreateEnumerationBuilder() , true)
		{
		    
		}
		
		public EnumerationBuilder(Enumeration enumeration) 
			: this(
				NativeMethods.BNCreateEnumerationBuilderFromEnumeration(
					enumeration.DangerousGetHandle()
				) , true)
		{
		    
		}
		
	    internal EnumerationBuilder(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }
	    
	    internal static EnumerationBuilder? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new EnumerationBuilder(handle, true);
	    }
	    
	    internal static EnumerationBuilder MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new EnumerationBuilder(handle, true);
	    }
	    
	    internal static EnumerationBuilder? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new EnumerationBuilder(handle, false);
	    }
	    
	    internal static EnumerationBuilder MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new EnumerationBuilder(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeEnumerationBuilder(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public EnumerationBuilder Duplicate()
	    {
		    return new EnumerationBuilder(
			    NativeMethods.BNDuplicateEnumerationBuilder(this.handle),
			    true
		    );
	    }

	    public EnumerationMember[] Members
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetEnumerationBuilderMembers(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArrayEx<BNEnumerationMember , EnumerationMember>(
				    arrayPointer ,
				    arrayLength ,
				    EnumerationMember.FromNative,
				    NativeMethods.BNFreeEnumerationMemberList
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

			    foreach (EnumerationMember member in value)
			    {
				    NativeMethods.BNAddEnumerationBuilderMemberWithValue(
					    this.handle, 
					    member.Name , 
					    member.Value
					);
			    }
		    }
	    }
	    
	    public void AddMember(string name)
	    {
		    NativeMethods.BNAddEnumerationBuilderMember(this.handle , name);
	    }
	    
	    public void AddMemberWithValue(string name , ulong value)
	    {
		    NativeMethods.BNAddEnumerationBuilderMemberWithValue(
			    this.handle , 
			    name,
			    value
			);
	    }
	    
	    public void RemoveMember(int index)
	    {
		    NativeMethods.BNRemoveEnumerationBuilderMember(this.handle , (ulong)index);
	    }
	    
	    public void ReplaceMember(int index , string name , ulong value)
	    {
		    NativeMethods.BNReplaceEnumerationBuilderMember(
			    this.handle , 
			    (ulong)index,
			    name,
			    value
		    );
	    }

	    public Enumeration Build()
	    {
		    return new Enumeration(
			    NativeMethods.BNFinalizeEnumerationBuilder(this.handle),
			    true
		    );
	    }
	    
	}

	
}