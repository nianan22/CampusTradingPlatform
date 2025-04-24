using Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EntityFrameworkCore
{
    public class MyDbContext : DbContext
    {
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<ProductInfo> Products { get; set; }
        public DbSet<EmailCaptchaInfo> EmailCaptchaInfos { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ChatRoom> ChatRooms {  get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; } // ÐÂÔö

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            var assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Entity.dll"));
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(type.Assembly);
            }
        }
    }
}