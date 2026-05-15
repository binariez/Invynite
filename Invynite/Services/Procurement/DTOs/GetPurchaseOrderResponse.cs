namespace Invynite.Services.Procurement.DTOs
{
    public record GetPurchaseOrderResponse(
        int PurchaseOrderId,
        string SupplierName,
        DateTime OrderDate,
        string Status
        );
}
