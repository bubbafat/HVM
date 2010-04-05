using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Cgt : OpCode
	{
		public Cgt()
		{
			_name = "cgt";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.LocalStack.DemandSize(2, this);

			ExecutionStackItem item_rhs = environment.LocalStack.PopItem();
			VariableItem var_rhs = environment.LocalStack.Scope.ResolveStackItem(item_rhs);

			ExecutionStackItem item_lhs = environment.LocalStack.PopItem();
			VariableItem var_lhs = environment.LocalStack.Scope.ResolveStackItem(item_lhs);

			bool result = (var_lhs.Value.CompareTo(var_rhs.Value) > 0);

			VariableItem v = new VariableItem(null, result);

			environment.LocalStack.PushItem(new ExecutionStackItem(v));
		}
	}
}
