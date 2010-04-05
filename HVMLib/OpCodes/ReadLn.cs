using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class ReadLn : OpCode
	{
		public ReadLn()
		{
			_name = "readln";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			string val = Console.ReadLine();
			VariableItem v = new VariableItem(null, val);
			environment.LocalStack.PushItem(new ExecutionStackItem(v));
		}
	}
}
