using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Ldstr : OpCode
	{
		public Ldstr()
		{
			_name = "ldstr";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.QuotedString };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(1);
			VariableItem v = new VariableItem(null, Arguments[0].Value.StringValue);
			environment.LocalStack.PushItem(new ExecutionStackItem(v));
		}
	}
}
