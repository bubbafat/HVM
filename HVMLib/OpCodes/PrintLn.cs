using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class PrintLn : OpCode
	{
		public PrintLn()
		{
			_name = "println";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.LocalStack.DemandSize(1, this);

			ExecutionStackItem item = environment.LocalStack.PopItem();
			VariableItem var = environment.LocalScope.ResolveStackItem(item);
			Console.WriteLine("{0}", var.Value.StringValue);
		}
	}
}
