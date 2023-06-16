using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using System.Collections.Generic;

namespace hmlib.Tests.UtilitiesTests.StringTools
{
	[TestClass]
	public class StringFormatter_Eval_Dynamic
	{
		[TestMethod]
		public void Expando_Test()
		{
			dynamic o = new ExpandoObject();
			o.Index = 1;
			o.Text = "Hello";

			Assert.AreEqual("1", StringFormatter.Format("{Index}", o));
		}

		[TestMethod]
		public void Dynamic_Test()
		{
			var o = new
			{
				Index = 1,
				Text = "Hello",
			};

			Assert.AreEqual("1 Hello", StringFormatter.Format("{Index} {Text}", o));
		}

		[TestMethod]
		public void Dic_Int32_Test()
		{
			var o = new Dictionary<string, Int32>();
			o["Index"] = 1;

			Assert.AreEqual("1", StringFormatter.Format("{Index}", o));
		}

		[TestMethod]
		public void Dic_Object_Test()
		{
			var o = new Dictionary<string, object>();
			o["Index"] = 1;

			Assert.AreEqual("1", StringFormatter.Format("{Index}", o));
		}

		[TestMethod]
		public void Dic_String_Test()
		{
			var o = new Dictionary<string, string>();
			o["Index"] = "1";

			Assert.AreEqual("1", StringFormatter.Format("{Index}", o));
		}

	}
}
