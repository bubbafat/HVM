using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Call : OpCode
	{
		public Call()
		{
			_name = "call";
			_arguments = new Argument[2];
			_argumenttypes = new HVMType[2]{ HVMType.String, HVMType.Integer,  };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(2);

			int offSet = environment.OpCodes.JumpTable.GetIndex(Arguments[0].Value.StringValue);
			int argCount = Arguments[1].Value.IntegerValue;

			LexicalScope ls = new LexicalScope(environment.GlobalScope);
			ExecutionStack s = new ExecutionStack(ls);

			if(argCount > 0)
			{
				ExecutionStackItem [] tmpItems = new ExecutionStackItem[argCount];
				for(int i = 0; i < argCount; i++)
				{
					tmpItems[i] = environment.LocalStack.PopItem();
				}

				for(int i = argCount - 1; i >= 0; i--)
				{
					s.PushItem(tmpItems[i]);
				}
			}

			environment.PushStack(s);
			environment.OpCodes.PushCurrentIPAndSet(offSet);
		}
	}
}
