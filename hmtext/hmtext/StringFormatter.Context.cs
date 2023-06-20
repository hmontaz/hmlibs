using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Dynamic;
using System.Collections;

namespace hmlib
{
	public partial class StringFormatter
	{
		internal class Context
		{
			internal readonly StringFormatterOptions Options;
			internal readonly string Format;

			/// <summary>
			/// Translated Arguments
			/// </summary>
			internal readonly object[] Args;

			public Context(StringFormatterOptions options, Item[] items, object[] args)
			{
				var list = new List<string>();
				Options = options;
				var temp_args = new List<object>();
				for (var i = 0; i < items.Length; i++)
				{
					var item = items[i];
					if (!item.IsArg)
					{
						list.Add(item.Text);
						continue;
					}
					var index = temp_args.Count;
					list.Add(item.GetText(index));
					object value;
					if (item.PropertyNames == null)
					{
						if (item.Index >= args.Length) throw new FormatException();
						value = args[item.Index];
					}
					else
					{
						value = Context.Eval(args[item.Index], Options, item.PropertyNames);
					}

					if (item.Type != null)
					{
						value = ConvertTo(value, item.Type);
					}
					temp_args.Add(value);
				}
				this.Args = temp_args.ToArray();
				this.Format = string.Join("", list);
			}

			internal object ConvertTo(object value, Type destinationType)
			{
				if (value == null) return value;
				if (this.Options.TypeConverter != null
					&& this.Options.TypeConverter.CanConvertFrom(value.GetType())
					&& this.Options.TypeConverter.CanConvertTo(destinationType))
				{
					return this.Options.TypeConverter.ConvertTo(value, destinationType);
				}
				value = Context.TryUnbox(value, destinationType);
				return Convert.ChangeType(value, destinationType);
			}

			internal static Context Translate(StringFormatterOptions options, string format, params object[] args)
			{
				var items = Item.Parse(format);
				return new Context(options, items, args);
			}

			internal static object Eval(object obj, StringFormatterOptions options, string[] memberNames)
			{
				if (obj == null) return null;
				//var type = obj.GetType();
				object value;
				//-----------
				var name = memberNames[0];
				//if (obj is IDictionary<string, object>)
				//{
				//	var dic = (IDictionary<string, object>)obj;
				//	value = dic[name];
				//}
				//if (obj is IDictionary && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
				//{
				//	var keyType = type.GetGenericArguments()[0];
				//	var valueType = type.GetGenericArguments()[1];
				//	//if (keyType != typeof(string)) throw new Exception("Invalid Dictionary Type");
				//	var dic = (IDictionary)obj;
				//	//var dic = (IDictionary<string, object>)obj;
				//	value = dic[name];
				//}
				if (!_tryAsDictionary(obj, name, out value))
					if (!_tryAsMember(obj, name, out value))
						throw new FormatException("Member Not Found '" + name + "'");

				if (value == null && options.IgnoreNull) return null;
				//-----------
				if (memberNames.Length == 1) return value;
				//-----------
				return Context.Eval(value, options, memberNames.Skip(1).ToArray());
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
}
