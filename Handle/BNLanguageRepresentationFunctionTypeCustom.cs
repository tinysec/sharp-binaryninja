using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public abstract partial class LanguageRepresentationFunctionType
    {
        private static readonly object registrationLock = new object();

        private static readonly List<LanguageRepresentationFunctionType>
            registeredTypes = new List<LanguageRepresentationFunctionType>();

        private readonly object lineOutputLock = new object();

        private readonly Dictionary<IntPtr, ScopedAllocator> lineOutputs =
            new Dictionary<IntPtr, ScopedAllocator>();

        private NativeDelegates.BNLanguageRepresentationCreate? createCallback;

        private NativeDelegates.BNLanguageRepresentationIsValid? isValidCallback;

        private NativeDelegates.BNLanguageRepresentationGetObject?
            getPrinterCallback;

        private NativeDelegates.BNLanguageRepresentationGetObject? getParserCallback;

        private NativeDelegates.BNLanguageRepresentationGetObject?
            getFormatterCallback;

        private NativeDelegates.BNLanguageRepresentationGetFunctionTypeTokens?
            getFunctionTypeTokensCallback;

        private NativeDelegates.BNLanguageRepresentationFreeLines? freeLinesCallback;

        private void RegisterCustomType(string name)
        {
            this.InitializeCallbacks();
            BNCustomLanguageRepresentationFunctionType callbacks =
                new BNCustomLanguageRepresentationFunctionType();
            callbacks.context = IntPtr.Zero;
            callbacks.create = this.GetCallbackPointer(this.createCallback!);
            callbacks.isValid = this.GetCallbackPointer(this.isValidCallback!);
            callbacks.getTypePrinter = this.GetCallbackPointer(
                this.getPrinterCallback!
            );
            callbacks.getTypeParser = this.GetCallbackPointer(
                this.getParserCallback!
            );
            callbacks.getLineFormatter = this.GetCallbackPointer(
                this.getFormatterCallback!
            );
            callbacks.getFunctionTypeTokens = this.GetCallbackPointer(
                this.getFunctionTypeTokensCallback!
            );
            callbacks.freeLines = this.GetCallbackPointer(
                this.freeLinesCallback!
            );

            IntPtr handle = NativeMethods.BNRegisterLanguageRepresentationFunctionType(
                name,
                in callbacks
            );
            if (IntPtr.Zero == handle)
            {
                throw new InvalidOperationException(
                    "The core rejected the language representation type."
                );
            }

            this.SetHandle(handle);
            lock (LanguageRepresentationFunctionType.registrationLock)
            {
                LanguageRepresentationFunctionType.registeredTypes.Add(this);
            }
        }

        private void InitializeCallbacks()
        {
            this.createCallback = new NativeDelegates.BNLanguageRepresentationCreate(
                this.InvokeCreate
            );
            this.isValidCallback =
                new NativeDelegates.BNLanguageRepresentationIsValid(
                    this.InvokeIsValid
                );
            this.getPrinterCallback =
                new NativeDelegates.BNLanguageRepresentationGetObject(
                    this.InvokeGetPrinter
                );
            this.getParserCallback =
                new NativeDelegates.BNLanguageRepresentationGetObject(
                    this.InvokeGetParser
                );
            this.getFormatterCallback =
                new NativeDelegates.BNLanguageRepresentationGetObject(
                    this.InvokeGetFormatter
                );
            this.getFunctionTypeTokensCallback =
                new NativeDelegates.BNLanguageRepresentationGetFunctionTypeTokens(
                    this.InvokeGetFunctionTypeTokens
                );
            this.freeLinesCallback =
                new NativeDelegates.BNLanguageRepresentationFreeLines(
                    this.InvokeFreeLines
                );
        }

        private IntPtr InvokeCreate(
            IntPtr context,
            IntPtr architecture,
            IntPtr owner,
            IntPtr highLevelIL
        )
        {
            Function? managedOwner = null;
            HighLevelILFunction? managedIL = null;
            LanguageRepresentationFunction? function = null;
            try
            {
                managedOwner = Function.NewFromHandle(owner);
                managedIL = HighLevelILFunction.NewFromHandle(highLevelIL);
                Architecture managedArchitecture = Architecture.MustFromHandle(
                    architecture
                );
                function = this.Create(
                    managedArchitecture,
                    managedOwner!,
                    managedIL!
                );
                if (null == function)
                {
                    return IntPtr.Zero;
                }

                IntPtr result =
                    NativeMethods.BNNewLanguageRepresentationFunctionReference(
                        function.DangerousGetHandle()
                    );
                function.ReleaseInitialReferenceForRegistration();

                return result;
            }
            catch (Exception exception)
            {
                this.LogCallbackException("Create", exception);

                return IntPtr.Zero;
            }
            finally
            {
                if (null != function)
                {
                    function.Dispose();
                }

                if (null != managedIL)
                {
                    managedIL.Dispose();
                }

                if (null != managedOwner)
                {
                    managedOwner.Dispose();
                }
            }
        }

        private bool InvokeIsValid(IntPtr context, IntPtr view)
        {
            BinaryView? managedView = null;
            try
            {
                managedView = BinaryView.TakeHandle(
                    NativeMethods.BNNewViewReference(view)
                );
                return this.IsValidFor(managedView!);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("IsValidFor", exception);

                return false;
            }
            finally
            {
                if (null != managedView)
                {
                    managedView.Dispose();
                }
            }
        }

        private IntPtr InvokeGetPrinter(IntPtr context)
        {
            try
            {
                TypePrinter? printer = this.Printer;

                return null == printer
                    ? IntPtr.Zero
                    : printer.DangerousGetHandle();
            }
            catch (Exception exception)
            {
                this.LogCallbackException("Printer", exception);

                return IntPtr.Zero;
            }
        }

        private IntPtr InvokeGetParser(IntPtr context)
        {
            try
            {
                TypeParser? parser = this.Parser;

                return null == parser
                    ? IntPtr.Zero
                    : parser.DangerousGetHandle();
            }
            catch (Exception exception)
            {
                this.LogCallbackException("Parser", exception);

                return IntPtr.Zero;
            }
        }

        private IntPtr InvokeGetFormatter(IntPtr context)
        {
            try
            {
                LineFormatter? formatter = this.Formatter;

                return null == formatter
                    ? IntPtr.Zero
                    : formatter.DangerousGetHandle();
            }
            catch (Exception exception)
            {
                this.LogCallbackException("Formatter", exception);

                return IntPtr.Zero;
            }
        }

        private IntPtr InvokeGetFunctionTypeTokens(
            IntPtr context,
            IntPtr function,
            IntPtr settings,
            IntPtr count
        )
        {
            Function? managedFunction = null;
            DisassemblySettings? managedSettings = null;
            try
            {
                managedFunction = Function.NewFromHandle(function);
                managedSettings = DisassemblySettings.NewFromHandle(settings);
                DisassemblyTextLine[] lines = this.GetFunctionTypeTokens(
                    managedFunction!,
                    managedSettings
                );

                return this.AllocateLines(lines, count);
            }
            catch (Exception exception)
            {
                Marshal.WriteIntPtr(count, IntPtr.Zero);
                this.LogCallbackException("GetFunctionTypeTokens", exception);

                return IntPtr.Zero;
            }
            finally
            {
                if (null != managedSettings)
                {
                    managedSettings.Dispose();
                }

                if (null != managedFunction)
                {
                    managedFunction.Dispose();
                }
            }
        }

        private IntPtr AllocateLines(
            DisassemblyTextLine[]? lines,
            IntPtr count
        )
        {
            DisassemblyTextLine[] safeLines =
                lines ?? Array.Empty<DisassemblyTextLine>();
            Marshal.WriteIntPtr(count, new IntPtr(safeLines.Length));
            if (0 == safeLines.Length)
            {
                return IntPtr.Zero;
            }

            ScopedAllocator allocator = new ScopedAllocator();
            try
            {
                BNDisassemblyTextLine[] nativeLines =
                    new BNDisassemblyTextLine[safeLines.Length];
                for (int i = 0; i < safeLines.Length; i++)
                {
                    nativeLines[i] = safeLines[i].ToNativeEx(allocator);
                }

                IntPtr output = allocator.AllocStructArray(nativeLines);
                lock (this.lineOutputLock)
                {
                    this.lineOutputs.Add(output, allocator);
                }

                return output;
            }
            catch
            {
                allocator.Dispose();
                throw;
            }
        }

        private void InvokeFreeLines(
            IntPtr context,
            IntPtr lines,
            UIntPtr count
        )
        {
            try
            {
                ScopedAllocator? allocator = null;
                lock (this.lineOutputLock)
                {
                    if (this.lineOutputs.TryGetValue(lines, out allocator))
                    {
                        this.lineOutputs.Remove(lines);
                    }
                }

                if (null != allocator)
                {
                    allocator.Dispose();
                }
            }
            catch (Exception exception)
            {
                this.LogCallbackException("FreeLines", exception);
            }
        }

        private IntPtr GetCallbackPointer<TDelegate>(TDelegate callback)
            where TDelegate : Delegate
        {
            return Marshal.GetFunctionPointerForDelegate<TDelegate>(callback);
        }

        private void LogCallbackException(string name, Exception exception)
        {
            Core.LogError(
                "Unhandled exception in LanguageRepresentationFunctionType.{0}: {1}",
                name,
                exception
            );
        }
    }
}
