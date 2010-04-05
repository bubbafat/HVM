using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class PopN : OpCode
	{
		public PopN()
		{
			_name = "popn";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.Integer };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(1);

			int arg = Arguments[0].Value.IntegerValue;

			if(arg <= 0)
			{
				return;
			}

			environment.LocalStack.DemandSize(arg, this);

			for(int i = 0; i < arg; i++)
			{
				environment.LocalStack.PopItem();
			}
		}
	}
}
