using System;
using System.Collections;
using HVM.Runtime;
using HVM.Parser;
using HVM.OpCodes;

namespace HVM.Runtime
{
	/// <summary>
	/// Summary description for OpcodeStream.
	/// </summary>
	public class OpcodeStream
	{
		ArrayList opCodes;
		int InstructionPointer;
		JumpTable _jumpTable;
		Stack currentIP;

		public OpcodeStream(ParseStream strm, ExecutionStack stack)
		{
			_jumpTable = new JumpTable();
			opCodes = new ArrayList();
			currentIP = new Stack();
			Init(strm, stack);
		}

		private void Init(ParseStream strm, ExecutionStack stack)
		{
			InstructionPointer = 0;
			Hashtable ocTypes = InitOpCodes();

			while(true)
			{
				string word = null;
				if(strm.ReadWord(ref word))
				{
					if(ocTypes.ContainsKey(word))
					{
						OpCode oc = OpCode.Create(ocTypes[word] as Type);
						oc.Line = strm.CurrentLine;
						if(oc != null)
						{
							bool addToOpcodes = true;

							oc.ReadArguments(strm, stack.Scope);

							if(oc.GetType().IsSubclassOf(typeof(LabelOpCode)))
							{
								string name = (oc as LabelOpCode).GetName();
								JumpTable.AddIndex(name, opCodes.Count);
								addToOpcodes = false;
							}
							
							if(addToOpcodes)
							{
								opCodes.Add(oc);
							}
						}
						else
						{
							throw new ParseException( string.Format("Activate Opcode: {0}", word),
								string.Format("Unable to activate Opcode: {0}", word));
						}
					}
					else
					{
						throw new ParseException("Valid token", string.Format("Unexpected token found: {0}", word) );
					}
				}
				else
				{
					break;
				}
			}
		}


		public JumpTable JumpTable
		{
			get
			{
				return _jumpTable;
			}
		}

		public int Length
		{
			get
			{
				return opCodes.Count;
			}
		}

		public int Current
		{
			get
			{
				return InstructionPointer;
			}
		}

		public bool EndOfStream
		{
			get
			{
				return Current >= Length;
			}
		}

		public OpCode GetNext()
		{
			if(!EndOfStream)
			{
				return opCodes[InstructionPointer++] as OpCode;
			}

			return null;
		}

		public bool BranchTo(int ip)
		{
			InstructionPointer = ip;
			return !EndOfStream;
		}

		public OpCode BranchToRelative(int ipOffset)
		{
			InstructionPointer += ipOffset;
			return GetNext();
		}

		public void PushCurrentIPAndSet(int NewIP)
		{
			currentIP.Push(InstructionPointer);
			InstructionPointer = NewIP;
		}

		public int PopCurrentIPAndSet()
		{
			InstructionPointer = (int)currentIP.Pop();
			return InstructionPointer;
		}

		Hashtable InitOpCodes()
		{
			Type [] types = new Type[] {
										   typeof(Push),
										   typeof(PrintLn),
										   typeof(DecStr),
										   typeof(DecInt),
										   typeof(Dbg),
										   typeof(Add),
										   typeof(Sub),
										   typeof(Func),
										   typeof(Call),
										   typeof(Ret),
										   typeof(Exit),
										   typeof(Mul),
										   typeof(Div),
										   typeof(Dup),
										   typeof(And),
										   typeof(Pop),
										   typeof(Ceq),
										   typeof(Clt),
										   typeof(Cgt),
										   typeof(Inc),
										   typeof(Brtrue),
										   typeof(Brfalse),
										   typeof(Br),
										   typeof(Lbl),
										   typeof(Ldstr),
										   typeof(Maxstack),
										   typeof(Mod),
										   typeof(Neg),
										   typeof(Xor),
										   typeof(Or),
										   typeof(Not),
										   typeof(ReadLn),
										   typeof(PopN),
										   typeof(Print),
										   typeof(Read),
										   typeof(DecBlk),
										   typeof(LdIdx),
										   typeof(StIdx),
										   typeof(BlkLen),
			};

			Hashtable ocs = new Hashtable(types.Length);

			for(int idx = 0; idx < types.Length; idx++)
			{
				OpCode oc = OpCode.Create(types[idx]);
				ocs[oc.Name]=types[idx];
			}

			return ocs;
		}

	}
}
