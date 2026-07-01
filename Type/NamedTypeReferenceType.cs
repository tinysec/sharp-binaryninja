using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class NamedTypeReferenceType : BinaryNinja.Type
	{
		public NamedTypeReferenceType(
			NamedTypeReference namedType ,
			ulong width ,
			ulong align ,
			BoolWithConfidence cnst ,
			BoolWithConfidence vltl
		)
			: base(
				NamedTypeReferenceType.create(
					namedType ,
					width ,
					align ,
					cnst ,
					vltl
				) ,
				true
			)
		{

		}
		
		public NamedTypeReferenceType(BinaryNinja.Type type) 
			: base( type.DangerousGetHandle() , true)
		{
			
		}

		internal NamedTypeReferenceType(IntPtr handle , bool owner) : base(handle , owner)
		{

		}

		private static IntPtr create(
			NamedTypeReference namedType ,
			ulong width ,
			ulong align ,
			BoolWithConfidence cnst ,
			BoolWithConfidence vltl
		)
		{
			return NativeMethods.BNCreateNamedTypeReference(
				namedType.DangerousGetHandle() ,
				width ,
				align ,
				cnst.ToNative() ,
				vltl.ToNative()
			);
		}

		public NamedTypeReferenceClass NamedTypeReferenceClass
		{
			get
			{
				return NativeMethods.BNGetTypeReferenceClass(this.handle);
			}
		}
		
		public string TypeReferenceId
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetTypeReferenceId(this.handle));
			}
		}

		/// <summary>
		/// Resolves this named type reference to the concrete type it points at, by
		/// looking up its id in <paramref name="view"/> and following further named
		/// references until a concrete type is reached. Returns <c>null</c> if the id
		/// cannot be resolved or a reference cycle is detected. Mirrors Python
		/// <c>NamedTypeReferenceType.target(bv)</c>.
		/// </summary>
		public BinaryNinja.Type? GetTarget(BinaryView view)
		{
			HashSet<string> visited = new HashSet<string>();

			NamedTypeReferenceType current = this;

			while (true)
			{
				if (!visited.Add(current.TypeReferenceId))
				{
					// reference cycle
					return null;
				}

				BinaryNinja.Type? resolved = view.GetTypeById(current.TypeReferenceId);

				if (null == resolved)
				{
					return null;
				}

				NamedTypeReferenceType? next = resolved.AsNamedTypeReference();

				if (null == next)
				{
					return resolved;
				}

				current = next;
			}
		}

	}
}
