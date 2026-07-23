using System;
using System.Collections.Generic;

namespace BinaryNinja
{
    public partial class ScriptingInstance
    {
        private readonly object listenerLock = new object();

        private readonly List<ScriptingOutputListener> outputListeners =
            new List<ScriptingOutputListener>();

        /// <summary>Registers a scripting output listener.</summary>
        public void RegisterOutputListener(ScriptingOutputListener listener)
        {
            if (null == listener)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            lock (this.listenerLock)
            {
                listener.Register(this);
                this.outputListeners.Add(listener);
            }
        }

        /// <summary>Unregisters a scripting output listener.</summary>
        public void UnregisterOutputListener(ScriptingOutputListener listener)
        {
            if (null == listener)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            lock (this.listenerLock)
            {
                if (!this.outputListeners.Remove(listener))
                {
                    return;
                }

                listener.Unregister(this);
            }
        }

        private void UnregisterAllOutputListeners()
        {
            lock (this.listenerLock)
            {
                while (0 != this.outputListeners.Count)
                {
                    ScriptingOutputListener listener = this.outputListeners[
                        this.outputListeners.Count - 1
                    ];
                    this.outputListeners.RemoveAt(this.outputListeners.Count - 1);
                    listener.Unregister(this);
                }
            }
        }
    }
}
