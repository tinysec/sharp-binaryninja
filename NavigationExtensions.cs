using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
    }
}
