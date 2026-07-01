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

    public class BinaryDataNotification 
    {
		public BinaryDataNotification() 
		{
		    
		}
    }
}