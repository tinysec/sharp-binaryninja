using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
    {
	    public static void LogString(
		    LogLevel level ,
		    string text,
		    string logger_name = "" ,
		    ulong session  = 0
		)
	    {
		    BinaryNinja.NativeMethods.BNLogString(
			    session ,
			    level ,
			    logger_name ,
			    (ulong)Thread.CurrentThread.ManagedThreadId ,
			    text
		    );
	    }

	    public static void LogDebug(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    LogString( LogLevel.DebugLog ,  text );
	    }
	    
	    public static void LogInfo(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    LogString( LogLevel.InfoLog ,  text );
	    }

	    public static void LogWarn(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    LogString( LogLevel.WarningLog ,  text );
	    }
	    
	    public static void LogError(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    LogString( LogLevel.ErrorLog ,  text );
	    }
	    
	    public static void LogAlert(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    LogString( LogLevel.AlertLog ,  text );
	    }
	}
    
    internal static partial class NativeMethods
    {
	    /// <summary>
	    /// void BNLogString(uint64_t session, BNLogLevel level, const char* logger_name, uint64_t tid, const char* str)
	    /// </summary>
	    [DllImport(
		    "binaryninjacore", 
		    CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
		    CharSet = CharSet.Ansi,
		    EntryPoint = "BNLogString"
	    )]
	    public static extern void BNLogString(
			
		    // uint64_t session
		    ulong session  , 
			
		    // BNLogLevel level
		    LogLevel level  , 
			
		    // const char* logger_name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string logger_name  , 
			
		    // uint64_t tid
		    ulong tid  , 
			
		    // const char* str
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string str  
	    );
    }
}