using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Ret : OpCode
	{
		public Ret()
		{
			_name = "ret";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.Integer };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(1);

			environment.OpCodes.PopCurrentIPAndSet();
			ExecutionStack es = environment.PopStack();

			int retCount = Arguments[0].Value.IntegerValue;

			if(retCount > 0)
			{
				if(retCount > es.Count)
				{
					throw new StackException("Not enough arguments on stack for return values.");
				}

				ExecutionStackItem [] tmpItems = new ExecutionStackItem[retCount];
				for(int i = 0; i < retCount; i++)
				{
					tmpItems[i] = es.PopItem();
				}

				for(int i = retCount - 1; i >= 0; i--)
				{
					environment.LocalStack.PushItem(tmpItems[i]);
				}
			}
		}
	}
}
