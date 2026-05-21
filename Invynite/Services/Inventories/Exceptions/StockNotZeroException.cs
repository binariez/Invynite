namespace Invynite.Services.Inventories.Exceptions
{
    public class StockNotZeroException : Exception
    {
        public StockNotZeroException(string message) : base(message)
        {
            
        }
    }
}
