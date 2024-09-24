namespace GatewayService.Models
{
    public class PageDto<T>
    {
        public int TotalElementcCount { get; set; }
        public int TotalPagesCount { get; set; }
        public int ChoosenPageNumber { get; set; }
        public int ElementOnPageCount { get; set; }
        public List<T> Products { get; set; }
    }
}
