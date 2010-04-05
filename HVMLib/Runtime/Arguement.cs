using System;
using HVM.Runtime;

namespace HVM.Runtime
{
	public class Argument
	{
		Variant _value;

		public Argument()
		{
			_value = new Variant();
			_value.Type = HVMType.Unknown;
		}

		public Argument(Variant val)
		{
			_value = val;
		}

		public Variant Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
	}
}
