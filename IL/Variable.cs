namespace BinaryNinja
{
	public abstract class AbstractFunctionVariable<T_SELF> : AbstractVariable<T_SELF>
		where T_SELF : AbstractFunctionVariable<T_SELF>
	{
		public Function Function { get; }
	
		internal AbstractFunctionVariable(AbstractFunctionVariable<T_SELF> other) 
			:base(other.Type , other.Index ,other.Storage)
		{
			this.Function = other.Function;
		}

		internal AbstractFunctionVariable(
			Function function ,
			VariableSourceType type ,
			uint index ,
			long storage
		) : base(type , index , storage)
		{
			this.Function = function;
		}
		
		public AbstractFunctionVariable(Function function , BNVariable native)
			:base(native)
		{
			this.Function = function;
		}
		
		public string Name
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(
					NativeMethods.BNGetVariableNameOrDefault(
						this.Function.DangerousGetHandle() ,
						this.ToNative()
					)
				);
			}
		}
		
		public string LastSeenName
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(
					NativeMethods.BNGetLastSeenVariableNameOrDefault(
						this.Function.DangerousGetHandle() ,
						this.ToNative()
					)
				);
			}
		}

		public override string ToString()
		{
			return this.Name;
		}

		/// <summary>
		/// This variable as a plain <see cref="CoreVariable"/> (type/index/storage
		/// without the owning function), for the <c>Function</c> APIs that take one.
		/// </summary>
		public CoreVariable AsCoreVariable()
		{
			return CoreVariable.FromNative(this.ToNative());
		}

		/// <summary>
		/// The data type of this variable. Mirrors Python <c>Variable.type</c>.
		/// (Named <c>VariableType</c> because <see cref="AbstractVariable{T}.Type"/>
		/// already denotes the variable's <see cref="VariableSourceType"/>.)
		/// The setter defines a user variable keeping the current name.
		/// </summary>
		public TypeWithConfidence VariableType
		{
			get
			{
				return this.Function.GetVariableType(this.AsCoreVariable());
			}

			set
			{
				this.Function.CreateUserVariable(
					this.AsCoreVariable() ,
					value ,
					this.Name ,
					false
				);
			}
		}

		/// <summary>
		/// Whether this variable is a parameter of its function.
		/// Mirrors Python <c>Variable.is_parameter_variable</c>.
		/// </summary>
		public bool IsParameter
		{
			get
			{
				foreach (Variable parameter in this.Function.ParameterVariables.Variables)
				{
					if (parameter.Identifier == this.Identifier)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// The dead-store elimination mode for this variable.
		/// Mirrors Python <c>Variable.dead_store_elimination</c>.
		/// </summary>
		public DeadStoreElimination DeadStoreElimination
		{
			get
			{
				return this.Function.GetFunctionVariableDeadStoreElimination(this.AsCoreVariable());
			}

			set
			{
				this.Function.SetFunctionVariableDeadStoreElimination(this.AsCoreVariable() , value);
			}
		}

		public void SetUserValue(ArchitectureAndAddress defSite , PossibleValueSet value , bool after)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNSetUserVariableValue(
					this.Function.DangerousGetHandle() ,
					this.ToNative() ,
					defSite.ToNative() ,
					after ,
					value.ToNativeEx(allocator)
				);
			}
		}
		
		public void ClearUserValue(ArchitectureAndAddress defSite ,  bool after)
		{
			NativeMethods.BNClearUserVariableValue(
				this.Function.DangerousGetHandle() ,
				this.ToNative() ,
				defSite.ToNative() ,
				after 
			);
		}
	}
	
	public sealed class Variable : AbstractFunctionVariable<Variable>
	{
		internal Variable(Variable other) 
			:base(other)
		{
			
		}

		public Variable(Function function , BNVariable native)
			:base(function , native)
		{
			
		}
		
		internal static Variable FromIdentifierEx(Function function ,ulong identifier)
		{
			return Variable.FromNativeEx(
				function,
				NativeMethods.BNFromVariableIdentifier(identifier)
			);
		}
		
		internal static Variable FromNativeEx(Function function , BNVariable native)
		{
			return new Variable(function,  native);
		}
	}
}
