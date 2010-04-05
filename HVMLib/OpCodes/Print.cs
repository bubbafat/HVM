using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Print : OpCode
	{
		public Print()
		{
			_name = "print";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.LocalStack.DemandSize(1, this);

			ExecutionStackItem item = environment.LocalStack.PopItem();
			VariableItem var = environment.LocalScope.ResolveStackItem(item);
			Console.Write("{0}", var.Value.StringValue);
		}
	}
}
