using CTP.IRepository;
using Entity;
using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Repository
{
    public class ProductRepository : BaseRepository<ProductInfo>, IProductRepository
    {
        public ProductRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }

        public async Task<IEnumerable<ProductInfo>> GetProductsByUserId(int userId)
        {
            return await LoadEntities(p => p.UserId == userId && p.Status != "下架").ToListAsync();
        }

        public async Task<IEnumerable<ProductInfo>> GetSoldProductsByUserId(int userId)
        {
            return await LoadEntities(p => p.UserId == userId && p.Status == "已售出").ToListAsync();
        }
    }
}
