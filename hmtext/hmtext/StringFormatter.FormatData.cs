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
		internal class FormatData
		{
			readonly StringFormatterOptions _options;
			readonly public string Format;

			/// <summary>
			/// Translated Arguments
			/// </summary>
			readonly public object[] Args;

			public FormatData(StringFormatterOptions options, Item[] items, object[] args)
			{
				var list = new List<string>();
				_options = options;
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
						value = FormatData.Eval(args[item.Index], _options, item.PropertyNames);
					}

					if (item.Type != null)
					{
						value = ConvertTo(value, item.Type);
						//if (options.TypeConverter != null)
						//{
						//	value = options.TryConvertTo(value, item.Type, null);
						//}
						//else
						//	value = ChangeType(value, item.Type);
					}
					temp_args.Add(value);
				}
				this.Args = temp_args.ToArray();
				this.Format = string.Join("", list);
			}

			internal object ConvertTo(object value, Type destinationType)
			{
				if (value == null) return value;
				if (this._options.TypeConverter != null
					&& this._options.TypeConverter.CanConvertFrom(value.GetType())
					&& this._options.TypeConverter.CanConvertTo(destinationType))
				{
					return this._options.TypeConverter.ConvertTo(value, destinationType);
				}
				value = StringFormatter.TryUnbox(value, destinationType);
				return Convert.ChangeType(value, destinationType);
			}

			internal static FormatData Translate(StringFormatterOptions options, string format, params object[] args)
			{
				var items = Item.Parse(format);
				return new FormatData(options, items, args);
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
				return FormatData.Eval(value, options, memberNames.Skip(1).ToArray());
			}

		}
	}
}
