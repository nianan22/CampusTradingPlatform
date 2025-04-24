using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTP.IService
{
    public interface IProductService : IBaseService<ProductInfo>
    {
        Task<IEnumerable<ProductInfo>> GetProductsByUserId(int userId);
        Task<IEnumerable<ProductInfo>> GetSoldProductsByUserId(int userId);
    }
}