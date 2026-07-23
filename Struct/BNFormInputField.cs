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

		internal static FormInputField FromNative(BNFormInputField native)
		{
			FormInputField field = new FormInputField();
			field.Type = native.type;
			field.Prompt = UnsafeUtils.ReadUtf8String(native.prompt);
			field.View = BinaryView.TakeHandle(
				IntPtr.Zero == native.view
					? IntPtr.Zero
					: NativeMethods.BNNewViewReference(native.view)
			);
			field.CurrentAddress = native.currentAddress;
			field.Choices = UnsafeUtils.ReadUtf8StringArray(
				native.choices,
				native.count
			);
			field.Ext = UnsafeUtils.ReadUtf8String(native.ext);
			field.DefaultName = UnsafeUtils.ReadUtf8String(native.defaultName);
			field.HasDefault = native.hasDefault;
			field.IntDefault = native.intDefault;
			field.AddressDefault = native.addressDefault;
			field.StringDefault = UnsafeUtils.ReadUtf8String(
				native.stringDefault
			);
			field.IndexDefault = native.indexDefault;

			return field;
		}

		internal BNFormInputField ToNative(ScopedAllocator allocator)
		{
			BNFormInputField native = new BNFormInputField();
			native.type = this.Type;
			native.prompt = allocator.AllocUtf8String(this.Prompt);
			native.view = null == this.View
				? IntPtr.Zero
				: this.View.DangerousGetHandle();
			native.currentAddress = this.CurrentAddress;
			native.choices = allocator.AllocUtf8StringArray(this.Choices);
			native.count = (ulong)this.Choices.Length;
			native.ext = allocator.AllocUtf8String(this.Ext);
			native.defaultName = allocator.AllocUtf8String(this.DefaultName);
			native.hasDefault = this.HasDefault;
			native.intDefault = this.IntDefault;
			native.addressDefault = this.AddressDefault;
			native.stringDefault = allocator.AllocUtf8String(
				this.StringDefault
			);
			native.indexDefault = this.IndexDefault;

			return native;
		}

		internal void ReadResult(BNFormInputField native)
		{
			this.IntResult = native.intResult;
			this.AddressResult = native.addressResult;
			this.StringResult = UnsafeUtils.ReadUtf8String(native.stringResult);
			this.IndexResult = native.indexResult;
		}

		internal IntPtr WriteResult(ref BNFormInputField native)
		{
			native.intResult = this.IntResult;
			native.addressResult = this.AddressResult;
			native.indexResult = this.IndexResult;
			native.stringResult = IntPtr.Zero;
			if (
				FormInputFieldType.TextLineFormField == this.Type
				|| FormInputFieldType.MultilineTextFormField == this.Type
				|| FormInputFieldType.OpenFileNameFormField == this.Type
				|| FormInputFieldType.SaveFileNameFormField == this.Type
				|| FormInputFieldType.DirectoryNameFormField == this.Type
			)
			{
				native.stringResult = NativeMethods.BNAllocString(
					this.StringResult ?? string.Empty
				);
			}

			return native.stringResult;
		}

		internal void ReleaseCallbackReferences()
		{
			if (null != this.View)
			{
				this.View.Dispose();
				this.View = null;
			}
		}
    }

    public abstract class AbstractFormInputField
    {
	    public abstract BNFormInputField ToNativeEx(ScopedAllocator allocator);
    }
}
