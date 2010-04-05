using System;
using System.IO;
using HVM.OpCodes;
using HVM.Parser;
using HVM.Runtime;

namespace HVMConsole
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class MainClass
	{
		[STAThread]
		public static void Main(string [] args)
		{
			DateTime start = DateTime.Now;

			if(args.Length == 0)
			{
				Console.WriteLine("missing argument: file name");
				return;
			}

			if(!File.Exists(args[0]))
			{
				Console.WriteLine("File not found: {0}", args[0]);
				return;
			}

			try
			{
				ExecutionEnvironment ee;
				using(StreamReader sr = new StreamReader(args[0]))
				{
					ParseStream ps = new ParseStream(sr.BaseStream);
					ee = new ExecutionEnvironment();
					ee.Initialize(ps);
					sr.Close();
				}

				while(!ee.OpCodes.EndOfStream)
				{
					OpCode oc = ee.OpCodes.GetNext();
					oc.Execute(ee);
				}
			}
			catch(HVMException he)
			{
				Console.WriteLine(he.Message);
			}

			TimeSpan ts = DateTime.Now - start;

			Console.WriteLine("Runtime: {0}ms", ts.TotalMilliseconds);

			return;
		}
	}
}
