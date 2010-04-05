using System;
using System.Collections;
using HVM.Parser;

namespace HVM.Runtime
{
	public class ExecutionEnvironment
	{
		Stack executionStacks;
		LexicalScope globalLexicalScope;
		OpcodeStream opcodeStream;

		public ExecutionEnvironment()
		{
			executionStacks = new Stack();
		}

		public void Initialize(ParseStream strm)
		{
			if(strm == null)
			{
				throw new ArgumentNullException("strm", "Can not initialize from a null stream");
			}

			globalLexicalScope = new LexicalScope(null);
			globalLexicalScope.Add(new VariableItem("true", true));
			globalLexicalScope.Add(new VariableItem("false", false));

			executionStacks.Push(new ExecutionStack(globalLexicalScope));
			
			opcodeStream = new OpcodeStream(strm, LocalStack);
		}

		public void PushStack(ExecutionStack stack)
		{
			executionStacks.Push(stack);
		}

		public ExecutionStack PopStack()
		{
			return executionStacks.Pop() as ExecutionStack;
		}

		public OpcodeStream OpCodes
		{
			get
			{
				return opcodeStream;
			}
		}

		public ExecutionStack LocalStack
		{
			get
			{
				return executionStacks.Peek() as ExecutionStack;
			}
		}

		public LexicalScope GlobalScope
		{
			get
			{
				return globalLexicalScope;
			}
		}

		public LexicalScope LocalScope
		{
			get
			{
				return LocalStack.Scope;
			}
		}
	}
}
