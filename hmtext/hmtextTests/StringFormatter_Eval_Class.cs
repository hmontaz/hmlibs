using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hmlib.Tests.UtilitiesTests.StringTools
{
	[TestClass]
	public class StringFormatter_Eval_Class
	{
		public class test
		{
			public int _Int32 { get { return 1; } }
			public string _String = "hello";
			public Boolean _Boolean = true;


		}
		[TestMethod]
		public void TestMethod1()
		{
			var o = new test();

			Assert.AreEqual(1, StringFormatter.Eval(o, "_Int32"));
			Assert.AreEqual("hello", StringFormatter.Eval(o, "_String"));
			Assert.AreEqual(true, StringFormatter.Eval(o, "_Boolean"));

		}
	}
}
