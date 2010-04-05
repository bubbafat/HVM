using System;

namespace HVM.Parser
{
	public class ParseException : HVM.Runtime.HVMException
	{
		string expectedDescription;
		string actuallyFound;

		public ParseException(string expected, string found)
			: base( string.Format("Error parsing.  Expected: {0}  -  Found: {1}", expected, found))
		{
			this.expectedDescription = expected;
			this.actuallyFound = found;
		}

		public string ExpectedDescription
		{
			get
			{
				return this.expectedDescription;
			}
		}

		public string ActuallyFound
		{
			get
			{
				return this.actuallyFound;
			}
		}
	}
}
