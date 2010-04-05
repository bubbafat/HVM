using System;
using System.Collections;
using HVM.Parser;

namespace HVM.Runtime
{
	/// <summary>
	/// Summary description for JumpTable.
	/// </summary>
	public class JumpTable
	{
		Hashtable _jumpTable;

		public JumpTable()
		{
			_jumpTable = new Hashtable();
		}

		public int GetIndex(string label)
		{
			if(_jumpTable.ContainsKey(label))
			{
				return (int)_jumpTable[label];
			}

			throw new RuntimeException( string.Format("Attempt to get index of non-existant label: {0}", label) );
		}

		public void AddIndex(string label, int index)
		{
			if(_jumpTable.ContainsKey(label))
			{
				throw new ParseException("Unique index (attempt made to add duplicate)", label);
			}

			_jumpTable[label]=index;
		}
	}
}
