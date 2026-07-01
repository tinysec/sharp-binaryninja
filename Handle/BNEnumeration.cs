using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Enumeration : AbstractSafeHandle<Enumeration>
	{
		public Enumeration(EnumerationType type) 
			: base( NativeMethods.BNGetTypeEnumeration(type.DangerousGetHandle()) , true)
		{
	        
		}
		
		internal Enumeration(IntPtr handle , bool owner)
			: base(handle , owner)
	    {

	    }

        /// <summary>
        /// Creates a new empty Enumeration by building an empty EnumerationBuilder.
        /// Members can be added by first creating an EnumerationBuilder, populating it, and calling Build().
        /// </summary>
        /// <returns>A new owned Enumeration instance with no members.</returns>
        public static Enumeration Create()
        {
            // 1. Create an empty builder and finalize it into an Enumeration.
            using (EnumerationBuilder builder = new EnumerationBuilder())
            {
                return builder.Build();
            }
        }

		internal static Enumeration? NewFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new Enumeration(
				NativeMethods.BNNewEnumerationReference(handle) ,
				true
			);
		}
	    
		internal static Enumeration MustNewFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new Enumeration(
				NativeMethods.BNNewEnumerationReference(handle) ,
				true
			);
		}
	    
		internal static Enumeration? TakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new Enumeration(handle, true);
		}
	    
		internal static Enumeration MustTakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new Enumeration(handle, true);
		}
	    
		internal static Enumeration? BorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new Enumeration(handle, false);
		}
	    
		internal static Enumeration MustBorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new Enumeration(handle, false);
		}
		
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeEnumeration(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public override string ToString()
	    {
		    StringBuilder builder = new StringBuilder();

		    foreach (EnumerationMember member in Members)
		    {
			    builder.AppendLine( member.ToString() + ";");
		    }
		    
		    return builder.ToString();
	    }

	    public EnumerationMember[] Members
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetEnumerationMembers(
				    this.handle,
				    out ulong arrayLength
				);

			    return UnsafeUtils.TakeStructArrayEx<BNEnumerationMember , EnumerationMember>(
				    arrayPointer ,
				    arrayLength ,
				    EnumerationMember.FromNative ,
				    NativeMethods.BNFreeEnumerationMemberList
			    );
		    }
	    }
	    
	    public EnumerationType CreateType(ulong width , BoolWithConfidence signed)
	    {
		    IntPtr raw = NativeMethods.BNCreateEnumerationTypeOfWidth(
			    this.handle ,
			    width , 
			    signed.ToNative()
			);

		    if (IntPtr.Zero == raw)
		    {
			    throw new Exception("Could not create an enumeration type");
		    }
		    
		    return new EnumerationType(raw , true );
	    }

	    public EnumerationBuilder CreateBuilder()
	    {
		    return new EnumerationBuilder(
			    NativeMethods.BNCreateEnumerationBuilderFromEnumeration(this.handle) ,
			    true
		    );
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="value"></param>
	    /// <param name="width">1,2,4,8</param>
	    /// <param name="type"></param>
	    /// <returns></returns>
	    public InstructionTextToken[] GetTokensForValue(
		    uint value,
		    ulong width,
		    BinaryNinja.Type? type = null
		)
	    {
		    BinaryNinja.Type autoType;

		    if (type == null)
		    {
			    autoType = BinaryNinja.Type.MustTakeHandle(
				    NativeMethods.BNCreateEnumerationTypeOfWidth(
					    this.handle ,
					    width ,
					    new BoolWithConfidence().ToNative()
				    )
			    );
		    }
		    else
		    {
			    autoType = type;
		    }
		    
		    IntPtr arrayPointer = NativeMethods.BNGetEnumerationTokensForValue(
			    this.handle ,
			    value ,
			    width ,
			    out ulong arrayLength ,
			    autoType.DangerousGetHandle()
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
			    arrayPointer ,
			    arrayLength ,
			    InstructionTextToken.FromNative ,
			    NativeMethods.BNFreeInstructionText
		    );
	    }
	}
}