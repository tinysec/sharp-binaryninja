using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNCustomCallingConvention 
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;

		/// <summary>
		/// void (*freeObject)(void* ctxt)
		/// </summary>
		internal IntPtr freeObject;

		/// <summary>
		/// uint32_t* (*getCallerSavedRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getCallerSavedRegisters;

		/// <summary>
		/// uint32_t* (*getCalleeSavedRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getCalleeSavedRegisters;

		/// <summary>
		/// uint32_t* (*getIntegerArgumentRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getIntegerArgumentRegisters;

		/// <summary>
		/// uint32_t* (*getFloatArgumentRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getFloatArgumentRegisters;

		/// <summary>
		/// uint32_t* (*getRequiredArgumentRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getRequiredArgumentRegisters;

		/// <summary>
		/// uint32_t* (*getRequiredClobberedRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getRequiredClobberedRegisters;

		/// <summary>
		/// void (*freeRegisterList)(void* ctxt, uint32_t* regs, size_t len)
		/// </summary>
		internal IntPtr freeRegisterList;

		/// <summary>
		/// bool (*areArgumentRegistersSharedIndex)(void* ctxt)
		/// </summary>
		internal IntPtr areArgumentRegistersSharedIndex;

		/// <summary>
		/// bool (*isStackReservedForArgumentRegisters)(void* ctxt)
		/// </summary>
		internal IntPtr isStackReservedForArgumentRegisters;

		/// <summary>
		/// bool (*isStackAdjustedOnReturn)(void* ctxt)
		/// </summary>
		internal IntPtr isStackAdjustedOnReturn;

		/// <summary>
		/// bool (*isEligibleForHeuristics)(void* ctxt)
		/// </summary>
		internal IntPtr isEligibleForHeuristics;

		/// <summary>
		/// uint32_t (*getIntegerReturnValueRegister)(void* ctxt)
		/// </summary>
		internal IntPtr getIntegerReturnValueRegister;

		/// <summary>
		/// uint32_t (*getHighIntegerReturnValueRegister)(void* ctxt)
		/// </summary>
		internal IntPtr getHighIntegerReturnValueRegister;

		/// <summary>
		/// uint32_t (*getFloatReturnValueRegister)(void* ctxt)
		/// </summary>
		internal IntPtr getFloatReturnValueRegister;

		/// <summary>
		/// uint32_t* (*getGlobalPointerRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getGlobalPointerRegisters;

		/// <summary>
		/// uint32_t* (*getImplicitlyDefinedRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getImplicitlyDefinedRegisters;

		/// <summary>
		/// void (*getIncomingRegisterValue)(void* ctxt, uint32_t reg, BNFunction* func, BNRegisterValue* result)
		/// </summary>
		internal IntPtr getIncomingRegisterValue;

		/// <summary>
		/// void (*getIncomingFlagValue)(void* ctxt, uint32_t flag, BNFunction* func, BNRegisterValue* result)
		/// </summary>
		internal IntPtr getIncomingFlagValue;

		/// <summary>
		/// bool (*isReturnTypeRegisterCompatible)(void* ctxt, BNBinaryView* view, BNType* type)
		/// </summary>
		internal IntPtr isReturnTypeRegisterCompatible;

		/// <summary>
		/// void (*getIndirectReturnValueLocation)(void* ctxt, BNVariable* outVar)
		/// </summary>
		internal IntPtr getIndirectReturnValueLocation;

		/// <summary>
		/// bool (*getReturnedIndirectReturnValuePointer)(void* ctxt, BNVariable* outVar)
		/// </summary>
		internal IntPtr getReturnedIndirectReturnValuePointer;

		/// <summary>
		/// bool (*isArgumentTypeRegisterCompatible)(void* ctxt, BNBinaryView* view, BNType* type)
		/// </summary>
		internal IntPtr isArgumentTypeRegisterCompatible;

		/// <summary>
		/// bool (*isNonRegisterArgumentIndirect)(void* ctxt, BNBinaryView* view, BNType* type)
		/// </summary>
		internal IntPtr isNonRegisterArgumentIndirect;

		/// <summary>
		/// bool (*areStackArgumentsNaturallyAligned)(void* ctxt)
		/// </summary>
		internal IntPtr areStackArgumentsNaturallyAligned;

		/// <summary>
		/// bool (*areStackArgumentsPushedLeftToRight)(void* ctxt)
		/// </summary>
		internal IntPtr areStackArgumentsPushedLeftToRight;

		/// <summary>
		/// void (*getIncomingVariableForParameterVariable)( void* ctxt, const BNVariable* var, BNFunction* func, BNVariable* result)
		/// </summary>
		internal IntPtr getIncomingVariableForParameterVariable;

		/// <summary>
		/// void (*getParameterVariableForIncomingVariable)( void* ctxt, const BNVariable* var, BNFunction* func, BNVariable* result)
		/// </summary>
		internal IntPtr getParameterVariableForIncomingVariable;

		/// <summary>
		/// bool (*areArgumentRegistersUsedForVarArgs)(void* ctxt)
		/// </summary>
		internal IntPtr areArgumentRegistersUsedForVarArgs;

		/// <summary>
		/// void (*getCallLayout)(void* ctxt, BNBinaryView* view, BNReturnValue* returnValue, BNFunctionParameter* params, size_t paramCount, bool hasPermittedRegs, uint32_t* permittedRegs, size_t permittedRegCount, BNCallLayout* result)
		/// </summary>
		internal IntPtr getCallLayout;

		/// <summary>
		/// void (*freeCallLayout)(void* ctxt, BNCallLayout* layout)
		/// </summary>
		internal IntPtr freeCallLayout;

		/// <summary>
		/// void (*getReturnValueLocation)( void* ctxt, BNBinaryView* view, BNReturnValue* returnValue, BNValueLocation* outLocation)
		/// </summary>
		internal IntPtr getReturnValueLocation;

		/// <summary>
		/// void (*freeValueLocation)(void* ctxt, BNValueLocation* location)
		/// </summary>
		internal IntPtr freeValueLocation;

		/// <summary>
		/// BNValueLocation* (*getParameterLocations)(void* ctxt, BNBinaryView* view, BNValueLocation* returnValue, BNFunctionParameter* params, size_t paramCount, bool hasPermittedRegs, uint32_t* permittedRegs, size_t permittedRegCount, size_t* outLocationCount)
		/// </summary>
		internal IntPtr getParameterLocations;

		/// <summary>
		/// void (*freeParameterLocations)(void* ctxt, BNValueLocation* locations, size_t count)
		/// </summary>
		internal IntPtr freeParameterLocations;

		/// <summary>
		/// BNVariable* (*getParameterOrderingForVariables)( void* ctxt, BNBinaryView* view, BNVariable* vars, BNType** types, size_t paramCount, size_t* outCount)
		/// </summary>
		internal IntPtr getParameterOrderingForVariables;

		/// <summary>
		/// void (*freeVariableList)(void* ctxt, BNVariable* vars, size_t count)
		/// </summary>
		internal IntPtr freeVariableList;

		/// <summary>
		/// int64_t (*getStackAdjustmentForLocations)(void* ctxt, BNBinaryView* view, BNValueLocation* returnValue, BNValueLocation* locations, BNType** types, size_t paramCount)
		/// </summary>
		internal IntPtr getStackAdjustmentForLocations;

		/// <summary>
		/// size_t (*getRegisterStackAdjustments)(void* ctxt, BNBinaryView* view, BNValueLocation* returnValue, BNValueLocation* params, size_t paramCount, uint32_t** outRegs, int32_t** outAdjust)
		/// </summary>
		internal IntPtr getRegisterStackAdjustments;

		/// <summary>
		/// void (*freeRegisterStackAdjustments)(void* ctxt, uint32_t* regs, int32_t* adjust, size_t count)
		/// </summary>
		internal IntPtr freeRegisterStackAdjustments;
	}

    public class CustomCallingConvention 
    {
	    
		public CustomCallingConvention() 
		{
		    
		}
    }
}