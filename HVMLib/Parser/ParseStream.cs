using System;
using System.IO;
using System.Text;

namespace HVM.Parser
{
	public class ParseStream : StreamReader
	{
		StringBuilder sbTemp;
		int currentLine = 0;

		static string hexDigits = "abcdefABCDEF0123456789";

		public ParseStream(Stream strm)
			: base(strm)
		{
			sbTemp = new StringBuilder(1024);
		}

		public bool Eof
		{
			get
			{
				return Peek() == -1;
			}
		}

		public char ReadChar()
		{
			if(Eof)
			{
				return char.MinValue;
			}

			return Convert.ToChar(Read());
		}

		private bool SkipWhiteSpace()
		{
			while(!Eof)
			{
				char c = Convert.ToChar(Peek());
				if(c == '\n')
				{
					currentLine++;
				}

				if(char.IsWhiteSpace(c))
				{
					Read();
					continue;
				}

				break;
			}

			return !Eof;
		}

		public bool NextCouldBeNumeric()
		{
			return NextIsDigit() || NextIsOneOf("-+");
		}

		private bool NextIsDigit()
		{
			if(Eof)
			{
				return false;
			}

			return char.IsNumber(Convert.ToChar(Peek()));
		}

		private bool NextIsLetter()
		{
			if(Eof)
			{
				return false;
			}

			return char.IsLetter(Convert.ToChar(Peek()));
		}

		private bool NextIsHexDigit()
		{
			return NextIsOneOf(ParseStream.hexDigits);
		}

		private bool NextIsOneOf(string s)
		{
			if(Eof)
			{
				return false;
			}

			return s.IndexOf(Convert.ToChar(Peek())) != -1;
		}

		public bool NextTokenCouldBeVariable()
		{
			if(SkipWhiteSpace())
			{
				return Convert.ToChar(Peek()) == '@';
			}
			return false;
		}

		private bool NextIsAcceptVariableTrailingCharacter()
		{
			return NextIsDigit() && NextIsLetter() && NextIsOneOf("_");
		}

		private bool NextIsAcceptVariableStartingCharacter()
		{
			return NextIsLetter() && NextIsOneOf("_");
		}

		private bool NextIsWhiteSpace()
		{
			if(Eof)
			{
				return false;
			}

			char c = Convert.ToChar(Peek());
			return char.IsWhiteSpace(c);
		}

		public char PeekNextChar_SkipWS()
		{
			if(SkipWhiteSpace())
			{
				return Convert.ToChar(Peek());
			}

			return char.MinValue;
		}

		public bool ReadWord(ref string word)
		{
			sbTemp.Length = 0;

			while(!Eof)
			{
				if(SkipWhiteSpace())
				{
					char c = Convert.ToChar(Read());
					if(c == ';')
					{
						ReadLine();
						currentLine++;
						continue;
					}
					else
					{
						sbTemp.Append(c);
					}

					if(NextIsWhiteSpace())
						break;
				}
			}

			if(sbTemp.Length == 0)
			{
				if(Eof)
				{
					return false;
				}

				throw new ParseException("Token data", "Unable to parse next token.  Not Eof but unable to read data.");
			}

			word = sbTemp.ToString();
			return true;
		}

		public bool ReadBoolean(ref bool boolean)
		{
			string word = null;
			if(ReadWord(ref word))
			{
				switch(word)
				{
					case "true":
						boolean = true;
						break;
					case "false":
						boolean = false;
						break;
					default:
						throw new ParseException( "Boolean value", word );
				}

				return true;
			}

			return false;
		}

		public bool ReadNumeric(ref int val)
		{
			string word = null;
			if(ReadWord(ref word))
			{
				if(word[word.Length - 1] == 'h' || 
					word[word.Length - 1] == 'H')
				{
					string temp = word.Substring(0, word.Length-1);
					
					try
					{
						val = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
					}
					catch(FormatException)
					{
						throw new ParseException("Hex value", word);
					}
					catch(OverflowException)
					{
						throw new ParseException("Integer value overflow.", word);
					}

					return true;
				}
				else
				{
					try
					{
						val = int.Parse(word, System.Globalization.NumberStyles.Integer);
					}
					catch(FormatException)
					{
						throw new ParseException("Integer value", word);
					}
					catch(OverflowException)
					{
						throw new ParseException("Integer value overflow.", word);
					}

					return true;
				}
			}

			return false;
		}

		public bool ReadQuotedString(ref string str)
		{
			sbTemp.Length = 0;
			SkipWhiteSpace();

			if(Eof)
			{
				return false;
			}

			if(Convert.ToChar(Peek()) != '\"')
			{
				throw new ParseException("Double quote", Convert.ToChar(Peek()).ToString());
			}

			Read();

			while(!Eof && Peek() != '\"')
			{
				char c = Convert.ToChar(Read());
				if(c == '\n' || c == '\r')
				{
					currentLine++;
					throw new ParseException("Character data", "Newline in string literal");
				}

				sbTemp.Append(c);
			}

			if(Peek() == '\"')
			{
				Read();
			}

			str = sbTemp.ToString();
			return true;
		}

		public bool ReadVariableName(ref string var)
		{
			if(NextTokenCouldBeVariable())
			{
				Read();
				return ReadWord(ref var);
			}
			
			return false;
		}

		public int CurrentLine
		{
			get
			{
				return currentLine;
			}
		}
	}
}
