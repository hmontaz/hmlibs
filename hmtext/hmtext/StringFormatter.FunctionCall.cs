using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hmlib
{
	public partial class StringFormatter
	{
		internal class FunctionCall
		{
			internal static bool Test(string part)
			{
				if (part.IndexOf('(') == -1) return false;
				if (part.IndexOf('(') >= part.LastIndexOf(')')) return false;
				var index = part.IndexOf('(');
				var left = part.Substring(0, index);
				var right = part.Substring(0, part.Length - 1).Substring(index + 1);
				return StringFormatter.IsMultiPart(left)
					&& (StringFormatter.IsMultiPart(right) || FunctionCall.Test(right));
			}
		}
	}
}