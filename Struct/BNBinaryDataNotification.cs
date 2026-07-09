using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNBinaryDataNotification 
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;

		/// <summary>
		/// uint64_t (*notificationBarrier)(void*ctxt, BNBinaryView* view)
		/// </summary>
		internal IntPtr notificationBarrier;

		/// <summary>
		/// void (*dataWritten)(void* ctxt, BNBinaryView* view, uint64_t offset, size_t len)
		/// </summary>
		internal IntPtr dataWritten;

		/// <summary>
		/// void (*dataInserted)(void* ctxt, BNBinaryView* view, uint64_t offset, size_t len)
		/// </summary>
		internal IntPtr dataInserted;

		/// <summary>
		/// void (*dataRemoved)(void* ctxt, BNBinaryView* view, uint64_t offset, uint64_t len)
		/// </summary>
		internal IntPtr dataRemoved;

		/// <summary>
		/// void (*functionAdded)(void* ctxt, BNBinaryView* view, BNFunction* func)
		/// </summary>
		internal IntPtr functionAdded;

		/// <summary>
		/// void (*functionRemoved)(void* ctxt, BNBinaryView* view, BNFunction* func)
		/// </summary>
		internal IntPtr functionRemoved;

		/// <summary>
		/// void (*functionUpdated)(void* ctxt, BNBinaryView* view, BNFunction* func)
		/// </summary>
		internal IntPtr functionUpdated;

		/// <summary>
		/// void (*functionUpdateRequested)(void* ctxt, BNBinaryView* view, BNFunction* func)
		/// </summary>
		internal IntPtr functionUpdateRequested;

		/// <summary>
		/// void (*dataVariableAdded)(void* ctxt, BNBinaryView* view, BNDataVariable* var)
		/// </summary>
		internal IntPtr dataVariableAdded;

		/// <summary>
		/// void (*dataVariableRemoved)(void* ctxt, BNBinaryView* view, BNDataVariable* var)
		/// </summary>
		internal IntPtr dataVariableRemoved;

		/// <summary>
		/// void (*dataVariableUpdated)(void* ctxt, BNBinaryView* view, BNDataVariable* var)
		/// </summary>
		internal IntPtr dataVariableUpdated;

		/// <summary>
		/// void (*dataMetadataUpdated)(void* ctxt, BNBinaryView* view, uint64_t offset)
		/// </summary>
		internal IntPtr dataMetadataUpdated;

		/// <summary>
		/// void (*tagTypeUpdated)(void* ctxt, BNBinaryView* view, BNTagType* tagType)
		/// </summary>
		internal IntPtr tagTypeUpdated;

		/// <summary>
		/// void (*tagAdded)(void* ctxt, BNBinaryView* view, BNTagReference* tagRef)
		/// </summary>
		internal IntPtr tagAdded;

		/// <summary>
		/// void (*tagRemoved)(void* ctxt, BNBinaryView* view, BNTagReference* tagRef)
		/// </summary>
		internal IntPtr tagRemoved;

		/// <summary>
		/// void (*tagUpdated)(void* ctxt, BNBinaryView* view, BNTagReference* tagRef)
		/// </summary>
		internal IntPtr tagUpdated;

		/// <summary>
		/// void (*symbolAdded)(void* ctxt, BNBinaryView* view, BNSymbol* sym)
		/// </summary>
		internal IntPtr symbolAdded;

		/// <summary>
		/// void (*symbolRemoved)(void* ctxt, BNBinaryView* view, BNSymbol* sym)
		/// </summary>
		internal IntPtr symbolRemoved;

		/// <summary>
		/// void (*symbolUpdated)(void* ctxt, BNBinaryView* view, BNSymbol* sym)
		/// </summary>
		internal IntPtr symbolUpdated;

		/// <summary>
		/// void (*stringFound)(void* ctxt, BNBinaryView* view, BNStringType type, uint64_t offset, size_t len)
		/// </summary>
		internal IntPtr stringFound;

		/// <summary>
		/// void (*stringRemoved)(void* ctxt, BNBinaryView* view, BNStringType type, uint64_t offset, size_t len)
		/// </summary>
		internal IntPtr stringRemoved;

		/// <summary>
		/// void (*derivedStringFound)(void* ctxt, BNBinaryView* view, BNDerivedString* str)
		/// </summary>
		internal IntPtr derivedStringFound;

		/// <summary>
		/// void (*derivedStringRemoved)(void* ctxt, BNBinaryView* view, BNDerivedString* str)
		/// </summary>
		internal IntPtr derivedStringRemoved;

		/// <summary>
		/// void (*typeDefined)(void* ctxt, BNBinaryView* view, BNQualifiedName* name, BNType* type)
		/// </summary>
		internal IntPtr typeDefined;

		/// <summary>
		/// void (*typeUndefined)(void* ctxt, BNBinaryView* view, BNQualifiedName* name, BNType* type)
		/// </summary>
		internal IntPtr typeUndefined;

		/// <summary>
		/// void (*typeReferenceChanged)(void* ctxt, BNBinaryView* view, BNQualifiedName* name, BNType* type)
		/// </summary>
		internal IntPtr typeReferenceChanged;

		/// <summary>
		/// void (*typeFieldReferenceChanged)(void* ctxt, BNBinaryView* view, BNQualifiedName* name, uint64_t offset)
		/// </summary>
		internal IntPtr typeFieldReferenceChanged;

		/// <summary>
		/// void (*segmentAdded)(void* ctxt, BNBinaryView* view, BNSegment* segment)
		/// </summary>
		internal IntPtr segmentAdded;

		/// <summary>
		/// void (*segmentRemoved)(void* ctxt, BNBinaryView* view, BNSegment* segment)
		/// </summary>
		internal IntPtr segmentRemoved;

		/// <summary>
		/// void (*segmentUpdated)(void* ctxt, BNBinaryView* view, BNSegment* segment)
		/// </summary>
		internal IntPtr segmentUpdated;

		/// <summary>
		/// void (*sectionAdded)(void* ctxt, BNBinaryView* view, BNSection* section)
		/// </summary>
		internal IntPtr sectionAdded;

		/// <summary>
		/// void (*sectionRemoved)(void* ctxt, BNBinaryView* view, BNSection* section)
		/// </summary>
		internal IntPtr sectionRemoved;

		/// <summary>
		/// void (*sectionUpdated)(void* ctxt, BNBinaryView* view, BNSection* section)
		/// </summary>
		internal IntPtr sectionUpdated;

		/// <summary>
		/// void (*componentNameUpdated)(void* ctxt, BNBinaryView* view, char* previousName, BNComponent* component)
		/// </summary>
		internal IntPtr componentNameUpdated;

		/// <summary>
		/// void (*componentAdded)(void*ctxt, BNBinaryView* view, BNComponent* component)
		/// </summary>
		internal IntPtr componentAdded;

		/// <summary>
		/// void (*componentMoved)(void*ctxt, BNBinaryView* view, BNComponent* formerParent, BNComponent* newParent, BNComponent* component)
		/// </summary>
		internal IntPtr componentMoved;

		/// <summary>
		/// void (*componentRemoved)(void*ctxt, BNBinaryView* view, BNComponent* formerParent, BNComponent* component)
		/// </summary>
		internal IntPtr componentRemoved;

		/// <summary>
		/// void (*componentFunctionAdded)(void*ctxt, BNBinaryView* view, BNComponent* component, BNFunction* function)
		/// </summary>
		internal IntPtr componentFunctionAdded;

		/// <summary>
		/// void (*componentFunctionRemoved)(void*ctxt, BNBinaryView* view, BNComponent* component, BNFunction* function)
		/// </summary>
		internal IntPtr componentFunctionRemoved;

		/// <summary>
		/// void (*componentDataVariableAdded)(void*ctxt, BNBinaryView* view, BNComponent* component, BNDataVariable* var)
		/// </summary>
		internal IntPtr componentDataVariableAdded;

		/// <summary>
		/// void (*componentDataVariableRemoved)(void*ctxt, BNBinaryView* view, BNComponent* component, BNDataVariable* var)
		/// </summary>
		internal IntPtr componentDataVariableRemoved;

		/// <summary>
		/// void (*externalLibraryAdded)(void* ctxt, BNBinaryView* data, BNExternalLibrary* library)
		/// </summary>
		internal IntPtr externalLibraryAdded;

		/// <summary>
		/// void (*externalLibraryUpdated)(void* ctxt, BNBinaryView* data, BNExternalLibrary* library)
		/// </summary>
		internal IntPtr externalLibraryUpdated;

		/// <summary>
		/// void (*externalLibraryRemoved)(void* ctxt, BNBinaryView* data, BNExternalLibrary* library)
		/// </summary>
		internal IntPtr externalLibraryRemoved;

		/// <summary>
		/// void (*externalLocationAdded)(void* ctxt, BNBinaryView* data, BNExternalLocation* location)
		/// </summary>
		internal IntPtr externalLocationAdded;

		/// <summary>
		/// void (*externalLocationUpdated)(void* ctxt, BNBinaryView* data, BNExternalLocation* location)
		/// </summary>
		internal IntPtr externalLocationUpdated;

		/// <summary>
		/// void (*externalLocationRemoved)(void* ctxt, BNBinaryView* data, BNExternalLocation* location)
		/// </summary>
		internal IntPtr externalLocationRemoved;

		/// <summary>
		/// void (*typeArchiveAttached)(void* ctxt, BNBinaryView* view, const char* id, const char* path)
		/// </summary>
		internal IntPtr typeArchiveAttached;

		/// <summary>
		/// void (*typeArchiveDetached)(void* ctxt, BNBinaryView* view, const char* id, const char* path)
		/// </summary>
		internal IntPtr typeArchiveDetached;

		/// <summary>
		/// void (*typeArchiveConnected)(void* ctxt, BNBinaryView* view, BNTypeArchive* archive)
		/// </summary>
		internal IntPtr typeArchiveConnected;

		/// <summary>
		/// void (*typeArchiveDisconnected)(void* ctxt, BNBinaryView* view, BNTypeArchive* archive)
		/// </summary>
		internal IntPtr typeArchiveDisconnected;

		/// <summary>
		/// void (*undoEntryAdded)(void* ctxt, BNBinaryView* view, BNUndoEntry* entry)
		/// </summary>
		internal IntPtr undoEntryAdded;

		/// <summary>
		/// void (*undoEntryTaken)(void* ctxt, BNBinaryView* view, BNUndoEntry* entry)
		/// </summary>
		internal IntPtr undoEntryTaken;

		/// <summary>
		/// void (*redoEntryTaken)(void* ctxt, BNBinaryView* view, BNUndoEntry* entry)
		/// </summary>
		internal IntPtr redoEntryTaken;

		/// <summary>
		/// void (*rebased)(void* ctxt, BNBinaryView* oldView, BNBinaryView* newView)
		/// </summary>
		internal IntPtr rebased;
	}

    /// <summary>
    /// Base class for receiving event notifications from a <see cref="BinaryView"/>, mirroring Python
    /// <c>BinaryDataNotification</c> (binaryview.py:247). Subclass and override the virtual methods for
    /// the events of interest, then register an instance via
    /// <see cref="BinaryView.RegisterDataNotification"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default every event is delivered. The callback context holds a global lock, so handlers must
    /// stay brief and avoid blocking calls (see the Python class docstring at binaryview.py:257).
    /// A throwing override does not abort dispatch -- the context swallows exceptions, mirroring Python
    /// which logs and continues.
    /// </para>
    /// <para>
    /// The six <c>OnExternalLibrary*</c>/<c>OnExternalLocation*</c> events have no Python equivalent;
    /// they expose the matching <c>BNBinaryDataNotification</c> struct slots directly for completeness.
    /// </para>
    /// </remarks>
    public abstract class BinaryDataNotification
    {
        /// <summary>
        /// Called by the core to pace batch processing, mirroring Python <c>notification_barrier</c>
        /// (binaryview.py:311). Return the milliseconds until the next barrier, or zero to quiesce.
        /// </summary>
        public virtual ulong OnNotificationBarrier(BinaryView view)
        {
            return 0;
        }

        /// <summary>Mirrors Python <c>data_written</c> (binaryview.py:314).</summary>
        public virtual void OnDataWritten(BinaryView view, ulong offset, ulong length)
        {
        }

        /// <summary>Mirrors Python <c>data_inserted</c> (binaryview.py:317).</summary>
        public virtual void OnDataInserted(BinaryView view, ulong offset, ulong length)
        {
        }

        /// <summary>Mirrors Python <c>data_removed</c> (binaryview.py:320).</summary>
        public virtual void OnDataRemoved(BinaryView view, ulong offset, ulong length)
        {
        }

        /// <summary>Mirrors Python <c>function_added</c> (binaryview.py:323).</summary>
        public virtual void OnFunctionAdded(BinaryView view, Function? function)
        {
        }

        /// <summary>Mirrors Python <c>function_removed</c> (binaryview.py:329).</summary>
        public virtual void OnFunctionRemoved(BinaryView view, Function? function)
        {
        }

        /// <summary>Mirrors Python <c>function_updated</c> (binaryview.py:335).</summary>
        public virtual void OnFunctionUpdated(BinaryView view, Function? function)
        {
        }

        /// <summary>Mirrors Python <c>function_update_requested</c> (binaryview.py:338).</summary>
        public virtual void OnFunctionUpdateRequested(BinaryView view, Function? function)
        {
        }

        /// <summary>Mirrors Python <c>data_var_added</c> (binaryview.py:341).</summary>
        public virtual void OnDataVariableAdded(BinaryView view, DataVariable dataVariable)
        {
        }

        /// <summary>Mirrors Python <c>data_var_removed</c> (binaryview.py:347).</summary>
        public virtual void OnDataVariableRemoved(BinaryView view, DataVariable dataVariable)
        {
        }

        /// <summary>Mirrors Python <c>data_var_updated</c> (binaryview.py:353).</summary>
        public virtual void OnDataVariableUpdated(BinaryView view, DataVariable dataVariable)
        {
        }

        /// <summary>Mirrors Python <c>data_metadata_updated</c> (binaryview.py:356).</summary>
        public virtual void OnDataMetadataUpdated(BinaryView view, ulong offset)
        {
        }

        /// <summary>Mirrors Python <c>tag_type_updated</c> (binaryview.py:359).</summary>
        public virtual void OnTagTypeUpdated(BinaryView view, TagType? tagType)
        {
        }

        /// <summary>Mirrors Python <c>tag_added</c> (binaryview.py:362).</summary>
        public virtual void OnTagAdded(
            BinaryView view, Tag? tag, TagReferenceType refType, bool autoDefined,
            Architecture? arch, Function? function, ulong address)
        {
        }

        /// <summary>Mirrors Python <c>tag_updated</c> (binaryview.py:368).</summary>
        public virtual void OnTagUpdated(
            BinaryView view, Tag? tag, TagReferenceType refType, bool autoDefined,
            Architecture? arch, Function? function, ulong address)
        {
        }

        /// <summary>Mirrors Python <c>tag_removed</c> (binaryview.py:374).</summary>
        public virtual void OnTagRemoved(
            BinaryView view, Tag? tag, TagReferenceType refType, bool autoDefined,
            Architecture? arch, Function? function, ulong address)
        {
        }

        /// <summary>Mirrors Python <c>symbol_added</c> (binaryview.py:380).</summary>
        public virtual void OnSymbolAdded(BinaryView view, Symbol? symbol)
        {
        }

        /// <summary>Mirrors Python <c>symbol_updated</c> (binaryview.py:383).</summary>
        public virtual void OnSymbolUpdated(BinaryView view, Symbol? symbol)
        {
        }

        /// <summary>Mirrors Python <c>symbol_removed</c> (binaryview.py:386).</summary>
        public virtual void OnSymbolRemoved(BinaryView view, Symbol? symbol)
        {
        }

        /// <summary>Mirrors Python <c>string_found</c> (binaryview.py:389).</summary>
        public virtual void OnStringFound(BinaryView view, StringType type, ulong offset, ulong length)
        {
        }

        /// <summary>Mirrors Python <c>string_removed</c> (binaryview.py:392).</summary>
        public virtual void OnStringRemoved(BinaryView view, StringType type, ulong offset, ulong length)
        {
        }

        /// <summary>Mirrors Python <c>derived_string_found</c> (binaryview.py:395).</summary>
        public virtual void OnDerivedStringFound(BinaryView view, DerivedString derivedString)
        {
        }

        /// <summary>Mirrors Python <c>derived_string_removed</c> (binaryview.py:398).</summary>
        public virtual void OnDerivedStringRemoved(BinaryView view, DerivedString derivedString)
        {
        }

        /// <summary>Mirrors Python <c>type_defined</c> (binaryview.py:401).</summary>
        public virtual void OnTypeDefined(BinaryView view, QualifiedName name, Type? type)
        {
        }

        /// <summary>Mirrors Python <c>type_undefined</c> (binaryview.py:404).</summary>
        public virtual void OnTypeUndefined(BinaryView view, QualifiedName name, Type? type)
        {
        }

        /// <summary>Mirrors Python <c>type_ref_changed</c> (binaryview.py:407).</summary>
        public virtual void OnTypeReferenceChanged(BinaryView view, QualifiedName name, Type? type)
        {
        }

        /// <summary>Mirrors Python <c>type_field_ref_changed</c> (binaryview.py:410).</summary>
        public virtual void OnTypeFieldReferenceChanged(BinaryView view, QualifiedName name, ulong offset)
        {
        }

        /// <summary>Mirrors Python <c>segment_added</c> (binaryview.py:413).</summary>
        public virtual void OnSegmentAdded(BinaryView view, Segment? segment)
        {
        }

        /// <summary>Mirrors Python <c>segment_updated</c> (binaryview.py:416).</summary>
        public virtual void OnSegmentUpdated(BinaryView view, Segment? segment)
        {
        }

        /// <summary>Mirrors Python <c>segment_removed</c> (binaryview.py:419).</summary>
        public virtual void OnSegmentRemoved(BinaryView view, Segment? segment)
        {
        }

        /// <summary>Mirrors Python <c>section_added</c> (binaryview.py:422).</summary>
        public virtual void OnSectionAdded(BinaryView view, Section? section)
        {
        }

        /// <summary>Mirrors Python <c>section_updated</c> (binaryview.py:425).</summary>
        public virtual void OnSectionUpdated(BinaryView view, Section? section)
        {
        }

        /// <summary>Mirrors Python <c>section_removed</c> (binaryview.py:428).</summary>
        public virtual void OnSectionRemoved(BinaryView view, Section? section)
        {
        }

        /// <summary>Mirrors Python <c>component_name_updated</c> (binaryview.py:438).</summary>
        public virtual void OnComponentNameUpdated(BinaryView view, string previousName, Component? component)
        {
        }

        /// <summary>Mirrors Python <c>component_added</c> (binaryview.py:431).</summary>
        public virtual void OnComponentAdded(BinaryView view, Component? component)
        {
        }

        /// <summary>Mirrors Python <c>component_moved</c> (binaryview.py:441).</summary>
        public virtual void OnComponentMoved(
            BinaryView view, Component? formerParent, Component? newParent, Component? component)
        {
        }

        /// <summary>Mirrors Python <c>component_removed</c> (binaryview.py:434).</summary>
        public virtual void OnComponentRemoved(BinaryView view, Component? formerParent, Component? component)
        {
        }

        /// <summary>Mirrors Python <c>component_function_added</c> (binaryview.py:445).</summary>
        public virtual void OnComponentFunctionAdded(BinaryView view, Component? component, Function? function)
        {
        }

        /// <summary>Mirrors Python <c>component_function_removed</c> (binaryview.py:448).</summary>
        public virtual void OnComponentFunctionRemoved(BinaryView view, Component? component, Function? function)
        {
        }

        /// <summary>Mirrors Python <c>component_data_var_added</c> (binaryview.py:452).</summary>
        public virtual void OnComponentDataVariableAdded(BinaryView view, Component? component, DataVariable dataVariable)
        {
        }

        /// <summary>Mirrors Python <c>component_data_var_removed</c> (binaryview.py:455).</summary>
        public virtual void OnComponentDataVariableRemoved(BinaryView view, Component? component, DataVariable dataVariable)
        {
        }

        /// <summary>
        /// No Python equivalent; exposes the <c>externalLibraryAdded</c> struct slot directly.
        /// </summary>
        public virtual void OnExternalLibraryAdded(BinaryView view, ExternalLibrary? library)
        {
        }

        /// <summary>
        /// No Python equivalent; exposes the <c>externalLibraryUpdated</c> struct slot directly.
        /// </summary>
        public virtual void OnExternalLibraryUpdated(BinaryView view, ExternalLibrary? library)
        {
        }

        /// <summary>
        /// No Python equivalent; exposes the <c>externalLibraryRemoved</c> struct slot directly.
        /// </summary>
        public virtual void OnExternalLibraryRemoved(BinaryView view, ExternalLibrary? library)
        {
        }

        /// <summary>
        /// No Python equivalent; exposes the <c>externalLocationAdded</c> struct slot directly.
        /// </summary>
        public virtual void OnExternalLocationAdded(BinaryView view, ExternalLocation? location)
        {
        }

        /// <summary>
        /// No Python equivalent; exposes the <c>externalLocationUpdated</c> struct slot directly.
        /// </summary>
        public virtual void OnExternalLocationUpdated(BinaryView view, ExternalLocation? location)
        {
        }

        /// <summary>
        /// No Python equivalent; exposes the <c>externalLocationRemoved</c> struct slot directly.
        /// </summary>
        public virtual void OnExternalLocationRemoved(BinaryView view, ExternalLocation? location)
        {
        }

        /// <summary>Mirrors Python <c>type_archive_attached</c> (binaryview.py:458).</summary>
        public virtual void OnTypeArchiveAttached(BinaryView view, string id, string path)
        {
        }

        /// <summary>Mirrors Python <c>type_archive_detached</c> (binaryview.py:461).</summary>
        public virtual void OnTypeArchiveDetached(BinaryView view, string id, string path)
        {
        }

        /// <summary>Mirrors Python <c>type_archive_connected</c> (binaryview.py:464).</summary>
        public virtual void OnTypeArchiveConnected(BinaryView view, TypeArchive? archive)
        {
        }

        /// <summary>Mirrors Python <c>type_archive_disconnected</c> (binaryview.py:467).</summary>
        public virtual void OnTypeArchiveDisconnected(BinaryView view, TypeArchive? archive)
        {
        }

        /// <summary>Mirrors Python <c>undo_entry_added</c> (binaryview.py:470).</summary>
        public virtual void OnUndoEntryAdded(BinaryView view, UndoEntry? entry)
        {
        }

        /// <summary>Mirrors Python <c>undo_entry_taken</c> (binaryview.py:473).</summary>
        public virtual void OnUndoEntryTaken(BinaryView view, UndoEntry? entry)
        {
        }

        /// <summary>Mirrors Python <c>redo_entry_taken</c> (binaryview.py:476).</summary>
        public virtual void OnRedoEntryTaken(BinaryView view, UndoEntry? entry)
        {
        }

        /// <summary>Mirrors Python <c>rebased</c> (binaryview.py:479).</summary>
        public virtual void OnRebased(BinaryView oldView, BinaryView newView)
        {
        }
    }
}