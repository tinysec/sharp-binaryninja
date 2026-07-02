using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static MessageBoxButtonResult ShowMessageBox(
			string title = "info",
			string text = "ok",
			MessageBoxButtonSet buttons = MessageBoxButtonSet.OKButtonSet ,
			MessageBoxIcon icon = MessageBoxIcon.InformationIcon
		)
		{
			return NativeMethods.BNShowMessageBox(title ,text , buttons ,icon);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNMessageBoxButtonResult BNShowMessageBox(const char* title, const char* text, BNMessageBoxButtonSet buttons, BNMessageBoxIcon icon)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNShowMessageBox"
        )]
		internal static extern MessageBoxButtonResult BNShowMessageBox(
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// const char* text
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string text  , 
			
			// BNMessageBoxButtonSet buttons
		    MessageBoxButtonSet buttons  , 
			
			// BNMessageBoxIcon icon
		    MessageBoxIcon icon  
		);
	}
}