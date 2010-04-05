using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public abstract class LabelOpCode : OpCode
	{
		public virtual string GetName()
		{
			return Arguments[0].Value.StringValue;
		}
	}
}
