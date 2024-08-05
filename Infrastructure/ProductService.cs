using Application;
using Dai_2022.Models;
using Dai_2022.Views.Product;
using Domain.Image;
using Domain.Product;


namespace Infrastructure
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponseModel2> Add_Product(ProductReqvestModel2 model)
        {
            var productExist = await _repository.Get_Product_ByName(model.ProductName);

            if (productExist != null)
            {
                throw new InvalidOperationException("A product with the same name already exists.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await model.PhotoData.CopyToAsync(memoryStream);

                var product = new Product
                {
                    ProductCategory = model.ProductCategoryName,
                    ProductName = model.ProductName,
                    ProdutCount = model.ProductCount,
                    ContentType = model.PhotoData.ContentType,
                    Image = new Image
                    {
                        ImageData = memoryStream.ToArray()
                    }
                };

                var addedProduct = await _repository.AddAsync(product);

                return new ProductResponseModel2
                {
                    Id_Pr = addedProduct.Id,
                    ProductCategory = addedProduct.ProductCategory,
                    ProductName = addedProduct.ProductName,
                    ProdutCount = addedProduct.ProdutCount,
                    ImageData = addedProduct.Image.ImageData,
                    ContentType = addedProduct.ContentType,
                };
            }
        }

        public async Task Delete_Product(int id)
        {
            var product = await _repository.Get_Product_ById(id);
            if (product != null)
            {
                await _repository.DeleteAsync(product);
            }
        }

        //public async Task<IEnumerable<ProductResponseModel2>> GetAll_Productasync(int page,int pegSize)
        //{
        //    var allProducts = await _repository.GetAll_Productasync( page,pegSize);

        //    var productResponseModels = allProducts.Select(item => new ProductResponseModel2
        //    {
        //        Id_Pr = item.Id,
        //        ProductCategory = item.ProductCategory,
        //        ProductName = item.ProductName,
        //        ProdutCount = item.ProdutCount,
        //        ImageData = item.Image?.ImageData, // Handle null Image
        //        ContentType = item.ContentType,
        //    }).ToList();

        //    return productResponseModels;
        //}


        public async Task<ProductResponseModel2> Get_Product_ById(int id)
        {
            var product = await _repository.Get_Product_ById(id);

            return new ProductResponseModel2
            {
                Id_Pr = product.Id,
                ProductCategory = product.ProductCategory,
                ProductName = product.ProductName,
                ProdutCount = product.ProdutCount,
                ImageData = product.Image.ImageData,
                ContentType = product.ContentType,
            };
        }

        public async Task<ProductResponseModel2> Get_Product_ByName(string name)
        {
            var product = await _repository.Get_Product_ByName(name);

            return new ProductResponseModel2
            {
                Id_Pr = product.Id,
                ProductCategory = product.ProductCategory,
                ProductName = product.ProductName,
                ProdutCount = product.ProdutCount,
                ImageData = product.Image.ImageData,
                ContentType = product.ContentType,
            };
        }
        public async Task<ProductResponseModel2> Update_Product(ProductReqvestModel2 model, int id)
        {
            var productUpdate = await _repository.Get_Product_ById(id);

            if (productUpdate != null)
            {
                // Update product properties
                productUpdate.ProductName = model.ProductName;
                productUpdate.ProductCategory = model.ProductCategoryName;
                productUpdate.ProdutCount = model.ProductCount;

                // Update or create image
                if (model.PhotoData != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.PhotoData.CopyToAsync(memoryStream);
                        var imageData = memoryStream.ToArray();
                        var contentType = model.PhotoData.ContentType;

                        if (productUpdate.Image != null)
                        {
                            // Update existing image
                            productUpdate.Image.ImageData = imageData;
                            productUpdate.ContentType = contentType;
                        }
                        else
                        {
                            // Create new image
                            var newImage = new Image
                            {
                                ImageData = imageData,
                                Id = productUpdate.Id // Ensure ProductId is set
                            };
                          //  _repository.Images.Add(newImage); // Ensure image is added correctly to the context
                            productUpdate.Image = newImage;
                            productUpdate.ContentType = contentType;
                        }
                    }
                }

                // Save changes to database
                await _repository.UpdateAsync(productUpdate);

                // Prepare response model
                var responseModel = new ProductResponseModel2
                {
                    Id_Pr = productUpdate.Id,
                    ProductCategory = productUpdate.ProductCategory,
                    ProductName = productUpdate.ProductName,
                    ProdutCount = productUpdate.ProdutCount,
                    ImageData = productUpdate.Image?.ImageData,
                    ContentType = productUpdate.ContentType,
                };

                return responseModel;
            }

            throw new InvalidOperationException("Product not found.");
        }


        public async Task<Product> ShowImage(int id)
        {
            var product = await _repository.ShowImage(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            return product;
        }

 public async Task<IEnumerable<ProductResponseModel2>> GetByCategory(string categoryName)

        {
    var allProducts = await _repository.GetByCategory(categoryName);

    var productResponseModels = allProducts.Select(item => new ProductResponseModel2
    {
        Id_Pr = item.Id,
        ProductCategory = item.ProductCategory,
        ProductName = item.ProductName,
        ProdutCount = item.ProdutCount,
        ImageData = item.Image?.ImageData, 
        ContentType = item.ContentType,
    }).ToList(); 

    return productResponseModels;
}



        public async Task<IEnumerable<ProductResponseModel2>> GetAll_Productasync()
        {
            var allProducts = await _repository.GetAll_Productasync();

            var productResponseModels = allProducts.Select(item => new ProductResponseModel2
            {
                Id_Pr = item.Id,
                ProductCategory = item.ProductCategory,
                ProductName = item.ProductName,
                ProdutCount = item.ProdutCount,
                ImageData = item.Image?.ImageData, // Handle null Image
                ContentType = item.ContentType,
            }).ToList();

            return productResponseModels;

        }



        /*
        public async Task<IEnumerable<ProductResponseModel2>> GetAll_Productasync(int page, int pageSize)
        {
            var allProducts = await _repository.GetAll_Productasync(page, pageSize);

            var productResponseModels = allProducts.Select(item => new ProductResponseModel2
            {
                Id_Pr = item.Id,
                ProductCategory = item.ProductCategory,
                ProductName = item.ProductName,
                ProdutCount = item.ProdutCount,
                ImageData = item.Image?.ImageData, // Handle null Image
                ContentType = item.ContentType,
            }).ToList();

            return productResponseModels;
        }
        */
    }
}
