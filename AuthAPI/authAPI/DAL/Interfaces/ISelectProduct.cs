using authAPI.Model;

namespace authAPI.DAL.Interfaces
{
    public interface ISelectProduct
    {
        IQueryable<Product> SelectAllProducts();
    }
}
