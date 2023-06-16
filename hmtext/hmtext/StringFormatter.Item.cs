using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace hmlib
{
	public partial class StringFormatter
	{
		internal class Item
		{
			readonly string[] _split;
			readonly public int Index;
			readonly public string Text;
			readonly public string[] PropertyNames;
			readonly public string Format;
			readonly public bool IsArg;
			readonly public Type Type;
			readonly public int? Spacing;
			//------
			public int Length
			{
				get { return Text.Length + (IsArg ? 2 : 0); }
			}
			public Item(string text)
			{
				this._split = null;
				this.Text = text;
				this.IsArg = false;
			}
			public Item(string text, string[] split)
			{
				this.Text = text;
				this._split = split;
				var l = (string)null;
				if (split.Length > 0)
				{
					l = split[0];
					int index;
					this.Spacing = StringFormatter.GetSpacing(split[0], out index);
					if (this.Spacing != null) l = l.SplitAt(index)[0];// remove Spacing()
					GetIndexProperty(IgnoreType(l), out Index, out PropertyNames);
					this.IsArg = true;
					this.Type = GetType(l);
				}
				//		{Index.Property} or {Index} or {Property}
				if (split.Length == 1)
				{
					return;
				}
				//		{Index.Property:Format}
				//or	{Property:Format}
				//or	{(Type)Index.Property:Format}
				//or	{(Type)Property:Format}
				if (split.Length == 2)
				{
					if (StringFormatter.IsMultiPart(l) || StringFormatter.IsCast(l))
					{
						this.Format = split[1];
						return;
					}
				}
				throw new FormatException();
			}

			Type GetType(string left)
			{
				if (left.Count(a => a.In('(', ')')) == 0) return null;
				if (left.Count(a => a.In('(')) != 1) throw new FormatException();
				if (left.Count(a => a.In(')')) != 1) throw new FormatException();
				var typeName = Regex.Match(left, @"(?<=\().+?(?=\))").Value;
				var type = Type.GetType(typeName);
				if (!typeName.IsNullOrEmpty() && type == null)
					throw new FormatException("Undefined Type '" + typeName + "'");
				return type;
			}

			private string IgnoreType(string left)
			{
				if (left.Count(a => a.In('(', ')')) == 0) return left;
				if (left.Count(a => a.In('(')) != 1) throw new FormatException();
				if (left.Count(a => a.In(')')) != 1) throw new FormatException();
				if (left.IndexOf(')') > left.Length - 2) throw new FormatException();
				return left.Substring(left.IndexOf(')') + 1);
			}

			void GetIndexProperty(string left, out int index, out string[] propertyNames)
			{
				var l_split = SplitFirst(left, '.');
				int? temp_index = null;
				if (l_split.Length > 0) temp_index = l_split[0].TryParseAsInt32(null);
				// {[Property]}
				if (l_split.Length == 2 && temp_index == null)
				{
					index = 0;
					propertyNames = left.Split('.');
					return;
				}
				// {Index.[Property]}
				if (l_split.Length == 2 && temp_index != null)
				{
					index = int.Parse(l_split[0]);
					propertyNames = l_split[1].Split('.');
					return;
				}
				// {Index} or {Property}
				if (l_split.Length == 1)
				{
					if (temp_index != null)
					{
						index = temp_index.Value;
						propertyNames = null;
						return;
					}
					if (l_split[0].Contains('.'))
					{
						index = 0;
						propertyNames = l_split[0].Split('.');
						return;
					}
				}
				index = 0;
				propertyNames = l_split;
			}

			internal static Item NewArg(string s)
			{
				if (s.Length == 0) throw new FormatException();
				var split_colon = Item.SplitFirst(s, ':');
				return new Item(s, split_colon);
			}

			private static string[] SplitFirst(string str, char c)
			{
				var index = str.IndexOf(c);
				if (index == -1)
					return new[] { str };
				else
					return new string[] { str.Take(index).JoinToString(), str.Skip(index + 1).ToArray().JoinToString() };
			}

			internal static Item NewText(string s)
			{
				return new Item(s);
			}

			public override string ToString()
			{
				if (!IsArg) return this.Text;
				return "{" + this.Text + "}";
			}

			internal static Item[] Parse(string format)
			{
				if (format.IsNullOrEmpty()) return new Item[] { };
				//--------------- Handle _____{{__
				var index_0 = format.IndexOf("{{");
				if (index_0 >= 0)
				{
					var l = format.Substring(0, index_0);
					var r = format.Substring(index_0 + 2);
					return new[] { Item.NewText(l), Item.NewText("{{") }.Where(a => !a.Text.IsNullOrEmpty()).Append(Item.Parse(r)).ToArray();
				}
				//--------------- Handle _____}}__
				var index_1 = format.LastIndexOf("}}");
				if (index_1 >= 0)
				{
					var l = format.Substring(0, index_1);
					var r = format.Substring(index_1 + 2);
					return Item.Parse(l).Append(new[] { Item.NewText("}}") }).Append(Item.Parse(r)).ToArray();
				}
				//--------------- Handle __{__}
				var index_3 = format.IndexOf("{");
				if (index_3 > 0)
				{
					var l = format.Substring(0, index_3);
					var r = format.Substring(index_3);
					return new[] { Item.NewText(l) }.Append(Item.Parse(r)).ToArray();
				}
				//--------------- Handle {__}
				var index_4 = format.IndexOf('}');
				if (format.StartsWith("{") && index_4 > 0)
				{
					var c = format.Substring(1, index_4 - 1);
					var r = format.Substring(index_4 + 1);
					return new[] { Item.NewArg(c) }.Append(Item.Parse(r)).ToArray();
				}
				return new[] { Item.NewText(format) };
			}

			/*
			internal static Item[] Parse(string format)
			{
				var regex = new Regex("{([^}]*)}", RegexOptions.Compiled);
				var matches = regex.Matches(format).Cast<Match>().ToArray();
				var split = regex.Split(format).Where(a => !a.IsNullOrEmpty());
				var list = new List<Item>();
				foreach (var s in split)
				{
					var len = list.Sum(a => a.Length);
					var matchFound = false;
					foreach (var match in matches)
					{
						if (match.Index == len)
						{
							var sub = format.Substring(match.Index, match.Length);
							list.Add(Item.NewArg(s));
							matchFound = true;
							break;
						}
						if (match.Index > len) break;
					}
					if (!matchFound && !s.IsNullOrEmpty())
						list.Add(Item.NewText(s));
				}
				return list.ToArray();
			}
			*/

			public string GetText(int index)
			{
				if (!IsArg) return this.Text;
				var list = new List<object>();
				var spacing = "";
				if (Spacing.HasValue) spacing = "," + Spacing;
				list.Add(index + spacing);
				if (!Format.IsNullOrEmpty())
					list.Add(Format);
				return "{" + list.JoinToString(":") + "}";
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (!(obj is Item)) return false;
				var that = obj as Item;
				return this.Format == that.Format
						&& this.Index == that.Index
						&& this.IsArg == that.IsArg
						&& this.Length == that.Length
						&& this.Text == that.Text
						&& this.Type == that.Type
						&& (this.PropertyNames == that.PropertyNames || this.PropertyNames.SequenceEqual(that.PropertyNames))
						&& (this._split == that._split || this._split.SequenceEqual(that._split));
			}

		}

		internal static bool IsType(string part)
		{
			// Index.Property
			var split = part.Split('.');
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ_".ToArray();
			var chars_and_digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ_0123456789".ToArray();
			foreach (var item in split)
			{
				//if (!item.ToUpper().ContainsOnly(chars)) return false;
				if (item.Length == 0) return false;
				if (!chars.Contains(item.ToUpper()[0])) return false;
				if (!item.ToUpper().ContainsOnly(chars_and_digits)) return false;
			}
			return true;
		}
		internal static bool IsCast(string part)
		{
			//(Type)Index.Property
			//or
			//(Type)Property
			if (part.FirstOrDefault() != '(') return false;
			if (part.IndexOf(')') == -1) return false;
			var index = part.LastIndexOf(')');
			var split = new[] { part.Substring(1, index - 1), part.Substring(index + 1) };
			if (!StringFormatter.IsType(split[0])) return false;
			return StringFormatter.IsMultiPart(split[1]);
		}
		internal static bool IsMultiPart(string part)
		{
			// Index.Property
			var split = part.Split('.');
			if (split.Length == 0) return false;
			var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_".ToArray();
			foreach (var item in split)
			{
				if (item.Length == 0) return false;
				if (!item.ToUpper().ContainsOnly(chars)) return false;
			}
			return true;
		}
		internal static int? GetSpacing(string part, out int index)
		{
			index = part.LastIndexOf(',');
			if (index == -1) return null;
			var split = part.SplitAt(index);
			if (split.Length != 2) return null;
			var l = split[0];
			var r = split[1].Substring(1).Trim();
			if (!IsMultiPart(l) && !IsCast(l)) return null;
			return r.TryParseAsInt32(null);
		}
	}
}
