using Xunit;
using Moq;
using PlainOldStoreApp.App;

namespace TestProjectPosa
{
    public class PosaTests
    {
        [Fact]
        public void StoreTests()
        {
            //arrange
            Validate.ValidateNameOrEmail();
            //act
            //assert
        }
    }
}