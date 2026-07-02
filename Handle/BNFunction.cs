using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Function : AbstractSafeHandle<Function>
	{
	    internal Function(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static Function? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Function(
			    NativeMethods.BNNewFunctionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Function MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Function(
			    NativeMethods.BNNewFunctionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Function? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Function(handle, true);
	    }
	    
	    internal static Function MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Function(handle, true);
	    }
	    
	    internal static Function? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Function(handle, false);
	    }
	    
	    internal static Function MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Function(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeFunction(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        return true;
	    }
	    

	    /// <summary>
	    /// BinaryView that contains this function ?
	    /// </summary>
	    public BinaryView View
	    {
		    get
		    {
			    return BinaryView.MustTakeHandle(
				    NativeMethods.BNGetFunctionData(this.handle)
				);
		    }
	    }

	    /// <summary>
	    /// Components in the component tree that contain this function.
	    /// Mirrors Python <c>Function.components</c> / C++ <c>Function::GetParentComponents</c>.
	    /// </summary>
	    public Component[] Components
	    {
		    get
		    {
			    return this.View.GetFunctionParentComponents(this);
		    }
	    }

	    public Architecture Architecture
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetFunctionArchitecture(this.handle);

			    if (IntPtr.Zero == raw)
			    {
				    throw new NoNullAllowedException("function must have a architecture");
			    }
			    
			    return new Architecture(raw);
		    }
	    }

	    // Resolves an optional architecture argument to a native BNArchitecture* handle,
	    // defaulting to this function's own architecture when the caller does not specify one.
	    // Core APIs that take a BNArchitecture* dereference it, so an unspecified architecture
	    // must resolve to a valid handle rather than a null pointer -- passing IntPtr.Zero here
	    // crashes the core. The Architecture wrapper is non-owning, so returning its raw handle
	    // is safe.
	    private IntPtr ArchHandleOrDefault(Architecture? arch)
	    {
		    if (null != arch)
		    {
			    return arch.DangerousGetHandle();
		    }

		    return this.Architecture.DangerousGetHandle();
	    }

	    public Platform? Platform
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetFunctionPlatform(this.handle);

			    if (IntPtr.Zero == raw)
			    {
				    return null;
			    }
			    
			    return new Platform(raw, true);
		    }
	    }

	    public ulong Start
	    {
		    get
		    {
			    return NativeMethods.BNGetFunctionStart(this.handle);
		    }
	    }

	    public ulong HighestAddress
	    {
		    get
		    {
			    return NativeMethods.BNGetFunctionHighestAddress(this.handle);
		    }
	    }

	    public ulong LowestAddress
	    {
		    get
		    {
			    return NativeMethods.BNGetFunctionLowestAddress(this.handle);
		    }
	    }

	    public AddressRange[] AddressRanges
	    {
		    get
		    {
			    ulong arrayLength = 0;
			    
			    IntPtr arrayPointer = NativeMethods.BNGetFunctionAddressRanges(this.handle , out arrayLength);

			    return UnsafeUtils.TakeStructArray<BNAddressRange , AddressRange>(
				    arrayPointer ,
				    arrayLength ,
				    AddressRange.FromNative ,
				    NativeMethods.BNFreeAddressRanges
			    );
		    }
	    }

	    public Symbol Symbol
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetFunctionSymbol(this.handle);

			    if (IntPtr.Zero == raw)
			    {
				    throw new NoNullAllowedException("function must have a symbol");
			    }

			    return new Symbol(raw , true);
		    }
	    }

	    public ulong Address
	    {
		    get
		    {
			    return this.Symbol.Address;
		    }
	    }

	    public string ShortName
	    {
		    get
		    {
			    return this.Symbol.ShortName;
		    }
	    }
	    
	    public string FullName
	    {
		    get
		    {
			    return this.Symbol.FullName;
		    }
	    }
	    
	    public string RawName
	    {
		    get
		    {
			    return this.Symbol.RawName;
		    }
	    }

	    public bool WasAutomaticallyDiscovered
	    {
		    get
		    {
			    return NativeMethods.BNWasFunctionAutomaticallyDiscovered(this.handle);
		    }
	    }

	    public bool HasUserAnnotations
	    {
		    get
		    {
			    return NativeMethods.BNFunctionHasUserAnnotations(this.handle);
		    }
	    }

	    public BoolWithConfidence CanFunctionReturn
	    {
		    get
		    {
			    return BoolWithConfidence.FromNative( NativeMethods.BNCanFunctionReturn(this.handle) );
		    }

		    set
		    {
			    NativeMethods.BNSetUserFunctionCanReturn(this.handle,  value.ToNative() );
		    }
	    }

	    public BoolWithConfidence IsPure
	    {
		    get
		    {
			    return BoolWithConfidence.FromNative( NativeMethods.BNIsFunctionPure(this.handle));
		    }
		    
		    set
		    {
			    NativeMethods.BNSetUserFunctionPure(this.handle, value.ToNative() );
		    }
	    }
		
	    public void SetAutoType(BinaryNinja.Type type)
	    {
		    NativeMethods.BNSetFunctionAutoType(this.handle , type.DangerousGetHandle());
	    }
	    
	    public void SetUserType(BinaryNinja.Type type)
	    {
		    NativeMethods.BNSetFunctionUserType(this.handle , type.DangerousGetHandle());
	    }

	    public bool HasUserType
	    {
		    get
		    {
			    return NativeMethods.BNFunctionHasUserType(this.handle);
		    }
	    }
	    
	    public bool HasExplicitlyDefinedType
	    {
		    get
		    {
			    return NativeMethods.BNFunctionHasExplicitlyDefinedType(this.handle);
		    }
	    }
	    
	    public bool UpdateNeeded
	    {
		    get
		    {
			    return NativeMethods.BNIsFunctionUpdateNeeded(this.handle);
		    }
	    }

	    public BasicBlock[] BasicBlocks
	    {
		    get
		    {
			    ulong arrayLength = 0;
			    
			    IntPtr arrayPointer = NativeMethods.BNGetFunctionBasicBlockList(this.handle, out arrayLength);

			    return UnsafeUtils.TakeHandleArrayEx<BasicBlock>(
				    arrayPointer ,
				    arrayLength ,
				    BasicBlock.MustNewFromHandle ,
				    NativeMethods.BNFreeBasicBlockList
			    );
		    }
	    }

	    public IReadOnlyDictionary<ulong , string> Comments
	    {
		    get
		    {
			    ulong addressCount = 0;

			    IntPtr addressArrayPointer = NativeMethods.BNGetCommentedAddresses(this.handle , out addressCount);

			    ulong[] addresses = UnsafeUtils.TakeNumberArray<ulong>(
				    addressArrayPointer ,
				    addressCount ,
				    NativeMethods.BNFreeAddressList
			    );
			    
			    Dictionary<ulong, string> comments = new Dictionary<ulong, string>();

			    foreach (ulong address in addresses)
			    {
				    comments[address] = GetCommentForAddress(address);
			    }

			    return comments;
		    }
	    }

	    public Tag[] GetAddressTags(Architecture arch , ulong address)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetAddressTags(
			    this.handle,
			    arch.DangerousGetHandle(),
			    address,
			    out arrayLength
			);

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetAutoAddressTags(Architecture arch , ulong address)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetAutoAddressTags(
			    this.handle,
			     arch.DangerousGetHandle(),
			    address,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetUserAddressTags(Architecture arch , ulong address)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetUserAddressTags(
			    this.handle,
			    arch.DangerousGetHandle(),
			    address,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetAddressTagsInRange(Architecture arch , ulong start , ulong end)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetAddressTagsInRange(
			    this.handle,
			     arch.DangerousGetHandle(),
			    start,
			    end,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetAutoAddressTagsInRange(Architecture arch , ulong start , ulong end)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetAutoAddressTagsInRange(
			    this.handle,
			    arch.DangerousGetHandle(),
			    start,
			    end,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetUserAddressTagsInRange(Architecture arch , ulong start , ulong end)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetUserAddressTagsInRange(
			    this.handle,
			    arch.DangerousGetHandle(),
			    start,
			    end,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetFunctionTags()
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetFunctionTags(
			    this.handle,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetAutoFunctionTags()
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetAutoFunctionTags(
			    this.handle,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetUserFunctionTags()
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetUserFunctionTags(
			    this.handle,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetFunctionTagsOfType(TagType kind)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetFunctionTagsOfType(
			    this.handle,
			    kind.DangerousGetHandle(),
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetAutoFunctionTagsOfType(TagType kind)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetAutoFunctionTagsOfType(
			    this.handle,
			    kind.DangerousGetHandle(),
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle,
			    NativeMethods.BNFreeTagList
		    );
	    }
	    
	    public Tag[] GetUserFunctionTagsOfType(TagType kind)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetUserFunctionTagsOfType(
			    this.handle,
			    kind.DangerousGetHandle(),
			    out arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    arrayPointer ,
			    arrayLength ,
			    Tag.MustNewFromHandle,
			    NativeMethods.BNFreeTagList
		    );
	    }

	    
	    public void AddAutoFunctionTag(Tag tag)
	    {
		   NativeMethods.BNAddAutoFunctionTag(this.handle, tag.DangerousGetHandle());
	    }
	    
	    public void AddUserFunctionTag(Tag tag)
	    {
		    NativeMethods.BNAddUserFunctionTag(this.handle, tag.DangerousGetHandle());
	    }
	    
	    public void AddAutoAddressTag(Architecture arch ,ulong address , Tag tag)
	    {
		    NativeMethods.BNAddAutoAddressTag(
			    this.handle, 
			    arch.DangerousGetHandle(),
			    address,
			    tag.DangerousGetHandle()
			);
	    }
	    
	    public void AddUserAddressTag(Architecture arch ,ulong address , Tag tag)
	    {
		    NativeMethods.BNAddUserAddressTag(
			    this.handle, 
			    arch.DangerousGetHandle(),
			    address,
			    tag.DangerousGetHandle()
		    );
	    }

	    public void RemoveAutoAddressTag(Architecture arch , ulong address , Tag tag)
	    {
		    NativeMethods.BNRemoveAutoAddressTag(
			    this.handle, 
			    arch.DangerousGetHandle(),
			    address ,
			    tag.DangerousGetHandle()
		    );
	    }
	    
	    public void RemoveAutoAddressTagsOfType(Architecture arch , ulong address , TagType kind)
	    {
		    NativeMethods.BNRemoveAutoAddressTagsOfType(
			    this.handle, 
			    arch.DangerousGetHandle(),
			    address ,
			    kind.DangerousGetHandle()
		    );
	    }
	    
	    public void RemoveUserAddressTag(Architecture arch , ulong address , Tag tag)
	    {
		    NativeMethods.BNRemoveUserAddressTag(
			    this.handle, 
			    arch.DangerousGetHandle(),
			    address ,
			    tag.DangerousGetHandle()
			);
	    }
	    
	    public void RemoveUserAddressTagsOfType(Architecture arch , ulong address , TagType kind)
	    {
		    NativeMethods.BNRemoveUserAddressTagsOfType(
			    this.handle, 
			    arch.DangerousGetHandle(),
			    address ,
			    kind.DangerousGetHandle()
		    );
	    }
	    
	    public void RemoveUserFunctionTag( Tag tag)
	    {
		    NativeMethods.BNRemoveUserFunctionTag(
			    this.handle, 
			    tag.DangerousGetHandle()
		    );
	    }
	    
	    public void RemoveUserFunctionTagsOfType( TagType kind)
	    {
		    NativeMethods.BNRemoveUserFunctionTagsOfType(
			    this.handle,
			    kind.DangerousGetHandle()
		    );
	    }

	    public LowLevelILFunction LowLevelIL
	    {
		    get
		    {
			    return LowLevelILFunction.MustTakeHandle(
				    NativeMethods.BNGetFunctionLowLevelIL(this.handle)
			    );
		    }
	    }
	    
	    public LowLevelILFunction? GetLowLevelIL()
	    {
		    
		    return LowLevelILFunction.TakeHandle(
			    NativeMethods.BNGetFunctionLowLevelIL(
				    this.handle
			    )
		    );
	    }
	    
	    public LowLevelILFunction? GetLowLevelILIfAvailable()
	    {
		    
		    return LowLevelILFunction.TakeHandle(
			    NativeMethods.BNGetFunctionLowLevelILIfAvailable(
				    this.handle
			    )
		    );
	    }
	    
	    public LowLevelILFunction LiftedIL
	    {
		    get
		    {
			    return LowLevelILFunction.MustTakeHandle(
				    NativeMethods.BNGetFunctionLiftedIL(this.handle)
			    );
		    }
	    }
	    
	    public LowLevelILFunction? GetLiftedILIfAvailable()
	    {
		    IntPtr raw = NativeMethods.BNGetFunctionLiftedILIfAvailable(this.handle);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }

		    return new LowLevelILFunction(raw , true);
	    }

	    /// <summary>
	    /// The Lifted IL for this function (generating variant), or <c>null</c> if unavailable.
	    /// Null-safe counterpart of <see cref="LiftedIL"/>, mirroring <see cref="GetLowLevelIL"/>.
	    /// </summary>
	    public LowLevelILFunction? GetLiftedIL()
	    {
		    return LowLevelILFunction.TakeHandle(
			    NativeMethods.BNGetFunctionLiftedIL(
				    this.handle
			    )
		    );
	    }

	    public MediumLevelILFunction MediumLevelIL
	    {
		    get
		    {
			    return MediumLevelILFunction.MustTakeHandle(
				    NativeMethods.BNGetFunctionMediumLevelIL(this.handle)
			    );
		    }
	    }
	    
	    public MediumLevelILFunction? GetMediumLevelIL()
	    {
		    return MediumLevelILFunction.TakeHandle(
			    NativeMethods.BNGetFunctionMediumLevelIL(this.handle)
		    );
	    }
	    
	    public MediumLevelILFunction? GetMediumLevelILIfAvailable()
	    {
		    return MediumLevelILFunction.TakeHandle(
			    NativeMethods.BNGetFunctionMediumLevelILIfAvailable(this.handle)
		    );
	    }
	    
	    public MediumLevelILFunction MappedMediumLevelIL
	    {
		    get
		    {
			    return MediumLevelILFunction.MustTakeHandle(
				    NativeMethods.BNGetFunctionMappedMediumLevelIL(this.handle)
			    );
		    }
	    }
	    
	    public MediumLevelILFunction? GetMappedMediumLevelILIfAvailable()
	    {
		    IntPtr raw = NativeMethods.BNGetFunctionMappedMediumLevelILIfAvailable(this.handle);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }

		    return new MediumLevelILFunction(raw , true);
	    }
	    
	    
	    public HighLevelILFunction HighLevelIL
	    {
		    get
		    {
			    return HighLevelILFunction.MustTakeHandle(
				    NativeMethods.BNGetFunctionHighLevelIL(this.handle)
			    );
		    }
	    }
	    
	    public HighLevelILFunction? GetHighLevelIL()
	    {
		    return HighLevelILFunction.TakeHandle(
			    NativeMethods.BNGetFunctionHighLevelIL(this.handle)
		    );
	    }
	    
	    public HighLevelILFunction? GetHighLevelILIfAvailable()
	    {
		    IntPtr raw = NativeMethods.BNGetFunctionHighLevelILIfAvailable(this.handle);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }

		    return new HighLevelILFunction(raw , true);
	    }
	    
	    public LanguageRepresentationFunction? GetLanguageRepresentation(string language = "Pseudo C")
	    {
		    return LanguageRepresentationFunction.TakeHandle(
			    NativeMethods.BNGetFunctionLanguageRepresentation(this.handle , language)
		    );
	    }
	    
	    public LanguageRepresentationFunction? GetLanguageRepresentationIfAvailable(string language = "Pseudo C")
	    {
		    IntPtr raw = NativeMethods.BNGetFunctionLanguageRepresentationIfAvailable(this.handle , language);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }
		  
		    return new LanguageRepresentationFunction(raw , true);
	    }
	    
	    public FunctionType Type
	    {
		    get
		    {
			    return BinaryNinja.FunctionType.MustTakeHandle(
				    NativeMethods.BNGetFunctionType(this.handle)
			    );
		    }
	    }

	    public VariableNameAndType[] StackLayout
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetStackLayout(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArrayEx<BNVariableNameAndType , VariableNameAndType>(
				    arrayPointer ,
				    arrayLength ,
				    VariableNameAndType.FromNative ,
				    NativeMethods.BNFreeVariableNameAndTypeList
			    );
		    }
	    }

	    public VariableNameAndType[] Variables
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetFunctionVariables(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArrayEx<BNVariableNameAndType , VariableNameAndType>(
				    arrayPointer ,
				    arrayLength ,
				    VariableNameAndType.FromNative,
				    NativeMethods.BNFreeVariableNameAndTypeList
			    );
		    }
	    }

	    public VariableNameAndType? GetVariableByName(string name)
	    {
		    foreach (VariableNameAndType variable in Variables)
		    {
			    if (variable.Name == name)
			    {
				    return variable;
			    }
		    }

		    return null;
	    }

	    public IndirectBranchInfo[] IndirectBranchs
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetIndirectBranches(
				    this.handle ,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeStructArray<BNIndirectBranchInfo , IndirectBranchInfo>(
				    arrayPointer ,
				    arrayLength ,
				    IndirectBranchInfo.FromNative ,
				    NativeMethods.BNFreeIndirectBranchList
			    );
		    }
	    }
	    
	    public ArchitectureAndAddress[] UnresolvedIndirectBranches
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetUnresolvedIndirectBranches(
				    this.handle ,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeStructArray<BNArchitectureAndAddress , ArchitectureAndAddress>(
				    arrayPointer ,
				    arrayLength ,
				    ArchitectureAndAddress.FromNative ,
				    NativeMethods.BNFreeArchitectureAndAddressList
			    );
		    }
	    }

	    public bool HasUnresolvedIndirectBranches
	    {
		    get
		    {
			    return NativeMethods.BNHasUnresolvedIndirectBranches(this.handle);
		    }
	    }
	    
	    public TypeWithConfidence ReturnType
	    {
		    get
		    {
			    return TypeWithConfidence.FromNative(
				    NativeMethods.BNGetFunctionReturnType(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNSetUserFunctionReturnType(
				    this.handle, 
				    value.ToNative()
				);
		    }
	    }

	    public CallingConventionWithConfidence CallingConvention
	    {
		    get
		    {
			    return CallingConventionWithConfidence.FromNative(
				    NativeMethods.BNGetFunctionCallingConvention(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNSetUserFunctionCallingConvention(
				    this.handle, 
				    value.ToNative()
				);
		    }
	    }

	    public ParameterVariablesWithConfidence ParameterVariables
	    {
		    get
		    {
			    return new ParameterVariablesWithConfidence(
				    this,
				    NativeMethods.BNGetFunctionParameterVariables(this.handle)
			    );
		    }
	    }
	    
	    public BoolWithConfidence HasVariableArguments
	    {
		    get
		    {
			    return BoolWithConfidence.FromNative(
				    NativeMethods.BNFunctionHasVariableArguments(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNSetUserFunctionHasVariableArguments(
				    this.handle, 
				    value.ToNative()
			    );
		    }
	    }

	    public string Comment
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetFunctionComment(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNSetFunctionComment(this.handle, value);
		    }
	    }

	    public bool TooLarge
	    {
		    get
		    {
			    return NativeMethods.BNIsFunctionTooLarge(this.handle);
		    }
	    }
	    
	    public bool AnalysisSkipped
	    {
		    get
		    {
			    return NativeMethods.BNIsFunctionAnalysisSkipped(this.handle);
		    }
	    }

	    public string GetCommentForAddress(ulong address)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetCommentForAddress(this.handle , address)
		    );
	    }

	    public void SetCommentForAddress(ulong address , string comment)
	    {
		    NativeMethods.BNSetCommentForAddress(this.handle , address, comment);
	    }

	    public void AddUserCodeReference(
		    ulong fromAddress , 
		    ulong toAddress,
		    Architecture? arch = null
		)
	    {
		    NativeMethods.BNAddUserCodeReference(
			    this.handle ,
			    this.ArchHandleOrDefault(arch) ,
			    fromAddress ,
			    toAddress
		    );
	    }
	    
	    public void RemoveUserCodeReference(
		    ulong fromAddress , 
		    ulong toAddress,
		    Architecture? arch = null
	    )
	    {
		    NativeMethods.BNRemoveUserCodeReference(
			    this.handle ,
			    this.ArchHandleOrDefault(arch) ,
			    fromAddress ,
			    toAddress
		    );
	    }
	    
	    public RegisterValue GetRegisterValueAtInstruction(
		    ulong address,
		    RegisterIndex reg,
		    Architecture? arch = null
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetRegisterValueAtInstruction(
				    this.handle ,
				    this.ArchHandleOrDefault(arch) ,
				    address ,
				    reg
			    )
		    );
	    }
	    
	    public RegisterValue GetRegisterValueAfterInstruction(
		    ulong address,
		    RegisterIndex reg,
		    Architecture? arch = null
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetRegisterValueAfterInstruction(
				    this.handle ,
				    this.ArchHandleOrDefault(arch) ,
				    address ,
				    reg
			    )
		    );
	    }
	    
	    
	    public RegisterValue GetStackContentsAtInstruction(
		    ulong address,
		    long offset,
		    ulong size,
		    Architecture? arch = null
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetStackContentsAtInstruction(
				    this.handle ,
				    this.ArchHandleOrDefault(arch) ,
				    address ,
				    offset,
				    size
			    )
		    );
	    }
	    
	    public RegisterValue GetStackContentsAfterInstruction(
		    ulong address,
		    long offset,
		    ulong size,
		    Architecture? arch = null
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetStackContentsAfterInstruction(
				    this.handle ,
				    this.ArchHandleOrDefault(arch) ,
				    address ,
				    offset,
				    size
			    )
		    );
	    }
	    
	    
	    public RegisterValue GetParameterValueAtInstruction(
		    ulong address,
		    ulong index,
		    BinaryNinja.Type? type,
		    Architecture? arch = null
		)
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetParameterValueAtInstruction(
				    this.handle ,
				    this.ArchHandleOrDefault(arch) ,
				    address ,
				    null == type ? IntPtr.Zero : type.DangerousGetHandle() ,
				    index
			    )
		    );
	    }
	    
	    public RegisterValue GetParameterValueAtLowLevelILInstruction(
		    ulong instructionIndex,
		    ulong index,
		    BinaryNinja.Type? type
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetParameterValueAtLowLevelILInstruction(
				    this.handle ,
				    instructionIndex,
				    null == type ? IntPtr.Zero : type.DangerousGetHandle() ,
				    index
			    )
		    );
	    }
	    
	    
	    
	    public void Analyze()
	    {
		    NativeMethods.BNAnalyzeFunction(this.handle);
	    }
	    
	    public void Reanalyze(FunctionUpdateType type = FunctionUpdateType.FullAutoFunctionUpdate)
	    {
		    NativeMethods.BNReanalyzeFunction(this.handle , type);
	    }

	    public BasicBlock? GetBasicBlockAtAddress(ulong address , Architecture? arch = null)
	    {
		    return BasicBlock.TakeHandle(
			    NativeMethods.BNGetFunctionBasicBlockAtAddress(
				    this.handle, 
				    this.ArchHandleOrDefault(arch),
				    address)
		    );
	    }
	    
	    public ReferenceSource[] CallSites
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetFunctionCallSites(
				    this.handle,
				    out ulong arrayLength
				);

			    return UnsafeUtils.TakeStructArrayEx<BNReferenceSource , ReferenceSource>(
				    arrayPointer ,
				    arrayLength ,
				    ReferenceSource.FromNative ,
				    NativeMethods.BNFreeCodeReferences
			    );
		    }
	    }

	    /// <summary>
	    /// The call sites that reference this function (i.e. the locations that call
	    /// <b>into</b> this function). Mirrors Python <c>Function.caller_sites</c>.
	    /// </summary>
	    public ReferenceSource[] CallerSites
	    {
		    get
		    {
			    return this.View.GetCallers(this.Start);
		    }
	    }

	    /// <summary>
	    /// The functions that call this function. Mirrors Python <c>Function.callers</c>.
	    /// </summary>
	    public Function[] Callers
	    {
		    get
		    {
			    Dictionary<ulong ,Function> functions = new Dictionary<ulong , Function>();

			    foreach (ReferenceSource callerSite in this.CallerSites)
			    {
				    if (null != callerSite.Function)
				    {
					    functions[callerSite.Function.Start] = callerSite.Function;
				    }
			    }

			    return functions.Values.ToArray();
		    }
	    }

	    /// <summary>
	    /// The functions called by this function. Mirrors Python <c>Function.callees</c>.
	    /// </summary>
	    public Function[] Callees
	    {
		    get
		    {
			    Dictionary<ulong , Function> functions = new Dictionary<ulong , Function>();

			    foreach (ulong address in this.CalleeAddresses)
			    {
				    Function? function = this.View.GetFunctionByAddress(address);

				    if (null != function)
				    {
					    functions[address] = function;
				    }
			    }

			    return functions.Values.ToArray();
		    }
	    }

	    /// <summary>
	    /// The start addresses of the functions called by this function. Mirrors Python
	    /// <c>Function.callee_addresses</c>: for every outgoing call site, the resolved
	    /// call target(s).
	    /// </summary>
	    public ulong[] CalleeAddresses
	    {
		    get
		    {
			    HashSet<ulong> addresses = new HashSet<ulong>();

			    foreach (ReferenceSource callSite in this.CallSites)
			    {
				    foreach (ulong target in this.View.GetCallees(callSite))
				    {
					    addresses.Add(target);
				    }
			    }

			    return addresses.ToArray();
		    }
	    }

	    public Workflow? Workflow
	    {
		    get
		    {
			    return Workflow.TakeHandle(
				    NativeMethods.BNGetWorkflowForFunction(this.handle)
				);
		    }
	    }

	    public ILReferenceSource[] GetMediumLevelILVariableReferences(Variable variable)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableReferences(
			    this.handle,
			    variable.ToNative(),
			    out ulong arrayLength
			);

		    return UnsafeUtils.TakeStructArrayEx<BNILReferenceSource , ILReferenceSource>(
			    arrayPointer ,
			    arrayLength ,
			    ILReferenceSource.FromNative,
			    NativeMethods.BNFreeVariableReferenceSourceList
		    );
	    }
	    
	    public ILReferenceSource[] GetMediumLevelILVariableReferencesFrom(
		    ulong address,
		    Architecture? arch = null
		)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableReferencesFrom(
			    this.handle,
			    this.ArchHandleOrDefault(arch),
			    address,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNILReferenceSource , ILReferenceSource>(
			    arrayPointer ,
			    arrayLength ,
			    ILReferenceSource.FromNative,
			    NativeMethods.BNFreeVariableReferenceSourceList
		    );
	    }
	    
	    public ILReferenceSource[] GetHighLevelILVariableReferencesFrom(
		    ulong address,
		    Architecture? arch = null
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableReferencesFrom(
			    this.handle,
			    this.ArchHandleOrDefault(arch),
			    address,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNILReferenceSource , ILReferenceSource>(
			    arrayPointer ,
			    arrayLength ,
			    ILReferenceSource.FromNative,
			    NativeMethods.BNFreeVariableReferenceSourceList
		    );
	    }

	    public ulong? GetInstructionContainingAddress(ulong address , Architecture? arch = null)
	    {
		    bool ok = NativeMethods.BNGetInstructionContainingAddress(
			    this.handle, 
			    this.ArchHandleOrDefault(arch),
			    address,
			    out ulong start
			);

		    if (!ok)
		    {
			    return null;
		    }

		    return start;
	    }

	    /// <summary>
	    /// The analysis hint used to order the basic block containing <paramref name="address"/>
	    /// during layout, or <c>null</c> when no hint exists. Mirrors C++
	    /// <c>Function::GetBlockSortHint</c>.
	    /// </summary>
	    public long? GetBlockSortHint(ulong address , Architecture? arch = null)
	    {
		    bool ok = NativeMethods.BNGetFunctionBlockSortHint(
			    this.handle ,
			    this.ArchHandleOrDefault(arch) ,
			    address ,
			    out long result
		    );

		    if (!ok)
		    {
			    return null;
		    }

		    return result;
	    }

	    /// <summary>
	    /// The Low Level IL instruction at <paramref name="address"/>, or <c>null</c>.
	    /// Mirrors Python <c>Function.get_low_level_il_at</c> / <c>get_llil_at</c>.
	    /// </summary>
	    public LowLevelILInstruction? GetLowLevelILAt(ulong address , Architecture? arch = null)
	    {
		    return this.GetLowLevelIL()?.GetInstructionStart(address , arch);
	    }

	    /// <summary>
	    /// Every Low Level IL instruction at <paramref name="address"/>.
	    /// Mirrors Python <c>Function.get_low_level_ils_at</c> / <c>get_llils_at</c>.
	    /// </summary>
	    public LowLevelILInstruction[] GetLowLevelILsAt(ulong address , Architecture? arch = null)
	    {
		    return this.GetLowLevelIL()?.GetInstructionsAt(address , arch)
			    ?? Array.Empty<LowLevelILInstruction>();
	    }

	    /// <summary>
	    /// The Lifted IL instruction at <paramref name="address"/>, or <c>null</c>.
	    /// Mirrors Python <c>Function.get_lifted_il_at</c>.
	    /// </summary>
	    public LowLevelILInstruction? GetLiftedILAt(ulong address , Architecture? arch = null)
	    {
		    return this.GetLiftedIL()?.GetInstructionStart(address , arch);
	    }

	    /// <summary>
	    /// Every Lifted IL instruction at <paramref name="address"/>.
	    /// Mirrors Python <c>Function.get_lifted_ils_at</c>.
	    /// </summary>
	    public LowLevelILInstruction[] GetLiftedILsAt(ulong address , Architecture? arch = null)
	    {
		    return this.GetLiftedIL()?.GetInstructionsAt(address , arch)
			    ?? Array.Empty<LowLevelILInstruction>();
	    }

	    /// <summary>
	    /// The Medium Level IL instruction at <paramref name="address"/>, or <c>null</c>.
	    /// Mirrors Python <c>Function.get_medium_level_il_at</c> / <c>get_mlil_at</c>.
	    /// </summary>
	    public MediumLevelILInstruction? GetMediumLevelILAt(ulong address , Architecture? arch = null)
	    {
		    return this.GetMediumLevelIL()?.GetInstructionStart(address , arch);
	    }

	    /// <summary>
	    /// The High Level IL instruction at <paramref name="address"/>, or <c>null</c>.
	    /// Resolved through the Medium Level IL mapping, mirroring Python
	    /// <c>Function.get_high_level_il_at</c> / <c>get_hlil_at</c>.
	    /// </summary>
	    public HighLevelILInstruction? GetHighLevelILAt(ulong address , Architecture? arch = null)
	    {
		    return this.GetMediumLevelILAt(address , arch)?.HighLevelILInstruction;
	    }

	    public LinearViewObject Disassembly( DisassemblySettings? settings = null)
	    {
		    return this.CreateLinearViewDisassembly(settings);
	    }
	    
	    public LinearViewObject CreateLinearViewDisassembly( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionDisassembly(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject CreateLinearViewLiftedIL( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionLiftedIL(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }

	    public LinearViewObject CreateLinearViewLowLevelIL( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionLowLevelIL(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject CreateLinearViewLowLevelILSSAForm( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionLowLevelILSSAForm(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject CreateLinearViewMediumLevelIL( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionMediumLevelIL(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject CreateLinearViewMediumLevelILSSAForm( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionMediumLevelILSSAForm(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject CreateLinearViewMappedMediumLevelIL( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionMappedMediumLevelIL(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject CreateLinearViewMappedMediumLevelILSSAForm( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionMappedMediumLevelILSSAForm(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject CreateLinearViewHighLevelIL( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionHighLevelIL(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject CreateLinearViewHighLevelILSSAForm( DisassemblySettings? settings = null) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionHighLevelILSSAForm(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LinearViewObject? CreateLinearViewLanguageRepresentation( 
		    DisassemblySettings? settings = null,
		    string language = "Pseudo C"
		) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    LanguageRepresentationFunction? pseudo = this.GetLanguageRepresentation(language);

		    if (null == pseudo)
		    {
			    return null;
		    }
		    
		    return LinearViewObject.TakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionLanguageRepresentation(
				    pseudo.OwnerFunction.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle() ,
				    language
			    )
		    );
	    }

	    /// <summary>
	    /// Builds a FlowGraph (disassembly or IL) for this function, waiting for analysis.
	    /// Mirrors Python <c>Function.create_graph</c>. A null <paramref name="type"/> defaults
	    /// to the normal disassembly graph; null <paramref name="settings"/> uses core defaults.
	    /// </summary>
	    public FlowGraph CreateGraph(
		    FunctionViewType? type = null ,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == type)
		    {
			    type = new FunctionViewType(FunctionGraphType.NormalFunctionGraph);
		    }

		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return FlowGraph.MustTakeHandle(
				    NativeMethods.BNCreateFunctionGraph(
					    this.handle ,
					    type.ToNativeEx(allocator) ,
					    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				    )
			    );
		    }
	    }

	    /// <summary>
	    /// Builds a FlowGraph from the function's current contents without waiting for analysis
	    /// (intended for workflow use). Mirrors Python <c>Function.create_graph_immediate</c>.
	    /// </summary>
	    public FlowGraph CreateGraphImmediate(
		    FunctionViewType? type = null ,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == type)
		    {
			    type = new FunctionViewType(FunctionGraphType.NormalFunctionGraph);
		    }

		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return FlowGraph.MustTakeHandle(
				    NativeMethods.BNCreateImmediateFunctionGraph(
					    this.handle ,
					    type.ToNativeEx(allocator) ,
					    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				    )
			    );
		    }
	    }

	    /// <summary>
	    /// A FlowGraph highlighting basic blocks whose stack-pointer adjustment could not be
	    /// resolved by analysis, or <c>null</c> when none. Mirrors Python
	    /// <c>Function.unresolved_stack_adjustment_graph</c>.
	    /// </summary>
	    public FlowGraph? UnresolvedStackAdjustmentGraph
	    {
		    get
		    {
			    // The core returns an already-owned reference (or NULL), so take ownership directly.
			    return FlowGraph.TakeHandle(
				    NativeMethods.BNGetUnresolvedStackAdjustmentGraph(this.handle)
			    );
		    }
	    }

		//  disassembly
	    public LinearDisassemblyLine[] GetLinearDisassemblyLines(DisassemblySettings? settings = null)
	    {
		    LinearViewObject linear = this.CreateLinearViewDisassembly(settings);
		    
		    LinearViewCursor cursor = linear.CreateCursor();
		    
		    List<LinearDisassemblyLine> targets = new List<LinearDisassemblyLine>();
		    
		    cursor.SeekToAddress(this.LowestAddress);
		    foreach (LinearDisassemblyLine line in cursor.PreviousLines)
		    {
			    if (!targets.Contains(line))
			    {
				    targets.Add(line);
			    }
		    }
		    
		    cursor.SeekToAddress(this.HighestAddress);
		    foreach (LinearDisassemblyLine line in cursor.NextLines)
		    {
			    if (!targets.Contains(line))
			    {
				    targets.Add(line);
			    }
		    }

		    return targets.ToArray();
	    }
	    
	    public IEnumerable<LinearDisassemblyLine> LinearDisassemblyLines
	    {
		    get
		    {
			    return this.GetLinearDisassemblyLines();
		    }
	    }
	    
	    public string GetLinearDisassemblyText(DisassemblySettings? settings = null)
	    {
		    StringBuilder builder = new StringBuilder();

		    foreach (LinearDisassemblyLine line in this.GetLinearDisassemblyLines(settings))
		    {
			    builder.AppendLine(line.ToString());
		    }

		    return builder.ToString();
	    }
	    
	    public string LinearDisassemblyText
	    {
		    get
		    {
			    return this.GetLinearDisassemblyText();
		    }
	    }
	    
	
	    public PluginCommand[] ValidPluginCommands
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForFunction(
				    this.View.DangerousGetHandle(),
				    this.handle,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
				    arrayPointer ,
				    arrayLength ,
				    PluginCommand.FromNative ,
				    NativeMethods.BNFreePluginCommandList
			    );
		    }
	    }

	    public string GetVariableNameOrDefault(CoreVariable variable)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetVariableNameOrDefault(
				    this.handle ,
				    variable.ToNative()
			    )
		    );
	    }
	    
	    public string GetLastSeenVariableNameOrDefault(CoreVariable variable)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetLastSeenVariableNameOrDefault(
				    this.handle ,
				    variable.ToNative()
			    )
		    );
	    }
	    
	    public DisassemblyTextLine[] GetTypeTokens(DisassemblySettings? settings = null)
	    {
		    ulong arrayLength = 0;
			
		    IntPtr arrayPointer = NativeMethods.BNGetFunctionTypeTokens(
			    this.handle ,
			    null == settings ? IntPtr.Zero :  settings.DangerousGetHandle() ,
			    out arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
			    arrayPointer ,
			    arrayLength ,
			    DisassemblyTextLine.FromNative ,
			    NativeMethods.BNFreeDisassemblyTextLines
		    );
	    }

	    public DisassemblyTextLine[] TypeTokens
	    {
		    get
		    {
			    return this.GetTypeTokens();
		    }
	    }

	    /// <summary>
	    /// Checks whether the function has a debug report with the specified name.
	    /// </summary>
	    public bool CheckForDebugReport(string name)
	    {
		    return NativeMethods.BNFunctionCheckForDebugReport(this.handle , name);
	    }

	    /// <summary>
	    /// Collapses the region identified by the given hash.
	    /// </summary>
	    public void CollapseRegion(ulong hash)
	    {
		    NativeMethods.BNFunctionCollapseRegion(this.handle , hash);
	    }

	    /// <summary>
	    /// Expands all collapsed regions in this function.
	    /// </summary>
	    public void ExpandAll()
	    {
		    NativeMethods.BNFunctionExpandAll(this.handle);
	    }

	    /// <summary>
	    /// Expands the region identified by the given hash.
	    /// </summary>
	    public void ExpandRegion(ulong hash)
	    {
		    NativeMethods.BNFunctionExpandRegion(this.handle , hash);
	    }

	    /// <summary>
	    /// Gets the automatically generated metadata for this function.
	    /// </summary>
	    public Metadata AutoMetadata
	    {
		    get
		    {
			    return Metadata.MustTakeHandle(
				    NativeMethods.BNFunctionGetAutoMetadata(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the full metadata for this function.
	    /// </summary>
	    public Metadata FunctionMetadata
	    {
		    get
		    {
			    return Metadata.MustTakeHandle(
				    NativeMethods.BNFunctionGetMetadata(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Checks whether the region identified by the given hash is collapsed.
	    /// </summary>
	    public bool IsRegionCollapsed(ulong hash)
	    {
		    return NativeMethods.BNFunctionIsRegionCollapsed(this.handle , hash);
	    }

	    /// <summary>
	    /// Queries a metadata entry by key, returning null if not found.
	    /// </summary>
	    public Metadata? QueryMetadata(string key)
	    {
		    return Metadata.TakeHandle(
			    NativeMethods.BNFunctionQueryMetadata(this.handle , key)
		    );
	    }

	    /// <summary>
	    /// Removes the metadata entry with the specified key.
	    /// </summary>
	    public void RemoveMetadata(string key)
	    {
		    NativeMethods.BNFunctionRemoveMetadata(this.handle , key);
	    }

	    /// <summary>
	    /// Stores a metadata entry with the specified key and value.
	    /// </summary>
	    public void StoreMetadata(string key , Metadata value , bool isAuto = false)
	    {
		    NativeMethods.BNFunctionStoreMetadata(
			    this.handle ,
			    key ,
			    value.DangerousGetHandle() ,
			    isAuto
		    );
	    }

	    /// <summary>
	    /// Toggles the collapsed state of the region identified by the given hash.
	    /// </summary>
	    public void ToggleRegion(ulong hash)
	    {
		    NativeMethods.BNFunctionToggleRegion(this.handle , hash);
	    }

	    /// <summary>
	    /// Gets whether this function uses an incoming global pointer.
	    /// </summary>
	    public bool UsesIncomingGlobalPointer
	    {
		    get
		    {
			    return NativeMethods.BNFunctionUsesIncomingGlobalPointer(this.handle);
		    }
	    }

	    // ===================================================================
	    // Auto-discovered and imported type methods
	    // ===================================================================

	    /// <summary>
	    /// Applies an automatically discovered function type to this function.
	    /// </summary>
	    public void ApplyAutoDiscoveredFunctionType(BinaryNinja.Type type)
	    {
		    // forward to native, passing the type handle
		    NativeMethods.BNApplyAutoDiscoveredFunctionType(
			    this.handle ,
			    type.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Applies imported types from a symbol and type to this function.
	    /// </summary>
	    public void ApplyImportedTypes(Symbol sym , BinaryNinja.Type type)
	    {
		    // forward to native with both symbol and type handles
		    NativeMethods.BNApplyImportedTypes(
			    this.handle ,
			    sym.DangerousGetHandle() ,
			    type.DangerousGetHandle()
		    );
	    }

	    // ===================================================================
	    // Analysis skip reason and override
	    // ===================================================================

	    /// <summary>
	    /// Gets the reason why analysis of this function was skipped.
	    /// </summary>
	    public AnalysisSkipReason AnalysisSkipReason
	    {
		    get
		    {
			    return NativeMethods.BNGetAnalysisSkipReason(this.handle);
		    }
	    }

	    /// <summary>
	    /// Gets or sets the analysis skip override for this function.
	    /// </summary>
	    public FunctionAnalysisSkipOverride AnalysisSkipOverride
	    {
		    get
		    {
			    return NativeMethods.BNGetFunctionAnalysisSkipOverride(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetFunctionAnalysisSkipOverride(this.handle , value);
		    }
	    }

	    // ===================================================================
	    // Update and analysis request methods
	    // ===================================================================

	    /// <summary>
	    /// Marks callers of this function as requiring updates of the specified type.
	    /// </summary>
	    public void MarkCallerUpdatesRequired(FunctionUpdateType type)
	    {
		    NativeMethods.BNMarkCallerUpdatesRequired(this.handle , type);
	    }

	    /// <summary>
	    /// Marks this function as requiring updates of the specified type.
	    /// </summary>
	    public void MarkUpdatesRequired(FunctionUpdateType type)
	    {
		    NativeMethods.BNMarkUpdatesRequired(this.handle , type);
	    }

	    /// <summary>
	    /// Requests advanced analysis data for this function, incrementing the reference count.
	    /// </summary>
	    public void RequestAdvancedFunctionAnalysisData()
	    {
		    NativeMethods.BNRequestAdvancedFunctionAnalysisData(this.handle);
	    }

	    /// <summary>
	    /// Releases advanced analysis data for this function, decrementing the reference count.
	    /// </summary>
	    public void ReleaseAdvancedFunctionAnalysisData()
	    {
		    NativeMethods.BNReleaseAdvancedFunctionAnalysisData(this.handle);
	    }

	    /// <summary>
	    /// Releases advanced analysis data multiple times at once.
	    /// </summary>
	    public void ReleaseAdvancedFunctionAnalysisDataMultiple(ulong count)
	    {
		    NativeMethods.BNReleaseAdvancedFunctionAnalysisDataMultiple(this.handle , count);
	    }

	    /// <summary>
	    /// Requests a debug report for this function with the specified name.
	    /// </summary>
	    public void RequestFunctionDebugReport(string name)
	    {
		    NativeMethods.BNRequestFunctionDebugReport(this.handle , name);
	    }

	    // ===================================================================
	    // Condition and tag manipulation
	    // ===================================================================

	    /// <summary>
	    /// Sets whether the branch condition at the given address is inverted.
	    /// </summary>
	    public void SetConditionInverted(ulong addr , bool invert)
	    {
		    NativeMethods.BNSetConditionInverted(this.handle , addr , invert);
	    }

	    /// <summary>
	    /// Removes an auto-analysis function-level tag.
	    /// </summary>
	    public void RemoveAutoFunctionTag(Tag tag)
	    {
		    NativeMethods.BNRemoveAutoFunctionTag(
			    this.handle ,
			    tag.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Marks this function as recently used in the UI.
	    /// </summary>
	    public void MarkFunctionAsRecentlyUsed()
	    {
		    NativeMethods.BNMarkFunctionAsRecentlyUsed(this.handle);
	    }

	    // ===================================================================
	    // Auto function property setters
	    // ===================================================================

	    /// <summary>
	    /// Sets the auto-analysis return type for this function.
	    /// </summary>
	    public void SetAutoReturnType(TypeWithConfidence type)
	    {
		    // allocate a native BNTypeWithConfidence struct and pass as pointer
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(type.ToNative());

			    NativeMethods.BNSetAutoFunctionReturnType(this.handle , nativePtr);
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis calling convention for this function.
	    /// </summary>
	    public void SetAutoCallingConvention(CallingConventionWithConfidence cc)
	    {
		    // allocate a native BNCallingConventionWithConfidence struct and pass as pointer
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(cc.ToNative());

			    NativeMethods.BNSetAutoFunctionCallingConvention(this.handle , nativePtr);
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis can-return property for this function.
	    /// </summary>
	    public void SetAutoCanReturn(BoolWithConfidence value)
	    {
		    // allocate a native BNBoolWithConfidence struct and pass as pointer
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(value.ToNative());

			    NativeMethods.BNSetAutoFunctionCanReturn(this.handle , nativePtr);
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis variable-arguments property for this function.
	    /// </summary>
	    public void SetAutoHasVariableArguments(BoolWithConfidence value)
	    {
		    // allocate a native BNBoolWithConfidence struct and pass as pointer
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(value.ToNative());

			    NativeMethods.BNSetAutoFunctionHasVariableArguments(this.handle , nativePtr);
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis pure property for this function.
	    /// </summary>
	    public void SetAutoPure(BoolWithConfidence value)
	    {
		    // allocate a native BNBoolWithConfidence struct and pass as pointer
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(value.ToNative());

			    NativeMethods.BNSetAutoFunctionPure(this.handle , nativePtr);
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis inlined-during-analysis property for this function.
	    /// </summary>
	    public void SetAutoFunctionInlinedDuringAnalysis(BoolWithConfidence inlined)
	    {
		    // pass the struct directly (by value), matching the P/Invoke signature
		    NativeMethods.BNSetAutoFunctionInlinedDuringAnalysis(this.handle , inlined);
	    }

	    /// <summary>
	    /// Sets the user-defined inlined-during-analysis property for this function.
	    /// </summary>
	    public void SetUserFunctionInlinedDuringAnalysis(BoolWithConfidence inlined)
	    {
		    // pass the struct directly (by value), matching the P/Invoke signature
		    NativeMethods.BNSetUserFunctionInlinedDuringAnalysis(this.handle , inlined);
	    }

	    /// <summary>
	    /// Gets whether this function is inlined during analysis, with confidence.
	    /// </summary>
	    public BoolWithConfidence IsFunctionInlinedDuringAnalysis
	    {
		    get
		    {
			    // call native and convert the raw struct to managed type
			    return BoolWithConfidence.FromNative(
				    NativeMethods.BNIsFunctionInlinedDuringAnalysis(this.handle)
			    );
		    }
	    }

	    // ===================================================================
	    // Clobbered registers (get / set auto / set user)
	    // ===================================================================

	    /// <summary>
	    /// Gets the set of registers clobbered by this function, with confidence.
	    /// </summary>
	    public RegisterSetWithConfidence ClobberedRegisters
	    {
		    get
		    {
			    return RegisterSetWithConfidence.FromNative(
				    NativeMethods.BNGetFunctionClobberedRegisters(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis clobbered registers for this function.
	    /// </summary>
	    public void SetAutoFunctionClobberedRegisters(RegisterSetWithConfidence regs)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert to native struct, then allocate a pointer to it
			    BNRegisterSetWithConfidence native = regs.ToNativeEx(allocator);

			    IntPtr nativePtr = allocator.AllocStruct(native);

			    NativeMethods.BNSetAutoFunctionClobberedRegisters(this.handle , nativePtr);
		    }
	    }

	    /// <summary>
	    /// Sets the user-defined clobbered registers for this function.
	    /// </summary>
	    public void SetUserFunctionClobberedRegisters(RegisterSetWithConfidence regs)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert to native struct, then allocate a pointer to it
			    BNRegisterSetWithConfidence native = regs.ToNativeEx(allocator);

			    IntPtr nativePtr = allocator.AllocStruct(native);

			    NativeMethods.BNSetUserFunctionClobberedRegisters(this.handle , nativePtr);
		    }
	    }

	    // ===================================================================
	    // Return registers (get / set auto / set user)
	    // ===================================================================

	    /// <summary>
	    /// Gets the set of registers used for return values, with confidence.
	    /// </summary>
	    public RegisterSetWithConfidence ReturnRegisters
	    {
		    get
		    {
			    return RegisterSetWithConfidence.FromNative(
				    NativeMethods.BNGetFunctionReturnRegisters(this.handle)
			    );
		    }
	    }


	    // ===================================================================
	    // Stack adjustment (get / set auto / set user)
	    // ===================================================================

	    /// <summary>
	    /// Gets the stack adjustment for this function, with confidence.
	    /// </summary>
	    public OffsetWithConfidence FunctionStackAdjustment
	    {
		    get
		    {
			    return OffsetWithConfidence.FromNative(
				    NativeMethods.BNGetFunctionStackAdjustment(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis stack adjustment for this function.
	    /// </summary>
	    public void SetAutoFunctionStackAdjustment(OffsetWithConfidence stackAdjust)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(stackAdjust.ToNative());

			    NativeMethods.BNSetAutoFunctionStackAdjustment(this.handle , nativePtr);
		    }
	    }

	    /// <summary>
	    /// Sets the user-defined stack adjustment for this function.
	    /// </summary>
	    public void SetUserFunctionStackAdjustment(OffsetWithConfidence stackAdjust)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(stackAdjust.ToNative());

			    NativeMethods.BNSetUserFunctionStackAdjustment(this.handle , nativePtr);
		    }
	    }

	    // ===================================================================
	    // Global pointer value and register value at exit
	    // ===================================================================

	    /// <summary>
	    /// Gets the per-register global pointer values for this function, with confidence.
	    /// </summary>
	    public RegisterValueWithConfidenceAndRegister[] FunctionGlobalPointerValues
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetFunctionGlobalPointerValues(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArray<BNRegisterValueWithConfidenceAndRegister , RegisterValueWithConfidenceAndRegister>(
				    arrayPointer ,
				    arrayLength ,
				    RegisterValueWithConfidenceAndRegister.FromNative ,
				    NativeMethods.BNFreeRegisterValueWithConfidenceAndRegisterList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the register value at function exit for the specified register.
	    /// </summary>
	    public RegisterValueWithConfidence GetFunctionRegisterValueAtExit(uint reg)
	    {
		    return RegisterValueWithConfidence.FromNative(
			    NativeMethods.BNGetFunctionRegisterValueAtExit(this.handle , reg)
		    );
	    }

	    // ===================================================================
	    // Stack variable creation and deletion
	    // ===================================================================

	    /// <summary>
	    /// Creates an auto-analysis stack variable at the given frame offset.
	    /// </summary>
	    public void CreateAutoStackVariable(long offset , TypeWithConfidence type , string name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the type struct as a pointer for the native call
			    IntPtr typePtr = allocator.AllocStruct(type.ToNative());

			    NativeMethods.BNCreateAutoStackVariable(this.handle , offset , typePtr , name);
		    }
	    }

	    /// <summary>
	    /// Creates a user-defined stack variable at the given frame offset.
	    /// </summary>
	    public void CreateUserStackVariable(long offset , TypeWithConfidence type , string name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the type struct as a pointer for the native call
			    IntPtr typePtr = allocator.AllocStruct(type.ToNative());

			    NativeMethods.BNCreateUserStackVariable(this.handle , offset , typePtr , name);
		    }
	    }

	    /// <summary>
	    /// Creates an auto-analysis variable with the specified attributes.
	    /// </summary>
	    public void CreateAutoVariable(CoreVariable variable , TypeWithConfidence type , string name , bool ignoreDisjointUses)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate both the variable and type structs as pointers
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    IntPtr typePtr = allocator.AllocStruct(type.ToNative());

			    NativeMethods.BNCreateAutoVariable(this.handle , varPtr , typePtr , name , ignoreDisjointUses);
		    }
	    }

	    /// <summary>
	    /// Creates a user-defined variable with the specified attributes.
	    /// </summary>
	    public void CreateUserVariable(CoreVariable variable , TypeWithConfidence type , string name , bool ignoreDisjointUses)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate both the variable and type structs as pointers
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    IntPtr typePtr = allocator.AllocStruct(type.ToNative());

			    NativeMethods.BNCreateUserVariable(this.handle , varPtr , typePtr , name , ignoreDisjointUses);
		    }
	    }

	    /// <summary>
	    /// Deletes an auto-analysis stack variable at the given frame offset.
	    /// </summary>
	    public void DeleteAutoStackVariable(long offset)
	    {
		    NativeMethods.BNDeleteAutoStackVariable(this.handle , offset);
	    }

	    /// <summary>
	    /// Deletes a user-defined stack variable at the given frame offset.
	    /// </summary>
	    public void DeleteUserStackVariable(long offset)
	    {
		    NativeMethods.BNDeleteUserStackVariable(this.handle , offset);
	    }

	    /// <summary>
	    /// Deletes a user-defined variable.
	    /// </summary>
	    public void DeleteUserVariable(CoreVariable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the variable struct as a pointer
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    NativeMethods.BNDeleteUserVariable(this.handle , varPtr);
		    }
	    }

	    // ===================================================================
	    // Variable query methods
	    // ===================================================================

	    /// <summary>
	    /// Gets the name of the specified variable.
	    /// </summary>
	    public string GetVariableName(CoreVariable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the variable struct as a pointer
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetVariableName(this.handle , varPtr)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the type of the specified variable, with confidence.
	    /// </summary>
	    public TypeWithConfidence GetVariableType(CoreVariable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the variable struct as a pointer
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    return TypeWithConfidence.FromNative(
				    NativeMethods.BNGetVariableType(this.handle , varPtr)
			    );
		    }
	    }

	    /// <summary>
	    /// Returns true if the specified variable has been defined by the user.
	    /// </summary>
	    public bool IsVariableUserDefined(CoreVariable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the variable struct as a pointer
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    return NativeMethods.BNIsVariableUserDefined(this.handle , varPtr);
		    }
	    }

	    /// <summary>
	    /// Gets the dead store elimination mode for the specified variable.
	    /// </summary>
	    public DeadStoreElimination GetFunctionVariableDeadStoreElimination(CoreVariable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the variable struct as a pointer
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    return NativeMethods.BNGetFunctionVariableDeadStoreElimination(this.handle , varPtr);
		    }
	    }

	    /// <summary>
	    /// Sets the dead store elimination mode for the specified variable.
	    /// </summary>
	    public void SetFunctionVariableDeadStoreElimination(CoreVariable variable , DeadStoreElimination mode)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the variable struct as a pointer
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    NativeMethods.BNSetFunctionVariableDeadStoreElimination(this.handle , varPtr , mode);
		    }
	    }

	    // ===================================================================
	    // Stack variable at frame offset
	    // ===================================================================

	    /// <summary>
	    /// Gets the stack variable at the specified frame offset, or null if none exists.
	    /// </summary>
	    public VariableNameAndType? GetStackVariableAtFrameOffset(
		    Architecture arch ,
		    ulong addr ,
		    long offset
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate space for the output BNVariableNameAndType struct
			    IntPtr outPtr = allocator.AllocHGlobal(Marshal.SizeOf<BNVariableNameAndType>());

			    bool found = NativeMethods.BNGetStackVariableAtFrameOffset(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    offset ,
				    outPtr
			    );

			    if (!found)
			    {
				    return null;
			    }

			    // marshal the output struct from the pointer
			    BNVariableNameAndType native = Marshal.PtrToStructure<BNVariableNameAndType>(outPtr);

			    return VariableNameAndType.FromNative(native);
		    }
	    }

	    /// <summary>
	    /// Gets the stack variable at the specified frame offset after instruction execution, or null if none exists.
	    /// </summary>
	    public VariableNameAndType? GetStackVariableAtFrameOffsetAfterInstruction(
		    Architecture arch ,
		    ulong addr ,
		    long offset
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate space for the output BNVariableNameAndType struct
			    IntPtr outPtr = allocator.AllocHGlobal(Marshal.SizeOf<BNVariableNameAndType>());

			    bool found = NativeMethods.BNGetStackVariableAtFrameOffsetAfterInstruction(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    offset ,
				    outPtr
			    );

			    if (!found)
			    {
				    return null;
			    }

			    // marshal the output struct from the pointer
			    BNVariableNameAndType native = Marshal.PtrToStructure<BNVariableNameAndType>(outPtr);

			    return VariableNameAndType.FromNative(native);
		    }
	    }

	    // ===================================================================
	    // Tag operations (address tags of type)
	    // ===================================================================

	    /// <summary>
	    /// Gets auto-analysis address tags of the specified type at the given address.
	    /// </summary>
	    public Tag[] GetAutoAddressTagsOfType(Architecture arch , ulong addr , TagType tagType)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetAutoAddressTagsOfType(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    tagType.DangerousGetHandle() ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeHandleArrayEx<Tag>(
				    arrayPointer ,
				    arrayLength ,
				    Tag.MustNewFromHandle ,
				    NativeMethods.BNFreeTagList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets user-defined address tags of the specified type at the given address.
	    /// </summary>
	    public Tag[] GetUserAddressTagsOfType(Architecture arch , ulong addr , TagType tagType)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetUserAddressTagsOfType(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    tagType.DangerousGetHandle() ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeHandleArrayEx<Tag>(
				    arrayPointer ,
				    arrayLength ,
				    Tag.MustNewFromHandle ,
				    NativeMethods.BNFreeTagList
			    );
		    }
	    }

	    // ===================================================================
	    // Type references (add / remove user)
	    // ===================================================================

	    /// <summary>
	    /// Adds a user-defined type field reference from the specified address.
	    /// </summary>
	    public void AddUserTypeFieldReference(
		    Architecture fromArch ,
		    ulong fromAddr ,
		    QualifiedName name ,
		    ulong offset ,
		    ulong size
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the qualified name struct as a pointer
			    IntPtr namePtr = allocator.AllocStruct(name.ToNativeEx(allocator));

			    NativeMethods.BNAddUserTypeFieldReference(
				    this.handle ,
				    fromArch.DangerousGetHandle() ,
				    fromAddr ,
				    namePtr ,
				    offset ,
				    size
			    );
		    }
	    }

	    /// <summary>
	    /// Adds a user-defined type reference from the specified address.
	    /// </summary>
	    public void AddUserTypeReference(
		    Architecture fromArch ,
		    ulong fromAddr ,
		    QualifiedName name
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the qualified name struct as a pointer
			    IntPtr namePtr = allocator.AllocStruct(name.ToNativeEx(allocator));

			    NativeMethods.BNAddUserTypeReference(
				    this.handle ,
				    fromArch.DangerousGetHandle() ,
				    fromAddr ,
				    namePtr
			    );
		    }
	    }

	    /// <summary>
	    /// Removes a user-defined type field reference from the specified address.
	    /// </summary>
	    public void RemoveUserTypeFieldReference(
		    Architecture fromArch ,
		    ulong fromAddr ,
		    QualifiedName name ,
		    ulong offset ,
		    ulong size
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the qualified name struct as a pointer
			    IntPtr namePtr = allocator.AllocStruct(name.ToNativeEx(allocator));

			    NativeMethods.BNRemoveUserTypeFieldReference(
				    this.handle ,
				    fromArch.DangerousGetHandle() ,
				    fromAddr ,
				    namePtr ,
				    offset ,
				    size
			    );
		    }
	    }

	    /// <summary>
	    /// Removes a user-defined type reference from the specified address.
	    /// </summary>
	    public void RemoveUserTypeReference(
		    Architecture fromArch ,
		    ulong fromAddr ,
		    QualifiedName name
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate the qualified name struct as a pointer
			    IntPtr namePtr = allocator.AllocStruct(name.ToNativeEx(allocator));

			    NativeMethods.BNRemoveUserTypeReference(
				    this.handle ,
				    fromArch.DangerousGetHandle() ,
				    fromAddr ,
				    namePtr
			    );
		    }
	    }

	    // ===================================================================
	    // Indirect branches (set auto / set user)
	    // ===================================================================

	    /// <summary>
	    /// Sets the auto-analysis indirect branch targets at the specified source address.
	    /// </summary>
	    public void SetAutoIndirectBranches(
		    Architecture sourceArch ,
		    ulong source ,
		    ArchitectureAndAddress[] branches
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNArchitectureAndAddress[] nativeArray = new BNArchitectureAndAddress[branches.Length];

			    for (int i = 0; i < branches.Length; i++)
			    {
				    nativeArray[i] = branches[i].ToNative();
			    }

			    // allocate the native array as a contiguous block
			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNSetAutoIndirectBranches(
				    this.handle ,
				    sourceArch.DangerousGetHandle() ,
				    source ,
				    arrayPtr ,
				    (ulong)branches.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Sets the user-defined indirect branch targets at the specified source address.
	    /// </summary>
	    public void SetUserIndirectBranches(
		    Architecture sourceArch ,
		    ulong source ,
		    ArchitectureAndAddress[] branches
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNArchitectureAndAddress[] nativeArray = new BNArchitectureAndAddress[branches.Length];

			    for (int i = 0; i < branches.Length; i++)
			    {
				    nativeArray[i] = branches[i].ToNative();
			    }

			    // allocate the native array as a contiguous block
			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNSetUserIndirectBranches(
				    this.handle ,
				    sourceArch.DangerousGetHandle() ,
				    source ,
				    arrayPtr ,
				    (ulong)branches.Length
			    );
		    }
	    }

	    // ===================================================================
	    // Instruction highlight (get / set auto / set user)
	    // ===================================================================

	    /// <summary>
	    /// Gets the instruction highlight color at the specified address for the given architecture.
	    /// </summary>
	    /// <param name="arch">The architecture context for the instruction.</param>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <returns>The highlight color currently applied to the instruction.</returns>
	    public HighlightColor GetInstructionHighlight(Architecture arch , ulong addr)
	    {
		    BNHighlightColor raw = NativeMethods.BNGetInstructionHighlight(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr
		    );

		    return HighlightColor.FromNative(raw);
	    }

	    /// <summary>
	    /// Sets the auto-analysis instruction highlight color at the specified address.
	    /// </summary>
	    public void SetAutoInstructionHighlight(Architecture arch , ulong addr , HighlightColor color)
	    {
		    // pass the color struct by value, matching the P/Invoke signature
		    NativeMethods.BNSetAutoInstructionHighlight(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr ,
			    color
		    );
	    }

	    /// <summary>
	    /// Sets the user-defined instruction highlight color at the specified address.
	    /// </summary>
	    public void SetUserInstructionHighlight(Architecture arch , ulong addr , HighlightColor color)
	    {
		    // pass the color struct by value, matching the P/Invoke signature
		    NativeMethods.BNSetUserInstructionHighlight(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr ,
			    color
		    );
	    }

	    // ===================================================================
	    // Registers read/written by instruction
	    // ===================================================================

	    /// <summary>
	    /// Gets the registers read by the instruction at the specified address.
	    /// </summary>
	    public uint[] GetRegistersReadByInstruction(ulong addr , Architecture? arch = null)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetRegistersReadByInstruction(
				    this.handle ,
				    this.ArchHandleOrDefault(arch) ,
				    addr ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the registers written by the instruction at the specified address.
	    /// </summary>
	    public uint[] GetRegistersWrittenByInstruction(ulong addr , Architecture? arch = null)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetRegistersWrittenByInstruction(
				    this.handle ,
				    this.ArchHandleOrDefault(arch) ,
				    addr ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
		    }
	    }

	    // ===================================================================
	    // Callee for analysis
	    // ===================================================================

	    /// <summary>
	    /// Gets the callee function at the specified call address for analysis purposes.
	    /// Returns null if no callee is found.
	    /// </summary>
	    public Function? GetCalleeForAnalysis(Platform platform , ulong addr , bool exact = true)
	    {
		    return Function.TakeHandle(
			    NativeMethods.BNGetCalleeForAnalysis(
				    this.handle ,
				    platform.DangerousGetHandle() ,
				    addr ,
				    exact
			    )
		    );
	    }

	    // ===================================================================
	    // Early return / expression folding / switch recovery
	    // ===================================================================

	    /// <summary>
	    /// Gets the early return mode for the instruction at the specified address.
	    /// </summary>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <returns>The early return mode setting.</returns>
	    public EarlyReturn GetEarlyReturn(ulong addr)
	    {
		    return NativeMethods.BNGetEarlyReturn(this.handle , addr);
	    }

	    /// <summary>
	    /// Sets the early return mode for the instruction at the specified address.
	    /// </summary>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <param name="mode">The early return mode to apply.</param>
	    public void SetEarlyReturn(ulong addr , EarlyReturn mode)
	    {
		    NativeMethods.BNSetEarlyReturn(this.handle , addr , mode);
	    }

	    /// <summary>
	    /// Gets the expression folding mode for the expression at the specified address.
	    /// </summary>
	    /// <param name="addr">The address of the expression.</param>
	    /// <returns>The expression folding mode setting.</returns>
	    public ExprFolding GetExprFolding(ulong addr)
	    {
		    return NativeMethods.BNGetExprFolding(this.handle , addr);
	    }

	    /// <summary>
	    /// Sets the expression folding mode for the expression at the specified address.
	    /// </summary>
	    /// <param name="addr">The address of the expression.</param>
	    /// <param name="mode">The expression folding mode to apply.</param>
	    public void SetExprFolding(ulong addr , ExprFolding mode)
	    {
		    NativeMethods.BNSetExprFolding(this.handle , addr , mode);
	    }

	    /// <summary>
	    /// Gets the switch recovery mode for the instruction at the specified address.
	    /// </summary>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <returns>The switch recovery mode setting.</returns>
	    public SwitchRecovery GetSwitchRecovery(ulong addr)
	    {
		    return NativeMethods.BNGetSwitchRecovery(this.handle , addr);
	    }

	    /// <summary>
	    /// Sets the switch recovery mode for the instruction at the specified address.
	    /// </summary>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <param name="mode">The switch recovery mode to apply.</param>
	    public void SetSwitchRecovery(ulong addr , SwitchRecovery mode)
	    {
		    NativeMethods.BNSetSwitchRecovery(this.handle , addr , mode);
	    }

	    // ===================================================================
	    // Call instruction check
	    // ===================================================================

	    /// <summary>
	    /// Checks whether the instruction at the specified address is a call instruction.
	    /// </summary>
	    /// <param name="arch">The architecture context for the instruction.</param>
	    /// <param name="addr">The address of the instruction to check.</param>
	    /// <returns>True if the instruction is a call instruction.</returns>
	    public bool IsCallInstruction(Architecture arch , ulong addr)
	    {
		    return NativeMethods.BNIsCallInstruction(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr
		    );
	    }

	    // ===================================================================
	    // Tag references
	    // ===================================================================

	    /// <summary>
	    /// Retrieves all address tag references for this function.
	    /// </summary>
	    /// <returns>An array of TagReference objects for address tags.</returns>
	    public TagReference[] GetAddressTagReferences()
	    {
		    IntPtr ptr = NativeMethods.BNGetAddressTagReferences(
			    this.handle ,
			    out ulong count
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
			    ptr ,
			    count ,
			    TagReference.FromNative ,
			    NativeMethods.BNFreeTagReferences
		    );
	    }

	    /// <summary>
	    /// Retrieves auto address tag references for this function.
	    /// </summary>
	    /// <returns>An array of TagReference objects for auto address tags.</returns>
	    public unsafe TagReference[] GetAutoAddressTagReferences()
	    {
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetAutoAddressTagReferences(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
			    ptr ,
			    count ,
			    TagReference.FromNative ,
			    NativeMethods.BNFreeTagReferences
		    );
	    }

	    /// <summary>
	    /// Retrieves auto function tag references for this function.
	    /// </summary>
	    /// <returns>An array of TagReference objects for auto function tags.</returns>
	    public unsafe TagReference[] GetAutoFunctionTagReferences()
	    {
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetAutoFunctionTagReferences(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
			    ptr ,
			    count ,
			    TagReference.FromNative ,
			    NativeMethods.BNFreeTagReferences
		    );
	    }

	    /// <summary>
	    /// Retrieves all tag references (address and function) for this function.
	    /// </summary>
	    /// <returns>An array of TagReference objects for all tags on this function.</returns>
	    public unsafe TagReference[] GetAllTagReferences()
	    {
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetFunctionAllTagReferences(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
			    ptr ,
			    count ,
			    TagReference.FromNative ,
			    NativeMethods.BNFreeTagReferences
		    );
	    }

	    /// <summary>
	    /// Retrieves function-level tag references for this function.
	    /// </summary>
	    /// <returns>An array of TagReference objects for function-level tags.</returns>
	    public unsafe TagReference[] GetFunctionTagReferences()
	    {
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetFunctionTagReferences(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
			    ptr ,
			    count ,
			    TagReference.FromNative ,
			    NativeMethods.BNFreeTagReferences
		    );
	    }

	    /// <summary>
	    /// Retrieves function-level tag references of a specific type for this function.
	    /// </summary>
	    /// <param name="tagType">The tag type to filter by.</param>
	    /// <returns>An array of matching TagReference objects.</returns>
	    public unsafe TagReference[] GetFunctionTagReferencesOfType(TagType tagType)
	    {
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetFunctionTagReferencesOfType(
			    this.handle ,
			    tagType.DangerousGetHandle() ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
			    ptr ,
			    count ,
			    TagReference.FromNative ,
			    NativeMethods.BNFreeTagReferences
		    );
	    }

	    /// <summary>
	    /// Retrieves user address tag references for this function.
	    /// </summary>
	    /// <returns>An array of TagReference objects for user address tags.</returns>
	    public unsafe TagReference[] GetUserAddressTagReferences()
	    {
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetUserAddressTagReferences(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
			    ptr ,
			    count ,
			    TagReference.FromNative ,
			    NativeMethods.BNFreeTagReferences
		    );
	    }

	    /// <summary>
	    /// Retrieves user function tag references for this function.
	    /// </summary>
	    /// <returns>An array of TagReference objects for user function tags.</returns>
	    public unsafe TagReference[] GetUserFunctionTagReferences()
	    {
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetUserFunctionTagReferences(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
			    ptr ,
			    count ,
			    TagReference.FromNative ,
			    NativeMethods.BNFreeTagReferences
		    );
	    }

	    // ===================================================================
	    // Guided source blocks
	    // ===================================================================

	    /// <summary>
	    /// Adds guided source blocks to this function from the provided array.
	    /// </summary>
	    /// <param name="blocks">The array of architecture-and-address pairs to add as guided source blocks.</param>
	    public void AddGuidedSourceBlocks(ArchitectureAndAddress[] blocks)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNArchitectureAndAddress[] nativeArray = new BNArchitectureAndAddress[blocks.Length];

			    for (int i = 0; i < blocks.Length; i++)
			    {
				    nativeArray[i] = blocks[i].ToNative();
			    }

			    // allocate the native array as a contiguous block
			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNAddGuidedSourceBlocks(
				    this.handle ,
				    arrayPtr ,
				    (ulong)blocks.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Removes guided source blocks from this function for the provided array.
	    /// </summary>
	    /// <param name="blocks">The array of architecture-and-address pairs to remove from guided source blocks.</param>
	    public void RemoveGuidedSourceBlocks(ArchitectureAndAddress[] blocks)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNArchitectureAndAddress[] nativeArray = new BNArchitectureAndAddress[blocks.Length];

			    for (int i = 0; i < blocks.Length; i++)
			    {
				    nativeArray[i] = blocks[i].ToNative();
			    }

			    // allocate the native array as a contiguous block
			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNRemoveGuidedSourceBlocks(
				    this.handle ,
				    arrayPtr ,
				    (ulong)blocks.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Checks whether this function has any guided source blocks defined.
	    /// </summary>
	    /// <returns>True if guided source blocks are present.</returns>
	    public bool HasGuidedSourceBlocks()
	    {
		    return NativeMethods.BNHasGuidedSourceBlocks(this.handle);
	    }

	    /// <summary>
	    /// Checks whether the given architecture and address is a guided source block.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="addr">The address to check.</param>
	    /// <returns>True if the specified address is a guided source block.</returns>
	    public bool IsGuidedSourceBlock(Architecture arch , ulong addr)
	    {
		    return NativeMethods.BNIsGuidedSourceBlock(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr
		    );
	    }

	    /// <summary>
	    /// Sets the guided source blocks for this function, replacing any existing ones.
	    /// </summary>
	    /// <param name="blocks">The array of architecture-and-address pairs to set as guided source blocks.</param>
	    public void SetGuidedSourceBlocks(ArchitectureAndAddress[] blocks)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNArchitectureAndAddress[] nativeArray = new BNArchitectureAndAddress[blocks.Length];

			    for (int i = 0; i < blocks.Length; i++)
			    {
				    nativeArray[i] = blocks[i].ToNative();
			    }

			    // allocate the native array as a contiguous block
			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNSetGuidedSourceBlocks(
				    this.handle ,
				    arrayPtr ,
				    (ulong)blocks.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Gets all guided source blocks defined for this function.
	    /// </summary>
	    /// <returns>An array of architecture-and-address pairs representing the guided source blocks.</returns>
	    public ArchitectureAndAddress[] GetGuidedSourceBlocks()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetGuidedSourceBlocks(
				    this.handle ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArray<BNArchitectureAndAddress , ArchitectureAndAddress>(
				    arrayPointer ,
				    arrayLength ,
				    ArchitectureAndAddress.FromNative ,
				    NativeMethods.BNFreeArchitectureAndAddressList
			    );
		    }
	    }

	    // ===================================================================
	    // Variable operations
	    // ===================================================================

	    /// <summary>
	    /// Clears the field resolution for a variable at the specified definition site.
	    /// </summary>
	    /// <param name="variable">The variable to clear field resolution for.</param>
	    /// <param name="defSite">The definition site (architecture and address) of the variable.</param>
	    public void ClearFieldResolutionForVariableAt(CoreVariable variable , ArchitectureAndAddress defSite)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    IntPtr defSitePtr = allocator.AllocStruct(defSite.ToNative());

			    NativeMethods.BNClearFieldResolutionForVariableAt(
				    this.handle ,
				    varPtr ,
				    defSitePtr
			    );
		    }
	    }

	    /// <summary>
	    /// Clears any forced variable version at the specified definition site.
	    /// </summary>
	    /// <param name="variable">The variable to clear the forced version for.</param>
	    /// <param name="defSite">The definition site (architecture and address) of the variable.</param>
	    public void ClearForcedVariableVersion(CoreVariable variable , ArchitectureAndAddress defSite)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    IntPtr defSitePtr = allocator.AllocStruct(defSite.ToNative());

			    NativeMethods.BNClearForcedVariableVersion(
				    this.handle ,
				    varPtr ,
				    defSitePtr
			    );
		    }
	    }

	    /// <summary>
	    /// Creates a forced variable version at the specified definition site.
	    /// </summary>
	    /// <param name="variable">The variable to create the forced version for.</param>
	    /// <param name="defSite">The definition site (architecture and address) of the variable.</param>
	    public void CreateForcedVariableVersion(CoreVariable variable , ArchitectureAndAddress defSite)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    IntPtr defSitePtr = allocator.AllocStruct(defSite.ToNative());

			    NativeMethods.BNCreateForcedVariableVersion(
				    this.handle ,
				    varPtr ,
				    defSitePtr
			    );
		    }
	    }

	    /// <summary>
	    /// Merges multiple source variables into a single target variable.
	    /// </summary>
	    /// <param name="target">The target variable that will receive the merge.</param>
	    /// <param name="sources">The array of source variables to merge into the target.</param>
	    public void MergeVariables(CoreVariable target , CoreVariable[] sources)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr targetPtr = allocator.AllocStruct(target.ToNative());

			    // convert the managed source array to native struct array
			    BNVariable[] nativeArray = new BNVariable[sources.Length];

			    for (int i = 0; i < sources.Length; i++)
			    {
				    nativeArray[i] = sources[i].ToNative();
			    }

			    IntPtr sourcesPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNMergeVariables(
				    this.handle ,
				    targetPtr ,
				    sourcesPtr ,
				    (ulong)sources.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Splits a variable so that each definition site creates an independent version.
	    /// </summary>
	    /// <param name="variable">The variable to split.</param>
	    public void SplitVariable(CoreVariable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    NativeMethods.BNSplitVariable(this.handle , varPtr);
		    }
	    }

	    /// <summary>
	    /// Unmerges previously merged source variables from the target variable.
	    /// </summary>
	    /// <param name="target">The target variable that was the merge destination.</param>
	    /// <param name="sources">The array of source variables to unmerge from the target.</param>
	    public void UnmergeVariables(CoreVariable target , CoreVariable[] sources)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr targetPtr = allocator.AllocStruct(target.ToNative());

			    // convert the managed source array to native struct array
			    BNVariable[] nativeArray = new BNVariable[sources.Length];

			    for (int i = 0; i < sources.Length; i++)
			    {
				    nativeArray[i] = sources[i].ToNative();
			    }

			    IntPtr sourcesPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNUnmergeVariables(
				    this.handle ,
				    targetPtr ,
				    sourcesPtr ,
				    (ulong)sources.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Unsplits a previously split variable, reverting it to a single version.
	    /// </summary>
	    /// <param name="variable">The variable to unsplit.</param>
	    public void UnsplitVariable(CoreVariable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    NativeMethods.BNUnsplitVariable(this.handle , varPtr);
		    }
	    }

	    /// <summary>
	    /// Sets the field resolution info for a variable at the specified definition site.
	    /// </summary>
	    /// <param name="variable">The variable to set field resolution for.</param>
	    /// <param name="defSite">The definition site (architecture and address) of the variable.</param>
	    /// <param name="info">The field resolution info to apply.</param>
	    public void SetFieldResolutionForVariableAt(CoreVariable variable , ArchitectureAndAddress defSite , FieldResolutionInfo info)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr varPtr = allocator.AllocStruct(variable.ToNative());

			    IntPtr defSitePtr = allocator.AllocStruct(defSite.ToNative());

			    NativeMethods.BNSetFieldResolutionForVariableAt(
				    this.handle ,
				    varPtr ,
				    defSitePtr ,
				    info.DangerousGetHandle()
			    );
		    }
	    }

	    // ===================================================================
	    // Call stack / type / register stack adjustments
	    // ===================================================================

	    /// <summary>
	    /// Gets the call stack adjustment at the specified call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <returns>The stack offset adjustment with confidence.</returns>
	    public OffsetWithConfidence GetCallStackAdjustment(Architecture arch , ulong addr)
	    {
		    return OffsetWithConfidence.FromNative(
			    NativeMethods.BNGetCallStackAdjustment(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr
			    )
		    );
	    }

	    /// <summary>
	    /// Gets the call type adjustment at the specified call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <returns>The type with confidence representing the call type adjustment.</returns>
	    public TypeWithConfidence GetCallTypeAdjustment(Architecture arch , ulong addr)
	    {
		    return TypeWithConfidence.FromNative(
			    NativeMethods.BNGetCallTypeAdjustment(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr
			    )
		    );
	    }

	    /// <summary>
	    /// Gets the call register stack adjustment for a specific register stack at a call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="regStack">The register stack identifier.</param>
	    /// <returns>The register stack adjustment value.</returns>
	    public RegisterStackAdjustment GetCallRegisterStackAdjustmentForRegisterStack(Architecture arch , ulong addr , uint regStack)
	    {
		    BNRegisterStackAdjustment native = NativeMethods.BNGetCallRegisterStackAdjustmentForRegisterStack(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr ,
			    regStack
		    );

		    return new RegisterStackAdjustment()
		    {
			    RegStack = native.regStack ,
			    Adjustment = native.adjustment ,
			    Confidence = native.confidence
		    };
	    }

	    /// <summary>
	    /// Sets the auto-analysis call stack adjustment at the specified call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="adjust">The stack adjustment value in bytes.</param>
	    /// <param name="confidence">The confidence level of the adjustment.</param>
	    public void SetAutoCallStackAdjustment(Architecture arch , ulong addr , long adjust , byte confidence)
	    {
		    NativeMethods.BNSetAutoCallStackAdjustment(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr ,
			    adjust ,
			    confidence
		    );
	    }

	    /// <summary>
	    /// Sets the auto-analysis call type adjustment at the specified call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="type">The type with confidence to set as the call type adjustment.</param>
	    public void SetAutoCallTypeAdjustment(Architecture arch , ulong addr , TypeWithConfidence type)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(type.ToNative());

			    NativeMethods.BNSetAutoCallTypeAdjustment(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    nativePtr
			    );
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis call register stack adjustments at the specified call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="adjustments">The array of register stack adjustments to apply.</param>
	    public void SetAutoCallRegisterStackAdjustment(Architecture arch , ulong addr , RegisterStackAdjustment[] adjustments)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNRegisterStackAdjustment[] nativeArray = new BNRegisterStackAdjustment[adjustments.Length];

			    for (int i = 0; i < adjustments.Length; i++)
			    {
				    nativeArray[i].regStack = adjustments[i].RegStack;
				    nativeArray[i].adjustment = adjustments[i].Adjustment;
				    nativeArray[i].confidence = adjustments[i].Confidence;
			    }

			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNSetAutoCallRegisterStackAdjustment(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    arrayPtr ,
				    (ulong)adjustments.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Sets the auto-analysis call register stack adjustment for a specific register stack at a call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="regStack">The register stack identifier.</param>
	    /// <param name="adjust">The stack adjustment value.</param>
	    /// <param name="confidence">The confidence level of the adjustment.</param>
	    public void SetAutoCallRegisterStackAdjustmentForRegisterStack(Architecture arch , ulong addr , uint regStack , int adjust , byte confidence)
	    {
		    NativeMethods.BNSetAutoCallRegisterStackAdjustmentForRegisterStack(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr ,
			    regStack ,
			    adjust ,
			    confidence
		    );
	    }

	    /// <summary>
	    /// Sets the auto-analysis function-level register stack adjustments.
	    /// </summary>
	    /// <param name="adjustments">The array of register stack adjustments to apply at the function level.</param>
	    public void SetAutoFunctionRegisterStackAdjustments(RegisterStackAdjustment[] adjustments)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNRegisterStackAdjustment[] nativeArray = new BNRegisterStackAdjustment[adjustments.Length];

			    for (int i = 0; i < adjustments.Length; i++)
			    {
				    nativeArray[i].regStack = adjustments[i].RegStack;
				    nativeArray[i].adjustment = adjustments[i].Adjustment;
				    nativeArray[i].confidence = adjustments[i].Confidence;
			    }

			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNSetAutoFunctionRegisterStackAdjustments(
				    this.handle ,
				    arrayPtr ,
				    (ulong)adjustments.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Sets the user-defined call stack adjustment at the specified call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="adjust">The stack adjustment value in bytes.</param>
	    /// <param name="confidence">The confidence level of the adjustment.</param>
	    public void SetUserCallStackAdjustment(Architecture arch , ulong addr , long adjust , byte confidence)
	    {
		    NativeMethods.BNSetUserCallStackAdjustment(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr ,
			    adjust ,
			    confidence
		    );
	    }

	    /// <summary>
	    /// Sets the user-defined call type adjustment at the specified call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="type">The type with confidence to set as the call type adjustment.</param>
	    public void SetUserCallTypeAdjustment(Architecture arch , ulong addr , TypeWithConfidence type)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr nativePtr = allocator.AllocStruct(type.ToNative());

			    NativeMethods.BNSetUserCallTypeAdjustment(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    nativePtr
			    );
		    }
	    }

	    /// <summary>
	    /// Sets the user-defined call register stack adjustments at the specified call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="adjustments">The array of register stack adjustments to apply.</param>
	    public void SetUserCallRegisterStackAdjustment(Architecture arch , ulong addr , RegisterStackAdjustment[] adjustments)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNRegisterStackAdjustment[] nativeArray = new BNRegisterStackAdjustment[adjustments.Length];

			    for (int i = 0; i < adjustments.Length; i++)
			    {
				    nativeArray[i].regStack = adjustments[i].RegStack;
				    nativeArray[i].adjustment = adjustments[i].Adjustment;
				    nativeArray[i].confidence = adjustments[i].Confidence;
			    }

			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNSetUserCallRegisterStackAdjustment(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    arrayPtr ,
				    (ulong)adjustments.Length
			    );
		    }
	    }

	    /// <summary>
	    /// Sets the user-defined call register stack adjustment for a specific register stack at a call site.
	    /// </summary>
	    /// <param name="arch">The architecture context for the call site.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <param name="regStack">The register stack identifier.</param>
	    /// <param name="adjust">The stack adjustment value.</param>
	    /// <param name="confidence">The confidence level of the adjustment.</param>
	    public void SetUserCallRegisterStackAdjustmentForRegisterStack(Architecture arch , ulong addr , uint regStack , int adjust , byte confidence)
	    {
		    NativeMethods.BNSetUserCallRegisterStackAdjustmentForRegisterStack(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    addr ,
			    regStack ,
			    adjust ,
			    confidence
		    );
	    }

	    /// <summary>
	    /// Sets the user-defined function-level register stack adjustments.
	    /// </summary>
	    /// <param name="adjustments">The array of register stack adjustments to apply at the function level.</param>
	    public void SetUserFunctionRegisterStackAdjustments(RegisterStackAdjustment[] adjustments)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // convert the managed array to native struct array
			    BNRegisterStackAdjustment[] nativeArray = new BNRegisterStackAdjustment[adjustments.Length];

			    for (int i = 0; i < adjustments.Length; i++)
			    {
				    nativeArray[i].regStack = adjustments[i].RegStack;
				    nativeArray[i].adjustment = adjustments[i].Adjustment;
				    nativeArray[i].confidence = adjustments[i].Confidence;
			    }

			    IntPtr arrayPtr = allocator.AllocStructArray(nativeArray);

			    NativeMethods.BNSetUserFunctionRegisterStackAdjustments(
				    this.handle ,
				    arrayPtr ,
				    (ulong)adjustments.Length
			    );
		    }
	    }

	    // ===================================================================
	    // IL flag operations
	    // ===================================================================

	    /// <summary>
	    /// Gets the flags read by the lifted IL instruction at the specified index.
	    /// </summary>
	    /// <param name="i">The index of the lifted IL instruction.</param>
	    /// <returns>An array of flag register identifiers read by the instruction.</returns>
	    public uint[] GetFlagsReadByLiftedILInstruction(ulong i)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetFlagsReadByLiftedILInstruction(
				    this.handle ,
				    i ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the flags written by the lifted IL instruction at the specified index.
	    /// </summary>
	    /// <param name="i">The index of the lifted IL instruction.</param>
	    /// <returns>An array of flag register identifiers written by the instruction.</returns>
	    public uint[] GetFlagsWrittenByLiftedILInstruction(ulong i)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetFlagsWrittenByLiftedILInstruction(
				    this.handle ,
				    i ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the lifted IL instruction indices that define the specified flag for a given use.
	    /// </summary>
	    /// <param name="i">The index of the lifted IL instruction that uses the flag.</param>
	    /// <param name="flag">The flag register identifier.</param>
	    /// <returns>An array of instruction indices that define the flag.</returns>
	    public ulong[] GetLiftedILFlagDefinitionsForUse(ulong i , uint flag)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetLiftedILFlagDefinitionsForUse(
				    this.handle ,
				    i ,
				    flag ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeNumberArray<ulong>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeILInstructionList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the lifted IL instruction indices that use the specified flag from a given definition.
	    /// </summary>
	    /// <param name="i">The index of the lifted IL instruction that defines the flag.</param>
	    /// <param name="flag">The flag register identifier.</param>
	    /// <returns>An array of instruction indices that use the flag.</returns>
	    public ulong[] GetLiftedILFlagUsesForDefinition(ulong i , uint flag)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetLiftedILFlagUsesForDefinition(
				    this.handle ,
				    i ,
				    flag ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeNumberArray<ulong>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeILInstructionList
			    );
		    }
	    }

	    // ===================================================================
	    // Integer constant display type
	    // ===================================================================

	    /// <summary>
	    /// Gets the display type for an integer constant at the specified instruction address.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="instrAddr">The address of the instruction containing the constant.</param>
	    /// <param name="value">The integer constant value.</param>
	    /// <param name="operand">The operand index within the instruction.</param>
	    /// <returns>The integer display type for the constant.</returns>
	    public IntegerDisplayType GetIntegerConstantDisplayType(Architecture arch , ulong instrAddr , ulong value , ulong operand)
	    {
		    return NativeMethods.BNGetIntegerConstantDisplayType(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    instrAddr ,
			    value ,
			    operand
		    );
	    }

	    /// <summary>
	    /// Gets the enumeration type name used for displaying an integer constant at the specified instruction address.
	    /// Returns null if no enumeration type is associated.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="instrAddr">The address of the instruction containing the constant.</param>
	    /// <param name="value">The integer constant value.</param>
	    /// <param name="operand">The operand index within the instruction.</param>
	    /// <returns>The enumeration type name string, or null if none.</returns>
	    public string? GetIntegerConstantDisplayTypeEnumerationType(Architecture arch , ulong instrAddr , ulong value , ulong operand)
	    {
		    IntPtr resultPtr = NativeMethods.BNGetIntegerConstantDisplayTypeEnumerationType(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    instrAddr ,
			    value ,
			    operand
		    );

		    if (IntPtr.Zero == resultPtr)
		    {
			    return null;
		    }

		    // Core returns an owned UTF-8 char* (the C++ API frees it with BNFreeString);
		    // TakeUtf8String decodes as UTF-8 and frees, matching that ownership.
		    return UnsafeUtils.TakeUtf8String(resultPtr);
	    }

	    /// <summary>
	    /// Sets the display type for an integer constant at the specified instruction address.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="instrAddr">The address of the instruction containing the constant.</param>
	    /// <param name="value">The integer constant value.</param>
	    /// <param name="operand">The operand index within the instruction.</param>
	    /// <param name="type">The integer display type to apply.</param>
	    /// <param name="typeID">The type identifier string for enumeration display types.</param>
	    public void SetIntegerConstantDisplayType(Architecture arch , ulong instrAddr , ulong value , ulong operand , IntegerDisplayType type , string typeID)
	    {
		    NativeMethods.BNSetIntegerConstantDisplayType(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    instrAddr ,
			    value ,
			    operand ,
			    type ,
			    typeID
		    );
	    }

	    // ===================================================================
	    // Address tags of type
	    // ===================================================================

	    /// <summary>
	    /// Gets the address tags of the specified type at the given architecture and address.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="addr">The address to retrieve tags for.</param>
	    /// <param name="tagType">The tag type to filter by.</param>
	    /// <returns>An array of tags matching the specified type at the address.</returns>
	    public Tag[] GetAddressTagsOfType(Architecture arch , ulong addr , TagType tagType)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // allocate a ulong for the count output parameter
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetAddressTagsOfType(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    tagType.DangerousGetHandle() ,
				    countPtr
			    );

			    // read the count from the allocated pointer
			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeHandleArrayEx<Tag>(
				    arrayPointer ,
				    arrayLength ,
				    Tag.MustNewFromHandle ,
				    NativeMethods.BNFreeTagList
			    );
		    }
	    }

	    // ===================================================================
	    // Provenance
	    // ===================================================================

	    /// <summary>
	    /// Gets the provenance string for this function, which describes its origin or source.
	    /// </summary>
	    /// <returns>The provenance string.</returns>
	    public string GetProvenanceString()
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetProvenanceString(this.handle)
		    );
	    }

	    // ===================================================================
	    // Condition inversion
	    // ===================================================================

	    /// <summary>
	    /// Checks whether the branch condition at the specified address is inverted.
	    /// </summary>
	    /// <param name="addr">The address of the conditional branch instruction.</param>
	    /// <returns>True if the condition is inverted.</returns>
	    public bool IsConditionInverted(ulong addr)
	    {
		    return NativeMethods.BNIsConditionInverted(this.handle , addr);
	    }

	    // ===================================================================
	    // Workflow report
	    // ===================================================================

	    /// <summary>
	    /// Shows a workflow report for this function with the specified name.
	    /// </summary>
	    /// <param name="name">The name of the workflow report to display.</param>
	    public void ShowWorkflowReportForFunction(string name)
	    {
		    NativeMethods.BNShowWorkflowReportForFunction(this.handle , name);
	    }

	    // ===================================================================
	    // Constant data
	    // ===================================================================

	    /// <summary>
	    /// Gets constant data from the function for the specified register value state.
	    /// </summary>
	    /// <param name="state">The register value type describing the constant source.</param>
	    /// <param name="value">The constant value.</param>
	    /// <param name="size">The size of the constant data in bytes.</param>
	    /// <returns>A DataBuffer containing the constant data, or null if not found.</returns>
	    public DataBuffer? GetConstantData(RegisterValueType state , ulong value , ulong size)
	    {
		    IntPtr raw = NativeMethods.BNGetConstantData(
			    this.handle ,
			    state ,
			    value ,
			    size ,
			    out BuiltinType builtin
		    );

		    return DataBuffer.TakeHandle(raw);
	    }

	    // ===================================================================
	    // Constants referenced by instruction
	    // ===================================================================

	    /// <summary>
	    /// Gets the list of constants referenced by the instruction at the given address.
	    /// </summary>
	    /// <param name="arch">The architecture context for the instruction.</param>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <returns>An array of ConstantReference values referenced by the instruction.</returns>
	    public unsafe ConstantReference[] GetConstantsReferencedByInstruction(Architecture arch , ulong addr)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetConstantsReferencedByInstruction(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArray<BNConstantReference , ConstantReference>(
				    arrayPointer ,
				    arrayLength ,
				    ConstantReference.FromNative ,
				    NativeMethods.BNFreeConstantReferenceList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the list of constants referenced by the instruction at the given address,
	    /// if available without triggering additional analysis.
	    /// </summary>
	    /// <param name="arch">The architecture context for the instruction.</param>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <returns>An array of ConstantReference values referenced by the instruction.</returns>
	    public unsafe ConstantReference[] GetConstantsReferencedByInstructionIfAvailable(Architecture arch , ulong addr)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetConstantsReferencedByInstructionIfAvailable(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArray<BNConstantReference , ConstantReference>(
				    arrayPointer ,
				    arrayLength ,
				    ConstantReference.FromNative ,
				    NativeMethods.BNFreeConstantReferenceList
			    );
		    }
	    }

	    // ===================================================================
	    // User variable values
	    // ===================================================================

	    /// <summary>
	    /// Gets all user-defined variable values for this function.
	    /// </summary>
	    /// <returns>An array of UserVariableValue describing each user-set variable value.</returns>
	    public unsafe UserVariableValue[] GetAllUserVariableValues()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetAllUserVariableValues(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    UserVariableValue[] results = UnsafeUtils.ReadStructArray<BNUserVariableValue , UserVariableValue>(
				    arrayPointer ,
				    arrayLength ,
				    (BNUserVariableValue native) => new UserVariableValue(native)
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeUserVariableValues(arrayPointer);
			    }

			    return results;
		    }
	    }

	    // ===================================================================
	    // Variable field resolutions
	    // ===================================================================

	    /// <summary>
	    /// Gets all variable field resolution information for this function.
	    /// </summary>
	    /// <returns>An array of VariableFieldResolutionInfo for each resolved variable field.</returns>
	    public unsafe VariableFieldResolutionInfo[] GetAllVariableFieldResolutions()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetAllVariableFieldResolutions(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    VariableFieldResolutionInfo[] results = UnsafeUtils.ReadStructArray<BNVariableFieldResolutionInfo , VariableFieldResolutionInfo>(
				    arrayPointer ,
				    arrayLength ,
				    (BNVariableFieldResolutionInfo native) => new VariableFieldResolutionInfo(native)
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeVariableFieldResolutions(arrayPointer , arrayLength);
			    }

			    return results;
		    }
	    }

	    // ===================================================================
	    // Analysis performance info
	    // ===================================================================

	    /// <summary>
	    /// Gets analysis performance information for this function, showing time spent in each analysis phase.
	    /// </summary>
	    /// <returns>An array of PerformanceInfo with the name and duration of each analysis phase.</returns>
	    public unsafe PerformanceInfo[] GetFunctionAnalysisPerformanceInfo()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetFunctionAnalysisPerformanceInfo(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    PerformanceInfo[] results = UnsafeUtils.ReadStructArray<BNPerformanceInfo , PerformanceInfo>(
				    arrayPointer ,
				    arrayLength ,
				    (BNPerformanceInfo native) => new PerformanceInfo()
				    {
					    Name = UnsafeUtils.ReadAnsiString(native.name) ,
					    Seconds = native.seconds
				    }
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeAnalysisPerformanceInfo(arrayPointer , arrayLength);
			    }

			    return results;
		    }
	    }

	    // ===================================================================
	    // Block annotations
	    // ===================================================================

	    /// <summary>
	    /// Gets the block annotation text lines for the instruction at the given address.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <returns>An array of DisassemblyTextLine representing the block annotations.</returns>
	    public unsafe DisassemblyTextLine[] GetFunctionBlockAnnotations(Architecture arch , ulong addr)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetFunctionBlockAnnotations(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
				    arrayPointer ,
				    arrayLength ,
				    DisassemblyTextLine.FromNative ,
				    NativeMethods.BNFreeDisassemblyTextLines
			    );
		    }
	    }

	    // ===================================================================
	    // Register stack adjustments (function-level)
	    // ===================================================================

	    /// <summary>
	    /// Gets the register stack adjustments for this function at all call sites.
	    /// </summary>
	    /// <returns>An array of RegisterStackAdjustment describing each adjustment.</returns>
	    public unsafe RegisterStackAdjustment[] GetFunctionRegisterStackAdjustments()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetFunctionRegisterStackAdjustments(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    RegisterStackAdjustment[] results = UnsafeUtils.ReadStructArray<BNRegisterStackAdjustment , RegisterStackAdjustment>(
				    arrayPointer ,
				    arrayLength ,
				    (BNRegisterStackAdjustment native) => new RegisterStackAdjustment()
				    {
					    RegStack = native.regStack ,
					    Adjustment = native.adjustment ,
					    Confidence = native.confidence
				    }
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeRegisterStackAdjustments(arrayPointer);
			    }

			    return results;
		    }
	    }

	    // ===================================================================
	    // Indirect branches at address
	    // ===================================================================

	    /// <summary>
	    /// Gets the indirect branch targets at the specified address.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="addr">The address of the indirect branch instruction.</param>
	    /// <returns>An array of IndirectBranchInfo for each resolved target.</returns>
	    public unsafe IndirectBranchInfo[] GetIndirectBranchesAt(Architecture arch , ulong addr)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetIndirectBranchesAt(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArray<BNIndirectBranchInfo , IndirectBranchInfo>(
				    arrayPointer ,
				    arrayLength ,
				    IndirectBranchInfo.FromNative ,
				    NativeMethods.BNFreeIndirectBranchList
			    );
		    }
	    }

	    // ===================================================================
	    // Stack variables referenced by instruction
	    // ===================================================================

	    /// <summary>
	    /// Gets the stack variables referenced by the instruction at the given address.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <returns>An array of StackVariableReference for each referenced stack variable.</returns>
	    public unsafe StackVariableReference[] GetStackVariablesReferencedByInstruction(Architecture arch , ulong addr)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetStackVariablesReferencedByInstruction(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    StackVariableReference[] results = UnsafeUtils.ReadStructArray<BNStackVariableReference , StackVariableReference>(
				    arrayPointer ,
				    arrayLength ,
				    (BNStackVariableReference native) => new StackVariableReference()
				    {
					    SourceOperand = native.sourceOperand ,
					    TypeConfidence = native.typeConfidence ,
					    Type = (native.type != IntPtr.Zero)
						    ? new BinaryNinja.Type(NativeMethods.BNNewTypeReference(native.type) , true)
						    : null ,
					    Name = UnsafeUtils.ReadAnsiString(native.name) ,
					    VarIdentifier = native.varIdentifier ,
					    ReferencedOffset = native.referencedOffset ,
					    Size = native.size
				    }
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeStackVariableReferenceList(arrayPointer , arrayLength);
			    }

			    return results;
		    }
	    }

	    /// <summary>
	    /// Gets the stack variables referenced by the instruction at the given address,
	    /// if available without triggering additional analysis.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="addr">The address of the instruction.</param>
	    /// <returns>An array of StackVariableReference for each referenced stack variable.</returns>
	    public unsafe StackVariableReference[] GetStackVariablesReferencedByInstructionIfAvailable(Architecture arch , ulong addr)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetStackVariablesReferencedByInstructionIfAvailable(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    StackVariableReference[] results = UnsafeUtils.ReadStructArray<BNStackVariableReference , StackVariableReference>(
				    arrayPointer ,
				    arrayLength ,
				    (BNStackVariableReference native) => new StackVariableReference()
				    {
					    SourceOperand = native.sourceOperand ,
					    TypeConfidence = native.typeConfidence ,
					    Type = (native.type != IntPtr.Zero)
						    ? new BinaryNinja.Type(NativeMethods.BNNewTypeReference(native.type) , true)
						    : null ,
					    Name = UnsafeUtils.ReadAnsiString(native.name) ,
					    VarIdentifier = native.varIdentifier ,
					    ReferencedOffset = native.referencedOffset ,
					    Size = native.size
				    }
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeStackVariableReferenceList(arrayPointer , arrayLength);
			    }

			    return results;
		    }
	    }

	    // ===================================================================
	    // Workflow request
	    // ===================================================================

	    /// <summary>
	    /// Posts a workflow request for this function with the specified request string.
	    /// </summary>
	    /// <param name="request">The workflow request string to post.</param>
	    /// <returns>The response string from the workflow engine.</returns>
	    public string PostWorkflowRequestForFunction(string request)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNPostWorkflowRequestForFunction(this.handle , request)
		    );
	    }

	    // ===================================================================
	    // Call register stack adjustment
	    // ===================================================================

	    /// <summary>
	    /// Gets the register stack adjustments for the call instruction at the given address.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="addr">The address of the call instruction.</param>
	    /// <returns>An array of RegisterStackAdjustment at the specified call site.</returns>
	    public unsafe RegisterStackAdjustment[] GetCallRegisterStackAdjustment(Architecture arch , ulong addr)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetCallRegisterStackAdjustment(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    addr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    RegisterStackAdjustment[] results = UnsafeUtils.ReadStructArray<BNRegisterStackAdjustment , RegisterStackAdjustment>(
				    arrayPointer ,
				    arrayLength ,
				    (BNRegisterStackAdjustment native) => new RegisterStackAdjustment()
				    {
					    RegStack = native.regStack ,
					    Adjustment = native.adjustment ,
					    Confidence = native.confidence
				    }
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeRegisterStackAdjustments(arrayPointer);
			    }

			    return results;
		    }
	    }

	    // ===================================================================
	    // Merged variables
	    // ===================================================================

	    /// <summary>
	    /// Gets all merged variable groups in this function. Each merged variable maps
	    /// a target variable to the set of source variables that were merged into it.
	    /// </summary>
	    /// <returns>An array of MergedVariable describing each merge group.</returns>
	    public unsafe MergedVariable[] GetMergedVariables()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetMergedVariables(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    MergedVariable[] results = UnsafeUtils.ReadStructArray<BNMergedVariable , MergedVariable>(
				    arrayPointer ,
				    arrayLength ,
				    (BNMergedVariable native) => new MergedVariable()
				    {
					    Target = CoreVariable.FromNative(native.target) ,
					    Sources = UnsafeUtils.ReadStructArray<BNVariable , CoreVariable>(
						    native.sources ,
						    native.sourceCount ,
						    CoreVariable.FromNative
					    )
				    }
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeMergedVariableList(arrayPointer , arrayLength);
			    }

			    return results;
		    }
	    }

	    // ===================================================================
	    // Split variables
	    // ===================================================================

	    /// <summary>
	    /// Gets the list of variables that have been split in this function.
	    /// </summary>
	    /// <returns>An array of CoreVariable for each split variable.</returns>
	    public unsafe CoreVariable[] GetSplitVariables()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNGetSplitVariables(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    CoreVariable[] results = UnsafeUtils.ReadStructArray<BNVariable , CoreVariable>(
				    arrayPointer ,
				    arrayLength ,
				    CoreVariable.FromNative
			    );

			    if (arrayPointer != IntPtr.Zero)
			    {
				    NativeMethods.BNFreeVariableList(arrayPointer);
			    }

			    return results;
		    }
	    }

	    // ===================================================================
	    // HLIL / MLIL variable references
	    // ===================================================================

	    /// <summary>
	    /// Gets high-level IL variable references for the specified variable.
	    /// </summary>
	    /// <param name="variable">The variable to get references for.</param>
	    /// <returns>An array of ILReferenceSource for each reference location.</returns>
	    public unsafe ILReferenceSource[] GetHighLevelILVariableReferences(Variable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);
			    IntPtr varPtr = allocator.AllocStruct<BNVariable>(variable.ToNative());

			    IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableReferences(
				    this.handle ,
				    varPtr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArrayEx<BNILReferenceSource , ILReferenceSource>(
				    arrayPointer ,
				    arrayLength ,
				    ILReferenceSource.FromNative ,
				    NativeMethods.BNFreeVariableReferenceSourceList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets high-level IL variable references for the specified variable,
	    /// if available without triggering additional analysis.
	    /// </summary>
	    /// <param name="variable">The variable to get references for.</param>
	    /// <returns>An array of ILReferenceSource for each reference location.</returns>
	    public unsafe ILReferenceSource[] GetHighLevelILVariableReferencesIfAvailable(Variable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);
			    IntPtr varPtr = allocator.AllocStruct<BNVariable>(variable.ToNative());

			    IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableReferencesIfAvailable(
				    this.handle ,
				    varPtr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArrayEx<BNILReferenceSource , ILReferenceSource>(
				    arrayPointer ,
				    arrayLength ,
				    ILReferenceSource.FromNative ,
				    NativeMethods.BNFreeVariableReferenceSourceList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets medium-level IL variable references for the specified variable,
	    /// if available without triggering additional analysis.
	    /// </summary>
	    /// <param name="variable">The variable to get references for.</param>
	    /// <returns>An array of ILReferenceSource for each reference location.</returns>
	    public unsafe ILReferenceSource[] GetMediumLevelILVariableReferencesIfAvailable(Variable variable)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);
			    IntPtr varPtr = allocator.AllocStruct<BNVariable>(variable.ToNative());

			    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableReferencesIfAvailable(
				    this.handle ,
				    varPtr ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArrayEx<BNILReferenceSource , ILReferenceSource>(
				    arrayPointer ,
				    arrayLength ,
				    ILReferenceSource.FromNative ,
				    NativeMethods.BNFreeVariableReferenceSourceList
			    );
		    }
	    }

	    // ===================================================================
	    // HLIL / MLIL variable references — from if available
	    // ===================================================================

	    /// <summary>
	    /// Gets HLIL variable reference sources from the given address, if available without
	    /// triggering analysis. Returns an empty array if the data is not yet computed.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="address">The instruction address to query.</param>
	    /// <returns>An array of variable reference sources at the given address.</returns>
	    public unsafe VariableReferenceSource[] GetHighLevelILVariableReferencesFromIfAvailable(
		    Architecture arch ,
		    ulong address
	    )
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    // 2. Call the native API.
		    IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableReferencesFromIfAvailable(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    address ,
			    (IntPtr)(&count)
		    );

		    // 3. Convert the native array to managed objects.
		    return MarshalVariableReferenceSources(arrayPointer , count);
	    }

	    // ===================================================================
	    // HLIL / MLIL variable references in range
	    // ===================================================================

	    /// <summary>
	    /// Gets HLIL variable reference sources in the given address range.
	    /// Triggers analysis if needed.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="address">The start address of the range.</param>
	    /// <param name="len">The length of the address range in bytes.</param>
	    /// <returns>An array of variable reference sources in the given range.</returns>
	    public unsafe VariableReferenceSource[] GetHighLevelILVariableReferencesInRange(
		    Architecture arch ,
		    ulong address ,
		    ulong len
	    )
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    // 2. Call the native API.
		    IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableReferencesInRange(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    address ,
			    len ,
			    (IntPtr)(&count)
		    );

		    // 3. Convert the native array to managed objects.
		    return MarshalVariableReferenceSources(arrayPointer , count);
	    }

	    /// <summary>
	    /// Gets HLIL variable reference sources in the given address range, if available
	    /// without triggering analysis.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="address">The start address of the range.</param>
	    /// <param name="len">The length of the address range in bytes.</param>
	    /// <returns>An array of variable reference sources in the given range.</returns>
	    public unsafe VariableReferenceSource[] GetHighLevelILVariableReferencesInRangeIfAvailable(
		    Architecture arch ,
		    ulong address ,
		    ulong len
	    )
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    // 2. Call the native API.
		    IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableReferencesInRangeIfAvailable(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    address ,
			    len ,
			    (IntPtr)(&count)
		    );

		    // 3. Convert the native array to managed objects.
		    return MarshalVariableReferenceSources(arrayPointer , count);
	    }

	    /// <summary>
	    /// Gets MLIL variable reference sources from the given address, if available without
	    /// triggering analysis.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="address">The instruction address to query.</param>
	    /// <returns>An array of variable reference sources at the given address.</returns>
	    public unsafe VariableReferenceSource[] GetMediumLevelILVariableReferencesFromIfAvailable(
		    Architecture arch ,
		    ulong address
	    )
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    // 2. Call the native API.
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableReferencesFromIfAvailable(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    address ,
			    (IntPtr)(&count)
		    );

		    // 3. Convert the native array to managed objects.
		    return MarshalVariableReferenceSources(arrayPointer , count);
	    }

	    /// <summary>
	    /// Gets MLIL variable reference sources in the given address range.
	    /// Triggers analysis if needed.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="address">The start address of the range.</param>
	    /// <param name="len">The length of the address range in bytes.</param>
	    /// <returns>An array of variable reference sources in the given range.</returns>
	    public unsafe VariableReferenceSource[] GetMediumLevelILVariableReferencesInRange(
		    Architecture arch ,
		    ulong address ,
		    ulong len
	    )
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    // 2. Call the native API.
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableReferencesInRange(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    address ,
			    len ,
			    (IntPtr)(&count)
		    );

		    // 3. Convert the native array to managed objects.
		    return MarshalVariableReferenceSources(arrayPointer , count);
	    }

	    /// <summary>
	    /// Gets MLIL variable reference sources in the given address range, if available
	    /// without triggering analysis.
	    /// </summary>
	    /// <param name="arch">The architecture context.</param>
	    /// <param name="address">The start address of the range.</param>
	    /// <param name="len">The length of the address range in bytes.</param>
	    /// <returns>An array of variable reference sources in the given range.</returns>
	    public unsafe VariableReferenceSource[] GetMediumLevelILVariableReferencesInRangeIfAvailable(
		    Architecture arch ,
		    ulong address ,
		    ulong len
	    )
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    // 2. Call the native API.
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableReferencesInRangeIfAvailable(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    address ,
			    len ,
			    (IntPtr)(&count)
		    );

		    // 3. Convert the native array to managed objects.
		    return MarshalVariableReferenceSources(arrayPointer , count);
	    }

	    // ===================================================================
	    // Field resolution for variables
	    // ===================================================================

	    /// <summary>
	    /// Gets the field resolution information for a variable at a given definition site.
	    /// Returns the FieldResolutionInfo handle or null if no resolution is available.
	    /// </summary>
	    /// <param name="variable">The variable to query.</param>
	    /// <param name="defSite">The definition site (architecture + address) for the variable.</param>
	    /// <returns>A FieldResolutionInfo, or null if no resolution exists.</returns>
	    public unsafe FieldResolutionInfo? GetFieldResolutionForVariableAt(
		    CoreVariable variable ,
		    ArchitectureAndAddress defSite
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // 1. Marshal the variable and definition site structs.
			    BNVariable nativeVar = variable.ToNative();
			    BNArchitectureAndAddress nativeDefSite = defSite.ToNative();

			    IntPtr varPtr = allocator.AllocStruct<BNVariable>(nativeVar);
			    IntPtr defSitePtr = allocator.AllocStruct<BNArchitectureAndAddress>(nativeDefSite);

			    // 2. Call the native API.
			    IntPtr result = NativeMethods.BNGetFieldResolutionForVariableAt(
				    this.handle ,
				    varPtr ,
				    defSitePtr
			    );

			    // 3. Wrap the returned handle (takes ownership).
			    return FieldResolutionInfo.TakeHandle(result);
		    }
	    }

	    // ===================================================================
	    // Remove auto function tags of type
	    // ===================================================================

	    /// <summary>
	    /// Removes all auto function tags of the given tag type from this function.
	    /// </summary>
	    /// <param name="tagType">The tag type whose auto function tags should be removed.</param>
	    public void RemoveAutoFunctionTagsOfType(TagType tagType)
	    {
		    // Forward to the native API with the function and tag type handles.
		    NativeMethods.BNRemoveAutoFunctionTagsOfType(
			    this.handle ,
			    tagType.DangerousGetHandle()
		    );
	    }

	    // ===================================================================
	    // Private helpers
	    // ===================================================================

	    /// <summary>
	    /// Marshals a native BNVariableReferenceSource array into managed VariableReferenceSource[].
	    /// Each BNVariableReferenceSource contains a BNVariable (converted to a Function-bound Variable)
	    /// and a BNILReferenceSource. The native array is freed after marshalling.
	    /// </summary>
	    /// <param name="arrayPointer">Pointer to the native BNVariableReferenceSource array.</param>
	    /// <param name="count">The number of elements in the array.</param>
	    /// <returns>An array of managed VariableReferenceSource objects.</returns>
	    private unsafe VariableReferenceSource[] MarshalVariableReferenceSources(IntPtr arrayPointer , ulong count)
	    {
		    // 1. Handle empty result.
		    if (IntPtr.Zero == arrayPointer || 0 == count)
		    {
			    return Array.Empty<VariableReferenceSource>();
		    }

		    // 2. Cast to native struct pointer for indexed access.
		    BNVariableReferenceSource* rawArray = (BNVariableReferenceSource*)arrayPointer;

		    // 3. Build managed array.
		    VariableReferenceSource[] result = new VariableReferenceSource[(int)count];

		    for (ulong i = 0; i < count; i++)
		    {
			    // 3.1 Create a Function-bound Variable from the native BNVariable.
			    Variable variable = Variable.FromNativeEx(this , rawArray[i].variable);

			    // 3.2 Convert the ILReferenceSource part.
			    ILReferenceSource source = ILReferenceSource.FromNative(rawArray[i].source);

			    // 3.3 Combine into the managed wrapper.
			    result[i] = new VariableReferenceSource(variable , source);
		    }

		    // 4. Free the native array.
		    NativeMethods.BNFreeVariableReferenceSourceList(arrayPointer , count);

		    return result;
	    }

	}
}