namespace GatewayService.Models
{
    public class GetProductsWithPaginationResponseDto
    {
        public int elementsCount { get; set; }
        public int currentPageNumber { get; set; }
        public int elementsOnCurrentPageCount { get; set; }
        public List<ProductWithIdDto> products { get; set; }
    }
}
