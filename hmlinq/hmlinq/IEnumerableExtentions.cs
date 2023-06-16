using System;
using System.Collections.Generic;
using System.Text;

namespace hmlinq
{
	public static class IEnumerableExtentions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static string JoinToString<T>(this IEnumerable<T> values, string separator)
		{
			return string.Join(separator, values);
		}
	}
}
