using Xunit;

namespace Logger.Tests
{
	public class DemoTests
	{
		[Fact]
		public void PassingTest()
		{
			Assert.Equal(4, Add(2, 2));
		}

		[Fact]
		public void FailingTest()
		{
			Assert.Equal(4, Add(2, 2));
		}

		

		[Theory]
		[InlineData(3)]
		[InlineData(5)]
		//[InlineData(6)]
		public void MyFirstTheory(int value)
		{
			Assert.True(IsOdd(value));
		}

		// Demo Functions

		static int Add(int x, int y)
		{
			return x + y;
		}

		static bool IsOdd(int value)
		{
			return value % 2 == 1;
		}
	}
}