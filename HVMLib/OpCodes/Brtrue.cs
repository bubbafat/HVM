using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Brtrue : OpCode
	{
		public Brtrue()
		{
			_name = "brtrue";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.String };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			ExecutionStackItem esi = environment.LocalStack.PopItem();
			if(esi.Value.Value.Type != HVMType.Boolean)
			{
				throw new RuntimeException("Conditional Branch brtrue requires a boolean type on the stack");
			}

			if(esi.Value.Value.BooleanValue == true)
			{
				int offSet = environment.OpCodes.JumpTable.GetIndex(Arguments[0].Value.StringValue);
				environment.OpCodes.BranchTo(offSet);
			}
		}
	}
}
