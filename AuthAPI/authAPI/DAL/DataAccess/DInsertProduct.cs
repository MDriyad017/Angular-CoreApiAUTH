using authAPI.Extensions;
using authAPI.Model;
using authAPI.Model.Common;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
    public class DInsertProduct : IInsertProduct
    {
        private AppDBContext _db;
        private Product _entity;

        public DInsertProduct()
        {
            _db = EFCoreExtensions.CreateDbContext();
        }

        public async Task<bool> InsertProduct(commonProduct entity)
        {
            try
            {
                _entity = new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductCode = entity.ProductCode,
                    ProductName = entity.ProductName,
                };

                _db.Product.Add(_entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                // Log specific database errors
                throw new Exception("Database update error occurred", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting product", ex);
            }
        }
    }
}
