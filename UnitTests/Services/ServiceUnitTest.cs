using Xunit;

namespace technical_tests_backend_ssr.UnitTests.Services
{
    public class ServiceUnitTest
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            int expected = 5;
            int actual = 2 + 3;
            // Act
            // Call the service.
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
