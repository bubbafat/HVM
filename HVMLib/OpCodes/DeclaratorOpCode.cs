using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public abstract class DeclaratorOpCode : OpCode
	{
		public virtual VariableItem GetVariable()
		{
			string name = Arguments[0].Value.StringValue;
			switch(ArgumentTypes[1])
			{
				case HVMType.QuotedString:
					return new VariableItem(name, Arguments[1].Value.StringValue);
				case HVMType.Integer:
					return new VariableItem(name, Arguments[1].Value.IntegerValue);
				default:
					throw new ParseException( 
						"Variable of known type", ArgumentTypes[1].ToString());
			}
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.LocalScope.Add(GetVariable());
		}
	}
}
