using System;

namespace BinaryNinja
{
    /// <summary>Defines a registered high-level language representation.</summary>
    public abstract partial class LanguageRepresentationFunctionType :
        AbstractSafeHandle<LanguageRepresentationFunctionType>
    {
        private readonly string? registrationName;

        private bool custom;

        private bool registered;

        internal IntPtr RegistrationHandle
        {
            get
            {
                return this.handle;
            }
        }

        /// <summary>Creates an unregistered custom representation type.</summary>
        protected LanguageRepresentationFunctionType(string name)
            : base(false)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.custom = true;
            this.registrationName = name;
        }

        private LanguageRepresentationFunctionType(IntPtr handle)
            : base(handle, false)
        {
        }

        internal static LanguageRepresentationFunctionType? BorrowHandle(
            IntPtr handle
        )
        {
            if (IntPtr.Zero == handle)
            {
                return null;
            }

            return new CoreLanguageRepresentationFunctionType(handle);
        }

        internal static LanguageRepresentationFunctionType MustBorrowHandle(
            IntPtr handle
        )
        {
            LanguageRepresentationFunctionType? type =
                LanguageRepresentationFunctionType.BorrowHandle(handle);
            if (null == type)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return type;
        }

        /// <summary>Gets the unique registered language name.</summary>
        public string Name
        {
            get
            {
                if (this.IsInvalid)
                {
                    return this.registrationName ?? string.Empty;
                }

                return UnsafeUtils.TakeUtf8String(
                    NativeMethods.BNGetLanguageRepresentationFunctionTypeName(
                        this.handle
                    )
                );
            }
        }

        /// <summary>Gets the associated line formatter.</summary>
        public virtual LineFormatter? Formatter
        {
            get
            {
                if (this.custom)
                {
                    return null;
                }

                return LineFormatter.BorrowHandle(
                    NativeMethods.BNGetLanguageRepresentationFunctionTypeLineFormatter(
                        this.handle
                    )
                );
            }
        }

        /// <summary>Gets the associated type parser.</summary>
        public virtual TypeParser? Parser
        {
            get
            {
                if (this.custom)
                {
                    return null;
                }

                return TypeParser.BorrowHandle(
                    NativeMethods.BNGetLanguageRepresentationFunctionTypeParser(
                        this.handle
                    )
                );
            }
        }

        /// <summary>Gets the associated type printer.</summary>
        public virtual TypePrinter? Printer
        {
            get
            {
                if (this.custom)
                {
                    return null;
                }

                return TypePrinter.BorrowHandle(
                    NativeMethods.BNGetLanguageRepresentationFunctionTypePrinter(
                        this.handle
                    )
                );
            }
        }

        /// <summary>Registers this custom representation type.</summary>
        public void Register()
        {
            if (!this.custom)
            {
                throw new InvalidOperationException(
                    "Core representation types cannot be registered again."
                );
            }

            if (this.registered || !this.IsInvalid)
            {
                throw new InvalidOperationException(
                    "The language representation type is already registered."
                );
            }

            this.RegisterCustomType(this.registrationName ?? string.Empty);
            this.registered = true;
        }

        /// <summary>Creates a language representation for one high-level IL function.</summary>
        public abstract LanguageRepresentationFunction? Create(
            Architecture architecture,
            Function owner,
            HighLevelILFunction highLevelIL
        );

        /// <summary>Determines whether this type applies to a view.</summary>
        public virtual bool IsValidFor(BinaryView view)
        {
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (this.custom)
            {
                return true;
            }

            return NativeMethods.BNIsLanguageRepresentationFunctionTypeValid(
                this.handle,
                view.DangerousGetHandle()
            );
        }

        /// <summary>Gets function declaration lines for this language.</summary>
        public virtual unsafe DisassemblyTextLine[] GetFunctionTypeTokens(
            Function function,
            DisassemblySettings? settings = null
        )
        {
            if (this.custom)
            {
                return Array.Empty<DisassemblyTextLine>();
            }

            ulong count = 0;
            IntPtr lines =
                NativeMethods.BNGetLanguageRepresentationFunctionTypeFunctionTypeTokens(
                    this.handle,
                    function.DangerousGetHandle(),
                    null == settings
                        ? IntPtr.Zero
                        : settings.DangerousGetHandle(),
                    (IntPtr)(&count)
                );

            return UnsafeUtils.TakeStructArrayEx<
                BNDisassemblyTextLine,
                DisassemblyTextLine
            >(
                lines,
                count,
                DisassemblyTextLine.FromNative,
                NativeMethods.BNFreeDisassemblyTextLines
            );
        }

        /// <summary>Looks up a registered representation type by name.</summary>
        public static LanguageRepresentationFunctionType? GetByName(string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return LanguageRepresentationFunctionType.BorrowHandle(
                NativeMethods.BNGetLanguageRepresentationFunctionTypeByName(name)
            );
        }

        /// <summary>Checks a named representation type against a view.</summary>
        public static bool IsValidByName(string name, BinaryView view)
        {
            using LanguageRepresentationFunctionType? type =
                LanguageRepresentationFunctionType.GetByName(name);

            return null != type && type.IsValidFor(view);
        }

        /// <summary>Gets every registered representation type.</summary>
        public static LanguageRepresentationFunctionType[] GetList()
        {
            IntPtr types =
                NativeMethods.BNGetLanguageRepresentationFunctionTypeList(
                    out ulong count
                );

            return UnsafeUtils.TakeHandleArray<LanguageRepresentationFunctionType>(
                types,
                count,
                LanguageRepresentationFunctionType.MustBorrowHandle,
                NativeMethods.BNFreeLanguageRepresentationFunctionTypeList
            );
        }

        protected override bool ReleaseHandle()
        {
            return true;
        }

        private sealed class CoreLanguageRepresentationFunctionType :
            LanguageRepresentationFunctionType
        {
            internal CoreLanguageRepresentationFunctionType(IntPtr handle)
                : base(handle)
            {
            }

            public override LanguageRepresentationFunction? Create(
                Architecture architecture,
                Function owner,
                HighLevelILFunction highLevelIL
            )
            {
                return LanguageRepresentationFunction.TakeHandle(
                    NativeMethods.BNCreateLanguageRepresentationFunction(
                        this.handle,
                        architecture.DangerousGetHandle(),
                        owner.DangerousGetHandle(),
                        highLevelIL.DangerousGetHandle()
                    )
                );
            }
        }
    }
}
