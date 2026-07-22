using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryNinja
{
    // Hand-written, durable object-navigation conveniences that mirror the ergonomic
    // navigation surface of the official Python binding over the types this binding
    // already exposes. This file lives at the repository root (NOT inside the generated
    // Handle/Type/Struct tree) on purpose: the generator regenerates the wrapper classes
    // from the upstream C++ header on every resync, so anything added there is wiped,
    // while a root file like UnsafeUtils.cs survives.
    //
    // Every member here is either pure C# composition over existing, working accessors or
    // a thin wrapper over a P/Invoke whose export is already present in the installed core,
    // so each one is fully end-to-end validatable. No edits are made to generated files.

    // ---- BNDerivedString interop (binaryninjacore.h :1830 / :1837) -------------------------
    //
    // The generator emits BNGetDerivedStrings with an opaque IntPtr for BNDerivedString, so
    // defining the struct here does not collide with generated interop. Field offsets are
    // pinned with LayoutKind.Explicit to reproduce the C ABI byte-for-byte; the non-obvious
    // case is BNDerivedString, where the embedded `bool locationValid` is one byte and the
    // following BNDerivedStringLocation is 8-byte aligned, so `location` lands at offset 16
    // (7 bytes of padding after the bool). A runtime probe asserts the managed size matches
    // the C size (BNDerivedString == 48, BNDerivedStringLocation == 24) to lock the layout.

    [StructLayout(LayoutKind.Explicit)]
    internal struct BNDerivedStringLocation
    {
        // BNDerivedStringLocationType (C enum = int, 4 bytes); 4 bytes pad before addr.
        [FieldOffset(0)]
        public int locationType;

        // uint64_t addr
        [FieldOffset(8)]
        public ulong addr;

        // uint64_t len
        [FieldOffset(16)]
        public ulong len;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct BNDerivedString
    {
        // BNStringRef* value
        [FieldOffset(0)]
        public IntPtr value;

        // bool locationValid (C _Bool, 1 byte); 7 bytes pad before the aligned `location`.
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.I1)]
        public bool locationValid;

        // BNDerivedStringLocation location (8-byte aligned -> offset 16)
        [FieldOffset(16)]
        public BNDerivedStringLocation location;

        // BNCustomStringType* customType
        [FieldOffset(40)]
        public IntPtr customType;
    }

    // Managed projection of a BNDerivedString. The text is read eagerly (via
    // BNGetStringRefContents, decoded UTF-8) because the caller frees the native array,
    // including its embedded BNStringRef* values, immediately after marshaling (Python passes
    // owned = False to BNFreeDerivedStringList). The location is surfaced only when
    // locationValid is true, matching the Python DerivedString contract.
    public sealed class DerivedString
    {
        public string Value { get; }

        public bool LocationValid { get; }

        public DerivedStringLocationType LocationType { get; }

        public ulong Address { get; }

        public ulong Length { get; }

        /// <summary>
        /// Constructs a value-only DerivedString with no location, mirroring Python
        /// <c>DerivedString(value=..., location=None, custom_type=None)</c> for the common case of
        /// attaching a derived string via
        /// <see cref="HighLevelILFunction.SetDerivedStringReferenceForExpr"/>.
        /// </summary>
        public DerivedString(string value)
            : this(value, false, 0, 0UL, 0UL)
        {
        }

        private DerivedString(
            string value,
            bool locationValid,
            int locationType,
            ulong addr,
            ulong len)
        {
            this.Value = value;
            this.LocationValid = locationValid;
            this.LocationType = (DerivedStringLocationType)locationType;
            this.Address = addr;
            this.Length = len;
        }

        // Reads one BNDerivedString out of a by-value native array. Must run before the array
        // (and its BNStringRef* values) is freed; TakeStructArrayEx calls this per element
        // before invoking the array free callback, so the order is safe.
        internal static DerivedString FromNative(BNDerivedString raw)
        {
            string value = ReadStringRef(raw.value);

            return new DerivedString(
                value,
                raw.locationValid,
                raw.location.locationType,
                raw.location.addr,
                raw.location.len
            );
        }

        // Re-materializes this projection as a native BNDerivedString for the Set path
        // (BNSetHighLevelILDerivedStringReferenceForExpr). The Value text is rebuilt as a fresh
        // BNStringRef* because this projection reads eagerly and does not retain the original core
        // handle. The caller owns that BNStringRef* and must free it (BNFreeStringRef) once the
        // core has consumed the struct: core copies/add-refs the string, mirroring Python's
        // owned=False borrow. custom_type is not surfaced by this read-only projection (Python
        // CustomStringType handle is dropped), so it is left null.
        internal BNDerivedString ToNativeEx()
        {
            ulong byteCount = (ulong)Encoding.UTF8.GetByteCount(this.Value);
            IntPtr stringRef = NativeMethods.BNCreateStringRefOfLength(
                this.Value,
                (UIntPtr)byteCount
            );

            BNDerivedString native = new BNDerivedString();
            native.value = stringRef;
            native.locationValid = this.LocationValid;

            if (this.LocationValid)
            {
                native.location.locationType = (int)this.LocationType;
                native.location.addr = this.Address;
                native.location.len = this.Length;
            }

            native.customType = IntPtr.Zero;

            return native;
        }

        // Decodes a BNStringRef* as a UTF-8 string. The core hands back a NUL-terminated C
        // string; UTF-8 decoding covers the non-ASCII case the rest of the binding targets.
        private static string ReadStringRef(IntPtr reference)
        {
            if (IntPtr.Zero == reference)
            {
                return string.Empty;
            }

            IntPtr contents = NativeMethods.BNGetStringRefContents(reference);

            return UnsafeUtils.ReadUtf8String(contents);
        }
    }

    // The navigation conveniences themselves. Grouped by the type they extend.
    public static class NavigationExtensions
    {
        // ---- Enumeration / EnumerationBuilder ----------------------------------------------

        /// <summary>
        /// The first enumeration member whose <see cref="EnumerationMember.Name"/> equals
        /// <paramref name="name"/>, or <c>null</c> when no such member exists. Mirrors the
        /// string branch of Python <c>Enumeration.__getitem__</c> over
        /// <see cref="Enumeration.Members"/>.
        /// </summary>
        public static EnumerationMember? GetMemberByName(this Enumeration enumeration, string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            foreach (EnumerationMember member in enumeration.Members)
            {
                if (member.Name == name)
                {
                    return member;
                }
            }

            return null;
        }

        /// <summary>
        /// The first enumeration member whose <see cref="EnumerationMember.Value"/> equals
        /// <paramref name="value"/>, or <c>null</c> when no such member exists. A C#
        /// ergonomic companion to <see cref="GetMemberByName"/> (Python exposes name and
        /// index lookup only, not value lookup).
        /// </summary>
        public static EnumerationMember? GetMemberByValue(this Enumeration enumeration, ulong value)
        {
            foreach (EnumerationMember member in enumeration.Members)
            {
                if (member.Value == value)
                {
                    return member;
                }
            }

            return null;
        }

        /// <summary>
        /// The first builder member whose <see cref="EnumerationMember.Name"/> equals
        /// <paramref name="name"/>, or <c>null</c>. Mirrors Python
        /// <c>EnumerationBuilder.__getitem__</c> over <see cref="EnumerationBuilder.Members"/>.
        /// </summary>
        public static EnumerationMember? GetMemberByName(this EnumerationBuilder builder, string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            foreach (EnumerationMember member in builder.Members)
            {
                if (member.Name == name)
                {
                    return member;
                }
            }

            return null;
        }

        /// <summary>
        /// The first builder member whose <see cref="EnumerationMember.Value"/> equals
        /// <paramref name="value"/>, or <c>null</c>. C# ergonomic companion to
        /// <see cref="GetMemberByName(EnumerationBuilder, string)"/>.
        /// </summary>
        public static EnumerationMember? GetMemberByValue(this EnumerationBuilder builder, ulong value)
        {
            foreach (EnumerationMember member in builder.Members)
            {
                if (member.Value == value)
                {
                    return member;
                }
            }

            return null;
        }

        // ---- StructureType -----------------------------------------------------------------

        /// <summary>
        /// The structure member with the given name, or <c>null</c>. Forwards the underlying
        /// <see cref="Structure.GetMemberByName"/> (Python <c>StructureType.__getitem__</c>)
        /// so callers need not drop to <see cref="StructureType.Structure"/>. The
        /// offset-based lookup already exists as <see cref="StructureType.MemberAtOffset"/>.
        /// </summary>
        public static StructureMember? GetMemberByName(this StructureType structureType, string name)
        {
            return structureType.Structure.GetMemberByName(name);
        }

        // ---- Type --------------------------------------------------------------------------

        /// <summary>
        /// The child types of this type, mirroring Python <c>Type.children</c>: a pointer
        /// yields its target, an array its element type, a function its return type followed
        /// by each parameter type, and a structure each member type. Leaf classes (void,
        /// integer, float, enumeration, named-type-reference, ...) yield an empty array.
        /// </summary>
        /// <remarks>
        /// Each returned <see cref="Type"/> is an independently owned reference (the accessors
        /// take a fresh <c>BNNewTypeReference</c>), so the caller is responsible for disposing
        /// them, exactly as for <see cref="BinaryView.Types"/>.
        /// </remarks>
        public static BinaryNinja.Type[] GetChildren(this BinaryNinja.Type type)
        {
            List<BinaryNinja.Type> children = new List<BinaryNinja.Type>();

            switch (type.TypeClass)
            {
                case TypeClass.PointerTypeClass:
                {
                    using (PointerType? pointer = type.AsPointer())
                    {
                        if (null != pointer)
                        {
                            children.Add(pointer.Target.Type);
                        }
                    }
                    break;
                }
                case TypeClass.ArrayTypeClass:
                {
                    using (ArrayType? array = type.AsArray())
                    {
                        if (null != array)
                        {
                            children.Add(array.ElementType.Type);
                        }
                    }
                    break;
                }
                case TypeClass.FunctionTypeClass:
                {
                    using (FunctionType? function = type.AsFunction())
                    {
                        if (null != function)
                        {
                            children.Add(function.ReturnType.Type);

                            foreach (FunctionParameter parameter in function.Parameters)
                            {
                                children.Add(parameter.Type);
                            }
                        }
                    }
                    break;
                }
                case TypeClass.StructureTypeClass:
                {
                    using (StructureType? structure = type.AsStructure())
                    {
                        if (null != structure)
                        {
                            foreach (StructureMember member in structure.Members)
                            {
                                children.Add(member.Type);
                            }
                        }
                    }
                    break;
                }
                default:
                {
                    // Void/Bool/Char/WideChar/Integer/Float/Enumeration/NamedTypeReference are
                    // leaves: they carry no child types (matches Python Type.children base case).
                    break;
                }
            }

            return children.ToArray();
        }

        // ---- Function ----------------------------------------------------------------------

        /// <summary>
        /// Every Low Level IL exit target of the instruction starting at
        /// <paramref name="address"/> under <paramref name="arch"/> (or the function's own
        /// architecture when null). Composes
        /// <see cref="LowLevelILFunction.GetInstructionStart"/> with
        /// <see cref="LowLevelILFunction.GetExitsForInstruction"/>; mirrors Python
        /// <c>Function.get_low_level_il_exits_at</c>. Returns an empty array when the
        /// function has no LLIL or no instruction starts at the address.
        /// </summary>
        public static LowLevelILInstruction[] GetLowLevelILExitsAt(
            this Function function,
            ulong address,
            Architecture? arch = null)
        {
            LowLevelILFunction? llil = function.GetLowLevelIL();

            if (null == llil)
            {
                return Array.Empty<LowLevelILInstruction>();
            }

            LowLevelILInstruction? instruction = llil.GetInstructionStart(address, arch);

            if (null == instruction)
            {
                return Array.Empty<LowLevelILInstruction>();
            }

            return llil.GetExitsForInstruction(instruction.InstructionIndex);
        }

        /// <summary>
        /// Whether any basic block of <paramref name="function"/> spans
        /// <paramref name="address"/> (<c>block.Start &lt;= address &lt; block.End</c>). Mirrors
        /// the <c>int</c> branch of Python <c>Function.__contains__</c> (function.py:497).
        /// </summary>
        public static bool Contains(this Function function, ulong address)
        {
            foreach (BasicBlock block in function.BasicBlocks)
            {
                if (block.Start <= address && address < block.End)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Whether <paramref name="block"/> belongs to <paramref name="function"/>, by handle
        /// identity of its owning function. Mirrors the <c>BasicBlock</c> branch of Python
        /// <c>Function.__contains__</c> (function.py:497).
        /// </summary>
        public static bool Contains(this Function function, BasicBlock block)
        {
            if (null == block)
            {
                return false;
            }

            return block.Function == function;
        }

        /// <summary>
        /// Every disassembly instruction line across all of the function's basic blocks, in
        /// block-then-instruction order. Mirrors Python <c>Function.instructions</c>
        /// (function.py:1580); yields the per-instruction token lines (the lightweight
        /// disassembly-only iterator, distinct from the heavier linear-disassembly walk).
        /// Lazily evaluated.
        /// </summary>
        public static System.Collections.Generic.IEnumerable<InstructionTextLine> GetInstructions(
            this Function function)
        {
            foreach (BasicBlock block in function.BasicBlocks)
            {
                foreach (InstructionTextLine line in block.InstructionTextLines)
                {
                    yield return line;
                }
            }
        }

        // ---- BasicBlock --------------------------------------------------------------------

        /// <summary>
        /// Whether <paramref name="address"/> falls inside this block's half-open span
        /// <c>[Start, End)</c>. Mirrors Python <c>BasicBlock.__contains__</c>
        /// (basicblock.py:224).
        /// </summary>
        public static bool Contains(this BasicBlock block, ulong address)
        {
            return block.Start <= address && address < block.End;
        }

        /// <summary>
        /// The <paramref name="index"/>-th disassembly instruction line of this block, or
        /// <c>null</c> when <paramref name="index"/> is out of range. Mirrors Python
        /// <c>BasicBlock.__getitem__</c> (basicblock.py:210), returning <c>null</c> on a miss
        /// rather than raising, matching the navigation-style lookups elsewhere in this layer.
        /// </summary>
        public static InstructionTextLine? GetInstructionByIndex(this BasicBlock block, int index)
        {
            if (index < 0)
            {
                return null;
            }

            int current = 0;

            foreach (InstructionTextLine line in block.InstructionTextLines)
            {
                if (current == index)
                {
                    return line;
                }

                current++;
            }

            return null;
        }

        // ---- BinaryView --------------------------------------------------------------------

        /// <summary>
        /// The derived strings the analysis produced for this view, mirroring Python
        /// <c>BinaryView.get_derived_strings</c> (C++ <c>BinaryView::GetDerivedStrings</c>).
        /// </summary>
        /// <remarks>
        /// The <c>BNGetDerivedStrings</c> stub declares its count out-parameter as
        /// <c>IntPtr</c> (<c>size_t*</c>), so the count is read back from a native slot rather
        /// than via <c>out</c>. <c>BNFreeDerivedStringList</c> frees the array and its embedded
        /// <c>BNStringRef*</c> values, so <see cref="DerivedString"/> must not free them.
        /// </remarks>
        public static DerivedString[] GetDerivedStrings(this BinaryView view)
        {
            VerifyDerivedStringLayout();

            IntPtr countSlot = Marshal.AllocHGlobal(IntPtr.Size);

            try
            {
                IntPtr head = NativeMethods.BNGetDerivedStrings(
                    view.DangerousGetHandle(),
                    countSlot
                );

                ulong count = (ulong)Marshal.ReadInt64(countSlot);

                return UnsafeUtils.TakeStructArrayEx<BNDerivedString, DerivedString>(
                    head,
                    count,
                    DerivedString.FromNative,
                    FreeDerivedStringList
                );
            }
            finally
            {
                Marshal.FreeHGlobal(countSlot);
            }
        }

		/// <summary>
		/// Returns code references associated with an analyzed derived string.
		/// </summary>
		public static ReferenceSource[] GetDerivedStringCodeReferences(
			this BinaryView view,
			DerivedString derivedString,
			ulong? maxItems = null
		)
		{
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			if (null == derivedString)
			{
				throw new ArgumentNullException(nameof(derivedString));
			}

			BNDerivedString native = derivedString.ToNativeEx();
			try
			{
				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					IntPtr references = NativeMethods.BNGetDerivedStringCodeReferences(
						view.DangerousGetHandle(),
						allocator.AllocStruct(native),
						out UIntPtr countValue,
						maxItems.HasValue,
						new UIntPtr(maxItems.GetValueOrDefault())
					);

					return UnsafeUtils.TakeStructArrayEx<BNReferenceSource, ReferenceSource>(
						references,
						countValue.ToUInt64(),
						ReferenceSource.FromNative,
						NativeMethods.BNFreeCodeReferences
					);
				}
			}
			finally
			{
				if (IntPtr.Zero != native.value)
				{
					NativeMethods.BNFreeStringRef(native.value);
				}
			}
		}

        // Adapts BNFreeDerivedStringList (which takes a UIntPtr count) to the
        // Action<IntPtr, ulong> signature TakeStructArrayEx expects. Named method, not a
        // lambda, per the project's C# style rules.
        private static void FreeDerivedStringList(IntPtr head, ulong count)
        {
            NativeMethods.BNFreeDerivedStringList(head, (UIntPtr)count);
        }

        // One-shot check that the managed BNDerivedString layout reproduces the C ABI from
        // binaryninjacore.h (48 bytes for BNDerivedString, 24 for BNDerivedStringLocation).
        // The bool-padding offset is the one non-obvious case, so this locks it rather than
        // assuming the [FieldOffset] values are correct. Runs once per process.
        private static int s_derivedStringLayoutSize = -1;

        private static void VerifyDerivedStringLayout()
        {
            if (s_derivedStringLayoutSize >= 0)
            {
                return;
            }

            int outerSize = Marshal.SizeOf<BNDerivedString>();
            int innerSize = Marshal.SizeOf<BNDerivedStringLocation>();

            if (48 != outerSize || 24 != innerSize)
            {
                throw new InvalidOperationException(
                    "BNDerivedString interop layout mismatch: outer=" + outerSize
                    + " (expected 48), inner=" + innerSize + " (expected 24)");
            }

            s_derivedStringLayoutSize = outerSize;
        }

        /// <summary>
        /// The function starting at <paramref name="address"/> whose platform matches
        /// <paramref name="platform"/> (defaulting to the view's
        /// <see cref="BinaryView.DefaultPlatform"/>), falling back to the first function at
        /// that address when no platform match exists, or <c>null</c> when none starts there.
        /// Mirrors Python <c>BinaryView.get_function_at(addr, plat=None)</c>
        /// (binaryview.py:5832) — the single most-used lookup in the Python surface, which the
        /// generated <see cref="BinaryView.GetFunctionByAddress"/> does not cover because it
        /// requires an explicit platform.
        /// </summary>
        public static Function? GetFunctionAt(
            this BinaryView view,
            ulong address,
            Platform? platform = null)
        {
            Function[] candidates = view.GetFunctionsForAddress(address);

            if (0 == candidates.Length)
            {
                return null;
            }

            // Match the requested platform (or the view default) first.
            Platform? resolved = platform;
            if (null == resolved)
            {
                resolved = view.DefaultPlatform;
            }

            foreach (Function function in candidates)
            {
                if (function.Platform == resolved)
                {
                    return function;
                }
            }

            // Implicit-platform lookups fall back to the first candidate, exactly as Python does
            // when plat is None (get_function_at falls back to funcs[0]). An explicit platform
            // with no match returns null: Python's plat-is-not-None branch is a strict
            // BNGetAnalysisFunction(view, plat, addr) lookup that returns None when no function on
            // that platform sits at the address -- it must NOT silently return a different
            // platform's function.
            if (null == platform)
            {
                return candidates[0];
            }

            return null;
        }

        /// <summary>
        /// Whether <paramref name="address"/> lies inside any segment of the view. Mirrors
        /// Python <c>BinaryView.__contains__</c> (binaryview.py:3316).
        /// </summary>
        public static bool Contains(this BinaryView view, ulong address)
        {
            return null != view.GetSegmentAt(address);
        }

        /// <summary>
        /// The ASCII string that begins at <paramref name="address"/>, regardless of whether
        /// analysis flagged it as a string, or <c>null</c> when no qualifying string is found.
        /// Mirrors Python <c>BinaryView.get_ascii_string_at</c> (binaryview.py:8023): an ASCII byte
        /// is any value in 0x01..0x7f (control characters such as tab and newline are included),
        /// <paramref name="requireCstring"/> defaults to requiring a NUL terminator, and an address
        /// outside the view yields <c>null</c>. Read the returned bytes with
        /// <see cref="BinaryView.ReadData"/>(<see cref="StringReference.Start"/>,
        /// <see cref="StringReference.Length"/>).
        /// </summary>
        public static StringReference? GetAsciiStringAt(
            this BinaryView view,
            ulong address,
            ulong minLength = 4,
            ulong? maxLength = null,
            bool requireCstring = true)
        {
            // 1. Bounds: an address outside the view has no string (Python: addr < start or
            // addr >= end returns None).
            if (address < view.Start || address >= view.End)
            {
                return null;
            }

            // 2. Walk bytes lazily from the address. The predicate is 0 < c <= 0x7f -- any non-NUL
            // ASCII byte -- matching Python. Printable-only (0x20..0x7e) would wrongly truncate runs
            // that contain control characters such as tab (0x09) or newline (0x0a).
            using BinaryReader reader = new BinaryReader(view);
            reader.Position = address;

            ulong length = 0;
            byte? current = reader.ReadByte();

            while (current.HasValue && 0 < current.Value && current.Value <= 0x7f)
            {
                // maxLength caps the run length; when null (unset) the run is uncapped.
                if (maxLength.HasValue && length == maxLength.Value)
                {
                    break;
                }

                length++;
                current = reader.ReadByte();
            }

            // 3. Minimum-length gate.
            if (length < minLength)
            {
                return null;
            }

            // 4. requireCstring: the run must end on a NUL byte. EOF, or any non-NUL terminator
            // (a byte > 0x7f), means the run is not a C string.
            bool terminatorIsNotNul = !current.HasValue || 0 != current.Value;

            if (requireCstring && terminatorIsNotNul)
            {
                return null;
            }

            return new StringReference(
                view,
                StringType.AsciiString,
                address,
                length);
        }

        /// <summary>
        /// The raw bytes of this string reference, mirroring Python <c>StringReference.raw</c>
        /// (binaryview.py:514): reads <see cref="StringReference.Length"/> bytes at
        /// <see cref="StringReference.Start"/> from <paramref name="view"/>. This compatibility
        /// overload remains for manually constructed references; core-produced references expose
        /// the official-style <see cref="StringReference.Raw"/> property directly.
        /// </summary>
        public static byte[] GetRaw(this StringReference stringReference, BinaryView view)
        {
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            return view.ReadData(stringReference.Start, stringReference.Length);
        }

        /// <summary>
        /// The decoded text of this string reference, mirroring Python <c>StringReference.value</c>
        /// (binaryview.py:510): reads the bytes via <see cref="GetRaw"/> and decodes them according
        /// to <see cref="StringReference.Type"/>. Undecodable bytes are replaced rather than
        /// thrown, matching Python <c>errors='replace'</c>.
        /// </summary>
        public static string GetValue(this StringReference stringReference, BinaryView view)
        {
            byte[] raw = GetRaw(stringReference, view);

            return StringReference.Decode(raw, stringReference.Type);
        }

        /// <summary>
        /// Every disassembly instruction line across the whole view, in
        /// view-block-then-instruction order. Mirrors Python <c>BinaryView.instructions</c>
        /// (binaryview.py:3600); lazily evaluated.
        /// </summary>
        public static System.Collections.Generic.IEnumerable<InstructionTextLine> GetInstructions(
            this BinaryView view)
        {
            foreach (BasicBlock block in view.BasicBlocks)
            {
                foreach (InstructionTextLine line in block.InstructionTextLines)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// Every Low Level IL instruction across the whole view. Mirrors Python
        /// <c>BinaryView.llil_instructions</c> (binaryview.py:3609); lazily evaluated.
        /// </summary>
        public static System.Collections.Generic.IEnumerable<LowLevelILInstruction>
            GetLowLevelILInstructions(this BinaryView view)
        {
            foreach (LowLevelILFunction function in view.LowLevelILFunctions)
            {
                foreach (LowLevelILInstruction instruction in function.Instructions)
                {
                    yield return instruction;
                }
            }
        }

        /// <summary>
        /// Every Medium Level IL instruction across the whole view. Mirrors Python
        /// <c>BinaryView.mlil_instructions</c> (binaryview.py:3615); lazily evaluated.
        /// </summary>
        public static System.Collections.Generic.IEnumerable<MediumLevelILInstruction>
            GetMediumLevelILInstructions(this BinaryView view)
        {
            foreach (MediumLevelILFunction function in view.MediumLevelILFunctions)
            {
                foreach (MediumLevelILInstruction instruction in function.Instructions)
                {
                    yield return instruction;
                }
            }
        }

        /// <summary>
        /// Every High Level IL instruction across the whole view. Mirrors Python
        /// <c>BinaryView.hlil_instructions</c> (binaryview.py:3621); lazily evaluated.
        /// </summary>
        public static System.Collections.Generic.IEnumerable<HighLevelILInstruction>
            GetHighLevelILInstructions(this BinaryView view)
        {
            foreach (HighLevelILFunction function in view.HighLevelILFunctions)
            {
                foreach (HighLevelILInstruction instruction in function.Instructions)
                {
                    yield return instruction;
                }
            }
        }
    }
}
