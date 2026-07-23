using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public sealed partial class PluginCommand
    {
        private static readonly object globalRegistrationLock = new object();

        private static readonly List<GlobalRegistration> globalRegistrations =
            new List<GlobalRegistration>();

        /// <summary>Registers a process-wide command.</summary>
        public static void RegisterGlobal(
            string name,
            string description,
            GlobalCommandDelegate action,
            GlobalIsValidDelegate? isValid = null
        )
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(action);

            GlobalRegistration registration = new GlobalRegistration(
                action,
                isValid
            );
            lock (PluginCommand.globalRegistrationLock)
            {
                NativeMethods.BNRegisterPluginCommandGlobal(
                    name,
                    description,
                    registration.ActionPointer,
                    registration.IsValidPointer,
                    IntPtr.Zero
                );
                PluginCommand.globalRegistrations.Add(registration);
            }
        }

        /// <summary>Gets every currently valid process-wide command.</summary>
        public static PluginCommand[] GetValidGlobalPluginCommands()
        {
            IntPtr commands = NativeMethods.BNGetValidPluginCommandsGlobal(
                out UIntPtr count
            );
            return UnsafeUtils.TakeStructArray<BNPluginCommand, PluginCommand>(
                commands,
                count.ToUInt64(),
                PluginCommand.FromNative,
                NativeMethods.BNFreePluginCommandList
            );
        }

        private sealed class GlobalRegistration
        {
            private readonly GlobalCommandDelegate action;
            private readonly GlobalIsValidDelegate? isValid;
            private readonly BNPluginCommand.GlobalCommandDelegate actionCallback;
            private readonly BNPluginCommand.GlobalIsValidDelegate isValidCallback;

            internal GlobalRegistration(
                GlobalCommandDelegate action,
                GlobalIsValidDelegate? isValid
            )
            {
                this.action = action;
                this.isValid = isValid;
                this.actionCallback =
                    new BNPluginCommand.GlobalCommandDelegate(
                        this.InvokeAction
                    );
                this.isValidCallback =
                    new BNPluginCommand.GlobalIsValidDelegate(
                        this.InvokeIsValid
                    );
            }

            internal IntPtr ActionPointer
            {
                get
                {
                    return Marshal.GetFunctionPointerForDelegate(
                        this.actionCallback
                    );
                }
            }

            internal IntPtr IsValidPointer
            {
                get
                {
                    return Marshal.GetFunctionPointerForDelegate(
                        this.isValidCallback
                    );
                }
            }

            private void InvokeAction(IntPtr context)
            {
                try
                {
                    this.action();
                }
                catch (Exception exception)
                {
                    Core.LogError(
                        "Unhandled exception in global plugin command: {0}",
                        exception
                    );
                }
            }

            private bool InvokeIsValid(IntPtr context)
            {
                try
                {
                    GlobalIsValidDelegate? current = this.isValid;
                    return null == current || current();
                }
                catch (Exception exception)
                {
                    Core.LogError(
                        "Unhandled exception validating global plugin command: {0}",
                        exception
                    );
                    return false;
                }
            }
        }
    }
}
