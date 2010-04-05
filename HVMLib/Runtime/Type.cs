using System;
using System.Collections;

namespace HVM.Runtime
{
	public enum HVMType
	{
		Unknown,
		Boolean,
		Integer,
		String,
		QuotedString,
		Variable,
		VariableName,
		Array
	}

	public class Variant : ICloneable, IComparable
	{
		public HVMType Type;
		string _value;

		public Variant()
		{
			Type = HVMType.Unknown;
		}

		public Variant(int val)
		{
			IntegerValue = val;
		}

		public Variant(string val)
		{
			StringValue = val;
		}

		public Variant(bool val)
		{
			BooleanValue = val;
		}

		public bool BooleanValue
		{
			get
			{
				bool b = Convert.ToBoolean(_value);
				this.Type = HVMType.Boolean;
				return b;
			}
			set
			{
				if(this.Type == HVMType.Unknown)
				{
					this.Type = HVMType.Boolean;
				}
				_value = value.ToString().ToLower();
			}
		}

		public string StringValue
		{
			get
			{
				return _value;
			}
			set
			{
				if(this.Type == HVMType.Unknown)
				{
					this.Type = HVMType.String;
				}
				_value = value;
			}
		}

		public int IntegerValue
		{
			get
			{
				int result = int.MinValue;
				if(_value.EndsWith("h") || _value.EndsWith("H"))
				{
					// could be hex .. could be evil
					string v = _value.Substring(0, _value.Length-1);
					result = int.Parse(v, System.Globalization.NumberStyles.HexNumber);
				}
				else
				{
					result = Convert.ToInt32(_value);
				}

				this.Type = HVMType.Integer;
				return result;
			}
			set
			{
				if(this.Type == HVMType.Unknown)
				{
					this.Type = HVMType.Integer;
				}
				_value = value.ToString();
			}
		}

		public override string ToString()
		{
			return _value.ToString();
		}
		#region ICloneable Members

		public object Clone()
		{
			Variant v = new Variant();
			v._value = _value;
			v.Type = Type;

			return v;
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if(obj == null)
			{
				throw new RuntimeException("Can not compare to a null value");
			}

			Variant v = obj as Variant;

			if(v == null)
			{
				throw new RuntimeException("Item was not convertable to Variant");
			}

			if(Type == HVMType.Integer || v.Type == HVMType.Integer)
			{
				return IntegerValue.CompareTo(v.IntegerValue);
			}

			if(Type == HVMType.Boolean || v.Type == HVMType.Boolean)
			{
				return BooleanValue.CompareTo(v.BooleanValue);
			}

			if(Type == HVMType.String || Type == HVMType.QuotedString)
			{
				return StringValue.CompareTo(v.StringValue);
			}

			if(Type == HVMType.Variable)
			{
				throw new RuntimeException("Can not compare variable types.  Dereference variable before attempting to compare");
			}

			throw new RuntimeException("Attempt to convert a value of unknown type");
		}

		#endregion
	}


	public abstract class Variable
	{
		public string Name;

		#region ICloneable Members

		public abstract object Clone();

		public abstract HVMType Type
		{
			get;
		}

		#endregion
	}

	public class VariableItem : Variable
	{
		public Variant Value;

		public VariableItem()
		{
			Value = new Variant();
		}

		public VariableItem(string name, int val)
		{
			Name = name;
			Value = new Variant(val);
		}

		public VariableItem(string name, string val)
		{
			Name = name;
			Value = new Variant(val);
		}

		public VariableItem(string name, bool val)
		{
			Name = name;
			Value = new Variant(val);
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public override HVMType Type
		{
			get
			{
				return Value.Type;
			}
		}

		public override object Clone()
		{
			VariableItem v = new VariableItem();

			v.Value = this.Value.Clone() as Variant;
			v.Name = this.Name;

			return v;
		}
	}

	public class VariableArray : Variable
	{
		int length;
		ArrayList variables;

		public VariableArray(string name, int len)
		{
			base.Name = name;
			length = len;

			if(length > 0)
			{
				variables = new ArrayList(length);
				for(int i = 0; i < length; i++)
				{
					variables.Add(new VariableItem());
				}
			}
		}

		public int Length
		{
			get
			{
				return length;
			}
		}


		public void SetAtIndex(int idx, VariableItem v)
		{
			if(idx < length && idx >= 0)
			{
				variables[idx] = v.Clone();
				return;
			}

			throw new RuntimeException(string.Format("Array index out of bounds: {0}[{1}]", this.Name, idx));
		}

		public VariableItem GetAtIndex(int idx)
		{
			if(idx < length && idx >= 0)
			{
				return (variables[idx] as VariableItem).Clone() as VariableItem;
			}

			throw new RuntimeException(string.Format("Array index out of bounds: {0}[{1}]", this.Name, idx));
		}

		public override object Clone()
		{
			VariableArray av = new VariableArray(this.Name, length);
			for(int i = 0; i < length; i++)
			{
				av.variables[i] = (variables[i] as VariableItem).Clone();
			}

			return av;
		}

		public override HVMType Type
		{
			get
			{
				return HVMType.Array;
			}
		}

	}
}
