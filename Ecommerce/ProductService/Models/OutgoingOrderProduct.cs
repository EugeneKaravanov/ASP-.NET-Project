﻿namespace ProductService.Models
{
    public class OutgoingOrderProduct
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
