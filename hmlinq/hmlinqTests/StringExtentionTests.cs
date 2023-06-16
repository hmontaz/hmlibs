using FluentAssertions;
using hmlinq;

namespace hmlinqTests
{
	public class StringExtentionTests
	{
		[Fact]
		public void Test1()
		{
			StringExtensions.IsNullOrEmpty("").Should().BeTrue();
			StringExtensions.IsNullOrEmpty(" ").Should().BeFalse();
			StringExtensions.IsNullOrEmpty(null).Should().BeTrue();

			StringExtensions.IsNullOrWhiteSpace("").Should().BeTrue();
			StringExtensions.IsNullOrWhiteSpace(" ").Should().BeTrue();
			StringExtensions.IsNullOrWhiteSpace(null).Should().BeTrue();
			StringExtensions.IfNullOrWhiteSpace("text", "*").Should().Be("text");
		}

		[Fact]
		public void Test2()
		{
			StringExtensions.IfNullOrEmpty("", "*").Should().Be("*");
			StringExtensions.IfNullOrEmpty(" ", "*").Should().Be(" ");
			StringExtensions.IfNullOrEmpty(null, "*").Should().Be("*");

			StringExtensions.IfNullOrWhiteSpace("", "*").Should().Be("*");
			StringExtensions.IfNullOrWhiteSpace(" ", "*").Should().Be("*");
			StringExtensions.IfNullOrWhiteSpace(null, "*").Should().Be("*");
			StringExtensions.IfNullOrWhiteSpace("text", "*").Should().Be("text");
		}
	}
}