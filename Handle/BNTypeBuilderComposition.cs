using System;

namespace BinaryNinja
{
    public partial class TypeBuilder
    {
        /// <summary>Finalizes this generic builder into an immutable type.</summary>
        public BinaryNinja.Type FinalizeType()
        {
            return BinaryNinja.Type.MustTakeHandle(
                NativeMethods.BNFinalizeTypeBuilder(this.handle)
            );
        }

        /// <summary>Replaces the mutable structure owned by this type builder.</summary>
        /// <remarks>
        /// The core takes ownership and invalidates <paramref name="builder"/>.
        /// </remarks>
        public void SetStructureBuilder(StructureBuilder builder)
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            NativeMethods.BNSetStructureBuilder(
                this.handle,
                builder.DangerousGetHandle()
            );
            builder.TransferOwnershipToCore();
        }

        /// <summary>Replaces the mutable enumeration owned by this type builder.</summary>
        /// <remarks>
        /// The core takes ownership and invalidates <paramref name="builder"/>.
        /// </remarks>
        public void SetEnumerationBuilder(EnumerationBuilder builder)
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            NativeMethods.BNSetEnumerationBuilder(
                this.handle,
                builder.DangerousGetHandle()
            );
            builder.TransferOwnershipToCore();
        }

        /// <summary>Replaces the mutable named reference owned by this type builder.</summary>
        /// <remarks>
        /// The core takes ownership and invalidates <paramref name="builder"/>.
        /// </remarks>
        public void SetNamedTypeReferenceBuilder(
            NamedTypeReferenceBuilder builder
        )
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            NativeMethods.BNSetNamedTypeReferenceBuilder(
                this.handle,
                builder.DangerousGetHandle()
            );
            builder.TransferOwnershipToCore();
        }
    }
}
