using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Sub : OpCode
	{
		public Sub()
		{
			_name = "sub";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.LocalStack.DemandSize(2, this);

			ExecutionStackItem item_rhs = environment.LocalStack.PopItem();
			VariableItem var_rhs = environment.LocalStack.Scope.ResolveStackItem(item_rhs);

			ExecutionStackItem item_lhs = environment.LocalStack.PopItem();
			VariableItem var_lhs = environment.LocalStack.Scope.ResolveStackItem(item_lhs);

			int result = var_lhs.Value.IntegerValue - var_rhs.Value.IntegerValue;

			Variant vResult = new Variant(result);

			environment.LocalStack.PushItem(new ExecutionStackItem(new Argument(vResult)));
		}
	}
}
