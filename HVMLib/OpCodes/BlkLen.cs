using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class BlkLen : OpCode
	{
		public BlkLen()
		{
			_name = "blklen";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.Variable };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(1);

			Variable v = environment.LocalScope.ResolveAny(Arguments[0].Value.StringValue);

			if(v == null || v.Type != HVMType.Array)
			{
				throw new OpCodeArgumentException(0, HVMType.Variable, this);
			}

			VariableArray va = v as VariableArray;

			if(va == null)
			{
				throw new OpCodeArgumentException(0, HVMType.Variable, this);
			}

			VariableItem result = new VariableItem(null, va.Length);
			environment.LocalStack.PushItem(new ExecutionStackItem(result));
		}
	}
}
