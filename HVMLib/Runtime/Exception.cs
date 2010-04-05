using System;

namespace HVM.Runtime
{
	public class HVMException : ApplicationException
	{
		public HVMException(string msg)
			: base(msg)
		{
		}
	}

	/// <summary>
	/// Summary description for Exception.
	/// </summary>
	public class RuntimeException : HVMException
	{
		public RuntimeException(string msg)
			: base(msg)
		{
		}
	}

	public class StackException : HVMException
	{
		public StackException(string msg)
			: base(msg)
		{
		}
	}
}
