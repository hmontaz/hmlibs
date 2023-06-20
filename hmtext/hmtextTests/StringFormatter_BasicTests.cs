using System;

namespace hmlib.Tests.Utilities.StringTools
{
	public class StringFormatterTests
	{
		class B
		{
			public string Muh { get { return "Muh"; } }
		}
		class A
		{
			public string Name { get { return "John"; } }
			public double Age { get { return 35; } }
			public B Null_B = null;
			public DateTime Date
			{
				get
				{
					return new DateTime(2009, 10, 11, 12, 13, 14).AddMilliseconds(150);
				}
			}
			public override string ToString()
			{
				return "Text";
			}
		}
		[Fact]
		public void TestMethod_Native()
		{
			var a = new A();
			_Compare("", "", 0, 1);
			_Compare("hello", "hello");
			_Compare("hello{", "hello{{");
			_Compare("hello{ }", "hello{{ }}");
			_Compare("hello{10}", "hello{{{0}}}", 10);
			_Compare("hello\r\n{}", "hello\r\n{{}}");
			_Compare("hello [John] age:[35.00]", "hello [{0}] age:[{1:#.00}]", a.Name, a.Age);
			_Compare("01:29:30", "{0:hh':'mm':'ss}", TimeSpan.FromMinutes(89.5));
			_Compare("2015/01/02 15:20", "{0:yyyy/MM/dd HH:mm}", new DateTime(2015, 1, 2, 15, 20, 30));
			_Compare("2009/10/11 12:13:14.15", "{0:yyyy/MM/dd HH:mm:ss.ff}", a.Date);
			_Compare("2009.00", "{0:00.00}", a.Date.Year);
			//_Compare("1388/07/19", "{0:yyyy/MM/dd}", (JalaliDateTime)a.Date);
			_Compare("0", "{0}", 0, 1);
			_Compare("5A", "{0:0A;-0B;Zero;}", 5);
			_Compare("-5B", "{0:0A;-0B;Zero;}", -5);
			_Compare("Zero", "{0:0A;-0B;Zero;}", 0);
			_Compare("      2013|     1,025", "{0, 10}|{1,10:N0}", 2013, 1025);
			_Compare("2013      |1,025     ", "{0,-10}|{1, -10:N0}", 2013, 1025);
			_Compare("32767|7FFF|3,276,700.00%", "{0:G}|{0:X}|{0:P}", 32767);
			//Assert.AreEqual("", String.Format("{0:A;B;C;}", ""));
			//Assert.AreEqual("", String.Format("{0:A;B;C;}", (string)null));
			//Assert.AreEqual("Hello", String.Format("{0:A;B;C;}", "Hello"));
			try
			{
				Assert.Equal("0", String.Format("{0} {1}", 0));
				Assert.Fail("");
			}
			catch (FormatException) { }
			catch (Exception) { Assert.Fail(""); }

			try
			{
				String.Format("{}", 0);
				Assert.Fail("");
			}
			catch (FormatException) { }
			catch (Exception) { Assert.Fail(""); }
		}

		void _Compare(string expected, string format, params object[] args)
		{
			Assert.Equal(expected, String.Format(format, args));
			Assert.Equal(expected, StringFormatter.Format(format, args));
		}

		[Fact]
		public void TestMethod_hmlib_StringFormatter()
		{
			var a = new A();
			//var typeConverter = new System.ComponentModel.TypeConverter();
			Assert.Equal("", StringFormatter.Format("", 0, 1));

			Assert.Equal("2009/10/11 12:13:14.15", StringFormatter.Format("{0.Date:yyyy/MM/dd HH:mm:ss.ff}", a));
			Assert.Equal("2009.00", StringFormatter.Format("{0.Date.Year:00.00}", a));
			//todo:test this: Assert.Equal("1388/07/19", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)0.Date:yyyy/MM/dd}", a));

			Assert.Equal("", StringFormatter.Format("{0.Null_B}", a));
			Assert.Equal("", StringFormatter.Format("{0.Null_B.Muh}", a));

			//Assert.AreEqual("Zero", StringFormatter.Format("{0.IfNull(\"Goodbye\")}", "Hello"));
			//Assert.AreEqual("8.0", StringFormatter.Format("{Math.Floor(0):n0}", 8.5));
		}

		[Fact]
		public void InternalTests_ParseTest()
		{
			Assert.Equal("", StringFormatter.Item.Parse("").JoinToString(","));
			Assert.Equal("text", StringFormatter.Item.Parse("text").JoinToString(","));
			Assert.Equal("{0}", StringFormatter.Item.Parse("{0}").JoinToString(","));
			Assert.Equal("{0},{1}", StringFormatter.Item.Parse("{0}{1}").JoinToString(","));
			Assert.Equal("text,{0}", StringFormatter.Item.Parse("text{0}").JoinToString(","));
			Assert.Equal("text,{0},age,{1}", StringFormatter.Item.Parse("text{0}age{1}").JoinToString(","));

			// Odd but ok
			Assert.Equal("{{,text", StringFormatter.Item.Parse("{{text").JoinToString(","));
			Assert.Equal("}},text", StringFormatter.Item.Parse("}}text").JoinToString(","));
			Assert.Equal("{{,text,}}", StringFormatter.Item.Parse("{{text}}").JoinToString(","));
			Assert.Equal("text,}}", StringFormatter.Item.Parse("text}}").JoinToString(","));
			Assert.Equal("text,{{", StringFormatter.Item.Parse("text{{").JoinToString(","));
			Assert.Equal("{0},}}", StringFormatter.Item.Parse("{0}}}").JoinToString(","));
			Assert.Equal("{{,{0}", StringFormatter.Item.Parse("{{{0}").JoinToString(","));
			Assert.Equal("{{,{0},}}", StringFormatter.Item.Parse("{{{0}}}").JoinToString(","));
			Assert.Equal("text,{{, ,}}", StringFormatter.Item.Parse("text{{ }}").JoinToString(","));
			Assert.Equal("text,{{,{0},}}", StringFormatter.Item.Parse("text{{{0}}}").JoinToString(","));
		}

		[Fact]
		public void InternalTests_IsMultiPart()
		{
			Assert.True(StringFormatter.IsMultiPart("0.Name"));
			Assert.True(StringFormatter.IsMultiPart("0.User.Name"));
			//TODO:Assert.IsTrue(StringFormatter.MultiPart.Test("((System.DateTime)User.DOB).Year"));

			Assert.False(StringFormatter.IsMultiPart("0."));
			Assert.False(StringFormatter.IsMultiPart(".Name"));
			Assert.False(StringFormatter.IsMultiPart("0..Name"));
		}

		[Fact]
		public void InternalTests_IsCast()
		{
			Assert.True(StringFormatter.IsCast("(System.Int32)0"));
			Assert.True(StringFormatter.IsCast("(System.Int32)0.Length"));
			//TODO:Assert.True(StringFormatter.IsCast("((System.Int32)0)"));

			//Assert.False(StringFormatter.MultiPart.Test("0."));
			//Assert.False(StringFormatter.MultiPart.Test(".Name"));
			//Assert.False(StringFormatter.MultiPart.Test("0..Name"));
		}

		[Fact]
		public void InternalTests_IsFunctionCall()
		{
			Assert.True(StringFormatter.FunctionCall.Test("Math.Floor(0)"));
			Assert.True(StringFormatter.FunctionCall.Test("Math.Floor(0.Value)"));
			Assert.True(StringFormatter.FunctionCall.Test("Math.Floor(0.Property.Value)"));
			Assert.True(StringFormatter.FunctionCall.Test("Math.Floor(A(0.Property.Value))"));
		}

		[Fact]
		public void InvalidFormat_Tests()
		{
			try
			{
				// Missmatching Arguments
				StringFormatter.Format("{0} {1}", 0);
				Assert.Fail("");
			}
			catch (FormatException) { }
			catch (Exception) { Assert.Fail(""); }

			try
			{
				// Malformed Format 1
				StringFormatter.Format("{}", 0);
				Assert.Fail("");
			}
			catch (FormatException) { }
			catch (Exception) { Assert.Fail(""); }

			try
			{
				// Malformed Format 2
				StringFormatter.Format("{0}}", 0);
				Assert.Fail("");
			}
			catch (FormatException) { }
			catch (Exception) { Assert.Fail(""); }
		}

		[Fact]
		public void Cast_From_Double_Test()
		{
			Assert.Equal("15", ((Int32)15.75).ToString());
			Assert.Equal("15", StringFormatter.Context.TryUnbox(15.75, typeof(Int32)).ToString());

			Assert.Equal("15", StringFormatter.Format("{(System.Int16)0}", 15.75));
			Assert.Equal("15", StringFormatter.Format("{(System.Int32)0}", 15.75));
			Assert.Equal("15", StringFormatter.Format("{(System.Int64)0}", 15.75));
			Assert.Equal("15", StringFormatter.Format("{(System.UInt16)0}", 15.75));
			Assert.Equal("15", StringFormatter.Format("{(System.UInt32)0}", 15.75));
			Assert.Equal("15", StringFormatter.Format("{(System.UInt64)0}", 15.75));

			Assert.Equal("-15", ((Int32)(-15.75)).ToString());
			Assert.Equal("-15", StringFormatter.Format("{(System.Int16)0}", -15.75));
			Assert.Equal("-15", StringFormatter.Format("{(System.Int32)0}", -15.75));
			Assert.Equal("-15", StringFormatter.Format("{(System.Int64)0}", -15.75));

			Assert.Equal("62:45", StringFormatter.Format("{(System.Int32)0.TotalHours:00}:{0.Minutes:00}", new TimeSpan(2, 14, 45, 20)));
			Assert.Equal("2.14:45", StringFormatter.Format("{0.Days}.{0:hh':'mm}", new TimeSpan(2, 14, 45, 20)));

			Assert.Equal("2", StringFormatter.Format("{0:n0}", 1.5));
			Assert.Equal("2", StringFormatter.Format("{0:0}", 1.5));
			Assert.Equal("1.13:00", StringFormatter.Format("{0.Days:0\\.;;'';}{0:hh':'mm}", new TimeSpan(1, 13, 0, 0)));
			Assert.Equal("13:00", StringFormatter.Format("{0.Days:0\\.;;'';}{0:hh':'mm}", new TimeSpan(0, 13, 0, 0)));
		}
	}
}
