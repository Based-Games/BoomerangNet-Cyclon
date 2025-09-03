using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace WyrmTale
{
	// Token: 0x02000222 RID: 546
	public class JSON
	{
		// Token: 0x17000244 RID: 580
		public object this[string fieldName]
		{
			get
			{
				if (this.fields.ContainsKey(fieldName))
				{
					return this.fields[fieldName];
				}
				return null;
			}
			set
			{
				if (this.fields.ContainsKey(fieldName))
				{
					this.fields[fieldName] = value;
				}
				else
				{
					this.fields.Add(fieldName, value);
				}
			}
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0000D72A File Offset: 0x0000B92A
		public string ToString(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName))
			{
				return Convert.ToString(this.fields[fieldName]);
			}
			return string.Empty;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0000D754 File Offset: 0x0000B954
		public int ToInt(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName))
			{
				return Convert.ToInt32(this.fields[fieldName]);
			}
			return 0;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0000D77A File Offset: 0x0000B97A
		public float ToFloat(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName))
			{
				return Convert.ToSingle(this.fields[fieldName]);
			}
			return 0f;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
		public bool ToBoolean(string fieldName)
		{
			return this.fields.ContainsKey(fieldName) && Convert.ToBoolean(this.fields[fieldName]);
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x0000D7CA File Offset: 0x0000B9CA
		// (set) Token: 0x06000FAF RID: 4015 RVA: 0x00071000 File Offset: 0x0006F200
		public string serialized
		{
			get
			{
				return JSON._JSON.Serialize(this);
			}
			set
			{
				JSON json = JSON._JSON.Deserialize(value);
				if (json != null)
				{
					this.fields = json.fields;
				}
			}
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0000D7D2 File Offset: 0x0000B9D2
		public JSON ToJSON(string fieldName)
		{
			if (!this.fields.ContainsKey(fieldName))
			{
				this.fields.Add(fieldName, new JSON());
			}
			return (JSON)this[fieldName];
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00071028 File Offset: 0x0006F228
		public T[] ToArray<T>(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName) && this.fields[fieldName] is IEnumerable)
			{
				List<T> list = new List<T>();
				foreach (object obj in (this.fields[fieldName] as IEnumerable))
				{
					if (list is List<string>)
					{
						(list as List<string>).Add(Convert.ToString(obj));
					}
					else if (list is List<int>)
					{
						(list as List<int>).Add(Convert.ToInt32(obj));
					}
					else if (list is List<float>)
					{
						(list as List<float>).Add(Convert.ToSingle(obj));
					}
					else if (list is List<bool>)
					{
						(list as List<bool>).Add(Convert.ToBoolean(obj));
					}
					else if (list is List<Vector2>)
					{
						(list as List<Vector2>).Add((JSON)obj);
					}
					else if (list is List<Vector3>)
					{
						(list as List<Vector3>).Add((JSON)obj);
					}
					else if (list is List<Rect>)
					{
						(list as List<Rect>).Add((JSON)obj);
					}
					else if (list is List<Color>)
					{
						(list as List<Color>).Add((JSON)obj);
					}
					else if (list is List<Color32>)
					{
						(list as List<Color32>).Add((JSON)obj);
					}
					else if (list is List<Quaternion>)
					{
						(list as List<Quaternion>).Add((JSON)obj);
					}
					else if (list is List<JSON>)
					{
						(list as List<JSON>).Add((JSON)obj);
					}
				}
				return list.ToArray();
			}
			return new T[0];
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0000D802 File Offset: 0x0000BA02
		public static implicit operator Vector2(JSON value)
		{
			return new Vector3(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]));
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0007128C File Offset: 0x0006F48C
		public static explicit operator JSON(Vector2 value)
		{
			JSON json = new JSON();
			json["x"] = value.x;
			json["y"] = value.y;
			return json;
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0000D82E File Offset: 0x0000BA2E
		public static implicit operator Vector3(JSON value)
		{
			return new Vector3(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]), Convert.ToSingle(value["z"]));
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x000712D0 File Offset: 0x0006F4D0
		public static explicit operator JSON(Vector3 value)
		{
			JSON json = new JSON();
			json["x"] = value.x;
			json["y"] = value.y;
			json["z"] = value.z;
			return json;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0007132C File Offset: 0x0006F52C
		public static implicit operator Quaternion(JSON value)
		{
			return new Quaternion(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]), Convert.ToSingle(value["z"]), Convert.ToSingle(value["w"]));
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00071380 File Offset: 0x0006F580
		public static explicit operator JSON(Quaternion value)
		{
			JSON json = new JSON();
			json["x"] = value.x;
			json["y"] = value.y;
			json["z"] = value.z;
			json["w"] = value.w;
			return json;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x000713F0 File Offset: 0x0006F5F0
		public static implicit operator Color(JSON value)
		{
			return new Color(Convert.ToSingle(value["r"]), Convert.ToSingle(value["g"]), Convert.ToSingle(value["b"]), Convert.ToSingle(value["a"]));
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00071444 File Offset: 0x0006F644
		public static explicit operator JSON(Color value)
		{
			JSON json = new JSON();
			json["r"] = value.r;
			json["g"] = value.g;
			json["b"] = value.b;
			json["a"] = value.a;
			return json;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x000714B4 File Offset: 0x0006F6B4
		public static implicit operator Color32(JSON value)
		{
			return new Color32(Convert.ToByte(value["r"]), Convert.ToByte(value["g"]), Convert.ToByte(value["b"]), Convert.ToByte(value["a"]));
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00071508 File Offset: 0x0006F708
		public static explicit operator JSON(Color32 value)
		{
			JSON json = new JSON();
			json["r"] = value.r;
			json["g"] = value.g;
			json["b"] = value.b;
			json["a"] = value.a;
			return json;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00071578 File Offset: 0x0006F778
		public static implicit operator Rect(JSON value)
		{
			return new Rect((float)Convert.ToByte(value["left"]), (float)Convert.ToByte(value["top"]), (float)Convert.ToByte(value["width"]), (float)Convert.ToByte(value["height"]));
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x000715D0 File Offset: 0x0006F7D0
		public static explicit operator JSON(Rect value)
		{
			JSON json = new JSON();
			json["left"] = value.xMin;
			json["top"] = value.yMax;
			json["width"] = value.width;
			json["height"] = value.height;
			return json;
		}

		// Token: 0x04001175 RID: 4469
		public Dictionary<string, object> fields = new Dictionary<string, object>();

		// Token: 0x02000223 RID: 547
		private sealed class _JSON
		{
			// Token: 0x06000FBF RID: 4031 RVA: 0x0000D865 File Offset: 0x0000BA65
			public static JSON Deserialize(string json)
			{
				if (json == null)
				{
					return null;
				}
				return JSON._JSON.Parser.Parse(json);
			}

			// Token: 0x06000FC0 RID: 4032 RVA: 0x0000D875 File Offset: 0x0000BA75
			public static string Serialize(JSON obj)
			{
				return JSON._JSON.Serializer.Serialize(obj);
			}

			// Token: 0x02000224 RID: 548
			private sealed class Parser : IDisposable
			{
				// Token: 0x06000FC1 RID: 4033 RVA: 0x0000D87D File Offset: 0x0000BA7D
				private Parser(string jsonString)
				{
					this.json = new StringReader(jsonString);
				}

				// Token: 0x06000FC2 RID: 4034 RVA: 0x00071640 File Offset: 0x0006F840
				public static JSON Parse(string jsonString)
				{
					JSON json;
					using (JSON._JSON.Parser parser = new JSON._JSON.Parser(jsonString))
					{
						json = parser.ParseValue() as JSON;
					}
					return json;
				}

				// Token: 0x06000FC3 RID: 4035 RVA: 0x0000D891 File Offset: 0x0000BA91
				public void Dispose()
				{
					this.json.Dispose();
					this.json = null;
				}

				// Token: 0x06000FC4 RID: 4036 RVA: 0x00071688 File Offset: 0x0006F888
				private JSON ParseObject()
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					JSON json = new JSON();
					json.fields = dictionary;
					this.json.Read();
					for (;;)
					{
						JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
						switch (nextToken)
						{
						case JSON._JSON.Parser.TOKEN.NONE:
							goto IL_44;
						default:
							if (nextToken != JSON._JSON.Parser.TOKEN.COMMA)
							{
								string text = this.ParseString();
								if (text == null)
								{
									goto Block_2;
								}
								if (this.NextToken != JSON._JSON.Parser.TOKEN.COLON)
								{
									goto Block_3;
								}
								this.json.Read();
								dictionary[text] = this.ParseValue();
							}
							break;
						case JSON._JSON.Parser.TOKEN.CURLY_CLOSE:
							return json;
						}
					}
					IL_44:
					return null;
					Block_2:
					return null;
					Block_3:
					return null;
				}

				// Token: 0x06000FC5 RID: 4037 RVA: 0x00071724 File Offset: 0x0006F924
				private List<object> ParseArray()
				{
					List<object> list = new List<object>();
					this.json.Read();
					bool flag = true;
					while (flag)
					{
						JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
						JSON._JSON.Parser.TOKEN token = nextToken;
						switch (token)
						{
						case JSON._JSON.Parser.TOKEN.SQUARED_CLOSE:
							flag = false;
							break;
						default:
						{
							if (token == JSON._JSON.Parser.TOKEN.NONE)
							{
								return null;
							}
							object obj = this.ParseByToken(nextToken);
							list.Add(obj);
							break;
						}
						case JSON._JSON.Parser.TOKEN.COMMA:
							break;
						}
					}
					return list;
				}

				// Token: 0x06000FC6 RID: 4038 RVA: 0x000717A0 File Offset: 0x0006F9A0
				private object ParseValue()
				{
					JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
					return this.ParseByToken(nextToken);
				}

				// Token: 0x06000FC7 RID: 4039 RVA: 0x000717BC File Offset: 0x0006F9BC
				private object ParseByToken(JSON._JSON.Parser.TOKEN token)
				{
					switch (token)
					{
					case JSON._JSON.Parser.TOKEN.CURLY_OPEN:
						return this.ParseObject();
					case JSON._JSON.Parser.TOKEN.SQUARED_OPEN:
						return this.ParseArray();
					case JSON._JSON.Parser.TOKEN.STRING:
						return this.ParseString();
					case JSON._JSON.Parser.TOKEN.NUMBER:
						return this.ParseNumber();
					case JSON._JSON.Parser.TOKEN.TRUE:
						return true;
					case JSON._JSON.Parser.TOKEN.FALSE:
						return false;
					case JSON._JSON.Parser.TOKEN.NULL:
						return null;
					}
					return null;
				}

				// Token: 0x06000FC8 RID: 4040 RVA: 0x00071834 File Offset: 0x0006FA34
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

				// Token: 0x06000FC9 RID: 4041 RVA: 0x000719CC File Offset: 0x0006FBCC
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

				// Token: 0x06000FCA RID: 4042 RVA: 0x0000D8A5 File Offset: 0x0000BAA5
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

				// Token: 0x17000246 RID: 582
				// (get) Token: 0x06000FCB RID: 4043 RVA: 0x0000D8E4 File Offset: 0x0000BAE4
				private char PeekChar
				{
					get
					{
						return Convert.ToChar(this.json.Peek());
					}
				}

				// Token: 0x17000247 RID: 583
				// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0000D8F6 File Offset: 0x0000BAF6
				private char NextChar
				{
					get
					{
						return Convert.ToChar(this.json.Read());
					}
				}

				// Token: 0x17000248 RID: 584
				// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00071A10 File Offset: 0x0006FC10
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

				// Token: 0x17000249 RID: 585
				// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00071A68 File Offset: 0x0006FC68
				private JSON._JSON.Parser.TOKEN NextToken
				{
					get
					{
						this.EatWhitespace();
						if (this.json.Peek() == -1)
						{
							return JSON._JSON.Parser.TOKEN.NONE;
						}
						char peekChar = this.PeekChar;
						char c = peekChar;
						switch (c)
						{
						case '"':
							return JSON._JSON.Parser.TOKEN.STRING;
						default:
							switch (c)
							{
							case '[':
								return JSON._JSON.Parser.TOKEN.SQUARED_OPEN;
							default:
							{
								switch (c)
								{
								case '{':
									return JSON._JSON.Parser.TOKEN.CURLY_OPEN;
								case '}':
									this.json.Read();
									return JSON._JSON.Parser.TOKEN.CURLY_CLOSE;
								}
								string nextWord = this.NextWord;
								string text = nextWord;
								switch (text)
								{
								case "false":
									return JSON._JSON.Parser.TOKEN.FALSE;
								case "true":
									return JSON._JSON.Parser.TOKEN.TRUE;
								case "null":
									return JSON._JSON.Parser.TOKEN.NULL;
								}
								return JSON._JSON.Parser.TOKEN.NONE;
							}
							case ']':
								this.json.Read();
								return JSON._JSON.Parser.TOKEN.SQUARED_CLOSE;
							}
							break;
						case ',':
							this.json.Read();
							return JSON._JSON.Parser.TOKEN.COMMA;
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
							return JSON._JSON.Parser.TOKEN.NUMBER;
						case ':':
							return JSON._JSON.Parser.TOKEN.COLON;
						}
					}
				}

				// Token: 0x04001176 RID: 4470
				private const string WHITE_SPACE = " \t\n\r";

				// Token: 0x04001177 RID: 4471
				private const string WORD_BREAK = " \t\n\r{}[],:\"";

				// Token: 0x04001178 RID: 4472
				private StringReader json;

				// Token: 0x02000225 RID: 549
				private enum TOKEN
				{
					// Token: 0x0400117B RID: 4475
					NONE,
					// Token: 0x0400117C RID: 4476
					CURLY_OPEN,
					// Token: 0x0400117D RID: 4477
					CURLY_CLOSE,
					// Token: 0x0400117E RID: 4478
					SQUARED_OPEN,
					// Token: 0x0400117F RID: 4479
					SQUARED_CLOSE,
					// Token: 0x04001180 RID: 4480
					COLON,
					// Token: 0x04001181 RID: 4481
					COMMA,
					// Token: 0x04001182 RID: 4482
					STRING,
					// Token: 0x04001183 RID: 4483
					NUMBER,
					// Token: 0x04001184 RID: 4484
					TRUE,
					// Token: 0x04001185 RID: 4485
					FALSE,
					// Token: 0x04001186 RID: 4486
					NULL
				}
			}

			// Token: 0x02000226 RID: 550
			private sealed class Serializer
			{
				// Token: 0x06000FCF RID: 4047 RVA: 0x0000D908 File Offset: 0x0000BB08
				private Serializer()
				{
					this.builder = new StringBuilder();
				}

				// Token: 0x06000FD0 RID: 4048 RVA: 0x00071BEC File Offset: 0x0006FDEC
				public static string Serialize(JSON obj)
				{
					JSON._JSON.Serializer serializer = new JSON._JSON.Serializer();
					serializer.SerializeValue(obj);
					return serializer.builder.ToString();
				}

				// Token: 0x06000FD1 RID: 4049 RVA: 0x00071C14 File Offset: 0x0006FE14
				private void SerializeValue(object value)
				{
					if (value == null)
					{
						this.builder.Append("null");
					}
					else if (value is string)
					{
						this.SerializeString(value as string);
					}
					else if (value is bool)
					{
						this.builder.Append(value.ToString().ToLower());
					}
					else if (value is JSON)
					{
						this.SerializeObject(value as JSON);
					}
					else if (value is IDictionary)
					{
						this.SerializeDictionary(value as IDictionary);
					}
					else if (value is IList)
					{
						this.SerializeArray(value as IList);
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

				// Token: 0x06000FD2 RID: 4050 RVA: 0x0000D91B File Offset: 0x0000BB1B
				private void SerializeObject(JSON obj)
				{
					this.SerializeDictionary(obj.fields);
				}

				// Token: 0x06000FD3 RID: 4051 RVA: 0x00071CF8 File Offset: 0x0006FEF8
				private void SerializeDictionary(IDictionary obj)
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

				// Token: 0x06000FD4 RID: 4052 RVA: 0x00071DAC File Offset: 0x0006FFAC
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

				// Token: 0x06000FD5 RID: 4053 RVA: 0x00071E3C File Offset: 0x0007003C
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

				// Token: 0x06000FD6 RID: 4054 RVA: 0x00071FB4 File Offset: 0x000701B4
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

				// Token: 0x04001187 RID: 4487
				private StringBuilder builder;
			}
		}
	}
}
