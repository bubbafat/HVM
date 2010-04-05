using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Dbg : OpCode
	{
		public Dbg()
		{
			_name = "dbg";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			if(System.Diagnostics.Debugger.IsAttached)
			{
				System.Diagnostics.Debugger.Break();
			}
			else
			{
				System.Diagnostics.Debug.Assert(false, "dbg opcode hit.");
			}
		}
	}
}
