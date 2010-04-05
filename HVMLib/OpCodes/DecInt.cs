using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	/// <summary>
	/// Summary description for DecStr.
	/// </summary>
	public class DecInt : DeclaratorOpCode
	{
		public DecInt()
		{
			_name = "decint";
			_argumenttypes = new HVMType[2] { HVMType.VariableName, HVMType.Integer };
			_arguments = new Argument[2];
		}
	}
}
