using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNScriptingInstanceCallbacks 
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;

		/// <summary>
		/// void (*destroyInstance)(void* ctxt)
		/// </summary>
		internal IntPtr destroyInstance;

		/// <summary>
		/// void (*externalRefTaken)(void* ctxt)
		/// </summary>
		internal IntPtr externalRefTaken;

		/// <summary>
		/// void (*externalRefReleased)(void* ctxt)
		/// </summary>
		internal IntPtr externalRefReleased;

		/// <summary>
		/// BNScriptingProviderExecuteResult (*executeScriptInput)(void* ctxt, const char* input)
		/// </summary>
		internal IntPtr executeScriptInput;

		/// <summary>
		/// BNScriptingProviderExecuteResult (*executeScriptInputFromFilename)(void *ctxt, const char* input)
		/// </summary>
		internal IntPtr executeScriptInputFromFilename;

		/// <summary>
		/// void (*cancelScriptInput)(void* ctxt)
		/// </summary>
		internal IntPtr cancelScriptInput;

		/// <summary>
		/// void (*releaseBinaryView)(void* ctxt, BNBinaryView* view)
		/// </summary>
		internal IntPtr releaseBinaryView;

		/// <summary>
		/// void (*setCurrentBinaryView)(void* ctxt, BNBinaryView* view)
		/// </summary>
		internal IntPtr setCurrentBinaryView;

		/// <summary>
		/// void (*setCurrentFunction)(void* ctxt, BNFunction* func)
		/// </summary>
		internal IntPtr setCurrentFunction;

		/// <summary>
		/// void (*setCurrentBasicBlock)(void* ctxt, BNBasicBlock* block)
		/// </summary>
		internal IntPtr setCurrentBasicBlock;

		/// <summary>
		/// void (*setCurrentAddress)(void* ctxt, uint64_t addr)
		/// </summary>
		internal IntPtr setCurrentAddress;

		/// <summary>
		/// void (*setCurrentSelection)(void* ctxt, uint64_t begin, uint64_t end)
		/// </summary>
		internal IntPtr setCurrentSelection;

		/// <summary>
		/// char* (*completeInput)(void* ctxt, const char* text, uint64_t state)
		/// </summary>
		internal IntPtr completeInput;

		/// <summary>
		/// void (*stop)(void* ctxt)
		/// </summary>
		internal IntPtr stop;

		/// <summary>
		/// bool (*canCompleteArguments)(void* ctx, const char* text)
		/// </summary>
		internal IntPtr canCompleteArguments;

		/// <summary>
		/// char* (*completeArguments)(void* ctxt, const char* text, uint64_t* argumentStart)
		/// </summary>
		internal IntPtr completeArguments;
	}

    public class ScriptingInstanceCallbacks 
    {
		public ScriptingInstanceCallbacks() 
		{
		    
		}
    }
}