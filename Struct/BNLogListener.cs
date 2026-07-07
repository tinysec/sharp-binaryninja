using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNLogListener 
	{
		/// <summary>
		/// void (*log)(void* ctxt, size_t sessionId, BNLogLevel level, const char* msg, const char* logger_name, size_t tid);
		/// </summary>
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate void LogDelegate(
			IntPtr ctxt, 
			ulong sessionId, 
			LogLevel level,
			IntPtr msg, 
			IntPtr loggerName, 
			ulong tid
		);
	    
		/// <summary>
		/// void (*logWithStackTrace)(void* ctxt, size_t sessionId, BNLogLevel level, const char* stackTrace,const char* msg, const char* logger_name, size_t tid);
		/// </summary>
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate void LogWithStackTraceDelegate(
			IntPtr ctxt,
			ulong sessionId, 
			LogLevel level,
			IntPtr stackTrace, 
			IntPtr msg, 
			IntPtr loggerName,
			ulong tid
		);
	    
		/// <summary>
		/// void (*close)(void* ctxt);
		/// </summary>
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate void LogCloseDelegate(IntPtr ctxt);
		
		/// <summary>
		/// BNLogLevel (*getLogLevel)(void* ctxt);
		/// </summary>
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate LogLevel GetLogLevelDelegate(IntPtr ctxt);
		
		
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void (*log)(void* ctxt, size_t sessionId, BNLogLevel level, const char* msg, const char* logger_name, size_t tid);
		/// </summary>
		internal IntPtr log;
		
		/// <summary>
		/// void (*logWithStackTrace)(void* ctxt, size_t sessionId, BNLogLevel level, const char* stackTrace,const char* msg, const char* logger_name, size_t tid);
		/// </summary>
		internal IntPtr logWithStackTrace;
		
		/// <summary>
		/// void (*close)(void* ctxt);
		/// </summary>
		internal IntPtr close;
		
		/// <summary>
		/// BNLogLevel (*getLogLevel)(void* ctxt);
		/// </summary>
		internal IntPtr getLogLevel;
	}
	
    public sealed class LogListener 
    {
	    public delegate void LogDelegate(
		    ulong sessionId, 
		    LogLevel level,
		    string msg, 
		    string loggerName, 
		    ulong tid
		);

	    public delegate void LogWithStackTraceDelegate(
		    ulong sessionId, 
		    LogLevel level,
		    string stackTrace, 
		    string msg, 
		    string loggerName,
		    ulong tid
		);
	    
	    public delegate void LogCloseDelegate();
	    
	    public delegate LogLevel GetLogLevelDelegate();

	    public LogDelegate? Log { get; private set; } = null;
	    
	    public LogWithStackTraceDelegate? LogWithStackTrace { get; private set; } = null;
	    
	    public LogCloseDelegate? LogClose { get; private set; } = null;
	    
	    public GetLogLevelDelegate? GetLogLevel { get; private set; } = null;
	    
	    
	    // keep ref to delegate
	    private BNLogListener.LogDelegate? m_log = null;
	    
	    private BNLogListener.LogWithStackTraceDelegate? m_logWithStackTrace = null;
	    
	    private BNLogListener.LogCloseDelegate? m_logClose = null; 
	    
	    private BNLogListener.GetLogLevelDelegate? m_getLogLevel = null;
	    
		public LogListener()
		{
			
		}
		
		internal static LogListener FromNative(BNLogListener native)
		{
			LogListener listener = new LogListener();
			
			if (IntPtr.Zero != native.log)
			{
				listener.m_log = Marshal.GetDelegateForFunctionPointer<BNLogListener.LogDelegate>(
					native.log
				);

				listener.Log = listener.BridgeLog;
			}

			if (IntPtr.Zero != native.logWithStackTrace)
			{
				listener.m_logWithStackTrace =
					Marshal.GetDelegateForFunctionPointer<BNLogListener.LogWithStackTraceDelegate>(
						native.logWithStackTrace
				);
				
				listener.LogWithStackTrace = listener.BridgeLogWithStackTrace;
			}
		
			if (IntPtr.Zero != native.close)
			{
				// Read the close callback from native.close, not native.log (a copy-paste
				// error would otherwise bind the log function as the close callback).
				listener.m_logClose = Marshal.GetDelegateForFunctionPointer<BNLogListener.LogCloseDelegate>(
					native.close
				);

				listener.LogClose = listener.BridgeLogClose;
			}

			if (IntPtr.Zero != native.getLogLevel)
			{
				// Read the log-level callback from native.getLogLevel, not native.log.
				listener.m_getLogLevel = Marshal.GetDelegateForFunctionPointer<BNLogListener.GetLogLevelDelegate>(
					native.getLogLevel
				);

				listener.GetLogLevel = listener.BridgeGetLogLevel;
			}
			
			return listener;
		}
	
		private void BridgeLog(
			ulong sessionId ,
			LogLevel level , 
			string msg , 
			string loggerName ,
			ulong tid)
		{
			if (null == this.m_log)
			{
				throw new NullReferenceException();
			}

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				this.m_log(
					IntPtr.Zero,
					sessionId,
					level,
					allocator.AllocAnsiString(msg),
					allocator.AllocAnsiString(loggerName),
					tid
				);
			}
		}
		
		private void BridgeLogWithStackTrace(
			ulong sessionId, 
			LogLevel level,
			string stackTrace, 
			string msg, 
			string loggerName,
			ulong tid)
		{
			if (null == this.m_logWithStackTrace)
			{
				throw new NullReferenceException();
			}

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				this.m_logWithStackTrace(
					IntPtr.Zero,
					sessionId,
					level,
					allocator.AllocAnsiString(stackTrace),
					allocator.AllocAnsiString(msg),
					allocator.AllocAnsiString(loggerName),
					tid
				);
			}
		}

		private void BridgeLogClose()
		{
			if (null == this.m_logClose)
			{
				throw new NullReferenceException();
			}

			this.m_logClose(IntPtr.Zero);
		}
		
		private LogLevel BridgeGetLogLevel()
		{
			if (null == this.m_getLogLevel)
			{
				throw new NullReferenceException();
			}

			return this.m_getLogLevel(IntPtr.Zero);
		}
    }
    
    public abstract class CustomLogListener : IDisposable
    {
	    private bool m_disposed = false;

	    private bool m_registered = false;

	    private IntPtr m_pointer = IntPtr.Zero;

	    // Cached thunk delegates for the ToNative() direction. A function pointer returned by
	    // GetFunctionPointerForDelegate stays valid only while its source delegate is alive; the
	    // inline method-group delegates (this.LogThunk) would otherwise be collectible the moment
	    // ToNative() returns, and the next native callback into this registered listener would
	    // dereference freed memory (AccessViolation).
	    private BNLogListener.LogDelegate? m_logThunk = null;

	    private BNLogListener.LogWithStackTraceDelegate? m_logWithStackTraceThunk = null;

	    private BNLogListener.LogCloseDelegate? m_logCloseThunk = null;

	    private BNLogListener.GetLogLevelDelegate? m_getLogLevelThunk = null;

		public CustomLogListener()
		{
			this.m_pointer = Marshal.AllocHGlobal(
				Marshal.SizeOf<BNLogListener>()
			);
			
			Marshal.StructureToPtr<BNLogListener>(
				this.ToNative(), 
				this.m_pointer,
				false
			);
		}
		
		~CustomLogListener()
		{
			Dispose(false);
		}
		
		public void Dispose()
		{
			Dispose(true);
			
			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (this.m_disposed)
			{
				return;
			}

			this.m_disposed = true;

			if (this.m_registered)
			{
				this.m_registered = false;
			
				NativeMethods.BNUnregisterLogListener(this.m_pointer);
			}
		
			if (this.m_pointer != IntPtr.Zero)
			{
				Marshal.DestroyStructure<BNLogListener>(this.m_pointer);
				
				Marshal.FreeHGlobal(this.m_pointer);
			
				this.m_pointer = IntPtr.Zero;
			}
		}

		public BNLogListener ToNative()
		{
			// Build the thunk delegates once and store them in fields so they stay rooted for the
			// lifetime of this listener. The core keeps the function pointers after Register(), so
			// the delegate objects must outlive every native callback.
			BNLogListener.LogDelegate logThunk = new BNLogListener.LogDelegate(this.LogThunk);

			BNLogListener.LogWithStackTraceDelegate logWithStackTraceThunk =
				new BNLogListener.LogWithStackTraceDelegate(this.LogWithStackTraceThunk);

			BNLogListener.LogCloseDelegate logCloseThunk =
				new BNLogListener.LogCloseDelegate(this.CloseLogThunk);

			BNLogListener.GetLogLevelDelegate getLogLevelThunk =
				new BNLogListener.GetLogLevelDelegate(this.GetLogLevelThunk);

			this.m_logThunk = logThunk;
			this.m_logWithStackTraceThunk = logWithStackTraceThunk;
			this.m_logCloseThunk = logCloseThunk;
			this.m_getLogLevelThunk = getLogLevelThunk;

			return new BNLogListener()
			{
				context = IntPtr.Zero,
				log = Marshal.GetFunctionPointerForDelegate(logThunk),
				logWithStackTrace = Marshal.GetFunctionPointerForDelegate(logWithStackTraceThunk),
				close = Marshal.GetFunctionPointerForDelegate(logCloseThunk),
				getLogLevel = Marshal.GetFunctionPointerForDelegate(getLogLevelThunk),
			};
		}

		public void Register()
		{
			if (this.m_disposed)
			{
				return;
			}
			
			if (this.m_registered)
			{
				return;
			}
			
			NativeMethods.BNRegisterLogListener(this.m_pointer);
			
			this.m_registered = true;
		}

		#region thunk
		
		private void LogThunk(
			IntPtr ctxt ,
			ulong sessionId ,
			LogLevel level ,
			IntPtr msg ,
			IntPtr loggerName ,
			ulong tid
		)
		{
			this.Log(
				(ulong)sessionId,
				level, 
				UnsafeUtils.ReadAnsiString(msg), 
				UnsafeUtils.ReadAnsiString(loggerName), 
				(ulong)tid
			);
		}
		
		private void LogWithStackTraceThunk(
			IntPtr ctxt,
			ulong sessionId, 
			LogLevel level,
			IntPtr stackTrace, 
			IntPtr msg, 
			IntPtr loggerName,
			ulong tid)
		{
			
			this.LogWithStackTrace(
				(ulong)sessionId,
				level, 
				UnsafeUtils.ReadAnsiString(stackTrace),
				UnsafeUtils.ReadAnsiString(msg), 
				UnsafeUtils.ReadAnsiString(loggerName), 
				(ulong)tid
			);
		}
	
		private void CloseLogThunk(IntPtr ctxt)
		{
			this.CloseLog();
		}
		
		private LogLevel GetLogLevelThunk(IntPtr ctxt)
		{
			return this.GetLogLevel();
		}
		
		#endregion thunk
	
		public virtual void Log(
			ulong sessionId ,
			LogLevel level , 
			string msg , 
			string loggerName ,
			ulong tid)
		{
			
		}
		
		public virtual void LogWithStackTrace(
			ulong sessionId, 
			LogLevel level,
			string stackTrace, 
			string msg, 
			string loggerName,
			ulong tid)
		{
			
		}

		public virtual void CloseLog()
		{
			
		}
		
		public virtual LogLevel GetLogLevel()
		{
			return LogLevel.InfoLog;
		}

    }
	
}