using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class BinaryView : AbstractSafeHandle<BinaryView> , IEnumerable<Function>
	{
		/// <summary>
		/// Iterates the analysis functions in this view, so that
		/// <c>foreach (Function function in view)</c> works. Mirrors Python's
		/// <c>for func in bv</c>.
		/// </summary>
		public IEnumerator<Function> GetEnumerator()
		{
			return ((IEnumerable<Function>)this.Functions).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		internal BinaryView(IntPtr handle , bool owner) 
			: base(handle , owner)
		{
			
		}
		
		internal static BinaryView? TakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new BinaryView(handle, true);
		}
	    
		internal static BinaryView MustTakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new BinaryView(handle, true);
		}
	    
		internal static BinaryView? BorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new BinaryView(handle, false);
		}
	    
		internal static BinaryView MustBorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new BinaryView(handle, false);
		}

		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				NativeMethods.BNFreeBinaryView(this.handle);
				this.SetHandleAsInvalid();
			}

			return true;
		}

		public ulong Length
		{
			get
			{
				return NativeMethods.BNGetViewLength(this.handle);
			}
		}
		
		public static BinaryView? FromFile(
			string filename ,
			FileMetadata? file = null
		)
		{
			FileMetadata? autoFile = file;

			if (null == autoFile)
			{
				autoFile = new FileMetadata(filename);
			}

			return BinaryView.TakeHandle(
				NativeMethods.BNCreateBinaryDataViewFromFilename(
					autoFile.DangerousGetHandle() ,
					filename
				)
			);
		}

		public static BinaryView? Create(FileMetadata file)
		{
			return BinaryView.TakeHandle(
				NativeMethods.BNCreateBinaryDataView(file.DangerousGetHandle())
			);
		}

		public static BinaryView? FromBuffer( DataBuffer buffer , FileMetadata? file = null )
		{
			if (null == file)
			{
				file = new FileMetadata();
			}
			
			return BinaryView.TakeHandle(
				NativeMethods.BNCreateBinaryDataViewFromBuffer(
					file.DangerousGetHandle() ,
					buffer.DangerousGetHandle()
				)
			);
		}

		public static BinaryView? FromBytes(byte[] data , FileMetadata? file = null )
		{
			if (null == file)
			{
				file = new FileMetadata();
			}
		
			return BinaryView.TakeHandle(
				NativeMethods.BNCreateBinaryDataViewFromData(
					file.DangerousGetHandle() ,
					data,
					(ulong)data.Length
				)
			);
		}

		public BinaryView? Load(
			bool updateAnalysis ,
			string options ,
			ProgressDelegate? progress
		)
		{
			NativeDelegates.BNProgressFunction? progressWrapper =
				null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

			BinaryView? result = BinaryView.TakeHandle(
				NativeMethods.BNLoadBinaryView(
					this.handle ,
					updateAnalysis ,
					options ,
					null == progressWrapper
						? IntPtr.Zero
						: Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
					IntPtr.Zero
				)
			);

			GC.KeepAlive(progressWrapper);

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename">like afd.sys.bndb</param>
		/// <param name="updateAnalysis"></param>
		/// <param name="options"></param>
		/// <param name="progress"></param>
		/// <returns></returns>
		public static BinaryView? LoadFile(
			string filename ,
			bool updateAnalysis = false ,
			string options = "",
			ProgressDelegate? progress = null
		)
		{
			NativeDelegates.BNProgressFunction? progressWrapper =
				null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

			BinaryView? result = BinaryView.TakeHandle(
				NativeMethods.BNLoadFilename(
					filename ,
					updateAnalysis ,
					options ,
					null == progressWrapper
						? IntPtr.Zero
						: Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
					IntPtr.Zero
				)
			);

			GC.KeepAlive(progressWrapper);

			return result;
		}

		public static BinaryView? OpenExisting(string filename , ProgressDelegate? progress = null)
		{
			FileMetadata file = new FileMetadata(filename);
			
			if (null == progress)
			{
				return BinaryView.TakeHandle(
					NativeMethods.BNOpenExistingDatabase(
						file.DangerousGetHandle() ,
						filename
					)
				);
			}
			else
			{
				NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

				BinaryView? result = BinaryView.TakeHandle(
					NativeMethods.BNOpenExistingDatabaseWithProgress(
						file.DangerousGetHandle() ,
						filename,
						IntPtr.Zero,
						Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper)
					)
				);

				GC.KeepAlive(progressWrapper);

				return result;
			}
		}
		
		public BinaryView? Parent
		{
			get
			{
				return BinaryView.TakeHandle(
					NativeMethods.BNGetParentView(this.handle)
				);
			}
		}

		public FileMetadata File
		{
			get
			{
				return FileMetadata.MustTakeHandle(
					NativeMethods.BNGetFileForView(this.handle)
				);
			}
		}

		/// <summary>
		/// Sibling BinaryView of another view type (for example "Raw" or "PE") backed by the
		/// same file. Mirrors Python <c>BinaryView.get_view_of_type</c>. Returns null when the
		/// file has no view of that type.
		/// </summary>
		public BinaryView? GetViewOfType(string viewType)
		{
			return this.File.GetFileViewOfType(viewType);
		}

		/// <summary>
		/// The raw ("Raw") BinaryView backed by the same file. Mirrors Python <c>BinaryView.raw</c>.
		/// </summary>
		public BinaryView? RawView
		{
			get
			{
				return this.File.RawBinaryView;
			}
		}

		/// <summary>
		/// The ProjectFile backing this view, if it belongs to a project.
		/// Mirrors Python <c>BinaryView.project_file</c>.
		/// </summary>
		public ProjectFile? ProjectFile
		{
			get
			{
				return this.File.ProjectFile;
			}
		}

		public ulong ImageBase
		{
			get
			{
				return NativeMethods.BNGetImageBase(this.handle);
			}
		}

		public ulong OriginalImageBase
		{
			get
			{
				return NativeMethods.BNGetOriginalImageBase(this.handle);
			}

			set
			{
				NativeMethods.BNSetOriginalImageBase(this.handle , value);
			}
		}

		public ulong Start
		{
			get
			{
				return NativeMethods.BNGetStartOffset(this.handle);
			}
		}

		public ulong End
		{
			get
			{
				return NativeMethods.BNGetEndOffset(this.handle);
			}
		}

		public ulong EntryPoint
		{
			get
			{
				return NativeMethods.BNGetEntryPoint(this.handle);
			}
		}

		public Architecture? DefaultArchitecture
		{
			get
			{
				return Architecture.FromHandle(
					NativeMethods.BNGetDefaultArchitecture(this.handle)
				);
			}

			set
			{
				NativeMethods.BNSetDefaultArchitecture(
					this.handle ,
					null == value ? IntPtr.Zero : value.DangerousGetHandle()
				);
			}
		}

		public Platform? DefaultPlatform
		{
			get
			{
				IntPtr raw = NativeMethods.BNGetDefaultPlatform(this.handle);

				if (IntPtr.Zero == raw)
				{
					return null;
				}

				return new Platform(raw , true);
			}

			set
			{
				NativeMethods.BNSetDefaultPlatform(
					this.handle ,
					null == value ? IntPtr.Zero : value.DangerousGetHandle()
				);
			}
		}

		public Endianness DefaultEndiannes
		{
			get
			{
				return NativeMethods.BNGetDefaultEndianness(this.handle);
			}
		}

		public bool Relocatable
		{
			get
			{
				return NativeMethods.BNIsRelocatable(this.handle);
			}
		}

		public ulong AddressSize
		{
			get
			{
				return NativeMethods.BNGetViewAddressSize(this.handle);
			}
		}

		public bool Modified
		{
			get
			{
				return NativeMethods.BNIsViewModified(this.handle);
			}
		}
		
		public bool Executable
		{
			get
			{
				return NativeMethods.BNIsExecutableView(this.handle);
			}
		}

		public Function[] Functions
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetAnalysisFunctionList(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeHandleArrayEx<Function>(
					arrayPointer ,
					arrayLength ,
					Function.MustNewFromHandle ,
					NativeMethods.BNFreeFunctionList
				);
			}
		}
		
		public IEnumerable<BasicBlock> BasicBlocks
		{
			get
			{
				foreach (Function function in this.Functions)
				{
					foreach (BasicBlock basicBlock in function.BasicBlocks)
					{
						yield return basicBlock;
					}
				}
			}
		}
		
		public IEnumerable<LowLevelILFunction> LowLevelILFunctions
		{
			get
			{
				foreach (Function function in this.Functions)
				{
					yield return function.LowLevelIL;
				}
			}
		}
		
		public IEnumerable<LowLevelILBasicBlock> LowLevelILBasicBlocks
		{
			get
			{
				foreach (LowLevelILFunction function in this.LowLevelILFunctions)
				{
					foreach (LowLevelILBasicBlock basicBlock in function.BasicBlocks)
					{
						yield return basicBlock;
					}
				}
			}
		}
		
		public IEnumerable<LowLevelILFunction> LiftedILFunctions
		{
			get
			{
				foreach (Function function in this.Functions)
				{
					yield return function.LiftedIL;
				}
			}
		}
		
		public IEnumerable<LowLevelILBasicBlock> LiftedILBasicBlocks
		{
			get
			{
				foreach (LowLevelILFunction function in this.LiftedILFunctions)
				{
					foreach (LowLevelILBasicBlock basicBlock in function.BasicBlocks)
					{
						yield return basicBlock;
					}
				}
			}
		}
		
		public IEnumerable<MediumLevelILFunction> MediumLevelILFunctions
		{
			get
			{
				foreach (Function function in this.Functions)
				{
					yield return function.MediumLevelIL;
				}
			}
		}
		
		public IEnumerable<MediumLevelILBasicBlock> MediumLevelILBasicBlocks
		{
			get
			{
				foreach (MediumLevelILFunction function in this.MediumLevelILFunctions)
				{
					foreach (MediumLevelILBasicBlock basicBlock in function.BasicBlocks)
					{
						yield return basicBlock;
					}
				}
			}
		}
		
		public IEnumerable<MediumLevelILFunction> MappedMediumLevelILFunctions
		{
			get
			{
				foreach (Function function in this.Functions)
				{
					yield return function.MappedMediumLevelIL;
				}
			}
		}

		public IEnumerable<MediumLevelILBasicBlock> MappedMediumLevelILBasicBlocks
		{
			get
			{
				foreach (MediumLevelILFunction function in this.MappedMediumLevelILFunctions)
				{
					foreach (MediumLevelILBasicBlock basicBlock in function.BasicBlocks)
					{
						yield return basicBlock;
					}
				}
			}
		}
		
		public IEnumerable<HighLevelILFunction> HighLevelILFunctions
		{
			get
			{
				foreach (Function function in this.Functions)
				{
					yield return function.HighLevelIL;
				}
			}
		}
		
		public IEnumerable<HighLevelILBasicBlock> HighLevelILBasicBlocks
		{
			get
			{
				foreach (HighLevelILFunction function in this.HighLevelILFunctions)
				{
					foreach (HighLevelILBasicBlock basicBlock in function.BasicBlocks)
					{
						yield return basicBlock;
					}
				}
			}
		}
	
		
		public bool HasFunctions
		{
			get
			{
				return NativeMethods.BNHasFunctions(this.handle);
			}
		}

		public bool HasSymbols
		{
			get
			{
				return NativeMethods.BNHasSymbols(this.handle);
			}
		}

		public bool HasDataVariables
		{
			get
			{
				return NativeMethods.BNHasDataVariables(this.handle);
			}
		}

		public Function? EntryFunction
		{
			get
			{
				IntPtr raw = NativeMethods.BNGetAnalysisEntryPoint(this.handle);

				if (IntPtr.Zero == raw)
				{
					return null;
				}

				return new Function(raw , true);
			}
		}

		public Function[] EntryFunctions
		{
			get
			{
				ulong arrayLength = 0;

				IntPtr arrayPointer = NativeMethods.BNGetAllEntryFunctions(this.handle , out arrayLength);

				return UnsafeUtils.TakeHandleArrayEx<Function>(
					arrayPointer ,
					arrayLength ,
					Function.MustNewFromHandle ,
					NativeMethods.BNFreeFunctionList
				);
			}
		}

		public NameSpace[] NameSpaces
		{
			get
			{
				ulong arrayLength = 0;

				IntPtr arrayPointer = NativeMethods.BNGetNameSpaces(this.handle , out arrayLength);

				return UnsafeUtils.TakeStructArrayEx<BNNameSpace , NameSpace>(
					arrayPointer ,
					arrayLength ,
					NameSpace.FromNative ,
					NativeMethods.BNFreeNameSpaceList
				);
			}
		}

		public string ViewType
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(
					NativeMethods.BNGetViewType(this.handle)
				);
			}
		}

		public BinaryViewType[] ViewTypes
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetBinaryViewTypesForData(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeHandleArray<BinaryViewType>(
					arrayPointer ,
					arrayLength ,
					BinaryViewType.MustFromHandle ,
					NativeMethods.BNFreeBinaryViewTypeList
				);
			}
		}

		public StringReference[] Strings
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetStrings(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArray<BNStringReference , StringReference>(
					arrayPointer ,
					arrayLength ,
					StringReference.FromNative ,
					NativeMethods.BNFreeStringReferenceList
				);
			}
		}

		public AnalysisInfo GetAnalysisInfo()
		{
			return AnalysisInfo.MustTakeNativePointer(
				NativeMethods.BNGetAnalysisInfo(this.handle)
			);
		}

		public AnalysisProgress AnalysisProgress
		{
			get
			{
				return AnalysisProgress.FromNative(NativeMethods.BNGetAnalysisProgress(this.handle));
			}
		}

		public AnalysisState AnalysisState
		{
			get
			{
				return NativeMethods.BNGetAnalysisState(this.handle);
			}
		}

		public LinearViewObject CreateLinearViewDisassembly(
			DisassemblySettings? settings = null
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}

			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewDisassembly(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}
		
		public LinearViewObject CreateLinearViewLiftedIL(
			DisassemblySettings? settings = null
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewLiftedIL(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewLowLevelIL(
			DisassemblySettings? settings = null
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewLowLevelIL(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewLowLevelILSSAForm(
			DisassemblySettings? settings = null
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewLowLevelILSSAForm(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewMediumLevelIL(
			DisassemblySettings? settings = null
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewMediumLevelIL(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewMediumLevelILSSAForm(
			DisassemblySettings? settings = null
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewMediumLevelILSSAForm(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewMappedMediumLevelIL(
			DisassemblySettings? settings = null
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewMappedMediumLevelIL(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewMappedMediumLevelILSSAForm(
			DisassemblySettings? settings = null
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewMappedMediumLevelILSSAForm(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewHighLevelIL(DisassemblySettings? settings = null)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewHighLevelIL(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewHighLevelILSSAForm(DisassemblySettings? settings = null)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewHighLevelILSSAForm(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public LinearViewObject CreateLinearViewLanguageRepresentation(
			DisassemblySettings? settings = null ,
			string language = "Pseudo C"
		)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewLanguageRepresentation(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle() ,
					language
				)
			);
		}

		public LinearViewObject CreateLinearViewDataOnly(DisassemblySettings? settings = null)
		{
			if (null == settings)
			{
				settings = DisassemblySettings.Default();
			}
			
			return LinearViewObject.MustTakeHandle(
				NativeMethods.BNCreateLinearViewDataOnly(
					this.handle ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				)
			);
		}

		public DataVariable[] DataVariables
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetDataVariables(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNDataVariable , DataVariable>(
					arrayPointer ,
					arrayLength ,
					native => DataVariable.FromNative(native , this) ,
					NativeMethods.BNFreeDataVariables
				);
			}
		}

		public QualifiedNameAndType[] Types
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetAnalysisTypeList(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNQualifiedNameAndType , QualifiedNameAndType>(
					arrayPointer ,
					arrayLength ,
					QualifiedNameAndType.FromNative ,
					NativeMethods.BNFreeTypeAndNameList
				);
			}
		}

		public QualifiedNameAndType[] DependencySortedTypes
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetAnalysisDependencySortedTypeList(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNQualifiedNameAndType , QualifiedNameAndType>(
					arrayPointer ,
					arrayLength ,
					QualifiedNameAndType.FromNative ,
					NativeMethods.BNFreeTypeAndNameList
				);
			}
		}

		public QualifiedName[] GetTypeNames(string match)
		{
			IntPtr arrayPointer = NativeMethods.BNGetAnalysisTypeNames(
				this.handle ,
				out ulong arrayLength ,
				match
			);

			return UnsafeUtils.TakeStructArrayEx<BNQualifiedName , QualifiedName>(
				arrayPointer ,
				arrayLength ,
				QualifiedName.FromNative ,
				NativeMethods.BNFreeTypeNameList
			);
		}

		public TypeLibrary[] TypeLibraries
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetBinaryViewTypeLibraries(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeHandleArrayEx<TypeLibrary>(
					arrayPointer ,
					arrayLength ,
					TypeLibrary.MustNewFromHandle ,
					NativeMethods.BNFreeTypeLibraryList
				);
			}
		}

		public IDictionary<string , string> TypeArchives
		{
			get
			{
				ulong arrayLength = NativeMethods.BNBinaryViewGetTypeArchives(
					this.handle ,
					out IntPtr idArrayPointer ,
					out IntPtr pathArrayPointer
				);

				string[] ids = UnsafeUtils.TakeAnsiStringArray(
					idArrayPointer ,
					arrayLength,
					NativeMethods.BNFreeStringList
				);

				string[] paths = UnsafeUtils.TakeAnsiStringArray(
					pathArrayPointer ,
					arrayLength,
					NativeMethods.BNFreeStringList
				);

				Dictionary<string , string> targets = new Dictionary<string , string>();

				for (ulong i = 0; i < arrayLength; i++)
				{
					targets[ids[i]] = paths[i];
				}

				return targets;
			}
		}

		public TypeArchive? GetTypeArchive(string id)
		{
			return TypeArchive.TakeHandle(
				NativeMethods.BNBinaryViewGetTypeArchive(this.handle , id)
			);
		}

		public TypeArchive[] ConnectedTypeArchives
		{
			get
			{
				List<TypeArchive> targets = new List<TypeArchive>();

				foreach (string id in this.TypeArchives.Keys)
				{
					TypeArchive? archive = this.GetTypeArchive(id);

					if (null != archive)
					{
						targets.Add(archive);
					}
				}

				return targets.ToArray();
			}
		}

		public Segment[] Segments
		{
			get
			{
				ulong arrayLength = 0;

				IntPtr arrayPointer = NativeMethods.BNGetSegments(this.handle , out arrayLength);

				return UnsafeUtils.TakeHandleArrayEx<Segment>(
					arrayPointer ,
					arrayLength ,
					Segment.MustNewFromHandle ,
					NativeMethods.BNFreeSegmentList
				);
			}
		}

		public Section[] Sections
		{
			get
			{
				ulong arrayLength = 0;

				IntPtr arrayPointer = NativeMethods.BNGetSections(this.handle , out arrayLength);

				return UnsafeUtils.TakeHandleArrayEx<Section>(
					arrayPointer ,
					arrayLength ,
					Section.MustNewFromHandle ,
					NativeMethods.BNFreeSectionList
				);
			}
		}

		public AddressRange[] AllocatedRanges
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetAllocatedRanges(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArray<BNAddressRange , AddressRange>(
					arrayPointer ,
					arrayLength ,
					AddressRange.FromNative ,
					NativeMethods.BNFreeAddressRanges
				);
			}
		}

		public AddressRange[] MappedAddressRange
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetMappedAddressRanges(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArray<BNAddressRange , AddressRange>(
					arrayPointer ,
					arrayLength ,
					AddressRange.FromNative ,
					NativeMethods.BNFreeAddressRanges
				);
			}
		}

		public AddressRange[] BackedAddressRanges
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetBackedAddressRanges(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArray<BNAddressRange , AddressRange>(
					arrayPointer ,
					arrayLength ,
					AddressRange.FromNative ,
					NativeMethods.BNFreeAddressRanges
				);
			}
		}

		public RegisterValueWithConfidenceAndRegister[] GlobalPointerValues
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetGlobalPointerValues(
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

			set
			{
				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					BNRegisterValueWithConfidenceAndRegister[] natives =
						new BNRegisterValueWithConfidenceAndRegister[value.Length];

					for (int i = 0; i < value.Length; i++)
					{
						natives[i] = value[i].ToNative();
					}

					IntPtr nativePtr = allocator.AllocStructArray(natives);

					NativeMethods.BNSetUserGlobalPointerValues(
						this.handle ,
						nativePtr ,
						(UIntPtr)value.Length
					);
				}
			}
		}

		/// <summary>
		/// The user-specified global-pointer register values (read-only).
		/// Empty when the user has not overridden them. Mirrors Python
		/// <c>BinaryView.user_global_pointer_values</c>.
		/// </summary>
		public RegisterValueWithConfidenceAndRegister[] UserGlobalPointerValues
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetUserGlobalPointerValues(
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
		/// The default (analysis-derived) global-pointer register values (read-only).
		/// Mirrors Python <c>BinaryView.default_global_pointer_values</c>.
		/// </summary>
		public RegisterValueWithConfidenceAndRegister[] DefaultGlobalPointerValues
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetDefaultGlobalPointerValues(
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

		public bool UserGlobalPointerValuesSet
		{
			get
			{
				return NativeMethods.BNUserGlobalPointerValuesSet(this.handle);
			}
		}

		public void ClearUserGlobalPointerValues()
		{
			NativeMethods.BNClearUserGlobalPointerValues(this.handle);
		}


		public AnalysisParameters ParametersForAnalysis
		{
			get
			{
				return AnalysisParameters.FromNative(
					NativeMethods.BNGetParametersForAnalysis(this.handle)
				);
			}

			set
			{
				NativeMethods.BNSetParametersForAnalysis(this.handle , value.ToNative());
			}
		}

		public ulong MaxFunctionSizeForAnalysis
		{
			get
			{
				return NativeMethods.BNGetMaxFunctionSizeForAnalysis(this.handle);
			}

			set
			{
				NativeMethods.BNSetMaxFunctionSizeForAnalysis(this.handle , value);
			}
		}

		public Relocation[] GetRelocationsAt(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetRelocationsAt(
				this.handle ,
				address ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<Relocation>(
				arrayPointer ,
				arrayLength ,
				Relocation.MustNewFromHandle ,
				NativeMethods.BNFreeRelocationList
			);
		}

		/// <summary>
		/// The first relocation at or after <paramref name="address"/>, or <c>null</c> if none.
		/// When <paramref name="maxAddr"/> is non-zero the search stops before that address.
		/// Mirrors C++ <c>BinaryView::GetNextRelocation</c>.
		/// </summary>
		public Relocation? GetNextRelocation(ulong address , ulong maxAddr = 0)
		{
			// The core returns an already-owned reference (or NULL), so take ownership directly.
			return Relocation.TakeHandle(
				NativeMethods.BNGetNextRelocation(this.handle , address , maxAddr)
			);
		}

		public Range[] RelocationRanges
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetRelocationRanges(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArray<BNRange , Range>(
					arrayPointer ,
					arrayLength ,
					Range.FromNative ,
					NativeMethods.BNFreeRelocationRanges
				);
			}
		}

		public Range[] GetRelocationRangesAtAddress(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetRelocationRangesAtAddress(
				this.handle ,
				address ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeStructArray<BNRange , Range>(
				arrayPointer ,
				arrayLength ,
				Range.FromNative ,
				NativeMethods.BNFreeRelocationRanges
			);
		}

		public Range[] GetRelocationRangesInRange(ulong address , ulong length)
		{
			IntPtr arrayPointer = NativeMethods.BNGetRelocationRangesInRange(
				this.handle ,
				address ,
				length ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeStructArray<BNRange , Range>(
				arrayPointer ,
				arrayLength ,
				Range.FromNative ,
				NativeMethods.BNFreeRelocationRanges
			);
		}

		public bool FinalizeNewSegments()
		{
			return NativeMethods.BNBinaryViewFinalizeNewSegments(this.handle);
		}

		public bool RangeContainsRelocation(ulong address , ulong length)
		{
			return NativeMethods.BNRangeContainsRelocation(this.handle , address , length);
		}

		public bool NewAutoFunctionAnalysisSuppressed
		{
			get
			{
				return NativeMethods.BNGetNewAutoFunctionAnalysisSuppressed(this.handle);
			}

			set
			{
				NativeMethods.BNSetNewAutoFunctionAnalysisSuppressed(this.handle , value);
			}
		}

		public DataBuffer ReadBuffer(ulong offset , ulong length)
		{
			return DataBuffer.MustTakeHandle(
				NativeMethods.BNReadViewBuffer(this.handle , offset , length)
			);
		}

		public byte[] ReadData(ulong offset , ulong length)
		{
			if (0 == length)
			{
				return Array.Empty<byte>();
			}
			
			byte[] buffer = new byte[length];


			ulong readed = NativeMethods.BNReadViewData(
				this.handle ,
				buffer ,
				offset ,
				length
			);

			if (readed < length)
			{
				if (0 == readed)
				{
					return Array.Empty<byte>();
				}
				
				byte[] part = new byte[readed];
				
				Array.Copy(
					buffer,
					0,
					part,
					0,
					(int)readed
				);
				
				return part;
			}
		
			return buffer;
		}

		
		public ulong? ReadPointer(ulong offset)
		{
			BinaryReader reader = new BinaryReader(this);

			reader.Position = offset;
			
			bool ok = NativeMethods.BNReadPointer(
				this.handle ,
				reader.DangerousGetHandle(),
				out ulong value
			);
			
			return ok ? value : null;
		}
		
		public ulong WriteBuffer(ulong offset , DataBuffer buffer)
		{
			return NativeMethods.BNWriteViewBuffer(
				this.handle ,
				offset ,
				buffer.DangerousGetHandle()
			);
		}

		public ulong WriteData(ulong offset , byte[] data)
		{
			return NativeMethods.BNWriteViewData(
				this.handle ,
				offset ,
				data,
				(ulong)data.Length
			);
		}
		
		public ulong InsertBuffer(ulong offset , DataBuffer buffer)
		{
			return NativeMethods.BNInsertViewBuffer(
				this.handle ,
				offset ,
				buffer.DangerousGetHandle()
			);
		}

		public ulong InsertData(ulong offset , byte[] data)
		{
			return NativeMethods.BNInsertViewData(
				this.handle ,
				offset ,
				data,
				(ulong)data.Length
			);
		}

		
		public ulong RemoveData(ulong offset , ulong length)
		{
			return NativeMethods.BNRemoveViewData(this.handle , offset , length);
		}

		public float[] GetEntropy(ulong offset , ulong length , ulong blockSize)
		{
			if (0 == length)
			{
				return Array.Empty<float>();
			}

			if (0 == blockSize)
			{
				blockSize = this.Length;
			}

			// The core writes into this caller-allocated buffer (same sizing as the C++ API).
			float[] buffer = new float[( length / blockSize ) + 1];

			ulong writtenCount = NativeMethods.BNGetEntropy(
				this.handle ,
				offset ,
				length ,
				blockSize,
				buffer
			);

			// The core returns the number of block values it wrote; the buffer is one larger,
			// so trim to the written count (matches the C++ BinaryView::GetEntropy behavior).
			if (writtenCount >= (ulong)buffer.Length)
			{
				return buffer;
			}

			float[] result = new float[writtenCount];

			Array.Copy(buffer , result , (long)writtenCount);

			return result;
		}

		public ModificationStatus GetModification(ulong offset)
		{
			return NativeMethods.BNGetModification(
				this.handle , 
				offset 
			);
		}
		
		public ModificationStatus[] GetModificationStatus(ulong offset , ulong length )
		{
			ModificationStatus[] buffer = new ModificationStatus[length];
			
			ulong written = NativeMethods.BNGetModificationArray(
				this.handle , 
				offset ,
				buffer,
				length
			);

			if (0 == written)
			{
				return Array.Empty<ModificationStatus>();
			}
			
			if (written == length)
			{
				return buffer;
			}

			ModificationStatus[] status = new ModificationStatus[written];
			
			Array.Copy(buffer, status, (long)written);
			
			return status;
		}

		public ulong GetNextValidOffset(ulong offset)
		{
			return NativeMethods.BNGetNextValidOffset(this.handle , offset);
		}

		public bool IsValidOffset(ulong offset)
		{
			return NativeMethods.BNIsValidOffset(this.handle , offset);
		}

		public bool IsOffsetReadable(ulong offset)
		{
			return NativeMethods.BNIsOffsetReadable(this.handle , offset);
		}
		
		public bool IsOffsetWritable(ulong offset)
		{
			return NativeMethods.BNIsOffsetWritable(this.handle , offset);
		}

		public bool IsOffsetExecutable(ulong offset)
		{
			return NativeMethods.BNIsOffsetExecutable(this.handle , offset);
		}
		
		public bool IsOffsetBackedByFile(ulong offset)
		{
			return NativeMethods.BNIsOffsetBackedByFile(this.handle , offset);
		}
		
		public bool IsOffsetCodeSemantics(ulong offset)
		{
			return NativeMethods.BNIsOffsetCodeSemantics(this.handle , offset);
		}
		
		public bool IsOffsetExternSemantics(ulong offset)
		{
			return NativeMethods.BNIsOffsetExternSemantics(this.handle , offset);
		}

		public bool IsOffsetWritableSemantics(ulong offset)
		{
			return NativeMethods.BNIsOffsetWritableSemantics(this.handle , offset);
		}

		public bool SaveToFile(FileAccessor accessor)
		{
			return NativeMethods.BNSaveToFile(this.handle , accessor.ToNative());
		}

		public bool SaveToFilename(string filename)
		{
			if (!Path.IsPathRooted(filename))
			{
				filename = Path.GetFullPath(filename);
			}
		
			string? dirname = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dirname) && !Directory.Exists(dirname))
			{
				Directory.CreateDirectory(dirname);
			}
			
			return NativeMethods.BNSaveToFilename(
				this.handle , 
				filename
			);
		}

		public Function? AddFunctionForAnalysis(
			ulong address ,
			Platform? platform = null,
			bool autoDiscovered = false,
			BinaryNinja.Type? functionType = null
		)
		{
			if (null == platform)
			{
				platform = this.DefaultPlatform;
			}

			return Function.TakeHandle(

				NativeMethods.BNAddFunctionForAnalysis(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
					address ,
					autoDiscovered ,
					null == functionType ? IntPtr.Zero : functionType.DangerousGetHandle()
				)
			);
		}
		
		public void AddEntryPointForAnalysis(ulong address , Platform? platform = null )
		{
			if (null == platform)
			{
				platform = this.DefaultPlatform;
			}

			NativeMethods.BNAddEntryPointForAnalysis(
				this.handle ,
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
				address 
			);
		}

		public void AddToEntryFunctions(Function function)
		{
			NativeMethods.BNAddToEntryFunctions(
				this.handle ,
				function.DangerousGetHandle()
			);
		}

		public void RemoveAnalysisFunction(Function function , bool updateRefs = false  )
		{
			NativeMethods.BNRemoveAnalysisFunction(
				this.handle , 
				function.DangerousGetHandle(),
				updateRefs
			);
		}

		public Function? CreateUserFunction(ulong address , Platform? platform = null)
		{
			if (null == platform)
			{
				platform = this.DefaultPlatform;
			}

			return Function.TakeHandle(

				NativeMethods.BNCreateUserFunction(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
					address
				)
			);
		}
		
		public void RemoveUserFunction(Function function  )
		{
			NativeMethods.BNRemoveUserFunction(
				this.handle , 
				function.DangerousGetHandle()
			);
		}

		public void AddAnalysisOption(string option)
		{
			NativeMethods.BNAddAnalysisOption(this.handle , option);
		}

		public bool HasInitialAnalysis
		{
			get
			{
				return NativeMethods.BNHasInitialAnalysis(this.handle);
			}
		}

		public void SetAnalysisHold(bool enable  )
		{
			NativeMethods.BNSetAnalysisHold(this.handle , enable);
		}

		public bool FunctionAnalysisUpdateDisabled
		{
			get
			{
				return NativeMethods.BNGetFunctionAnalysisUpdateDisabled(this.handle);
			}

			set
			{
				NativeMethods.BNSetFunctionAnalysisUpdateDisabled(this.handle ,value);
			}
		}

		public void UpdateAnalysis()
		{
			NativeMethods.BNUpdateAnalysis(this.handle);
		}
		
		public void UpdateAnalysisAndWait()
		{
			NativeMethods.BNUpdateAnalysisAndWait(this.handle);
		}
		
		/// <summary>
		/// Registers a callback that fires once when the next analysis pass on this view completes,
		/// mirroring Python <c>BinaryView.add_analysis_completion_event</c> (binaryview.py:8068).
		/// Useful with <see cref="UpdateAnalysis"/>, which returns before analysis finishes. The
		/// returned event must be kept alive by the caller for as long as the callback should remain
		/// armed; disposing or GC-collecting it cancels the event.
		/// </summary>
		/// <param name="callback">The action invoked when analysis completes.</param>
		/// <returns>The armed event, or <c>null</c> if the core refused to create it.</returns>
		public AnalysisCompletionEvent? AddAnalysisCompletionEvent(Action callback)
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			return AnalysisCompletionEvent.Create(this, callback);
		}

		public void AbortAnalysis()
		{
			NativeMethods.BNAbortAnalysis(this.handle);
		}

		public bool AnalysisIsAborted
		{
			get
			{
				return NativeMethods.BNAnalysisIsAborted(this.handle);
			}
		}

		public bool ShouldSkipTargetAnalysis(
			ArchitectureAndAddress source ,
			Function sourceFunction,
			ulong sourceEnd,
			ArchitectureAndAddress target
			)
		{
			return NativeMethods.BNShouldSkipTargetAnalysis(
				this.handle,
				source.ToNative(),
				sourceFunction.DangerousGetHandle(),
				sourceEnd,
				target.ToNative()
			);
		}

		public void DefineDataVariable(ulong address , TypeWithConfidence type)
		{
			NativeMethods.BNDefineDataVariable(
				this.handle,
				address ,
				type.ToNative()
			);
		}
		
		public void DefineUserDataVariable(ulong address , TypeWithConfidence type)
		{
			NativeMethods.BNDefineUserDataVariable(
				this.handle,
				address ,
				type.ToNative()
			);
		}

		public void UndefineDataVariable(ulong address , bool blacklist = false)
		{
			NativeMethods.BNUndefineDataVariable(this.handle , address , blacklist);
		}
		
		public void UndefineUserDataVariable(ulong address )
		{
			NativeMethods.BNUndefineUserDataVariable(this.handle , address);
		}

		public DataVariable? GetDataVariableAtAddress(ulong address)
		{
			bool ok = NativeMethods.BNGetDataVariableAtAddress(
				this.handle ,
				address ,
				out BNDataVariable native
			);

			if (!ok)
			{
				return null;
			}

			return DataVariable.TakeNative(native , this);
		}

		public Function[] GetFunctionsContainingAddress(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetAnalysisFunctionsContainingAddress(
				this.handle ,
				address ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<Function>(
				arrayPointer ,
				arrayLength ,
				Function.MustNewFromHandle,
				NativeMethods.BNFreeFunctionList
			);
		}

		public Function[] GetFunctionsByName(string name)
		{
			Symbol[] symbols = this.GetSymbolsByName(name);
			
			List<Function> functions = new List<Function>();

			foreach (Symbol symbol in symbols)
			{
				Function? function = this.GetFunctionByAddress(
					symbol.Address
				);

				if (null != function)
				{
					functions.Add(function);
				}
			}

			return functions.ToArray();
		}

		public Function? GetFunctionByRawName(string name , Platform? platform = null)
		{
			Symbol? symbol = this.GetSymbolByRawName(name);

			if (null == symbol)
			{
				return null;
			}
			
			return this.GetFunctionByAddress(symbol.Address);
		}
		
		public LowLevelILFunction? GetLowLevelILFunctionByRawName(
			string name , 
			Platform? platform = null)
		{
			Function? function = this.GetFunctionByRawName(name);

			if (null == function)
			{
				return null;
			}
			
			return function.GetLowLevelIL();
		}
		
		public MediumLevelILFunction? GetMediumLevelILFunctionByRawName(
			string name , 
			Platform? platform = null)
		{
			Function? function = this.GetFunctionByRawName(name);

			if (null == function)
			{
				return null;
			}
			
			return function.GetMediumLevelIL();
		}
		
		public HighLevelILFunction? GetHighLevelILFunctionByRawName(
			string name , 
			Platform? platform = null)
		{
			Function? function = this.GetFunctionByRawName(name);

			if (null == function)
			{
				return null;
			}
			
			return function.GetHighLevelIL();
		}
		
		public LanguageRepresentationFunction? GetLanguageRepresentationFunctionByRawName(
			string name , 
			string language = "Pseudo C",
			Platform? platform = null)
		{
			Function? function = this.GetFunctionByRawName(name);

			if (null == function)
			{
				return null;
			}
			
			return function.GetLanguageRepresentation(language);
		}
		
		public Function? GetFunctionByAddress(ulong address , Platform? platform = null)
		{
			if (null == platform)
			{
				platform = this.DefaultPlatform;
			}
			
			return Function.TakeHandle(
				NativeMethods.BNGetAnalysisFunction(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
					address
				)
			);
		}
		
		public Function[] GetFunctionsForAddress(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetAnalysisFunctionsForAddress(
				this.handle ,
				address ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<Function>(
				arrayPointer ,
				arrayLength ,
				Function.MustNewFromHandle,
				NativeMethods.BNFreeFunctionList
			);
		}
		
		public Function? GetRecentFunctionForAddress(ulong address )
		{
			return Function.TakeHandle(
				NativeMethods.BNGetRecentAnalysisFunctionForAddress(
					this.handle ,
					address
				)
			);
		}
		
		public BasicBlock[] GetBasicBlocksForAddress(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetBasicBlocksForAddress(
				this.handle ,
				address ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<BasicBlock>(
				arrayPointer ,
				arrayLength ,
				BasicBlock.MustNewFromHandle,
				NativeMethods.BNFreeBasicBlockList
			);
		}

		public BasicBlock[] GetBasicBlocksStartingAtAddress(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetBasicBlocksStartingAtAddress(
				this.handle ,
				address ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<BasicBlock>(
				arrayPointer ,
				arrayLength ,
				BasicBlock.MustNewFromHandle ,
				NativeMethods.BNFreeBasicBlockList
			);
		}
		
		public BasicBlock? GetRecentBasicBlockForAddress(ulong address )
		{
			return BasicBlock.TakeHandle(
				NativeMethods.BNGetRecentBasicBlockForAddress(
					this.handle ,
					address
				)
			);
		}

		public ReferenceSource[] GetCodeReferences(
			ulong address,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			IntPtr arrayPointer = NativeMethods.BNGetCodeReferences(
				this.handle ,
				address ,
				out ulong arrayLength ,
				limit ,
				maxItems
			);

			return UnsafeUtils.TakeStructArrayEx<BNReferenceSource , ReferenceSource>(
				arrayPointer ,
				arrayLength ,
				ReferenceSource.FromNative ,
				NativeMethods.BNFreeCodeReferences
			);
		}
		
		public ReferenceSource[] GetCodeReferencesInRange(
			ulong address,
			ulong length,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesInRange(
				this.handle ,
				address ,
				length,
				out ulong arrayLength ,
				limit ,
				maxItems
			);

			return UnsafeUtils.TakeStructArrayEx<BNReferenceSource , ReferenceSource>(
				arrayPointer ,
				arrayLength ,
				ReferenceSource.FromNative ,
				NativeMethods.BNFreeCodeReferences
			);
		}
		
		public ulong[] GetCodeReferencesFrom(ReferenceSource source)
		{
			IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesFrom(
				this.handle ,
				source.ToNative() ,
				out ulong arrayLength 
			);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNFreeAddressList
			);
		}
		
		/// <summary>
		/// Code references made from <paramref name="address"/>. When no function is
		/// supplied, every function containing the address is considered (mirroring
		/// Python <c>get_code_refs_from(addr, func=None, arch=None)</c>).
		/// </summary>
		public ulong[] GetCodeReferencesFrom(
			ulong address ,
			Function? function = null ,
			Architecture? arch = null
		)
		{
			HashSet<ulong> targets = new HashSet<ulong>();

			foreach (ReferenceSource source in this.BuildReferenceSources(address , function , arch))
			{
				foreach (ulong reference in this.GetCodeReferencesFrom(source))
				{
					targets.Add(reference);
				}
			}

			return targets.ToArray();
		}

		private ReferenceSource[] BuildReferenceSources(
			ulong address ,
			Function? function ,
			Architecture? arch
		)
		{
			if (null != function)
			{
				return new ReferenceSource[]
				{
					new ReferenceSource()
					{
						Function = function ,
						Architecture = arch ?? function.Architecture ,
						Address = address
					}
				};
			}

			List<ReferenceSource> sources = new List<ReferenceSource>();

			foreach (Function containing in this.GetFunctionsContainingAddress(address))
			{
				sources.Add(new ReferenceSource()
				{
					Function = containing ,
					Architecture = arch ?? containing.Architecture ,
					Address = address
				});
			}

			return sources.ToArray();
		}

		public ulong[] GetCodeReferencesFromInRange(ReferenceSource source , ulong length)
		{
			IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesFromInRange(
				this.handle ,
				source.ToNative() ,
				length,
				out ulong arrayLength 
			);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNFreeAddressList
			);
		}
		
		public ulong[] GetDataReferences(
			ulong address,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			IntPtr arrayPointer = NativeMethods.BNGetDataReferences(
				this.handle ,
				address ,
				out ulong arrayLength ,
				limit,
				maxItems
			);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNFreeDataReferences
			);
		}
		
		public ulong[] GetDataReferencesInRange(
			ulong address,
			ulong length,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			IntPtr arrayPointer = NativeMethods.BNGetDataReferencesInRange(
				this.handle ,
				address ,
				length,
				out ulong arrayLength ,
				limit,
				maxItems
			);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNFreeDataReferences
			);
		}
		
		public ulong[] GetDataReferencesFrom(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetDataReferencesFrom(
				this.handle ,
				address ,
				out ulong arrayLength 
			);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNFreeDataReferences
			);
		}
		
		public ulong[] GetDataReferencesFromInRange(ulong address , ulong length)
		{
			IntPtr arrayPointer = NativeMethods.BNGetDataReferencesFromInRange(
				this.handle ,
				address ,
				length,
				out ulong arrayLength 
			);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNFreeDataReferences
			);
		}
		
		public ReferenceSource[] GetCodeReferencesForType(
			QualifiedName type,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesForType(
					this.handle ,
					type.ToNativeEx(allocator) ,
					out ulong arrayLength ,
					limit ,
					maxItems
				);

				return UnsafeUtils.TakeStructArrayEx<BNReferenceSource , ReferenceSource>(
					arrayPointer ,
					arrayLength ,
					ReferenceSource.FromNative ,
					NativeMethods.BNFreeCodeReferences
				);
			}
		}

		public ReferenceSource[] GetCodeReferencesForTypeField(
			QualifiedName type,
			ulong offset,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesForTypeField(
					this.handle ,
					type.ToNativeEx(allocator) ,
					offset,
					out ulong arrayLength ,
					limit ,
					maxItems
				);

				return UnsafeUtils.TakeStructArrayEx<BNReferenceSource , ReferenceSource>(
					arrayPointer ,
					arrayLength ,
					ReferenceSource.FromNative ,
					NativeMethods.BNFreeCodeReferences
				);
			}
		}
		
		public ulong[] GetDataReferencesForType(
			QualifiedName type,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetDataReferencesForType(
					this.handle ,
					type.ToNativeEx(allocator) ,
					out ulong arrayLength ,
					limit ,
					maxItems
				);

				return UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeDataReferences
				);
			}
		}
		
		public ulong[] GetDataReferencesForTypeField(
			QualifiedName type,
			ulong offset,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetDataReferencesForTypeField(
					this.handle ,
					type.ToNativeEx(allocator) ,
					offset,
					out ulong arrayLength ,
					limit ,
					maxItems
				);

				return UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeDataReferences
				);
			}
		}
		
		public ulong[] GetDataReferencesFromForTypeField(
			QualifiedName type,
			ulong offset,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetDataReferencesFromForTypeField(
					this.handle ,
					type.ToNativeEx(allocator) ,
					offset,
					out ulong arrayLength ,
					limit ,
					maxItems
				);

				return UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeDataReferences
				);
			}
		}
		
		public TypeReferenceSource[] GetTypeReferencesForType(
			QualifiedName type,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetTypeReferencesForType(
					this.handle ,
					type.ToNativeEx(allocator) ,
					out ulong arrayLength ,
					limit ,
					maxItems
				);

				return UnsafeUtils.TakeStructArrayEx<BNTypeReferenceSource , TypeReferenceSource>(
					arrayPointer ,
					arrayLength ,
					TypeReferenceSource.FromNative ,
					NativeMethods.BNFreeTypeReferences
				);
			}
		}
		
		public TypeReferenceSource[] GetTypeReferencesForTypeField(
			QualifiedName type,
			ulong offset ,
			bool limit = false,
			ulong maxItems = 0
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetTypeReferencesForTypeField(
					this.handle ,
					type.ToNativeEx(allocator) ,
					offset,
					out ulong arrayLength ,
					limit ,
					maxItems
				);

				return UnsafeUtils.TakeStructArrayEx<BNTypeReferenceSource , TypeReferenceSource>(
					arrayPointer ,
					arrayLength ,
					TypeReferenceSource.FromNative ,
					NativeMethods.BNFreeTypeReferences
				);
			}
		}

		public TypeReferenceSource[] GetCodeReferencesForTypeFrom(ReferenceSource source)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesForTypeFrom(
					this.handle ,
					source.ToNative(),
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNTypeReferenceSource , TypeReferenceSource>(
					arrayPointer ,
					arrayLength ,
					TypeReferenceSource.FromNative ,
					NativeMethods.BNFreeTypeReferences
				);
			}
		}

		public TypeReferenceSource[] GetCodeReferencesForTypeFromInRange(
			ReferenceSource source,
			ulong length
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesForTypeFromInRange(
					this.handle ,
					source.ToNative(),
					length,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNTypeReferenceSource , TypeReferenceSource>(
					arrayPointer ,
					arrayLength ,
					TypeReferenceSource.FromNative ,
					NativeMethods.BNFreeTypeReferences
				);
			}
		}
		
		public TypeReferenceSource[] GetCodeReferencesForTypeFieldsFrom(ReferenceSource source)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesForTypeFieldsFrom(
					this.handle ,
					source.ToNative(),
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNTypeReferenceSource , TypeReferenceSource>(
					arrayPointer ,
					arrayLength ,
					TypeReferenceSource.FromNative ,
					NativeMethods.BNFreeTypeReferences
				);
			}
		}
		
		public TypeReferenceSource[] GetCodeReferencesForTypeFieldsFromInRange(
			ReferenceSource source,
			ulong length
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetCodeReferencesForTypeFieldsFromInRange(
					this.handle ,
					source.ToNative(),
					length,
					out ulong arrayLength 
				);
				
				return UnsafeUtils.TakeStructArrayEx<BNTypeReferenceSource , TypeReferenceSource>(
					arrayPointer ,
					arrayLength ,
					TypeReferenceSource.FromNative ,
					NativeMethods.BNFreeTypeReferences
				);
			}
		}

	
		public void AddUserDataReference(ulong from  , ulong to)
		{
			NativeMethods.BNAddUserDataReference(this.handle ,from ,to);
		}
		
		public void RemoveDataReference(ulong from  , ulong to)
		{
			NativeMethods.BNRemoveDataReference(this.handle ,from ,to);
		}
		
		public void RemoveUserDataReference(ulong from  , ulong to)
		{
			NativeMethods.BNRemoveUserDataReference(this.handle ,from ,to);
		}
		
		public ulong[] GetAllFieldsReferenced(QualifiedName type)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetAllFieldsReferenced(
					this.handle ,
					type.ToNativeEx(allocator) ,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeDataReferences
				);
			}
		}
		
		public TypeFieldReferenceSizeInfo[] GetAllSizesReferenced(QualifiedName type)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetAllSizesReferenced(
					this.handle ,
					type.ToNativeEx(allocator) ,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNTypeFieldReferenceSizeInfo,TypeFieldReferenceSizeInfo>(
					arrayPointer ,
					arrayLength ,
					TypeFieldReferenceSizeInfo.FromNative,
					NativeMethods.BNFreeTypeFieldReferenceSizeInfo
				);
			}
		}
		
		public TypeFieldReferenceTypeInfo[] GetAllTypesReferenced(QualifiedName type)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetAllTypesReferenced(
					this.handle ,
					type.ToNativeEx(allocator) ,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNTypeFieldReferenceTypeInfo,TypeFieldReferenceTypeInfo>(
					arrayPointer ,
					arrayLength ,
					TypeFieldReferenceTypeInfo.FromNative,
					NativeMethods.BNFreeTypeFieldReferenceTypeInfo
				);
			}
		}
		
		public ulong[] GetSizesReferenced(QualifiedName type , ulong offset)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetSizesReferenced(
					this.handle ,
					type.ToNativeEx(allocator) ,
					offset,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeNumberArrayEx<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeTypeFieldReferenceSizes
				);
			}
		}
		
		public TypeWithConfidence[] GetTypesReferenced(QualifiedName type , ulong offset)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetTypesReferenced(
					this.handle ,
					type.ToNativeEx(allocator) ,
					offset,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNTypeWithConfidence,TypeWithConfidence>(
					arrayPointer ,
					arrayLength ,
					TypeWithConfidence.FromNative,
					NativeMethods.BNFreeTypeFieldReferenceTypes
				);
			}
		}
		
		public QualifiedName[] GetOutgoingDirectTypeReferences(QualifiedName type)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetOutgoingDirectTypeReferences(
					this.handle ,
					type.ToNativeEx(allocator) ,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNQualifiedName,QualifiedName>(
					arrayPointer ,
					arrayLength ,
					QualifiedName.FromNative,
					NativeMethods.BNFreeTypeNameList
				);
			}
		}
		
		public QualifiedName[] GetOutgoingRecursiveTypeReferences(QualifiedName[] types)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetOutgoingRecursiveTypeReferences(
					this.handle ,
					allocator.ConvertToNativeArrayEx<BNQualifiedName,QualifiedName>(
						types),
					(ulong)types.Length,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNQualifiedName,QualifiedName>(
					arrayPointer ,
					arrayLength ,
					QualifiedName.FromNative,
					NativeMethods.BNFreeTypeNameList
				);
			}
		}
		
		public QualifiedName[] GetIncomingDirectTypeReferences(QualifiedName type)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetIncomingDirectTypeReferences(
					this.handle ,
					type.ToNativeEx(allocator) ,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNQualifiedName,QualifiedName>(
					arrayPointer ,
					arrayLength ,
					QualifiedName.FromNative,
					NativeMethods.BNFreeTypeNameList
				);
			}
		}
		
		public QualifiedName[] GetIncomingRecursiveTypeReferences(QualifiedName[] types)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetIncomingRecursiveTypeReferences(
					this.handle ,
					allocator.ConvertToNativeArrayEx<BNQualifiedName,QualifiedName>(
						types),
					(ulong)types.Length,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeStructArrayEx<BNQualifiedName,QualifiedName>(
					arrayPointer ,
					arrayLength ,
					QualifiedName.FromNative,
					NativeMethods.BNFreeTypeNameList
				);
			}
		}

		public Structure CreateStructureFromOffsetAccess(QualifiedName name , out bool newMember)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return Structure.MustTakeHandle(
					NativeMethods.BNCreateStructureFromOffsetAccess(
						this.handle,
						name.ToNativeEx(allocator) ,
						out newMember
					)
				);
			}
		}
		
		public TypeWithConfidence CreateStructureMemberFromAccess(QualifiedName name , ulong offset)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return TypeWithConfidence.FromNative(
					NativeMethods.BNCreateStructureMemberFromAccess(
						this.handle,
						name.ToNativeEx(allocator) ,
						offset
					)
				);
			}
		}

		public void AddExpressionParserMagicValue(string name , ulong value)
		{
			NativeMethods.BNAddExpressionParserMagicValue(this.handle , name , value);
		}
		
		public void RemoveExpressionParserMagicValue(string name )
		{
			NativeMethods.BNRemoveExpressionParserMagicValue(this.handle , name );
		}
		
		public void AddExpressionParserMagicValues(IDictionary<string,ulong> items)
		{
			// Build the names const char** as a UTF-8 block (.NET cannot apply
			// LPUTF8Str to string[] elements, so non-ASCII would otherwise
			// corrupt through the system ANSI code page).
			string[] safeNames = items.Keys.ToArray();
			ulong[] safeValues = items.Values.ToArray();

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr namesBlock = allocator.AllocUtf8StringArray(safeNames);

				NativeMethods.BNAddExpressionParserMagicValues(
					this.handle ,
					namesBlock ,
					safeValues ,
					(ulong)safeNames.Length
				);
			}
		}
		
		public void RemoveExpressionParserMagicValues(string[] names)
		{
			// Build the names const char** as a UTF-8 block (.NET cannot apply
			// LPUTF8Str to string[] elements, so non-ASCII would otherwise
			// corrupt through the system ANSI code page).
			string[] safeNames = names ?? Array.Empty<string>();

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr namesBlock = allocator.AllocUtf8StringArray(safeNames);

				NativeMethods.BNRemoveExpressionParserMagicValues(
					this.handle ,
					namesBlock ,
					(ulong)safeNames.Length
				);
			}
		}

		public bool GetExpressionParserMagicValue(string name , out ulong value)
		{
			return NativeMethods.BNGetExpressionParserMagicValue(
				this.handle ,
				name ,
				out value
			);
		}
		
		
		public ReferenceSource[] GetCallers(ulong callee)
		{
			IntPtr arrayPointer = NativeMethods.BNGetCallers(
				this.handle ,
				callee ,
				out ulong arrayLength 
			);

			return UnsafeUtils.TakeStructArrayEx<BNReferenceSource , ReferenceSource>(
				arrayPointer ,
				arrayLength ,
				ReferenceSource.FromNative ,
				NativeMethods.BNFreeCodeReferences
			);
		}
		
		public ulong[] GetCallees(ReferenceSource callSite)
		{
			IntPtr arrayPointer = NativeMethods.BNGetCallees(
				this.handle ,
				callSite.ToNative() ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNFreeAddressList
			);
		}

		/// <summary>
		/// The call targets of the call site at <paramref name="address"/>. When no
		/// function is supplied, every function containing the address is considered
		/// (mirrors Python <c>get_callees(addr, func=None, arch=None)</c>).
		/// </summary>
		public ulong[] GetCallees(
			ulong address ,
			Function? function = null ,
			Architecture? arch = null
		)
		{
			HashSet<ulong> targets = new HashSet<ulong>();

			foreach (ReferenceSource source in this.BuildReferenceSources(address , function , arch))
			{
				foreach (ulong target in this.GetCallees(source))
				{
					targets.Add(target);
				}
			}

			return targets.ToArray();
		}

		public Symbol? GetSymbolByAddress(ulong address , NameSpace? ns = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return Symbol.TakeHandle(
					NativeMethods.BNGetSymbolByAddress(
						this.handle ,
						address ,
						null == ns
							? IntPtr.Zero
							: allocator.AllocStruct<BNNameSpace>(
								ns.ToNativeEx(allocator)
							)
					)
				);
			}
		}
		
		public Symbol[] GetSymbolsByRawName(string name, NameSpace? ns = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetSymbolsByRawName(
					this.handle ,
					name ,
					out ulong arrayLength ,
					null == ns
						? IntPtr.Zero
						: allocator.AllocStruct<BNNameSpace>(
							ns.ToNativeEx(allocator)
						)
				);

				return UnsafeUtils.TakeHandleArrayEx<Symbol>(
					arrayPointer ,
					arrayLength ,
					Symbol.MustNewFromHandle ,
					NativeMethods.BNFreeSymbolList
				);
			}
		}
		
		public Symbol? GetSymbolByRawName(string name, NameSpace? ns = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return Symbol.TakeHandle(
					NativeMethods.BNGetSymbolByRawName(
						this.handle ,
						name ,
						null == ns
							? IntPtr.Zero
							: allocator.AllocStruct<BNNameSpace>(
								ns.ToNativeEx(allocator)
							)
					)
				);
			}
		}
		
		public Symbol[] GetSymbolsByName(string name, NameSpace? ns = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetSymbolsByName(
					this.handle ,
					name ,
					out ulong arrayLength ,
					null == ns
						? IntPtr.Zero
						: allocator.AllocStruct<BNNameSpace>(
							ns.ToNativeEx(allocator)
						)
				);

				return UnsafeUtils.TakeHandleArrayEx<Symbol>(
					arrayPointer ,
					arrayLength ,
					Symbol.MustNewFromHandle ,
					NativeMethods.BNFreeSymbolList
				);
			}
		}

		public Symbol[] GetSymbols(NameSpace? ns = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetSymbols(
					this.handle ,
					out ulong arrayLength ,
					null == ns
						? IntPtr.Zero
						: allocator.AllocStruct<BNNameSpace>(
							ns.ToNativeEx(allocator)
						)
				);

				return UnsafeUtils.TakeHandleArrayEx<Symbol>(
					arrayPointer ,
					arrayLength ,
					Symbol.MustNewFromHandle ,
					NativeMethods.BNFreeSymbolList
				);
			}
		}

		
		public Symbol[] Symbols
		{
			get
			{
				return this.GetSymbols(null);
			}
		}
		
		public string[] SymbolNames
		{
			get
			{
				List<string> items = new List<string>();

				foreach (Symbol symbol in this.Symbols)
				{
					items.Add(symbol.RawName);
				}

				return items.ToArray();
			}
		}
		
		public string[] SymbolFullNames
		{
			get
			{
				List<string> items = new List<string>();

				foreach (Symbol symbol in this.Symbols)
				{
					items.Add(symbol.FullName);
				}

				return items.ToArray();
			}
		}
		
		public Symbol[] GetSymbolsInRange(
			ulong start ,
			ulong length,
			NameSpace? ns = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetSymbolsInRange(
					this.handle ,
					start,
					length,
					out ulong arrayLength ,
					null == ns
						? IntPtr.Zero
						: allocator.AllocStruct<BNNameSpace>(
							ns.ToNativeEx(allocator)
						)
				);

				return UnsafeUtils.TakeHandleArrayEx<Symbol>(
					arrayPointer ,
					arrayLength ,
					Symbol.MustNewFromHandle ,
					NativeMethods.BNFreeSymbolList
				);
			}
		}
		
		
		public Symbol[] GetSymbolsOfType(
			SymbolType type,
			NameSpace? ns = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetSymbolsOfType(
					this.handle ,
					type,
					out ulong arrayLength ,
					null == ns
						? IntPtr.Zero
						: allocator.AllocStruct<BNNameSpace>(
							ns.ToNativeEx(allocator)
						)
				);

				return UnsafeUtils.TakeHandleArrayEx<Symbol>(
					arrayPointer ,
					arrayLength ,
					Symbol.MustNewFromHandle ,
					NativeMethods.BNFreeSymbolList
				);
			}
		}
		
		public Symbol[] GetSymbolsOfTypeInRange(
			SymbolType type,
			ulong start ,
			ulong length,
			NameSpace? ns = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr arrayPointer = NativeMethods.BNGetSymbolsOfTypeInRange(
					this.handle ,
					type,
					start,
					length,
					out ulong arrayLength ,
					null == ns
						? IntPtr.Zero
						: allocator.AllocStruct<BNNameSpace>(
							ns.ToNativeEx(allocator)
						)
				);

				return UnsafeUtils.TakeHandleArrayEx<Symbol>(
					arrayPointer ,
					arrayLength ,
					Symbol.MustNewFromHandle ,
					NativeMethods.BNFreeSymbolList
				);
			}
		}

		public void DefineAutoSymbol(Symbol symbol)
		{
			NativeMethods.BNDefineAutoSymbol(this.handle , symbol.DangerousGetHandle());
		}
		
		public Symbol? DefineAutoSymbolAndVariableOrFunction(
			Symbol symbol,
			Platform? platform = null,
			TypeWithConfidence? type = null
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return Symbol.TakeHandle(
					NativeMethods.BNDefineAutoSymbolAndVariableOrFunction(
						this.handle ,
						symbol.DangerousGetHandle(),
						symbol.DangerousGetHandle(),
						null == type ? IntPtr.Zero : 
							allocator.AllocStruct<BNTypeWithConfidence>( type.ToNative() )
					)
				);
			}
		}
		
		public void UndefineAutoSymbol(Symbol symbol)
		{
			NativeMethods.BNUndefineAutoSymbol(this.handle , symbol.DangerousGetHandle());
		}
		
		public void DefineUserSymbol(Symbol symbol)
		{
			NativeMethods.BNDefineUserSymbol(this.handle , symbol.DangerousGetHandle());
		}
		
		public void UndefineUserSymbol(Symbol symbol)
		{
			NativeMethods.BNUndefineUserSymbol(this.handle , symbol.DangerousGetHandle());
		}
		
		public void DefineImportedFunction(
			Symbol symbol,
			Function function,
			BinaryNinja.Type? type
	    )
		{
			NativeMethods.BNDefineImportedFunction(
				this.handle , 
				symbol.DangerousGetHandle(),
				function.DangerousGetHandle(),
				null == type ? IntPtr.Zero :  type.DangerousGetHandle()
			);
		}

		public void BeginBulkModifySymbols()
		{
			NativeMethods.BNBeginBulkModifySymbols(this.handle );
		}
		
		public void EndBulkModifySymbols()
		{
			NativeMethods.BNEndBulkModifySymbols(this.handle );
		}

		public TagType CreateTagType()
		{
			return TagType.MustTakeHandle(
				NativeMethods.BNCreateTagType(this.handle)
			);
		}

		public void AddTagType(TagType tagType)
		{
			NativeMethods.BNAddTagType(this.handle , tagType.DangerousGetHandle());
		}

		public void RemoveTag(Tag tag , bool user)
		{
			NativeMethods.BNRemoveTag(this.handle , tag.DangerousGetHandle() , user);
		}
		
		public void RemoveTagType(TagType tagType)
		{
			NativeMethods.BNRemoveTagType(this.handle , tagType.DangerousGetHandle());
		}

		public TagType[] TagTypes
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetTagTypes(
					this.handle,
					out ulong arrayLength 
				);

				return UnsafeUtils.TakeHandleArrayEx<TagType>(
					arrayPointer , 
					arrayLength,
					TagType.MustNewFromHandle,
					NativeMethods.BNFreeTagTypeList
				);
			}
		}

		public TagType? GetTagType(string name)
		{
			return TagType.TakeHandle(
				NativeMethods.BNGetTagType(this.handle , name )
			);
		}
		
		public TagType? GetTagTypeById(string name)
		{
			return TagType.TakeHandle(
				NativeMethods.BNGetTagTypeById(this.handle , name )
			);
		}
		
		public void AddTag(Tag tag , bool user)
		{
			NativeMethods.BNAddTag(this.handle , tag.DangerousGetHandle() , user);
		}

		public void AddUserDataTag(ulong address ,  Tag tag )
		{
			NativeMethods.BNAddUserDataTag(
				this.handle ,
				address , tag.DangerousGetHandle());
		}

		public TagReference[] TagReferences
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetAllTagReferences(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
					arrayPointer ,
					arrayLength ,
					TagReference.FromNative ,
					NativeMethods.BNFreeTagReferences
				);
			}
		}

		public TagReference[] AddressTagReferences
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetAllAddressTagReferences(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
					arrayPointer ,
					arrayLength ,
					TagReference.FromNative ,
					NativeMethods.BNFreeTagReferences
				);
			}
		}
		
		public TagReference[] FunctionTagReferences
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetAllFunctionTagReferences(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
					arrayPointer ,
					arrayLength ,
					TagReference.FromNative ,
					NativeMethods.BNFreeTagReferences
				);
			}
		}
		
		public TagReference[] DataTagReferences
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetDataTagReferences(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
					arrayPointer ,
					arrayLength ,
					TagReference.FromNative ,
					NativeMethods.BNFreeTagReferences
				);
			}
		}
		
		public TagReference[] AutoDataTagReferences
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetAutoDataTagReferences(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
					arrayPointer ,
					arrayLength ,
					TagReference.FromNative ,
					NativeMethods.BNFreeTagReferences
				);
			}
		}
		
		public TagReference[] UserDataTagReferences
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetUserDataTagReferences(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNTagReference , TagReference>(
					arrayPointer ,
					arrayLength ,
					TagReference.FromNative ,
					NativeMethods.BNFreeTagReferences
				);
			}
		}

		/// <summary>
		/// Retrieves all tag references of a specific type from this binary view.
		/// </summary>
		/// <param name="tagType">The tag type to filter references by.</param>
		/// <returns>An array of matching TagReference objects.</returns>
		public unsafe TagReference[] GetAllTagReferencesOfType(TagType tagType)
		{
			ulong count = 0;

			IntPtr ptr = NativeMethods.BNGetAllTagReferencesOfType(
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
		/// Retrieves tag references of a specific type from this binary view.
		/// </summary>
		/// <param name="tagType">The tag type to filter references by.</param>
		/// <returns>An array of matching TagReference objects.</returns>
		public unsafe TagReference[] GetTagReferencesOfType(TagType tagType)
		{
			ulong count = 0;

			IntPtr ptr = NativeMethods.BNGetTagReferencesOfType(
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

		public Tag[] GetDataTags(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetDataTags(
				this.handle ,
				address,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<Tag>(
				arrayPointer ,
				arrayLength ,
				Tag.MustNewFromHandle ,
				NativeMethods.BNFreeTagList
			);
		}
		
		public Tag[] GetAutoDataTags(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetAutoDataTags(
				this.handle ,
				address,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<Tag>(
				arrayPointer ,
				arrayLength ,
				Tag.MustNewFromHandle ,
				NativeMethods.BNFreeTagList
			);
		}
		
		public Tag[] GetUserDataTags(ulong address)
		{
			IntPtr arrayPointer = NativeMethods.BNGetUserDataTags(
				this.handle ,
				address,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<Tag>(
				arrayPointer ,
				arrayLength ,
				Tag.MustNewFromHandle ,
				NativeMethods.BNFreeTagList
			);
		}
		
		
		public TagReference[] GetDataTagsInRange(ulong start , ulong end)
		{
			IntPtr arrayPointer = NativeMethods.BNGetDataTagsInRange(
				this.handle ,
				start,
				end,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeStructArrayEx<BNTagReference,TagReference>(
				arrayPointer ,
				arrayLength ,
				TagReference.FromNative ,
				NativeMethods.BNFreeTagReferences
			);
		}
		
		public TagReference[] GetAutoDataTagsInRange(ulong start , ulong end)
		{
			IntPtr arrayPointer = NativeMethods.BNGetAutoDataTagsInRange(
				this.handle ,
				start,
				end,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeStructArrayEx<BNTagReference,TagReference>(
				arrayPointer ,
				arrayLength ,
				TagReference.FromNative ,
				NativeMethods.BNFreeTagReferences
			);
		}
		
		public TagReference[] GetUserDataTagsInRange(ulong start , ulong end)
		{
			IntPtr arrayPointer = NativeMethods.BNGetUserDataTagsInRange(
				this.handle ,
				start,
				end,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeStructArrayEx<BNTagReference,TagReference>(
				arrayPointer ,
				arrayLength ,
				TagReference.FromNative ,
				NativeMethods.BNFreeTagReferences
			);
		}

		public void RemoveUserDataTag(ulong address , Tag tag)
		{
			NativeMethods.BNRemoveUserDataTag(
				this.handle , 
				address ,
				tag.DangerousGetHandle()
			);
		}
		
		public void RemoveUserDataTagsOfType(ulong address , TagType tagType)
		{
			NativeMethods.BNRemoveUserDataTagsOfType(
				this.handle , 
				address ,
				tagType.DangerousGetHandle()
			);
		}
		
		
		public void RemoveAutoDataTag(ulong address , Tag tag)
		{
			NativeMethods.BNRemoveAutoDataTag(
				this.handle , 
				address ,
				tag.DangerousGetHandle()
			);
		}

		public void RemoveAutoDataTagsOfType(ulong address , TagType tagType)
		{
			NativeMethods.BNRemoveAutoDataTagsOfType(
				this.handle , 
				address ,
				tagType.DangerousGetHandle()
			);
		}

		public string CheckForStringAnnotationType(
		
			ulong address  , 
			
			out StringType strType  , 
			
			bool allowShortStrings  , 
			
			bool allowLargeStrings  , 
			
			ulong childWidth  
		)
		{
			bool ok = NativeMethods.BNCheckForStringAnnotationType(
				this.handle ,
				address ,
				out IntPtr textPointer ,
				out strType ,
				allowShortStrings ,
				allowLargeStrings ,
				childWidth
			);

			if (!ok)
			{
				return string.Empty;
			}

			return UnsafeUtils.TakeUtf8String(textPointer);
		}

		public bool CanAssemble(Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			return NativeMethods.BNCanAssemble(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle()
			);
		}
		
		public bool IsNeverBranchPatchAvailable(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			return NativeMethods.BNIsNeverBranchPatchAvailable(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		} 
		
		public bool IsAlwaysBranchPatchAvailable(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			return NativeMethods.BNIsAlwaysBranchPatchAvailable(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		} 
		
		public bool IsInvertBranchPatchAvailable(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			return NativeMethods.BNIsInvertBranchPatchAvailable(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		} 
		
		public bool IsSkipAndReturnZeroPatchAvailable(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			return NativeMethods.BNIsSkipAndReturnZeroPatchAvailable(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		} 
		
		public bool IsSkipAndReturnValuePatchAvailable(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			return NativeMethods.BNIsSkipAndReturnValuePatchAvailable(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		} 
		
		public bool ConvertToNop(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			
			return NativeMethods.BNConvertToNop(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		} 
		
		public bool AlwaysBranch(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			
			return NativeMethods.BNAlwaysBranch(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		} 
		
		public bool InvertBranch(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			
			return NativeMethods.BNInvertBranch(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		} 
		
		public bool SkipAndReturnValue(
			ulong address ,
			ulong value = 0,
			Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			
			return NativeMethods.BNSkipAndReturnValue(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address,
				value
			);
		}

		public ulong GetInstructionLength(ulong address , Architecture? arch = null)
		{
			if (null == arch)
			{
				arch = this.DefaultArchitecture;
			}

			return NativeMethods.BNGetInstructionLength(
				this.handle ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				address
			);
		}
		
		public void NotifyDataWritten(ulong offset , ulong length)
		{
			NativeMethods.BNNotifyDataWritten(
				this.handle ,
				offset,
				length
			);
		}
		
		public void NotifyDataInserted(ulong offset , ulong length)
		{
			NativeMethods.BNNotifyDataInserted(
				this.handle ,
				offset,
				length
			);
		}
		
		public void NotifyDataRemoved(ulong offset , ulong length)
		{
			NativeMethods.BNNotifyDataRemoved(
				this.handle ,
				offset,
				length
			);
		}

		public Component? GetComponentByGuid( string guid)
		{
			return Component.TakeHandle(
				NativeMethods.BNGetComponentByGuid(this.handle , guid)
			);
		}
		
		public Component? GetComponentByPath( string path)
		{
			return Component.TakeHandle(
				NativeMethods.BNGetComponentByPath(this.handle , path)
			);
		}

		public Component RootComponent
		{
			get
			{
				return Component.MustTakeHandle(
					NativeMethods.BNGetRootComponent(this.handle )
				);
			}
		}

		public Component CreateComponent()
		{
			return Component.MustTakeHandle(
				NativeMethods.BNCreateComponent(
					this.handle 
				)
			);
		}
		
		public Component CreateComponentWithParent(string parentGUID)
		{
			return Component.MustTakeHandle(
				NativeMethods.BNCreateComponentWithParent(
					this.handle ,
					parentGUID 
				)
			);
		}
		
		public Component CreateComponentWithName( string name)
		{
			return Component.MustTakeHandle(
				NativeMethods.BNCreateComponentWithName(
					this.handle ,
					name
				)
			);
		}
		
		public Component CreateComponentWithParentAndName(string parentGUID , string name)
		{
			return Component.MustTakeHandle(
				NativeMethods.BNCreateComponentWithParentAndName(
					this.handle ,
					parentGUID ,
					name
				)
			);
		}

		public bool RemoveComponent(Component component)
		{
			return NativeMethods.BNRemoveComponent(
				this.handle ,
				component.DangerousGetHandle()
			);
		}
		
		public bool RemoveComponentByGuid(string guid)
		{
			return NativeMethods.BNRemoveComponentByGuid(
				this.handle ,
				guid
			);
		}

		public Component[] GetFunctionParentComponents(Function function)
		{
			IntPtr arrayPointer = NativeMethods.BNGetFunctionParentComponents(
				this.handle ,
				function.DangerousGetHandle() ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<Component>(
				arrayPointer ,
				arrayLength,
				Component.MustNewFromHandle,
				NativeMethods.BNFreeComponents
			);
		}
		
		public Component[] GetDataVariableParentComponents(ulong dataVariable)
		{
			IntPtr arrayPointer = NativeMethods.BNGetDataVariableParentComponents(
				this.handle ,
				dataVariable,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<Component>(
				arrayPointer ,
				arrayLength,
				Component.MustNewFromHandle,
				NativeMethods.BNFreeComponents
			);
		}

		public StringReference[] StringReferences
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetStrings(
					this.handle ,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeStructArray<BNStringReference , StringReference>(
					arrayPointer ,
					arrayLength ,
					StringReference.FromNative ,
					NativeMethods.BNFreeStringReferenceList
				);
			}
		}
		
		public StringReference[] GetStringsInRange(ulong start , ulong length)
		{
			IntPtr arrayPointer = NativeMethods.BNGetStringsInRange(
				this.handle ,
				start,
				length,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeStructArray<BNStringReference , StringReference>(
				arrayPointer ,
				arrayLength ,
				StringReference.FromNative ,
				NativeMethods.BNFreeStringReferenceList
			);
		}
		
		public StringReference? GetStringAtAddress(ulong address)
		{
			bool ok = NativeMethods.BNGetStringAtAddress(
				this.handle ,
				address,
				out BNStringReference strRef
			);

			if (!ok)
			{
				return null;
			}
			
			return StringReference.FromNative(strRef);
		}

		public ulong GetNextFunctionStartAfterAddress(ulong address)
		{
			return NativeMethods.BNGetNextFunctionStartAfterAddress(this.handle , address);
		}
		
		public ulong GetNextBasicBlockStartAfterAddress(ulong address)
		{
			return NativeMethods.BNGetNextBasicBlockStartAfterAddress(this.handle , address);
		}
		
		public ulong GetNextDataAfterAddress(ulong address)
		{
			return NativeMethods.BNGetNextDataAfterAddress(this.handle , address);
		}
		
		public ulong GetNextDataVariableStartAfterAddress(ulong address)
		{
			return NativeMethods.BNGetNextDataVariableStartAfterAddress(this.handle , address);
		}
		
		public DataVariable? GetNextDataVariableAfterAddress(ulong address)
		{
			while (address < this.End)
			{
				ulong dataVariableStart = this.GetNextDataVariableStartAfterAddress(address);

				if (dataVariableStart == this.End)
				{
					return null;
				}

				DataVariable? variable = this.GetDataVariableAtAddress(dataVariableStart);

				if (null != variable)
				{
					if (variable.Address < dataVariableStart)
					{
						address = variable.Address + variable.Type.Width;
						continue;
					}
					
					return variable;
				}
				else
				{
					return null;
				}
			}

			return null;
		}
		
		public ulong GetPreviousFunctionStartBeforeAddress(ulong address)
		{
			return NativeMethods.BNGetPreviousFunctionStartBeforeAddress(this.handle , address);
		}
		
		public ulong GetPreviousBasicBlockStartBeforeAddress(ulong address)
		{
			return NativeMethods.BNGetPreviousBasicBlockStartBeforeAddress(this.handle , address);
		}
		
		public ulong GetPreviousBasicBlockEndBeforeAddress(ulong address)
		{
			return NativeMethods.BNGetPreviousBasicBlockEndBeforeAddress(this.handle , address);
		}
		
		public ulong GetPreviousDataBeforeAddress(ulong address)
		{
			return NativeMethods.BNGetPreviousDataBeforeAddress(this.handle , address);
		}

		
		public ulong GetPreviousDataVariableStartBeforeAddress(ulong address)
		{
			return NativeMethods.BNGetPreviousDataVariableStartBeforeAddress(this.handle , address);
		}
		
		public DataVariable? GetPreviousDataVariableBeforeAddress(ulong address)
		{
			ulong dataVariableStart = this.GetNextDataVariableStartAfterAddress(address);

			if (dataVariableStart == this.Start)
			{
				return null;
			}

			return this.GetDataVariableAtAddress(dataVariableStart);
		}

		public QualifiedNameAndType? ParseTypeString(
			string text ,
			QualifiedName[]? typesAllowRedefinition = null,
			bool importDepencencies = true
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNParseTypeString(
					this.handle , 
					text,
					out BNQualifiedNameAndType result,
					out IntPtr errorPointer,
					new QualifiedNameList(typesAllowRedefinition).ToNativeEx(allocator),
					importDepencencies
				);

				if (!ok)
				{
					string errors = UnsafeUtils.TakeAnsiString(errorPointer);

					if (!string.IsNullOrEmpty(errors))
					{
						throw new Exception(errors);
					}		
					
					return null;
				}
				
				return QualifiedNameAndType.TakeNative(result);
			}
		}
		
		public TypeParserResult? ParseTypesString(
			string text ,
			string[] options,
			string[] includeDirs,
			QualifiedName[]? typesAllowRedefinition = null,
			bool importDepencencies = true
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				// options and includeDirs are const char** UTF-8 input blocks; build them
				// by hand because .NET cannot apply LPUTF8Str to string[] array elements.
				string[] safeOptions = options ?? Array.Empty<string>();
				string[] safeDirs = includeDirs ?? Array.Empty<string>();

				IntPtr optionsBlock = allocator.AllocUtf8StringArray(safeOptions);
				IntPtr includeDirsBlock = allocator.AllocUtf8StringArray(safeDirs);

				bool ok = NativeMethods.BNParseTypesString(
					this.handle ,
					text,
					optionsBlock,
					(ulong)safeOptions.Length,
					includeDirsBlock,
					(ulong)safeDirs.Length,
					out BNTypeParserResult result,
					out IntPtr errorPointer,
					new QualifiedNameList(typesAllowRedefinition).ToNativeEx(allocator),
					importDepencencies
				);

				if (!ok)
				{
					string errors = UnsafeUtils.TakeAnsiString(errorPointer);

					if (!string.IsNullOrEmpty(errors))
					{
						throw new Exception(errors);
					}		
					
					return null;
				}
				
				return TypeParserResult.TakeNative(result);
			}
		}

		public PossibleValueSet? ParsePossibleValueSet(
			string text,
			RegisterValueType state,
			ulong here
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNParsePossibleValueSet(
					this.handle , 
					text,
					state,
					out BNPossibleValueSet result,
					here,
					out IntPtr errorPointer
				);

				if (!ok)
				{
					string errors = UnsafeUtils.TakeAnsiString(errorPointer);

					if (!string.IsNullOrEmpty(errors))
					{
						throw new Exception(errors);
					}		
					
					return null;
				}
				
				return PossibleValueSet.TakeNative(result);
			}
		}

		public TypeContainer TypeContainer
		{
			get
			{
				return TypeContainer.MustTakeHandle(
					NativeMethods.BNGetAnalysisTypeContainer(this.handle)
				);
			}
		}
		
		public TypeContainer AutoTypeContainer
		{
			get
			{
				return TypeContainer.MustTakeHandle(
					NativeMethods.BNGetAnalysisAutoTypeContainer(this.handle)
				);
			}
		}
		
		public TypeContainer UserTypeContainer
		{
			get
			{
				return TypeContainer.MustTakeHandle(
					NativeMethods.BNGetAnalysisUserTypeContainer(this.handle)
				);
			}
		}

		public BinaryNinja.Type? GetTypeByName(QualifiedName name)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return BinaryNinja.Type.TakeHandle(

					NativeMethods.BNGetAnalysisTypeByName(
						this.handle ,
						name.ToNativeEx(allocator)
					)
				);
			}
		}
		
		public BinaryNinja.Type? GetTypeById(string id)
		{
			return BinaryNinja.Type.TakeHandle(

				NativeMethods.BNGetAnalysisTypeById(
					this.handle ,
					id
				)
			);
		}
		
		public QualifiedName GetTypeNameById(string id)
		{
			return QualifiedName.TakeNative(

				NativeMethods.BNGetAnalysisTypeNameById(
					this.handle ,
					id
				)
			);
		}
		
		public string GetTypeId(QualifiedName name)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return UnsafeUtils.TakeAnsiString(

					NativeMethods.BNGetAnalysisTypeId(
						this.handle ,
						name.ToNativeEx(allocator)
					)
				);
			}
		}

		public void AddTypeLibrary(TypeLibrary library)
		{
			NativeMethods.BNAddBinaryViewTypeLibrary(
				this.handle, 
				library.DangerousGetHandle()
			);
		}

		public TypeLibrary? GetTypeLibrary(string name)
		{
			return TypeLibrary.TakeHandle(
				NativeMethods.BNGetBinaryViewTypeLibrary(
					this.handle ,
					name
				)
			);
		}
		
		public bool IsTypeAutoDefined(QualifiedName name)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return NativeMethods.BNIsAnalysisTypeAutoDefined(
					this.handle ,
					name.ToNativeEx(allocator)
				);
			}
		}

		public QualifiedName DefineType(
			string typeId ,
			QualifiedName defaultName,
			BinaryNinja.Type type
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return QualifiedName.TakeNative(

					NativeMethods.BNDefineAnalysisType(
						this.handle ,
						typeId,
						defaultName.ToNativeEx(allocator),
						type.DangerousGetHandle()
					)
				);
			}
		}
		
		public void DefineUserType(
			QualifiedName name ,
			BinaryNinja.Type type
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNDefineUserAnalysisType(
					this.handle ,
					name.ToNativeEx(allocator),
					type.DangerousGetHandle()
				);
			}
		}
		
		public QualifiedNameAndId[] DefineTypes(
			QualifiedNameTypeAndId[] types,
			ProgressDelegate? progress = null
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeDelegates.BNProgressFunction? progressWrapper =
					null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

				ulong arrayLength = NativeMethods.BNDefineAnalysisTypes(
					this.handle ,
					allocator.ConvertToNativeArrayEx<BNQualifiedNameTypeAndId,QualifiedNameTypeAndId>(types),
					(ulong)types.Length,
					null == progressWrapper ? IntPtr.Zero :
						Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
					IntPtr.Zero,
					out IntPtr idsPointer,
					out IntPtr namesPointer
				);

				GC.KeepAlive(progressWrapper);

				string[] ids = UnsafeUtils.TakeStringArrayEx(
					idsPointer , 
					arrayLength);
				
				QualifiedName[] names = UnsafeUtils.TakeStructArrayEx<BNQualifiedName,QualifiedName>(
					namesPointer ,
					arrayLength,
					QualifiedName.FromNative,
					NativeMethods.BNFreeTypeNameList
				);
				
				List<QualifiedNameAndId> targets = new List<QualifiedNameAndId>();

				for (ulong i = 0; i < arrayLength; i++)
				{
					targets.Add(
						new QualifiedNameAndId(
							names[i],
							ids[i]
						)
					);
				}

				return targets.ToArray();
			}
		}
		
		public void DefineUserTypes(
			QualifiedNameAndType[] types,
			ProgressDelegate? progress = null
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeDelegates.BNProgressFunction? progressWrapper =
					null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

				NativeMethods.BNDefineUserAnalysisTypes(
					this.handle ,
					allocator.ConvertToNativeArrayEx<BNQualifiedNameAndType,QualifiedNameAndType>(types),
					(ulong)types.Length,
					null == progressWrapper ? IntPtr.Zero :
						Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
					IntPtr.Zero
				);

				GC.KeepAlive(progressWrapper);
			}
		}

		public void UndefineType(string typeId)
		{
			NativeMethods.BNUndefineAnalysisType(this.handle , typeId);
		}
		
		public void UndefineUserType(QualifiedName name)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNUndefineUserAnalysisType(
					this.handle , 
					name.ToNativeEx(allocator)
				);
			}
		}
		
		public void RenameType(QualifiedName oldName , QualifiedName newName)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNRenameAnalysisType(
					this.handle , 
					oldName.ToNativeEx(allocator),
					newName.ToNativeEx(allocator)
				);
			}
		}

		public BinaryNinja.Type? GetSystemCallType(uint id , Platform? platform = null)
		{
			if (null == platform)
			{
				platform = this.DefaultPlatform;
			}

			return BinaryNinja.Type.TakeHandle(

				NativeMethods.BNGetAnalysisSystemCallType(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
					id
				)
			);
		}
		
		public string GetSystemCallName(uint id , Platform? platform = null)
		{
			if (null == platform)
			{
				platform = this.DefaultPlatform;
			}

			return UnsafeUtils.TakeAnsiString(

				NativeMethods.BNGetAnalysisSystemCallName(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
					id
				)
			);
		}

		public BinaryNinja.Type? ImportTypeLibraryType(QualifiedName name , TypeLibrary? library = null)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return BinaryNinja.Type.TakeHandle(

					NativeMethods.BNBinaryViewImportTypeLibraryType(
						this.handle ,
						null == library ? IntPtr.Zero : library.DangerousGetHandle() ,
						name.ToNativeEx(allocator)
					)
				);
			}
		}
		
		public BinaryNinja.Type? ImportTypeLibraryTypeByGuid(string guid )
		{
			return BinaryNinja.Type.TakeHandle(

				NativeMethods.BNBinaryViewImportTypeLibraryTypeByGuid(
					this.handle ,
					guid
				)
			);
		}
		
		public BinaryNinja.Type? ImportTypeLibraryObject(
			QualifiedName name , 
			TypeLibrary? library 
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr libraryHandle = ( null == library ? IntPtr.Zero : library.DangerousGetHandle() );

				IntPtr typeHandle = NativeMethods.BNBinaryViewImportTypeLibraryObject(
					this.handle ,
					ref libraryHandle ,
					name.ToNativeEx(allocator)
				);

				return BinaryNinja.Type.TakeHandle(

					typeHandle
				);
			}
		}
		
		public void ExportTypeToTypeLibrary(
			TypeLibrary library ,
			QualifiedName name , 
			BinaryNinja.Type type
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNBinaryViewExportTypeToTypeLibrary(
					this.handle ,
					library.DangerousGetHandle(), 
					name.ToNativeEx(allocator),
					type.DangerousGetHandle()
				);
			}
		}
		
		public void ExportObjectToTypeLibrar(
			TypeLibrary library ,
			QualifiedName name , 
			BinaryNinja.Type type
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNBinaryViewExportObjectToTypeLibrary(
					this.handle ,
					library.DangerousGetHandle(), 
					name.ToNativeEx(allocator),
					type.DangerousGetHandle()
				);
			}
		}
	
		public void SetManualDependencies(
			TypeLibrary library ,
			QualifiedName[] viewTypeNames,
			QualifiedName[] libTypeNames,
			string[] libNames 
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				// libNames is a const char** UTF-8 input block (count-based); build it by
				// hand because .NET cannot apply LPUTF8Str to string[] array elements.
				string[] safeLibNames = libNames ?? Array.Empty<string>();
				IntPtr libNamesBlock = allocator.AllocUtf8StringArray(safeLibNames);

				NativeMethods.BNBinaryViewSetManualDependencies(
					this.handle,
					allocator.ConvertToNativeArrayEx<BNQualifiedName,QualifiedName>(viewTypeNames),
					allocator.ConvertToNativeArrayEx<BNQualifiedName,QualifiedName>(libTypeNames),
					libNamesBlock,
					( ulong )safeLibNames.Length
				);
			}
		}
		
		public void RecordImportedObjectLibrary(
			ulong address,
			TypeLibrary library ,
			QualifiedName name , 
			Platform? platform = null
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNBinaryViewRecordImportedObjectLibrary(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle(),
					address,
					library.DangerousGetHandle(), 
					name.ToNativeEx(allocator)
				);
			}
		}
		
		
		public TypeLibrary? LookupImportedObjectLibrary(
			ulong address,
			out QualifiedName? name,
			Platform? platform = null
		)
		{
			bool ok = NativeMethods.BNBinaryViewLookupImportedObjectLibrary(
				this.handle ,
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle(),
				address,
				out IntPtr libraryHandle,
				out BNQualifiedName rawName
			);

			if (!ok)
			{
				name = null;
				return null;
			}
			
			name = QualifiedName.TakeNative(rawName);
			
			return TypeLibrary.MustTakeHandle(libraryHandle);
		}
		
		public TypeLibrary? LookupImportedTypeLibrary(
			QualifiedName typeName,
			out QualifiedName? libraryName
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNBinaryViewLookupImportedTypeLibrary(
					this.handle ,
					typeName.ToNativeEx(allocator),
					out IntPtr libraryHandle,
					out BNQualifiedName rawLibraryName
				);

				if (!ok)
				{
					libraryName = null;
					return null;
				}
			
				libraryName = QualifiedName.TakeNative(rawLibraryName);
			
				return TypeLibrary.MustTakeHandle(libraryHandle);
			}
			
		}

		public TypeArchive? AttachTypeArchive(string id , string path)
		{
			return TypeArchive.TakeHandle(
				NativeMethods.BNBinaryViewAttachTypeArchive(this.handle , id , path)
			);
		}
		
		public bool DetachTypeArchive(string id)
		{
			return NativeMethods.BNBinaryViewDetachTypeArchive(this.handle , id);
		}
		
		public string GetTypeArchivePath(string id)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNBinaryViewGetTypeArchivePath(this.handle , id)
			);
		}

		public void RegisterPlatformTypes(Platform platform)
		{
			NativeMethods.BNRegisterPlatformTypes(this.handle , platform.DangerousGetHandle());
		}

		public Platform? LookupImportedTypePlatform(
			QualifiedName typeName,
			out QualifiedName? resultName
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNLookupImportedTypePlatform(
					this.handle ,
					typeName.ToNativeEx(allocator) ,
					out IntPtr platformHandle ,
					out BNQualifiedName rawResultName
				);

				if (!ok)
				{
					resultName = null;
					return null;
				}
				
				resultName = QualifiedName.TakeNative(rawResultName);
				
				return Platform.MustTakeHandle(platformHandle);
			}
		}

		public ulong? FindNextData(
			byte[] data ,
			ulong start = 0 ,
			FindFlag flags = FindFlag.FindCaseSensitive
		)
		{
			bool ok = NativeMethods.BNFindNextData(
				this.handle ,
				start ,
				new DataBuffer(data).DangerousGetHandle() ,
				out ulong result ,
				flags
			);

			if (!ok)
			{
				return null;
			}

			return result;
		}

		public ulong? FindNextDataWithProgress(
			byte[] data ,
			ulong start ,
			ulong end ,
			ProgressDelegate progress ,
			FindFlag flags = FindFlag.FindCaseSensitive
		)
		{
			NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

			bool ok = NativeMethods.BNFindNextDataWithProgress(
				this.handle ,
				start ,
				end ,
				new DataBuffer(data).DangerousGetHandle() ,
				out ulong result ,
				flags ,
				IntPtr.Zero ,
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper)
			);

			GC.KeepAlive(progressWrapper);

			if (!ok)
			{
				return null;
			}

			return result;
		}

		public ulong? FindNextText(
			string data ,
			ulong start ,
			FunctionViewType viewType ,
			DisassemblySettings? settings = null ,
			FindFlag flags = FindFlag.FindCaseSensitive
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNFindNextText(
					this.handle ,
					start ,
					data ,
					out ulong result ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle() ,
					flags ,
					viewType.ToNativeEx(allocator)
				);

				if (!ok)
				{
					return null;
				}

				return result;
			}
		}

		public ulong? FindNextTextWithProgress(
			string data ,
			ulong start ,
			ulong end ,
			ProgressDelegate progress ,
			FunctionViewType viewType ,
			DisassemblySettings? settings = null ,
			FindFlag flags = FindFlag.FindCaseSensitive
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

				bool ok = NativeMethods.BNFindNextTextWithProgress(
					this.handle ,
					start ,
					end ,
					data ,
					out ulong result ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle() ,
					flags ,
					viewType.ToNativeEx(allocator) ,
					IntPtr.Zero ,
					Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper)
				);

				GC.KeepAlive(progressWrapper);

				if (!ok)
				{
					return null;
				}

				return result;
			}
		}

		public ulong? FindNextConstant(
			ulong start ,
			ulong value ,
			FunctionViewType viewType ,
			DisassemblySettings? settings = null
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNFindNextConstant(
					this.handle ,
					start ,
					value ,
					out ulong result ,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle() ,
					viewType.ToNativeEx(allocator)
				);

				if (!ok)
				{
					return null;
				}

				return result;
			}
		}

		public ulong? FindNextConstantWithProgress(
			ulong data,
			ulong start,
			ulong end,
			ProgressDelegate progress,
			FunctionViewType viewType,
			DisassemblySettings? settings = null
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

				bool ok =  NativeMethods.BNFindNextConstantWithProgress(
					this.handle ,
					start,
					end,
					data,
					out ulong result,
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle(),
					viewType.ToNativeEx(allocator),
					IntPtr.Zero,
					Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper)
				);

				GC.KeepAlive(progressWrapper);
			
				if (!ok)
				{
					return null;
				}

				return result;
			}
		}

		public bool FindAllData(
			byte[] data ,
			ulong start ,
			ulong end ,
			MatchDataDelegate match ,
			ProgressDelegate? progress = null ,
			FindFlag flags = FindFlag.FindCaseSensitive
		)
		{
			NativeDelegates.BNProgressFunction? progressWrapper =
				null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

			// The match thunk is handed to the core as a raw function pointer and must survive the
			// whole synchronous scan; without KeepAlive a GC during the call could free it and
			// crash the native callback. The thunk takes (owns) each matched BNDataBuffer.
			NativeDelegates.MatchDataDelegate matchWrapper =
				UnsafeUtils.WrapMatchDataDelegate(match);

			bool result = NativeMethods.BNFindAllDataWithProgress(
				this.handle ,
				start ,
				end ,
				new DataBuffer(data).DangerousGetHandle() ,
				flags ,
				IntPtr.Zero ,
				null == progressWrapper
					? IntPtr.Zero
					: Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
				IntPtr.Zero ,
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.MatchDataDelegate>(matchWrapper)
			);

			GC.KeepAlive(progressWrapper);
			GC.KeepAlive(matchWrapper);

			return result;
		}


		public bool FindAllText(
			string data ,
			ulong start ,
			ulong end ,
			MatchTextDelegate match ,
			FunctionViewType viewType ,
			ProgressDelegate? progress = null ,
			FindFlag flags = FindFlag.FindCaseSensitive ,
			DisassemblySettings? settings = null
		)
		{
			// The core dereferences the settings pointer while rendering text, so a null settings
			// crashes the process. The official Python binding builds a default DisassemblySettings
			// when none is supplied; do the same and dispose only the one allocated here, never a
			// caller-owned instance.
			bool ownsSettings = null == settings;
			DisassemblySettings effectiveSettings =
				ownsSettings ? new DisassemblySettings() : settings!;

			try
			{
				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					NativeDelegates.BNProgressFunction? progressWrapper =
						null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

					// BNFindAllTextWithProgress invokes a 4-argument match callback
					// (ctxt, addr, const char* text, BNLinearDisassemblyLine*); the public
					// MatchTextDelegate mirrors that shape. Passing MatchDataDelegate here
					// reinterpreted the text pointer as a BNDataBuffer handle and crashed on the
					// first match.
					NativeDelegates.MatchTextDelegate matchWrapper =
						UnsafeUtils.WrapMatchTextDelegate(match);

					bool result = NativeMethods.BNFindAllTextWithProgress(
						this.handle ,
						start ,
						end ,
						data ,
						effectiveSettings.DangerousGetHandle() ,
						flags ,
						viewType.ToNativeEx(allocator) ,
						IntPtr.Zero ,
						null == progressWrapper
							? IntPtr.Zero
							: Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
						IntPtr.Zero ,
						Marshal.GetFunctionPointerForDelegate<NativeDelegates.MatchTextDelegate>(matchWrapper)
					);

					// The match thunk is handed to the core as a raw function pointer and must
					// survive the whole synchronous scan; without KeepAlive a GC during the call
					// could free it and crash the native callback.
					GC.KeepAlive(progressWrapper);
					GC.KeepAlive(matchWrapper);

					return result;
				}
			}
			finally
			{
				if (ownsSettings)
				{
					effectiveSettings.Dispose();
				}
			}
		}

		public bool FindAllConstant(
			ulong data ,
			ulong start ,
			ulong end ,
			MatchConstantDelegate match ,
			FunctionViewType viewType ,
			ProgressDelegate? progress = null ,
			DisassemblySettings? settings = null
		)
		{
			// The core dereferences the settings pointer while rendering instructions, so a null
			// settings crashes the process. The official Python binding builds a default
			// DisassemblySettings when none is supplied; do the same and dispose only the one
			// allocated here, never a caller-owned instance.
			bool ownsSettings = null == settings;
			DisassemblySettings effectiveSettings =
				ownsSettings ? new DisassemblySettings() : settings!;

			try
			{
				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					NativeDelegates.BNProgressFunction? progressWrapper =
						null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

					// BNFindAllConstantWithProgress invokes a 3-argument match callback
					// (ctxt, addr, BNLinearDisassemblyLine*); the public MatchConstantDelegate
					// mirrors that shape. Passing MatchDataDelegate here fed the line pointer to
					// the data-buffer thunk and crashed on the first match.
					NativeDelegates.MatchConstantDelegate matchWrapper =
						UnsafeUtils.WrapMatchConstantDelegate(match);

					bool result = NativeMethods.BNFindAllConstantWithProgress(
						this.handle ,
						start ,
						end ,
						data ,
						effectiveSettings.DangerousGetHandle() ,
						viewType.ToNativeEx(allocator) ,
						IntPtr.Zero ,
						null == progressWrapper
							? IntPtr.Zero
							: Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
						IntPtr.Zero ,
						Marshal.GetFunctionPointerForDelegate<NativeDelegates.MatchConstantDelegate>(matchWrapper)
					);

					// The match thunk is handed to the core as a raw function pointer and must
					// survive the whole synchronous scan; without KeepAlive a GC during the call
					// could free it and crash the native callback.
					GC.KeepAlive(progressWrapper);
					GC.KeepAlive(matchWrapper);

					return result;
				}
			}
			finally
			{
				if (ownsSettings)
				{
					effectiveSettings.Dispose();
				}
			}
		}
		
		public bool Search(
			string query ,
			MatchDataDelegate match ,
			ProgressDelegate? progress = null 
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeDelegates.BNProgressFunction? progressWrapper =
					null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

				// The match thunk is handed to the core as a raw function pointer and must survive
				// the whole synchronous search; without KeepAlive a GC during the call could free it
				// and crash the native callback. The thunk takes (owns) each matched BNDataBuffer.
				NativeDelegates.MatchDataDelegate matchWrapper =
					UnsafeUtils.WrapMatchDataDelegate(match);

				bool result = NativeMethods.BNSearch(
					this.handle ,
					query ,
					IntPtr.Zero ,
					null == progressWrapper
						? IntPtr.Zero
						: Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
					IntPtr.Zero ,
					Marshal.GetFunctionPointerForDelegate<NativeDelegates.MatchDataDelegate>(matchWrapper)
				);

				GC.KeepAlive(progressWrapper);
				GC.KeepAlive(matchWrapper);

				return result;
			}
		}
		
		public void Reanalyze()
		{
			NativeMethods.BNReanalyzeAllFunctions(this.handle);
		}

		public Workflow? Workflow
		{
			get
			{
				return Workflow.TakeHandle(

					NativeMethods.BNGetWorkflowForBinaryView(this.handle)
				);
			}
		}

		public bool Rebase(ulong address , ProgressDelegate? progress)
		{
			if (null == progress)
			{
				 return NativeMethods.BNRebase(
					this.handle,
					address
				);
			}
			else
			{
				NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

				bool result = NativeMethods.BNRebaseWithProgress(
					this.handle,
					address,
					IntPtr.Zero,
					Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper)
				);

				GC.KeepAlive(progressWrapper);

				return result;
			}
			
		}

		public void ShowPlainTextReport(string title , string contents)
		{
			NativeMethods.BNShowPlainTextReport(this.handle , title , contents);
		}
		
		public void ShowMarkdownReport(string title , string contents , string plaintext)
		{
			NativeMethods.BNShowMarkdownReport(this.handle , title , contents , plaintext);
		}
		
		public void ShowHTMLReport(string title , string contents , string plaintext)
		{
			NativeMethods.BNShowHTMLReport(this.handle , title , contents , plaintext);
		}
		
		public void ShowGraphReport(string title , FlowGraph graph)
		{
			NativeMethods.BNShowGraphReport(
				this.handle ,
				title , 
				graph.DangerousGetHandle() 
			) ;
		}

		public bool GetAddressInput(
			out ulong result,
			string prompt,
			string title ,
			ulong? currentAddress = null
		)
		{
			if (null == currentAddress)
			{
				currentAddress = this.File.Offset;
			}
			
			return NativeMethods.BNGetAddressInput(
				out result,
				prompt,
				title,
				this.handle,
				currentAddress?? 0
			);
		}
		
		public void BeginBulkAddSegments()
		{
			NativeMethods.BNBeginBulkAddSegments(this.handle);
		}
		
		public void EndBulkAddSegments()
		{
			NativeMethods.BNEndBulkAddSegments(this.handle);
		}
		
		public void CancelBulkAddSegments()
		{
			NativeMethods.BNCancelBulkAddSegments(this.handle);
		}

		public void AddAutoSegment(
		    ulong start ,
		    ulong length , 
		    ulong dataOffset ,
		    ulong dataLength,
		    uint flags
	    )
	    {
		    NativeMethods.BNAddAutoSegment(
			    this.handle ,
			    start , 
			    length , 
			    dataOffset ,
			    dataLength , 
			    flags
		    );
	    }

		public void AddAutoSegments(SegmentInfo[] segments)
		{
			NativeMethods.BNAddAutoSegments(
				this.handle,
				UnsafeUtils.ConvertToNativeArray<BNSegmentInfo,SegmentInfo>(
					segments
					),
				(ulong)segments.Length
			);
		}
	    
	    public void RemoveAutoSegment(ulong start , ulong length)
	    {
		    NativeMethods.BNRemoveAutoSegment(
			    this.handle ,
			    start , 
			    length 
		    );
	    }
	    
	    public void AddUserSegment(
		    ulong start ,
		    ulong length , 
		    ulong dataOffset ,
		    ulong dataLength,
		    uint flags
	    )
	    {
		    NativeMethods.BNAddUserSegment(
			    this.handle ,
			    start , 
			    length , 
			    dataOffset ,
			    dataLength , 
			    flags
		    );
	    }
	    
	    public void AddUserSegments(SegmentInfo[] segments)
	    {
		    NativeMethods.BNAddUserSegments(
			    this.handle,
			    UnsafeUtils.ConvertToNativeArray<BNSegmentInfo,SegmentInfo>(
				    segments
			    ),
			    (ulong)segments.Length
		    );
	    }
	    
	    public void RemoveUserSegment(
		    ulong start ,
		    ulong length 
	    )
	    {
		    NativeMethods.BNRemoveUserSegment(
			    this.handle ,
			    start , 
			    length 
		    );
	    }

	   

	    public Segment? GetSegmentAt(ulong address)
	    {
		    return Segment.TakeHandle(
			    NativeMethods.BNGetSegmentAt(this.handle , address)
		    );
	    }
	    
	    public bool GetAddressForDataOffset(ulong offset , out ulong address)
	    {
		    return NativeMethods.BNGetAddressForDataOffset(this.handle , offset , out address);
	    }
	    
	    public bool GetDataOffsetForAddress(ulong address , out ulong offset)
	    {
		    offset = 0;
		    
		    Segment? segment = this.GetSegmentAt(address);

		    if (null == segment)
		    {
			    return false;
		    }

		    if (address >= segment.Start && address < segment.End)
		    {
			    offset = address - segment.Start;

			    if (offset >segment.DataLength)
			    {
				    return false;
			    }
			    
			    offset = segment.DataOffset + offset;

			    return true;
		    }

		    return false;
	    }

	    
	    public void AddAutoSection(
		    string name ,
		    ulong start,
		    ulong length , 
		    SectionSemantics semantics = SectionSemantics.DefaultSectionSemantics,
		    string type = "",
		    ulong align = 1,
		    ulong entrySize = 1 ,
		    string linkedSection = "",
		    string infoSection = "",
		    ulong infoData = 0
	    )
	    {
		    NativeMethods.BNAddAutoSection(
			    this.handle ,
			    name,
			    start , 
			    length , 
			    semantics,
			    type,
			    align ,
			    entrySize , 
			    linkedSection,
			    infoSection,
			    infoData
		    );
	    }
	    
	    public void RemoveAutoSection(string name)
	    {
		    NativeMethods.BNRemoveAutoSection(this.handle , name);
	    }
	    
	    public void AddUserSection(
		    string name ,
		    ulong start,
		    ulong length , 
		    SectionSemantics semantics = SectionSemantics.DefaultSectionSemantics,
		    string type = "",
		    ulong align = 1,
		    ulong entrySize = 1 ,
		    string linkedSection = "",
		    string infoSection = "",
		    ulong infoData = 0
	    )
	    {
		    NativeMethods.BNAddUserSection(
			    this.handle ,
			    name,
			    start , 
			    length , 
			    semantics,
			    type,
			    align ,
			    entrySize , 
			    linkedSection,
			    infoSection,
			    infoData
		    );
	    }
	    
	    public void RemoveUserSection(string name)
	    {
		    NativeMethods.BNRemoveUserSection(this.handle , name);
	    }
	    
	    public Section[] GetSectionsAt(ulong address)
	    {
		    IntPtr arrayPointer =  NativeMethods.BNGetSectionsAt(
			    this.handle ,
			    address,
			    out ulong arrayLength
			 );
		    
		    return UnsafeUtils.TakeHandleArrayEx<Section>(
			    arrayPointer,
			    arrayLength,
			    Section.MustNewFromHandle,
			    NativeMethods.BNFreeSectionList
			 );
	    }

	    public Section? GetSectionByName(string name)
	    {
		    return Section.TakeHandle(
			    NativeMethods.BNGetSectionByName(this.handle , name)
		    );
	    }

	 

	    public ulong[] GlobalCommentedAddresses
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetGlobalCommentedAddresses(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeNumberArray<ulong>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeAddressList
			    );
		    }
	    }

	    public string GetGlobalCommentForAddress(ulong address)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetGlobalCommentForAddress(this.handle , address)
		    );
	    }

	    public void SetGlobalCommentForAddress(ulong address , string comment)
	    {
		    NativeMethods.BNSetGlobalCommentForAddress(this.handle , address, comment);
	    }

	    public DebugInfo DebugInfo
	    {
		    get
		    {
			    return DebugInfo.MustTakeHandle(
				    NativeMethods.BNGetDebugInfo(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNSetDebugInfo(
				    this.handle, 
				    null == value ? IntPtr.Zero : value.DangerousGetHandle()
				);
		    }
	    }

	    public void ApplyDebugInfo(DebugInfo debugInfo)
	    {
		    NativeMethods.BNApplyDebugInfo(this.handle, debugInfo.DangerousGetHandle());
	    }

	    public Metadata? QueryMetadata(string key)
	    {
		    return Metadata.TakeHandle(
			    NativeMethods.BNBinaryViewQueryMetadata(this.handle , key)
		    );
	    }

	    public void StoreMetadata(string key , Metadata data , bool isAuto = false )
	    {
		    NativeMethods.BNBinaryViewStoreMetadata(
			    this.handle ,
			    key , 
			    data.DangerousGetHandle() ,
			    isAuto
			);
	    }

	    public void RemoveMetadata(string key)
	    {
		    NativeMethods.BNBinaryViewRemoveMetadata(this.handle , key);
	    }

	    public Metadata Metadata
	    {
		    get
		    {
			    return Metadata.MustTakeHandle(
				    NativeMethods.BNBinaryViewGetMetadata(this.handle)
			    );
		    }
	    }
	    
	    public Metadata AutoMetadata
	    {
		    get
		    {
			    return Metadata.MustTakeHandle(
				    NativeMethods.BNBinaryViewGetAutoMetadata(this.handle)
			    );
		    }
	    }

	    public string[] LoadSettingsTypeNames
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNBinaryViewGetLoadSettingsTypeNames(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeAnsiStringArray(
				    arrayPointer , 
				    arrayLength,
				    NativeMethods.BNFreeStringList
				    );
		    }
	    }

	    public Settings? GetLoadSettings(string typeName)
	    {
		    return Settings.TakeHandle(
			    NativeMethods.BNBinaryViewGetLoadSettings(this.handle , typeName)
		    );
	    }

	    public void SetLoadSettings(string typeName , Settings settings)
	    {
		    NativeMethods.BNBinaryViewSetLoadSettings(
			    this.handle ,
			    typeName ,
			    settings.DangerousGetHandle()
			);
	    }

	    public ulong ParseExpression(string expression, ulong here = 0)
	    {
		    bool ok = NativeMethods.BNParseExpression(
			    this.handle ,
			    expression ,
			    out ulong offset ,
			    here ,
			    out IntPtr errorPointer
		    );

		    if (!ok)
		    {
			    string errors = UnsafeUtils.TakeUtf8String(
				    errorPointer ,
				    NativeMethods.BNFreeParseError
				);
			    
			    throw new Exception(errors);
		    }
		    
		    return offset;
	    }

	    public BinaryReader CreateReader()
	    {
		    return new BinaryReader(this);
	    }
	    
	    public BinaryWriter CreateWriter()
	    {
		    return new BinaryWriter(this);
	    }
	    
	   
	    public ExternalLibrary AddExternalLibrary(
		    string name ,
		    ProjectFile? projectFile = null,
		    bool isAuto = false
		)
	    {
		    return ExternalLibrary.MustTakeHandle(
			    NativeMethods.BNBinaryViewAddExternalLibrary(
				    this.handle ,
				    name ,
				    null == projectFile ? IntPtr.Zero : projectFile.DangerousGetHandle() ,
				    isAuto
			    )
		    );
	    }

	    public void RemoveExternalLibrary(string name )
	    {
		    NativeMethods.BNBinaryViewRemoveExternalLibrary(this.handle , name);
	    }

	    public ExternalLibrary? GetExternalLibrary(string name)
	    {
		    return ExternalLibrary.TakeHandle(
			    NativeMethods.BNBinaryViewGetExternalLibrary(this.handle , name)
			);
	    }

	    public ExternalLibrary[] ExternalLibraries
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNBinaryViewGetExternalLibraries(
				    this.handle,
				    out ulong arrayLength
				);

			    return UnsafeUtils.TakeHandleArrayEx<ExternalLibrary>(
				    arrayPointer ,
				    arrayLength ,
				    ExternalLibrary.MustNewFromHandle,
				    NativeMethods.BNFreeExternalLibraryList
			    );
		    }
	    }

	    public string StringifyUnicodeData(
		    Architecture arch,
		    DataBuffer buffer,
		    bool nullTerminates,
		    bool allowShortStrings,
		    out StringType stringType
		)
	    {
		    string text = string.Empty;
		    
		    bool ok = NativeMethods.BNStringifyUnicodeData(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    buffer.DangerousGetHandle() ,
			    nullTerminates ,
			    allowShortStrings ,
			    out IntPtr textPointer ,
			    out stringType
		    );
		    
		    if (ok)
		    {
			    if (StringType.AsciiString == stringType)
			    {
					text = UnsafeUtils.TakeAnsiString(textPointer);
			    }
			    else if (StringType.Utf16String == stringType)
			    {
				    text = UnsafeUtils.TakeUtf16String(textPointer);
			    }
			    else if (StringType.Utf32String == stringType)
			    {
				    text = UnsafeUtils.TakeUtf32String(textPointer);
			    }
			    else if (StringType.Utf8String == stringType)
			    {
				    text = UnsafeUtils.TakeUtf8String(textPointer);
			    }
			    else
			    {
				    throw new Exception("unknown string type");
			    }
		    }
		    else
		    {
			    text = String.Empty;
			    stringType = StringType.AsciiString;
		    }

		    return text;
	    }

	    
	    public PluginCommand[] ValidPluginCommands
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommands(
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

	    public PluginCommand[] GetValidPluginCommandsForAddress(ulong address)
	    {
		    
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForAddress(
			    this.handle,
			    address,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public PluginCommand[] GetValidPluginCommandsForRange(
		    ulong address,
		    ulong length
		)
	    {
		    
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForRange(
			    this.handle,
			    address,
			    length,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public PluginCommand[] GetValidPluginCommandsForFunction(Function function)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForFunction(
			    this.handle,
			    function.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public PluginCommand[] GetValidPluginCommandsForLowLevelILFunction(LowLevelILFunction function)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForLowLevelILFunction(
			    this.handle,
			    function.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public PluginCommand[] GetValidPluginCommandsForLowLevelILInstruction(
		    LowLevelILFunction function,
		    ulong instruction
		    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForLowLevelILInstruction(
			    this.handle,
			    function.DangerousGetHandle(),
			    instruction,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    
	    public PluginCommand[] GetValidPluginCommandsForMediumLevelILFunction(MediumLevelILFunction function)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForMediumLevelILFunction(
			    this.handle,
			    function.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public PluginCommand[] GetValidPluginCommandsForMediumLevelILInstruction(
		    MediumLevelILFunction function,
		    ulong instruction
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForMediumLevelILInstruction(
			    this.handle,
			    function.DangerousGetHandle(),
			    instruction,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public PluginCommand[] GetValidPluginCommandsForHighLevelILFunction(HighLevelILFunction function)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForHighLevelILFunction(
			    this.handle,
			    function.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public PluginCommand[] GetValidPluginCommandsForHighLevelILInstruction(
		    HighLevelILFunction function,
		    ulong instruction
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForHighLevelILInstruction(
			    this.handle,
			    function.DangerousGetHandle(),
			    instruction,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public bool CreateDatabase(
		    string filename,
		    SaveSettings? settings = null,
		    ProgressDelegate? progress = null)
	    {
		    // A derived view (for example the ELF view layered on raw bytes) cannot be saved
		    // directly: BNCreateDatabase must run against the root (raw) parent view. Walk up
		    // the parent chain first, exactly as C++ BinaryView::CreateDatabase does
		    // (binaryview.cpp:1673) and as Python FileMetadata.create_database does by passing
		    // self.raw.handle (filemetadata.py). Without this, saving a freshly loaded binary
		    // returns false and writes a truncated 90112-byte database.
		    BinaryView? parent = this.Parent;

		    if (null != parent)
		    {
			    using (parent)
			    {
				    return parent.CreateDatabase(filename, settings, progress);
			    }
		    }

		    if (null == progress)
		    {
			    return NativeMethods.BNCreateDatabase(
				    this.handle ,
				    filename,
				    null == settings ? IntPtr.Zero :  settings.DangerousGetHandle()
			    );
		    }
		    else
		    {
			    NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

			    bool result = NativeMethods.BNCreateDatabaseWithProgress(
				    this.handle ,
				    filename ,
				    IntPtr.Zero ,
				    Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    );

			    GC.KeepAlive(progressWrapper);

			    return result;
		    }
	    }
	    
	    public bool SaveAutoSnapshot(
		    SaveSettings? settings = null,
		    ProgressDelegate? progress = null)
	    {
		    if (null == progress)
		    {
			    return NativeMethods.BNSaveAutoSnapshot(
				    this.handle ,
				    null == settings ? IntPtr.Zero :  settings.DangerousGetHandle()
			    );
		    }
		    else
		    {
			    NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

			    bool result = NativeMethods.BNSaveAutoSnapshotWithProgress(
				    this.handle ,
				    IntPtr.Zero ,
				    Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    );

			    GC.KeepAlive(progressWrapper);

			    return result;
		    }
	    }
	    
	    public string BaseMemoryMapDescription
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetBaseMemoryMapDescription(this.handle)
			    );
		    }
	    }
		
	    public string MemoryMapDescription
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetMemoryMapDescription(this.handle)
			    );
		    }
	    }

	    public bool LogicalMemoryMapEnabled
	    {
		    set
		    {
			    NativeMethods.BNSetLogicalMemoryMapEnabled(this.handle, value);
		    }
	    }

	    public bool MemoryMapActivated
	    {
		    get
		    {
			    return NativeMethods.BNIsMemoryMapActivated(this.handle);
		    }
	    }

	    public bool AddMemoryRegion(
		    string name,
		    ulong start,
		    BinaryView data,
		    uint flags
		)
	    {
		    return NativeMethods.BNAddBinaryMemoryRegion(
			    this.handle, 
			    name, 
			    start,
			    data.DangerousGetHandle(), 
			    flags
			);
	    }
	    
	    public bool AddMemoryRegion(
		    string name,
		    ulong start,
		    DataBuffer data,
		    uint flags
	    )
	    {
		    return NativeMethods.BNAddDataMemoryRegion(
			    this.handle, 
			    name, 
			    start,
			    data.DangerousGetHandle(), 
			    flags
		    );
	    }
	    
	    public bool AddMemoryRegion(
		    string name,
		    ulong start,
		    FileAccessor data,
		    uint flags
	    )
	    {
		    return NativeMethods.BNAddRemoteMemoryRegion(
			    this.handle, 
			    name, 
			    start,
			    data.ToNative(), 
			    flags
		    );
	    }
	    
	    public bool AddMemoryRegion(
		    string name,
		    ulong start,
		    ulong length,
		    uint flags,
		    byte fill
	    )
	    {
		    return NativeMethods.BNAddUnbackedMemoryRegion(
			    this.handle, 
			    name, 
			    start,
			    length, 
			    flags,
			    fill
		    );
	    }

	    public bool RemoveMemoryRegion(string name)
	    {
		    return NativeMethods.BNRemoveMemoryRegion(this.handle, name);
	    }

	    public string GetActiveMemoryRegionAt(ulong address)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetActiveMemoryRegionAt(this.handle , address)
		    );
	    }
	    
	    public uint GetMemoryRegionFlags(string name)
	    {
		    return NativeMethods.BNGetMemoryRegionFlags(this.handle , name);
	    }
	    
	    public bool SetMemoryRegionFlags(string name , uint flags)
	    {
		    return NativeMethods.BNSetMemoryRegionFlags(this.handle , name , flags);
	    }
	    
	    public bool IsMemoryRegionEnabled(string name)
	    {
		    return NativeMethods.BNIsMemoryRegionEnabled(this.handle , name);
	    }
	    
	    public bool SetMemoryRegionEnabled(string name , bool enable)
	    {
		    return NativeMethods.BNSetMemoryRegionEnabled(this.handle , name , enable);
	    }
	    
	    public bool IsMemoryRegionRebaseable(string name)
	    {
		    return NativeMethods.BNIsMemoryRegionRebaseable(this.handle , name );
	    }
	    
	    public bool SetMemoryRegionRebaseable(string name , bool rebaseable)
	    {
		    return NativeMethods.BNSetMemoryRegionRebaseable(this.handle , name , rebaseable);
	    }
	    
	    public byte GetMemoryRegionFill(string name)
	    {
		    return NativeMethods.BNGetMemoryRegionFill(this.handle , name);
	    }
	    
	    public bool SetMemoryRegionFill(string name , byte fill)
	    {
		    return NativeMethods.BNSetMemoryRegionFill(this.handle , name , fill);
	    }
	   
	    public bool IsMemoryRegionLocal(string name)
	    {
		    return NativeMethods.BNIsMemoryRegionLocal(this.handle , name );
	    }

	    public void ResetMemoryMap()
	    {
		    NativeMethods.BNResetMemoryMap(this.handle);
	    }
	    
	    public BinaryViewType[] BinaryViewTypes
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBinaryViewTypesForData(
				    this.handle,
				    out ulong arrayLength
			    );
	
			    return UnsafeUtils.TakeHandleArray<BinaryViewType>(
				    arrayPointer, 
				    arrayLength,
				    BinaryViewType.MustFromHandle,
				    NativeMethods.BNFreeBinaryViewTypeList
			    );
		    }
	    }
	  
	    public Logger? GetLogger(string name)
	    {
		    return Logger.GetLogger(name , this.File.SessionId);
	    }
	    
	    public Logger CreateLogger(string name)
	    {
		    return Logger.CreateLogger(name , this.File.SessionId);
	    }

	    public Logger GetOrCreateLogger(string name)
	    {
		    return Logger.GetOrCreateLogger(name , this.File.SessionId);
	    }
	    
	    public BinaryView? CreateCustomView(
		    string  name,
		    CustomBinaryView view,
		    FileMetadata? file = null
	    )
	    {
		    if (null == file)
		    {
			    file = this.File;
		    }

		    return BinaryView.TakeHandle(
			    NativeMethods.BNCreateCustomBinaryView(
				    name ,
				    file.DangerousGetHandle() ,
				    this.DangerousGetHandle() ,
				    view.ToNative()
			    )
		    );
	    }

	    public Symbol? ChooseSymbol(string prompt = "Choose" , string title = "Choose a symbol")
	    {
		    int? index = Core.GetLargeChoiceInput(
			    prompt ,
			    title ,
			    this.SymbolNames
		    );

		    if (null == index)
		    {
			    return null;
		    }
		    
		    return this.GetSymbolByRawName(this.SymbolNames[(int)index]);
	    }
	    
	    public Function? ChooseFunction(string prompt = "Choose" , string title = "Choose a function")
	    {
		    int? index = Core.GetLargeChoiceInput(
			    prompt ,
			    title ,
			    this.SymbolNames
		    );

		    if (null == index)
		    {
			    return null;
		    }
		    
		    return this.GetFunctionByRawName(this.SymbolNames[(int)index]);
	    }

	    // ─── Type archive association methods ────────────────────────────────────

	    /// <summary>
	    /// Disassociates a type in this binary view from its corresponding type archive entry.
	    /// After disassociation the type is treated as a local type with no archive link.
	    /// </summary>
	    /// <param name="typeId">The analysis type ID to disassociate from its archive type.</param>
	    /// <returns>True if the disassociation succeeded, false otherwise.</returns>
	    public bool DisassociateTypeArchiveType(string typeId)
	    {
		    // Forward the disassociation request to the native API.
		    return NativeMethods.BNBinaryViewDisassociateTypeArchiveType(
			    this.handle ,
			    typeId
		    );
	    }

	    /// <summary>
	    /// Retrieves the synchronisation status of the given type with respect to its
	    /// associated type archive entry.
	    /// </summary>
	    /// <param name="typeId">The analysis type ID to query.</param>
	    /// <returns>The current SyncStatus value for the type.</returns>
	    public SyncStatus GetTypeArchiveSyncStatus(string typeId)
	    {
		    // Query the native sync status for the given analysis type.
		    return NativeMethods.BNBinaryViewGetTypeArchiveSyncStatus(
			    this.handle ,
			    typeId
		    );
	    }

	    /// <summary>
	    /// Looks up a type name by its GUID string.
	    /// </summary>
	    /// <param name="guid">The GUID identifying the type.</param>
	    /// <returns>The qualified name corresponding to the GUID.</returns>
	    public QualifiedName GetTypeNameByGuid(string guid)
	    {
		    // Retrieve the native BNQualifiedName struct and convert to managed form.
		    return QualifiedName.TakeNative(
			    NativeMethods.BNBinaryViewGetTypeNameByGuid(
				    this.handle ,
				    guid
			    )
		    );
	    }

	    /// <summary>
	    /// Removes the external location mapping for the given source symbol.
	    /// After removal the symbol no longer maps to an external target.
	    /// </summary>
	    /// <param name="sourceSymbol">The source symbol whose external location should be removed.</param>
	    public void RemoveExternalLocation(Symbol sourceSymbol)
	    {
		    // Forward the removal request to the native API.
		    NativeMethods.BNBinaryViewRemoveExternalLocation(
			    this.handle ,
			    sourceSymbol.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Gets the external location mapping for the given source symbol, if one exists.
	    /// </summary>
	    /// <param name="sourceSymbol">The source symbol to query.</param>
	    /// <returns>The ExternalLocation for the symbol, or null if none is set.</returns>
	    public ExternalLocation? GetExternalLocation(Symbol sourceSymbol)
	    {
		    // Retrieve the handle and take ownership; returns null when the handle is zero.
		    return ExternalLocation.TakeHandle(
			    NativeMethods.BNBinaryViewGetExternalLocation(
				    this.handle ,
				    sourceSymbol.DangerousGetHandle()
			    )
		    );
	    }

	    /// <summary>
	    /// Adds an external location mapping from a source symbol to a target in an
	    /// external library.
	    /// </summary>
	    /// <param name="sourceSymbol">The import stub or reference symbol in this binary.</param>
	    /// <param name="library">The external library that contains the target.</param>
	    /// <param name="targetSymbol">The symbol name within the external library.</param>
	    /// <param name="targetAddress">Pointer to the target address, or IntPtr.Zero if unset.</param>
	    /// <param name="isAuto">True if the location was auto-discovered, false if user-defined.</param>
	    /// <returns>The newly created ExternalLocation, or null on failure.</returns>
	    public ExternalLocation? AddExternalLocation(
		    Symbol sourceSymbol ,
		    ExternalLibrary library ,
		    string targetSymbol ,
		    IntPtr targetAddress ,
		    bool isAuto = false
	    )
	    {
		    // Create the native external location and take ownership of the returned handle.
		    return ExternalLocation.TakeHandle(
			    NativeMethods.BNBinaryViewAddExternalLocation(
				    this.handle ,
				    sourceSymbol.DangerousGetHandle() ,
				    library.DangerousGetHandle() ,
				    targetSymbol ,
				    targetAddress ,
				    isAuto
			    )
		    );
	    }

	    /// <summary>
	    /// Retrieves all external locations defined in this binary view.
	    /// </summary>
	    /// <returns>An array of ExternalLocation objects.</returns>
	    public unsafe ExternalLocation[] GetExternalLocations()
	    {
		    // 1. Stack-allocate the count variable and retrieve the native handle array.
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNBinaryViewGetExternalLocations(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    // 2. Convert the handle array to managed objects and free the native list.
		    return UnsafeUtils.TakeHandleArrayEx<ExternalLocation>(
			    ptr ,
			    count ,
			    ExternalLocation.MustNewFromHandle ,
			    NativeMethods.BNFreeExternalLocationList
		    );
	    }

	    // ─── Type archive association query methods ──────────────────────────────

	    /// <summary>
	    /// Given an archive ID and an archive type ID, retrieves the corresponding
	    /// analysis type ID in this binary view.
	    /// </summary>
	    /// <param name="archiveId">The type archive identifier.</param>
	    /// <param name="archiveTypeId">The type ID within the archive.</param>
	    /// <param name="typeId">On success, receives the analysis type ID.</param>
	    /// <returns>True if the mapping was found, false otherwise.</returns>
	    public bool GetAssociatedTypeArchiveTypeSource(
		    string archiveId ,
		    string archiveTypeId ,
		    out string typeId
	    )
	    {
		    // 1. Call the native function; typeId is an out char* the core allocates.
		    IntPtr typeIdPointer;
		    bool ok = NativeMethods.BNBinaryViewGetAssociatedTypeArchiveTypeSource(
			    this.handle ,
			    archiveId ,
			    archiveTypeId ,
			    out typeIdPointer
		    );

		    // 2. Decode + free the result (no-op on null); empty on failure.
		    typeId = ok ? UnsafeUtils.TakeUtf8String(typeIdPointer) : string.Empty;

		    return ok;
	    }

	    /// <summary>
	    /// Given an analysis type ID, retrieves the archive ID and archive type ID it
	    /// is associated with.
	    /// </summary>
	    /// <param name="typeId">The analysis type ID to look up.</param>
	    /// <param name="archiveId">On success, receives the archive identifier.</param>
	    /// <param name="archiveTypeId">On success, receives the type ID within the archive.</param>
	    /// <returns>True if the mapping was found, false otherwise.</returns>
	    public bool GetAssociatedTypeArchiveTypeTarget(
		    string typeId ,
		    out string archiveId ,
		    out string archiveTypeId
	    )
	    {
		    // 1. Call the native function; archiveId/archiveTypeId are out char* the
		    //    core allocates.
		    IntPtr archiveIdPointer;
		    IntPtr archiveTypeIdPointer;
		    bool ok = NativeMethods.BNBinaryViewGetAssociatedTypeArchiveTypeTarget(
			    this.handle ,
			    typeId ,
			    out archiveIdPointer ,
			    out archiveTypeIdPointer
		    );

		    // 2. Decode + free the results (no-op on null); empty on failure.
		    archiveId = ok ? UnsafeUtils.TakeUtf8String(archiveIdPointer) : string.Empty;
		    archiveTypeId = ok ? UnsafeUtils.TakeUtf8String(archiveTypeIdPointer) : string.Empty;

		    return ok;
	    }

	    // ─── Type archive pull / push methods ───────────────────────────────────

	    /// <summary>
	    /// Pulls (imports) types from a type archive into this binary view. Returns
	    /// the updated archive type IDs and the corresponding analysis type IDs that
	    /// were created or refreshed.
	    /// </summary>
	    /// <param name="archiveId">The type archive identifier to pull from.</param>
	    /// <param name="archiveTypeIds">The archive type IDs to pull.</param>
	    /// <param name="updatedArchiveTypeIds">On success, receives the archive type IDs that were updated.</param>
	    /// <param name="updatedAnalysisTypeIds">On success, receives the analysis type IDs that were updated.</param>
	    /// <returns>True if the pull operation succeeded, false otherwise.</returns>
	    public unsafe bool PullTypeArchiveTypes(
		    string archiveId ,
		    string[] archiveTypeIds ,
		    out string[] updatedArchiveTypeIds ,
		    out string[] updatedAnalysisTypeIds
	    )
	    {
		    // 1. Stack-allocate locals for the native out-params.
		    ulong count = 0;
		    IntPtr updatedArchiveTypeIdsPtr = IntPtr.Zero;
		    IntPtr updatedAnalysisTypeIdsPtr = IntPtr.Zero;

		    // 2. Build archiveTypeIds as a UTF-8 const char** block, then call the
		    // native pull function (.NET cannot apply LPUTF8Str to string[] elements,
		    // so non-ASCII would otherwise corrupt through the system ANSI code page).
		    string[] safeArchiveTypeIds = archiveTypeIds ?? Array.Empty<string>();

		    bool ok;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr archiveTypeIdsBlock = allocator.AllocUtf8StringArray(safeArchiveTypeIds);

			    ok = NativeMethods.BNBinaryViewPullTypeArchiveTypes(
				    this.handle ,
				    archiveId ,
				    archiveTypeIdsBlock ,
				    (ulong)safeArchiveTypeIds.Length ,
				    (IntPtr)(&updatedArchiveTypeIdsPtr) ,
				    (IntPtr)(&updatedAnalysisTypeIdsPtr) ,
				    (IntPtr)(&count)
			    );
		    }

		    // 3. On success, convert the native string arrays to managed arrays.
		    if (ok)
		    {
			    updatedArchiveTypeIds = UnsafeUtils.TakeAnsiStringArray(
				    updatedArchiveTypeIdsPtr ,
				    count ,
				    NativeMethods.BNFreeStringList
			    );

			    updatedAnalysisTypeIds = UnsafeUtils.TakeAnsiStringArray(
				    updatedAnalysisTypeIdsPtr ,
				    count ,
				    NativeMethods.BNFreeStringList
			    );
		    }
		    else
		    {
			    // 4. On failure, return empty arrays.
			    updatedArchiveTypeIds = Array.Empty<string>();
			    updatedAnalysisTypeIds = Array.Empty<string>();
		    }

		    return ok;
	    }

	    /// <summary>
	    /// Pushes (exports) types from this binary view into a type archive. Returns
	    /// the updated analysis type IDs and the corresponding archive type IDs that
	    /// were created or refreshed.
	    /// </summary>
	    /// <param name="archiveId">The type archive identifier to push to.</param>
	    /// <param name="typeIds">The analysis type IDs to push.</param>
	    /// <param name="updatedAnalysisTypeIds">On success, receives the analysis type IDs that were updated.</param>
	    /// <param name="updatedArchiveTypeIds">On success, receives the archive type IDs that were updated.</param>
	    /// <returns>True if the push operation succeeded, false otherwise.</returns>
	    public unsafe bool PushTypeArchiveTypes(
		    string archiveId ,
		    string[] typeIds ,
		    out string[] updatedAnalysisTypeIds ,
		    out string[] updatedArchiveTypeIds
	    )
	    {
		    // 1. Stack-allocate locals for the native out-params.
		    ulong count = 0;
		    IntPtr updatedAnalysisTypeIdsPtr = IntPtr.Zero;
		    IntPtr updatedArchiveTypeIdsPtr = IntPtr.Zero;

		    // 2. Build typeIds as a UTF-8 const char** block, then call the native
		    // push function (.NET cannot apply LPUTF8Str to string[] elements, so
		    // non-ASCII would otherwise corrupt through the system ANSI code page).
		    string[] safeTypeIds = typeIds ?? Array.Empty<string>();

		    bool ok;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr typeIdsBlock = allocator.AllocUtf8StringArray(safeTypeIds);

			    ok = NativeMethods.BNBinaryViewPushTypeArchiveTypes(
				    this.handle ,
				    archiveId ,
				    typeIdsBlock ,
				    (ulong)safeTypeIds.Length ,
				    (IntPtr)(&updatedAnalysisTypeIdsPtr) ,
				    (IntPtr)(&updatedArchiveTypeIdsPtr) ,
				    (IntPtr)(&count)
			    );
		    }

		    // 3. On success, convert the native string arrays to managed arrays.
		    if (ok)
		    {
			    updatedAnalysisTypeIds = UnsafeUtils.TakeAnsiStringArray(
				    updatedAnalysisTypeIdsPtr ,
				    count ,
				    NativeMethods.BNFreeStringList
			    );

			    updatedArchiveTypeIds = UnsafeUtils.TakeAnsiStringArray(
				    updatedArchiveTypeIdsPtr ,
				    count ,
				    NativeMethods.BNFreeStringList
			    );
		    }
		    else
		    {
			    // 4. On failure, return empty arrays.
			    updatedAnalysisTypeIds = Array.Empty<string>();
			    updatedArchiveTypeIds = Array.Empty<string>();
		    }

		    return ok;
	    }

	    // ─── Type archive bulk query methods ─────────────────────────────────────

	    /// <summary>
	    /// Retrieves all type archive associations for this binary view, returning
	    /// triples of analysis type ID, archive ID, and archive type ID.
	    /// </summary>
	    /// <param name="typeIds">On return, the analysis type IDs that have archive associations.</param>
	    /// <param name="archiveIds">On return, the archive IDs for each association.</param>
	    /// <param name="archiveTypeIds">On return, the archive type IDs for each association.</param>
	    /// <returns>The number of associations returned.</returns>
	    public unsafe ulong GetAssociatedTypeArchiveTypes(
		    out string[] typeIds ,
		    out string[] archiveIds ,
		    out string[] archiveTypeIds
	    )
	    {
		    // 1. Stack-allocate locals for the native out-param pointers.
		    IntPtr typeIdsPtr = IntPtr.Zero;
		    IntPtr archiveIdsPtr = IntPtr.Zero;
		    IntPtr archiveTypeIdsPtr = IntPtr.Zero;

		    // 2. Call the native function to retrieve all associated type triples.
		    ulong count = NativeMethods.BNBinaryViewGetAssociatedTypeArchiveTypes(
			    this.handle ,
			    (IntPtr)(&typeIdsPtr) ,
			    (IntPtr)(&archiveIdsPtr) ,
			    (IntPtr)(&archiveTypeIdsPtr)
		    );

		    // 3. Convert each native string array to a managed array and free the native memory.
		    typeIds = UnsafeUtils.TakeAnsiStringArray(
			    typeIdsPtr ,
			    count ,
			    NativeMethods.BNFreeStringList
		    );

		    archiveIds = UnsafeUtils.TakeAnsiStringArray(
			    archiveIdsPtr ,
			    count ,
			    NativeMethods.BNFreeStringList
		    );

		    archiveTypeIds = UnsafeUtils.TakeAnsiStringArray(
			    archiveTypeIdsPtr ,
			    count ,
			    NativeMethods.BNFreeStringList
		    );

		    return count;
	    }

	    /// <summary>
	    /// Retrieves all type associations from a specific archive, returning pairs of
	    /// analysis type ID and archive type ID.
	    /// </summary>
	    /// <param name="archiveId">The type archive identifier to query.</param>
	    /// <param name="typeIds">On return, the analysis type IDs associated with the archive.</param>
	    /// <param name="archiveTypeIds">On return, the archive type IDs for each association.</param>
	    /// <returns>The number of associations returned.</returns>
	    public unsafe ulong GetAssociatedTypesFromArchive(
		    string archiveId ,
		    out string[] typeIds ,
		    out string[] archiveTypeIds
	    )
	    {
		    // 1. Stack-allocate locals for the native out-param pointers.
		    IntPtr typeIdsPtr = IntPtr.Zero;
		    IntPtr archiveTypeIdsPtr = IntPtr.Zero;

		    // 2. Call the native function to retrieve associations for the given archive.
		    ulong count = NativeMethods.BNBinaryViewGetAssociatedTypesFromArchive(
			    this.handle ,
			    archiveId ,
			    (IntPtr)(&typeIdsPtr) ,
			    (IntPtr)(&archiveTypeIdsPtr)
		    );

		    // 3. Convert the native string arrays and free the native memory.
		    typeIds = UnsafeUtils.TakeAnsiStringArray(
			    typeIdsPtr ,
			    count ,
			    NativeMethods.BNFreeStringList
		    );

		    archiveTypeIds = UnsafeUtils.TakeAnsiStringArray(
			    archiveTypeIdsPtr ,
			    count ,
			    NativeMethods.BNFreeStringList
		    );

		    return count;
	    }

	    /// <summary>
	    /// Retrieves the list of all type names that have type archive associations.
	    /// </summary>
	    /// <returns>An array of qualified names with archive associations.</returns>
	    public unsafe QualifiedName[] GetTypeArchiveTypeNameList()
	    {
		    // 1. Stack-allocate a local for the native BNQualifiedName* out-param.
		    IntPtr namesPtr = IntPtr.Zero;

		    // 2. Call the native function; it returns the count and fills the pointer.
		    ulong count = NativeMethods.BNBinaryViewGetTypeArchiveTypeNameList(
			    this.handle ,
			    (IntPtr)(&namesPtr)
		    );

		    // 3. Convert the native BNQualifiedName array to managed QualifiedName objects.
		    return UnsafeUtils.TakeStructArrayEx<BNQualifiedName , QualifiedName>(
			    namesPtr ,
			    count ,
			    QualifiedName.FromNative ,
			    NativeMethods.BNFreeTypeNameList
		    );
	    }

	    /// <summary>
	    /// Given a type name, retrieves all type archive associations for that name,
	    /// returning pairs of archive ID and archive type ID.
	    /// </summary>
	    /// <param name="name">The qualified type name to query.</param>
	    /// <param name="archiveIds">On return, the archive IDs for each association.</param>
	    /// <param name="archiveTypeIds">On return, the archive type IDs for each association.</param>
	    /// <returns>The number of associations returned.</returns>
	    public unsafe ulong GetTypeArchiveTypeNames(
		    QualifiedName name ,
		    out string[] archiveIds ,
		    out string[] archiveTypeIds
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // 1. Stack-allocate locals for the native out-param pointers.
			    IntPtr archiveIdsPtr = IntPtr.Zero;
			    IntPtr archiveTypeIdsPtr = IntPtr.Zero;

			    // 2. Call the native function with the marshalled name pointer.
			    ulong count = NativeMethods.BNBinaryViewGetTypeArchiveTypeNames(
				    this.handle ,
				    allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)) ,
				    (IntPtr)(&archiveIdsPtr) ,
				    (IntPtr)(&archiveTypeIdsPtr)
			    );

			    // 3. Convert the native string arrays and free the native memory.
			    archiveIds = UnsafeUtils.TakeAnsiStringArray(
				    archiveIdsPtr ,
				    count ,
				    NativeMethods.BNFreeStringList
			    );

			    archiveTypeIds = UnsafeUtils.TakeAnsiStringArray(
				    archiveTypeIdsPtr ,
				    count ,
				    NativeMethods.BNFreeStringList
			    );

			    return count;
		    }
	    }

	    // ─── Tag data methods ────────────────────────────────────────────────────

	    /// <summary>
	    /// Adds an auto data tag at the specified address.
	    /// </summary>
	    /// <param name="addr">The address to add the tag at.</param>
	    /// <param name="tag">The tag to add.</param>
	    public void AddAutoDataTag(ulong addr , Tag tag)
	    {
		    NativeMethods.BNAddAutoDataTag(
			    this.handle ,
			    addr ,
			    tag.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Returns the total count of all tag references of the given tag type across the binary view.
	    /// </summary>
	    /// <param name="tagType">The tag type to count references for.</param>
	    /// <returns>The number of tag references of the specified type.</returns>
	    public ulong GetAllTagReferencesOfTypeCount(TagType tagType)
	    {
		    return NativeMethods.BNGetAllTagReferencesOfTypeCount(
			    this.handle ,
			    tagType.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Retrieves data tags of a specific type at the given address.
	    /// </summary>
	    /// <param name="addr">The address to query tags at.</param>
	    /// <param name="tagType">The tag type to filter by.</param>
	    /// <returns>An array of matching Tag objects.</returns>
	    public unsafe Tag[] GetDataTagsOfType(ulong addr , TagType tagType)
	    {
		    // 1. Stack-allocate the count variable and retrieve the native handle array.
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetDataTagsOfType(
			    this.handle ,
			    addr ,
			    tagType.DangerousGetHandle() ,
			    (IntPtr)(&count)
		    );

		    // 2. Convert the handle array to managed Tag objects and free the native list.
		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    ptr ,
			    count ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }

	    /// <summary>
	    /// Retrieves auto data tags of a specific type at the given address.
	    /// </summary>
	    /// <param name="addr">The address to query tags at.</param>
	    /// <param name="tagType">The tag type to filter by.</param>
	    /// <returns>An array of matching Tag objects.</returns>
	    public unsafe Tag[] GetAutoDataTagsOfType(ulong addr , TagType tagType)
	    {
		    // 1. Stack-allocate the count variable and retrieve the native handle array.
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetAutoDataTagsOfType(
			    this.handle ,
			    addr ,
			    tagType.DangerousGetHandle() ,
			    (IntPtr)(&count)
		    );

		    // 2. Convert the handle array to managed Tag objects and free the native list.
		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    ptr ,
			    count ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }

	    /// <summary>
	    /// Retrieves user data tags of a specific type at the given address.
	    /// </summary>
	    /// <param name="addr">The address to query tags at.</param>
	    /// <param name="tagType">The tag type to filter by.</param>
	    /// <returns>An array of matching Tag objects.</returns>
	    public unsafe Tag[] GetUserDataTagsOfType(ulong addr , TagType tagType)
	    {
		    // 1. Stack-allocate the count variable and retrieve the native handle array.
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetUserDataTagsOfType(
			    this.handle ,
			    addr ,
			    tagType.DangerousGetHandle() ,
			    (IntPtr)(&count)
		    );

		    // 2. Convert the handle array to managed Tag objects and free the native list.
		    return UnsafeUtils.TakeHandleArrayEx<Tag>(
			    ptr ,
			    count ,
			    Tag.MustNewFromHandle ,
			    NativeMethods.BNFreeTagList
		    );
	    }

	    // ─── Debug info methods ──────────────────────────────────────────────────

	    /// <summary>
	    /// Retrieves all debug info parsers applicable to this binary view.
	    /// </summary>
	    /// <returns>An array of DebugInfoParser objects.</returns>
	    public unsafe DebugInfoParser[] GetDebugInfoParsersForView()
	    {
		    // 1. Stack-allocate the count variable and retrieve the native handle array.
		    ulong count = 0;

		    IntPtr ptr = NativeMethods.BNGetDebugInfoParsersForView(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    // 2. Convert the handle array to managed objects and free the native list.
		    return UnsafeUtils.TakeHandleArrayEx<DebugInfoParser>(
			    ptr ,
			    count ,
			    DebugInfoParser.MustNewFromHandle ,
			    NativeMethods.BNFreeDebugInfoParserList
		    );
	    }

	    // ─── Relocation methods ──────────────────────────────────────────────────

	    /// <summary>
	    /// Defines a relocation with a numeric target address.
	    /// </summary>
	    /// <param name="arch">The architecture for the relocation.</param>
	    /// <param name="info">The relocation info describing the relocation parameters.</param>
	    /// <param name="target">The target address of the relocation.</param>
	    /// <param name="reloc">The address of the relocation entry.</param>
	    internal void DefineRelocation(Architecture arch , BNRelocationInfo info , ulong target , ulong reloc)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    NativeMethods.BNDefineRelocation(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    allocator.AllocStruct<BNRelocationInfo>(info) ,
				    target ,
				    reloc
			    );
		    }
	    }

	    /// <summary>
	    /// Defines a relocation with a symbol as the target.
	    /// </summary>
	    /// <param name="arch">The architecture for the relocation.</param>
	    /// <param name="info">The relocation info describing the relocation parameters.</param>
	    /// <param name="target">The target symbol of the relocation.</param>
	    /// <param name="reloc">The address of the relocation entry.</param>
	    internal void DefineSymbolRelocation(Architecture arch , BNRelocationInfo info , Symbol target , ulong reloc)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    NativeMethods.BNDefineSymbolRelocation(
				    this.handle ,
				    arch.DangerousGetHandle() ,
				    allocator.AllocStruct<BNRelocationInfo>(info) ,
				    target.DangerousGetHandle() ,
				    reloc
			    );
		    }
	    }

	    // ─── All type reference methods ──────────────────────────────────────────

	    /// <summary>
	    /// Retrieves all references (code, data, and type) for the specified type.
	    /// </summary>
	    /// <param name="type">The qualified type name to query.</param>
	    /// <param name="limit">Whether to limit the number of results.</param>
	    /// <param name="maxItems">The maximum number of items to return when limit is true.</param>
	    /// <returns>An AllTypeReferences object containing code, data, and type references.</returns>
	    public unsafe AllTypeReferences GetAllReferencesForType(
		    QualifiedName type ,
		    bool limit = false ,
		    ulong maxItems = 0
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // 1. Call the native function to retrieve all references for the type.
			    BNAllTypeReferences native = NativeMethods.BNGetAllReferencesForType(
				    this.handle ,
				    allocator.AllocStruct<BNQualifiedName>(type.ToNativeEx(allocator)) ,
				    limit ,
				    maxItems
			    );

			    // 2. Convert the inner arrays from the native struct to managed arrays.
			    AllTypeReferences result = new AllTypeReferences();

			    result.CodeRefs = UnsafeUtils.TakeStructArrayEx<BNReferenceSource , ReferenceSource>(
				    native.codeRefs ,
				    native.codeRefCount ,
				    ReferenceSource.FromNative ,
				    NativeMethods.BNFreeCodeReferences
			    );

			    result.DataRefs = UnsafeUtils.TakeNumberArray<ulong>(
				    native.dataRefs ,
				    native.dataRefCount ,
				    NativeMethods.BNFreeDataReferences
			    );

			    result.TypeRefs = UnsafeUtils.TakeStructArrayEx<BNTypeReferenceSource , TypeReferenceSource>(
				    native.typeRefs ,
				    native.typeRefCount ,
				    TypeReferenceSource.FromNative ,
				    NativeMethods.BNFreeTypeReferences
			    );

			    return result;
		    }
	    }

	    /// <summary>
	    /// Retrieves all references (code, data-to, data-from, and type) for the specified type field.
	    /// </summary>
	    /// <param name="type">The qualified type name to query.</param>
	    /// <param name="offset">The field offset within the type.</param>
	    /// <param name="limit">Whether to limit the number of results.</param>
	    /// <param name="maxItems">The maximum number of items to return when limit is true.</param>
	    /// <returns>An AllTypeFieldReferences object containing code, data, and type references.</returns>
	    public unsafe AllTypeFieldReferences GetAllReferencesForTypeField(
		    QualifiedName type ,
		    ulong offset ,
		    bool limit = false ,
		    ulong maxItems = 0
	    )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // 1. Call the native function to retrieve all references for the type field.
			    BNAllTypeFieldReferences native = NativeMethods.BNGetAllReferencesForTypeField(
				    this.handle ,
				    allocator.AllocStruct<BNQualifiedName>(type.ToNativeEx(allocator)) ,
				    offset ,
				    limit ,
				    maxItems
			    );

			    // 2. Convert the inner arrays from the native struct to managed arrays.
			    AllTypeFieldReferences result = new AllTypeFieldReferences();

			    result.CodeRefs = UnsafeUtils.TakeStructArrayEx<BNTypeFieldReference , TypeFieldReference>(
				    native.codeRefs ,
				    native.codeRefCount ,
				    TypeFieldReference.FromNative ,
				    NativeMethods.BNFreeTypeFieldReferences
			    );

			    result.DataRefsTo = UnsafeUtils.TakeNumberArray<ulong>(
				    native.dataRefsTo ,
				    native.dataRefToCount ,
				    NativeMethods.BNFreeDataReferences
			    );

			    result.DataRefsFrom = UnsafeUtils.TakeNumberArray<ulong>(
				    native.dataRefsFrom ,
				    native.dataRefFromCount ,
				    NativeMethods.BNFreeDataReferences
			    );

			    result.TypeRefs = UnsafeUtils.TakeStructArrayEx<BNTypeReferenceSource , TypeReferenceSource>(
				    native.typeRefs ,
				    native.typeRefCount ,
				    TypeReferenceSource.FromNative ,
				    NativeMethods.BNFreeTypeReferences
			    );

			    return result;
		    }
	    }

	    // ─── Tag reference type count methods ────────────────────────────────────

	    /// <summary>
	    /// Retrieves a dictionary mapping each tag type to its total reference count
	    /// across the entire binary view.
	    /// </summary>
	    /// <returns>A dictionary mapping TagType to its reference count.</returns>
	    public unsafe IDictionary<TagType , ulong> GetAllTagReferenceTypeCounts()
	    {
		    // 1. Stack-allocate the out-param pointers for the parallel arrays.
		    IntPtr tagTypesPtr = IntPtr.Zero;
		    IntPtr countsPtr = IntPtr.Zero;
		    ulong count = 0;

		    // 2. Call the native function to retrieve parallel arrays of tag types and counts.
		    NativeMethods.BNGetAllTagReferenceTypeCounts(
			    this.handle ,
			    (IntPtr)(&tagTypesPtr) ,
			    (IntPtr)(&countsPtr) ,
			    (IntPtr)(&count)
		    );

		    // 3. Build a dictionary by reading from the parallel native arrays.
		    Dictionary<TagType , ulong> result = new Dictionary<TagType , ulong>();

		    if (IntPtr.Zero != tagTypesPtr && IntPtr.Zero != countsPtr && 0 != count)
		    {
			    for (ulong i = 0; i < count; i++)
			    {
				    // 3.1 Read the tag type handle pointer from the tag types array.
				    IntPtr tagTypeHandle = Marshal.ReadIntPtr(
					    tagTypesPtr ,
					    checked((int)(i * (ulong)IntPtr.Size))
				    );

				    // 3.2 Read the corresponding count value from the counts array.
				    ulong refCount = (ulong)Marshal.ReadInt64(
					    countsPtr ,
					    checked((int)(i * sizeof(ulong)))
				    );

				    // 3.3 Wrap the native handle and add to the dictionary.
				    TagType tagType = TagType.MustNewFromHandle(tagTypeHandle);
				    result[tagType] = refCount;
			    }
		    }

		    // 4. Free both native arrays using the dedicated free function.
		    if (IntPtr.Zero != tagTypesPtr || IntPtr.Zero != countsPtr)
		    {
			    NativeMethods.BNFreeTagReferenceTypeCounts(tagTypesPtr , countsPtr , count);
		    }

		    return result;
	    }

	    /// <summary>
	    /// Adds an auto-analysis data reference from one address to another.
	    /// Unlike AddUserDataReference, this reference is managed by the analysis engine.
	    /// </summary>
	    /// <param name="from">The source address of the reference.</param>
	    /// <param name="to">The target address of the reference.</param>
	    public void AddDataReference(ulong from , ulong to)
	    {
		    NativeMethods.BNAddDataReference(this.handle , from , to);
	    }

	    /// <summary>
	    /// Adds inferred names for outer structure members from a HLIL variable expression.
	    /// Returns the list of names added.
	    /// </summary>
	    /// <param name="type">The structure type to resolve names for.</param>
	    /// <param name="hlil">The high-level IL function containing the variable expression.</param>
	    /// <param name="varExpr">The HLIL expression index referencing the variable.</param>
	    /// <returns>An array of added member names.</returns>
	    public unsafe string[] AddNamesForOuterStructureMembers(
		    BinaryNinja.Type type ,
		    HighLevelILFunction hlil ,
		    ulong varExpr
	    )
	    {
		    // 1. Stack-allocate the count variable.
		    ulong nameCount = 0;

		    // 2. Call the native function to retrieve the names.
		    IntPtr arrayPointer = NativeMethods.BNAddNamesForOuterStructureMembers(
			    this.handle ,
			    type.DangerousGetHandle() ,
			    hlil.DangerousGetHandle() ,
			    varExpr ,
			    (IntPtr)(&nameCount)
		    );

		    // 3. Convert the native string array to managed strings and free.
		    return UnsafeUtils.TakeAnsiStringArray(
			    arrayPointer ,
			    nameCount ,
			    NativeMethods.BNFreeStringList
		    );
	    }

	    /// <summary>
	    /// Creates a snapshot of the current view state with the given view name.
	    /// </summary>
	    /// <param name="viewName">The name to assign to the snapshot.</param>
	    /// <returns>True if the snapshot was created successfully.</returns>
	    public bool CreateSnapshotedView(string viewName)
	    {
		    return NativeMethods.BNCreateSnapshotedView(this.handle , viewName);
	    }

	    /// <summary>
	    /// Creates a snapshot of the current view state with progress reporting.
	    /// </summary>
	    /// <param name="viewName">The name to assign to the snapshot.</param>
	    /// <param name="progress">Optional progress callback, or null for no progress reporting.</param>
	    /// <returns>True if the snapshot was created successfully.</returns>
	    public bool CreateSnapshotedViewWithProgress(
		    string viewName ,
		    ProgressDelegate? progress = null
	    )
	    {
		    if (null == progress)
		    {
			    return NativeMethods.BNCreateSnapshotedViewWithProgress(
				    this.handle ,
				    viewName ,
				    IntPtr.Zero ,
				    IntPtr.Zero
			    );
		    }
		    else
		    {
			    NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

			    bool result = NativeMethods.BNCreateSnapshotedViewWithProgress(
				    this.handle ,
				    viewName ,
				    IntPtr.Zero ,
				    Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper)
			    );

			    GC.KeepAlive(progressWrapper);

			    return result;
		    }
	    }

	    /// <summary>
	    /// Gets the display string for an integer value using the specified display type.
	    /// </summary>
	    /// <param name="displayType">The integer display format (hex, decimal, etc.).</param>
	    /// <param name="value">The integer value to format.</param>
	    /// <param name="inputWidth">The width of the input value in bytes.</param>
	    /// <param name="isSigned">Whether the value should be treated as signed.</param>
	    /// <returns>A formatted display string for the integer.</returns>
	    public string GetDisplayStringForInteger(
		    IntegerDisplayType displayType ,
		    ulong value ,
		    ulong inputWidth ,
		    bool isSigned
	    )
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetDisplayStringForInteger(
				    this.handle ,
				    displayType ,
				    value ,
				    inputWidth ,
				    isSigned
			    )
		    );
	    }

	    /// <summary>
	    /// Gets the count of tag references for a specific tag type.
	    /// </summary>
	    /// <param name="tagType">The tag type to count references for.</param>
	    /// <returns>The number of tag references of the specified type.</returns>
	    public ulong GetTagReferencesOfTypeCount(TagType tagType)
	    {
		    return NativeMethods.BNGetTagReferencesOfTypeCount(
			    this.handle ,
			    tagType.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Generates unique section names from an input array of proposed names.
	    /// </summary>
	    /// <param name="names">The array of proposed section names.</param>
	    /// <returns>An array of unique section names with conflicts resolved.</returns>
	    public string[] GetUniqueSectionNames(string[] names)
	    {
		    // 1. Build the names const char** as a UTF-8 block, then call native
		    // (.NET cannot apply LPUTF8Str to string[] elements, so non-ASCII would
		    // otherwise corrupt through the system ANSI code page).
		    string[] safeNames = names ?? Array.Empty<string>();

		    IntPtr arrayPointer;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr namesBlock = allocator.AllocUtf8StringArray(safeNames);

			    arrayPointer = NativeMethods.BNGetUniqueSectionNames(
				    this.handle ,
				    namesBlock ,
				    (ulong)safeNames.Length
			    );
		    }

		    // 2. Convert the result array and free.
		    return UnsafeUtils.TakeAnsiStringArray(
			    arrayPointer ,
			    (ulong)safeNames.Length ,
			    NativeMethods.BNFreeStringList
		    );
	    }

	    /// <summary>
	    /// Gets all symbols visible in the current binary view within the given namespace.
	    /// </summary>
	    /// <param name="ns">Optional namespace filter, or null for all namespaces.</param>
	    /// <returns>An array of visible Symbol objects.</returns>
	    public unsafe Symbol[] GetVisibleSymbols(NameSpace? ns = null)
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // 2. Call the native function with optional namespace.
			    IntPtr arrayPointer = NativeMethods.BNGetVisibleSymbols(
				    this.handle ,
				    (IntPtr)(&count) ,
				    null == ns
					    ? IntPtr.Zero
					    : allocator.AllocStruct<BNNameSpace>(
						    ns.ToNativeEx(allocator)
					    )
			    );

			    // 3. Convert the handle array to managed Symbol objects.
			    return UnsafeUtils.TakeHandleArrayEx<Symbol>(
				    arrayPointer ,
				    count ,
				    Symbol.MustNewFromHandle ,
				    NativeMethods.BNFreeSymbolList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets whether this binary view is currently applying debug info.
	    /// </summary>
	    public bool IsApplyingDebugInfo
	    {
		    get
		    {
			    return NativeMethods.BNIsApplyingDebugInfo(this.handle);
		    }
	    }

	    /// <summary>
	    /// Parses a text-format file and returns a BinaryView representing its contents.
	    /// </summary>
	    /// <param name="filename">The path to the text-format file.</param>
	    /// <returns>A BinaryView if parsing succeeds, or null on failure.</returns>
	    public static BinaryView? ParseTextFormat(string filename)
	    {
		    return BinaryView.TakeHandle(
			    NativeMethods.BNParseTextFormat(filename)
		    );
	    }

	    /// <summary>
	    /// Removes a tag reference from this binary view.
	    /// </summary>
	    /// <param name="tagRef">The tag reference to remove.</param>
	    public void RemoveTagReference(TagReference tagRef)
	    {
		    NativeMethods.BNRemoveTagReference(
			    this.handle ,
			    tagRef
		    );
	    }

	    /// <summary>
	    /// Posts a workflow request for this binary view.
	    /// </summary>
	    /// <param name="request">The workflow request string.</param>
	    /// <returns>The response string from the workflow engine.</returns>
	    public string PostWorkflowRequestForBinaryView(string request)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNPostWorkflowRequestForBinaryView(
				    this.handle ,
				    request
			    )
		    );
	    }

	    /// <summary>
	    /// Shows a workflow report for this binary view.
	    /// </summary>
	    /// <param name="name">The name of the workflow report to show.</param>
	    public void ShowWorkflowReportForBinaryView(string name)
	    {
		    NativeMethods.BNShowWorkflowReportForBinaryView(this.handle , name);
	    }

	    // Per-view registration tracking. A notification registered on N views gets N independent
	    // contexts (one struct + delegate set per registration), mirroring TypeArchive's per-archive map.
	    private readonly Dictionary<BinaryDataNotification, UnsafeUtils.BinaryDataNotificationContext>
		    m_dataNotifications = new Dictionary<BinaryDataNotification, UnsafeUtils.BinaryDataNotificationContext>();

	    /// <summary>
	    /// Registers a notification object to receive binary-data events from this view, mirroring Python
	    /// <c>BinaryView.register_notification</c> (binaryview.py:5570). The notification must be
	    /// unregistered (or this view released) to stop delivery. Registering the same instance again is
	    /// a no-op (Python tolerates this; the core would otherwise emit a gratuitous notification_barrier).
	    /// </summary>
	    /// <param name="notification">The notification whose virtual methods receive the events.</param>
	    public void RegisterDataNotification(BinaryDataNotification notification)
	    {
		    if (null == notification)
		    {
			    throw new ArgumentNullException(nameof(notification));
		    }

		    if (this.m_dataNotifications.ContainsKey(notification))
		    {
			    return;
		    }

		    // 1. Build the native callback struct (roots the 55 delegates + the context).
		    UnsafeUtils.BinaryDataNotificationContext context =
			    new UnsafeUtils.BinaryDataNotificationContext(notification);
		    IntPtr nativePointer = context.BuildNative();

		    // 2. Register with the core, then keep the context rooted so the delegates outlive the
		    // registration.
		    NativeMethods.BNRegisterDataNotification(this.handle, nativePointer);
		    this.m_dataNotifications[notification] = context;
	    }

	    /// <summary>
	    /// Unregisters a previously-registered notification, mirroring Python
	    /// <c>BinaryView.unregister_notification</c> (binaryview.py:5588). Frees the native callback
	    /// struct and releases the rooted delegates. Unregistering an unknown notification is a no-op.
	    /// </summary>
	    /// <param name="notification">The notification to unregister.</param>
	    public void UnregisterDataNotification(BinaryDataNotification notification)
	    {
		    if (null == notification)
		    {
			    throw new ArgumentNullException(nameof(notification));
		    }

		    if (!this.m_dataNotifications.TryGetValue(
				    notification, out UnsafeUtils.BinaryDataNotificationContext? context))
		    {
			    return;
		    }

		    NativeMethods.BNUnregisterDataNotification(this.handle, context.NativeStruct);
		    context.FreeNative();
		    this.m_dataNotifications.Remove(notification);
	    }

	    /// <summary>
	    /// Gets the analysis type that corresponds to the given named type reference.
	    /// </summary>
	    /// <param name="typeRef">The named type reference to resolve.</param>
	    /// <returns>The resolved Type, or null if not found.</returns>
	    public BinaryNinja.Type? GetAnalysisTypeByRef(NamedTypeReference typeRef)
	    {
		    return BinaryNinja.Type.TakeHandle(
			    NativeMethods.BNGetAnalysisTypeByRef(
				    this.handle ,
				    typeRef.DangerousGetHandle()
			    )
		    );
	    }

	    // ===================================================================
	    // Tag retrieval
	    // ===================================================================

	    /// <summary>
	    /// Gets a tag by its unique identifier string.
	    /// </summary>
	    /// <param name="tagId">The tag identifier string.</param>
	    /// <returns>The Tag with the given ID, or null if not found.</returns>
	    public Tag? GetTag(string tagId)
	    {
		    // BNGetTag returns an OWNED handle (the C++ wrapper adopts it with no addref), so
		    // take it directly instead of bumping the reference count.
		    return Tag.TakeHandle(
			    NativeMethods.BNGetTag(this.handle , tagId)
		    );
	    }

	    // ===================================================================
	    // Tag type retrieval with type filter
	    // ===================================================================

	    /// <summary>
	    /// Gets a tag type by name and tag type kind.
	    /// </summary>
	    /// <param name="name">The name of the tag type.</param>
	    /// <param name="type">The tag type kind to filter by.</param>
	    /// <returns>The matching TagType, or null if not found.</returns>
	    public TagType? GetTagTypeWithType(string name , TagTypeType type)
	    {
		    // BNGetTagTypeWithType returns an OWNED handle (the C++ wrapper adopts it with no
		    // addref), matching the untyped GetTagType overload; take it directly.
		    return TagType.TakeHandle(
			    NativeMethods.BNGetTagTypeWithType(this.handle , name , type)
		    );
	    }

	    /// <summary>
	    /// Gets a tag type by its unique identifier and tag type kind.
	    /// </summary>
	    /// <param name="id">The unique identifier of the tag type.</param>
	    /// <param name="type">The tag type kind to filter by.</param>
	    /// <returns>The matching TagType, or null if not found.</returns>
	    public TagType? GetTagTypeByIdWithType(string id , TagTypeType type)
	    {
		    // BNGetTagTypeByIdWithType returns an OWNED handle (the C++ wrapper adopts it with
		    // no addref), matching the untyped GetTagTypeById overload; take it directly.
		    return TagType.TakeHandle(
			    NativeMethods.BNGetTagTypeByIdWithType(this.handle , id , type)
		    );
	    }

	    // ===================================================================
	    // Background analysis task
	    // ===================================================================

	    /// <summary>
	    /// Gets the background analysis task associated with this binary view.
	    /// </summary>
	    /// <returns>The BackgroundTask if analysis is running, or null otherwise.</returns>
	    public BackgroundTask? GetBackgroundAnalysisTask()
	    {
		    return BackgroundTask.TakeHandle(
			    NativeMethods.BNGetBackgroundAnalysisTask(this.handle)
		    );
	    }

	    // ===================================================================
	    // Import address symbol resolution
	    // ===================================================================

	    /// <summary>
	    /// Resolves the imported function symbol from an import address symbol.
	    /// </summary>
	    /// <param name="sym">The import address symbol to resolve.</param>
	    /// <param name="addr">The address associated with the import.</param>
	    /// <returns>The resolved function Symbol, or null if not found.</returns>
	    public Symbol? ImportedFunctionFromImportAddressSymbol(Symbol sym , ulong addr)
	    {
		    IntPtr raw = NativeMethods.BNImportedFunctionFromImportAddressSymbol(
			    sym.DangerousGetHandle() ,
			    addr
		    );

		    if (raw == IntPtr.Zero)
		    {
			    return null;
		    }

		    return new Symbol(raw , true);
	    }
	}

}