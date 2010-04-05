using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Read : OpCode
	{
		public Read()
		{
			_name = "read";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			string val = Convert.ToChar(Console.Read()).ToString();
			VariableItem v = new VariableItem(null, val);
			environment.LocalStack.PushItem(new ExecutionStackItem(v));
		}
	}
}
