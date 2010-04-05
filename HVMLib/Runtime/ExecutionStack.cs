using System;
using System.Collections;
using HVM.OpCodes;

namespace HVM.Runtime
{
	public class ExecutionStack : Stack
	{
		LexicalScope _scope;
		int _maxsize = short.MaxValue;

		public ExecutionStack(LexicalScope scope)
		{
			_scope = scope;
		}

		public LexicalScope Scope
		{
			get
			{
				return _scope;
			}
		}

		public override void Push(object obj)
		{
			if(MaxSize > Count)
			{
				base.Push (obj);
			}
			else
			{
				throw new StackException("Stack overflow - MaxSize exceeded.  Increase max size or use less stack.");
			}
		}

		public override object Pop()
		{
			try
			{
				return base.Pop ();
			}
			catch(InvalidOperationException)
			{
				throw new StackException("Stack underflow");
			}
		}

		public ExecutionStackItem PopItem()
		{
			object obj = Pop();
			if(obj == null)
			{
				throw new StackException("null object on ExecutionStack");
			}

			ExecutionStackItem esi = obj as ExecutionStackItem;
			if(esi == null)
			{
				throw new StackException("Could not convert stack item to ExecutionStackItem");
			}

			return esi;
		}

		public void PushItem(ExecutionStackItem item)
		{
			if(item == null)
			{
				throw new StackException("Can not push null item onto ExecutionStack");
			}

			Push(item);
		}

		public int MaxSize
		{
			get
			{
				return _maxsize;
			}
			set
			{
				if(value < this.Count)
				{
					_maxsize = Count;
				}
				else
				{
					_maxsize = value;
				}
			}
		}

		public void DemandSize(int count, OpCode oc)
		{
			if(this.Count < count)
			{
				throw new StackException( string.Format("not enough data on the stack to execute opcode: {0}", oc.Name));
			}
		}
	}
}
