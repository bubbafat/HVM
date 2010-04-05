using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Exit : OpCode
	{
		public Exit()
		{
			_name = "exit";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.OpCodes.BranchTo(environment.OpCodes.Length);
		}
	}
}
