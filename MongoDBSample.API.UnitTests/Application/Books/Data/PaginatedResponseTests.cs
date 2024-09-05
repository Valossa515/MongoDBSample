using FluentAssertions;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.API.UnitTests.Application.Books.Data
{
    public class PaginatedResponseTests
    {
        [Fact(DisplayName = "Instanciar objeto PaginatedResponse")]
        public void Instanciar_PaginatedResponse_ReturnsPaginatedResponse()
        {
            // Arrange
            IEnumerable<string> data = new List<string> { "data" };
            int totalCount = 1;
            int page = 1;
            int pageSize = 10;

            // Act
            PaginatedResponse<string> actual = new()
            {
                Data = data,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            // Assert
            actual.Data.Should().BeEquivalentTo(data);
            actual.TotalCount.Should().Be(totalCount);
            actual.Page.Should().Be(page);
            actual.PageSize.Should().Be(pageSize);
        }
    }
}
