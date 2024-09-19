namespace GatewayService.Models
{
    public class GetProductsWithPaginationRequestDto
    {
        public int ElementsOnCurrentPageCount { get; set; }
        public int CurrentPageNumber { get; set; }
    }
}
