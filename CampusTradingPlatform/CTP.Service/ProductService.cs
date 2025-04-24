using CTP.IRepository;
using CTP.IService;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Service
{
    public class ProductService : BaseService<ProductInfo>, IProductService
    {
        public ProductService(IBaseRepository<ProductInfo> repository) : base(repository)
        {
        }

        public async Task<IEnumerable<ProductInfo>> GetProductsByUserId(int userId)
        {
            return await LoadEntities(p => p.UserId == userId ).ToListAsync();
        }

        public async Task<IEnumerable<ProductInfo>> GetSoldProductsByUserId(int userId)
        {
            return await LoadEntities(p => p.UserId == userId && p.Status == "已售出").ToListAsync();
        }
    }
}