using authAPI.DAL.Interfaces;
using authAPI.Extensions;
using authAPI.Model;

namespace authAPI.DAL.DataAccess
{
    public class DSelectProducts : ISelectProduct
    {
        private AppDBContext _db;

        public DSelectProducts()
        {
            _db = EFCoreExtensions.CreateDbContext();
        }

        public IQueryable<Product> SelectAllProducts()
        {
            return _db.Product;
        }
    }
}
