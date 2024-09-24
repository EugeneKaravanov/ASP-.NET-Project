namespace ProductService.Models
{
    public class Page<T>
    {
        public Page(int totalElementcCount, int totalPagesCount, int choosenPageNumber, int elementOnPageCount, List<T> products)
        {
            TotalElementcCount = totalElementcCount;
            TotalPagesCount = totalPagesCount;
            ChoosenPageNumber = choosenPageNumber;
            ElementOnPageCount = elementOnPageCount;
            Products = products;
        }

        public int TotalElementcCount { get; set; }
        public int TotalPagesCount { get; set; }
        public int ChoosenPageNumber { get; set; }
        public int ElementOnPageCount { get; set; }
        public List<T> Products { get; set; }
    }
}
