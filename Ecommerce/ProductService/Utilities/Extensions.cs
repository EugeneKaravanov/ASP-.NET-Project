using ProductService.Models;

namespace ProductService.Utilities
{
    internal static class Extensions
    {
        internal static IEnumerable<KeyValuePair<int, Product>> GetProductsAfterSorting(this IEnumerable<KeyValuePair<int, Product>> products, string sortArgument, bool isReverseSort)
        {
            if (sortArgument != "Name" && sortArgument != "Price")
            {
                return products;
            }

            switch (sortArgument)
            {
                case "Name":
                    if (isReverseSort == false)
                        products = products.OrderBy(product => product.Value.Name);
                    else
                        products = products.OrderByDescending(product => product.Value.Name);
                    break;

                case "Price":
                    if (isReverseSort == false)
                        products = products.OrderBy(product => product.Value.Price);
                    else
                        products = products.OrderByDescending(product => product.Value.Price);
                    break;
            }

            return products;
        }
    }
}
