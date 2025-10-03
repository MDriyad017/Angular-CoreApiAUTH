using authAPI.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IInsertProduct
    {
        Task<bool> InsertProduct(commonProduct entity);
    }
}
