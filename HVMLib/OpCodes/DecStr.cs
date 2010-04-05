using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	/// <summary>
	/// Summary description for DecStr.
	/// </summary>
	public class DecStr : DeclaratorOpCode
	{
		public DecStr()
		{
			_name = "decstr";
			_argumenttypes = new HVMType[2] { HVMType.VariableName, HVMType.QuotedString };
			_arguments = new Argument[2];
		}
	}
}
