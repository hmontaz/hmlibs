namespace hmlib.Tests.Utilities.StringTools
{
	public class StringFormatter_Eval_Class
	{
		public class test
		{
			public int _Int32 { get { return 1; } }
			public string _String = "hello";
			public Boolean _Boolean = true;


		}
		[Fact]
		public void TestMethod1()
		{
			var o = new test();

			Assert.Equal(1, StringFormatter.Eval(o, "_Int32"));
			Assert.Equal("hello", StringFormatter.Eval(o, "_String"));
			Assert.Equal(true, StringFormatter.Eval(o, "_Boolean"));

		}
	}
}
