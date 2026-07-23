using System;

namespace BinaryNinja
{
    public static partial class Core
    {
        /// <summary>Displays a form and updates its managed result fields.</summary>
        public static bool GetFormInput(FormInputField[] fields, string title)
        {
            if (null == fields)
            {
                throw new ArgumentNullException(nameof(fields));
            }

            using ScopedAllocator allocator = new ScopedAllocator();
            BNFormInputField[] nativeFields =
                new BNFormInputField[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                if (null == fields[i])
                {
                    throw new ArgumentException(
                        "Form fields cannot contain null entries.",
                        nameof(fields)
                    );
                }

                nativeFields[i] = fields[i].ToNative(allocator);
            }

            bool result = NativeMethods.BNGetFormInput(
                nativeFields,
                (ulong)nativeFields.Length,
                title ?? string.Empty
            );
            if (!result)
            {
                return false;
            }

            try
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i].ReadResult(nativeFields[i]);
                }
            }
            finally
            {
                NativeMethods.BNFreeFormInputResults(
                    nativeFields,
                    (ulong)nativeFields.Length
                );
            }

            return true;
        }
    }
}
