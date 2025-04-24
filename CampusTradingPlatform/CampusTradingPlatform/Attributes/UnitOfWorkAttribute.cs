namespace CampusTradingPlatform.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
        public Type[] DbContextType;
        public UnitOfWorkAttribute(Type[] dbConextType)
        {
            this.DbContextType = dbConextType;
        }
    }
}
