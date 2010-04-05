using System;
using HVM.Parser;
using HVM.Runtime;

namespace HVM.OpCodes
{
	public class Push : OpCode
	{
		public Push()
		{
			_name = "push";
			_argumenttypes = new HVMType[1] { HVMType.Unknown };
			_arguments = new Argument[1];
		}

		public override int ReadArguments(ParseStream strm, LexicalScope scope)
		{
			int read = 0;

			Argument arg = new Argument();

			if(strm.NextTokenCouldBeVariable())
			{
				string vresult = null;
				if(strm.ReadVariableName(ref vresult))
				{
					arg.Value.StringValue = vresult;
					arg.Value.Type = HVMType.Variable;
					read++;
				}
			}
			else
			{
				if(strm.Eof)
				{
					throw new ParseException("Argument to opcode: push", "Unexpected end of file");
				}

				int val = int.MinValue;
				if(strm.ReadNumeric(ref val))
				{
					arg.Value.Type = HVMType.Integer;
					arg.Value.IntegerValue = val;
					read++;
				}
			}

			if(read == 0)
			{
				if(strm.Eof)
				{
					throw new ParseException("Argument to opcode: push", "Unexpected end of file");
				}

				throw new ParseException("Argument to opcode: push", "Unable to read valid data from stream");
			}

			Arguments[0] = arg;

			return read;
		}

		public override void Execute(ExecutionEnvironment environment)
		{
			DemandArgs(1);
			environment.LocalStack.PushItem(new ExecutionStackItem(Arguments[0]));
		}
	}
}
