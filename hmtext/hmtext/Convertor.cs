using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

namespace hmlib.Utilities
{
	internal static partial class Convertor
	{
		//public static UInt64 ToBase10(string value, UInt16 fromBase = 16)
		//{
		//	string charSet = "0123456789abcdefghijklmnopqrstuvwxyz";
		//	if (value == null) throw new Exception("value can not be null");
		//	if (value.Length == 0) throw new Exception("value can not be empty");
		//	if (fromBase <= 1) throw new ArgumentOutOfRangeException("fromBase must be greater than 1");
		//	if (fromBase >= charSet.Length) throw new ArgumentOutOfRangeException("fromBase must be less than " + (charSet.Length - 1));

		//	UInt64 result = 0;
		//	int len = value.Length;
		//	for (int i = 0; i < len; i++)
		//	{
		//		char c = value[i];
		//		int index = charSet.IndexOf(c);
		//		if (index < 0) throw new Exception();
		//		UInt64 vc = (UInt64)index;
		//		//if ("0123456789".Contains(c))
		//		//	vc = UInt64.Parse(c.ToString());
		//		//else
		//		//	vc = (UInt64)(c - 55);
		//		if (vc < 0 || vc >= fromBase) throw new Exception("digit out of range");
		//		UInt64 v = vc * (UInt64)System.Math.Pow(fromBase, len - i - 1);
		//		result += v;
		//	}
		//	return result;
		//}
		public static Stream ToStream(byte[] array)
		{
			try
			{
				return new MemoryStream(array);
			}
			catch { }
			return Stream.Null;
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

		/// <summary>
		/// Converts a byte array to it's hexadecimal string representation
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		//public static string ToHexString(byte[] bytes)
		//{
		//	string result = "";
		//	foreach (byte b in bytes)
		//	{
		//		result += "0x" + b.ToString("x").PadLeft(4, '0') + " ";
		//	}
		//	return result.Trim();
		//}

		public static T CastAs<T>(object value)
		{
			try { return (T)value; }
			catch { }

			var type = typeof(T);
			return (T)Convert.ChangeType(Convertor.TryUnbox(value, type), type);
		}
	}
}
