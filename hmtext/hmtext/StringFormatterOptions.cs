using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace hmlib
{
	public class StringFormatterOptions
	{
		public readonly TypeConverter TypeConverter;
		/// <summary>
		/// Ignore Null Properties
		/// </summary>
		public bool IgnoreNull { get; set; }

		public StringFormatterOptions() : this(null)
		{

		}
		public StringFormatterOptions(TypeConverter typeConverter)
		{
			this.TypeConverter = typeConverter;
			this.IgnoreNull = true;
		}

		public static StringFormatterOptions Default
		{
			get { return new StringFormatterOptions { IgnoreNull = true }; }
		}

	}
}
