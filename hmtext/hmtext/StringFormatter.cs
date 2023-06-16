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
		StringFormatterOptions _options = new StringFormatterOptions();
		public static string Format(IFormatProvider provider, string format, params object[] args)
		{
			var options = new StringFormatterOptions();
			var data = FormatData.Translate(options, format, args);
			return String.Format(provider, data.Format, data.Args);
		}

		public static string Format(string format, params object[] args)
		{
			var options = new StringFormatterOptions();
			var data = FormatData.Translate(options, format, args);
			return String.Format(data.Format, data.Args);
		}

		public static string Format(TypeConverter typeConverter, string format, params object[] args)
		{
			var options = new StringFormatterOptions(typeConverter);
			var data = FormatData.Translate(options, format, args);
			return String.Format(data.Format, data.Args);
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
			return StringFormatter.FormatData.Eval(obj, options, memberNames);
		}
		internal static object TryUnbox(object value, Type type)
		{
			if (value is Double)
			{
				if (type == typeof(Int16)) return (Int16)(Double)value;
				if (type == typeof(Int32)) return (Int32)(Double)value;
				if (type == typeof(Int64)) return (Int64)(Double)value;
				if (type == typeof(UInt16)) return (UInt16)(Double)value;
				if (type == typeof(UInt32)) return (UInt32)(Double)value;
				if (type == typeof(UInt64)) return (UInt64)(Double)value;
			}
			if (value is Single)
			{
				if (type == typeof(Int16)) return (Int16)(Single)value;
				if (type == typeof(Int32)) return (Int32)(Single)value;
				if (type == typeof(Int64)) return (Int64)(Single)value;
				if (type == typeof(UInt16)) return (UInt16)(Single)value;
				if (type == typeof(UInt32)) return (UInt32)(Single)value;
				if (type == typeof(UInt64)) return (UInt64)(Single)value;
			}
			return value;
		}
	}
}
