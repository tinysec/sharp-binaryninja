using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public class LibraryResolver
	{
		private List<string> m_searchDirs = new List<string>();
		
		public LibraryResolver()
		{
			this.AddSearchDir( 
				Directory.GetCurrentDirectory()
			);
			
			this.AddEnvironment(
				"BINARYNINJA_HOME"
			);
		}
		
		public void AddEnvironment(params string[] environments)
		{
			foreach (string environment in environments)
			{
				string? dirname = Environment.GetEnvironmentVariable(environment);

				if (!string.IsNullOrEmpty(dirname) && Directory.Exists(dirname))
				{
					this.AddSearchDir(dirname);
				}
			}
		}
		
		public void AddSearchDir(params string[] dirnames)
		{
			foreach (string dirname in dirnames)
			{
				if (!string.IsNullOrEmpty(dirname))
				{
					if (!this.m_searchDirs.Contains(dirname))
					{
						if (Directory.Exists(dirname))
						{
							this.m_searchDirs.Add(dirname);
						}
					}
				}
			}
		}
		
		public IntPtr ResolveDllImport(
			string libraryName ,
			Assembly assembly ,
			DllImportSearchPath? searchPath
		)
		{
			IntPtr handle = IntPtr.Zero;
			
			string[] possibleNames = this.GetPossibleNames(libraryName);

			foreach (string searchDir in this.m_searchDirs)
			{
				foreach (string possibleName in possibleNames)
				{
					string fullname = Path.Combine(searchDir, possibleName);

					if (Path.Exists(fullname))
					{
						try
						{
							if (NativeLibrary.TryLoad(
								    fullname,
								    assembly, 
								    searchPath,
								    out handle))
							{
								return handle;
							}
						}
						catch (Exception e)
						{
							Console.WriteLine(e);

							throw;
						}
					}
				}
			}
			
			return IntPtr.Zero;
		}

		public string[] GetPossibleNames(string libraryName)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return new string[]
				{
					$"{libraryName}.dll"
				};
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return new string[]
				{
					$"lib{libraryName}.so.1",
					$"lib{libraryName}.so",
					$"{libraryName}.so.1",
					$"{libraryName}.so"
				};
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return new string[]
				{
					$"lib{libraryName}.dylib",
					$"{libraryName}.dylib",
					$"lib{libraryName}.so.1",
					$"lib{libraryName}.so",
					$"{libraryName}.so.1",
					$"{libraryName}.so"
				};
			}
			else
			{
				return new string[]
				{
					libraryName
				};
			}
		}
	}
}
