using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum NameType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoNameType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		ConstructorNameType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		DestructorNameType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorNewNameType = 3,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorDeleteNameType = 4,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorAssignNameType = 5,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorRightShiftNameType = 6,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorLeftShiftNameType = 7,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorNotNameType = 8,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorEqualNameType = 9,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorNotEqualNameType = 10,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorArrayNameType = 11,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorArrowNameType = 12,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorStarNameType = 13,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorIncrementNameType = 14,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorDecrementNameType = 15,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorMinusNameType = 16,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorPlusNameType = 17,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorBitAndNameType = 18,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorArrowStarNameType = 19,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorDivideNameType = 20,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorModulusNameType = 21,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorLessThanNameType = 22,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorLessThanEqualNameType = 23,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorGreaterThanNameType = 24,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorGreaterThanEqualNameType = 25,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorCommaNameType = 26,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorParenthesesNameType = 27,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorTildeNameType = 28,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorXorNameType = 29,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorBitOrNameType = 30,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorLogicalAndNameType = 31,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorLogicalOrNameType = 32,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorStarEqualNameType = 33,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorPlusEqualNameType = 34,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorMinusEqualNameType = 35,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorDivideEqualNameType = 36,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorModulusEqualNameType = 37,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorRightShiftEqualNameType = 38,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorLeftShiftEqualNameType = 39,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorAndEqualNameType = 40,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorOrEqualNameType = 41,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorXorEqualNameType = 42,
		
		/// <summary>
		/// 
		/// </summary>
		VFTableNameType = 43,
		
		/// <summary>
		/// 
		/// </summary>
		VBTableNameType = 44,
		
		/// <summary>
		/// 
		/// </summary>
		VCallNameType = 45,
		
		/// <summary>
		/// 
		/// </summary>
		TypeofNameType = 46,
		
		/// <summary>
		/// 
		/// </summary>
		LocalStaticGuardNameType = 47,
		
		/// <summary>
		/// 
		/// </summary>
		StringNameType = 48,
		
		/// <summary>
		/// 
		/// </summary>
		VBaseDestructorNameType = 49,
		
		/// <summary>
		/// 
		/// </summary>
		VectorDeletingDestructorNameType = 50,
		
		/// <summary>
		/// 
		/// </summary>
		DefaultConstructorClosureNameType = 51,
		
		/// <summary>
		/// 
		/// </summary>
		ScalarDeletingDestructorNameType = 52,
		
		/// <summary>
		/// 
		/// </summary>
		VectorConstructorIteratorNameType = 53,
		
		/// <summary>
		/// 
		/// </summary>
		VectorDestructorIteratorNameType = 54,
		
		/// <summary>
		/// 
		/// </summary>
		VectorVBaseConstructorIteratorNameType = 55,
		
		/// <summary>
		/// 
		/// </summary>
		VirtualDisplacementMapNameType = 56,
		
		/// <summary>
		/// 
		/// </summary>
		EHVectorConstructorIteratorNameType = 57,
		
		/// <summary>
		/// 
		/// </summary>
		EHVectorDestructorIteratorNameType = 58,
		
		/// <summary>
		/// 
		/// </summary>
		EHVectorVBaseConstructorIteratorNameType = 59,
		
		/// <summary>
		/// 
		/// </summary>
		CopyConstructorClosureNameType = 60,
		
		/// <summary>
		/// 
		/// </summary>
		UDTReturningNameType = 61,
		
		/// <summary>
		/// 
		/// </summary>
		LocalVFTableNameType = 62,
		
		/// <summary>
		/// 
		/// </summary>
		LocalVFTableConstructorClosureNameType = 63,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorNewArrayNameType = 64,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorDeleteArrayNameType = 65,
		
		/// <summary>
		/// 
		/// </summary>
		PlacementDeleteClosureNameType = 66,
		
		/// <summary>
		/// 
		/// </summary>
		PlacementDeleteClosureArrayNameType = 67,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorReturnTypeNameType = 68,
		
		/// <summary>
		/// 
		/// </summary>
		RttiTypeDescriptor = 69,
		
		/// <summary>
		/// 
		/// </summary>
		RttiBaseClassDescriptor = 70,
		
		/// <summary>
		/// 
		/// </summary>
		RttiBaseClassArray = 71,
		
		/// <summary>
		/// 
		/// </summary>
		RttiClassHierarchyDescriptor = 72,
		
		/// <summary>
		/// 
		/// </summary>
		RttiCompleteObjectLocator = 73,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorUnaryMinusNameType = 74,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorUnaryPlusNameType = 75,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorUnaryBitAndNameType = 76,
		
		/// <summary>
		/// 
		/// </summary>
		OperatorUnaryStarNameType = 77,
		
		/// <summary>
		/// 
		/// </summary>
		OmniCallSigNameType = 78,
		
		/// <summary>
		/// 
		/// </summary>
		ManagedVectorConstructorIteratorNameType = 79,
		
		/// <summary>
		/// 
		/// </summary>
		ManagedVectorDestructorIteratorNameType = 80,
		
		/// <summary>
		/// 
		/// </summary>
		EHVectorCopyConstructorIteratorNameType = 81,
		
		/// <summary>
		/// 
		/// </summary>
		EHVectorVBaseCopyConstructorIteratorNameType = 82,
		
		/// <summary>
		/// 
		/// </summary>
		DynamicInitializerNameType = 83,
		
		/// <summary>
		/// 
		/// </summary>
		DynamicAtExitDestructorNameType = 84,
		
		/// <summary>
		/// 
		/// </summary>
		VectorCopyConstructorIteratorNameType = 85,
		
		/// <summary>
		/// 
		/// </summary>
		VectorVBaseCopyConstructorIteratorNameType = 86,
		
		/// <summary>
		/// 
		/// </summary>
		ManagedVectorCopyConstructorIteratorNameType = 87,
		
		/// <summary>
		/// 
		/// </summary>
		LocalStaticThreadGuardNameType = 88,
		
		/// <summary>
		/// 
		/// </summary>
		UserDefinedLiteralOperatorNameType = 89
	}
}