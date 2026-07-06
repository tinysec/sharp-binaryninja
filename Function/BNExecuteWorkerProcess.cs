using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNExecuteWorkerProcess(const char* path, const char** args, BNDataBuffer* input, const char** output, const char** error, bool stdoutIsText, bool stderrIsText)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNExecuteWorkerProcess"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNExecuteWorkerProcess(
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  , 
			
			// const char** args
		    string[] args  , 
			
			// BNDataBuffer* input
		    IntPtr input  , 
			
			// const char** output
		    out IntPtr output  , 
			
			// const char** error
		    out IntPtr error  , 
			
			// bool stdoutIsText
		    bool stdoutIsText  , 
			
			// bool stderrIsText
		    bool stderrIsText  
			
		);
	}
}