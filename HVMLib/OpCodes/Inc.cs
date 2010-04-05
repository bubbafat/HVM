using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Inc : OpCode
	{
		public Inc()
		{
			_name = "inc";
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			environment.LocalStack.DemandSize(1, this);

			ExecutionStackItem esi = environment.LocalStack.PopItem();
			if(esi.Value.Value.Type == HVMType.Variable)
			{
				VariableItem var = environment.LocalScope.ResolveStackItem(esi);
				if(var != null)
				{
					try
					{
						var.Value.IntegerValue++;
					}
					catch(Exception)
					{
						throw new RuntimeException("Opcode can only be used on integer types (or types convertable to int): inc");
					}
					environment.LocalScope.Update(var);
					environment.LocalStack.PushItem(esi);
				}
				else
				{
					throw new RuntimeException( string.Format("Unable to locate variable named: {0}", esi.Value.Name) );
				}
			}
			else
			{
				try
				{
					esi.Value.Value.IntegerValue++;
				}
				catch(Exception)
				{
					throw new RuntimeException("Opcode can only be used on integer types (or types convertable to int): inc");
				}

				environment.LocalStack.PushItem(esi);
			}
		}
	}
}
