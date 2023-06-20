using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hmlib
{
	public partial class StringFormatter
	{
		public static string Format(IFormatProvider provider, string format, params object[] args)
		{
			var options = new StringFormatterOptions();
			var context = Context.Translate(options, format, args);
			return String.Format(provider, context.Format, context.Args);
		}

		public static string Format(string format, params object[] args)
		{
			var options = new StringFormatterOptions();
			var context = Context.Translate(options, format, args);
			return String.Format(context.Format, context.Args);
		}

		public static string Format(TypeConverter typeConverter, string format, params object[] args)
		{
			var options = new StringFormatterOptions(typeConverter);
			var context = Context.Translate(options, format, args);
			return String.Format(context.Format, context.Args);
		}
		//public static string Format(IFormatProvider provider, string format, params object[] args)
		//{
		//	var formatter = new StringFormatter();
		//	return formatter.Format(provider, format, args);
		//}

		//public static string Format(string format, params object[] args)
		//{
		//	var formatter = new StringFormatter();
		//	return formatter.Format(format, args);
		//}

		public static object Eval(object obj, params string[] memberNames)
		{
			var options = StringFormatterOptions.Default;
			return Context.Eval(obj, options, memberNames);
		}
	}
}
