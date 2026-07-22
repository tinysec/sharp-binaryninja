using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>Hosts a global plugin command with managed callbacks.</summary>
	public abstract class GlobalPluginCommand
	{
		private BNPluginCommand.GlobalCommandDelegate? commandCallback;

		private BNPluginCommand.GlobalIsValidDelegate? isValidCallback;

		public void Register(string name, string description = "")
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.commandCallback = new BNPluginCommand.GlobalCommandDelegate(this.InvokeCommand);
			this.isValidCallback = new BNPluginCommand.GlobalIsValidDelegate(this.InvokeIsValid);
			NativeMethods.BNRegisterPluginCommandGlobal(
				name,
				description ?? string.Empty,
				Marshal.GetFunctionPointerForDelegate<BNPluginCommand.GlobalCommandDelegate>(this.commandCallback),
				Marshal.GetFunctionPointerForDelegate<BNPluginCommand.GlobalIsValidDelegate>(this.isValidCallback),
				IntPtr.Zero
			);
		}

		public abstract void Execute();

		public virtual bool IsValid() { return true; }

		private void InvokeCommand(IntPtr context) { this.Execute(); }

		private bool InvokeIsValid(IntPtr context) { return this.IsValid(); }
	}
}
