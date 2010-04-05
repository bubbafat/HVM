using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Maxstack : OpCode
	{
		public Maxstack()
		{
			_name = "maxstack";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.Integer };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(1);

			int size = Arguments[0].Value.IntegerValue;
			environment.LocalStack.MaxSize = size;
		}	
	}
}
