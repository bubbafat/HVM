using System;
using HVM.Runtime;
using HVM.OpCodes;

namespace HVM.Runtime
{
	/// <summary>
	/// Summary description for ExecutionStackItem.
	/// </summary>
	public class ExecutionStackItem
	{
		VariableItem _variable;

		public ExecutionStackItem(Argument arg)
		{
			_variable = new VariableItem();
			_variable.Value = arg.Value;
			if(arg.Value.Type == HVMType.Variable)
			{
				_variable.Name = arg.Value.StringValue;
			}
		}

		public ExecutionStackItem(VariableItem v)
		{
			_variable = v.Clone() as VariableItem;
		}

		public VariableItem Value
		{
			get
			{
				return _variable;
			}
		}
	}
}
