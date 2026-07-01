# BinaryNinja C# Bindings (Typed, Safe, Native AOT Ready)

[![NuGet](https://img.shields.io/nuget/v/BinaryNinja.svg)](https://www.nuget.org/packages/BinaryNinja)
![License](https://img.shields.io/github/license/tinysec/binaryninja)
![Platforms](https://img.shields.io/badge/platforms-windows%20%7C%20linux-blue)
![.NET](https://img.shields.io/badge/.NET-%3E%3D%208.0-512BD4)

Modern, fully statically typed C# bindings for the Binary Ninja C API. Designed for safety, performance, and Native AOT compatibility. 
Use it as a dynamic library in your apps, compile CLI tools with Native AOT, or build native Binary Ninja plugins.

- Statically typed API surface
- 100% SafeHandle-based resource management
- Idiomatic IDisposable/Dispose pattern
- Native AOT friendly (no reflection-heavy runtime dependencies)
- Works with .NET 8.0+

## WARNING
- Do not use this in production. The API is experimental and may introduce breaking changes without notice.

## Install

```shell
dotnet add package BinaryNinja
```
## Requirements

- .NET 8.0 or newer
- Binary Ninja installed (Desktop or Headless)
- Ensure the Binary Ninja native libraries are discoverable at runtime:
    - environment variable: BINARYNINJA_BASE or BINARYNINJA_HOME (points to Binary Ninja installation root)

## Using in cli
```csharp
using System;
using BinaryNinja;

class Program
{
    static void Main()
    {
        NativeLibrary.SetDllImportResolver(
            typeof(BinaryNinja.Core).Assembly,
            new LibraryResolver().ResolveDllImport
		);
			
        Core.InitPlugins(true);
        
        using BinaryView? view = BinaryView.LoadFile("driver.sys.bndb");
        
        if (null == view)
        {
            throw new Exception("load fail");
        }
        
        foreach(Function function in view.Functions)
        {
            Console.WriteLine(function.RawName);
        }
       
        Core.Shutdown();
    }
}
```

## Using in Plugin

```csharp
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using BinaryNinja;

namespace Dummy
{
	public static class Plugin
	{
		static Plugin()
		{
			NativeLibrary.SetDllImportResolver(
				typeof(BinaryNinja.Core).Assembly,
				new LibraryResolver().ResolveDllImport
			);
		}
		
		[UnmanagedCallersOnly(
			EntryPoint = "CorePluginABIVersion", 
			CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })
		]
		public static uint CorePluginABIVersion()
		{
			return BinaryNinja.Core.CurrentCoreABIVersion;
		}
		
		[UnmanagedCallersOnly(EntryPoint = "CorePluginInit", CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
		public static byte CorePluginInit()
		{
			BinaryNinja.Core.LogInfo("[+] i am native aot plugin\n");
			
			BinaryNinja.Core.LogInfo( $"[+] Core CurrentCoreABIVersion: {Core.GetCurrentCoreABIVersion()}\n");
			
			BinaryNinja.Core.LogInfo( $"[+] Core MinimumCoreABIVersion: {Core.GetMinimumCoreABIVersion()}\n");
			
			BinaryNinja.Core.LogInfo( $"[+] Plugin CurrentCoreABIVersion: {Core.CurrentCoreABIVersion }\n");
			
			BinaryNinja.Logger logger = BinaryNinja.Logger.GetOrCreateLogger("Dummy");
			
			logger.LogInfo("[+] use private logger\n");
			
			BinaryNinja.PluginCommand.RegisterPluginCommand(
				"Dummy command",
				"Dummy command description",
				Plugin.DefaultCommand,
				Plugin.DefaultIsValid
			);
			
			logger.LogInfo("[+] init done.");
			
			return 1; // success
		}
        
        public static void DefaultCommand(BinaryView view )
		{
			Logger logger = BinaryNinja.Logger.GetOrCreateLogger("Dummy");
			
			logger.LogInfo($"view length: {view.Length}");

			Core.OpenUrl("https://github.com/tinysec/binaryninja");
			
			Function? function = view.ChooseFunction();
			
			if (null != function)
			{
				logger.LogInfo($"function , RawName: {function.RawName}");
			}
		}

		public static bool DefaultIsValid(BinaryView view )
		{
			return true;
		}
	}
}
```

## Troubleshooting

- DllNotFoundException / EntryPointNotFoundException
    - Ensure set BINARYNINJA_BASE
    - On Native AOT, confirm your RID is correct and you published with PublishAot=true
- AccessViolationException
    - Check lifetime: ensure using-statements dispose views/sessions before exit
    - Make sure the target BN C API functions exist in the installed version