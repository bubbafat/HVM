using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	/// <summary>
	/// Summary description for DecStr.
	/// </summary>
	public class LdIdx : OpCode
	{
		public LdIdx()
		{
			_name = "ldidx";
			_argumenttypes = new HVMType[2] { HVMType.VariableName, HVMType.Integer };
			_arguments = new Argument[2];
		}

		public override int ReadArguments(ParseStream strm, LexicalScope scope)
		{
			return this.ReadVariableNameAndIntegerOrVariableInt(strm, scope);
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(2);

			string name = Arguments[0].Value.StringValue;
			int index = -1;
			if(Arguments[1].Value.Type == HVMType.Variable)
			{
				Variable v = environment.LocalScope.ResolveAny(Arguments[1].Value.StringValue);
				if(v == null)
				{
					throw new OpCodeArgumentException(1, HVMType.Variable, this);
				}

				VariableItem vi = v as VariableItem;
				if(vi == null)
				{
					throw new OpCodeArgumentException(1, HVMType.Variable, this);
				}

				index = vi.Value.IntegerValue;				
			}
			else
			{
				index = Arguments[1].Value.IntegerValue;
			}

			if(index < 0)
			{
				throw new OpCodeException("index parameter must be positive", this);
			}

			Variable vName = environment.LocalScope.ResolveAny(name);

			if(vName == null)
			{
				throw new OpCodeException(string.Format("Variable not found: {0}", name), this);
			}

			if(vName.Type != HVMType.Array)
			{
				throw new OpCodeException( string.Format("Attempt to index into non-array element: {0}", name), this);
			}

			VariableArray va = vName as VariableArray;

			if(va == null)
			{
				throw new OpCodeException( string.Format("Variable was not convertable to VariableArray: {0}", name), this);
			}

			ExecutionStackItem esi = new ExecutionStackItem(va.GetAtIndex(index));
			environment.LocalStack.PushItem(esi);
		}
	}
}
