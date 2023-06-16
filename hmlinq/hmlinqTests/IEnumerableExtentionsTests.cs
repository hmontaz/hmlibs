using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using hmlinq;

namespace hmlinqTests
{
	public class IEnumerableExtentionsTests
	{
		[Fact]
		public void JoinToStringTest()
		{
			new[] { 1, 2, 3 }.JoinToString(",").Should().Be("1,2,3");
		}
	}
}
