using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hmlib.Tests.Utilities.StringTools
{
	public class StringFormatter_TypeConverter_Tests
	{
		class T
		{
			public DateTime CreatedOn;
			public DateTime? UpdatedOn;
		}

		[TestMethod]
		public void TestMethod_TypeConversion()
		{
			var typeConverter = new TestTypeConverter();
			var a = new T();
			a.CreatedOn = new DateTime(2009, 10, 11, 10, 11, 12);
			a.UpdatedOn = null;

			Assert.AreEqual("10/11/2009 10:11:12 AM", StringFormatter.Format(typeConverter, "{CreatedOn}", a));
			Assert.AreEqual("2009", StringFormatter.Format(typeConverter, "{0.CreatedOn.Year}", a));
			Assert.AreEqual("2009", StringFormatter.Format(typeConverter, "{CreatedOn.Year}", a));
			Assert.AreEqual("2009/10/11", StringFormatter.Format(typeConverter, "{CreatedOn:yyyy/MM/dd}", a));
			Assert.AreEqual("2009", StringFormatter.Format(typeConverter, "{CreatedOn.Date.Year}", a));

			Assert.AreEqual("1388/07/19 10:11:12 AM", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)CreatedOn}", a));
			Assert.AreEqual("1388/07/19", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)CreatedOn:yyyy/MM/dd}", a));
			Assert.AreEqual("1388/07/19", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)0.CreatedOn:yyyy/MM/dd}", a));
			Assert.AreEqual("1388/07/19", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)0:yyyy/MM/dd}", a.CreatedOn));
			//todo:Assert.AreEqual("1388", StringFormatter.Format(typeConverter, "{((hmlib.Types.JalaliDateTime)CreatedOn).Date.Year}", a));

			////------------------

			Assert.AreEqual("", StringFormatter.Format(typeConverter, "{UpdatedOn}", a));
			Assert.AreEqual("", StringFormatter.Format(typeConverter, "{0.UpdatedOn.Year}", a));
			Assert.AreEqual("", StringFormatter.Format(typeConverter, "{UpdatedOn.Year}", a));
			Assert.AreEqual("", StringFormatter.Format(typeConverter, "{UpdatedOn.Date.Year}", a));

			Assert.AreEqual("", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)UpdatedOn}", a));
			Assert.AreEqual("", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)UpdatedOn:yyyy/MM/dd}", a));
			Assert.AreEqual("", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)0.UpdatedOn:yyyy/MM/dd}", a));
			Assert.AreEqual("", StringFormatter.Format(typeConverter, "{(hmlib.Types.JalaliDateTime)0:yyyy/MM/dd}", a.UpdatedOn));

		}


	}
}
