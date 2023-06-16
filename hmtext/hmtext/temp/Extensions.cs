using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using static hmlib.StringFormatter;

namespace hmlib
{
	internal static class Extensions
	{
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}
		/*public static bool ContainsOnly(this string s, char[] chars, StringComparison stringComparison)
		{
			//System.Globalization.CultureInfo.CurrentCulture.CompareInfo
			if (stringComparison.In(StringComparison.CurrentCultureIgnoreCase, StringComparison.OrdinalIgnoreCase))
			{
				return Extensions.ContainsOnly(s.ToUpper(), chars.JoinToString().ToUpper().ToArray());
			}
			if (stringComparison.In(StringComparison.InvariantCultureIgnoreCase))
			{
				return Extensions.ContainsOnly(s.ToUpperInvariant(), chars.JoinToString().ToUpperInvariant().ToArray());
			}
			return Extensions.ContainsOnly(s, chars);
		}*/

		public static bool ContainsOnly(this string s, char[] chars)
		{
			return s.All(c => chars.Contains(c));
		}
		public static string JoinToString<T>(this IEnumerable<T> values, string separator = "")
		{
			return string.Join(separator, values);
		}
		public static bool In<T>(this T item, params T[] values)
		{
			return values.Contains(item);
		}
		public static IEnumerable<T> Append<T>(this IEnumerable<T> values, params T[] items)
		{
			foreach (var value in values)
			{
				yield return value;
			}
			foreach (var item in items)
			{
				yield return item;
			}
		}
		/*public static string JoinToString<T>(this IEnumerable<T> values, char separator)
		{
			return string.Join(separator.ToString(), values);
		}*/
		public static int? TryParseAsInt32(this string str, int? defaultValue)
		{
			int result;
			if (int.TryParse(str, out result)) return result;
			return defaultValue;
		}
		public static string[] SplitAt(this string s, params int[] indexes)
		{
			var list = new List<string>();

			var index = 0;
			for (var j = 0; j < indexes.Length; j++)
			{
				if (indexes[j] == 0) continue;
				list.Add(s.Substring(index, indexes[j] - index));
				index = indexes[j];
			}
			list.Add(s.Substring(index));
			return list.ToArray();
		}
	}
}
