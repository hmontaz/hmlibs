using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace hmlib
{
	public partial class StringFormatter
	{
		static bool _tryAsDictionary(object obj, string name, out object value)
		{

			// Expando Object
			if (obj is IDictionary<string, object>)
			{
				value = ((IDictionary<string, object>)obj)[name];
				return true;
			}

			var type = obj.GetType();
			var isDic = obj is IDictionary && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
			if (!isDic)
			{
				value = null;
				return false;
			}

			//var keyType = type.GetGenericArguments()[0];
			//var valueType = type.GetGenericArguments()[1];
			//if (keyType != typeof(string)) throw new Exception("Invalid Dictionary Type");
			value = ((IDictionary)obj)[name];
			return true;
		}
		static bool _tryAsMember(object obj, string name, out object value)
		{
			var type = obj.GetType();
			var member = type.GetMember(name).FirstOrDefault();
			if (member == null)
			{
				value = null;
				return false;
			}
			if (member is PropertyInfo)
			{
				value = ((PropertyInfo)member).GetValue(obj, null);
				return true;
			}
			if (member is FieldInfo)
			{
				value = ((FieldInfo)member).GetValue(obj);
				return true;
			}
			throw new FormatException("Unhandled Member Type '" + member + "'");
		}
	}
}
