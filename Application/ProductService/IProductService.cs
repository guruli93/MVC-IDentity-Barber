﻿using Domain.Productentity;

namespace Application.Models_DB
{
    public interface IProductService
    {
        Task<ProductResponseModel2> Get_Product_ById(int id);
        Task<ProductResponseModel2> Get_Product_ByName(string name);
        Task<IEnumerable<ProductResponseModel2>> GetAll_Productasync();
        Task<ProductResponseModel2> Add_Product(ProductReqvestModel2 model);
        Task<ProductResponseModel2> Update_Product(ProductReqvestModel2 model, int id);
        Task Delete_Product(int id);
        Task<Product> ShowImage(int id);
        Task<IEnumerable<ProductResponseModel2>> GettingDatabyPages(int page, int pegSize);
        Task<IEnumerable<ProductResponseModel2>> GetByCategory(string CategoryName);
        Task<int> AmountOfallData();
    }
}
