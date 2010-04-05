using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Not : OpCode
	{
		public Not()
		{
			_name = "not";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.LocalStack.DemandSize(1, this);

			ExecutionStackItem item_rhs = environment.LocalStack.PopItem();
			VariableItem var_rhs = environment.LocalStack.Scope.ResolveStackItem(item_rhs);

			int result = ~var_rhs.Value.IntegerValue;

			Variant vResult = new Variant(result);

			environment.LocalStack.PushItem(new ExecutionStackItem(new Argument(vResult)));
		}
	}
}
