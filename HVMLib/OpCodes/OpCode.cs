using System;
using HVM.Runtime;
using HVM.Parser;

namespace HVM.OpCodes
{
	public abstract class OpCode
	{
		protected string _name;
		protected Argument[] _arguments;
		protected HVMType[] _argumenttypes;
		int lineNumber;

		protected OpCode()
		{
			_name = "<unknown>";
			_arguments = new Argument[]{};
			_argumenttypes = new HVMType[]{};
		}

		protected void DemandArgs(int count)
		{
			if(_arguments.Length != count)
			{
				throw new ParseException(
					string.Format("{0} arguments", count), 
					string.Format("{0} arguments", _arguments.Length));
			}
		}

		public virtual int ArgumentCount
		{
			get
			{
				return _argumenttypes.Length;
			}
		}

		public virtual Argument[] Arguments
		{
			get
			{
				return _arguments;
			}
		}

		public virtual HVMType[] ArgumentTypes
		{
			get
			{
				return _argumenttypes;
			}
		}

		public virtual int ReadArguments(ParseStream strm, LexicalScope scope)
		{
			int read = 0;

			for(int i = 0; i < ArgumentCount; i++)
			{
				HVMType type = ArgumentTypes[i];
				Argument arg = new Argument();
				arg.Value.Type = type;
				bool valParse = false;

				switch(type)
				{
					case HVMType.Boolean:
						bool bresult = false;
						if(strm.ReadBoolean(ref bresult))
						{
							arg.Value.BooleanValue = bresult;
							valParse = true;
						}
						break;
					case HVMType.Integer:
						int iresult = int.MinValue;
						if(strm.ReadNumeric(ref iresult))
						{
							arg.Value.IntegerValue = iresult;
							valParse = true;
						}
						break;
					case HVMType.String:
						string sresult = null;
						if(strm.ReadWord(ref sresult))
						{
							arg.Value.StringValue = sresult;
							valParse = true;
						}
						break;
					case HVMType.VariableName:
						string vnresult = null;
						valParse = strm.ReadVariableName(ref vnresult);
						arg.Value.StringValue = vnresult;
						if(!valParse)
						{
							throw new ParseException("Variable name", vnresult);
						}
						break;
					case HVMType.QuotedString:
						string qsresult = null;
						if(strm.ReadQuotedString(ref qsresult))
						{
							arg.Value.StringValue = qsresult;
							valParse = true;
						}
						break;
					case HVMType.Variable:
						string vresult = null;
						valParse = strm.ReadVariableName(ref vresult);
						arg.Value.StringValue = vresult;
						if(!valParse)
						{
							throw new ParseException("word begining with @ (variable name)", vresult);
						}
						break;
				}

				if(!valParse)
				{
					if(strm.Eof)
					{
						throw new ParseException( string.Format("{0} data", type.ToString()),
							"unexpected end of file");
					}

					throw new OpCodeArgumentException(i, type, this);
				}

				Arguments[i] = arg;
				read++;
			}

			return read;
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		public static OpCode Create(System.Type t)
		{
			if(t.IsSubclassOf(typeof(OpCode)))
			{
				return Activator.CreateInstance(t) as OpCode;
			}

			throw new ParseException("Type derived from HVM.OpCode", t.ToString());
		}

		public virtual int Line
		{
			get
			{
				return lineNumber;
			}
			set
			{
				lineNumber = value;
			}
		}

		public abstract void Execute(ExecutionEnvironment environment);

		protected int ReadVariableNameAndIntegerOrVariableInt(ParseStream strm, LexicalScope scope)
		{
			int read = 0;

			Argument argName = new Argument();
			Argument argIndex = new Argument();

			if(strm.NextTokenCouldBeVariable())
			{
				string vresult = null;
				if(strm.ReadVariableName(ref vresult))
				{
					argName.Value.StringValue = vresult;
					argName.Value.Type = HVMType.Variable;
					read++;
				}
			}
			else
			{
				throw new OpCodeArgumentException(0, HVMType.Variable, this);
			}

			if(strm.NextTokenCouldBeVariable())
			{
				string vresult = null;
				if(strm.ReadVariableName(ref vresult))
				{
					argIndex.Value.StringValue = vresult;
					argIndex.Value.Type = HVMType.Variable;
					read++;
				}
			}
			else
			{
				if(strm.Eof)
				{
					throw new ParseException("Argument to opcode: stidx", "Unexpected end of file");
				}
				int val = int.MinValue;
				if(strm.ReadNumeric(ref val))
				{
					argIndex.Value.Type = HVMType.Integer;
					argIndex.Value.IntegerValue = val;
					read++;
				}
			}

			if(read != 2)
			{
				if(strm.Eof)
				{
					throw new ParseException("Argument to opcode: stidx", "Unexpected end of file");
				}

				throw new ParseException("Argument to opcode: stidx", "Unable to read valid data from stream");
			}

			Arguments[0] = argName;
			Arguments[1] = argIndex;

			return read;
		}
	}
}
