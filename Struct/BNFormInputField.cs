using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNFormInputField 
	{
		/// <summary>
		/// BNFormInputFieldType type
		/// </summary>
		internal FormInputFieldType type;
		
		/// <summary>
		/// const char* prompt
		/// </summary>
		internal IntPtr prompt;
		
		/// <summary>
		/// BNBinaryView* view
		/// </summary>
		internal IntPtr view;
		
		/// <summary>
		/// uint64_t currentAddress
		/// </summary>
		internal ulong currentAddress;
		
		/// <summary>
		/// const char** choices
		/// </summary>
		internal IntPtr choices;
		
		/// <summary>
		/// uint64_t count
		/// </summary>
		internal ulong count;
		
		/// <summary>
		/// const char* ext
		/// </summary>
		internal IntPtr ext;
		
		/// <summary>
		/// const char* defaultName
		/// </summary>
		internal IntPtr defaultName;
		
		/// <summary>
		/// int64_t intResult
		/// </summary>
		internal long intResult;
		
		/// <summary>
		/// uint64_t addressResult
		/// </summary>
		internal ulong addressResult;
		
		/// <summary>
		/// const char* stringResult
		/// </summary>
		internal IntPtr stringResult;
		
		/// <summary>
		/// uint64_t indexResult
		/// </summary>
		internal ulong indexResult;
		
		/// <summary>
		/// bool hasDefault
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool hasDefault;
		
		/// <summary>
		/// int64_t intDefault
		/// </summary>
		internal long intDefault;
		
		/// <summary>
		/// uint64_t addressDefault
		/// </summary>
		internal ulong addressDefault;
		
		/// <summary>
		/// const char* stringDefault
		/// </summary>
		internal IntPtr stringDefault;
		
		/// <summary>
		/// uint64_t indexDefault
		/// </summary>
		internal ulong indexDefault;
	}

	/*
    public class FormInputField
    {
	    public FormInputFieldType Type { get; set; } = FormInputFieldType.LabelFormField;
		
		public string Prompt { get; set; } = string.Empty;

		public BinaryView? View { get; set; } = null;
		
		public ulong CurrentAddress { get; set; } = 0;
		
		public string[] Choices { get; set; } = Array.Empty<string>();
		
		public string Ext { get; set; } = string.Empty;
		
		public string DefaultName { get; set; } = string.Empty;
		
		public long IntResult { get; set; } = 0;
		
		public ulong AddressResult { get; set; } = 0;
		
		public string StringResult { get; set; } = string.Empty;
		
		public ulong IndexResult { get; set; } = 0;
		
		public bool HasDefault { get; set; } = false;
	
		public long IntDefault { get; set; } = 0;
		
		public ulong AddressDefault { get; set; } = 0;
		
		public string StringDefault { get; set; } = string.Empty;
		
		public ulong IndexDefault { get; set; } = 0;
		
		public FormInputField() 
		{
		    
		}
    }
    */

    public abstract class AbstractFormInputField
    {
	    public abstract BNFormInputField ToNativeEx(ScopedAllocator allocator);
    }
}