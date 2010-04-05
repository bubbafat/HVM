using System;
using System.Collections;
using HVM.Parser;

namespace HVM.Runtime
{
	public class LexicalScope
	{
		LexicalScope _parent;
		Hashtable _symbols;

		public LexicalScope()
			: this(null)
		{
		}

		public LexicalScope(LexicalScope parent)
		{
			_symbols = new Hashtable();
			_parent = parent;
		}

		public void Update(Variable var)
		{
			if(var == null)
			{
				throw new ParseException("Valid  variable", "null variable");
			}

			if(var.Name == null)
			{
				throw new ParseException("Valid  variable", "null named variable");
			}

			if(var.Name.Length == 0)
			{
				throw new ParseException("Valid variable", "non named variable");
			}

			if(var.Type == HVMType.Unknown)
			{
				throw new ParseException("Variable of known type", 
					string.Format("Variable of unknown type: {0}", var.Name));
			}

			LexicalScope ls = GetContainingLexicalScope(var.Name);
			if(ls != null)
			{
				ls._symbols[var.Name]=var.Clone();
			}
		}

		private bool VarIsArray(Variable var)
		{
			return var.Type == HVMType.Array;
		}

		public void Add(Variable var)
		{
			if(var == null)
			{
				throw new ParseException("Valid  variable", "null variable");
			}

			if(var.Name == null)
			{
				throw new ParseException("Valid  variable", "null named variable");
			}

			if(var.Name.Length == 0)
			{
				throw new ParseException("Valid variable", "non named variable");
			}

			if(!VarIsArray(var))
			{
				if((var as VariableItem).Value.Type == HVMType.Unknown)
				{
					throw new ParseException("Variable of known type", 
						string.Format("Variable of unknown type: {0}", var.Name));
				}
			}

			if(_symbols.ContainsKey(var.Name))
			{
				throw new ParseException("Unique variable", string.Format("Variable added to lexical scope twice: {0}", var.Name));
			}

			_symbols[var.Name]=var.Clone();
		}

		public LexicalScope Parent
		{
			get
			{
				return _parent;
			}
		}

		public Variable ResolveLocal(string name)
		{
			if(_symbols.ContainsKey(name))
			{
				return _symbols[name] as Variable;
			}

			return null;
		}

		public Variable ResolveAny(string name)
		{
			Variable v = ResolveLocal(name);
			if(v == null)
			{
				if(_parent != null)
				{
					v = _parent.ResolveAny(name);
				}
			}

			return v;
		}

		public bool IsResolvableLocal(string name)
		{
			if(_symbols.ContainsKey(name))
			{
				return true;
			}

			return false;
		}

		public bool IsResolvableAny(string name)
		{
			bool result = IsResolvableLocal(name);

			if(result == false)
			{
				if(_parent != null)
				{
					result = _parent.IsResolvableAny(name);
				}
			}

			return result;
		}

		public LexicalScope GetContainingLexicalScope(string name)
		{
			if(IsResolvableLocal(name))
			{
				return this;
			}

			if(_parent != null)
			{
				return _parent.GetContainingLexicalScope(name);
			}

			return null;
		}

		public VariableItem ResolveStackItem(ExecutionStackItem item)
		{
			VariableItem v = null;
			if(item.Value.Value.Type == HVMType.Variable)
			{
				v = ResolveAny(item.Value.Name) as VariableItem;
			}
			else
			{
				v = item.Value.Clone() as VariableItem;
			}

			if(v == null)
			{
				throw new ParseException( "Defined variable", string.Format("Undefined variable: {0}", item.Value.Name) );
			}

			return v;
		}
	}
}
