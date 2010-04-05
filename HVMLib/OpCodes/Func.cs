using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Func : LabelOpCode
	{
		public Func()
		{
			_name = ".func";
			_arguments = new Argument[1];
			_argumenttypes = new HVMType[1]{ HVMType.String };
		}

		public override void Execute(ExecutionEnvironment environment)
		{
		}
	}
}
