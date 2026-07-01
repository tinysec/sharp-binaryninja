using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeBuilder* BNCreateFunctionTypeBuilder(BNReturnValue* returnValue, BNCallingConventionWithConfidence* callingConvention, BNFunctionParameter* @params, uint64_t paramCount, BNBoolWithConfidence* varArg, BNBoolWithConfidence* canReturn, BNOffsetWithConfidence* stackAdjust, uint32_t* regStackAdjustRegs, BNOffsetWithConfidence* regStackAdjustValues, uint64_t regStackAdjustCount, BNNameType ft, BNBoolWithConfidence* pure)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateFunctionTypeBuilder"
        )]
		internal static extern IntPtr BNCreateFunctionTypeBuilder(
			
			// BNReturnValue* returnValue
			in BNReturnValue returnValue  ,
			
			// BNCallingConventionWithConfidence* callingConvention
			in BNCallingConventionWithConfidence callingConvention  , 
			
			// BNFunctionParameter* _params
			BNFunctionParameter[] _params  , 
			
			// uint64_t paramCount
		    ulong paramCount  , 
			
			// BNBoolWithConfidence* varArg
		    in BNBoolWithConfidence varArg  , 
			
			// BNBoolWithConfidence* canReturn
		    in BNBoolWithConfidence canReturn  , 
			
			// BNOffsetWithConfidence* stackAdjust
		    in BNOffsetWithConfidence stackAdjust  , 
			
			// uint32_t* regStackAdjustRegs
		    in uint[] regStackAdjustRegs  , 
			
			// BNOffsetWithConfidence* regStackAdjustValues
			BNOffsetWithConfidence[] regStackAdjustValues  , 
			
			// uint64_t regStackAdjustCount
		    ulong regStackAdjustCount  , 
			
			// BNNameType ft
		    NameType ft  , 
			
			// BNBoolWithConfidence* pure
		    in BNBoolWithConfidence pure  
		);
	}
}