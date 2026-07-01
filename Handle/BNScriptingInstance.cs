using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class ScriptingInstance : AbstractSafeHandle<ScriptingInstance>
	{
	    public ScriptingInstance(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static ScriptingInstance? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ScriptingInstance(
			    NativeMethods.BNNewScriptingInstanceReference(handle) ,
			    true
		    );
	    }
	    
	    internal static ScriptingInstance MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ScriptingInstance(
			    NativeMethods.BNNewScriptingInstanceReference(handle) ,
			    true
		    );
	    }
	    
	    internal static ScriptingInstance? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ScriptingInstance(handle, true);
	    }
	    
	    internal static ScriptingInstance MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ScriptingInstance(handle, true);
	    }
	    
	    internal static ScriptingInstance? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ScriptingInstance(handle, false);
	    }
	    
	    internal static ScriptingInstance MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ScriptingInstance(handle, false);
	    }

        /// <summary>
        /// Releases the native BNScriptingInstance handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native scripting instance and mark it invalid to prevent double-free.
                NativeMethods.BNFreeScriptingInstance(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the current input-ready state, indicating whether the scripting engine
        /// is ready to accept, is processing, or has no active input.
        /// </summary>
        public ScriptingProviderInputReadyState InputReadyState
        {
            get
            {
                // Query the native layer for the current input-ready state.
                return NativeMethods.BNGetScriptingInstanceInputReadyState(this.handle);
            }
        }

        /// <summary>
        /// Executes the given script input string in this scripting instance.
        /// </summary>
        /// <param name="input">The script text to execute.</param>
        /// <returns>The execution result (e.g., success, incomplete, invalid).</returns>
        public ScriptingProviderExecuteResult ExecuteScriptInput(string input)
        {
            // Delegate to the native script execution API.
            return NativeMethods.BNExecuteScriptInput(this.handle, input ?? string.Empty);
        }

        /// <summary>
        /// Executes the script contained in the given file in this scripting instance.
        /// </summary>
        /// <param name="filename">The path to the script file to execute.</param>
        /// <returns>The execution result (e.g., success, incomplete, invalid).</returns>
        public ScriptingProviderExecuteResult ExecuteScriptInputFromFile(string filename)
        {
            // Delegate to the native file execution API.
            return NativeMethods.BNExecuteScriptInputFromFilename(this.handle, filename ?? string.Empty);
        }

        /// <summary>
        /// Cancels the currently executing script input, if any.
        /// </summary>
        public void CancelScriptInput()
        {
            // Delegate to the native cancellation API.
            NativeMethods.BNCancelScriptInput(this.handle);
        }

        /// <summary>
        /// Stops this scripting instance, terminating any running script and releasing
        /// the scripting engine's resources.
        /// </summary>
        public void Stop()
        {
            // Delegate to the native stop API.
            NativeMethods.BNStopScriptingInstance(this.handle);
        }

        /// <summary>
        /// Sets the binary view that this scripting instance operates on.
        /// </summary>
        /// <param name="view">The binary view to associate; null clears the association.</param>
        public void SetCurrentBinaryView(BinaryView? view)
        {
            // Forward the binary view handle to the native setter.
            NativeMethods.BNSetScriptingInstanceCurrentBinaryView(
                this.handle,
                (view != null) ? view.DangerousGetHandle() : IntPtr.Zero
            );
        }

        /// <summary>
        /// Sets the function that this scripting instance currently has in focus.
        /// </summary>
        /// <param name="func">The function to associate; null clears the association.</param>
        public void SetCurrentFunction(Function? func)
        {
            // Forward the function handle to the native setter.
            NativeMethods.BNSetScriptingInstanceCurrentFunction(
                this.handle,
                (func != null) ? func.DangerousGetHandle() : IntPtr.Zero
            );
        }

        /// <summary>
        /// Sets the address that this scripting instance treats as the current location.
        /// </summary>
        /// <param name="address">The address to set as current.</param>
        public void SetCurrentAddress(ulong address)
        {
            // Forward the address to the native setter.
            NativeMethods.BNSetScriptingInstanceCurrentAddress(this.handle, address);
        }
    }
}