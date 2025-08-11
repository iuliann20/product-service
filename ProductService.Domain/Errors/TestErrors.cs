using ProductService.Domain.Shared;

namespace ProductService.Domain.Errors
{
    public static class TestErrors
    {
        public static readonly Error TestError = new(
           "Test.TestError",
           "This is a test error.");
    }
}
