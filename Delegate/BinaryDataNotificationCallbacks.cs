using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		// uint64_t (*notificationBarrier)(void* ctxt, BNBinaryView* view)
		public delegate ulong BinaryDataNotificationBarrierDelegate(IntPtr context, IntPtr view);

		// void (*dataWritten)(void* ctxt, BNBinaryView* view, uint64_t offset, size_t len)
		public delegate void BinaryDataViewOffsetLengthDelegate(
			IntPtr context, IntPtr view, ulong offset, ulong length);

		// void (*dataMetadataUpdated)(void* ctxt, BNBinaryView* view, uint64_t offset)
		public delegate void BinaryDataViewOffsetDelegate(IntPtr context, IntPtr view, ulong offset);

		// Generic (view, single pointer) shape. The pointer is a BNObject* for handle-bearing
		// callbacks (function/symbol/segment/section/tag-type/component/archive/undo/external*) and
		// a borrowed struct pointer for by-value callbacks (BNDataVariable*/BNTagReference*/
		// BNDerivedString*); each trampoline interprets it for its slot.
		public delegate void BinaryDataViewOnePointerDelegate(IntPtr context, IntPtr view, IntPtr pointer);

		// void (*stringFound)(void* ctxt, BNBinaryView* view, BNStringType type, uint64_t offset, size_t len)
		// BNStringType is a 4-byte C enum, so it marshals as int and is cast to StringType in the trampoline.
		public delegate void BinaryDataStringFoundDelegate(
			IntPtr context, IntPtr view, int type, ulong offset, ulong length);

		// Generic (view, two pointers) shape. Covers type-defined/undefined/ref-changed (name+type),
		// component-name-updated (previousName+component), component-removed (formerParent+component),
		// component-function/component-data-var (component+handle), and type-archive-attached/detached
		// (id+path). Each trampoline interprets the two pointers for its slot.
		public delegate void BinaryDataViewTwoPointerDelegate(
			IntPtr context, IntPtr view, IntPtr first, IntPtr second);

		// void (*typeFieldReferenceChanged)(void* ctxt, BNBinaryView* view, BNQualifiedName* name, uint64_t offset)
		public delegate void BinaryDataViewPointerOffsetDelegate(
			IntPtr context, IntPtr view, IntPtr pointer, ulong offset);

		// void (*componentMoved)(void* ctxt, BNBinaryView* view, BNComponent* formerParent, BNComponent* newParent, BNComponent* component)
		public delegate void BinaryDataViewThreePointerDelegate(
			IntPtr context, IntPtr view, IntPtr first, IntPtr second, IntPtr third);

		// void (*rebased)(void* ctxt, BNBinaryView* oldView, BNBinaryView* newView)
		public delegate void BinaryDataRebasedDelegate(IntPtr context, IntPtr oldView, IntPtr newView);
	}

	internal static partial class UnsafeUtils
	{
		// Adapts a BinaryDataNotification into the 55 native BNBinaryDataNotification callback slots. Each
		// trampoline marshals the native arguments into managed objects and forwards to the user's virtual
		// override. The core invokes the function pointers stored on the BNBinaryDataNotification struct;
		// this context is rooted by those delegates (see BinaryDataNotificationContext.BuildNative), so it
		// -- and the notification it holds -- stay alive for the registration lifetime.
		//
		// Mirrors TypeArchiveNotificationContext (PR #39) but at 55 slots; the delegate TYPES are shared by
		// signature shape (9 types), while each slot gets its own rooted delegate INSTANCE bound to a
		// distinct trampoline.
		internal sealed class BinaryDataNotificationContext
		{
			internal readonly BinaryDataNotification Notification;

			// The allocated BNBinaryDataNotification struct (IntPtr to AllocHGlobal memory), kept alive
			// until FreeNative so the core-held registration never sees freed memory.
			internal IntPtr NativeStruct = IntPtr.Zero;

			// Rooting the 55 wrapper delegates on this context prevents the GC from reclaiming their
			// function pointers while the core holds the registration. Grouped by delegate signature.
			private NativeDelegates.BinaryDataNotificationBarrierDelegate? m_notificationBarrier;

			private NativeDelegates.BinaryDataViewOffsetLengthDelegate? m_dataWritten;
			private NativeDelegates.BinaryDataViewOffsetLengthDelegate? m_dataInserted;
			private NativeDelegates.BinaryDataViewOffsetLengthDelegate? m_dataRemoved;

			private NativeDelegates.BinaryDataViewOffsetDelegate? m_dataMetadataUpdated;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_functionAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_functionRemoved;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_functionUpdated;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_functionUpdateRequested;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_dataVariableAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_dataVariableRemoved;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_dataVariableUpdated;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_tagTypeUpdated;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_tagAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_tagRemoved;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_tagUpdated;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_symbolAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_symbolRemoved;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_symbolUpdated;

			private NativeDelegates.BinaryDataStringFoundDelegate? m_stringFound;
			private NativeDelegates.BinaryDataStringFoundDelegate? m_stringRemoved;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_derivedStringFound;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_derivedStringRemoved;

			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_typeDefined;
			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_typeUndefined;
			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_typeReferenceChanged;

			private NativeDelegates.BinaryDataViewPointerOffsetDelegate? m_typeFieldReferenceChanged;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_segmentAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_segmentRemoved;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_segmentUpdated;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_sectionAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_sectionRemoved;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_sectionUpdated;

			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_componentNameUpdated;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_componentAdded;
			private NativeDelegates.BinaryDataViewThreePointerDelegate? m_componentMoved;
			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_componentRemoved;
			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_componentFunctionAdded;
			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_componentFunctionRemoved;
			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_componentDataVariableAdded;
			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_componentDataVariableRemoved;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_externalLibraryAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_externalLibraryUpdated;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_externalLibraryRemoved;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_externalLocationAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_externalLocationUpdated;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_externalLocationRemoved;

			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_typeArchiveAttached;
			private NativeDelegates.BinaryDataViewTwoPointerDelegate? m_typeArchiveDetached;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_typeArchiveConnected;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_typeArchiveDisconnected;

			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_undoEntryAdded;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_undoEntryTaken;
			private NativeDelegates.BinaryDataViewOnePointerDelegate? m_redoEntryTaken;

			private NativeDelegates.BinaryDataRebasedDelegate? m_rebased;

			// The GCHandle stored as the struct's context field; freed on unregistration. The delegates
			// already root this context, but the handle makes the rooting explicit and gives the core a
			// stable context value to pass back.
			private GCHandle m_selfHandle;

			internal BinaryDataNotificationContext(BinaryDataNotification notification)
			{
				this.Notification = notification;
			}

			// ---------------------------------------------------------------------------------------------
			// Trampolines. Each borrows the view, marshals the payload, and forwards to the user override.
			// A throwing override must not abort the core's notification dispatch, so exceptions are
			// swallowed (Python logs and continues).
			// ---------------------------------------------------------------------------------------------

			internal ulong OnNotificationBarrier(IntPtr context, IntPtr viewHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return 0;
					}

					return this.Notification.OnNotificationBarrier(view);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
					return 0;
				}
			}

			internal void OnDataWritten(IntPtr context, IntPtr viewHandle, ulong offset, ulong length)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					this.Notification.OnDataWritten(view, offset, length);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnDataInserted(IntPtr context, IntPtr viewHandle, ulong offset, ulong length)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					this.Notification.OnDataInserted(view, offset, length);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnDataRemoved(IntPtr context, IntPtr viewHandle, ulong offset, ulong length)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					this.Notification.OnDataRemoved(view, offset, length);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnFunctionAdded(IntPtr context, IntPtr viewHandle, IntPtr functionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Function? function = Function.NewFromHandle(functionHandle);

					this.Notification.OnFunctionAdded(view, function);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnFunctionRemoved(IntPtr context, IntPtr viewHandle, IntPtr functionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Function? function = Function.NewFromHandle(functionHandle);

					this.Notification.OnFunctionRemoved(view, function);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnFunctionUpdated(IntPtr context, IntPtr viewHandle, IntPtr functionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Function? function = Function.NewFromHandle(functionHandle);

					this.Notification.OnFunctionUpdated(view, function);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnFunctionUpdateRequested(IntPtr context, IntPtr viewHandle, IntPtr functionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Function? function = Function.NewFromHandle(functionHandle);

					this.Notification.OnFunctionUpdateRequested(view, function);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnDataVariableAdded(IntPtr context, IntPtr viewHandle, IntPtr variablePointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					DataVariable dataVariable = DataVariableFromPtr(variablePointer, view);

					this.Notification.OnDataVariableAdded(view, dataVariable);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnDataVariableRemoved(IntPtr context, IntPtr viewHandle, IntPtr variablePointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					DataVariable dataVariable = DataVariableFromPtr(variablePointer, view);

					this.Notification.OnDataVariableRemoved(view, dataVariable);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnDataVariableUpdated(IntPtr context, IntPtr viewHandle, IntPtr variablePointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					DataVariable dataVariable = DataVariableFromPtr(variablePointer, view);

					this.Notification.OnDataVariableUpdated(view, dataVariable);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnDataMetadataUpdated(IntPtr context, IntPtr viewHandle, ulong offset)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					this.Notification.OnDataMetadataUpdated(view, offset);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTagTypeUpdated(IntPtr context, IntPtr viewHandle, IntPtr tagTypeHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					TagType? tagType = TagType.NewFromHandle(tagTypeHandle);

					this.Notification.OnTagTypeUpdated(view, tagType);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTagAdded(IntPtr context, IntPtr viewHandle, IntPtr tagReferencePointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					TagReference tagReference = TagReferenceFromPtr(tagReferencePointer);

					this.Notification.OnTagAdded(
						view,
						tagReference.Tag,
						tagReference.RefType,
						tagReference.AutoDefined,
						tagReference.Architecture,
						tagReference.Function,
						tagReference.Address);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTagRemoved(IntPtr context, IntPtr viewHandle, IntPtr tagReferencePointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					TagReference tagReference = TagReferenceFromPtr(tagReferencePointer);

					this.Notification.OnTagRemoved(
						view,
						tagReference.Tag,
						tagReference.RefType,
						tagReference.AutoDefined,
						tagReference.Architecture,
						tagReference.Function,
						tagReference.Address);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTagUpdated(IntPtr context, IntPtr viewHandle, IntPtr tagReferencePointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					TagReference tagReference = TagReferenceFromPtr(tagReferencePointer);

					this.Notification.OnTagUpdated(
						view,
						tagReference.Tag,
						tagReference.RefType,
						tagReference.AutoDefined,
						tagReference.Architecture,
						tagReference.Function,
						tagReference.Address);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSymbolAdded(IntPtr context, IntPtr viewHandle, IntPtr symbolHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Symbol? symbol = Symbol.NewFromHandle(symbolHandle);

					this.Notification.OnSymbolAdded(view, symbol);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSymbolRemoved(IntPtr context, IntPtr viewHandle, IntPtr symbolHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Symbol? symbol = Symbol.NewFromHandle(symbolHandle);

					this.Notification.OnSymbolRemoved(view, symbol);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSymbolUpdated(IntPtr context, IntPtr viewHandle, IntPtr symbolHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Symbol? symbol = Symbol.NewFromHandle(symbolHandle);

					this.Notification.OnSymbolUpdated(view, symbol);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnStringFound(IntPtr context, IntPtr viewHandle, int type, ulong offset, ulong length)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					this.Notification.OnStringFound(view, (StringType)type, offset, length);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnStringRemoved(IntPtr context, IntPtr viewHandle, int type, ulong offset, ulong length)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					this.Notification.OnStringRemoved(view, (StringType)type, offset, length);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnDerivedStringFound(IntPtr context, IntPtr viewHandle, IntPtr derivedStringPointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					DerivedString derivedString = DerivedStringFromPtr(derivedStringPointer);

					this.Notification.OnDerivedStringFound(view, derivedString);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnDerivedStringRemoved(IntPtr context, IntPtr viewHandle, IntPtr derivedStringPointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					DerivedString derivedString = DerivedStringFromPtr(derivedStringPointer);

					this.Notification.OnDerivedStringRemoved(view, derivedString);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeDefined(IntPtr context, IntPtr viewHandle, IntPtr namePointer, IntPtr typeHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					QualifiedName name = QualifiedNameFromPtr(namePointer);
					Type? type = Type.NewFromHandle(typeHandle);

					this.Notification.OnTypeDefined(view, name, type);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeUndefined(IntPtr context, IntPtr viewHandle, IntPtr namePointer, IntPtr typeHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					QualifiedName name = QualifiedNameFromPtr(namePointer);
					Type? type = Type.NewFromHandle(typeHandle);

					this.Notification.OnTypeUndefined(view, name, type);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeReferenceChanged(IntPtr context, IntPtr viewHandle, IntPtr namePointer, IntPtr typeHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					QualifiedName name = QualifiedNameFromPtr(namePointer);
					Type? type = Type.NewFromHandle(typeHandle);

					this.Notification.OnTypeReferenceChanged(view, name, type);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeFieldReferenceChanged(IntPtr context, IntPtr viewHandle, IntPtr namePointer, ulong offset)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					QualifiedName name = QualifiedNameFromPtr(namePointer);

					this.Notification.OnTypeFieldReferenceChanged(view, name, offset);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSegmentAdded(IntPtr context, IntPtr viewHandle, IntPtr segmentHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Segment? segment = Segment.NewFromHandle(segmentHandle);

					this.Notification.OnSegmentAdded(view, segment);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSegmentRemoved(IntPtr context, IntPtr viewHandle, IntPtr segmentHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Segment? segment = Segment.NewFromHandle(segmentHandle);

					this.Notification.OnSegmentRemoved(view, segment);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSegmentUpdated(IntPtr context, IntPtr viewHandle, IntPtr segmentHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Segment? segment = Segment.NewFromHandle(segmentHandle);

					this.Notification.OnSegmentUpdated(view, segment);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSectionAdded(IntPtr context, IntPtr viewHandle, IntPtr sectionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Section? section = Section.NewFromHandle(sectionHandle);

					this.Notification.OnSectionAdded(view, section);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSectionRemoved(IntPtr context, IntPtr viewHandle, IntPtr sectionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Section? section = Section.NewFromHandle(sectionHandle);

					this.Notification.OnSectionRemoved(view, section);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnSectionUpdated(IntPtr context, IntPtr viewHandle, IntPtr sectionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Section? section = Section.NewFromHandle(sectionHandle);

					this.Notification.OnSectionUpdated(view, section);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnComponentNameUpdated(
				IntPtr context, IntPtr viewHandle, IntPtr previousNamePointer, IntPtr componentHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					string previousName = PtrToAnsiString(previousNamePointer);
					Component? component = Component.NewFromHandle(componentHandle);

					this.Notification.OnComponentNameUpdated(view, previousName, component);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnComponentAdded(IntPtr context, IntPtr viewHandle, IntPtr componentHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Component? component = Component.NewFromHandle(componentHandle);

					this.Notification.OnComponentAdded(view, component);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnComponentMoved(
				IntPtr context, IntPtr viewHandle,
				IntPtr formerParentHandle, IntPtr newParentHandle, IntPtr componentHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Component? formerParent = Component.NewFromHandle(formerParentHandle);
					Component? newParent = Component.NewFromHandle(newParentHandle);
					Component? component = Component.NewFromHandle(componentHandle);

					this.Notification.OnComponentMoved(view, formerParent, newParent, component);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnComponentRemoved(
				IntPtr context, IntPtr viewHandle, IntPtr formerParentHandle, IntPtr componentHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Component? formerParent = Component.NewFromHandle(formerParentHandle);
					Component? component = Component.NewFromHandle(componentHandle);

					this.Notification.OnComponentRemoved(view, formerParent, component);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnComponentFunctionAdded(
				IntPtr context, IntPtr viewHandle, IntPtr componentHandle, IntPtr functionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Component? component = Component.NewFromHandle(componentHandle);
					Function? function = Function.NewFromHandle(functionHandle);

					this.Notification.OnComponentFunctionAdded(view, component, function);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnComponentFunctionRemoved(
				IntPtr context, IntPtr viewHandle, IntPtr componentHandle, IntPtr functionHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Component? component = Component.NewFromHandle(componentHandle);
					Function? function = Function.NewFromHandle(functionHandle);

					this.Notification.OnComponentFunctionRemoved(view, component, function);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnComponentDataVariableAdded(
				IntPtr context, IntPtr viewHandle, IntPtr componentHandle, IntPtr variablePointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Component? component = Component.NewFromHandle(componentHandle);
					DataVariable dataVariable = DataVariableFromPtr(variablePointer, view);

					this.Notification.OnComponentDataVariableAdded(view, component, dataVariable);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnComponentDataVariableRemoved(
				IntPtr context, IntPtr viewHandle, IntPtr componentHandle, IntPtr variablePointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					Component? component = Component.NewFromHandle(componentHandle);
					DataVariable dataVariable = DataVariableFromPtr(variablePointer, view);

					this.Notification.OnComponentDataVariableRemoved(view, component, dataVariable);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnExternalLibraryAdded(IntPtr context, IntPtr viewHandle, IntPtr libraryHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					ExternalLibrary? library = ExternalLibrary.NewFromHandle(libraryHandle);

					this.Notification.OnExternalLibraryAdded(view, library);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnExternalLibraryUpdated(IntPtr context, IntPtr viewHandle, IntPtr libraryHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					ExternalLibrary? library = ExternalLibrary.NewFromHandle(libraryHandle);

					this.Notification.OnExternalLibraryUpdated(view, library);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnExternalLibraryRemoved(IntPtr context, IntPtr viewHandle, IntPtr libraryHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					ExternalLibrary? library = ExternalLibrary.NewFromHandle(libraryHandle);

					this.Notification.OnExternalLibraryRemoved(view, library);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnExternalLocationAdded(IntPtr context, IntPtr viewHandle, IntPtr locationHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					ExternalLocation? location = ExternalLocation.NewFromHandle(locationHandle);

					this.Notification.OnExternalLocationAdded(view, location);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnExternalLocationUpdated(IntPtr context, IntPtr viewHandle, IntPtr locationHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					ExternalLocation? location = ExternalLocation.NewFromHandle(locationHandle);

					this.Notification.OnExternalLocationUpdated(view, location);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnExternalLocationRemoved(IntPtr context, IntPtr viewHandle, IntPtr locationHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					ExternalLocation? location = ExternalLocation.NewFromHandle(locationHandle);

					this.Notification.OnExternalLocationRemoved(view, location);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeArchiveAttached(
				IntPtr context, IntPtr viewHandle, IntPtr idPointer, IntPtr pathPointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					string id = PtrToAnsiString(idPointer);
					string path = PtrToAnsiString(pathPointer);

					this.Notification.OnTypeArchiveAttached(view, id, path);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeArchiveDetached(
				IntPtr context, IntPtr viewHandle, IntPtr idPointer, IntPtr pathPointer)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					string id = PtrToAnsiString(idPointer);
					string path = PtrToAnsiString(pathPointer);

					this.Notification.OnTypeArchiveDetached(view, id, path);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeArchiveConnected(IntPtr context, IntPtr viewHandle, IntPtr archiveHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					TypeArchive? archive = TypeArchive.BorrowHandle(archiveHandle);

					this.Notification.OnTypeArchiveConnected(view, archive);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeArchiveDisconnected(IntPtr context, IntPtr viewHandle, IntPtr archiveHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					TypeArchive? archive = TypeArchive.BorrowHandle(archiveHandle);

					this.Notification.OnTypeArchiveDisconnected(view, archive);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnUndoEntryAdded(IntPtr context, IntPtr viewHandle, IntPtr entryHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					UndoEntry? entry = UndoEntry.NewFromHandle(entryHandle);

					this.Notification.OnUndoEntryAdded(view, entry);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnUndoEntryTaken(IntPtr context, IntPtr viewHandle, IntPtr entryHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					UndoEntry? entry = UndoEntry.NewFromHandle(entryHandle);

					this.Notification.OnUndoEntryTaken(view, entry);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnRedoEntryTaken(IntPtr context, IntPtr viewHandle, IntPtr entryHandle)
			{
				try
				{
					BinaryView? view = BorrowView(viewHandle);

					if (null == view)
					{
						return;
					}

					UndoEntry? entry = UndoEntry.NewFromHandle(entryHandle);

					this.Notification.OnRedoEntryTaken(view, entry);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnRebased(IntPtr context, IntPtr oldViewHandle, IntPtr newViewHandle)
			{
				try
				{
					BinaryView? oldView = BorrowView(oldViewHandle);
					BinaryView? newView = BorrowView(newViewHandle);

					if (null == oldView || null == newView)
					{
						return;
					}

					this.Notification.OnRebased(oldView, newView);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			// ---------------------------------------------------------------------------------------------
			// Native struct build/free.
			// ---------------------------------------------------------------------------------------------

			// Allocates the BNBinaryDataNotification struct, sets its context and the 55 function pointers
			// (taken from the rooted wrapper delegates), and returns the struct pointer. Idempotent: a
			// second call returns the existing struct.
			internal IntPtr BuildNative()
			{
				if (IntPtr.Zero != this.NativeStruct)
				{
					return this.NativeStruct;
				}

				// 1. Create the 55 wrapper delegates bound to this context and root them as fields.
				this.m_notificationBarrier = new NativeDelegates.BinaryDataNotificationBarrierDelegate(this.OnNotificationBarrier);

				this.m_dataWritten = new NativeDelegates.BinaryDataViewOffsetLengthDelegate(this.OnDataWritten);
				this.m_dataInserted = new NativeDelegates.BinaryDataViewOffsetLengthDelegate(this.OnDataInserted);
				this.m_dataRemoved = new NativeDelegates.BinaryDataViewOffsetLengthDelegate(this.OnDataRemoved);

				this.m_dataMetadataUpdated = new NativeDelegates.BinaryDataViewOffsetDelegate(this.OnDataMetadataUpdated);

				this.m_functionAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnFunctionAdded);
				this.m_functionRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnFunctionRemoved);
				this.m_functionUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnFunctionUpdated);
				this.m_functionUpdateRequested = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnFunctionUpdateRequested);

				this.m_dataVariableAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnDataVariableAdded);
				this.m_dataVariableRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnDataVariableRemoved);
				this.m_dataVariableUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnDataVariableUpdated);

				this.m_tagTypeUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnTagTypeUpdated);

				this.m_tagAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnTagAdded);
				this.m_tagRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnTagRemoved);
				this.m_tagUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnTagUpdated);

				this.m_symbolAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSymbolAdded);
				this.m_symbolRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSymbolRemoved);
				this.m_symbolUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSymbolUpdated);

				this.m_stringFound = new NativeDelegates.BinaryDataStringFoundDelegate(this.OnStringFound);
				this.m_stringRemoved = new NativeDelegates.BinaryDataStringFoundDelegate(this.OnStringRemoved);

				this.m_derivedStringFound = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnDerivedStringFound);
				this.m_derivedStringRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnDerivedStringRemoved);

				this.m_typeDefined = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnTypeDefined);
				this.m_typeUndefined = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnTypeUndefined);
				this.m_typeReferenceChanged = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnTypeReferenceChanged);

				this.m_typeFieldReferenceChanged = new NativeDelegates.BinaryDataViewPointerOffsetDelegate(this.OnTypeFieldReferenceChanged);

				this.m_segmentAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSegmentAdded);
				this.m_segmentRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSegmentRemoved);
				this.m_segmentUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSegmentUpdated);

				this.m_sectionAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSectionAdded);
				this.m_sectionRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSectionRemoved);
				this.m_sectionUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnSectionUpdated);

				this.m_componentNameUpdated = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnComponentNameUpdated);
				this.m_componentAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnComponentAdded);
				this.m_componentMoved = new NativeDelegates.BinaryDataViewThreePointerDelegate(this.OnComponentMoved);
				this.m_componentRemoved = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnComponentRemoved);
				this.m_componentFunctionAdded = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnComponentFunctionAdded);
				this.m_componentFunctionRemoved = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnComponentFunctionRemoved);
				this.m_componentDataVariableAdded = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnComponentDataVariableAdded);
				this.m_componentDataVariableRemoved = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnComponentDataVariableRemoved);

				this.m_externalLibraryAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnExternalLibraryAdded);
				this.m_externalLibraryUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnExternalLibraryUpdated);
				this.m_externalLibraryRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnExternalLibraryRemoved);
				this.m_externalLocationAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnExternalLocationAdded);
				this.m_externalLocationUpdated = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnExternalLocationUpdated);
				this.m_externalLocationRemoved = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnExternalLocationRemoved);

				this.m_typeArchiveAttached = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnTypeArchiveAttached);
				this.m_typeArchiveDetached = new NativeDelegates.BinaryDataViewTwoPointerDelegate(this.OnTypeArchiveDetached);
				this.m_typeArchiveConnected = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnTypeArchiveConnected);
				this.m_typeArchiveDisconnected = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnTypeArchiveDisconnected);

				this.m_undoEntryAdded = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnUndoEntryAdded);
				this.m_undoEntryTaken = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnUndoEntryTaken);
				this.m_redoEntryTaken = new NativeDelegates.BinaryDataViewOnePointerDelegate(this.OnRedoEntryTaken);

				this.m_rebased = new NativeDelegates.BinaryDataRebasedDelegate(this.OnRebased);

				// 2. Allocate a stable context handle so the core has a value to pass back, and so this
				// context is explicitly rooted.
				this.m_selfHandle = GCHandle.Alloc(this, GCHandleType.Normal);

				// 3. Build the struct: context + 55 function pointers.
				BNBinaryDataNotification native = new BNBinaryDataNotification();
				native.context = GCHandle.ToIntPtr(this.m_selfHandle);
				native.notificationBarrier = FunctionPointer(this.m_notificationBarrier);
				native.dataWritten = FunctionPointer(this.m_dataWritten);
				native.dataInserted = FunctionPointer(this.m_dataInserted);
				native.dataRemoved = FunctionPointer(this.m_dataRemoved);
				native.functionAdded = FunctionPointer(this.m_functionAdded);
				native.functionRemoved = FunctionPointer(this.m_functionRemoved);
				native.functionUpdated = FunctionPointer(this.m_functionUpdated);
				native.functionUpdateRequested = FunctionPointer(this.m_functionUpdateRequested);
				native.dataVariableAdded = FunctionPointer(this.m_dataVariableAdded);
				native.dataVariableRemoved = FunctionPointer(this.m_dataVariableRemoved);
				native.dataVariableUpdated = FunctionPointer(this.m_dataVariableUpdated);
				native.dataMetadataUpdated = FunctionPointer(this.m_dataMetadataUpdated);
				native.tagTypeUpdated = FunctionPointer(this.m_tagTypeUpdated);
				native.tagAdded = FunctionPointer(this.m_tagAdded);
				native.tagRemoved = FunctionPointer(this.m_tagRemoved);
				native.tagUpdated = FunctionPointer(this.m_tagUpdated);
				native.symbolAdded = FunctionPointer(this.m_symbolAdded);
				native.symbolRemoved = FunctionPointer(this.m_symbolRemoved);
				native.symbolUpdated = FunctionPointer(this.m_symbolUpdated);
				native.stringFound = FunctionPointer(this.m_stringFound);
				native.stringRemoved = FunctionPointer(this.m_stringRemoved);
				native.derivedStringFound = FunctionPointer(this.m_derivedStringFound);
				native.derivedStringRemoved = FunctionPointer(this.m_derivedStringRemoved);
				native.typeDefined = FunctionPointer(this.m_typeDefined);
				native.typeUndefined = FunctionPointer(this.m_typeUndefined);
				native.typeReferenceChanged = FunctionPointer(this.m_typeReferenceChanged);
				native.typeFieldReferenceChanged = FunctionPointer(this.m_typeFieldReferenceChanged);
				native.segmentAdded = FunctionPointer(this.m_segmentAdded);
				native.segmentRemoved = FunctionPointer(this.m_segmentRemoved);
				native.segmentUpdated = FunctionPointer(this.m_segmentUpdated);
				native.sectionAdded = FunctionPointer(this.m_sectionAdded);
				native.sectionRemoved = FunctionPointer(this.m_sectionRemoved);
				native.sectionUpdated = FunctionPointer(this.m_sectionUpdated);
				native.componentNameUpdated = FunctionPointer(this.m_componentNameUpdated);
				native.componentAdded = FunctionPointer(this.m_componentAdded);
				native.componentMoved = FunctionPointer(this.m_componentMoved);
				native.componentRemoved = FunctionPointer(this.m_componentRemoved);
				native.componentFunctionAdded = FunctionPointer(this.m_componentFunctionAdded);
				native.componentFunctionRemoved = FunctionPointer(this.m_componentFunctionRemoved);
				native.componentDataVariableAdded = FunctionPointer(this.m_componentDataVariableAdded);
				native.componentDataVariableRemoved = FunctionPointer(this.m_componentDataVariableRemoved);
				native.externalLibraryAdded = FunctionPointer(this.m_externalLibraryAdded);
				native.externalLibraryUpdated = FunctionPointer(this.m_externalLibraryUpdated);
				native.externalLibraryRemoved = FunctionPointer(this.m_externalLibraryRemoved);
				native.externalLocationAdded = FunctionPointer(this.m_externalLocationAdded);
				native.externalLocationUpdated = FunctionPointer(this.m_externalLocationUpdated);
				native.externalLocationRemoved = FunctionPointer(this.m_externalLocationRemoved);
				native.typeArchiveAttached = FunctionPointer(this.m_typeArchiveAttached);
				native.typeArchiveDetached = FunctionPointer(this.m_typeArchiveDetached);
				native.typeArchiveConnected = FunctionPointer(this.m_typeArchiveConnected);
				native.typeArchiveDisconnected = FunctionPointer(this.m_typeArchiveDisconnected);
				native.undoEntryAdded = FunctionPointer(this.m_undoEntryAdded);
				native.undoEntryTaken = FunctionPointer(this.m_undoEntryTaken);
				native.redoEntryTaken = FunctionPointer(this.m_redoEntryTaken);
				native.rebased = FunctionPointer(this.m_rebased);

				this.NativeStruct = Marshal.AllocHGlobal(Marshal.SizeOf<BNBinaryDataNotification>());
				Marshal.StructureToPtr<BNBinaryDataNotification>(native, this.NativeStruct, false);

				return this.NativeStruct;
			}

			// Frees the struct and the context handle. Called after the core has unregistered.
			internal void FreeNative()
			{
				if (IntPtr.Zero != this.NativeStruct)
				{
					Marshal.DestroyStructure<BNBinaryDataNotification>(this.NativeStruct);
					Marshal.FreeHGlobal(this.NativeStruct);
					this.NativeStruct = IntPtr.Zero;
				}

				if (this.m_selfHandle.IsAllocated)
				{
					this.m_selfHandle.Free();
				}
			}

			// ---------------------------------------------------------------------------------------------
			// Marshaling helpers.
			// ---------------------------------------------------------------------------------------------

			// Generic over the concrete delegate type so the marshaler uses its statically-known shape
			// (the GetFunctionPointerForDelegate<T> overload), avoiding the AOT warning the untyped
			// overload carries. The TypeArchive context inlines the same call per delegate.
			private static IntPtr FunctionPointer<T>(T wrapper) where T : Delegate
			{
				return Marshal.GetFunctionPointerForDelegate<T>(wrapper);
			}

			private static BinaryView? BorrowView(IntPtr handle)
			{
				if (IntPtr.Zero == handle)
				{
					return null;
				}

				return BinaryView.BorrowHandle(handle);
			}

			private static string PtrToAnsiString(IntPtr pointer)
			{
				if (IntPtr.Zero == pointer)
				{
					return string.Empty;
				}

				return Marshal.PtrToStringAnsi(pointer) ?? string.Empty;
			}

			// Reads a BNQualifiedName out of a pointer the core passes (the view owns the name storage, so
			// it is read without freeing, matching Python's _from_core_struct).
			private static QualifiedName QualifiedNameFromPtr(IntPtr pointer)
			{
				if (IntPtr.Zero == pointer)
				{
					return new QualifiedName(string.Empty);
				}

				BNQualifiedName native = Marshal.PtrToStructure<BNQualifiedName>(pointer);

				return QualifiedName.FromNative(native);
			}

			// Reads a borrowed BNDataVariable* (the core owns the storage) and attaches the originating
			// view so the DataVariable's navigation members work, matching Python's view-bearing var.
			private static DataVariable DataVariableFromPtr(IntPtr pointer, BinaryView view)
			{
				BNDataVariable native = Marshal.PtrToStructure<BNDataVariable>(pointer);

				return DataVariable.FromNative(native, view);
			}

			// Reads a borrowed BNTagReference* (the core owns the storage) into a TagReference, whose
			// embedded handles are add-ref'd by FromNative so they outlive the borrowed pointer.
			private static TagReference TagReferenceFromPtr(IntPtr pointer)
			{
				BNTagReference native = Marshal.PtrToStructure<BNTagReference>(pointer);

				return TagReference.FromNative(native);
			}

			// Reads a borrowed BNDerivedString* (the core owns the storage). FromNative copies the embedded
			// string text eagerly, so the projection is safe after the core frees the native struct.
			private static DerivedString DerivedStringFromPtr(IntPtr pointer)
			{
				BNDerivedString native = Marshal.PtrToStructure<BNDerivedString>(pointer);

				return DerivedString.FromNative(native);
			}
		}
	}
}
