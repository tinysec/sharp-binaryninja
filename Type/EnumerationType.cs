using System;

namespace BinaryNinja
{
	public sealed class EnumerationType : BinaryNinja.Type
	{
		public EnumerationType(BinaryNinja.Type type) 
			: base( type.DangerousGetHandle() , true)
		{
			
		}
		
		internal EnumerationType(IntPtr handle , bool owner) : base(handle , owner)
		{

		}
		
		public Enumeration Enumeration
		{
			get
			{
				return Enumeration.MustTakeHandle(
					NativeMethods.BNGetTypeEnumeration(this.handle)
				);
			}
		}

		/// <summary>
		/// The members of the underlying enumeration. Convenience forwarder for
		/// <c>Enumeration.Members</c> (Python <c>EnumerationType.members</c>).
		/// </summary>
		public EnumerationMember[] Members
		{
			get
			{
				return this.Enumeration.Members;
			}
		}
	}
}
