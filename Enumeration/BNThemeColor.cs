using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ThemeColor : byte
	{
		/// <summary>
		/// 
		/// </summary>
		AddressColor = 0,
		
		/// <summary>
		/// 
		/// </summary>
		ModifiedColor = 1,
		
		/// <summary>
		/// 
		/// </summary>
		InsertedColor = 2,
		
		/// <summary>
		/// 
		/// </summary>
		NotPresentColor = 3,
		
		/// <summary>
		/// 
		/// </summary>
		SelectionColor = 4,
		
		/// <summary>
		/// 
		/// </summary>
		OutlineColor = 5,
		
		/// <summary>
		/// 
		/// </summary>
		BackgroundHighlightDarkColor = 6,
		
		/// <summary>
		/// 
		/// </summary>
		BackgroundHighlightLightColor = 7,
		
		/// <summary>
		/// 
		/// </summary>
		BoldBackgroundHighlightDarkColor = 8,
		
		/// <summary>
		/// 
		/// </summary>
		BoldBackgroundHighlightLightColor = 9,
		
		/// <summary>
		/// 
		/// </summary>
		AlphanumericHighlightColor = 10,
		
		/// <summary>
		/// 
		/// </summary>
		PrintableHighlightColor = 11,
		
		/// <summary>
		/// 
		/// </summary>
		GraphBackgroundDarkColor = 12,
		
		/// <summary>
		/// 
		/// </summary>
		GraphBackgroundLightColor = 13,
		
		/// <summary>
		/// 
		/// </summary>
		GraphNodeDarkColor = 14,
		
		/// <summary>
		/// 
		/// </summary>
		GraphNodeLightColor = 15,
		
		/// <summary>
		/// 
		/// </summary>
		GraphNodeOutlineColor = 16,
		
		/// <summary>
		/// 
		/// </summary>
		GraphNodeShadowColor = 17,
		
		/// <summary>
		/// 
		/// </summary>
		GraphEntryNodeIndicatorColor = 18,
		
		/// <summary>
		/// 
		/// </summary>
		GraphExitNodeIndicatorColor = 19,
		
		/// <summary>
		/// 
		/// </summary>
		GraphExitNoreturnNodeIndicatorColor = 20,
		
		/// <summary>
		/// 
		/// </summary>
		TrueBranchColor = 21,
		
		/// <summary>
		/// 
		/// </summary>
		FalseBranchColor = 22,
		
		/// <summary>
		/// 
		/// </summary>
		UnconditionalBranchColor = 23,
		
		/// <summary>
		/// 
		/// </summary>
		AltTrueBranchColor = 24,
		
		/// <summary>
		/// 
		/// </summary>
		AltFalseBranchColor = 25,
		
		/// <summary>
		/// 
		/// </summary>
		AltUnconditionalBranchColor = 26,
		
		/// <summary>
		/// 
		/// </summary>
		InstructionColor = 27,
		
		/// <summary>
		/// 
		/// </summary>
		RegisterColor = 28,
		
		/// <summary>
		/// 
		/// </summary>
		NumberColor = 29,
		
		/// <summary>
		/// 
		/// </summary>
		CodeSymbolColor = 30,
		
		/// <summary>
		/// 
		/// </summary>
		DataSymbolColor = 31,
		
		/// <summary>
		/// 
		/// </summary>
		LocalVariableColor = 32,
		
		/// <summary>
		/// 
		/// </summary>
		StackVariableColor = 33,
		
		/// <summary>
		/// 
		/// </summary>
		ImportColor = 34,
		
		/// <summary>
		/// 
		/// </summary>
		ExportColor = 35,
		
		/// <summary>
		/// 
		/// </summary>
		InstructionHighlightColor = 36,
		
		/// <summary>
		/// 
		/// </summary>
		RelatedInstructionHighlightColor = 37,
		
		/// <summary>
		/// 
		/// </summary>
		TokenHighlightColor = 38,
		
		/// <summary>
		/// 
		/// </summary>
		TokenSelectionColor = 39,
		
		/// <summary>
		/// 
		/// </summary>
		AnnotationColor = 40,
		
		/// <summary>
		/// 
		/// </summary>
		OpcodeColor = 41,
		
		/// <summary>
		/// 
		/// </summary>
		LinearDisassemblyFunctionHeaderColor = 42,
		
		/// <summary>
		/// 
		/// </summary>
		LinearDisassemblyBlockColor = 43,
		
		/// <summary>
		/// 
		/// </summary>
		LinearDisassemblyNoteColor = 44,
		
		/// <summary>
		/// 
		/// </summary>
		LinearDisassemblySeparatorColor = 45,
		
		/// <summary>
		/// 
		/// </summary>
		LinearDisassemblyCodeFoldColor = 46,
		
		/// <summary>
		/// 
		/// </summary>
		StringColor = 47,
		
		/// <summary>
		/// 
		/// </summary>
		TypeNameColor = 48,
		
		/// <summary>
		/// 
		/// </summary>
		FieldNameColor = 49,
		
		/// <summary>
		/// 
		/// </summary>
		KeywordColor = 50,
		
		/// <summary>
		/// 
		/// </summary>
		UncertainColor = 51,
		
		/// <summary>
		/// 
		/// </summary>
		NameSpaceColor = 52,
		
		/// <summary>
		/// 
		/// </summary>
		NameSpaceSeparatorColor = 53,
		
		/// <summary>
		/// 
		/// </summary>
		GotoLabelColor = 54,
		
		/// <summary>
		/// 
		/// </summary>
		CommentColor = 55,
		
		/// <summary>
		/// 
		/// </summary>
		OperationColor = 56,
		
		/// <summary>
		/// 
		/// </summary>
		BaseStructureNameColor = 57,
		
		/// <summary>
		/// 
		/// </summary>
		IndentationLineColor = 58,
		
		/// <summary>
		/// 
		/// </summary>
		IndentationLineHighlightColor = 59,
		
		/// <summary>
		/// 
		/// </summary>
		ScriptConsoleOutputColor = 60,
		
		/// <summary>
		/// 
		/// </summary>
		ScriptConsoleWarningColor = 61,
		
		/// <summary>
		/// 
		/// </summary>
		ScriptConsoleErrorColor = 62,
		
		/// <summary>
		/// 
		/// </summary>
		ScriptConsoleEchoColor = 63,
		
		/// <summary>
		/// 
		/// </summary>
		BlueStandardHighlightColor = 64,
		
		/// <summary>
		/// 
		/// </summary>
		GreenStandardHighlightColor = 65,
		
		/// <summary>
		/// 
		/// </summary>
		CyanStandardHighlightColor = 66,
		
		/// <summary>
		/// 
		/// </summary>
		RedStandardHighlightColor = 67,
		
		/// <summary>
		/// 
		/// </summary>
		MagentaStandardHighlightColor = 68,
		
		/// <summary>
		/// 
		/// </summary>
		YellowStandardHighlightColor = 69,
		
		/// <summary>
		/// 
		/// </summary>
		OrangeStandardHighlightColor = 70,
		
		/// <summary>
		/// 
		/// </summary>
		WhiteStandardHighlightColor = 71,
		
		/// <summary>
		/// 
		/// </summary>
		BlackStandardHighlightColor = 72,
		
		/// <summary>
		/// 
		/// </summary>
		MiniGraphOverlayColor = 73,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapBaseColor = 74,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapNavLineColor = 75,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapNavHighlightColor = 76,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapDataVariableColor = 77,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapAsciiStringColor = 78,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapUnicodeStringColor = 79,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapFunctionColor = 80,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapImportColor = 81,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapExternColor = 82,
		
		/// <summary>
		/// 
		/// </summary>
		FeatureMapLibraryColor = 83,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarBackgroundColor = 84,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarInactiveIconColor = 85,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarHoverIconColor = 86,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarActiveIconColor = 87,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarFocusedIconColor = 88,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarHoverBackgroundColor = 89,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarActiveBackgroundColor = 90,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarFocusedBackgroundColor = 91,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarActiveIndicatorLineColor = 92,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarHeaderBackgroundColor = 93,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarHeaderTextColor = 94,
		
		/// <summary>
		/// 
		/// </summary>
		SidebarWidgetBackgroundColor = 95,
		
		/// <summary>
		/// 
		/// </summary>
		ActivePaneBackgroundColor = 96,
		
		/// <summary>
		/// 
		/// </summary>
		InactivePaneBackgroundColor = 97,
		
		/// <summary>
		/// 
		/// </summary>
		FocusedPaneBackgroundColor = 98,
		
		/// <summary>
		/// 
		/// </summary>
		TabBarTabActiveColor = 99,
		
		/// <summary>
		/// 
		/// </summary>
		TabBarTabHoverColor = 100,
		
		/// <summary>
		/// 
		/// </summary>
		TabBarTabInactiveColor = 101,
		
		/// <summary>
		/// 
		/// </summary>
		TabBarTabBorderColor = 102,
		
		/// <summary>
		/// 
		/// </summary>
		TabBarTabGlowColor = 103,
		
		/// <summary>
		/// 
		/// </summary>
		StatusBarServerConnectedColor = 104,
		
		/// <summary>
		/// 
		/// </summary>
		StatusBarServerDisconnectedColor = 105,
		
		/// <summary>
		/// 
		/// </summary>
		StatusBarServerWarningColor = 106,
		
		/// <summary>
		/// 
		/// </summary>
		StatusBarProjectColor = 107,
		
		/// <summary>
		/// 
		/// </summary>
		BraceOption1Color = 108,
		
		/// <summary>
		/// 
		/// </summary>
		BraceOption2Color = 109,
		
		/// <summary>
		/// 
		/// </summary>
		BraceOption3Color = 110,
		
		/// <summary>
		/// 
		/// </summary>
		BraceOption4Color = 111,
		
		/// <summary>
		/// 
		/// </summary>
		BraceOption5Color = 112,
		
		/// <summary>
		/// 
		/// </summary>
		BraceOption6Color = 113,
		
		/// <summary>
		/// 
		/// </summary>
		VoidTypeColor = 114,
		
		/// <summary>
		/// 
		/// </summary>
		StructureTypeColor = 115,
		
		/// <summary>
		/// 
		/// </summary>
		EnumerationTypeColor = 116,
		
		/// <summary>
		/// 
		/// </summary>
		FunctionTypeColor = 117,
		
		/// <summary>
		/// 
		/// </summary>
		BoolTypeColor = 118,
		
		/// <summary>
		/// 
		/// </summary>
		IntegerTypeColor = 119,
		
		/// <summary>
		/// 
		/// </summary>
		FloatTypeColor = 120,
		
		/// <summary>
		/// 
		/// </summary>
		PointerTypeColor = 121,
		
		/// <summary>
		/// 
		/// </summary>
		ArrayTypeColor = 122,
		
		/// <summary>
		/// 
		/// </summary>
		VarArgsTypeColor = 123,
		
		/// <summary>
		/// 
		/// </summary>
		ValueTypeColor = 124,
		
		/// <summary>
		/// 
		/// </summary>
		NamedTypeReferenceColor = 125,
		
		/// <summary>
		/// 
		/// </summary>
		WideCharTypeColor = 126
	}
}