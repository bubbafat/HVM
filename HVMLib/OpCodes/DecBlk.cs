using System;
using HVM.Runtime;
using HVM.Parser;

namespace HVM.OpCodes
{
	/// <summary>
	/// Summary description for DecStr.
	/// </summary>
	public class DecBlk : DeclaratorOpCode
	{
		public DecBlk()
		{
			_name = "decblk";
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


			int length = -1;
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

				length = vi.Value.IntegerValue;				
			}
			else
			{
				length = Arguments[1].Value.IntegerValue;
			}


			VariableArray va = new VariableArray(name, length);
			environment.LocalScope.Add(va);
		}
	}
}
