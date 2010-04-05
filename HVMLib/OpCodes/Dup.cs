using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Dup : OpCode
	{
		public Dup()
		{
			_name = "dup";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.LocalStack.DemandSize(1, this);

			ExecutionStackItem esi = environment.LocalStack.Peek() as ExecutionStackItem;
			if(esi.Value.Value.Type == HVMType.Variable)
			{
				VariableItem v = environment.LocalScope.ResolveStackItem(esi);
				if(v == null)
				{
					throw new RuntimeException("Unable to resolve stack item as variable");
				}

				VariableItem v2 = v.Clone() as VariableItem;

				environment.LocalStack.PushItem(new ExecutionStackItem(v2));
			}
			else
			{
				ExecutionStackItem esi2 = new ExecutionStackItem(esi.Value.Clone() as VariableItem);
				environment.LocalStack.PushItem(esi2);
			}
		}
	}
}
