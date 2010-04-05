using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Lbl : LabelOpCode
	{
		public Lbl()
		{
			_name = ".lbl";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.String };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
		}
	}
}
