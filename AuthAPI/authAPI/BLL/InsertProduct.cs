using authAPI.CommonModels;
using authAPI.DAL.DataAccess;
using authAPI.DAL.Interfaces;
using authAPI.Model.Common;
using DAL.DataAccess;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace authAPI.BLL
{
    public class InsertProduct
    {
        private readonly IInsertProduct _insertProduct;
        private readonly ISelectProduct _selectProduct;

        public InsertProduct(IInsertProduct insertProduct, ISelectProduct selectProduct)
        {
            _insertProduct = insertProduct;
            _selectProduct = selectProduct;
        }

        public async Task<CommonMessage> InsertProductFinally(commonProduct entity)
        {
            try
            {
                // Use injected dependency instead of creating new instance
                var exists = await _selectProduct.SelectAllProducts()
                    .AnyAsync(x => x.ProductCode == entity.ProductCode);

                if (exists)
                {
                    return new CommonMessage()
                    {
                        IsSuccess = false,
                        Message = "Duplicate Product Code"
                    };
                }

                var success = await _insertProduct.InsertProduct(entity);

                return new CommonMessage
                {
                    IsSuccess = success,
                    Message = success ? "Insert Successful." : "Insert Failed."
                };
            }
            catch (Exception ex)
            {
                return new CommonMessage
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
