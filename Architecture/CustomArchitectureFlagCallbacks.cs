using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate FlagRole GetFlagRoleCallback(
			IntPtr context,
			uint flag,
			uint semanticClass);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntPtr GetFlagsForConditionCallback(
			IntPtr context,
			LowLevelILFlagCondition condition,
			uint semanticClass,
			out ulong count);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntPtr GetIndexedListCallback(
			IntPtr context,
			uint index,
			out ulong count);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void FreeConditionListCallback(
			IntPtr context,
			IntPtr conditions,
			ulong count);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate uint GetSemanticClassCallback(IntPtr context, uint writeType);

		private void AddFlagCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.getFlagName = UnsafeUtils.PinCallback<GetRegisterNameCallback>(
				this.GetFlagNameAdapter);
			callbacks.getFlagWriteTypeName = UnsafeUtils.PinCallback<GetRegisterNameCallback>(
				this.GetFlagWriteTypeNameAdapter);
			callbacks.getSemanticFlagClassName =
				UnsafeUtils.PinCallback<GetRegisterNameCallback>(
					this.GetSemanticFlagClassNameAdapter);
			callbacks.getSemanticFlagGroupName =
				UnsafeUtils.PinCallback<GetRegisterNameCallback>(
					this.GetSemanticFlagGroupNameAdapter);
			callbacks.getAllFlags = UnsafeUtils.PinCallback<GetRegisterListCallback>(
				this.GetAllFlagsAdapter);
			callbacks.getAllFlagWriteTypes = UnsafeUtils.PinCallback<GetRegisterListCallback>(
				this.GetAllFlagWriteTypesAdapter);
			callbacks.getAllSemanticFlagClasses =
				UnsafeUtils.PinCallback<GetRegisterListCallback>(
					this.GetAllSemanticFlagClassesAdapter);
			callbacks.getAllSemanticFlagGroups =
				UnsafeUtils.PinCallback<GetRegisterListCallback>(
					this.GetAllSemanticFlagGroupsAdapter);
			callbacks.getFlagRole = UnsafeUtils.PinCallback<GetFlagRoleCallback>(
				this.GetFlagRoleAdapter);
			callbacks.getFlagsRequiredForFlagCondition =
				UnsafeUtils.PinCallback<GetFlagsForConditionCallback>(
					this.GetFlagsRequiredForFlagConditionAdapter);
			callbacks.getFlagsRequiredForSemanticFlagGroup =
				UnsafeUtils.PinCallback<GetIndexedListCallback>(
					this.GetFlagsRequiredForSemanticFlagGroupAdapter);
			callbacks.getFlagConditionsForSemanticFlagGroup =
				UnsafeUtils.PinCallback<GetIndexedListCallback>(
					this.GetFlagConditionsForSemanticFlagGroupAdapter);
			callbacks.freeFlagConditionsForSemanticFlagGroup =
				UnsafeUtils.PinCallback<FreeConditionListCallback>(
					this.FreeConditionListAdapter);
			callbacks.getFlagsWrittenByFlagWriteType =
				UnsafeUtils.PinCallback<GetIndexedListCallback>(
					this.GetFlagsWrittenByFlagWriteTypeAdapter);
			callbacks.getSemanticClassForFlagWriteType =
				UnsafeUtils.PinCallback<GetSemanticClassCallback>(
					this.GetSemanticClassForFlagWriteTypeAdapter);
		}

		private IntPtr GetFlagNameAdapter(IntPtr context, uint flag)
		{
			try
			{
				return NativeMethods.BNAllocString(this.getFlagName((FlagIndex)flag));
			}
			catch (Exception exception)
			{
				return this.AllocateEmptyFlagName("getFlagName", exception);
			}
		}

		private IntPtr GetFlagWriteTypeNameAdapter(IntPtr context, uint writeType)
		{
			try
			{
				return NativeMethods.BNAllocString(this.GetFlagWriteTypeName(writeType));
			}
			catch (Exception exception)
			{
				return this.AllocateEmptyFlagName("GetFlagWriteTypeName", exception);
			}
		}

		private IntPtr GetSemanticFlagClassNameAdapter(IntPtr context, uint semanticClass)
		{
			try
			{
				return NativeMethods.BNAllocString(
					this.GetSemanticFlagClassName(semanticClass));
			}
			catch (Exception exception)
			{
				return this.AllocateEmptyFlagName("GetSemanticFlagClassName", exception);
			}
		}

		private IntPtr GetSemanticFlagGroupNameAdapter(IntPtr context, uint semanticGroup)
		{
			try
			{
				return NativeMethods.BNAllocString(
					this.GetSemanticFlagGroupName(semanticGroup));
			}
			catch (Exception exception)
			{
				return this.AllocateEmptyFlagName("GetSemanticFlagGroupName", exception);
			}
		}

		private IntPtr AllocateEmptyFlagName(string callbackName, Exception exception)
		{
			Core.LogError(
				"Unhandled exception in CustomArchitecture.{0}: {1}",
				callbackName,
				exception);
			return NativeMethods.BNAllocString(string.Empty);
		}

		private IntPtr GetAllFlagsAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(this.GetAllFlags, "GetAllFlags", out count);
		}

		private IntPtr GetAllFlagWriteTypesAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetAllFlagWriteTypes,
				"GetAllFlagWriteTypes",
				out count);
		}

		private IntPtr GetAllSemanticFlagClassesAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetAllSemanticFlagClasses,
				"GetAllSemanticFlagClasses",
				out count);
		}

		private IntPtr GetAllSemanticFlagGroupsAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetAllSemanticFlagGroups,
				"GetAllSemanticFlagGroups",
				out count);
		}

		private FlagRole GetFlagRoleAdapter(
			IntPtr context,
			uint flag,
			uint semanticClass)
		{
			try
			{
				return this.GetFlagRole(flag, semanticClass);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in CustomArchitecture.GetFlagRole: {0}", exception);
				return FlagRole.SpecialFlagRole;
			}
		}

		private IntPtr GetFlagsRequiredForFlagConditionAdapter(
			IntPtr context,
			LowLevelILFlagCondition condition,
			uint semanticClass,
			out ulong count)
		{
			try
			{
				uint[] flags = this.GetFlagsRequiredForFlagCondition(condition, semanticClass);
				return this.AllocateFlagList(flags, out count);
			}
			catch (Exception exception)
			{
				return this.HandleFlagListException(
					"GetFlagsRequiredForFlagCondition",
					exception,
					out count);
			}
		}

		private IntPtr GetFlagsRequiredForSemanticFlagGroupAdapter(
			IntPtr context,
			uint semanticGroup,
			out ulong count)
		{
			try
			{
				return this.AllocateFlagList(
					this.GetFlagsRequiredForSemanticFlagGroup(semanticGroup),
					out count);
			}
			catch (Exception exception)
			{
				return this.HandleFlagListException(
					"GetFlagsRequiredForSemanticFlagGroup",
					exception,
					out count);
			}
		}

		private IntPtr GetFlagsWrittenByFlagWriteTypeAdapter(
			IntPtr context,
			uint writeType,
			out ulong count)
		{
			try
			{
				return this.AllocateFlagList(
					this.GetFlagsWrittenByFlagWriteType(writeType),
					out count);
			}
			catch (Exception exception)
			{
				return this.HandleFlagListException(
					"GetFlagsWrittenByFlagWriteType",
					exception,
					out count);
			}
		}

		private IntPtr AllocateFlagList(uint[] flags, out ulong count)
		{
			return this.AllocateRegisterList(flags ?? Array.Empty<uint>(), out count);
		}

		private IntPtr HandleFlagListException(
			string callbackName,
			Exception exception,
			out ulong count)
		{
			Core.LogError(
				"Unhandled exception in CustomArchitecture.{0}: {1}",
				callbackName,
				exception);
			count = 0;
			return IntPtr.Zero;
		}

		private IntPtr GetFlagConditionsForSemanticFlagGroupAdapter(
			IntPtr context,
			uint semanticGroup,
			out ulong count)
		{
			try
			{
				FlagConditionForSemanticClass[] conditions =
					this.GetFlagConditionsForSemanticFlagGroup(semanticGroup);
				BNFlagConditionForSemanticClass[] native =
					new BNFlagConditionForSemanticClass[conditions.Length];
				for (int index = 0; index < conditions.Length; index++)
				{
					native[index] = conditions[index].ToNative();
				}

				count = (ulong)native.Length;
				return UnsafeUtils.AllocStructArray(native);
			}
			catch (Exception exception)
			{
				return this.HandleFlagListException(
					"GetFlagConditionsForSemanticFlagGroup",
					exception,
					out count);
			}
		}

		private void FreeConditionListAdapter(
			IntPtr context,
			IntPtr conditions,
			ulong count)
		{
			if (IntPtr.Zero != conditions)
			{
				Marshal.FreeHGlobal(conditions);
			}
		}

		private uint GetSemanticClassForFlagWriteTypeAdapter(IntPtr context, uint writeType)
		{
			try
			{
				return this.GetSemanticClassForFlagWriteType(writeType);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetSemanticClassForFlagWriteType: {0}",
					exception);
				return 0;
			}
		}
	}
}
