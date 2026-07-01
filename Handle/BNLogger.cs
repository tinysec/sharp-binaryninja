using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Logger : AbstractSafeHandle<Logger>
	{
		public Logger( string name , ulong sessionId = 0 )
			:this ( Logger.rawGetOrCreateLogger(name , sessionId) , true)
		{
			
		}
		
	    internal Logger(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    internal static Logger? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Logger(handle, true);
	    }
	    
	    internal static Logger MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Logger(handle, true);
	    }
	    
	    public static Logger? GetLogger(string name , ulong sessionId = 0)
	    {
		    return Logger.TakeHandle(
			    NativeMethods.BNLogGetLogger(
				    name ,
				    sessionId
			    )
		    );
	    }
	    
	    public static Logger CreateLogger(string name , ulong sessionId = 0)
	    {
		    return Logger.MustTakeHandle(
			    NativeMethods.BNLogCreateLogger(
                	name ,
                	sessionId
                )
		    );
	    }
	    
	    private static IntPtr rawGetOrCreateLogger(string name , ulong sessionId = 0)
	    {
		    IntPtr loggerHandle = NativeMethods.BNLogGetLogger(name , sessionId);

		    if (IntPtr.Zero != loggerHandle)
		    {
			    return loggerHandle;
		    }

		    return NativeMethods.BNLogCreateLogger(
			    name ,
			    sessionId
		    );
	    }
	    
	    public static Logger GetOrCreateLogger(string name , ulong sessionId = 0)
	    {
		    return Logger.MustTakeHandle(
			    Logger.rawGetOrCreateLogger(name , sessionId)
		    );
	    }

	    public static string[] GetLoggerNames()
	    {
		    IntPtr arrayPointer = NativeMethods.BNLogGetLoggerNames(out ulong arrayLength);
		    
		    return UnsafeUtils.TakeAnsiStringArray(
			    arrayPointer, 
			    arrayLength,
			    NativeMethods.BNFreeStringList
			    );
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeLogger(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNLoggerGetName(this.handle)
			    );
		    }
	    }

	    public ulong SessionId
	    {
		    get
		    {
			    return NativeMethods.BNLoggerGetSessionId(this.handle);
		    }
	    }
	    
	    public void LogString(LogLevel level , string text)
	    {
		    NativeMethods.BNLoggerLogString(this.handle , level , text);
	    }
	    
	    public void LogDebug(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    this.LogString( LogLevel.DebugLog ,  text );
	    }
	    
	    public void LogInfo(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    this.LogString( LogLevel.InfoLog ,  text );
	    }

	    public void LogWarn(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    this.LogString( LogLevel.WarningLog ,  text );
	    }
	    
	    public void LogError(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    this.LogString( LogLevel.ErrorLog ,  text );
	    }
	    
	    public void LogAlert(string format , params object[] args)
	    {
		    string text = string.Format(format, args);
		    
		    this.LogString( LogLevel.AlertLog ,  text );
	    }
	}
}