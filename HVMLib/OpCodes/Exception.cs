using System;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class OpCodeException : HVMException
	{
		public OpCodeException(string msg, OpCode oc)
			: base( string.Format("OpCode Exception ({0}): {1}  -  OpCode: {2}", oc.Line, msg, oc.Name) )
		{
		}
	}

	public class OpCodeArgumentException : HVMException
	{
		public OpCodeArgumentException(int index, HVMType type, OpCode oc)
			: base( string.Format("OpCode Argument Exception ({0}): Argument {1} must be of type {2} -  OpCode: {3}", 
					oc.Line, index, type.ToString(), oc.Name) )
		{
		}
	}

}
