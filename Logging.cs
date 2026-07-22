using System;

namespace BinaryNinja
{
	public static partial class Core
	{
		/// <summary>Routes messages at or above the given level to standard output.</summary>
		public static void LogToStdout(LogLevel minimumLevel)
		{
			NativeMethods.BNLogToStdout(minimumLevel);
		}

		/// <summary>Routes messages at or above the given level to standard error.</summary>
		public static void LogToStderr(LogLevel minimumLevel)
		{
			NativeMethods.BNLogToStderr(minimumLevel);
		}

		/// <summary>Routes messages at or above the given level to a file.</summary>
		public static bool LogToFile(LogLevel minimumLevel, string path, bool append = false)
		{
			if (null == path)
			{
				throw new ArgumentNullException(nameof(path));
			}

			return NativeMethods.BNLogToFile(minimumLevel, path, append);
		}

		/// <summary>Notifies registered listeners that their configuration may have changed.</summary>
		public static void UpdateLogListeners()
		{
			NativeMethods.BNUpdateLogListeners();
		}

		/// <summary>Flushes and closes every core-managed log destination.</summary>
		public static void CloseLogs()
		{
			NativeMethods.BNCloseLogs();
		}
	}
}
