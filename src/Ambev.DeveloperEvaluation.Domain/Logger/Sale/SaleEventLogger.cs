using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Serilog;

namespace Ambev.DeveloperEvaluation.Domain.Logger.Sale
{
    public static class SaleEventLogger
    {
        public static void LogSaleCreated(SaleCreatedEvent saleEvent)
        {
            Log.Information("SaleCreated: {@SaleEvent}", saleEvent);
        }

        public static void LogSaleModified(SaleModifiedEvent saleEvent)
        {
            Log.Information("SaleModified: {@SaleEvent}", saleEvent);
        }

        public static void LogSaleDeleted(SaleDeleteEvent saleEvent)
        {
            Log.Information("SaleDeleteEvent: {@SaleEvent}", saleEvent);
        }
    }
}
