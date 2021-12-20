using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Product
    {
        internal int? ProductId { get; }
        internal string? ProductName { get; }
        internal string? ProductDescription { get; }
        internal decimal ProductPrice { get; }
        internal int? ProductQuantiy { get; }
        internal int StoreID { get; }

        private readonly IProductRepository? _productRepository;

        internal Product(int storeLocation, IProductRepository productRepository)
        {
            StoreID = storeLocation;
            _productRepository = productRepository;
        }
        internal Product(int productId, string productName, string productDescription, decimal productPrice, int productQuantiy, int storeID)
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
            ProductQuantiy = productQuantiy;
            StoreID = storeID;
        }

        internal List<Product> GetStoreInventory()
        {
            List<Product> products = _productRepository!.GetAllStoreProducts(StoreID);
            return products;
        }
    }
}
