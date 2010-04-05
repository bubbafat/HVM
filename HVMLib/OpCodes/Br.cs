using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Br : OpCode
	{
		public Br()
		{
			_name = "br";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.String };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(1);

			int offSet = environment.OpCodes.JumpTable.GetIndex(Arguments[0].Value.StringValue);
			environment.OpCodes.BranchTo(offSet);
		}
	}
}
