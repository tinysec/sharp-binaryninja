using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum OperatorPrecedence : byte
	{
		/// <summary>
		/// 
		/// </summary>
		TopLevelOperatorPrecedence = 0,
		
		/// <summary>
		/// 
		/// </summary>
		AssignmentOperatorPrecedence = 1,
		
		/// <summary>
		/// 
		/// </summary>
		TernaryOperatorPrecedence = 2,
		
		/// <summary>
		/// 
		/// </summary>
		LogicalOrOperatorPrecedence = 3,
		
		/// <summary>
		/// 
		/// </summary>
		LogicalAndOperatorPrecedence = 4,
		
		/// <summary>
		/// 
		/// </summary>
		BitwiseOrOperatorPrecedence = 5,
		
		/// <summary>
		/// 
		/// </summary>
		BitwiseXorOperatorPrecedence = 6,
		
		/// <summary>
		/// 
		/// </summary>
		BitwiseAndOperatorPrecedence = 7,
		
		/// <summary>
		/// 
		/// </summary>
		EqualityOperatorPrecedence = 8,
		
		/// <summary>
		/// 
		/// </summary>
		CompareOperatorPrecedence = 9,
		
		/// <summary>
		/// 
		/// </summary>
		ShiftOperatorPrecedence = 10,
		
		/// <summary>
		/// 
		/// </summary>
		AddOperatorPrecedence = 11,
		
		/// <summary>
		/// 
		/// </summary>
		SubOperatorPrecedence = 12,
		
		/// <summary>
		/// 
		/// </summary>
		MultiplyOperatorPrecedence = 13,
		
		/// <summary>
		/// 
		/// </summary>
		DivideOperatorPrecedence = 14,
		
		/// <summary>
		/// 
		/// </summary>
		LowUnaryOperatorPrecedence = 15,
		
		/// <summary>
		/// 
		/// </summary>
		UnaryOperatorPrecedence = 16,
		
		/// <summary>
		/// 
		/// </summary>
		HighUnaryOperatorPrecedence = 17,
		
		/// <summary>
		/// 
		/// </summary>
		MemberAndFunctionOperatorPrecedence = 18,
		
		/// <summary>
		/// 
		/// </summary>
		ScopeOperatorPrecedence = 19
	}
}