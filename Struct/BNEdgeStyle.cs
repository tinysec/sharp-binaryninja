using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNEdgeStyle 
	{
		/// <summary>
		/// BNEdgePenStyle style
		/// </summary>
		internal EdgePenStyle style;
		
		/// <summary>
		/// uint64_t width
		/// </summary>
		internal ulong width;
		
		/// <summary>
		/// BNThemeColor color
		/// </summary>
		internal ThemeColor color;
	}

    public sealed class EdgeStyle : INativeWrapper<BNEdgeStyle>
    {
		public EdgePenStyle Style { get; set; } = EdgePenStyle.NoPen;
		
		public ulong Width { get; set; } = 0;
		
		public ThemeColor Color { get; set; } = ThemeColor.AddressColor;
		
		public EdgeStyle() 
		{
		    
		}

		internal static EdgeStyle FromNative(BNEdgeStyle raw)
		{
			return new EdgeStyle()
			{
				Style = raw.style ,
				Width = raw.width ,
				Color = raw.color
			};
		}

		public BNEdgeStyle ToNative()
		{
			return new BNEdgeStyle
			{
				style = this.Style , 
				width = this.Width ,
				color = this.Color
			};
		}
    }
}