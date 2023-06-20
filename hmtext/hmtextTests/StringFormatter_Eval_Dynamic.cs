using System;
using System.Dynamic;
using System.Collections.Generic;

namespace hmlib.Tests.Utilities.StringTools
{
	public class StringFormatter_Eval_Dynamic
	{
		[Fact]
		public void Expando_Test()
		{
			dynamic o = new ExpandoObject();
			o.Index = 1;
			o.Text = "Hello";

			Assert.Equal("1", StringFormatter.Format("{Index}", o));
		}

		[Fact]
		public void Dynamic_Test()
		{
			var o = new
			{
				Index = 1,
				Text = "Hello",
			};

			Assert.Equal("1 Hello", StringFormatter.Format("{Index} {Text}", o));
		}

		[Fact]
		public void Dic_Int32_Test()
		{
			var o = new Dictionary<string, Int32>();
			o["Index"] = 1;

			Assert.Equal("1", StringFormatter.Format("{Index}", o));
		}

		[Fact]
		public void Dic_Object_Test()
		{
			var o = new Dictionary<string, object>();
			o["Index"] = 1;

			Assert.Equal("1", StringFormatter.Format("{Index}", o));
		}

		[Fact]
		public void Dic_String_Test()
		{
			var o = new Dictionary<string, string>();
			o["Index"] = "1";

			Assert.Equal("1", StringFormatter.Format("{Index}", o));
		}

	}
}
