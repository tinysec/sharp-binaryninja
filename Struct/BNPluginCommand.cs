using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNPluginCommand 
	{
		 /// <summary>
	    /// void (*defaultCommand)(void* ctxt, BNBinaryView* view);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void DefaultCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view
		);

	    /// <summary>
	    /// void (*addressCommand)(void* ctxt, BNBinaryView* view, uint64_t addr);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void AddressCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    ulong address
	    );
	    
	    /// <summary>
	    /// void (*rangeCommand)(void* ctxt, BNBinaryView* view, uint64_t addr, uint64_t len);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void RangeCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    ulong address,
		    ulong length
	    );
	    
	    /// <summary>
	    /// void (*functionCommand)(void* ctxt, BNBinaryView* view, BNFunction* func);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void FunctionCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    /// <summary>
	    /// void (*lowLevelILFunctionCommand)(void* ctxt, BNBinaryView* view, BNLowLevelILFunction* func);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void LowLevelILFunctionCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool LowLevelILFunctionIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    /// <summary>
	    /// void (*lowLevelILInstructionCommand)(void* ctxt, BNBinaryView* view, BNLowLevelILFunction* func, size_t instr);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void LowLevelILInstructionCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function,
		    ulong instruction
	    );
	    
	    /// <summary>
	    /// void (*mediumLevelILFunctionCommand)(void* ctxt, BNBinaryView* view, BNMediumLevelILFunction* func);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void MediumLevelILFunctionCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    /// <summary>
	    /// void (*mediumLevelILInstructionCommand)(void* ctxt, BNBinaryView* view, BNMediumLevelILFunction* func);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void MediumLevelILInstructionCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function,
		    ulong instruction
	    );
	    
	    /// <summary>
	    /// void (*highLevelILFunctionCommand)(void* ctxt, BNBinaryView* view, BNHighLevelILFunction* func);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void HighLevelILFunctionCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    /// <summary>
	    /// void (*highLevelILInstructionCommand)(void* ctxt, BNBinaryView* view, BNHighLevelILFunction* func, size_t instr);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void HighLevelILInstructionCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function,
		    ulong instruction
	    );
	    
	    /// <summary>
	    /// void (*projectCommand)(void* ctxt, BNProject* view);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate void ProjectCommandDelegate(
		    IntPtr ctxt,
		    IntPtr view
	    );
	    
	    /// <summary>
	    /// bool (*defaultIsValid)(void* ctxt, BNBinaryView* view);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool DefaultIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view
	    );
	    
	    /// <summary>
	    /// bool (*addressIsValid)(void* ctxt, BNBinaryView* view, uint64_t addr);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool AddressIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    ulong address
	    );
	    
	    /// <summary>
	    /// bool (*rangeIsValid)(void* ctxt, BNBinaryView* view, uint64_t addr, uint64_t len);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool RangeIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    ulong address,
		    ulong length
	    );
	    
	    /// <summary>
	    /// bool (*functionIsValid)(void* ctxt, BNBinaryView* view, BNFunction* func);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool FunctionIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    /// <summary>
	    /// bool (*lowLevelILFunctionIsValid)(void* ctxt, BNBinaryView* view, BNLowLevelILFunction* func);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool LowLevelILFunctionIsValiddDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    /// <summary>
	    /// bool (*lowLevelILInstructionIsValid)(void* ctxt, BNBinaryView* view, BNLowLevelILFunction* func, size_t instr);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool LowLevelILInstructionIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function,
		    ulong instruction
	    );
	    
	    /// <summary>
	    /// bool (*mediumLevelILFunctionIsValid)(void* ctxt, BNBinaryView* view, BNMediumLevelILFunction* func);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool MediumLevelILFunctionIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    /// <summary>
	    /// bool (*mediumLevelILInstructionIsValid)(void* ctxt, BNBinaryView* view, BNMediumLevelILFunction* func, size_t instr);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool MediumLevelILInstructionIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function,
		    ulong instruction
	    );
	    
	    /// <summary>
	    /// bool (*highLevelILFunctionIsValid)(void* ctxt, BNBinaryView* view, BNHighLevelILFunction* func)
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool HighLevelILFunctionIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function
	    );
	    
	    /// <summary>
	    /// bool (*highLevelILInstructionIsValid)(void* ctxt, BNBinaryView* view, BNHighLevelILFunction* func, size_t instr);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool HighLevelILInstructionIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view,
		    IntPtr function,
		    ulong instruction
	    );
	    
	    /// <summary>
	    /// bool (*projectIsValid)(void* ctxt, BNProject* view);
	    /// </summary>
	    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	    internal unsafe delegate bool ProjectIsValidDelegate(
		    IntPtr ctxt,
		    IntPtr view
	    );
	    
		/// <summary>
		/// const char* name
		/// </summary>
		internal IntPtr name;
		
		/// <summary>
		/// const char* description
		/// </summary>
		internal IntPtr description;
		
		/// <summary>
		/// BNPluginCommandType type
		/// </summary>
		internal PluginCommandType type;
		
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void* defaultCommand
		/// </summary>
		internal IntPtr defaultCommand;
		
		/// <summary>
		/// void* addressCommand
		/// </summary>
		internal IntPtr addressCommand;
		
		/// <summary>
		/// void* rangeCommand
		/// </summary>
		internal IntPtr rangeCommand;
		
		/// <summary>
		/// void* functionCommand
		/// </summary>
		internal IntPtr functionCommand;
		
		/// <summary>
		/// void* lowLevelILFunctionCommand
		/// </summary>
		internal IntPtr lowLevelILFunctionCommand;
		
		/// <summary>
		/// void* lowLevelILInstructionCommand
		/// </summary>
		internal IntPtr lowLevelILInstructionCommand;
		
		/// <summary>
		/// void* mediumLevelILFunctionCommand
		/// </summary>
		internal IntPtr mediumLevelILFunctionCommand;
		
		/// <summary>
		/// void* mediumLevelILInstructionCommand
		/// </summary>
		internal IntPtr mediumLevelILInstructionCommand;
		
		/// <summary>
		/// void* highLevelILFunctionCommand
		/// </summary>
		internal IntPtr highLevelILFunctionCommand;
		
		/// <summary>
		/// void* highLevelILInstructionCommand
		/// </summary>
		internal IntPtr highLevelILInstructionCommand;
		
		/// <summary>
		/// void* projectCommand
		/// </summary>
		internal IntPtr projectCommand;
		
		/// <summary>
		/// void* defaultIsValid
		/// </summary>
		internal IntPtr defaultIsValid;
		
		/// <summary>
		/// void* addressIsValid
		/// </summary>
		internal IntPtr addressIsValid;
		
		/// <summary>
		/// void* rangeIsValid
		/// </summary>
		internal IntPtr rangeIsValid;
		
		/// <summary>
		/// void* functionIsValid
		/// </summary>
		internal IntPtr functionIsValid;
		
		/// <summary>
		/// void* lowLevelILFunctionIsValid
		/// </summary>
		internal IntPtr lowLevelILFunctionIsValid;
		
		/// <summary>
		/// void* lowLevelILInstructionIsValid
		/// </summary>
		internal IntPtr lowLevelILInstructionIsValid;
		
		/// <summary>
		/// void* mediumLevelILFunctionIsValid
		/// </summary>
		internal IntPtr mediumLevelILFunctionIsValid;
		
		/// <summary>
		/// void* mediumLevelILInstructionIsValid
		/// </summary>
		internal IntPtr mediumLevelILInstructionIsValid;
		
		/// <summary>
		/// void* highLevelILFunctionIsValid
		/// </summary>
		internal IntPtr highLevelILFunctionIsValid;
		
		/// <summary>
		/// void* highLevelILInstructionIsValid
		/// </summary>
		internal IntPtr highLevelILInstructionIsValid;
		
		/// <summary>
		/// void* projectIsValid
		/// </summary>
		internal IntPtr projectIsValid;
	}
	
    public sealed class PluginCommand
    {
	    public delegate void DefaultCommandDelegate(
		    BinaryView view
		);
	    
	    public delegate void AddressCommandDelegate(
		    BinaryView view,
		    ulong address
	    );
	    
	    public delegate void RangeCommandDelegate(
		    BinaryView view,
		    ulong address,
		    ulong length
	    );
	    
	    public delegate void FunctionCommandDelegate(
		    BinaryView view,
		    Function function
	    );
	    
	    public delegate void LowLevelILFunctionCommandDelegate(
		    BinaryView view,
		    LowLevelILFunction function
	    );
	    
	    public delegate void LowLevelILInstructionCommandDelegate(
		    BinaryView view,
		    LowLevelILFunction function,
		    ulong instruction
	    );
		
	    public delegate void MediumLevelILFunctionCommandDelegate(
		    BinaryView view,
		    MediumLevelILFunction function
	    );
	    
	    public delegate void MediumLevelILInstructionCommandDelegate(
		    BinaryView view,
		    MediumLevelILFunction function,
		    ulong instruction
	    );
	    
	    public delegate void HighLevelILFunctionCommandDelegate(
		    BinaryView view,
		    HighLevelILFunction function
	    );
	    
	    public delegate void HighLevelILInstructionCommandDelegate(
		    BinaryView view,
		    HighLevelILFunction function,
		    ulong instruction
	    );
	    
	    public delegate void ProjectCommandDelegate(
		    Project project
	    );
	    
	    public delegate bool DefaultIsValidDelegate(
		    BinaryView view
	    );
	    
	    public delegate bool AddressIsValidDelegate(
		    BinaryView view,
		    ulong address
	    );
	    
	    public delegate bool RangeIsValidDelegate(
		    BinaryView view,
		    ulong address,
		    ulong length
	    );
	    
	    public delegate bool FunctionIsValidDelegate(
		    BinaryView view,
		    Function function
	    );
	    
	    public delegate bool LowLevelILFunctionIsValidDelegate(
		    BinaryView view,
		    LowLevelILFunction function
	    );
	    
	    public delegate bool LowLevelILInstructionIsValidDelegate(
		    BinaryView view,
		    LowLevelILFunction function,
		    ulong instruction
	    );
	    
	    public delegate bool MediumLevelILFunctionIsValidDelegate(
		    BinaryView view,
		    MediumLevelILFunction function
	    );
	    
	    public delegate bool MediumLevelILInstructionIsValidDelegate(
		    BinaryView view,
		    MediumLevelILFunction function,
		    ulong instruction
	    );
	    
	    public delegate bool HighLevelILFunctionIsValidDelegate(
		    BinaryView view,
		    HighLevelILFunction function
	    );
	    
	    public delegate bool HighLevelILInstructionIsValidDelegate(
		    BinaryView view,
		    HighLevelILFunction function,
		    ulong instruction
	    );
	    
	    public delegate bool ProjectIsValidDelegate(
		    Project project
	    );
	    
		public string Name { get; private set; } = string.Empty;
		
		public string Description { get; private set; } = string.Empty;
		
		public PluginCommandType Type { get; private set; } = PluginCommandType.DefaultPluginCommand;
		
		public DefaultCommandDelegate? DefaultCommand { get; private set; } = null;
		
		public AddressCommandDelegate? AddressCommand{ get; private set; } = null;
	
		public RangeCommandDelegate? RangeCommand{ get; private set; } = null;
		
		public FunctionCommandDelegate? FunctionCommand{ get; private set; } = null;
		
		public LowLevelILFunctionCommandDelegate? LowLevelILFunctionCommand{ get; private set; } = null;
		
		public LowLevelILInstructionCommandDelegate? LowLevelILInstructionCommand{ get; private set; } = null;
	
		public MediumLevelILFunctionCommandDelegate? MediumLevelILFunctionCommand{ get; private set; } = null;
		
		public MediumLevelILInstructionCommandDelegate? MediumLevelILInstructionCommand{ get; private set; } = null;
	
		public HighLevelILFunctionCommandDelegate? HighLevelILFunctionCommand{ get; private set; } = null;
		
		public HighLevelILInstructionCommandDelegate? HighLevelILInstructionCommand{ get; private set; } = null;
	
		public ProjectCommandDelegate? ProjectCommand{ get; private set; } = null;
		
		public DefaultIsValidDelegate? DefaultIsValid{ get; private set; } = null;
		
		public AddressIsValidDelegate? AddressIsValid{ get; private set; } = null;
		
		public RangeIsValidDelegate? RangeIsValid{ get; private set; } = null;
		
		public FunctionIsValidDelegate? FunctionIsValid{ get; private set; } = null;
	
		public LowLevelILFunctionIsValidDelegate? LowLevelILFunctionIsValid{ get; private set; } = null;
	
		public LowLevelILInstructionIsValidDelegate? LowLevelILInstructionIsValid{ get; private set; } = null;
	
		public MediumLevelILFunctionIsValidDelegate? MediumLevelILFunctionIsValid{ get; private set; } = null;
		
		public MediumLevelILInstructionIsValidDelegate? MediumLevelILInstructionIsValid{ get; private set; } = null;
		
		public HighLevelILFunctionIsValidDelegate? HighLevelILFunctionIsValid{ get; private set; } = null;
		
		public HighLevelILInstructionIsValidDelegate? HighLevelILInstructionIsValid{ get; private set; } = null;
		
		public ProjectIsValidDelegate? ProjectIsValid{ get; private set; } = null;
		
		#region native

		/// <summary>
		/// void* defaultCommand
		/// </summary>
		private BNPluginCommand.DefaultCommandDelegate? m_defaultCommand;
		
		/// <summary>
		/// void* addressCommand
		/// </summary>
		private BNPluginCommand.AddressCommandDelegate? m_addressCommand;
		
		/// <summary>
		/// void* rangeCommand
		/// </summary>
		private BNPluginCommand.RangeCommandDelegate? m_rangeCommand;
		
		/// <summary>
		/// void* functionCommand
		/// </summary>
		private BNPluginCommand.FunctionCommandDelegate? m_functionCommand;
		
		/// <summary>
		/// void* lowLevelILFunctionCommand
		/// </summary>
		private BNPluginCommand.LowLevelILFunctionCommandDelegate? m_lowLevelILFunctionCommand;
		
		/// <summary>
		/// void* lowLevelILInstructionCommand
		/// </summary>
		private BNPluginCommand.LowLevelILInstructionCommandDelegate? m_lowLevelILInstructionCommand;
		
		/// <summary>
		/// void* mediumLevelILFunctionCommand
		/// </summary>
		private BNPluginCommand.MediumLevelILFunctionCommandDelegate? m_mediumLevelILFunctionCommand;
		
		/// <summary>
		/// void* mediumLevelILInstructionCommand
		/// </summary>
		private BNPluginCommand.MediumLevelILInstructionCommandDelegate? m_mediumLevelILInstructionCommand;
		
		/// <summary>
		/// void* highLevelILFunctionCommand
		/// </summary>
		private BNPluginCommand.HighLevelILFunctionCommandDelegate? m_highLevelILFunctionCommand;
		
		/// <summary>
		/// void* highLevelILInstructionCommand
		/// </summary>
		private BNPluginCommand.HighLevelILInstructionCommandDelegate? m_highLevelILInstructionCommand;
		
		/// <summary>
		/// void* projectCommand
		/// </summary>
		private BNPluginCommand.ProjectCommandDelegate? m_projectCommand;
		
		/// <summary>
		/// void* defaultIsValid
		/// </summary>
		private BNPluginCommand.DefaultIsValidDelegate? m_defaultIsValid;
		
		/// <summary>
		/// void* addressIsValid
		/// </summary>
		private BNPluginCommand.AddressIsValidDelegate? m_addressIsValid;
		
		/// <summary>
		/// void* rangeIsValid
		/// </summary>
		private BNPluginCommand.RangeIsValidDelegate? m_rangeIsValid;
		
		/// <summary>
		/// void* functionIsValid
		/// </summary>
		private BNPluginCommand.FunctionIsValidDelegate? m_functionIsValid;
		
		/// <summary>
		/// void* lowLevelILFunctionIsValid
		/// </summary>
		private BNPluginCommand.LowLevelILFunctionIsValiddDelegate? m_lowLevelILFunctionIsValid;
		
		/// <summary>
		/// void* lowLevelILInstructionIsValid
		/// </summary>
		private BNPluginCommand.LowLevelILInstructionIsValidDelegate? m_lowLevelILInstructionIsValid;
		
		/// <summary>
		/// void* mediumLevelILFunctionIsValid
		/// </summary>
		private BNPluginCommand.MediumLevelILFunctionIsValidDelegate? m_mediumLevelILFunctionIsValid;
		
		/// <summary>
		/// void* mediumLevelILInstructionIsValid
		/// </summary>
		private BNPluginCommand.MediumLevelILInstructionIsValidDelegate? m_mediumLevelILInstructionIsValid;
		
		/// <summary>
		/// void* highLevelILFunctionIsValid
		/// </summary>
		private BNPluginCommand.HighLevelILFunctionIsValidDelegate? m_highLevelILFunctionIsValid;
		
		/// <summary>
		/// void* highLevelILInstructionIsValid
		/// </summary>
		private BNPluginCommand.HighLevelILInstructionIsValidDelegate? m_highLevelILInstructionIsValid;
		
		/// <summary>
		/// void* projectIsValid
		/// </summary>
		private BNPluginCommand.ProjectIsValidDelegate? m_projectIsValid;
		
		#endregion native
		
		public PluginCommand() 
		{
		    
		}

		internal static PluginCommand FromNative(BNPluginCommand native)
		{
			PluginCommand command = new PluginCommand()
			{
				Name = UnsafeUtils.ReadAnsiString(native.name) ,
				Description = UnsafeUtils.ReadAnsiString(native.description) ,
				Type = native.type 
			};

			if (PluginCommandType.DefaultPluginCommand == native.type)
			{
				if (IntPtr.Zero != native.defaultCommand)
				{
					command.m_defaultCommand = Marshal.GetDelegateForFunctionPointer<BNPluginCommand.DefaultCommandDelegate>(
						native.defaultCommand
					);

					command.DefaultCommand = command.DefaultCommandBridge;
				}

				if (IntPtr.Zero != native.defaultIsValid)
				{
					command.m_defaultIsValid =
						Marshal.GetDelegateForFunctionPointer<BNPluginCommand.DefaultIsValidDelegate>(
							native.defaultIsValid
					);
					
					command.DefaultIsValid = command.DefaultIsValidBridge;
				}
			}
			
			if (PluginCommandType.AddressPluginCommand == native.type)
			{
				
				if ( IntPtr.Zero != native.addressCommand )
				{
					command.m_addressCommand = Marshal.GetDelegateForFunctionPointer<BNPluginCommand.AddressCommandDelegate>(
						native.addressCommand
					);
					
					command.AddressCommand = command.AddressCommandBridge;
				}

				if ( IntPtr.Zero != native.addressIsValid )
				{
					command.m_addressIsValid =
						Marshal.GetDelegateForFunctionPointer<BNPluginCommand.AddressIsValidDelegate>(
							native.addressIsValid
						);
					
					command.AddressIsValid = command.AddressIsValidBridge;
				}
			}
			else if (PluginCommandType.RangePluginCommand == native.type)
			{
				
				if ( IntPtr.Zero != native.rangeCommand )
				{
					command.m_rangeCommand = Marshal.GetDelegateForFunctionPointer<BNPluginCommand.RangeCommandDelegate>(
						native.rangeCommand
					);
					
					command.RangeCommand = command.RangeCommandBridge;
				}
				
				if ( IntPtr.Zero != native.rangeIsValid )
				{
					command.m_rangeIsValid =
						Marshal.GetDelegateForFunctionPointer<BNPluginCommand.RangeIsValidDelegate>(
							native.rangeIsValid
						);
					
					command.RangeIsValid = command.RangeIsValidBridge;
				}
			}
			else if (PluginCommandType.FunctionPluginCommand == native.type)
			{
				if ( IntPtr.Zero != native.functionCommand )
				{
					command.m_functionCommand =
						Marshal.GetDelegateForFunctionPointer<BNPluginCommand.FunctionCommandDelegate>(
							native.functionCommand
						);
					
					command.FunctionCommand = command.FunctionCommandBridge;
				}
				
				if ( IntPtr.Zero != native.functionIsValid )
				{
					command.m_functionIsValid =
						Marshal.GetDelegateForFunctionPointer<BNPluginCommand.FunctionIsValidDelegate>(
							native.functionIsValid
						);
					
					command.FunctionIsValid = command.FunctionIsValidBridge;
				}
			}
			else if (PluginCommandType.LowLevelILFunctionPluginCommand == native.type)
			{
				if ( IntPtr.Zero != native.lowLevelILFunctionCommand )
				{
					command.m_lowLevelILFunctionCommand = Marshal.GetDelegateForFunctionPointer<BNPluginCommand.LowLevelILFunctionCommandDelegate>(
							native.lowLevelILFunctionCommand
					);
					
					command.LowLevelILFunctionCommand = command.LowLevelILFunctionCommandBridge;
				}
				
				if ( IntPtr.Zero != native.lowLevelILFunctionIsValid )
				{
					command.m_lowLevelILFunctionIsValid = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.LowLevelILFunctionIsValiddDelegate>(
							native.lowLevelILFunctionIsValid
						);
					
					command.LowLevelILFunctionIsValid = command.LowLevelILFunctionIsValidBridge;
				}
			}
			else if (PluginCommandType.LowLevelILInstructionPluginCommand == native.type)
			{
				if ( IntPtr.Zero != native.lowLevelILInstructionCommand )
				{
					command.m_lowLevelILInstructionCommand = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.LowLevelILInstructionCommandDelegate>(
							native.lowLevelILInstructionCommand
					);
					
					command.LowLevelILInstructionCommand = command.LowLevelILInstructionCommandBridge;
				}
				
				if ( IntPtr.Zero != native.lowLevelILInstructionIsValid )
				{
					command.m_lowLevelILInstructionIsValid = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.LowLevelILInstructionIsValidDelegate>(
							native.lowLevelILInstructionIsValid
						);
					
					command.LowLevelILInstructionIsValid = command.LowLevelILInstructionIsValidBridge;
				}
			}
			else if (PluginCommandType.MediumLevelILFunctionPluginCommand == native.type)
			{
				if ( IntPtr.Zero != native.mediumLevelILFunctionCommand )
				{
					command.m_mediumLevelILFunctionCommand = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.MediumLevelILFunctionCommandDelegate>(
							native.mediumLevelILFunctionCommand
						);
					
					command.MediumLevelILFunctionCommand = command.MediumLevelILFunctionCommandBridge;
				}
				
				if ( IntPtr.Zero != native.mediumLevelILFunctionIsValid )
				{
					command.m_mediumLevelILFunctionIsValid = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.MediumLevelILFunctionIsValidDelegate>(
							native.mediumLevelILFunctionIsValid
						);
					
					command.MediumLevelILFunctionIsValid = command.MediumLevelILFunctionIsValidBridge;
				}
			}
			else if (PluginCommandType.MediumLevelILInstructionPluginCommand == native.type)
			{
				if ( IntPtr.Zero != native.mediumLevelILInstructionCommand )
				{
					command.m_mediumLevelILInstructionCommand = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.MediumLevelILInstructionCommandDelegate>(
							native.mediumLevelILInstructionCommand
						);
					
					command.MediumLevelILInstructionCommand = command.MediumLevelILInstructionCommandBridge;
				}
				
				if ( IntPtr.Zero != native.mediumLevelILInstructionIsValid )
				{
					command.m_mediumLevelILInstructionIsValid = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.MediumLevelILInstructionIsValidDelegate>(
							native.mediumLevelILInstructionIsValid
						);
					
					command.MediumLevelILInstructionIsValid = command.MediumLevelILInstructionIsValidBridge;
				}
			}
			else if (PluginCommandType.HighLevelILFunctionPluginCommand == native.type)
			{
				if ( IntPtr.Zero != native.highLevelILFunctionCommand )
				{
					command.m_highLevelILFunctionCommand = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.HighLevelILFunctionCommandDelegate>(
							native.highLevelILFunctionCommand
						);
					
					command.HighLevelILFunctionCommand = command.HighLevelILFunctionCommandBridge;
				}
				
				if ( IntPtr.Zero != native.highLevelILFunctionIsValid )
				{
					command.m_highLevelILFunctionIsValid = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.HighLevelILFunctionIsValidDelegate>(
							native.highLevelILFunctionIsValid
						);
					
					command.HighLevelILFunctionIsValid = command.HighLevelILFunctionIsValidBridge;
				}
			}
			else if (PluginCommandType.HighLevelILInstructionPluginCommand == native.type)
			{
				if ( IntPtr.Zero != native.highLevelILInstructionCommand )
				{
					command.m_highLevelILInstructionCommand = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.HighLevelILInstructionCommandDelegate>(
							native.highLevelILInstructionCommand
						);
					
					command.HighLevelILInstructionCommand = command.HighLevelILInstructionCommandBridge;
				}
				
				if ( IntPtr.Zero != native.highLevelILInstructionIsValid )
				{
					command.m_highLevelILInstructionIsValid = Marshal
						.GetDelegateForFunctionPointer<BNPluginCommand.HighLevelILInstructionIsValidDelegate>(
							native.highLevelILInstructionIsValid
						);
					
					command.HighLevelILInstructionIsValid = command.HighLevelILInstructionIsValidBridge;
				}

			}
			else if (PluginCommandType.ProjectPluginCommand == native.type)
			{
				if ( IntPtr.Zero != native.projectCommand )
				{
					command.m_projectCommand =
						Marshal.GetDelegateForFunctionPointer<BNPluginCommand.ProjectCommandDelegate>(
							native.projectCommand
					);
					
					command.ProjectCommand = command.ProjectCommandBridge;
				}
				
				if ( IntPtr.Zero != native.projectIsValid )
				{
					command.m_projectIsValid =
						Marshal.GetDelegateForFunctionPointer<BNPluginCommand.ProjectIsValidDelegate>(
							native.projectIsValid
					);
					
					command.ProjectIsValid = command.ProjectIsValidBridge;
				}
			}
			
			return command;
		}


		private void DefaultCommandBridge(
			BinaryView view
		)
		{
			if (null == this.m_defaultCommand)
			{
				throw new ArgumentNullException(nameof(this.m_defaultCommand));
			}
			
			// void (*defaultCommand)(void* ctxt, BNBinaryView* view);
			this.m_defaultCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle()
			);
		}

		
		private void AddressCommandBridge(
			BinaryView view ,
			ulong address
		)
		{
			if (null == this.m_addressCommand)
			{
				throw new ArgumentNullException(nameof(this.m_addressCommand));
			}
			
			// void (*addressCommand)(void* ctxt, BNBinaryView* view, uint64_t addr);
			this.m_addressCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle(),
				address
			);
		}

		private void RangeCommandBridge(
			BinaryView view ,
			ulong address ,
			ulong length
		)
		{
			if (null == this.m_rangeCommand)
			{
				throw new ArgumentNullException(nameof(this.m_rangeCommand));
			}
			
			// void (*rangeCommand)(void* ctxt, BNBinaryView* view, uint64_t addr, uint64_t len);
			this.m_rangeCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle(),
				address,
				length
			);
		}


		private void FunctionCommandBridge(
			BinaryView view ,
			Function function
		)
		{
			if (null == this.m_functionCommand)
			{
				throw new ArgumentNullException(nameof(this.m_functionCommand));
			}
			
			// void (*functionCommand)(void* ctxt, BNBinaryView* view, BNFunction* func);
			this.m_functionCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle(),
				function.DangerousGetHandle()
			);
		}
		
		private void LowLevelILFunctionCommandBridge(
			BinaryView view ,
			LowLevelILFunction function
		)
		{
			if (null == this.m_lowLevelILFunctionCommand)
			{
				throw new ArgumentNullException(nameof(this.m_lowLevelILFunctionCommand));
			}
			
			// void (*lowLevelILFunctionCommand)(void* ctxt, BNBinaryView* view, BNLowLevelILFunction* func);
			this.m_lowLevelILFunctionCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle(),
				function.DangerousGetHandle()
			);
		}

		private void LowLevelILInstructionCommandBridge(
			BinaryView view ,
			LowLevelILFunction function ,
			ulong instruction
		)
		{
			if (null == this.m_lowLevelILInstructionCommand)
			{
				throw new ArgumentNullException(nameof(this.m_lowLevelILInstructionCommand));
			}
			
			// void (*lowLevelILInstructionCommand)(void* ctxt, BNBinaryView* view, BNLowLevelILFunction* func, size_t instr);
			this.m_lowLevelILInstructionCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle(),
				function.DangerousGetHandle(),
				instruction
			);
		}

		private void MediumLevelILFunctionCommandBridge(
			BinaryView view ,
			MediumLevelILFunction function
		)
		{
			if (null == this.m_mediumLevelILFunctionCommand)
			{
				throw new ArgumentNullException(nameof(this.m_mediumLevelILFunctionCommand));
			}
			
			// void (*mediumLevelILFunctionCommand)(void* ctxt, BNBinaryView* view, BNMediumLevelILFunction* func);
			this.m_mediumLevelILFunctionCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle(),
				function.DangerousGetHandle()
			);
		}

		private void MediumLevelILInstructionCommandBridge(
			BinaryView view ,
			MediumLevelILFunction function ,
			ulong instruction
		)
		{
			if (null == this.m_mediumLevelILInstructionCommand)
			{
				throw new ArgumentNullException(nameof(this.m_mediumLevelILInstructionCommand));
			}
			
			// void (*mediumLevelILInstructionCommand)(void* ctxt, BNBinaryView* view, BNMediumLevelILFunction* func);
			this.m_mediumLevelILInstructionCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle(),
				function.DangerousGetHandle(),
				instruction
			);
		}
		
		private void HighLevelILFunctionCommandBridge(
			BinaryView view ,
			HighLevelILFunction function
		)
		{
			if (null == this.m_highLevelILFunctionCommand)
			{
				throw new ArgumentNullException(nameof(this.m_highLevelILFunctionCommand));
			}
			
			// void (*highLevelILFunctionCommand)(void* ctxt, BNBinaryView* view, BNHighLevelILFunction* func);
			this.m_highLevelILFunctionCommand(
				IntPtr.Zero, 
				view.DangerousGetHandle(),
				function.DangerousGetHandle()
			);
		}

	    private void HighLevelILInstructionCommandBridge(
		    BinaryView view ,
		    HighLevelILFunction function ,
		    ulong instruction
	    )
	    {
		    if (null == this.m_highLevelILInstructionCommand)
		    {
			    throw new ArgumentNullException(nameof(this.m_highLevelILInstructionCommand));
		    }
			
		    // void (*highLevelILInstructionCommand)(void* ctxt, BNBinaryView* view, BNHighLevelILFunction* func, size_t instr);
		    this.m_highLevelILInstructionCommand(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    instruction
		    );
	    }

	    private void ProjectCommandBridge(
		    Project project
	    )
	    {
		    if (null == this.m_projectCommand)
		    {
			    throw new ArgumentNullException(nameof(this.m_projectCommand));
		    }
			
		    // void (*projectCommand)(void* ctxt, BNProject* view);
		    this.m_projectCommand(
			    IntPtr.Zero, 
			    project.DangerousGetHandle()
		    );
	    }
	    
	    
	    private bool DefaultIsValidBridge(
		    BinaryView view
	    )
	    {
		    if (null == this.m_defaultIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_defaultIsValid));
		    }
			
		    // bool (*defaultIsValid)(void* ctxt, BNBinaryView* view);
		    return this.m_defaultIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle()
		    );
	    }

	    private bool AddressIsValidBridge(
		    BinaryView view ,
		    ulong address
	    )
	    {
		    if (null == this.m_addressIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_addressIsValid));
		    }
			
		    // bool (*addressIsValid)(void* ctxt, BNBinaryView* view, uint64_t addr);
		    return this.m_addressIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    address
		    );
	    }

	    private bool RangeIsValidBridge(
		    BinaryView view ,
		    ulong address ,
		    ulong length
	    )
	    {
		    if (null == this.m_rangeIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_rangeIsValid));
		    }
			
		    // bool (*rangeIsValid)(void* ctxt, BNBinaryView* view, uint64_t addr, uint64_t len);
		    return this.m_rangeIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    address,
			    length
		    );
	    }

	    private bool FunctionIsValidBridge(
		    BinaryView view ,
		    Function function
	    )
	    {
		    if (null == this.m_functionIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_functionIsValid));
		    }
			
		    // bool (*functionIsValid)(void* ctxt, BNBinaryView* view, BNFunction* func);
		    return this.m_functionIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle()
		    );
	    }


	    private bool LowLevelILFunctionIsValidBridge(
		    BinaryView view ,
		    LowLevelILFunction function
	    )
	    {
		    if (null == this.m_lowLevelILFunctionIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_lowLevelILFunctionIsValid));
		    }
			
		    // bool (*lowLevelILFunctionIsValid)(void* ctxt, BNBinaryView* view, BNLowLevelILFunction* func);
		    return this.m_lowLevelILFunctionIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle()
		    );
	    }

	    private bool LowLevelILInstructionIsValidBridge(
		    BinaryView view ,
		    LowLevelILFunction function ,
		    ulong instruction
	    )
	    {
		    if (null == this.m_lowLevelILInstructionIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_lowLevelILInstructionIsValid));
		    }
			
		    // bool (*lowLevelILInstructionIsValid)(void* ctxt, BNBinaryView* view, BNLowLevelILFunction* func, size_t instr);
		    return this.m_lowLevelILInstructionIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    instruction
		    );
	    }

	    private bool MediumLevelILFunctionIsValidBridge(
		    BinaryView view ,
		    MediumLevelILFunction function
	    )
	    {
		    if (null == this.m_mediumLevelILFunctionIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_mediumLevelILFunctionIsValid));
		    }
			
		    // bool (*mediumLevelILFunctionIsValid)(void* ctxt, BNBinaryView* view, BNMediumLevelILFunction* func);
		    return this.m_mediumLevelILFunctionIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle()
		    );
	    }

	    private bool MediumLevelILInstructionIsValidBridge(
		    BinaryView view ,
		    MediumLevelILFunction function ,
		    ulong instruction
	    )
	    {
		    if (null == this.m_mediumLevelILInstructionIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_mediumLevelILInstructionIsValid));
		    }
			
		    // bool (*mediumLevelILInstructionIsValid)(void* ctxt, BNBinaryView* view, BNMediumLevelILFunction* func, size_t instr);
		    return this.m_mediumLevelILInstructionIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    instruction
		    );
	    }

	    private bool HighLevelILFunctionIsValidBridge(
		    BinaryView view ,
		    HighLevelILFunction function
	    )
	    {
		    if (null == this.m_highLevelILFunctionIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_highLevelILFunctionIsValid));
		    }
			
		    // bool (*highLevelILFunctionIsValid)(void* ctxt, BNBinaryView* view, BNHighLevelILFunction* func)
		    return this.m_highLevelILFunctionIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle()
		    );
	    }

	    private bool HighLevelILInstructionIsValidBridge(
		    BinaryView view ,
		    HighLevelILFunction function ,
		    ulong instruction
	    )
	    {
		    if (null == this.m_highLevelILInstructionIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_highLevelILInstructionIsValid));
		    }
			
		    // bool (*highLevelILInstructionIsValid)(void* ctxt, BNBinaryView* view, BNHighLevelILFunction* func, size_t instr);
		    return this.m_highLevelILInstructionIsValid(
			    IntPtr.Zero, 
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    instruction
		    );
	    }

	    private bool ProjectIsValidBridge(Project project)
	    {
		    if (null == this.m_projectIsValid)
		    {
			    throw new ArgumentNullException(nameof(this.m_projectIsValid));
		    }
			
		    // bool (*projectIsValid)(void* ctxt, BNProject* view);
		    return this.m_projectIsValid(
			    IntPtr.Zero, 
			    project.DangerousGetHandle()
		    );
	    }
	    
	    public static void RegisterPluginCommand(
		    string name,
		    string description,
		    DefaultCommandDelegate command,
		    DefaultIsValidDelegate isValid
		)
	    {
		    BNPluginCommand.DefaultCommandDelegate commandAdapter = (ctxt , view) =>
		    {
			    command( new BinaryView(view , false));
		    };

		    BNPluginCommand.DefaultIsValidDelegate isValidAdapter = bool (ctxt , view) =>
		    {
			    return isValid( new BinaryView(view , false));
		    };
		    
		    NativeMethods.BNRegisterPluginCommand(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.DefaultCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.DefaultIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static void RegisterForAddress(
		    string name,
		    string description,
		    AddressCommandDelegate command,
		    AddressIsValidDelegate isValid
		)
	    {
		    BNPluginCommand.AddressCommandDelegate commandAdapter = (ctxt , view , address) =>
		    {
			    command( new BinaryView(view , false) , address);
		    };

		    BNPluginCommand.AddressIsValidDelegate isValidAdapter = bool (ctxt , view , address) =>
		    {
			    return isValid( new BinaryView(view , false) , address);
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForAddress(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.AddressCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.AddressIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }

	    public static void RegisterForRange(
		    string name,
		    string description,
		    RangeCommandDelegate command,
		    RangeIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.RangeCommandDelegate commandAdapter = (ctxt , view , address , length) =>
		    {
			    command( new BinaryView(view , false) , address , length);
		    };

		    BNPluginCommand.RangeIsValidDelegate isValidAdapter = bool (ctxt , view , address , length) =>
		    {
			    return isValid( new BinaryView(view , false) , address , length);
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForRange(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.RangeCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.RangeIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static void RegisterForFunction(
		    string name,
		    string description,
		    FunctionCommandDelegate command,
		    FunctionIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.FunctionCommandDelegate commandAdapter = (ctxt , view , function) =>
		    {
			    command( 
				    new BinaryView(view , false) , 
				    new  Function(function , false)
				);
		    };

		    BNPluginCommand.FunctionIsValidDelegate isValidAdapter = bool (ctxt , view , function) =>
		    {
			    return isValid( 
				    new BinaryView(view , false) ,
				    new  Function(function , false)
				);
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForFunction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.FunctionCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.FunctionIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static void RegisterForLowLevelILFunction(
		    string name,
		    string description,
		    LowLevelILFunctionCommandDelegate command,
		    LowLevelILFunctionIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.LowLevelILFunctionCommandDelegate commandAdapter = (ctxt , view , function) =>
		    {
			    command( 
				    new BinaryView(view , false) , 
				    new  LowLevelILFunction(function , false)
			    );
		    };

		    BNPluginCommand.LowLevelILFunctionIsValidDelegate isValidAdapter = bool (ctxt , view , function) =>
		    {
			    return isValid( 
				    new BinaryView(view , false) ,
				    new  LowLevelILFunction(function , false)
			    );
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForLowLevelILFunction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.LowLevelILFunctionCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.LowLevelILFunctionIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static void RegisterForLowLevelILInstruction(
		    string name,
		    string description,
		    LowLevelILInstructionCommandDelegate command,
		    LowLevelILInstructionIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.LowLevelILInstructionCommandDelegate commandAdapter = (ctxt , view , function , instruction) =>
		    {
			    command( 
				    new BinaryView(view , false) , 
				    new  LowLevelILFunction(function , false),
				    instruction
			    );
		    };

		    BNPluginCommand.LowLevelILInstructionIsValidDelegate isValidAdapter = bool (ctxt , view , function , instruction) =>
		    {
			    return isValid( 
				    new BinaryView(view , false) ,
				    new  LowLevelILFunction(function , false),
				    instruction
			    );
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForLowLevelILInstruction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.LowLevelILInstructionCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.LowLevelILInstructionIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static void RegisterForMediumLevelILFunction(
		    string name,
		    string description,
		    MediumLevelILFunctionCommandDelegate command,
		    MediumLevelILFunctionIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.MediumLevelILFunctionCommandDelegate commandAdapter = (ctxt , view , function) =>
		    {
			    command( 
				    new BinaryView(view , false) , 
				    new  MediumLevelILFunction(function , false)
			    );
		    };

		    BNPluginCommand.MediumLevelILFunctionIsValidDelegate isValidAdapter = bool (ctxt , view , function) =>
		    {
			    return isValid( 
				    new BinaryView(view , false) ,
				    new  MediumLevelILFunction(function , false)
			    );
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForMediumLevelILFunction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.MediumLevelILFunctionCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.MediumLevelILFunctionIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    
	    public static void RegisterForMediumLevelILInstruction(
		    string name,
		    string description,
		    MediumLevelILInstructionCommandDelegate command,
		    MediumLevelILInstructionIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.MediumLevelILInstructionCommandDelegate commandAdapter = (ctxt , view , function , instruction) =>
		    {
			    command( 
				    new BinaryView(view , false) , 
				    new  MediumLevelILFunction(function , false),
				    instruction
			    );
		    };

		    BNPluginCommand.MediumLevelILInstructionIsValidDelegate isValidAdapter = bool (ctxt , view , function , instruction) =>
		    {
			    return isValid( 
				    new BinaryView(view , false) ,
				    new  MediumLevelILFunction(function , false),
				    instruction
			    );
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForMediumLevelILInstruction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.MediumLevelILInstructionCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.MediumLevelILInstructionIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static void RegisterForHighLevelILFunction(
		    string name,
		    string description,
		    HighLevelILFunctionCommandDelegate command,
		    HighLevelILFunctionIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.HighLevelILFunctionCommandDelegate commandAdapter = (ctxt , view , function) =>
		    {
			    command( 
				    new BinaryView(view , false) , 
				    new  HighLevelILFunction(function , false)
			    );
		    };

		    BNPluginCommand.HighLevelILFunctionIsValidDelegate isValidAdapter = bool (ctxt , view , function) =>
		    {
			    return isValid( 
				    new BinaryView(view , false) ,
				    new  HighLevelILFunction(function , false)
			    );
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForHighLevelILFunction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.HighLevelILFunctionCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.HighLevelILFunctionIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static void RegisterForHighLevelILInstruction(
		    string name,
		    string description,
		    HighLevelILInstructionCommandDelegate command,
		    HighLevelILInstructionIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.HighLevelILInstructionCommandDelegate commandAdapter = (ctxt , view , function , instruction) =>
		    {
			    command( 
				    new BinaryView(view , false) , 
				    new  HighLevelILFunction(function , false),
				    instruction
			    );
		    };

		    BNPluginCommand.HighLevelILInstructionIsValidDelegate isValidAdapter = bool (ctxt , view , function , instruction) =>
		    {
			    return isValid( 
				    new BinaryView(view , false) ,
				    new  HighLevelILFunction(function , false),
				    instruction
			    );
		    };

		    
		    NativeMethods.BNRegisterPluginCommandForHighLevelILInstruction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.HighLevelILInstructionCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.HighLevelILInstructionIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static void RegisterForProject(
		    string name,
		    string description,
		    ProjectCommandDelegate command,
		    ProjectIsValidDelegate isValid
	    )
	    {
		    BNPluginCommand.ProjectCommandDelegate commandAdapter = (ctxt , project) =>
		    {
			    command( 
				    new Project(project , false) 
			    );
		    };

		    BNPluginCommand.ProjectIsValidDelegate isValidAdapter = bool (ctxt , project) =>
		    {
			    return isValid( 
				    new Project(project , false) 
			    );
		    };
		    
		    NativeMethods.BNRegisterPluginCommandForProject(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.ProjectCommandDelegate>(commandAdapter),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.ProjectIsValidDelegate>(isValidAdapter),
			    IntPtr.Zero
		    );
	    }
	    
	    public static PluginCommand[] GetAllPluginCommands()
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetAllPluginCommands(
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public static PluginCommand[] GetValidPluginCommands(BinaryView view)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommands(
			    view.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }

	    public static PluginCommand[] GetValidPluginCommandsForAddress(
		    BinaryView view,
		    ulong address)
	    {
		    
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForAddress(
			    view.DangerousGetHandle(),
			    address,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public static PluginCommand[] GetValidPluginCommandsForRange(
		    BinaryView view,
		    ulong address,
		    ulong length
		)
	    {
		    
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForRange(
			    view.DangerousGetHandle(),
			    address,
			    length,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public static PluginCommand[] GetValidPluginCommandsForFunction(
		    BinaryView view,
		    Function function)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForFunction(
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public static PluginCommand[] GetValidPluginCommandsForLowLevelILFunction(
		    BinaryView view,
		    LowLevelILFunction function)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForLowLevelILFunction(
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public static PluginCommand[] GetValidPluginCommandsForLowLevelILInstruction(
		    BinaryView view,
		    LowLevelILFunction function,
		    ulong instruction
		    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForLowLevelILInstruction(
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    instruction,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    
	    public static PluginCommand[] GetValidPluginCommandsForMediumLevelILFunction(
		    BinaryView view,
		    MediumLevelILFunction function)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForMediumLevelILFunction(
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public static PluginCommand[] GetValidPluginCommandsForMediumLevelILInstruction(
		    BinaryView view,
		    MediumLevelILFunction function,
		    ulong instruction
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForMediumLevelILInstruction(
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    instruction,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public static PluginCommand[] GetValidPluginCommandsForHighLevelILFunction(
		    BinaryView view,
		    HighLevelILFunction function)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForHighLevelILFunction(
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public static PluginCommand[] GetValidPluginCommandsForHighLevelILInstruction(
		    BinaryView view,
		    HighLevelILFunction function,
		    ulong instruction
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForHighLevelILInstruction(
			    view.DangerousGetHandle(),
			    function.DangerousGetHandle(),
			    instruction,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
    }

    public abstract class CustomPluginCommand<TSelf> 
		where TSelf : CustomPluginCommand<TSelf>
    {
		public CustomPluginCommand() 
		{
			
		}
		
		public TSelf Register(
			string name,
			string description = ""
		)
		{
			NativeMethods.BNRegisterPluginCommand(
				name,
				description,
				Marshal.GetFunctionPointerForDelegate<BNPluginCommand.DefaultCommandDelegate>(this.CommandThunk),
				Marshal.GetFunctionPointerForDelegate<BNPluginCommand.DefaultIsValidDelegate>(this.IsValidThunk),
				IntPtr.Zero
			);

			return (TSelf)this;
		}
	
		public static void ClearInstances()
		{
			
		}
		
		private void CommandThunk(
			IntPtr ctxt ,
			IntPtr view
		)
		{
			this.Command(
				new BinaryView(view , false) 
			);
		}
		
		private bool IsValidThunk(
			IntPtr ctxt ,
			IntPtr view
		)
		{
			return this.IsValid(
				new BinaryView(view , false) 
			);
		}

		public virtual void Command(BinaryView view)
		{
			
		}

	    public virtual bool IsValid(BinaryView view)
	    {
		    return false;
	    }
    }
    
    public abstract class CustomAddressCommand
    {
	    public CustomAddressCommand() 
	    {
			
	    }
		
	    public void RegisterForAddress(
		    string name,
		    string description = ""
		)
	    {
		    NativeMethods.BNRegisterPluginCommandForAddress(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.AddressCommandDelegate>(this.AddressCommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.AddressIsValidDelegate>(this.AddressIsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void AddressCommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    ulong address
	    )
	    {
		    this.AddressCommand(
			    new BinaryView(view , false) ,
			    address
		    );
	    }
		
	    private bool AddressIsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    ulong address
	    )
	    {
		    return this.AddressIsValid(
			    new BinaryView(view , false),
			    address
		    );
	    }

	    public virtual void AddressCommand(
		    BinaryView view ,
		    ulong address
	    )
	    {
			
	    }

	    public virtual bool AddressIsValid(
		    BinaryView view ,
		    ulong address
	    )
	    {
		    return false;
	    }
    }
    
    public abstract class CustomRangeCommand 
    {
	    public CustomRangeCommand() 
	    {
			
	    }
		
	    public void RegisterPluginCommandForRange(
		    string name,
		    string description = ""
		)
	    {
		    NativeMethods.BNRegisterPluginCommandForRange(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.RangeCommandDelegate>(this.RangeCommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.RangeIsValidDelegate>(this.RangeIsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void RangeCommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    ulong address ,
		    ulong length
	    )
	    {
		    this.RangeCommand(
			    new BinaryView(view , false) ,
			    address ,
			    length
		    );
	    }
		
	    private bool RangeIsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    ulong address ,
		    ulong length
	    )
	    {
		    return this.RangeIsValid(
			    new BinaryView(
				    view , 
				    false
				) ,
			    address,
			    length
		    );
	    }

	    public virtual void RangeCommand(
		    BinaryView view ,
		    ulong address ,
		    ulong length
	    )
	    {
			
	    }

	    public virtual bool RangeIsValid(
		    BinaryView view ,
		    ulong address ,
		    ulong length
		)
	    {
		    return false;
	    }
    }
    
    
    public abstract class CustomFunctionCommand<TSelf>
		where TSelf : CustomFunctionCommand<TSelf>
    {
	    public CustomFunctionCommand() 
	    {
			
	    }
		
	    public TSelf RegisterForFunction(
		    string name,
		    string description = ""
		)
	    {
		    NativeMethods.BNRegisterPluginCommandForFunction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.FunctionCommandDelegate>(this.CommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.FunctionIsValidDelegate>(this.IsValidThunk),
			    IntPtr.Zero
		    );

		    return (TSelf)this;
	    }
		
	    private void CommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function
	    )
	    {
		    this.Command(
			    new BinaryView(view , false) ,
			    new Function(function,false)
		    );
	    }
		
	    private bool IsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function
	    )
	    {
		    return this.IsValid(
			    new BinaryView(view , false) ,
			    new Function(function,false)
		    );
	    }

	    public virtual void Command(
		    BinaryView view ,
		    Function function
	    )
	    {
			
	    }

	    public virtual bool IsValid(
		    BinaryView view ,
		    Function function
		)
	    {
		    return false;
	    }
    }
    
    
    
    public abstract class CustomLowLevelILFunctionCommand 
    {
	    public CustomLowLevelILFunctionCommand() 
	    {
			
	    }
		
	    public void RegisterForLowLevelILFunction(
		    string name,
		    string description = ""
		    )
	    {
		    NativeMethods.BNRegisterPluginCommandForLowLevelILFunction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.LowLevelILFunctionCommandDelegate>(this.CommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.LowLevelILFunctionIsValiddDelegate>(this.LowLevelILFunctionIsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void CommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function
	    )
	    {
		    this.Command(
			    new BinaryView(view , false) ,
			    new LowLevelILFunction(function,false)
		    );
	    }
		
	    private bool LowLevelILFunctionIsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function
	    )
	    {
		    return this.IsValid(
			    new BinaryView(view , false) ,
			    new LowLevelILFunction(function,false)
		    );
	    }

	    public virtual void Command(
		    BinaryView view ,
		    LowLevelILFunction function
	    )
	    {
			
	    }

	    public virtual bool IsValid(
		    BinaryView view ,
		    LowLevelILFunction function
		)
	    {
		    return false;
	    }
    }
    
    
    public abstract class CustomLowLevelILInstructionCommand 
    {
	    public CustomLowLevelILInstructionCommand() 
	    {
			
	    }
		
	    public void RegisterForLowLevelIlInstruction(
		    string name,
		    string description = ""
		    )
	    {
		    NativeMethods.BNRegisterPluginCommandForLowLevelILInstruction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.LowLevelILInstructionCommandDelegate>(this.CommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.LowLevelILInstructionIsValidDelegate>(this.IsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void CommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function ,
		    ulong instruction
	    )
	    {
		    this.Command(
			    new BinaryView(view , false) ,
			    new LowLevelILFunction(function,false),
			    instruction
		    );
	    }
		
	    private bool IsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function ,
		    ulong instruction
	    )
	    {
		    return this.IsValid(
			    new BinaryView(view , false) ,
			    new LowLevelILFunction(function,false),
			    instruction
		    );
	    }

	    public virtual void Command(
		    BinaryView view ,
		    LowLevelILFunction function ,
		    ulong instruction
	    )
	    {
			
	    }

	    public virtual bool IsValid(
		    BinaryView view ,
		    LowLevelILFunction function ,
		    ulong instruction
		)
	    {
		    return false;
	    }
    }
    
    public abstract class CustomMediumLevelILFunctionCommand
    {

	    public CustomMediumLevelILFunctionCommand() 
	    {
			
	    }
		
	    public void RegisterForMediumLevelILFunction(
		    string name,
		    string description = ""
		)
	    {
		    NativeMethods.BNRegisterPluginCommandForMediumLevelILFunction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.MediumLevelILFunctionCommandDelegate>(this.CommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.MediumLevelILFunctionIsValidDelegate>(this.IsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void CommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function
	    )
	    {
		    this.Command(
			    new BinaryView(view , false) ,
			    new MediumLevelILFunction(function,false)
		    );
	    }
		
	    private bool IsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function
	    )
	    {
		    return this.IsValid(
			    new BinaryView(view , false) ,
			    new MediumLevelILFunction(function,false)
		    );
	    }

	    public virtual void Command(
		    BinaryView view ,
		    MediumLevelILFunction function
	    )
	    {
			
	    }

	    public virtual bool IsValid(
		    BinaryView view ,
		    MediumLevelILFunction function
		)
	    {
		    return false;
	    }
    }
    
    
    public abstract class CustomMediumLevelILInstructionCommand 
    {
	    public CustomMediumLevelILInstructionCommand() 
	    {
			
	    }
		
	    public void RegisterForMediumLevelILInstruction(
		    string name,
		    string description = ""
		    )
	    {
		    NativeMethods.BNRegisterPluginCommandForMediumLevelILInstruction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.MediumLevelILInstructionCommandDelegate>(this.CommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.MediumLevelILInstructionIsValidDelegate>(this.IsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void CommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function ,
		    ulong instruction
	    )
	    {
		    this.Command(
			    new BinaryView(view , false) ,
			    new MediumLevelILFunction(function,false),
			    instruction
		    );
	    }
		
	    private bool IsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function ,
		    ulong instruction
	    )
	    {
		    return this.IsValid(
			    new BinaryView(view , false) ,
			    new MediumLevelILFunction(function,false),
			    instruction
		    );
	    }

	    public virtual void Command(
		    BinaryView view ,
		    MediumLevelILFunction function ,
		    ulong instruction
	    )
	    {
			
	    }

	    public virtual bool IsValid(
		    BinaryView view ,
		    MediumLevelILFunction function ,
		    ulong instruction
		)
	    {
		    return false;
	    }
    }
    
    public abstract class CustomHighLevelILFunctionCommand
    {
	    public CustomHighLevelILFunctionCommand() 
	    {
			
	    }
		
	    public void RegisterPluginCommandForHighLevelILFunction(
		    string name,
		    string description = ""
		)
	    {
		    NativeMethods.BNRegisterPluginCommandForHighLevelILFunction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.HighLevelILFunctionCommandDelegate>(this.CommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.HighLevelILFunctionIsValidDelegate>(this.IsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void CommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function
	    )
	    {
		    this.Command(
			    new BinaryView(view , false) ,
			    new HighLevelILFunction(function,false)
		    );
	    }
		
	    private bool IsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function
	    )
	    {
		    return this.IsValid(
			    new BinaryView(view , false) ,
			    new HighLevelILFunction(function,false)
		    );
	    }

	    public virtual void Command(
		    BinaryView view ,
		    HighLevelILFunction function
	    )
	    {
			
	    }

	    public virtual bool IsValid(
		    BinaryView view ,
		    HighLevelILFunction function
		)
	    {
		    return false;
	    }
    }
    
    public abstract class CustomHighLevelILInstructionCommand 
    {
	    public CustomHighLevelILInstructionCommand() 
	    {
			
	    }
		
	    public void RegisterForHighLevelILInstruction(
		    string name,
		    string description = ""
		)
	    {
		    NativeMethods.BNRegisterPluginCommandForHighLevelILInstruction(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.HighLevelILInstructionCommandDelegate>(this.CommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.HighLevelILInstructionIsValidDelegate>(this.IsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void CommandThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function ,
		    ulong instruction
	    )
	    {
		    this.Command(
			    new BinaryView(view , false) ,
			    new HighLevelILFunction(function,false),
			    instruction
		    );
	    }
		
	    private bool IsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view ,
		    IntPtr function ,
		    ulong instruction
	    )
	    {
		    return this.IsValid(
			    new BinaryView(view , false) ,
			    new HighLevelILFunction(function,false),
			    instruction
		    );
	    }

	    public virtual void Command(
		    BinaryView view ,
		    HighLevelILFunction function ,
		    ulong instruction
	    )
	    {
			
	    }

	    public virtual bool IsValid(
		    BinaryView view ,
		    HighLevelILFunction function ,
		    ulong instruction
		)
	    {
		    return false;
	    }
    }
    
    
    public abstract class CustomProjectCommand
    {
	    public CustomProjectCommand() 
	    {
			
	    }
		
	    public void RegisterForProject(
		    string name,
		    string description = ""
		)
	    {
		    NativeMethods.BNRegisterPluginCommandForProject(
			    name,
			    description,
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.ProjectCommandDelegate>(this.CommandThunk),
			    Marshal.GetFunctionPointerForDelegate<BNPluginCommand.ProjectIsValidDelegate>(this.IsValidThunk),
			    IntPtr.Zero
		    );
	    }
		
	    private void CommandThunk(
		    IntPtr ctxt ,
		    IntPtr view
	    )
	    {
		    this.Command(
			    new Project(view , false) 
		    );
	    }
		
	    private bool IsValidThunk(
		    IntPtr ctxt ,
		    IntPtr view
	    )
	    {
		    return this.IsValid(
			    new Project(view , false) 
		    );
	    }

	    public virtual void Command(Project project)
	    {
			
	    }

	    public virtual bool IsValid(Project project)
	    {
		    return false;
	    }
    }
}