using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiniJSON
{
	// Token: 0x02000227 RID: 551
	public static class Json
	{
		// Token: 0x06000FD7 RID: 4055 RVA: 0x0000D929 File Offset: 0x0000BB29
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0000D939 File Offset: 0x0000BB39
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x02000228 RID: 552
		private sealed class Parser : IDisposable
		{
			// Token: 0x06000FD9 RID: 4057 RVA: 0x0000D941 File Offset: 0x0000BB41
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06000FDA RID: 4058 RVA: 0x00072060 File Offset: 0x00070260
			public static object Parse(string jsonString)
			{
				object obj;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					obj = parser.ParseValue();
				}
				return obj;
			}

			// Token: 0x06000FDB RID: 4059 RVA: 0x0000D955 File Offset: 0x0000BB55
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x06000FDC RID: 4060 RVA: 0x000720A4 File Offset: 0x000702A4
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case Json.Parser.TOKEN.NONE:
						goto IL_37;
					default:
						if (nextToken != Json.Parser.TOKEN.COMMA)
						{
							string text = this.ParseString();
							if (text == null)
							{
								goto Block_2;
							}
							if (this.NextToken != Json.Parser.TOKEN.COLON)
							{
								goto Block_3;
							}
							this.json.Read();
							dictionary[text] = this.ParseValue();
						}
						break;
					case Json.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
				}
				IL_37:
				return null;
				Block_2:
				return null;
				Block_3:
				return null;
			}

			// Token: 0x06000FDD RID: 4061 RVA: 0x00072130 File Offset: 0x00070330
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					Json.Parser.TOKEN token = nextToken;
					switch (token)
					{
					case Json.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						break;
					default:
					{
						if (token == Json.Parser.TOKEN.NONE)
						{
							return null;
						}
						object obj = this.ParseByToken(nextToken);
						list.Add(obj);
						break;
					}
					case Json.Parser.TOKEN.COMMA:
						break;
					}
				}
				return list;
			}

			// Token: 0x06000FDE RID: 4062 RVA: 0x000721AC File Offset: 0x000703AC
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x06000FDF RID: 4063 RVA: 0x000721C8 File Offset: 0x000703C8
			private object ParseByToken(Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case Json.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case Json.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case Json.Parser.TOKEN.STRING:
					return this.ParseString();
				case Json.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case Json.Parser.TOKEN.TRUE:
					return true;
				case Json.Parser.TOKEN.FALSE:
					return false;
				case Json.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			// Token: 0x06000FE0 RID: 4064 RVA: 0x00072240 File Offset: 0x00070440
			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char c = this.NextChar;
					char c2 = c;
					if (c2 != '"')
					{
						if (c2 != '\\')
						{
							stringBuilder.Append(c);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							c = this.NextChar;
							char c3 = c;
							switch (c3)
							{
							case 'n':
								stringBuilder.Append('\n');
								break;
							default:
								if (c3 != '"' && c3 != '/' && c3 != '\\')
								{
									if (c3 != 'b')
									{
										if (c3 == 'f')
										{
											stringBuilder.Append('\f');
										}
									}
									else
									{
										stringBuilder.Append('\b');
									}
								}
								else
								{
									stringBuilder.Append(c);
								}
								break;
							case 'r':
								stringBuilder.Append('\r');
								break;
							case 't':
								stringBuilder.Append('\t');
								break;
							case 'u':
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								for (int i = 0; i < 4; i++)
								{
									stringBuilder2.Append(this.NextChar);
								}
								stringBuilder.Append((char)Convert.ToInt32(stringBuilder2.ToString(), 16));
								break;
							}
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06000FE1 RID: 4065 RVA: 0x000723D8 File Offset: 0x000705D8
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			// Token: 0x06000FE2 RID: 4066 RVA: 0x0000D969 File Offset: 0x0000BB69
			private void EatWhitespace()
			{
				while (" \t\n\r".IndexOf(this.PeekChar) != -1)
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x1700024A RID: 586
			// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0000D9A8 File Offset: 0x0000BBA8
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0000D9BA File Offset: 0x0000BBBA
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x1700024C RID: 588
			// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0007241C File Offset: 0x0007061C
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (" \t\n\r{}[],:\"".IndexOf(this.PeekChar) == -1)
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x1700024D RID: 589
			// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00072474 File Offset: 0x00070674
			private Json.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return Json.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					char c = peekChar;
					switch (c)
					{
					case '"':
						return Json.Parser.TOKEN.STRING;
					default:
						switch (c)
						{
						case '[':
							return Json.Parser.TOKEN.SQUARED_OPEN;
						default:
						{
							switch (c)
							{
							case '{':
								return Json.Parser.TOKEN.CURLY_OPEN;
							case '}':
								this.json.Read();
								return Json.Parser.TOKEN.CURLY_CLOSE;
							}
							string nextWord = this.NextWord;
							string text = nextWord;
							switch (text)
							{
							case "false":
								return Json.Parser.TOKEN.FALSE;
							case "true":
								return Json.Parser.TOKEN.TRUE;
							case "null":
								return Json.Parser.TOKEN.NULL;
							}
							return Json.Parser.TOKEN.NONE;
						}
						case ']':
							this.json.Read();
							return Json.Parser.TOKEN.SQUARED_CLOSE;
						}
						break;
					case ',':
						this.json.Read();
						return Json.Parser.TOKEN.COMMA;
					case '-':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						return Json.Parser.TOKEN.NUMBER;
					case ':':
						return Json.Parser.TOKEN.COLON;
					}
				}
			}

			// Token: 0x04001188 RID: 4488
			private const string WHITE_SPACE = " \t\n\r";

			// Token: 0x04001189 RID: 4489
			private const string WORD_BREAK = " \t\n\r{}[],:\"";

			// Token: 0x0400118A RID: 4490
			private StringReader json;

			// Token: 0x02000229 RID: 553
			private enum TOKEN
			{
				// Token: 0x0400118D RID: 4493
				NONE,
				// Token: 0x0400118E RID: 4494
				CURLY_OPEN,
				// Token: 0x0400118F RID: 4495
				CURLY_CLOSE,
				// Token: 0x04001190 RID: 4496
				SQUARED_OPEN,
				// Token: 0x04001191 RID: 4497
				SQUARED_CLOSE,
				// Token: 0x04001192 RID: 4498
				COLON,
				// Token: 0x04001193 RID: 4499
				COMMA,
				// Token: 0x04001194 RID: 4500
				STRING,
				// Token: 0x04001195 RID: 4501
				NUMBER,
				// Token: 0x04001196 RID: 4502
				TRUE,
				// Token: 0x04001197 RID: 4503
				FALSE,
				// Token: 0x04001198 RID: 4504
				NULL
			}
		}

		// Token: 0x0200022A RID: 554
		private sealed class Serializer
		{
			// Token: 0x06000FE7 RID: 4071 RVA: 0x0000D9CC File Offset: 0x0000BBCC
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06000FE8 RID: 4072 RVA: 0x000725F8 File Offset: 0x000707F8
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06000FE9 RID: 4073 RVA: 0x00072620 File Offset: 0x00070820
			private void SerializeValue(object value)
			{
				string text;
				IList list;
				IDictionary dictionary;
				if (value == null)
				{
					this.builder.Append("null");
				}
				else if ((text = value as string) != null)
				{
					this.SerializeString(text);
				}
				else if (value is bool)
				{
					this.builder.Append(value.ToString().ToLower());
				}
				else if ((list = value as IList) != null)
				{
					this.SerializeArray(list);
				}
				else if ((dictionary = value as IDictionary) != null)
				{
					this.SerializeObject(dictionary);
				}
				else if (value is char)
				{
					this.SerializeString(value.ToString());
				}
				else
				{
					this.SerializeOther(value);
				}
			}

			// Token: 0x06000FEA RID: 4074 RVA: 0x000726E0 File Offset: 0x000708E0
			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				foreach (object obj2 in obj.Keys)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeString(obj2.ToString());
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
				this.builder.Append('}');
			}

			// Token: 0x06000FEB RID: 4075 RVA: 0x00072794 File Offset: 0x00070994
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				foreach (object obj in anArray)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(obj);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x06000FEC RID: 4076 RVA: 0x00072824 File Offset: 0x00070A24
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				foreach (char c in array)
				{
					char c2 = c;
					switch (c2)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					default:
						if (c2 != '"')
						{
							if (c2 != '\\')
							{
								int num = Convert.ToInt32(c);
								if (num >= 32 && num <= 126)
								{
									this.builder.Append(c);
								}
								else
								{
									this.builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
								}
							}
							else
							{
								this.builder.Append("\\\\");
							}
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					}
				}
				this.builder.Append('"');
			}

			// Token: 0x06000FED RID: 4077 RVA: 0x0007299C File Offset: 0x00070B9C
			private void SerializeOther(object value)
			{
				if (value is float || value is int || value is uint || value is long || value is double || value is sbyte || value is byte || value is short || value is ushort || value is ulong || value is decimal)
				{
					this.builder.Append(value.ToString());
				}
				else
				{
					this.SerializeString(value.ToString());
				}
			}

			// Token: 0x04001199 RID: 4505
			private StringBuilder builder;
		}
	}
}
