using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Pop : OpCode
	{
		public Pop()
		{
			_name = "pop";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.Variable };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(1);
			environment.LocalStack.DemandSize(1, this);

			ExecutionStackItem item = environment.LocalStack.PopItem();
			
			Variable v = environment.LocalScope.ResolveAny(Arguments[0].Value.StringValue);
			VariableItem vi= null;
			if(v.Type != HVMType.Array)
			{
				vi = v as VariableItem;
			}

			if(v == null)
			{
				throw new OpCodeArgumentException(0, HVMType.String, this);
			}
			
			vi.Value = item.Value.Value.Clone() as Variant;
			environment.LocalScope.Update(vi);
		}
	}
}
